using UnityEngine;
using System.Collections;

public class FloorTrap : MonoBehaviour
{
    private Animator animator;
    private bool canDamage = true; // Allows damage to be reapplied
    public int damageAmount = 20; // Damage to the player

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            ActivateTrap(other);
        }
    }

    private void ActivateTrap(Collider2D player)
    {
        canDamage = false; // Prevent multiple damages during one activation
        animator.SetTrigger("Activate"); // Trigger the pop-up animation

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.TakeDamage(damageAmount); // Apply damage
        }

        StartCoroutine(ResetTrap());
    }

    private IEnumerator ResetTrap()
    {
        yield return new WaitForSeconds(2f); // Time to reset the trap (adjust as needed)
        animator.SetTrigger("Reset"); // Trigger reset animation (if any)
        canDamage = true; // Allow damage again
    }
}
