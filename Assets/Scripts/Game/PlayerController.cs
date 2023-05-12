using UnityEngine;
using Random = UnityEngine.Random;

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
    private readonly float _freezingTime = 3f;

    // guarda a ação de andar do jogador (é igual para todos os níveis)
    private WalkAction _walkAction;

    // guarda qual ação o jogador deve executar
    private IPlayerAction _currentAction;

    // para os efeitos das power ups no jogador
    private readonly float _effectTime = 3f;
    private float _normalSpeed;
    private float _halfSpeed;
    private float _doubleSpeed;


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

        _walkAction = gameObject.AddComponent<WalkAction>();

        _normalSpeed = _moveSpeed;
        _halfSpeed = _moveSpeed / 2;
        _doubleSpeed = _moveSpeed * 2;
    }

    /*
     * É executado uma vez por frame.
    */
    void Update()
    {
        if (!_isFrozen)
        {
            bool actionInput = GetCurrentActionInput();

            // se o jogador pressiona a tecla de ação
            if (actionInput)
            {
                if (_currentAction is KickAction)  // significa que está no nível 1
                {
                    _currentAction.Enter();
                }
                if (_currentAction is ThrowAction)  // significa que está no nível 4
                {
                    if (!_isWalking)
                    {
                        _currentAction.Enter();
                    }
                }
            }
            else
            {
                if (_currentAction is KickAction)
                {
                    _currentAction.Exit();
                }
                if (_currentAction is ThrowAction)
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
            (float horizontalInput, float verticalInput) = GetCurrentMovementInput();

            // se o jogador pressiona uma tecla de movimento
            if (horizontalInput != 0 || verticalInput != 0)
            {
                UpdateMovement(horizontalInput, verticalInput);
                UpdateDirection(horizontalInput, verticalInput);

                if (!_isWalking)
                {
                    _walkAction.Enter();
                    _isWalking = true;
                }
            }
            else
            {
                if (_isWalking)
                {
                    _walkAction.Exit();
                    _isWalking = false;
                }
            }
        }
        else
        {
            _walkAction.Exit();
        }
    }

    /*
     * É executado quando existe alguma colisão do jogador com outro objeto.
    */
    void OnCollisionEnter(Collision collision)
    {
        // colisão com alguma power up - destroi a power up e aplica o efeito ao jogador
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);

            int value = GenerateEffect();
            ApplyEffect(value);
        }

        string oppositePlayerTag = GetOppositePlayer().tag;

        // colisão com o outro jogador
        if (collision.gameObject.CompareTag(oppositePlayerTag))
        {
            if (_currentAction is ThrowAction)
            {
                _currentAction.Collide(collision);
            }
        }

        if (_currentAction is KickAction)
        {
            // colisão com a bola
            if (collision.gameObject.CompareTag("Ball"))
            {
                _currentAction.Collide(collision);

                bool actionInput = GetCurrentActionInput();

                // se o jogador pressiona a tecla de ação
                if (actionInput)
                {
                    if (_currentAction is KickAction)  // significa que está no nível 1
                    {
                        _currentAction.Enter();
                    }
                }
            }
        }
    }

    /*
     * É executado quando o jogador colide com algum objeto que tem o "isTrigger" ativado no seu colisor.
    */
    void OnTriggerEnter(Collider collider)
    {
        // colisão com alguma parede da arena - impede que o jogador saia da arena
        if (collider.CompareTag("Wall"))
        {
            // atualiza a posição do jogador para entrar novamente na arena
            Vector3 oppositeDirection = transform.position - collider.ClosestPoint(transform.position);
            transform.position += oppositeDirection.normalized * 0.11f;
        }
    }


    /* MÉTODOS DO PLAYERCONTROLLER */

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
        Invoke(nameof(Unfreeze), freezingTime);
    }

    public void Unfreeze()
    {
        _isFrozen = false;
    }

    int GenerateEffect()
    {
        return Random.Range(1, 4);
    }

    void ApplyEffect(int value)
    {
        switch (value)
        {
            case (int)PowerUpEffect.SPEED:
                _moveSpeed = _doubleSpeed;
                Invoke(nameof(SetNormalSpeed), _effectTime);
                break;

            case (int)PowerUpEffect.SLOW:
                _moveSpeed = _halfSpeed;
                Invoke(nameof(SetNormalSpeed), _effectTime);
                break;

            case (int)PowerUpEffect.STUN:
                _isWalking = false;
                Freeze(_freezingTime);
                break;
        }
    }

    public void SetNormalSpeed()
    {
        _moveSpeed = _normalSpeed;
    }
}