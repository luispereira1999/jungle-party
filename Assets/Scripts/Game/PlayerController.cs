using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// Armazena dados sobre cada personagem criada, no in�cio do jogo.
/// E controla os movimentos, a��es e caracter�sticas do mesmo.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para identifica��o do jogador
    [SerializeField] private int _playerID;

    // vari�veis para a f�sica (movimento e velocidade do personagem)
    private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    // para controlar as anima��es
    private Animator _animator;
    private bool _isWalking = false;

    // para verificar se o personagem est� a pisar no ch�o
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDistance;

    // para verificar se o personagem est� congelado
    private bool _isFrozen = false;
    private readonly float _freezingTime = 3f;

    // guarda a a��o de andar do jogador (� igual para todos os n�veis)
    private WalkAction _walkAction;

    // guarda qual a��o o jogador deve executar
    private IPlayerAction _currentAction;

    // para os efeitos das power ups no jogador
    private readonly float _effectTime = 3f;
    private float _normalSpeed;
    private float _halfSpeed;
    private float _doubleSpeed;

    [SerializeField] private AudioSource _stepsAudioSource;
    [SerializeField] private AudioClip[] _stepsAudioClip;

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

    /// <summary>
    /// � executado antes da primeira frame.
    /// </summary>
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _walkAction = gameObject.AddComponent<WalkAction>();

        _normalSpeed = _moveSpeed;
        _halfSpeed = _moveSpeed / 2;
        _doubleSpeed = _moveSpeed * 2;
    }

    /// <summary>
    /// � executado uma vez por frame.
    /// </summary>
    void Update()
    {
        if (!_isFrozen)
        {
            bool actionInput = GetCurrentActionInput();

            // se o jogador pressiona a tecla de a��o
            if (actionInput)
            {
                if (_currentAction is KickAction)  // significa que est� no n�vel 1
                {
                    _currentAction.Enter();
                }
                if (_currentAction is ThrowLvl2Action)  // significa que est� no n�vel 2
                {
                    _currentAction.Enter();
                }
                if (_currentAction is ThrowLvl4Action)  // significa que est� no n�vel 4
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
                if (_currentAction is CarryAction)
                {
                    _currentAction.Exit();
                }
                if (_currentAction is ThrowLvl4Action)
                {
                    _currentAction.Exit();
                }
            }
        }
    }

    /// <summary>
    /// � executado em intervalos fixos.
    /// </summary>
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

    /// <summary>
    /// � executado quando existe alguma colis�o do jogador com outro objeto.
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        // colis�o com alguma power up - destroi a power up e aplica o efeito ao jogador
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            Destroy(collision.gameObject);

            int value = GenerateEffect();
            ApplyEffect(value);
        }

        string oppositePlayerTag = GetOppositePlayer().tag;

        // colis�o com o outro jogador
        if (collision.gameObject.CompareTag(oppositePlayerTag))
        {
            if (_currentAction is CarryAction)
            {
                _currentAction.Collide(collision);
            }

            if (_currentAction is ThrowLvl4Action)
            {
                _currentAction.Collide(collision);
            }
        }

        if (_currentAction is KickAction)  // significa que est� no n�vel 1
        {
            // colis�o com a bola
            if (collision.gameObject.CompareTag("Ball"))
            {
                _currentAction.Collide(collision);

                bool actionInput = GetCurrentActionInput();

                // se o jogador pressiona a tecla de a��o
                if (actionInput)
                {
                    _currentAction.Enter();
                }
            }
        }
    }

    /// <summary>
    /// � executado quando colide com algum objeto e entra no seu colisor.
    /// </summary>
    void OnCollisionStay(Collision collision)
    {
        // colis�o com alguma parede da arena - impede que o jogador saia da arena
        if (collision.gameObject.CompareTag("Wall"))
        {
            // atualiza a posi��o do jogador para entrar novamente na arena
            Vector3 oppositeDirection = transform.position - collision.collider.ClosestPoint(transform.position);
            transform.position += oppositeDirection.normalized * 0.12f;
        }
    }

    /// <summary>
    /// � executado quando o jogador colide com algum objeto que tem o "isTrigger" ativado no seu colisor.
    /// </summary>
    void OnTriggerEnter(Collider collider)
    {
        // colis�o com alguma parede da arena - para saber que o jogador saiu da arena
        if (collider.CompareTag("Wall"))
        {
            if (_currentAction is CarryAction)
            {
                _currentAction.Trigger(collider);
            }
        }

        // colis�o com alguma ma��
        if (collider.gameObject.CompareTag("Apple"))
        {
            if (_currentAction is ThrowLvl2Action)
            {
                _currentAction.Trigger(collider);
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

    /// <param name="freezingTime">Tempo de congelamento. "-1" congela para sempre.</param>
    public void Freeze(float freezingTime)
    {
        _isFrozen = true;

        if (freezingTime != -1)
        {
            Invoke(nameof(Unfreeze), freezingTime);
        }
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

    private void Footstep()
    {
        _stepsAudioSource.PlayOneShot(_stepsAudioClip[Random.Range(0, _stepsAudioClip.Length)]);
    }
}