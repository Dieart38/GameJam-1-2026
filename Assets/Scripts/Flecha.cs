using UnityEngine;

public class Flecha : MonoBehaviour
{
    public float velocidade = 8f;
    public int dano = 1;
    private Rigidbody2D rb;
    private bool jaBateu = false;

    [Header("Transformação em Ouro")]
    public GameObject moedaPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        // Enquanto não bater em nada, ela aponta para onde voa
        if (!jaBateu && rb.linearVelocity != Vector2.zero)
        {
            float angulo = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se acertar o jogador (O Player precisa ter um Collider2D)
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.TomarDano(dano);
            Destroy(gameObject);
        }

        // Se bater em qualquer coisa com a tag "Colisor"
        if (collision.gameObject.CompareTag("Colisor"))
        {
            if (!jaBateu)
            {
                jaBateu = true;
                // Faz a flecha "perder a força" e cair no chão antes de virar moeda
                Invoke("TransformarEmMoeda", 0.5f); 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Flecha colidiu com " + other.gameObject.name);
         if (other.CompareTag("Barreira"))
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        jaBateu = false;
    }


       


    void TransformarEmMoeda()
    {
        if (moedaPrefab != null)
        {
            // Cria a moeda levemente acima da flecha
            GameObject moeda = Instantiate(moedaPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            
            // Dá um pequeno impulso para cima para ela cair bonitinho
            Rigidbody2D rbMoeda = moeda.GetComponent<Rigidbody2D>();
            if(rbMoeda != null) {
                rbMoeda.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
            }
        }
        Destroy(gameObject);
    }
}