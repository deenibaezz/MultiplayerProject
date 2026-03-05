using UnityEngine;

public class PlayerMovementLocal : MonoBehaviour
{
    public enum ControlScheme { WASD, Arrows }

    [SerializeField] private ControlScheme controls;
    [SerializeField] private float speed = 6f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (controls == ControlScheme.WASD)
        {
            input.x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
            input.y = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        }
        else
        {
            input.x = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
            input.y = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
        }

        rb.linearVelocity = input.normalized * speed;
    }
}