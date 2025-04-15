using System;
using UnityEngine;
using UnityEngine.UI;
using static CharacterSkinManager;
using static UnityEngine.EventSystems.EventTrigger;

public class Duck : MonoBehaviour
{
    // Movement Classes
    public Rigidbody2D duckBody;
    private float direction;
    public float jumpForce;
    public float speed;
    private bool onGround;
    public Transform checkGround;
    public LayerMask ground;

    // Colliders
    private Collider2D characterCollider;
    public LayerMask EnemyLayer;
    private Collider2D[] enemiesInDashArea;

    // Animations Classes
    private bool lookingRight = true;
    public Animator animator;

    // Wall Jump
    public float wallJumpForce = 10f;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public float wallSlideSpeed = 1f;

    // Double Jump
    private bool canDoubleJump = false;

    // Dash (Electric)
    public float dashSpeed = 20f;
    public float dashDuration = 0.5f;
    private float dashTime = 0f;
    private bool isDashing = false;
    public float dashCooldown = 1f;
    private float dashCooldownTime = 0f;

    public TrailRenderer trail;

    // Swim (Water)

    public float waterJumpForceFactor = 0.5f;  // Fator para reduzir a força do pulo na água
    private bool isInWater = false;  // Variável para verificar se está na água
    public float waterGravityScale = 0.2f;  // Gravidade reduzida na água (ajuste conforme necessário)
    private bool isSwimming = false;

    // Float (Ghost)

    private bool isFlying = false;  // Para controlar se o voo está ativo
    private float flightTime = 5f;  // Tempo máximo de voo (em segundos)
    private float fallSpeed = 0.5f;  // A velocidade da queda quando o Space é solto
    private bool isGhostActive = false;      // Para saber se a skin Ghost está ativa
    private float maxFlightTime = 2f;  // Tempo máximo para segurar o espaço e voar (em segundos)
    private float currentFlightTime = 0f;  // Tempo de voo atual enquanto o space é pressionado
    private bool ghostSkinMessageShown = false; // Variável para controlar se a mensagem foi exibida

    private bool isGliding = false;  // Para controlar quando o personagem está planando

    // Fireball (Fire)
    private bool isFireSkinActive = false;
    public CharacterSkinManager skinManager;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;

    private float nextFireTime = 0f;

    // Iceball (Ice)
    public GameObject iceballPrefab;
    public Transform iceballPoint;
    private bool isIceSkinActive = false;  // Para controlar se a skin de gelo está ativa

    // Waterball (Water)

    public GameObject waterballPrefab;  // Prefab da Waterball
    public Transform waterballPoint;    // Ponto onde a Waterball será lançada
    private bool isWaterSkinActive = false;  // Para controlar se a skin de água está ativa



    void Start()
    {
        duckBody = GetComponent<Rigidbody2D>();
        characterCollider = GetComponent<Collider2D>();  // Pegando o Collider do personagem

        trail.enabled = false; // Garante que o trail começa desligado
    }

