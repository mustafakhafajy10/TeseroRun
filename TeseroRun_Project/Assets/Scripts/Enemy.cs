using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject healthBarPrefab;
    private GameObject healthBar;
    private Image healthBarFill;

    void Start()
    {
        currentHealth = maxHealth;

        // Instantiate the health bar above the enemy
        healthBar = Instantiate(healthBarPrefab);
        healthBarFill = healthBar.transform.Find("Background/Fill").GetComponent<Image>();

        if (healthBar == null || healthBarFill == null)
        {
            Debug.LogError("Health Bar or Fill component could not be found.");
            return;
        }
    }

    void Update()
    {
        // Keep the health bar above the enemy
        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + new Vector3(0, 1.5f, 0); // Adjust height if necessary
        }
    }

    // Method to take damage
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Enemy took damage! Current Health: " + currentHealth);

        // Update health bar
        UpdateHealthBar();

        // Check if health is zero or below
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to update health bar fill amount and color based on health
    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
            UpdateHealthBarColor();
        }
    }

    // Method to change health bar color based on remaining health
    private void UpdateHealthBarColor()
    {
        if (healthBarFill == null) return;

        float healthPercent = (float)currentHealth / maxHealth;
        healthBarFill.color = healthPercent > 0.5f ? Color.green : (healthPercent > 0.2f ? Color.yellow : Color.red);
    }

    // Method to handle enemy death
    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
        if (healthBar != null)
        {
            Destroy(healthBar);
        }
    }
}
