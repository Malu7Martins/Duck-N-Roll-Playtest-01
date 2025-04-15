using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public Transform spawnPoint;  // A refer�ncia para o ponto de spawn

    void Start()
    {
        // Posiciona o jogador no ponto de spawn
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
    }
}
