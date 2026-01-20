using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LojaManager : MonoBehaviour
{
    public static LojaManager instance;

    [Header("Banco de Dados de Itens")]
    public List<ItemLoja> todosOsItens; 
    public int quantidadeItensAVenda = 3;

    [Header("Referências de UI")]
    public TextMeshProUGUI textoOuroLoja;
    public GameObject painelLoja;
    public Transform containerItens; 
    public GameObject prefabBotaoItem;

    [Header("Status da Barreira")]
    public bool barreiraEstaAtiva = false;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AbrirLoja()
    {
        painelLoja.SetActive(true);
        Time.timeScale = 0f; 
        AtualizarInterfaceLoja();
        GerarItensAleatorios();
    }

    void GerarItensAleatorios()
    {
        foreach (Transform filho in containerItens) Destroy(filho.gameObject);

        List<ItemLoja> sorteados = new List<ItemLoja>(todosOsItens);
        for (int i = 0; i < quantidadeItensAVenda && sorteados.Count > 0; i++)
        {
            int indice = Random.Range(0, sorteados.Count);
            CriarBotaoNaLoja(sorteados[indice]);
            sorteados.RemoveAt(indice);
        }
    }

    void CriarBotaoNaLoja(ItemLoja item)
    {
        GameObject novoBotao = Instantiate(prefabBotaoItem, containerItens);
        TextMeshProUGUI texto = novoBotao.GetComponentInChildren<TextMeshProUGUI>();
        if (texto != null) texto.text = $"{item.nome}\n${item.preco}";

        Transform buscaIcone = novoBotao.transform.Find("Icone");
        if (buscaIcone != null)
        {
            Image img = buscaIcone.GetComponent<Image>();
            if (img != null) img.sprite = item.icone;
        }

        novoBotao.GetComponent<Button>().onClick.AddListener(() => TentarComprar(item));
    }

    public void TentarComprar(ItemLoja item)
    {
        // TRAVA 1: Barreira já ativa
        if (item.tipo == ItemLoja.TipoItem.Barreira && barreiraEstaAtiva)
        {
            Debug.Log("Barreira já está ativa!");
            return; 
        }

        // TRAVA 2: Vida máxima atingida
        if (item.tipo == ItemLoja.TipoItem.Vida)
        {
            // Acessa a vida do GameManager para checar se pode comprar
            if (GameManager.instance.vidaAtual >= GameManager.instance.vidaMaxima)
            {
                Debug.Log("Vida já está no máximo!");
                return; // Impede a compra
            }
        }

        // Lógica de Ouro
        if (GameManager.instance.ouroAtual >= item.preco)
        {
            GameManager.instance.ouroAtual -= item.preco;
            AplicarEfeito(item);
            AtualizarInterfaceLoja();
            GameManager.instance.AdicionarOuro(0); 
        }
        else 
        { 
            Debug.Log("Ouro insuficiente!"); 
        }
    }

    void AplicarEfeito(ItemLoja item)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        switch (item.tipo)
        {
            case ItemLoja.TipoItem.Vida:
                GameManager.instance.AdicionarVida(1);
                break;

            case ItemLoja.TipoItem.Barreira:
                if (player != null)
                {
                    Transform b = player.transform.Find("Barreira");
                    if (b != null) b.gameObject.SetActive(true);
                }
                break;

            case ItemLoja.TipoItem.PuloDuplo:
                if (player != null) player.GetComponent<Player>().podePuloDuplo = true;
                break;
        }
    }

    public void AtualizarInterfaceLoja()
    {
        if (textoOuroLoja != null)
            textoOuroLoja.text = "Ouro: " + GameManager.instance.ouroAtual;
    }

    public void FecharLoja()
    {
        painelLoja.SetActive(false);
        Time.timeScale = 1f;
    }
}