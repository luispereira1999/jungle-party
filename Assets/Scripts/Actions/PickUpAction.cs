using UnityEngine;


/// <summary>
/// Trata da a��o do jogador de lan�ar a ma�a no n�vel 2.
/// </summary>
public class PickUpAction : MonoBehaviour, IPlayerAction
{
    // Start is called before the first frame update
    /* ATRIBUTOS PRIVADOS */

    // refer�ncia ao n�vel 4
    private Level2Controller _level2;

    // refer�ncia ao controlador do jogador
    private PlayerController _player;

    // para controlar as anima��es
    private Animator _animator;


    /* PROPRIEDADES P�BLICAS */

    public MonoBehaviour Level
    {
        get { return _level2; }
        set { _level2 = (Level2Controller)value; }
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

    public void Collide(Collision collision)
    {
        // nada para ser implementado
    }

    public void Trigger(Collider collider)
    {
        // nada para ser implementado 
    }
}