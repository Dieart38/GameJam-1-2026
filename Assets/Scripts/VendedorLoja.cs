using UnityEngine;

public class VendedorLoja : MonoBehaviour
{
    public GameObject painelLojaUI; // Arraste o Painel da Loja aqui no Inspector
    private bool playerPerto = false;

    void Update()
    {
        // IMPORTANTE: GetKeyDown não funciona se o Time.timeScale for 0 
        // a menos que seja configurado, mas aqui ele abre a loja (tempo ainda é 1)
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            LojaManager.instance.AbrirLoja();
        }
    }

    void AbrirLoja()
    {
        if (painelLojaUI != null)
        {
            painelLojaUI.SetActive(true);
            Time.timeScale = 0f; // Pausa o jogo
            
            // Avisa o LojaManager para atualizar o ouro na tela
            if (LojaManager.instance != null)
            {
                LojaManager.instance.AtualizarInterfaceLoja();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
            Debug.Log("Player entrou na área! Aperte E.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
            // Se o player sair de perto, a loja fecha automaticamente
            if(painelLojaUI != null)
            {
                painelLojaUI.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}