using UnityEngine;

public class Iceball : MonoBehaviour
{
    // Velocidade da bola de gelo
    public float speed = 10f;
    public float freezeDuration = 3f; // Duração do congelamento
    public float deceleration = 0.2f; // Quanto a bola perde de velocidade por segundo

    private Rigidbody2D rb;          // Referência para o Rigidbody2D
    private Collider2D col;          // Referência para o Collider2D
    public float lifetime = 5f;      // Tempo antes de a bola desaparecer

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Pega o Rigidbody2D da bola de gelo
        col = GetComponent<Collider2D>(); // Pega o Collider2D da bola de gelo

        if (rb == null || col == null)
        {
            Debug.LogError("Rigidbody2D ou Collider2D não encontrados! Verifique se ambos os componentes foram adicionados.");
            return;
        }

        // Desabilitar a gravidade inicialmente para não cair
        rb.gravityScale = 0;

        // Aplica a velocidade inicial na direção para a qual a bola de gelo está olhando
        rb.linearVelocity = transform.right * speed;

        // Habilitar o collider para que as colisões funcionem
        col.enabled = true;

        // Após o tempo de vida da bola, destrua a instância clonada (não o prefab)
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Reduzindo a velocidade com o tempo (dando o efeito de "perder força")
        if (rb.linearVelocity.magnitude > 0)
        {
            rb.linearVelocity = rb.linearVelocity * (1 - deceleration * Time.deltaTime); // Vai diminuindo a velocidade ao longo do tempo
        }

        // Quando a bola de gelo começa a perder força, podemos permitir que ela caia
        if (rb.linearVelocity.magnitude <= 0.1f) // Quando a velocidade estiver baixa o suficiente
        {
            rb.gravityScale = 1; // Começa a aplicar a gravidade
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto com o qual colidiu é um inimigo
        if (collision.CompareTag("Enemy"))
        {
            // Congela o inimigo ao colidir com a bola de gelo
            FreezeEnemy(collision.gameObject);
        }

        // Destroi a bola de gelo ao colidir com qualquer outro objeto
        Destroy(gameObject);
    }

    // Função para congelar o inimigo
    private void FreezeEnemy(GameObject enemy)
    {
        var enemyScript = enemy.GetComponent<Enemy>(); // Substitua "Enemy" pelo nome do seu script de inimigo

        if (enemyScript != null)
        {
            enemyScript.Freeze(freezeDuration); // Congela o inimigo por uma duração
        }
    }
}
