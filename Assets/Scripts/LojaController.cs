using UnityEngine;

public class LojaController : MonoBehaviour
{
    public GameObject vendedor; // Arraste o objeto do Vendedor aqui
    public float tempoParaAparecer = 30f; // 30 segundos sumido
    public float tempoParaFicarAtivo = 15f; // 15 segundos visível

    private float timer;
    private bool estaAtivo = false;

    void Start()
    {
        vendedor.SetActive(false);
        timer = tempoParaAparecer;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            estaAtivo = !estaAtivo;
            vendedor.SetActive(estaAtivo);

            // Reinicia o timer com base no novo estado
            timer = estaAtivo ? tempoParaFicarAtivo : tempoParaAparecer;
            
            if (estaAtivo) Debug.Log("O Vendedor apareceu!");
            else Debug.Log("O Vendedor foi embora!");
        }
    }
}
