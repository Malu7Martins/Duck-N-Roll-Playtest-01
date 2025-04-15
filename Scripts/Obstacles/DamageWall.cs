using UnityEngine;

public class DamageWall : MonoBehaviour
{
    public int dano = 1; // Define o dano causado

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Algo tocou o Collider de dano: " + collision.gameObject.name); // Verificar colis�o

        if (collision.CompareTag("Player")) // Verifica se colidiu com o jogador
        {
            Debug.Log("O jogador tocou na parede!");

            DuckHealth vida = collision.GetComponent<DuckHealth>();
            if (vida != null)
            {
                // Calcula a dire��o do dano com base na posi��o do jogador
                Vector2 direcaoDano = collision.transform.position - transform.position;
                direcaoDano = direcaoDano.normalized; // Normaliza a dire��o para garantir que tenha magnitude 1

                // Aplica o dano ao jogador com a dire��o calculada
                vida.TakeDamage(dano, direcaoDano);
                Debug.Log("Dano aplicado ao jogador!");
            }
        }
    }
}
