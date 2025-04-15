using System.Collections;
using UnityEngine;

public class IceStakeController : MonoBehaviour
{
    public float fallSpeed = 5f; // Velocidade de queda
    public Vector2 spawnPosition; // Posi��o onde a estaca ir� renascer
    public float fallDelay = 1f; // Tempo de delay entre as quedas

    private bool isFalling = false;  // Indica se a estaca est� caindo
    private bool canFall = false;    // Indica se a estaca pode come�ar a cair
    private float groundY;           // Posi��o Y do ch�o

    void Start()
    {
        spawnPosition = transform.position; // Posi��o inicial da estaca
        groundY = -5f; // Ajuste de acordo com a posi��o do ch�o
    }

    void Update()
    {
        if (isFalling)
        {
            // Move a estaca para baixo
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
    }

    // Detec��o quando o jogador entra na �rea de trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica se o objeto � o jogador
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

                // Aguarda at� a estaca atingir o ch�o
                while (transform.position.y > groundY)
                {
                    yield return null; // Espera at� a estaca cair
                }

                // A estaca atinge o ch�o, ent�o faz ela desaparecer e renascer
                isFalling = false;
                transform.position = spawnPosition; // Reseta a posi��o para o ponto de queda
                canFall = false; // Desativa a queda at� o jogador passar de novo
                yield return new WaitForSeconds(fallDelay); // Aguarda um tempo antes da pr�xima queda
            }

            yield return null;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obt�m o script de sa�de do jogador e aplica o dano
            DuckHealth playerHealth = collision.gameObject.GetComponent<DuckHealth>();
            if (playerHealth != null)
            {
                // Calcula a dire��o do dano (da estaca para o jogador)
                Vector2 direcaoDano = collision.transform.position - transform.position;
                direcaoDano = direcaoDano.normalized;  // Normaliza a dire��o para garantir que tenha magnitude 1

                // Aplica o dano e a dire��o
                playerHealth.TakeDamage(1, direcaoDano); // Aplica dano com dire��o
            }

            // Aqui voc� pode adicionar uma l�gica para a estaca desaparecer ou ser destru�da ap�s o dano
            Destroy(gameObject); // Destroi a estaca ap�s causar dano (opcional)
        }
    }
}
