using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float pointA = -3f;   // Offset for the starting X position relative to the initial position
    public float pointB = 3f;    // Offset for the ending X position relative to the initial position
    public float speed = 2f;     // Speed of movement

    private Vector2 startPosition;
    private Vector2 target;

    void Start()
    {
        // Save the initial position
        startPosition = transform.position;

        // Set initial target as pointB offset from the start position
        target = new Vector2(startPosition.x + pointB, startPosition.y);
    }

    void Update()
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
