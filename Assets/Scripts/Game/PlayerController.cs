using UnityEngine;

/*
 * Armazena dados sobre cada personagem criada, no in�cio do jogo.
 * E controla os movimentos, a��es e caracter�sticas do mesmo.
*/
public class PlayerController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para identifica��o do jogador
    [SerializeField] private int _playerID;

    // vari�veis para a f�sica (movimento e velocidade do personagem)
    private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    private float _halfSpeed;
    private float _doubleSpeed;
    private float _speed;

    // para controlar as anima��es
    private Animator _animator;
    private bool _isWalking = false;

    // para verificar se o personagem est� a pisar no ch�o
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDistance;

    // para o colisor do mapa onde o jogador pode andar


    // para verificar se o personagem est� congelado
    private bool _isFrozen = false;
    private float _freezingTime = 3f;

    // guarda qual a��o o jogador deve executar
    private IPlayerAction _currentAction;


    /* PROPRIEDADES P�BLICAS */

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


    /* M�TODOS DO MONOBEHAVIOUR */

    /*
     * � executado antes da primeira frame.
    */
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _speed = _moveSpeed;
        _halfSpeed = _moveSpeed / 2;
        _doubleSpeed = _moveSpeed * 2;
    }

    /*
     * � executado uma vez por frame.
    */
    void Update()
    {
        if (!_isFrozen)
        {
            bool actionInput = GetCurrentActionInput();

            if (actionInput)
            {
                if (_currentAction is KickController)  // n�vel 1
                {
                    _currentAction.Enter();
                }
                if (_currentAction is ThrowController)  // n�vel 4
                {
                    if (!_isWalking)
                    {
                        _currentAction.Enter();
                    }
                }
            }
            else
            {
                if (_currentAction is KickController)  // quando � o n�vel 1
                {
                    _currentAction.Exit();
                }
                if (_currentAction is ThrowController)  // quando � o n�vel 4
                {
                    _currentAction.Exit();
                }
            }
        }
    }

    /*
     * � executado em intervalos fixos.
    */
    void FixedUpdate()
    {
        if (!_isFrozen)
        {
            (float horizontal, float vertical) movementInputs = GetCurrentMovementInput();

            // se o jogador pressiona uma tecla de movimento
            if (movementInputs.horizontal != 0 || movementInputs.vertical != 0)
            {
                UpdateMovement(movementInputs.horizontal, movementInputs.vertical);
                UpdateDirection(movementInputs.horizontal, movementInputs.vertical);

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
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }

    /*
     * Trata das colis�es que acontecem em cada jogador.
    */
    void OnCollisionEnter(Collision collision)
    {
        // se houver colis�o com alguma parede
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Colidiu com parede");
        }

        // se houver colis�o com alguma power up
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            System.Random rnd = new System.Random();
            int value = rnd.Next(3);

            Destroy(collision.gameObject);

            switch (value)
            {
                case (int)PowerUpAction.SPEED:
                    _speed = _doubleSpeed;
                    Invoke("NormalSpeed", _freezingTime);
                    break;
                case (int)PowerUpAction.SLOW:
                    _speed = _halfSpeed;
                    Invoke("NormalSpeed", _freezingTime);
                    break;
                case (int)PowerUpAction.STUN:
                    _isWalking = false;
                    Freeze(_freezingTime);
                    break;
            }

        }

        string oppositePlayerTag = GetOppositePlayer().tag;

        // se houver colis�o com o outro jogador
        if (collision.gameObject.CompareTag(oppositePlayerTag))
        {
            if (_currentAction is ThrowController)
            {
                _currentAction.Collide(collision);
            }
        }

        if (_currentAction is KickController)
        {
            // se houver colis�o com a bola
            if (collision.gameObject.CompareTag("Ball"))
            {
                _currentAction.Collide(collision);
            }
        }
    }


    /* M�TODOS DO PLAYERCONTROLLER */

    public void SetAction(IPlayerAction action, MonoBehaviour level)
    {
        _currentAction = action;
        _currentAction.Level = level;
        _currentAction.Player = this;
    }

    public bool GetCurrentActionInput()
    {
        bool action = Input.GetButtonDown("Action" + _playerID);
        return action;
    }

    public (float, float) GetCurrentMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal" + _playerID);
        float vertical = Input.GetAxis("Vertical" + _playerID);
        return (horizontal, vertical);
    }

    void UpdateMovement(float horizontalInput, float verticalInput)
    {
        Vector3 movement = _speed * Time.fixedDeltaTime * new Vector3(horizontalInput, 0f, verticalInput);
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

    public void NormalSpeed()
    {
        _speed = _moveSpeed;
    }
}