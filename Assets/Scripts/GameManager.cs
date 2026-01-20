using UnityEngine;
using TMPro; // Necessário para o texto (instale o TextMeshPro no Unity)
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Configurações de Ouro")]
    public int ouroAtual = 0;
    public TextMeshProUGUI textoOuro;

    [Header("Configurações de Vida")]
    public int vidaMaxima = 10;
    public int vidaAtual;
    public GameObject coracaoPrefab; // Arraste um ícone de coração aqui
    public Transform containerCoracoes; // Um objeto com Layout Group para os corações
    private List<GameObject> listaCoracoes = new List<GameObject>();

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        vidaAtual = 3; // Vida inicial
        AtualizarTextoOuro();
        DesenharCoracoes();
    }

    // --- SISTEMA DE OURO ---
    public void AdicionarOuro(int quantidade)
    {
        ouroAtual += quantidade;
        AtualizarTextoOuro();
    }

    void AtualizarTextoOuro()
    {
        if (textoOuro != null) textoOuro.text = "OURO: " + ouroAtual;
    }

    // --- SISTEMA DE VIDA ---
    public void TomarDano(int dano)
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Player scriptPlayer = playerObj.GetComponent<Player>();

            // SE ESTIVER PISCANDO, NÃO TIRA DANO
            if (scriptPlayer.estaInvencivel) return;

            vidaAtual -= dano;
            vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
            DesenharCoracoes();

            // Inicia o pisca no script do Player
            scriptPlayer.IniciarEfeitoDano();
        }
    }

    void DesenharCoracoes()
    {
        // Limpa os corações antigos
        foreach (GameObject coracao in listaCoracoes) Destroy(coracao);
        listaCoracoes.Clear();

        // Cria novos corações baseados na vida atual
        for (int i = 0; i < vidaAtual; i++)
        {
            GameObject novoCoracao = Instantiate(coracaoPrefab, containerCoracoes);
            listaCoracoes.Add(novoCoracao);
        }
    }
    // Adicione isso ao seu GameManager para a loja funcionar:
    public void AdicionarVida(int quantidade)
    {
        vidaAtual += quantidade;
        // Não deixa passar da vida máxima que você definiu
        if (vidaAtual > vidaMaxima) vidaAtual = vidaMaxima;
        
        DesenharCoracoes();
    }
}