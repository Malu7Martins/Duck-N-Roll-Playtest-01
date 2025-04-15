using UnityEngine;

public class DamageWall : MonoBehaviour
{
    public int dano = 1; // Define o dano causado

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Algo tocou o Collider de dano: " + collision.gameObject.name); // Verificar colisão

        if (collision.CompareTag("Player")) // Verifica se colidiu com o jogador
        {
            Debug.Log("O jogador tocou na parede!");

            DuckHealth vida = collision.GetComponent<DuckHealth>();
            if (vida != null)
            {
                // Calcula a direção do dano com base na posição do jogador
                Vector2 direcaoDano = collision.transform.position - transform.position;
                direcaoDano = direcaoDano.normalized; // Normaliza a direção para garantir que tenha magnitude 1

                // Aplica o dano ao jogador com a direção calculada
                vida.TakeDamage(dano, direcaoDano);
                Debug.Log("Dano aplicado ao jogador!");
            }
        }
    }
}
