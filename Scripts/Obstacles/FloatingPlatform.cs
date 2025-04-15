using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float afundamento = 0.5f; // Quanto a plataforma afunda
    public float velocidadeAfundamento = 2.0f; // Velocidade do afundamento
    private Vector3 posicaoInicial; // Posição original da plataforma
    private bool jogadorEmCima = false; // Se o jogador está sobre a plataforma
    private Rigidbody2D rbJogador; // Referência ao Rigidbody2D do jogador

    void Start()
    {
        posicaoInicial = transform.position; // Guarda a posição inicial
    }

    // Chamado quando o jogador entra em colisão com a plataforma
    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.collider.CompareTag("Player")) // Verifica se é o jogador
        {
            jogadorEmCima = true;
            rbJogador = colisao.collider.GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D do jogador
            if (rbJogador != null)
            {
                rbJogador.gravityScale = 0; // Desativa a gravidade do jogador temporariamente
            }
        }
    }

    // Chamado quando o jogador sai da colisão com a plataforma
    void OnCollisionExit2D(Collision2D colisao)
    {
        if (colisao.collider.CompareTag("Player")) // Verifica se é o jogador
        {
            jogadorEmCima = false;
            if (rbJogador != null)
            {
                rbJogador.gravityScale = 1; // Reativa a gravidade do jogador
            }
        }
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        // Se o jogador está em cima, a plataforma afunda suavemente
        if (jogadorEmCima)
        {
            AfundarPlataforma();
        }
        // Se o jogador não está em cima, a plataforma volta à posição inicial suavemente
        else
        {
            VoltarPosicaoInicial();
        }
    }

    // Função para afundar a plataforma suavemente
    void AfundarPlataforma()
    {
        Vector3 destinoAfundamento = new Vector3(transform.position.x, posicaoInicial.y - afundamento, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destinoAfundamento, Time.deltaTime * velocidadeAfundamento);
    }

    // Função para voltar a plataforma à posição inicial suavemente
    void VoltarPosicaoInicial()
    {
        transform.position = Vector3.Lerp(transform.position, posicaoInicial, Time.deltaTime * 3f); // A velocidade de retorno é 3 vezes mais rápida
    }
}
