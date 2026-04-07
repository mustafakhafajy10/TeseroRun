using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAbilities : MonoBehaviour
{
    public GameObject abilitiesPanel; // Assign the abilities UI panel in the Inspector
    public Button speedBoostButton;
    public Button jumpBoostButton;
    public Button damageBoostButton;
    public Button healButton;

    public float speedBoostMultiplier = 1.5f;
    public float jumpBoostMultiplier = 1.5f;
    public int damageBoostAmount = 10;
    public int healAmount = 20;

    public float abilityDuration = 5f; // Duration of boosts

    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private GameManager gameManager;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        gameManager = FindFirstObjectByType<GameManager>(); // Reference to GameManager

        // Attach click events to buttons
        if (speedBoostButton != null)
        {
            speedBoostButton.onClick.AddListener(ActivateSpeedBoost);
        }

        if (jumpBoostButton != null)
        {
            jumpBoostButton.onClick.AddListener(ActivateJumpBoost);
        }

        if (damageBoostButton != null)
        {
            damageBoostButton.onClick.AddListener(ActivateDamageBoost);
        }

        if (healButton != null)
        {
            healButton.onClick.AddListener(ActivateHeal);
        }

        // Initially hide the abilities panel
        if (abilitiesPanel != null)
        {
            abilitiesPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for key inputs to activate abilities
        if (Input.GetKeyDown(KeyCode.Q)) // Q for Speed Boost
        {
            ActivateSpeedBoost();
        }
        if (Input.GetKeyDown(KeyCode.F)) // F for Jump Boost
        {
            ActivateJumpBoost();
        }
        if (Input.GetKeyDown(KeyCode.R)) // R for Damage Boost
        {
            ActivateDamageBoost();
        }
        if (Input.GetKeyDown(KeyCode.Z)) // Z for Heal
        {
            ActivateHeal();
        }
    }

    public void EnableAbilities(bool enable)
    {
        if (abilitiesPanel != null)
        {
            abilitiesPanel.SetActive(enable);
        }
        Debug.Log($"Abilities panel set to {(enable ? "visible" : "hidden")}");
    }

    private void ActivateSpeedBoost()
    {
        if (gameManager.GetScore() >= 5)
        {
            gameManager.removePoints(5); // Deduct coins using GameManager
            StartCoroutine(SpeedBoostCoroutine());
            Debug.Log("Speed Boost Activated!");
        }
        else
        {
            Debug.Log("Not enough coins for Speed Boost!");
        }
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        float originalSpeed = playerMovement.moveSpeed;
        playerMovement.moveSpeed *= speedBoostMultiplier;
        yield return new WaitForSeconds(abilityDuration);
        playerMovement.moveSpeed = originalSpeed;
    }

    private void ActivateJumpBoost()
    {
        if (gameManager.GetScore() >= 5)
        {
            gameManager.removePoints(5); // Deduct coins using GameManager
            StartCoroutine(JumpBoostCoroutine());
            Debug.Log("Jump Boost Activated!");
        }
        else
        {
            Debug.Log("Not enough coins for Jump Boost!");
        }
    }

    private IEnumerator JumpBoostCoroutine()
    {
        float originalJumpForce = playerMovement.jumpForce;
        playerMovement.jumpForce *= jumpBoostMultiplier;
        yield return new WaitForSeconds(abilityDuration);
        playerMovement.jumpForce = originalJumpForce;
    }

    private void ActivateDamageBoost()
    {
        if (gameManager.GetScore() >= 10)
        {
            gameManager.removePoints(10); // Deduct coins using GameManager
            StartCoroutine(DamageBoostCoroutine());
            Debug.Log("Damage Boost Activated!");
        }
        else
        {
            Debug.Log("Not enough coins for Damage Boost!");
        }
    }

    private IEnumerator DamageBoostCoroutine()
    {
        int originalDamage = playerAttack.knifeDamage;
        playerAttack.knifeDamage += damageBoostAmount;
        yield return new WaitForSeconds(abilityDuration);
        playerAttack.knifeDamage = originalDamage;
    }

    private void ActivateHeal()
    {
        if (gameManager.GetScore() >= 10)
        {
            if (playerMovement.currentHealth < playerMovement.maxHealth)
            {
                gameManager.removePoints(10); // Deduct coins using GameManager
                playerMovement.Heal(healAmount); // Heal the player
                Debug.Log("Player healed for " + healAmount + " health!");
            }
            else
            {
                Debug.Log("Player is already at max health!");
            }
        }
        else
        {
            Debug.Log("Not enough coins for Healing Boost!");
        }
    }
}
