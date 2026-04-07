using UnityEngine;
using UnityEngine.UI;

public class SlimeMonster : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject healthBarPrefab;
    private GameObject healthBar;
    private Image healthBarFill;

    public float speed = 2f;
    public float triggerDistance = 8f; // Distance at which the slime starts following the player
    public float attackRange = 1.5f; // Range for the attack
    public int attackDamage = 20;
    public float attackCooldown = 2f;
    public float damageTiming = 0.5f; // Time (in seconds) into the attack animation to apply damage

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isDead = false;
    private bool isAttacking = false;
    private bool isFollowing = false; // Whether the slime is actively following the player

    private float lastAttackTime;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = Instantiate(healthBarPrefab);
        healthBarFill = healthBar.transform.Find("Background/Fill").GetComponent<Image>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;

            // Ignore collisions between the slime and the player
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerObj.GetComponent<Collider2D>());
        }
    }

    void Update()
    {
        if (isDead) return;

        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        }

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // If the player is within the trigger distance, start following
            if (distanceToPlayer <= triggerDistance)
            {
                isFollowing = true;
            }

            // If following, handle movement and attacks
            if (isFollowing)
            {
                FollowPlayer();
            }
            else
            {
                StopMoving();
            }
        }
    }

    private void FollowPlayer()
    {
        if (isDead || isAttacking) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if the slime should attack
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            return;
        }

        // Enable running animation only if moving
        if (animator != null)
        {
            animator.SetBool("isRunning", true);
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y); // Maintain gravity on Y-axis

        // Flip the slime sprite based on the direction of movement
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction.x > 0; // Flip when moving right
        }
    }

    private void StopMoving()
    {
        rb.linearVelocity = Vector2.zero; // Stop all horizontal movement
        if (animator != null)
        {
            animator.SetBool("isRunning", false); // Stop running animation, transitions to idle
        }
    }

    private void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Stop movement during attack
        StopMoving();

        // Apply damage after a short delay (simulating damage timing during the attack animation)
        Invoke(nameof(ApplyDamage), damageTiming);

        // Reset attack state after the attack animation completes
        Invoke(nameof(ResetAttack), 1f); // Adjust duration to match your attack animation length
    }

    private void ApplyDamage()
    {
        if (player == null || isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.TakeDamage(attackDamage);
                Debug.Log("Slime damaged the player!");
            }
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Slime took damage! Current Health: {currentHealth}");

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        StopMoving();
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;

            float healthPercent = (float)currentHealth / maxHealth;
            healthBarFill.color = healthPercent > 0.5f ? Color.green : (healthPercent > 0.2f ? Color.yellow : Color.red);
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        StopMoving();
        rb.linearVelocity = Vector2.zero;

        Destroy(healthBar);
        Destroy(gameObject, 1.5f);
    }

    public GameObject getHealthBar()
    {
        return healthBar;
    }
}
