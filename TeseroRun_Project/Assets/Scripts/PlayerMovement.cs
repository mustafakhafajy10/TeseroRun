using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;
    public float attackDuration = 0.5f;
    public GameObject swordSlashPrefab;
    public GameObject slash;
    public Vector3 slashOffset;
    public GameManager gameManager;

    private bool isJumping = false;
    private bool isAttacking = false;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;

    // Health variables
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    private float lastDirection = 1.0f; // Direction player is facing
    private int jumpCount = 0; // Counter for double jump
    public float fallThreshold = -600f; // Y-coordinate threshold for falling off the map
    public static int LevelScore = 0;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth); // Initialize health bar
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
        UpdateAnimator();
        //CheckFallOffMap(); // Check if the player has fallen off the map
        // Check if 10 keys have been collected and move to the next level

        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager == null)
            {
                return;
            }
        }

        if (gameManager.GetKeyScore() <= 0 && SceneManager.GetActiveScene().buildIndex != 3)
        {
            LoadNextScene();
            gameManager.setKeyScore(10);
        }

    }

    private void HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (!isAttacking)
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
            rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

            if (moveInput != 0)
            {
                lastDirection = moveInput;
                FlipSprite(lastDirection, GetComponent<SpriteRenderer>());
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
    }


    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1; // Load the next scene
        LevelScore = gameManager.GetScore();
        gameManager.setKeyScore(10);
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void FlipSprite(float direction, SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null) return;
        spriteRenderer.flipX = direction < 0;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < 2)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
            isJumping = true;
        }

        if (rb.linearVelocity.y == 0)
        {
            isJumping = false;
            jumpCount = 0;
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private System.Collections.IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        animator.SetTrigger("doAttack"); // Trigger the attack animation
        SpawnSlash(); // Spawn the slash attack
        yield return new WaitForSeconds(attackDuration); // Wait for the attack duration
        isAttacking = false;
    }

    private void SpawnSlash()
    {
        Vector3 spawnPosition = transform.position + new Vector3(lastDirection * slashOffset.x, slashOffset.y, 0.0f);
        slash = Instantiate(swordSlashPrefab, spawnPosition, Quaternion.identity);

        SpriteRenderer slashRenderer = slash.GetComponent<SpriteRenderer>();
        if (slashRenderer != null)
        {
            slashRenderer.flipX = lastDirection < 0;
        }
    }
    public void UnspawnSlash()
    {
        if (slash != null)
        {
            Destroy(slash);
            slash = null; // Reset the reference
        }
    }
    private void UpdateAnimator()
    {
        if (moveInput == 0)
        {
            animator.SetFloat("CharacterSpeed", 1);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("CharacterSpeed", 6);
        }
        else
        {
            animator.SetFloat("CharacterSpeed", 4);
        }

        animator.SetBool("isJumping", isJumping);
    }

    private void CheckFallOffMap()
    {
        if (transform.position.y < fallThreshold)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            gameManager.AddScore(5); // Add score when colliding with Coin
            Destroy(other.gameObject); // Destroy the coin
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            TakeDamage(10); // Remove points on trap collision
        }
        else if (other.gameObject.CompareTag("TrapRed"))
        {
            TakeDamage(50); // Remove points on trap collision
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20); // Remove points on trap collision
        }
        else if (other.gameObject.CompareTag("Key"))
        {
            gameManager.AddKey(1); // Add key when colliding with Key
            Destroy(other.gameObject); // Destroy the key
        }

        if (other.gameObject.CompareTag("FallZone"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
            gameManager.setScore(LevelScore);
            gameManager.setKeyScore(10);
        }
    }

    private bool IsDeflecting()
    {
        // Replace with your deflect/blocking logic, e.g., check if the player is attacking
        return Input.GetKey(KeyCode.Space); // Example: Deflect when holding Space
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage! Current health: {currentHealth}");
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // Update health bar
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // Update health bar
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.setScore(LevelScore);
        gameManager.setKeyScore(10);
    }
}
