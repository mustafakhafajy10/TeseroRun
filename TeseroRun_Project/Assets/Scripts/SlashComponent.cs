using UnityEngine;

public class SlashComponent : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int damage = 10; // Damage dealt by the slash
    public float moveSpeed = 5f; // Speed at which the slash moves
    public float lifetime = 0.5f; // How long the slash exists before disappearing

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, lifetime); // Automatically destroy the slash after its lifetime
    }

    private void Update()
    {
        // Move the slash in the correct direction
        Vector3 direction = new Vector3(1, 0, 0);
        if (spriteRenderer.flipX) // Flip direction if sprite is flipped
        {
            direction = direction * -1.0f;
        }
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Slash hit: {other.gameObject.name}");

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit by slash!");

            // Check if the object has a SlimeMonster script
            SlimeMonster slimeEnemy = other.GetComponent<SlimeMonster>();
            if (slimeEnemy != null)
            {
                Debug.Log($"Applying damage to SlimeMonster: {damage}");
                slimeEnemy.TakeDamage(damage);
                return; // Exit after applying damage to avoid unnecessary checks
            }

            // If neither script is found
            Debug.Log("No valid enemy script found on this object!");
        }
    }
}
