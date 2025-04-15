using UnityEngine;
using System.Collections;  // Necessário para IEnumerator

public class Laser : MonoBehaviour
{
    public float tempoAparecer = 2f;  // Tempo que o objeto ficará visível
    public float tempoDesaparecer = 2f; // Tempo que o objeto ficará invisível
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
            spriteRenderer.enabled = true;  // Torna o objeto visível
            myCollider.enabled = true;  // Ativa o collider para detectar colisões
            yield return new WaitForSeconds(tempoAparecer);  // Espera o tempo definido para aparecer

            // Desaparece
            spriteRenderer.enabled = false;  // Torna o objeto invisível
            myCollider.enabled = false;  // Desativa o collider
            yield return new WaitForSeconds(tempoDesaparecer);  // Espera o tempo definido para desaparecer
        }
    }

    // Quando o objeto colide com o jogador, causa dano
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // Verifica se o objeto colidiu com o jogador
        {
            DuckHealth duckHealth = collision.gameObject.GetComponent<DuckHealth>();  // Obtém a referência para o DuckHealth
            if (duckHealth != null)
            {
                // Calcula a direção do dano (do laser para o jogador)
                Vector2 direcaoDano = collision.transform.position - transform.position;
                direcaoDano = direcaoDano.normalized;  // Normaliza a direção

                // Aplica o dano ao jogador com a direção
                duckHealth.TakeDamage(1, direcaoDano);
            }
        }
    }
}
