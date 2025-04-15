using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFollow : MonoBehaviour
{
    public float speed = 2f;  // Velocidade do inimigo
    public float moveRange = 5f;  // Dist�ncia m�xima de movimenta��o
    private bool movingRight = true;  // Dire��o inicial do inimigo
    private float initialPosition;  // Posi��o inicial do inimigo

    private bool isFrozen = false;  // Controle se o inimigo est� congelado
    private Rigidbody2D rb;  // Refer�ncia ao Rigidbody2D
    private SpriteRenderer spriteRenderer;  // Refer�ncia ao SpriteRenderer
    private Animator animator;  // Refer�ncia ao Animator para anima��es

    // SEGUE

    public Transform player; // Refer�ncia ao jogador
    public float followRange = 10f; // Dist�ncia m�xima para seguir o jogador

    public int health = 3;  // Vida do inimigo

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Obt�m o componente SpriteRenderer
        animator = GetComponent<Animator>();  // Obt�m o componente Animator
        initialPosition = transform.position.x;  // Armazena a posi��o inicial do inimigo
    }


    // SEGUE
    private void Update()
    {
        if (isFrozen) return;

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= followRange)
            {
                FollowPlayer();
            }
            else
            {
                Patrol(); // Patrulha normal se o jogador estiver longe
            }
        }
        else
        {
            Patrol(); // Se o jogador n�o estiver atribu�do, patrulha normalmente
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
                movingRight = false;  // Inverte a dire��o quando atingir o limite
                Flip();  // Inverte a dire��o do sprite
            }
        }
        else
        {
            transform.Translate(Vector2.left * movement);
            if (transform.position.x <= initialPosition - moveRange)
            {
                movingRight = true;  // Inverte a dire��o quando atingir o limite
                Flip();  // Inverte a dire��o do sprite
            }
        }
    }

    // Fun��o que inverte a dire��o do sprite
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;  // Inverte a escala no eixo X
        transform.localScale = localScale;
    }


    // Fun��o para tomar dano
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Inimigo tomou dano! Vida atual: " + health);

        // Muda a cor para vermelho imediatamente
        ChangeColor(Color.red);

        // Chama a coroutine para voltar a cor ao normal depois de 1 segundo (ajuste o tempo conforme necess�rio)
        StartCoroutine(RestoreColorAfterDamage(1f));

        // Verifica se o inimigo morreu
        if (health <= 0)
        {
            Die();
        }
    }

    // Fun��o para a morte do inimigo
    private void Die()
    {
        Debug.Log("Inimigo morreu!");

        // Inicia a anima��o de morte
        if (animator != null)
        {
            animator.SetTrigger("Die");  // Assumindo que voc� tenha um trigger chamado "Die" no Animator
        }

        // Espera a anima��o de morte terminar antes de destruir o inimigo
        // Voc� pode ajustar o tempo conforme a dura��o da anima��o de morte
        Destroy(gameObject, 1f);  // Espera 1 segundo antes de destruir o GameObject (ajuste o tempo conforme necess�rio)
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o inimigo est� congelado. Se sim, n�o causa dano ao jogador.
        if (isFrozen)
        {
            return;  // Ignora a colis�o e n�o causa dano
        }

        // Verifica se o inimigo colidiu com o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obt�m o ponto de contato da colis�o
            ContactPoint2D contact = collision.contacts[0];

            // Verifica se a colis�o ocorreu pela parte superior do inimigo
            if (contact.normal.y < 0)  // Se a normal da colis�o estiver apontando para baixo (parte de cima do inimigo)
            {
                // O jogador est� colidindo com a parte superior do inimigo, mata o inimigo
                Die();
            }
            else
            {
                // Caso o jogador tenha colidido com o inimigo de outras dire��es (n�o de cima)
                DuckHealth player = collision.gameObject.GetComponent<DuckHealth>();
                if (player != null)
                {
                    Vector2 direcaoDano = (player.transform.position - transform.position).normalized;
                    player.TakeDamage(1, direcaoDano);
                }
            }
        }

        // Verifica se o inimigo colidiu com a bola de fogo
        if (collision.gameObject.CompareTag("Attack"))
        {
            // Aplica dano ao inimigo
            TakeDamage(1);  // Diminui a vida do inimigo
            Destroy(collision.gameObject);  // Destr�i a bola de fogo
        }

        // Verifica se o inimigo colidiu com a bola de gelo
        if (collision.gameObject.CompareTag("IceAttack"))
        {
            // Congela o inimigo por uma dura��o espec�fica (exemplo: 3 segundos)
            Freeze(3f);  // Congela por 3 segundos
            Destroy(collision.gameObject);  // Destroi a bola de gelo
        }
    }

    // M�todo para congelar o inimigo permanentemente
    public void Freeze(float duration)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            // Para qualquer movimento ou anima��o enquanto congelado
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;  // Para o movimento
                rb.bodyType = RigidbodyType2D.Kinematic;  // Define como cinem�tico
            }

            // Muda a cor para ciano quando congelado
            ChangeColor(Color.cyan);

            // Voc� pode adicionar l�gica extra aqui, como parar anima��es ou mudan�as visuais
            Debug.Log("Inimigo congelado!");
        }
    }

    // M�todo para descongelar o inimigo (se necess�rio)
    public void Unfreeze()
    {
        if (isFrozen)
        {
            isFrozen = false;
            // Restaura a f�sica do inimigo e o movimento
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;  // Restaura a f�sica (movimento normal)
            }

            // Restaura a cor original (branca) quando descongelado
            ChangeColor(Color.white);

            Debug.Log("Inimigo descongelado!");
        }

    }

    // M�todo para mudar a cor do inimigo
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

    // SEGUE
    private void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        // Gira o sprite dependendo da dire��o do movimento
        if (direction.x > 0 && !movingRight)
        {
            movingRight = true;
            Flip();
        }
        else if (direction.x < 0 && movingRight)
        {
            movingRight = false;
            Flip();
        }
    }

    private void Patrol()
    {
        // C�digo de patrulha j� existente
    }
}