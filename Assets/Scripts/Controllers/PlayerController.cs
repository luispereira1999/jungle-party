using UnityEngine;

/*
 * Armazenar dados sobre cada personagem criada, no início do jogo.
 * E controlar os movimentos, ações e características do mesmo.
*/
public class PlayerController : MonoBehaviour
{
    // variável para identificar o jogador
    public int playerID;

    // variáveis para o movimento e velocidade do personagem
    private Rigidbody rb;
    public float moveSpeed = 4.25f;
    public float jumpForce = 4.25f;

    // variáveis para verificar se o personagem está no chão
    public LayerMask groundLayer;
    public float groundDistance = 0.1f;

    // variáveis para as animações
    private Animator animator;
    private bool isWalking = false;

    public PlayerAction currentAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool actionInput = Input.GetButtonDown("Action" + playerID);
        
        if (actionInput)
        {
            if (currentAction == PlayerAction.KICK)
            {
                animator.SetBool("isKicking", true);
                Kick();
            }
            if (currentAction == PlayerAction.PICK_UP_AND_THROW)
            {
                animator.SetBool("isPickingUpAndThrowing", true);
                PickUpAndThrow();
            }
            if (currentAction == PlayerAction.CARRY_MOVE)
            {
                animator.SetBool("isCarryingMove", true);
                CarryMove();
            }
            if (currentAction == PlayerAction.THROW)
            {
                if (!isWalking)
                {
                    animator.SetBool("isThrowing", true);
                    Throw();
                }
            }
            if (currentAction == PlayerAction.JUMP)
            {
                if (IsTouchingGround())
                {
                    animator.SetBool("isJumping", true);
                    Jump();
                }
            }
        }
        else
        {
            if (currentAction == PlayerAction.KICK)
            {
                animator.SetBool("isKicking", false);
            }
            if (currentAction == PlayerAction.PICK_UP_AND_THROW)
            {
                animator.SetBool("isPickingUpAndThrowing", false);
            }
            if (currentAction == PlayerAction.CARRY_MOVE)
            {
                animator.SetBool("isCarryingMove", false);
            }
            if (currentAction == PlayerAction.THROW)
            {
                animator.SetBool("isThrowing", false);
            }
            if (currentAction == PlayerAction.JUMP)
            {
                if (IsTouchingGround())
                {
                    animator.SetBool("isJumping", false);
                }
            }
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal" + playerID);
        float verticalInput = Input.GetAxis("Vertical" + playerID);

        // se o jogador pressiona uma tecla de movimento
        if (horizontalInput != 0 || verticalInput != 0)
        {
            UpdateMovement(horizontalInput, verticalInput);
            UpdateDirection(horizontalInput, verticalInput);

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

    public void UpdateMovement(float horizontalInput, float verticalInput)
    {
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }

    public void UpdateDirection(float horizontalInput, float verticalInput)
    {
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void Kick()
    {
        Debug.Log("Kick()");
    }

    public void PickUpAndThrow()
    {
        Debug.Log("PickUpAndThrow()");
    }

    public void CarryMove()
    {
        Debug.Log("Kick()");
    }

    public void Throw()
    {
        Debug.Log("Throw()");
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public bool IsTouchingGround()
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