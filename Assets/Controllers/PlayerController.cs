using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // variável para identificar o jogador
    public int playerID;

    // variáveis para o movimento e velocidade do personagem
    public float moveSpeed = 4.25f;
    public float jumpForce = 4.25f;
    private Rigidbody rb;

    // variáveis para os controlos de cada jogador
    public KeyCode left;
    public KeyCode right;
    public KeyCode top;
    public KeyCode bottom;
    public KeyCode action;

    // variáveis para verificar se o personagem está no chão
    public float groundDistance = 0.1f;
    public LayerMask groundLayer;

    // variáveis para as animações
    private Animator animator;
    private bool isWalking = false;

    // start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // update is called once per frame
    void Update()
    {
        bool isDoingAction = Input.GetKeyDown(action);

        // se o jogador pressiona uma tecla de saltar
        if (isDoingAction && isTouchingGround())
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
        float horizontalInput = Input.GetAxis("Horizontal" + playerID);
        float verticalInput = Input.GetAxis("Vertical" + playerID);

        // se o jogador pressiona uma tecla de movimento
        if (horizontalInput != 0 || verticalInput != 0)
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

        // se houver colisão com alguma bomba
        if (collision.gameObject.tag == "Bomb")
        {
            // TODO
        }
    }
}