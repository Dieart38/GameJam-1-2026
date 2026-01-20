using UnityEngine;

public class SpawnerInimigo : MonoBehaviour
{
    // Script para chamar inimigos em pontos específicos do mapa
    public GameObject inimigoPrefab;
    public float intervaloSpawn = 5f;
    private float timer;


    void Start()
    {
        

    }

    

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnInimigo();
            timer = intervaloSpawn;
        }


    }

    void SpawnInimigo()
    {
        Instantiate(inimigoPrefab, transform.position, Quaternion.identity);
    }
}
