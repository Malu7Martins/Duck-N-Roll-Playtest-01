using UnityEngine;
using System.Collections;  // Necess�rio para IEnumerator

public class Laser : MonoBehaviour
{
    public float tempoAparecer = 2f;  // Tempo que o objeto ficar� vis�vel
    public float tempoDesaparecer = 2f; // Tempo que o objeto ficar� invis�vel
    private SpriteRenderer spriteRenderer;  // Para controlar a visibilidade
    private Collider2D myCollider;  // Renomeado para evitar conflito

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // Pega o SpriteRenderer
        myCollider = GetComponent<Collider2D>();  // Pega o Collider2D
        StartCoroutine(AparecerDesaparecerCoroutine());  // Inicia o ciclo de aparecer/desaparecer
    }

    private IEnumerator AparecerDesaparecerCoroutine()
    {
        while (true)  // Loop infinito
        {
            // Aparece
            spriteRenderer.enabled = true;  // Torna o objeto vis�vel
            myCollider.enabled = true;  // Ativa o collider para detectar colis�es
            yield return new WaitForSeconds(tempoAparecer);  // Espera o tempo definido para aparecer

            // Desaparece
            spriteRenderer.enabled = false;  // Torna o objeto invis�vel
            myCollider.enabled = false;  // Desativa o collider
            yield return new WaitForSeconds(tempoDesaparecer);  // Espera o tempo definido para desaparecer
        }
    }

    // Quando o objeto colide com o jogador, causa dano
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // Verifica se o objeto colidiu com o jogador
        {
            DuckHealth duckHealth = collision.gameObject.GetComponent<DuckHealth>();  // Obt�m a refer�ncia para o DuckHealth
            if (duckHealth != null)
            {
                // Calcula a dire��o do dano (do laser para o jogador)
                Vector2 direcaoDano = collision.transform.position - transform.position;
                direcaoDano = direcaoDano.normalized;  // Normaliza a dire��o

                // Aplica o dano ao jogador com a dire��o
                duckHealth.TakeDamage(1, direcaoDano);
            }
        }
    }
}
