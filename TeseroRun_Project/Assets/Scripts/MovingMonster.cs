using UnityEngine;

public class MovingMonster : MonoBehaviour
{
    public float pointA = -3f;          // Offset for starting X position relative to initial position
    public float pointB = 3f;           // Offset for ending X position relative to initial position
    public float speed = 2f;            // Speed of movement
    public float followDistance = 5f;   // Distance at which the platform starts following the player

    private Vector2 startPosition;
    private Vector2 target;
    private GameObject player;

    void Start()
    {
        // Save the initial position
        startPosition = transform.position;
        target = new Vector2(startPosition.x + pointB, startPosition.y);

        // Find the player by tag (assumes the player GameObject is tagged as "Player")
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found! Make sure the player GameObject is tagged as 'Player'");
            return;
        }

        // Check distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= followDistance)
        {
            // Follow the player when they are within the follow distance
            FollowPlayer();
        }
        else
        {
            // Default back-and-forth movement
            MoveBetweenPoints();
        }
    }

    private void FollowPlayer()
    {
        // Move towards the player's position
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void MoveBetweenPoints()
    {
        // Move platform horizontally between start position + pointA and start position + pointB
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Switch target when reaching one of the points
        if (Mathf.Approximately(transform.position.x, startPosition.x + pointB))
        {
            target = new Vector2(startPosition.x + pointA, startPosition.y);
        }
        else if (Mathf.Approximately(transform.position.x, startPosition.x + pointA))
        {
            target = new Vector2(startPosition.x + pointB, startPosition.y);
        }
    }
}
