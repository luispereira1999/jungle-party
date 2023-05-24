using UnityEngine;


/// <summary>
/// Trata da a��o de andar do jogador.
/// </summary>
public class WalkAction : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // refer�ncia ao n�vel atual
    private MonoBehaviour _level;

    // refer�ncia ao controlador do jogador
    private PlayerController _player;

    // para controlar as anima��es
    private Animator _animator;


    /* PROPRIEDADES P�BLICAS */

    public MonoBehaviour Level
    {
        get { return _level; }
        set { _level = value; }
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
        _animator.SetBool("isWalking", true);
    }

    public void Exit()
    {
        _animator.SetBool("isWalking", false);
    }

    public void ApplyPhisics()
    {
        // nada para ser implementado
    }

    public void Collide(Collision collision)
    {
        // nada para ser implementado
    }

    public void Trigger(Collider collider)
    {
        // nada para ser implementado 
    }
}