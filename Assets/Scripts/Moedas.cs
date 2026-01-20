using UnityEngine;

public class Moedas : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float velocidadeAtracao = 10f;
    [SerializeField] private float distanciaAtivacao = 5f;
    [SerializeField] private int valorMoeda = 10;
    [SerializeField] private float vidaMoeda = 10f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, vidaMoeda); 
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distancia = Vector2.Distance(transform.position, playerTransform.position);

        // Se o player chegar perto, a moeda perde o peso e voa até ele
        if (distancia < distanciaAtivacao)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Para de sofrer gravidade para voar
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, velocidadeAtracao * Time.deltaTime);
        }
    }

    // MUDANÇA AQUI: Usamos OnCollisionEnter2D porque ela agora é sólida
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.AdicionarOuro(valorMoeda);
            Debug.Log("Moeda física coletada!");
            Destroy(gameObject);
        }
    }
}