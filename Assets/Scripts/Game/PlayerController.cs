using UnityEngine;

/*
 * Armazena dados sobre cada personagem criada, no início do jogo.
 * E controla os movimentos, ações e características do mesmo.
*/
public class PlayerController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // variável para identificação do jogador
    [SerializeField] private int _playerID;

    // variáveis para a física (movimento e velocidade do personagem)
    private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    // para controlar as animações
    private Animator _animator;
    private bool _isWalking = false;

    // para verificar se o personagem está a pisar no chão
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDistance;

    // para verificar se o personagem está congelado
    private bool _isFrozen = false;
    private float _freezingTime = 3f;

    // guarda qual ação o jogador deve executar
    private IPlayerAction _currentAction;


    /* PROPRIEDADES PÚBLICAS */

    public int PlayerID
    {
        get { return _playerID; }
        set { _playerID = value; }
    }

    public bool IsWalking
    {
        get { return _isWalking; }
        set { _isWalking = value; }
    }


    /* MÉTODOS DO MONOBEHAVIOUR */

    /*
     * É executado antes da primeira frame.
    */
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    /*
     * É executado uma vez por frame.
    */
    void Update()
    {
        if (!_isFrozen)
        {
            bool actionInput = Input.GetButtonDown("Action" + _playerID);

            if (actionInput)
            {
                if (_currentAction is KickController)  // nível 1
                {
                    _currentAction.Enter();
                }
                if (_currentAction is ThrowController)  // nível 4
                {
                    if (!_isWalking)
                    {
                        _currentAction.Enter();
                    }
                }
            }
            else
            {
                if (_currentAction is KickController)  // nível 1
                {
                    _currentAction.Exit();
                }
                if (_currentAction is ThrowController)  // nível 4
                {
                    _currentAction.Exit();
                }
            }
        }
    }

    /*
     * É executado em intervalos fixos.
    */
    void FixedUpdate()
    {
        if (!_isFrozen)
        {
            float horizontalInput = Input.GetAxis("Horizontal" + _playerID);
            float verticalInput = Input.GetAxis("Vertical" + _playerID);

            // se o jogador pressiona uma tecla de movimento
            if (horizontalInput != 0 || verticalInput != 0)
            {
                UpdateMovement(horizontalInput, verticalInput);
                UpdateDirection(horizontalInput, verticalInput);

                if (!_isWalking)
                {
                    _animator.SetBool("isWalking", true);
                    _isWalking = true;
                }
            }
            else
            {
                if (_isWalking)
                {
                    _animator.SetBool("isWalking", false);
                    _isWalking = false;
                }
            }
        }
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

        string oppositePlayerTag = GetOppositePlayer().tag;

        // se houver colisão com o outro jogador
        if (collision.gameObject.CompareTag(oppositePlayerTag))
        {
            if (_currentAction is ThrowController)
            {
                _currentAction.Collide(collision);
            }
        }
    }


    /* MÉTODOS DO PLAYERCONTROLLER */

    public void SetAction(IPlayerAction action, MonoBehaviour level)
    {
        _currentAction = action;
        _currentAction.Level = level;
        _currentAction.Player = this;
    }

    void UpdateMovement(float horizontalInput, float verticalInput)
    {
        Vector3 movement = _moveSpeed * Time.fixedDeltaTime * new Vector3(horizontalInput, 0f, verticalInput);
        _rigidbody.MovePosition(transform.position + movement);
    }

    void UpdateDirection(float horizontalInput, float verticalInput)
    {
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public GameObject GetCurrentPlayer()
    {
        return this.gameObject;
    }

    public GameObject GetOppositePlayer()
    {
        if (_playerID == 1)
        {
            GameObject[] oppositePlayer = GameObject.FindGameObjectsWithTag("Player2");
            return oppositePlayer[0];
        }
        else
        {
            GameObject[] oppositePlayer = GameObject.FindGameObjectsWithTag("Player1");
            return oppositePlayer[0];
        }
    }

    public float GetFreezingTime()
    {
        return _freezingTime;
    }

    public void Freeze(float freezingTime)
    {
        _isFrozen = true;
        Invoke("Unfreeze", freezingTime);
    }

    public void Unfreeze()
    {
        _isFrozen = false;
    }
}