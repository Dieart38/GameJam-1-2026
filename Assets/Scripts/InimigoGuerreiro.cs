using UnityEngine;

public class InimigoGuerreiro : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 2.5f;
    public float distanciaParar = 1.5f; // Distância que ele para para atacar
    
    [Header("Configurações de Ataque")]
    public float danoInimigo = 10f;
    public float taxaAtaque = 1.5f; // Segundos entre ataques
    private float proximoAtaque;

    [Header("Recompensa")]
    public GameObject moedaPrefab;
    public int moedasParaSoltar = 3;
    public float vida = 2f;

    private Animator anim; // Adicionado: Referência para o componente de animação

    private Transform player;

    void Awake()
    {
        anim = GetComponent<Animator>(); // Adicionado: Pega o Animator do dragão
    }
    void Start()
    {
        // Encontra o transform do jogador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

    }

    void Update()
    {
        // --- LÓGICA DE ANIMAÇÃO ---
        if (anim != null)
        {
            anim.SetFloat("Velocidade", velocidade);
        }


        if (player == null) return;

        float distancia = Vector2.Distance(transform.position, player.position);

        // Se estiver longe, persegue o dragão
        if (distancia > distanciaParar)
        {
            MoverParaPlayer();
        }
        else
        {
            // Se estiver perto e o tempo de recarga passou, ataca
            if (Time.time >= proximoAtaque)
            {
                Atacar();
                proximoAtaque = Time.time + taxaAtaque;
            }
        }
    }

    void MoverParaPlayer()
    {
        // Move em direção ao player
        Vector2 direcao = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), velocidade * Time.deltaTime);

        // Inverter o sprite do inimigo conforme a direção
        if (direcao.x < 0) transform.localScale = new Vector3(-1, 1, 1); // Olhando para a direita
        else transform.localScale = new Vector3(1, 1, 1); // Olhando para a esquerda
    }

    void Atacar()
    {
        anim.SetBool("Attack", true);
        Invoke("ResetarAtaque", 0.5f); // Reseta a animação de ataque após meio segundo
        Debug.Log("O Guerreiro atacou o Dragão!");
        GameManager.instance.TomarDano(1); // Tira um coração do jogador
        

        
    }

    void ResetarAtaque()
    {
        anim.SetBool("Attack", false);
    }

    public void TomarDano(float dano)
    {
        vida -= dano;
        if (vida <= 0) Morrer();
    }

    void Morrer()
    {
        for (int i = 0; i < moedasParaSoltar; i++)
        {
            // Instancia uma moeda na posição do inimigo
            GameObject moeda = Instantiate(moedaPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rbMoeda = moeda.GetComponent<Rigidbody2D>();
            if (rbMoeda != null)
            {
                // Aplica uma força aleatória para espalhar as moedas
                Vector2 forcaAleatoria = new Vector2(Random.Range(-2f, 2f), Random.Range(3f, 5f));
                rbMoeda.AddForce(forcaAleatoria, ForceMode2D.Impulse);
            }
        }
        Destroy(gameObject);
    }
}