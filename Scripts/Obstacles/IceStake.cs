using System.Collections;
using UnityEngine;

public class IceStakeController : MonoBehaviour
{
    public float fallSpeed = 5f; // Velocidade de queda
    public Vector2 spawnPosition; // Posição onde a estaca irá renascer
    public float fallDelay = 1f; // Tempo de delay entre as quedas

    private bool isFalling = false;  // Indica se a estaca está caindo
    private bool canFall = false;    // Indica se a estaca pode começar a cair
    private float groundY;           // Posição Y do chão

    void Start()
    {
        spawnPosition = transform.position; // Posição inicial da estaca
        groundY = -5f; // Ajuste de acordo com a posição do chão
    }

    void Update()
    {
        if (isFalling)
        {
            // Move a estaca para baixo
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
    }

    // Detecção quando o jogador entra na área de trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica se o objeto é o jogador
        {
            canFall = true; // Permite que a estaca comece a cair
            StartCoroutine(FallCycle()); // Inicia o ciclo de queda
        }
    }

    // Ciclo de queda e renascimento
    IEnumerator FallCycle()
    {
        while (true)
        {
            if (canFall)
            {
                yield return new WaitForSeconds(fallDelay); // Espera o delay antes de cair
                isFalling = true;

                // Aguarda até a estaca atingir o chão
                while (transform.position.y > groundY)
                {
                    yield return null; // Espera até a estaca cair
                }

                // A estaca atinge o chão, então faz ela desaparecer e renascer
                isFalling = false;
                transform.position = spawnPosition; // Reseta a posição para o ponto de queda
                canFall = false; // Desativa a queda até o jogador passar de novo
                yield return new WaitForSeconds(fallDelay); // Aguarda um tempo antes da próxima queda
            }

            yield return null;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtém o script de saúde do jogador e aplica o dano
            DuckHealth playerHealth = collision.gameObject.GetComponent<DuckHealth>();
            if (playerHealth != null)
            {
                // Calcula a direção do dano (da estaca para o jogador)
                Vector2 direcaoDano = collision.transform.position - transform.position;
                direcaoDano = direcaoDano.normalized;  // Normaliza a direção para garantir que tenha magnitude 1

                // Aplica o dano e a direção
                playerHealth.TakeDamage(1, direcaoDano); // Aplica dano com direção
            }

            // Aqui você pode adicionar uma lógica para a estaca desaparecer ou ser destruída após o dano
            Destroy(gameObject); // Destroi a estaca após causar dano (opcional)
        }
    }
}
