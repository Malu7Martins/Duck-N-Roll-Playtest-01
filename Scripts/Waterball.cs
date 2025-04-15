using System;
using UnityEngine;

public class Waterball : MonoBehaviour
{
    public float speed = 10f;        // Velocidade inicial da bola de fogo
    public float lifetime = 5f;      // Tempo antes de a bola desaparecer
    public float deceleration = 0.2f; // Quanto a bola perde de velocidade por segundo
    private Rigidbody2D rb;          // Refer�ncia para o Rigidbody2D
    private Collider2D col;          // Refer�ncia para o Collider2D

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Pega o Rigidbody2D da bola de fogo
        col = GetComponent<Collider2D>(); // Pega o Collider2D da bola de fogo

        if (rb == null || col == null)
        {
            Debug.LogError("Rigidbody2D ou Collider2D n�o encontrados! Verifique se ambos os componentes foram adicionados.");
            return;
        }

        // Desabilitar a gravidade inicialmente para n�o cair
        rb.gravityScale = 0;

        // Aplica a velocidade inicial na dire��o para a qual a bola de fogo est� olhando
        rb.linearVelocity = transform.right * speed;

        // Habilitar o collider para que as colis�es funcionem
        col.enabled = true;

        // Ap�s o tempo de vida da bola, destrua a inst�ncia clonada (n�o o prefab)
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        // Reduzindo a velocidade com o tempo (dando o efeito de "perder for�a")
        if (rb.linearVelocity.magnitude > 0)
        {
            rb.linearVelocity = rb.linearVelocity * (1 - deceleration * Time.deltaTime); // Vai diminuindo a velocidade ao longo do tempo
        }

        // Quando a bola de fogo come�a a perder for�a, podemos permitir que ela caia
        if (rb.linearVelocity.magnitude <= 0.1f) // Quando a velocidade estiver baixa o suficiente
        {
            rb.gravityScale = 1; // Come�a a aplicar a gravidade
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto colidido � um inimigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Acessa o script do inimigo 
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {

            }
        }

        // Destr�i a bola de fogo ap�s a colis�o com qualquer objeto
        Destroy(gameObject);
    }

    // Implementa��o do SetDirection

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Certifique-se de que o Rigidbody2D � atribu�do na inicializa��o
        col = GetComponent<Collider2D>(); // Certifique-se de que o Collider2D tamb�m � atribu�do
        if (rb == null || col == null)
        {
            Debug.LogError("Rigidbody2D ou Collider2D n�o encontrados! Certifique-se de que ambos os componentes foram adicionados.");
        }
    }

    [Obsolete]
    internal void SetDirection(Vector2 direction)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D não encontrado ao tentar definir a dire��o. Verifique a inicialização.");
            return; // N�o faz nada se o Rigidbody2D n�o for encontrado
        }

        // Altera a dire��o da bola de fogo com base no vetor passado
        rb.linearVelocity = direction * speed;  // Aplique a velocidade na dire��o fornecida
        transform.right = direction;  // Rotaciona a bola de fogo para a dire��o correta
    }
}
