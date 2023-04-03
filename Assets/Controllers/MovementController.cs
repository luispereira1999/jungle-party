using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 4f;
    private Rigidbody rigidbody;

    public float groundDistance = 0.1f;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // quando o jogador pressiona uma tecla de saltar
        if (Input.GetButtonDown("Jump") && isTouchingGround())
        {
            jump();
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // quando o jogador pressiona uma tecla de movimento
        if (horizontalInput != 0f || verticalInput != 0f)
        {
            updateMovement(horizontalInput, verticalInput);
            updateDirection(horizontalInput, verticalInput);
        }
    }

    void updateMovement(float horizontalInput, float verticalInput)
    {
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(transform.position + movement);
    }

    void updateDirection(float horizontalInput, float verticalInput)
    {
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void jump()
    {
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool isTouchingGround()
    {
        return Physics.Raycast(transform.position, -Vector3.up, groundDistance, groundLayer);
    }
}