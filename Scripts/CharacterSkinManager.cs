using UnityEngine;

public class CharacterSkinManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public BoxCollider2D characterCollider;

    public Sprite duckSprite;
    public Sprite fireSprite;
    public Sprite waterSprite;
    public Sprite iceSprite;
    public Sprite electricSprite;
    public Sprite ghostSprite;

    public RuntimeAnimatorController duckController;
    public RuntimeAnimatorController fireController;
    public RuntimeAnimatorController waterController;
    public RuntimeAnimatorController iceController;
    public RuntimeAnimatorController electricController;
    public RuntimeAnimatorController ghostController;

    public enum SkinType { Duck, Fire, Water, Ice, Electric, Ghost }

    // Variável privada para armazenar a skin atual
    private SkinType currentSkin = SkinType.Duck;

    // Propriedade pública para acessar a skin atual
    public SkinType CurrentSkin
    {
        get { return currentSkin; }
        private set { currentSkin = value; }
    }

    // Lista de skins desbloqueadas. Inicialmente só a skin "Duck" está desbloqueada.
    private bool[] unlockedSkins = new bool[6];

    void Start()
    {
        // Inicializa com a skin Duck desbloqueada
        unlockedSkins[(int)SkinType.Duck] = true;

        // Começa com a skin Duck ativa
        ChangeSkin(SkinType.Duck);
    }

    void Update()
    {
        // Verifica se o jogador apertou alguma tecla para trocar de skin, mas só troca se a skin estiver desbloqueada.
        if (Input.GetKeyDown(KeyCode.T) && unlockedSkins[(int)SkinType.Duck])
        {
            ChangeSkin(SkinType.Duck);
        }
        else if (Input.GetKeyDown(KeyCode.Y) && unlockedSkins[(int)SkinType.Fire])
        {
            ChangeSkin(SkinType.Fire);
        }
        else if (Input.GetKeyDown(KeyCode.U) && unlockedSkins[(int)SkinType.Water])
        {
            ChangeSkin(SkinType.Water);
        }
        else if (Input.GetKeyDown(KeyCode.I) && unlockedSkins[(int)SkinType.Ice])
        {
            ChangeSkin(SkinType.Ice);
        }
        else if (Input.GetKeyDown(KeyCode.O) && unlockedSkins[(int)SkinType.Electric])
        {
            ChangeSkin(SkinType.Electric);
        }
        else if (Input.GetKeyDown(KeyCode.P) && unlockedSkins[(int)SkinType.Ghost])
        {
            ChangeSkin(SkinType.Ghost);
        }
    }

    // Função para mudar a skin
    public void ChangeSkin(SkinType skin)
    {
        // Verifica se a skin está desbloqueada antes de mudar
        if (!unlockedSkins[(int)skin])
        {
            Debug.Log("Esta skin ainda não está desbloqueada!");
            return;
        }

        // Atualiza a skin atual
        CurrentSkin = skin;  // Usando a propriedade pública

        // Troca a sprite e o controller do animator
        switch (skin)
        {
            case SkinType.Duck:
                spriteRenderer.sprite = duckSprite;
                animator.runtimeAnimatorController = duckController;
                break;

            case SkinType.Fire:
                spriteRenderer.sprite = fireSprite;
                animator.runtimeAnimatorController = fireController;
                break;

            case SkinType.Water:
                spriteRenderer.sprite = waterSprite;
                animator.runtimeAnimatorController = waterController;
                break;

            case SkinType.Ice:
                spriteRenderer.sprite = iceSprite;
                animator.runtimeAnimatorController = iceController;
                break;

            case SkinType.Electric:
                spriteRenderer.sprite = electricSprite;
                animator.runtimeAnimatorController = electricController;
                break;

            case SkinType.Ghost:
                spriteRenderer.sprite = ghostSprite;
                animator.runtimeAnimatorController = ghostController;
                break;
        }

        // Ajuste da hitbox (Collider2D) dependendo da skin
        AdjustHitbox(skin);
    }

    // Função para ajustar o tamanho da hitbox
    private void AdjustHitbox(SkinType skin)
    {
        Vector2 newSize = Vector2.zero;

        // Baseado no tipo de skin, ajuste o tamanho da hitbox (Collider2D)
        switch (skin)
        {
            case SkinType.Duck:
                newSize = new Vector2(0.50f, 0.9f);
                break;

            case SkinType.Fire:
                newSize = new Vector2(0.50f, 1.0f);
                break;

            case SkinType.Water:
                newSize = new Vector2(0.50f, 0.9f);
                break;

            case SkinType.Ice:
                newSize = new Vector2(0.50f, 0.9f);
                break;

            case SkinType.Electric:
                newSize = new Vector2(0.50f, 1.0f);
                break;

            case SkinType.Ghost:
                newSize = new Vector2(0.50f, 1.0f);
                break;
        }

        // Verifique se o characterCollider está atribuído e ajusta a hitbox
        if (characterCollider != null)
        {
            characterCollider.size = newSize;
        }
        else
        {
            Debug.LogWarning("O BoxCollider2D não está atribuído no personagem!");
        }
    }

    // Função que será chamada para desbloquear uma skin
    public void UnlockSkin(SkinType skin)
    {
        // Desbloqueia a skin
        unlockedSkins[(int)skin] = true;
        Debug.Log(skin.ToString() + " desbloqueada!");
    }
}
