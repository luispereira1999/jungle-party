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

    private Level4Controller level4Controller;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        level4Controller = FindObjectOfType<Level4Controller>();
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

    void UpdateMovement(float horizontalInput, float verticalInput)
    {
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }

    void UpdateDirection(float horizontalInput, float verticalInput)
    {
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void Kick()
    {
        Debug.Log("Kick()");
    }

    void PickUpAndThrow()
    {
        Debug.Log("PickUpAndThrow()");
    }

    void CarryMove()
    {
        Debug.Log("Kick()");
    }

    void Throw()
    {
        Debug.Log("Throw()");
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsTouchingGround()
    {
        return Physics.Raycast(transform.position, -Vector3.up, groundDistance, groundLayer);
    }

    /*
     * Trata das colisões que acontecem em cada jogador.
    */
    void OnCollisionEnter(Collision collision)
    {
        // se houver colisão com alguma power up
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);
        }

        string tag = GetOppositePlayerTag();
       
        // se houver colisão com o outro jogador
        if (collision.gameObject.CompareTag(tag))
        {
            // uma vez que existem 2 objetos com este script (os 2 jogadores),
            // é necessário verificar se já ocorreu a colisão num jogador,
            // para que o mesmo código não seja executado no outro jogador
            if (!level4Controller.collisionOccurred)
            {
                level4Controller.ChangePlayerTurn();
                level4Controller.AssingBomb();
                level4Controller.collisionOccurred = true;
            }
            else
            {
                level4Controller.collisionOccurred = false;
            }
        }
    }

    string GetOppositePlayerTag()
    {
        if (playerID == 1)
        {
            return "Player2";
        }
        else
        {
            return "Player1";
        }
    }
}