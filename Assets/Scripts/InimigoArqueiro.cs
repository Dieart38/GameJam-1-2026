using UnityEngine;

public class InimigoArqueiro : MonoBehaviour
{
    [Header("Configurações de Alcance")]
    public float distanciaParar = 6f;

    [Header("Configurações de Ataque")]
    public float taxaAtaque = 2f;
    private float proximoAtaque;
    public GameObject flechaPrefab;
    public Transform pontoSaidaFlecha;

    [Header("Vida e Recompensa")]
    public float vida = 2f;
    public GameObject moedaPrefab;
    public int moedasParaSoltar = 3;

    private Transform player;
    private Animator anim;
    private Rigidbody2D rb; // Adicionado para controlar a física

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Pega o Rigidbody do arqueiro

        if (anim != null) anim.SetBool("Attack", false);
        
        // Se você não quer que o arqueiro saia voando com colisões:
        if (rb != null) {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Impede ele de girar feito louco
        }
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        // TRAVA DE MOVIMENTO: Garante que a velocidade horizontal seja sempre 0
        if (rb != null) {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        float distancia = Vector2.Distance(transform.position, player.position);

        if (distancia <= distanciaParar)
        {
            if (Time.time >= proximoAtaque)
            {
                Atirar();
                proximoAtaque = Time.time + taxaAtaque;
            }
        }

        OlharParaPlayer();
    }

    void OlharParaPlayer()
    {
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Atirar()
    {
        if (anim != null) anim.SetBool("Attack", true);

        GameObject flecha = Instantiate(flechaPrefab, pontoSaidaFlecha.position, Quaternion.identity);
        
        // SOLUÇÃO DE COLISÃO: Faz a flecha ignorar o colisor do arqueiro que a atirou
        Physics2D.IgnoreCollision(flecha.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        Rigidbody2D rbFlecha = flecha.GetComponent<Rigidbody2D>();
        Vector2 direcao = (player.position - pontoSaidaFlecha.position).normalized;
        direcao.y += 0.2f; 

        if (rbFlecha != null)
        {
            rbFlecha.linearVelocity = direcao * flecha.GetComponent<Flecha>().velocidade;
        }

        Invoke("ResetarAtaque", 2.5f); 
    }

    void ResetarAtaque()
    {
        if (anim != null) anim.SetBool("Attack", false);
    }

    // ... (restante do código: TomarDano e Morrer permanecem iguais)
    public void TomarDano(float dano)
    {
        vida -= dano;
        if (vida <= 0) Morrer();
    }

    void Morrer()
    {
        for (int i = 0; i < moedasParaSoltar; i++)
        {
            GameObject moeda = Instantiate(moedaPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rbMoeda = moeda.GetComponent<Rigidbody2D>();
            if (rbMoeda != null)
            {
                Vector2 forcaAleatoria = new Vector2(Random.Range(-2f, 2f), Random.Range(3f, 5f));
                rbMoeda.AddForce(forcaAleatoria, ForceMode2D.Impulse);
            }
        }
        Destroy(gameObject);
    }
}