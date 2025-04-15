using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float afundamento = 0.5f; // Quanto a plataforma afunda
    public float velocidadeAfundamento = 2.0f; // Velocidade do afundamento
    private Vector3 posicaoInicial; // Posi��o original da plataforma
    private bool jogadorEmCima = false; // Se o jogador est� sobre a plataforma
    private Rigidbody2D rbJogador; // Refer�ncia ao Rigidbody2D do jogador

    void Start()
    {
        posicaoInicial = transform.position; // Guarda a posi��o inicial
    }

    // Chamado quando o jogador entra em colis�o com a plataforma
    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.collider.CompareTag("Player")) // Verifica se � o jogador
        {
            jogadorEmCima = true;
            rbJogador = colisao.collider.GetComponent<Rigidbody2D>(); // Obt�m o Rigidbody2D do jogador
            if (rbJogador != null)
            {
                rbJogador.gravityScale = 0; // Desativa a gravidade do jogador temporariamente
            }
        }
    }

    // Chamado quando o jogador sai da colis�o com a plataforma
    void OnCollisionExit2D(Collision2D colisao)
    {
        if (colisao.collider.CompareTag("Player")) // Verifica se � o jogador
        {
            jogadorEmCima = false;
            if (rbJogador != null)
            {
                rbJogador.gravityScale = 1; // Reativa a gravidade do jogador
            }
        }
    }

    // Update � chamado uma vez por frame
    void Update()
    {
        // Se o jogador est� em cima, a plataforma afunda suavemente
        if (jogadorEmCima)
        {
            AfundarPlataforma();
        }
        // Se o jogador n�o est� em cima, a plataforma volta � posi��o inicial suavemente
        else
        {
            VoltarPosicaoInicial();
        }
    }

    // Fun��o para afundar a plataforma suavemente
    void AfundarPlataforma()
    {
        Vector3 destinoAfundamento = new Vector3(transform.position.x, posicaoInicial.y - afundamento, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destinoAfundamento, Time.deltaTime * velocidadeAfundamento);
    }

    // Fun��o para voltar a plataforma � posi��o inicial suavemente
    void VoltarPosicaoInicial()
    {
        transform.position = Vector3.Lerp(transform.position, posicaoInicial, Time.deltaTime * 3f); // A velocidade de retorno � 3 vezes mais r�pida
    }
}
