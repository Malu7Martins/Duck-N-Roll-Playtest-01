using UnityEngine;
using UnityEngine.UI;  // Para trabalhar com elementos UI como a Image
using System.Collections;
using UnityEngine.SceneManagement;

public class DuckHealth : MonoBehaviour
{
    public int vidaMaxima = 3;  // Vida máxima do personagem
    private int vidaAtual;  // Vida atual do personagem

    // Referência para as imagens
    public Image imagemVida;  // Referência para a imagem de vida no Canvas
    public Sprite spriteVerde;  // Sprite com 3 corações (vida cheia)
    public Sprite spriteAzul;   // Sprite com 2 corações
    public Sprite spriteVermelho; // Sprite com 1 coração
    public Sprite spriteCinza;   // Sprite com 0 corações (Game Over)

    // Referência para o SpriteRenderer do personagem
    private SpriteRenderer spriteRenderer;

    // Tempo para manter o personagem vermelho após tomar dano e verde após curar
    public float tempoEfeito = 0.2f;  // Tempo que o personagem ficará com o efeito de cor
    private Color corOriginal;

    private Animator animator;

    // Referência para o Rigidbody2D do personagem
    private Rigidbody2D rb;

    // Força do empurrão (ajuste conforme necessário)
    public float forçaEmpurrao = 15f;

    void Start()
    {
        vidaAtual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();
        corOriginal = spriteRenderer.color;
        AtualizarImagem();

        // Pegando o componente Animator
        animator = GetComponent<Animator>();

        // Pegando o componente Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    // Função para tomar dano
    public void TakeDamage(int dano, Vector2 direcaoDano)
    {
        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        // Obtém o Rigidbody2D do jogador
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Aplica a força para empurrar o jogador na direção do dano com a força definida
            rb.AddForce(direcaoDano.normalized * forçaEmpurrao, ForceMode2D.Impulse);  // Use a variável forçaEmpurrao aqui
        }

        // Só fica vermelho se ainda tiver vida
        if (vidaAtual > 0)
        {
            StartCoroutine(FicarVermelhoTemporariamente());
        }

        AtualizarImagem();
    }


    // Função para curar o personagem
    public void Heal(int cura)
    {
        vidaAtual += cura;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);  // Garante que não ultrapasse a vida máxima
        StartCoroutine(FicarVerdeTemporariamente());  // Adiciona o efeito verde de cura
        AtualizarImagem();
    }

    // Função para mudar a cor do personagem para vermelho por um tempo
    private IEnumerator FicarVermelhoTemporariamente()
    {
        spriteRenderer.color = Color.red;  // Torna o personagem vermelho
        yield return new WaitForSeconds(tempoEfeito);  // Espera pelo tempo definido
        spriteRenderer.color = corOriginal;  // Restaura a cor original
    }

    // Função para mudar a cor do personagem para verde por um tempo (efeito de cura)
    private IEnumerator FicarVerdeTemporariamente()
    {
        spriteRenderer.color = Color.green;  // Torna o personagem verde
        yield return new WaitForSeconds(tempoEfeito);  // Espera pelo tempo definido
        spriteRenderer.color = corOriginal;  // Restaura a cor original
    }

    // Atualiza a imagem da UI conforme a vida atual
    void AtualizarImagem()
    {
        if (vidaAtual == 3)
        {
            imagemVida.sprite = spriteVerde;  // Vida cheia - 3 corações
        }
        else if (vidaAtual == 2)
        {
            imagemVida.sprite = spriteAzul;  // 2 corações
        }
        else if (vidaAtual == 1)
        {
            imagemVida.sprite = spriteVermelho;  // 1 coração
        }
        else
        {
            imagemVida.sprite = spriteCinza;  // Game Over
            GameOver();  // Lógica do Game Over
        }
    }

    // Lógica do Game Over
    void GameOver()
    {
        animator.SetTrigger("Dead");
        Debug.Log("Game Over");
        StartCoroutine(ReiniciarCenaDepois(0.1f)); // espera 2 segundos antes de reiniciar a cena
    }

    IEnumerator ReiniciarCenaDepois(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reinicia a cena atual
    }

    // Função para empurrar o personagem para o lado oposto ao dano
    private void EmpurrarPersonagem(Vector2 direçãoDano)
    {
        // Aplica a força para o lado oposto da direção do dano
        Vector2 direçãoOposta = -direçãoDano.normalized;  // Inverte a direção do dano
        rb.AddForce(direçãoOposta * forçaEmpurrao, ForceMode2D.Impulse);  // Empurra o personagem
    }

    // Testar o dano pressionando a tecla "H" e curar pressionando a tecla "C"
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))  // Pressione "H" para tomar dano
        {
            TakeDamage(1, new Vector2(1, 0));  // Exemplo de dano vindo da direita (pode ser modificado para outras direções)
        }

        if (Input.GetKeyDown(KeyCode.C))  // Pressione "C" para curar
        {
            Heal(1);  // O personagem ganha 1 de vida
        }
    }

    IEnumerator EsperarEPararJogo()
    {
        yield return new WaitForSeconds(1.5f); // Ajuste conforme a duração da animação
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }
}
