using Unity.Netcode;
using UnityEngine;

public class PlayerMovementLocal : NetworkBehaviour
{
    [SerializeField] private float speed = 6f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Only the local owner controls this player
        if (!IsOwner) return;

        Vector2 input = Vector2.zero;

        input.x = ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1 : 0)
        - ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? 1 : 0);

        input.y = ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? 1 : 0)
        - ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? 1 : 0);
        rb.linearVelocity = input.normalized * speed;
    }
}