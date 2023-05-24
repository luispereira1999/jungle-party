using UnityEngine;


/// <summary>
/// Trata da a��o do jogador de lan�ar a bomba no n�vel 4.
/// </summary>
public class ThrowLvl4Action : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // refer�ncia ao n�vel 4
    private Level4Controller _level4;

    // refer�ncia ao controlador do jogador
    private PlayerController _player;

    // para controlar as anima��es
    private Animator _animator;


    /* PROPRIEDADES P�BLICAS */

    public MonoBehaviour Level
    {
        get { return _level4; }
        set { _level4 = (Level4Controller)value; }
    }

    public PlayerController Player
    {
        get { return _player; }
        set { _player = value; }
    }

    public Animator Animator
    {
        get { return _animator; }
        set { _animator = value; }
    }


    /* M�TODOS */

    public void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Enter()
    {
        // nada para ser implementado
    }

    public void Exit()
    {
        // nada para ser implementado
    }

    public void ApplyPhisics()
    {
        // nada para ser implementado
    }

    public void Collide(Collision collision)
    {
        // uma vez que existem 2 objetos com este script (os 2 jogadores),
        // � necess�rio verificar se j� ocorreu a colis�o num jogador,
        // para que o mesmo c�digo n�o seja executado no outro jogador
        if (_level4.CollisionOccurred)
        {
            _level4.CollisionOccurred = false;
            return;
        }

        string currentPlayerTag = _player.GetCurrentPlayer().tag;

        if (!_level4.GetPlayerWithBomb().CompareTag(currentPlayerTag))
        {
            _level4.ChangePlayerTurn();
            _level4.AssignBomb();
            _level4.CollisionOccurred = true;

            _player.Freeze(_player.GetFreezingTime());

            _animator.SetBool("isWalking", false);
            _player.IsWalking = false;
        }
    }

    public void Trigger(Collider collider)
    {
        // nada para ser implementado 
    }
}