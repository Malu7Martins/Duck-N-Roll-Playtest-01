using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float speed = 2f;  // Velocidade do inimigo
    public float moveRange = 5f;  // Distância máxima de movimentação
    private bool movingRight = true;  // Direção inicial do inimigo
    private float initialPosition;  // Posição inicial do inimigo

    private bool isFrozen = false;  // Controle se o inimigo está congelado
    private Rigidbody2D rb;  // Referência ao Rigidbody2D
    private SpriteRenderer spriteRenderer;  // Referência ao SpriteRenderer
    private Animator animator;  // Referência ao Animator para animações

    public int health = 3;  // Vida do inimigo

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Obtém o componente SpriteRenderer
        animator = GetComponent<Animator>();  // Obtém o componente Animator
        initialPosition = transform.position.x;  // Armazena a posição inicial do inimigo
    }

    private void Update()
    {
        // Se o inimigo não estiver congelado, ele se move
        if (!isFrozen)
        {
            Move();
        }
    }

    private void Move()
    {
        float movement = speed * Time.deltaTime;
        if (movingRight)
        {
            transform.Translate(Vector2.right * movement);
            if (transform.position.x >= initialPosition + moveRange)
            {
                movingRight = false;  // Inverte a direção quando atingir o limite
                Flip();  // Inverte a direção do sprite
            }
        }
        else
        {
            transform.Translate(Vector2.left * movement);
            if (transform.position.x <= initialPosition - moveRange)
            {
                movingRight = true;  // Inverte a direção quando atingir o limite
                Flip();  // Inverte a direção do sprite
            }
        }
    }

    // Função que inverte a direção do sprite
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;  // Inverte a escala no eixo X
        transform.localScale = localScale;
    }


    // Função para tomar dano
    public void TakeDamage(int damage, Vector2 direcaoDano)
    {
        health -= damage;
        Debug.Log("Inimigo tomou dano! Vida atual: " + health);

        // Muda a cor para vermelho imediatamente
        ChangeColor(Color.red);

        // Aplica a força de empurrão ao inimigo na direção do dano
        if (rb != null)
        {
            rb.AddForce(direcaoDano * 150f, ForceMode2D.Impulse);  // Aqui está a força do empurrão (ajuste 150f conforme necessário)
        }

        // Chama a coroutine para voltar a cor ao normal depois de 1 segundo
        StartCoroutine(RestoreColorAfterDamage(1f));

        // Verifica se o inimigo morreu
        if (health <= 0)
        {
            Die();
        }
    }


    // Função para a morte do inimigo
    private void Die()
    {
        Debug.Log("Inimigo morreu!");

        // Inicia a animação de morte
        if (animator != null)
        {
            animator.SetTrigger("Die");  // Assumindo que você tenha um trigger chamado "Die" no Animator
        }

        // Espera a animação de morte terminar antes de destruir o inimigo
        // Você pode ajustar o tempo conforme a duração da animação de morte
        Destroy(gameObject, 1f);  // Espera 1 segundo antes de destruir o GameObject (ajuste o tempo conforme necessário)
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o inimigo está congelado. Se sim, não causa dano ao jogador.
        if (isFrozen)
        {
            return;  // Ignora a colisão e não causa dano
        }

        // Verifica se o inimigo colidiu com o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtém o ponto de contato da colisão
            ContactPoint2D contact = collision.contacts[0];

            if (contact.normal.y < 0)  // Se a normal da colisão estiver apontando para baixo (parte de cima do inimigo)
            {
                // O jogador está colidindo com a parte superior do inimigo, congela e impede o movimento
                Freeze(Mathf.Infinity); // Congela o inimigo "para sempre"

                // Se você quiser que o inimigo morra também, mantenha:
                Die();
            }
            else
            {
                // Caso o jogador tenha colidido com o inimigo de outras direções (não de cima)
                DuckHealth player = collision.gameObject.GetComponent<DuckHealth>();
                if (player != null)
                {
                    // Calcula a direção do dano (do inimigo para o jogador)
                    Vector2 direcaoDano = collision.transform.position - transform.position;
                    direcaoDano = direcaoDano.normalized;  // Normaliza a direção para garantir que tenha magnitude 1

                    player.TakeDamage(1, direcaoDano);  // Aplica dano ao jogador com a direção calculada
                }
            }
        }

        // Verifica se o inimigo colidiu com a bola de fogo
        if (collision.gameObject.CompareTag("Attack"))
        {
            // Aplica dano ao inimigo
            Vector2 direcaoDano = collision.transform.position - transform.position;
            direcaoDano = direcaoDano.normalized;  // Normaliza a direção para garantir que tenha magnitude 1
            Destroy(collision.gameObject);  // Destrói a bola de fogo
        }

        // Verifica se o inimigo colidiu com a bola de gelo
        if (collision.gameObject.CompareTag("IceAttack"))
        {
            // Congela o inimigo por uma duração específica (exemplo: 3 segundos)
            Freeze(3f);  // Congela por 3 segundos
            Destroy(collision.gameObject);  // Destroi a bola de gelo
        }
    }

    // Método para congelar o inimigo permanentemente
    public void Freeze(float duration)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            // Para qualquer movimento ou animação enquanto congelado
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;  // Para o movimento
                rb.bodyType = RigidbodyType2D.Kinematic;  // Define como cinemático
            }

            // Muda a cor para ciano quando congelado
            // ChangeColor(Color.cyan);

            // Você pode adicionar lógica extra aqui, como parar animações ou mudanças visuais
            Debug.Log("Inimigo congelado!");
        }
    }

    // Método para descongelar o inimigo (se necessário)
    public void Unfreeze()
    {
        if (isFrozen)
        {
            isFrozen = false;
            // Restaura a física do inimigo e o movimento
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;  // Restaura a física (movimento normal)
            }

            // Restaura a cor original (branca) quando descongelado
            ChangeColor(Color.white);

            Debug.Log("Inimigo descongelado!");
        }

    }

    // Método para mudar a cor do inimigo
    private void ChangeColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    // Coroutine para restaurar a cor depois de algum tempo
    private IEnumerator RestoreColorAfterDamage(float time)
    {
        // Espera o tempo especificado
        yield return new WaitForSeconds(time);

        // Restaura a cor original (branca)
        ChangeColor(Color.white);
    }
}