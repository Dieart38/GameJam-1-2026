using UnityEngine;
using System.Collections;

public class BarreiraMagica : MonoBehaviour
{
    [Header("Configurações Visuais")]
    public float velocidadeGiro = 180f;
    public float velocidadePisca = 0.1f;

    [Header("Configurações de Tempo")]
    public float duracao = 7f; 

    private SpriteRenderer sr;
    private CircleCollider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
    }

    void OnEnable()
    {
        // Avisa a loja que a barreira ligou
        if (LojaManager.instance != null)
            LojaManager.instance.barreiraEstaAtiva = true;

        StartCoroutine(RotinaBarreira());
    }

    void OnDisable()
    {
        // Avisa a loja que a barreira desligou e pode ser comprada de novo
        if (LojaManager.instance != null)
            LojaManager.instance.barreiraEstaAtiva = false;
        
        StopAllCoroutines();
    }

    void Update()
    {
        // Efeito de giro constante
        transform.Rotate(Vector3.forward * velocidadeGiro * Time.deltaTime);
    }

    IEnumerator RotinaBarreira()
    {
        float tempoPassado = 0;
        if (col != null) col.enabled = true;

        while (tempoPassado < duracao)
        {
            // Efeito de piscar (altera a opacidade/Alpha)
            if (sr != null)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f);
                yield return new WaitForSeconds(velocidadePisca);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
                yield return new WaitForSeconds(velocidadePisca);
            }
            
            tempoPassado += (velocidadePisca * 2);
        }

        // Desliga a barreira após o tempo acabar
        gameObject.SetActive(false);
    }
}