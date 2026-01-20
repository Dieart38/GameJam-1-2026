using UnityEngine;
using System.Collections; // Necessário para Corrotinas

public class Player : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;
    
    [Header("Configurações de Movimento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private GameObject fogoPrefab; 
    [SerializeField] private Transform pontoSaidaFogo; 

    [Header("Habilidades Especiais")]
    public bool podePuloDuplo = false; 
    private int pulosRestantes;

    [Header("Efeitos de Dano")]
    [SerializeField] private float duracaoImunidade = 1.5f;
    [SerializeField] private float velocidadePisca = 0.15f;
    public bool estaInvencivel = false; // Mudado para public para o GameManager ler

    private Rigidbody2D rb; 
    private Animator anim;
    private SpriteRenderer sr; 

    void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Tenta pegar o SpriteRenderer no objeto principal ou nos filhos
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();

        controls.Player.Jump.performed += ctx => Jump();
        controls.Player.Fire.performed += ctx => Fire();
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        transform.Translate(new Vector3(moveInput.x, 0, 0) * speed * Time.deltaTime);

        float velocidadeHorizontal = Mathf.Abs(moveInput.x);
        if (anim != null) anim.SetFloat("Velocidade", velocidadeHorizontal);

        if (moveInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (moveInput.x > 0) transform.localScale = new Vector3(1, 1, 1);

        // Resetar pulos ao tocar o chão
        if (Mathf.Abs(rb.linearVelocity.y) < 0.001f)
        {
            pulosRestantes = podePuloDuplo ? 2 : 1;
        }
    }

    void Jump()
    {
        if (pulosRestantes > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pulosRestantes--;
        }
    }

    void Fire()
    {
        // Impede atirar enquanto a loja está aberta (Time.timeScale é 0)
        if (Time.timeScale == 0) return;

        if (fogoPrefab != null && pontoSaidaFogo != null)
        {
            GameObject tiro = Instantiate(fogoPrefab, pontoSaidaFogo.position, Quaternion.identity);
            float direcao = transform.localScale.x;
            tiro.GetComponent<ProjetilFogo>().ConfigurarDirecao(direcao);
        }
    }

    // Chamado pelo GameManager ao tirar vida
    public void IniciarEfeitoDano()
    {
        if (!estaInvencivel && sr != null)
        {
            StartCoroutine(EfeitoPiscar());
        }
    }

    IEnumerator EfeitoPiscar()
    {
        estaInvencivel = true;
        float tempoPassado = 0;

        while (tempoPassado < duracaoImunidade)
        {
            // Muda a opacidade (Alpha) para 0.3 (transparente)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f);
            yield return new WaitForSeconds(velocidadePisca);

            // Volta para 1.0 (opaco)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            yield return new WaitForSeconds(velocidadePisca);

            tempoPassado += (velocidadePisca * 2);
        }

        estaInvencivel = false;
    }
}