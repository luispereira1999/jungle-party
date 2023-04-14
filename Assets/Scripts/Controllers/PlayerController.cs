using UnityEngine;

/*
 * Armazena dados sobre cada personagem criada, no in�cio do jogo.
 * E controla os movimentos, a��es e caracter�sticas do mesmo.
*/
public class PlayerController : MonoBehaviour
{
    // vari�vel para identifica��o do jogador
    public int playerID;

    // vari�veis para a f�sica (movimento e velocidade do personagem)
    private Rigidbody rb;
    public float moveSpeed = 4.25f;
    public float jumpForce = 4.25f;

    // para verificar se o personagem est� congelado
    private bool isFrozen = false;
    private float freezingTime = 3f;

    // para verificar se o personagem est� a pisar no ch�o
    public LayerMask groundLayer;
    public float groundDistance = 0.1f;

    // para controlar as anima��es
    public Animator animator;
    private bool isWalking = false;

    // guarda qual a��o o jogador deve executar
    public PlayerAction currentAction;

    // refer�ncia do n�vel atual
    private Level4Controller level4Controller;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        level4Controller = FindObjectOfType<Level4Controller>();
    }

    void Update()
    {
        if (!isFrozen)
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
    }

    void FixedUpdate()
    {
        if (!isFrozen)
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
    }

    void UpdateMovement(float horizontalInput, float verticalInput)
    {
        Vector3 movement = moveSpeed * Time.fixedDeltaTime * new Vector3(horizontalInput, 0f, verticalInput);
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
     * Trata das colis�es que acontecem em cada jogador.
    */
    void OnCollisionEnter(Collision collision)
    {
        // se houver colis�o com alguma power up
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);
        }

        string currentPlayerTag = GetCurrentPlayerTag();
        string oppositePlayerTag = GetOppositePlayerTag();

        // se houver colis�o com o outro jogador
        if (collision.gameObject.CompareTag(oppositePlayerTag))
        {
            // uma vez que existem 2 objetos com este script (os 2 jogadores),
            // � necess�rio verificar se j� ocorreu a colis�o num jogador,
            // para que o mesmo c�digo n�o seja executado no outro jogador
            if (!level4Controller.collisionOccurred)
            {
                if (level4Controller.GetPlayerWithBomb().tag != currentPlayerTag)
                {
                    level4Controller.ChangePlayerTurn();
                    level4Controller.AssignBomb();
                    level4Controller.collisionOccurred = true;

                    Freeze(freezingTime);

                    animator.SetBool("isWalking", false);
                    isWalking = false;
                }
            }
            else
            {
                level4Controller.collisionOccurred = false;
            }
        }
    }

    string GetCurrentPlayerTag()
    {
        return this.gameObject.tag;
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

    void Freeze(float freezingTime)
    {
        isFrozen = true;
        Invoke("Unfreeze", freezingTime);
    }

    void Unfreeze()
    {
        isFrozen = false;
    }
}