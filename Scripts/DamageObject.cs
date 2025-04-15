using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int dano = 1;  // A quantidade de dano que este objeto causa ao personagem

    // Detecta a colis�o com o personagem (assumindo que o personagem tem o tag "Player")
    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            // Acessa o script de vida do personagem e aplica o dano
            DuckHealth personagem = colisao.gameObject.GetComponent<DuckHealth>();

            if (personagem != null)
            {
                // Calcula a dire��o do dano com base nas posi��es do objeto e do personagem
                Vector2 direcaoDano = colisao.transform.position - transform.position;
                direcaoDano = direcaoDano.normalized;  // Normaliza a dire��o para garantir que tenha magnitude 1

                // Aplica o dano com a dire��o calculada
                personagem.TakeDamage(dano, direcaoDano);
            }
        }
    }
}
