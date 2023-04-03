using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // variáveis para o movimento e velocidade do personagem
    public float moveSpeed = 4f;
    public float jumpForce = 4f;
    private Rigidbody rb;

    // variáveis para verificar se o personagem está no chão
    public float groundDistance = 0.1f;
    public LayerMask groundLayer;

    // variáveis para as animações
    Animator animator;
    bool isWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // se o jogador pressiona uma tecla de saltar
        if (Input.GetButtonDown("Jump") && isTouchingGround())
        {
            jump();
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // se o jogador pressiona uma tecla de movimento
        if (horizontalInput != 0f || verticalInput != 0f)
        {
            updateMovement(horizontalInput, verticalInput);
            updateDirection(horizontalInput, verticalInput);

            if (!isWalking)
            {
                animator.SetBool("isWalking", true);
                isWalking = true;
            }
        }
        else
        {
            if (isWalking)
            {
                animator.SetBool("isWalking", false);
                isWalking = false;
            }
        }
    }

    void updateMovement(float horizontalInput, float verticalInput)
    {
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }

    void updateDirection(float horizontalInput, float verticalInput)
    {
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool isTouchingGround()
    {
        return Physics.Raycast(transform.position, -Vector3.up, groundDistance, groundLayer);
    }

    void OnCollisionEnter(Collision collision)
    {
        // se houver colisão com alguma power up
        if (collision.gameObject.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
        }
    }
}