using UnityEngine;

public class ProjetilFogo : MonoBehaviour
{
    [SerializeField] private float velocidade = 10f;
    [SerializeField] private float tempoVida = 3f;
    [SerializeField] private int dano = 1;
    private float direcao;

    public void ConfigurarDirecao(float direcaoPlayer)
    {
        direcao = direcaoPlayer;
        
        // Inverte o sprite da bala se estiver atirando para a esquerda
        transform.localScale = new Vector3(direcao, 1, 1);
    }

    void Start()
    {
        Destroy(gameObject, tempoVida);
    }

    void Update()
    {
        // Agora multiplica a velocidade pela direção (1 ou -1)
        transform.Translate(Vector2.right * direcao * velocidade * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo"))
        {
            // 1. Tenta dar dano se for o Guerreiro
            InimigoGuerreiro guerreiro = collision.GetComponent<InimigoGuerreiro>();
            if (guerreiro != null)
            {
                guerreiro.TomarDano(dano);
            }

            // 2. Tenta dar dano se for o Arqueiro
            InimigoArqueiro arqueiro = collision.GetComponent<InimigoArqueiro>();
            if (arqueiro != null)
            {
                arqueiro.TomarDano(dano);
            }

            

            // Destrói o fogo ao colidir com qualquer inimigo
            Destroy(gameObject);
        }

        // Se bater em algo com a tag "Colisor", destrói o fogo
        if (collision.CompareTag("Colisor"))
        {
            Destroy(gameObject);
        }
    }
}