    void Update()
    {
        // Verifica se está no chão
        onGround = Physics2D.OverlapCircle(checkGround.position, 0.3f, ground);
        animator.SetBool("OnGround", onGround);
        animator.SetFloat("velocityY", duckBody.linearVelocity.y);

        // Movimento Horizontal
        direction = Input.GetAxisRaw("Horizontal");
        if (!isSwimming)
        {
            duckBody.linearVelocity = new Vector2(direction * speed, duckBody.linearVelocity.y);
        }
        animator.SetFloat("velocity", Mathf.Abs(direction));


        // Verificar se o personagem está na água
        if (isInWater)
        {
            // Lógica para nadar, gravidade reduzida, etc.
            duckBody.gravityScale = 0.2f;  // Gravidade reduzida para nadar
            isSwimming = true;  // O personagem está nadando
            animator.SetBool("isSwimming", true);  // Ativa animação de natação

            if (Input.GetKey(KeyCode.Space))  // Se pressionar espaço, o pulo será possível na água
            {
                // Permite que o personagem pule enquanto nada
                duckBody.linearVelocity = new Vector2(duckBody.linearVelocity.x, 5f);  // Ajuste o valor conforme necessário
            }
        }
        else
        {
            duckBody.gravityScale = 1f;  // Gravidade normal fora da água
            isSwimming = false;  // O personagem não está mais nadando
            animator.SetBool("isSwimming", false);  // Desativa animação de natação quando fora da água
        }

        // Reseta o Double Jump no chão e na parede,
        if ((onGround || isTouchingWall) && !isGhostActive)
        {
            canDoubleJump = true;
        }

        // Jump
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (!onGround && canDoubleJump && Input.GetKeyDown(KeyCode.Space) && !isGhostActive)
        {
            DoubleJump();
        }
        // Skin Reseta o Double Jump no chão e na parede, mas apenas se a skin não for Ghost
        if (skinManager != null)
        {
            bool isGhostActive = (skinManager.CurrentSkin == CharacterSkinManager.SkinType.Ghost);

            if (isGhostActive)
            {
                // Skin Ghost ativa
                if (!ghostSkinMessageShown)  // Verifica se a mensagem já foi mostrada
                {
                    Debug.Log("Skin Ghost está ativa.");
                    ghostSkinMessageShown = true; // Marca que a mensagem foi mostrada
                }
            }
            else
            {
                // Skin Ghost não está ativa
                ghostSkinMessageShown = false; // Reseta para permitir que a mensagem seja mostrada novamente.
            }
        }

        // Reseta o Double Jump no chão e na parede, mas apenas se a skin não for Ghost
        if ((onGround || isTouchingWall) && !isGhostActive)
        {
            canDoubleJump = true;
        }

        // Jump
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (!onGround && canDoubleJump && Input.GetKeyDown(KeyCode.Space) && !isGhostActive)
        {
            DoubleJump();
        }



        // Jump
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (!onGround && canDoubleJump && Input.GetKeyDown(KeyCode.Space) && !isGhostActive)
        {
            DoubleJump();
        }


        // Dash - Verifica se a skin é a Electric
        // Verifica se está no chão
        
        
        duckBody.linearVelocity = new Vector2(lookingRight ? dashSpeed : -dashSpeed, duckBody.linearVelocity.y);
        onGround = Physics2D.OverlapCircle(checkGround.position, 0.3f, ground);
        animator.SetBool("OnGround", onGround);
        animator.SetFloat("velocityY", duckBody.linearVelocity.y);

        // Movimento Horizontal
        if (!isDashing)
        {
            direction = Input.GetAxisRaw("Horizontal");
            duckBody.linearVelocity = new Vector2(direction * speed, duckBody.linearVelocity.y);
            animator.SetFloat("velocity", Mathf.Abs(direction));
        }

        // Se está dashing, manter a animação de dash
        if ((direction < 0 && lookingRight) || (direction > 0 && !lookingRight))
        {
            lookingRight = !lookingRight;
            transform.Rotate(0f, 180f, 0f);
        }



        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0f)
            {
                StopDash();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= dashCooldownTime)
        {
            if (skinManager.CurrentSkin == CharacterSkinManager.SkinType.Electric)  // Verifica se a skin ativa é a Electric
            {
                StartDash();
            }
            else
            {
                Debug.Log("Dash só pode ser usado com a skin Electric.");
            }
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0f)
            {
                StopDash();
            }
        }

        // Lógica de Wall Slide
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.2f, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(transform.position + Vector3.right * 0.2f, 0.2f, wallLayer) ||
                         Physics2D.OverlapCircle(transform.position + Vector3.left * 0.2f, 0.2f, wallLayer);

        if (isTouchingWall && !isGrounded && duckBody.linearVelocity.y < 0)
        {
            isWallSliding = true;
            duckBody.linearVelocity = new Vector2(duckBody.linearVelocity.x, Mathf.Max(duckBody.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }

        // Wall Jump
        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            WallJump();
        }

        // Fireball
        if (skinManager != null && skinManager.CurrentSkin == CharacterSkinManager.SkinType.Fire)
        {
            if (!isFireSkinActive)
            {
                isFireSkinActive = true;
                Debug.Log("A skin de fogo está ativa!");
            }

            if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextFireTime)
            {
                LaunchFireball();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            if (isFireSkinActive)
            {
                isFireSkinActive = false;
                Debug.Log("A skin de fogo não está mais ativa.");
            }
        }

        // Iceball
        // Lógica para a skin de gelo
        if (skinManager != null && skinManager.CurrentSkin == CharacterSkinManager.SkinType.Ice)
        {
            if (!isIceSkinActive)
            {
                isIceSkinActive = true;
                Debug.Log("A skin de gelo está ativa!");
            }

            if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextFireTime)
            {
                LaunchIceball();
                nextFireTime = Time.time + fireRate;  // Usando fireRate aqui também, mas pode ajustar conforme necessário
            }
        }
        else
        {
            if (isIceSkinActive)
            {
                isIceSkinActive = false;
                Debug.Log("A skin de gelo não está mais ativa.");
            }
        }
        // Waterball
        if (skinManager != null && skinManager.CurrentSkin == CharacterSkinManager.SkinType.Water)
        {
            if (!isWaterSkinActive)
            {
                isWaterSkinActive = true;
                Debug.Log("A skin de água está ativa!");
            }

            if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextFireTime)
            {
                LaunchWaterball();  // Lançando a Waterball
                nextFireTime = Time.time + fireRate;  // Usando fireRate também aqui
            }
        }
        else
        {
            if (isWaterSkinActive)
            {
                isWaterSkinActive = false;
                Debug.Log("A skin de água não está mais ativa.");
            }
        }

        // Detecta a entrada na água
        if (isInWater && skinManager.CurrentSkin != CharacterSkinManager.SkinType.Water)
        {
            // Muda a skin para a de água
            skinManager.ChangeSkin(CharacterSkinManager.SkinType.Water);
            Debug.Log("A skin foi alterada para Water");
        }


        // Outras lógicas de movimento e habilidade de skins...

        // Exemplo de código de habilidade Waterball (continua como está)
        if (skinManager != null && skinManager.CurrentSkin == CharacterSkinManager.SkinType.Water)
        {
            if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextFireTime)
            {
                LaunchWaterball();  // Lançando a Waterball
                nextFireTime = Time.time + fireRate;  // Usando fireRate também aqui
            }
        }

        // Ghost

        // Ghost - Controle de voo
        if (skinManager != null && skinManager.CurrentSkin == CharacterSkinManager.SkinType.Ghost)
        {
            isGhostActive = true;  // A skin Ghost está ativa

            // Se o jogador pressionar o espaço e o voo não estiver ativo
            if (Input.GetKey(KeyCode.Space) && !isFlying && currentFlightTime < maxFlightTime)
            {
                StartFlight();  // Começa o voo
                currentFlightTime += Time.deltaTime;  // Acumula o tempo de voo
            }

            // Se o voo estiver ativo, controlar o voo e a gravidade
            if (isFlying)
            {
                // Enquanto o jogador segurar o espaço, o voo é ativado
                duckBody.gravityScale = 0.1f;  // Reduz a gravidade para simular o voo

                // Se o jogador soltar o espaço, o personagem começa a planar
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    StartGliding();  // Começa a planar
                }

                // Se o tempo de voo terminar, fazer o personagem cair
                if (currentFlightTime >= maxFlightTime)
                {
                    StopFlight();
                }
            }

            // Se o personagem já estiver planando, controle da gravidade para planar
            if (isGliding)
            {
                // Continuar a planar com uma gravidade mais baixa
                duckBody.gravityScale = 0.2f;  // Gravedad reduzida enquanto planeja
            }
        }
        else
        {
            isGhostActive = false;  // A skin Ghost não está ativa
            duckBody.gravityScale = 1f;  // Retorna à gravidade normal

            // Restaurar colisões e gravidade normal se a skin não for Ghost
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (var collider in colliders)
            {
                Physics2D.IgnoreCollision(characterCollider, collider, false);
            }
        }

        // Intangibilidade - Ignora colisões com todos os objetos, exceto o chão
        if (isGhostActive)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (var collider in colliders)
            {
                if (collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
                {
                    Physics2D.IgnoreCollision(characterCollider, collider, true);
                }
            }
        }
    }


    private void StartDash()
    {
        trail.enabled = true; // Ativa o trail
        Physics2D.IgnoreLayerCollision(3, 7, true);
        
        isDashing = true;
        dashTime = dashDuration;
        dashCooldownTime = Time.time + dashCooldown;  // Inicia o cooldown após o dash
        duckBody.linearVelocity = new Vector2(lookingRight ? dashSpeed : -dashSpeed, duckBody.linearVelocity.y);
        animator.SetTrigger("Dash");

        // Ignorar colisões com inimigos durante o dash
        Collider2D[] enemiesInDashArea = Physics2D.OverlapCircleAll(transform.position, 1f, EnemyLayer);
        foreach (Collider2D enemy in enemiesInDashArea)
        {
            if (enemy != null)
            {
                Physics2D.IgnoreCollision(characterCollider, enemy, true);
            }
        }
    }

    private void StopDash()
    {
        trail.enabled = false; // Desativa o trail
        Physics2D.IgnoreLayerCollision(3, 7, false);

        isDashing = false;
        duckBody.linearVelocity = new Vector2(0, duckBody.linearVelocity.y);  // Para garantir que ele pare de se mover horizontalmente

        // Restaurar colisões com os inimigos após o dash
        Collider2D[] enemiesInDashArea = Physics2D.OverlapCircleAll(transform.position, 1f, EnemyLayer);
        foreach (Collider2D enemy in enemiesInDashArea)
        {
            if (enemy != null)
            {
                Physics2D.IgnoreCollision(characterCollider, enemy, false);
            }
        }

    }

    private void WallJump()
    {
        Vector2 wallJumpDirection = lookingRight ? Vector2.left : Vector2.right;
        duckBody.linearVelocity = new Vector2(wallJumpDirection.x * wallJumpForce, wallJumpForce);
        canDoubleJump = true;
    }

    private void Jump()
    {
        if (!isInWater)
        {
            duckBody.linearVelocity = new Vector2(duckBody.linearVelocity.x, jumpForce);  // Pulo normal fora da água
        }
        else
        {
            // Pulo reduzido enquanto está na água
            duckBody.linearVelocity = new Vector2(duckBody.linearVelocity.x, jumpForce * waterJumpForceFactor);
        }
    }

    private void DoubleJump()
    {
        // Apenas permite o double jump enquanto não está na água
        if (isInWater)
        {
            // Permite pulo contínuo enquanto estiver nadando com força reduzida
            duckBody.linearVelocity = new Vector2(duckBody.linearVelocity.x, jumpForce * waterJumpForceFactor);
            canDoubleJump = true;
        }
        else
        {
            // Normal double jump
            duckBody.linearVelocity = new Vector2(duckBody.linearVelocity.x, jumpForce);
            canDoubleJump = false;
            animator.SetTrigger("DuckFlip");
        }
    }


    void LaunchFireball()
    {
        if (firePoint == null || fireballPrefab == null)
        {
            Debug.LogError("FirePoint ou FireballPrefab não atribuídos!");
            return;
        }

        animator.SetTrigger("FireAttack");
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
        Fireball fireballScript = fireball.GetComponent<Fireball>();
        if (fireballScript != null)
        {
            Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
            fireballRb.linearVelocity = new Vector2(lookingRight ? fireballScript.speed : -fireballScript.speed, 0);
        }
        else
        {
            Debug.LogError("Prefab de Fireball não tem o componente Fireball!");
        }
    }

    void LaunchIceball()
    {
        if (iceballPoint == null || iceballPrefab == null)
        {
            Debug.LogError("IceballPoint ou IceballPrefab não atribuídos!");
            return;
        }

        animator.SetTrigger("IceAttack");
        GameObject iceball = Instantiate(iceballPrefab, iceballPoint.position, iceballPoint.rotation);
        Iceball iceballScript = iceball.GetComponent<Iceball>();
        if (iceballScript != null)
        {
            Rigidbody2D iceballRb = iceball.GetComponent<Rigidbody2D>();
            iceballRb.linearVelocity = new Vector2(lookingRight ? iceballScript.speed : -iceballScript.speed, 0);
        }
        else
        {
            Debug.LogError("Prefab de Iceball não tem o componente Iceball!");
        }

    }

    void LaunchWaterball()
    {
        if (waterballPoint == null || waterballPrefab == null)
        {
            Debug.LogError("WaterballPoint ou WaterballPrefab não atribuídos!");
            return;
        }

        animator.SetTrigger("WaterAttack");
        GameObject waterball = Instantiate(waterballPrefab, waterballPoint.position, waterballPoint.rotation);

        Waterball waterballScript = waterball.GetComponent<Waterball>();
        if (waterballScript != null)
        {
            Rigidbody2D waterballRb = waterball.GetComponent<Rigidbody2D>();
            waterballRb.linearVelocity = new Vector2(lookingRight ? waterballScript.speed : -waterballScript.speed, 0);  // Lança a água para a direita ou esquerda
        }
        else
        {
            Debug.LogError("Prefab de Waterball não tem o componente Waterball!");
        }
    }

    // Método para entrar na água
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))  // Certifique-se de que a água tem a tag "Water"
        {
            EnterWater();
        }
    }

    // Método para sair da água
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            ExitWater();
        }
    }

    /// Função chamada quando o personagem entra na água
    public void EnterWater()
    {
        isInWater = true;  // Marca que o personagem entrou na água
        animator.SetBool("isSwimming", true);  // Ativa animação de nadar
    }

    // Função chamada quando o personagem sai da água
    public void ExitWater()
    {
        isInWater = false;  // Marca que o personagem saiu da água
        if (skinManager.CurrentSkin == CharacterSkinManager.SkinType.Water)
        {
            animator.SetBool("isSwimming", false);  // Desativa animação de nadar se a skin for Water
        }
    }

    // Inicia o voo (quando o Space é pressionado)
    private void StartFlight()
    {
        isFlying = true;  // O voo está ativado
        flightTime = maxFlightTime;  // Reseta o tempo de voo para o máximo
        duckBody.linearVelocity = new Vector2(duckBody.linearVelocity.x, 0);  // Reseta a velocidade Y para evitar a queda imediata
        duckBody.gravityScale = 0.1f;  // Reduz a gravidade
        Debug.Log("Iniciando voo...");
    }

    private void StartGliding()
    {
        isGliding = true;  // O personagem começa a planar
        duckBody.gravityScale = 0.2f;  // Reduz a gravidade ainda mais para planar
        duckBody.linearVelocity = new Vector2(duckBody.linearVelocity.x, -fallSpeed);  // Ajusta a velocidade de queda durante o planamento
        Debug.Log("Iniciando planeio...");
    }

    // Para o voo
    private void StopFlight()
    {
        isFlying = false;  // Desativa o voo
        duckBody.gravityScale = 1f;  // Restaura a gravidade normal
        currentFlightTime = 0f;  // Reseta o tempo de voo
        Debug.Log("Voo terminado.");
    }

    // Para de planar e começa a cair com gravidade normal
    private void StopGliding()
    {
        isGliding = false;
        duckBody.gravityScale = 1f;  // Restaura a gravidade normal
        Debug.Log("Parando o planeio...");
    }
}

