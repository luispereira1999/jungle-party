using UnityEngine;


/// <summary>
/// Trata da a��o do jogador de lan�ar a bomba no n�vel 4.
/// </summary>
public class ThrowLvl2Action : MonoBehaviour, IPlayerAction
{
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



    }

    public void onTriggerCollision(Collider collider)
    {

        AppleController appleController = collider.gameObject.GetComponent<AppleController>();

        appleController.SetPlayer(_player.gameObject);
        appleController.SetPlayerAsParent(_player.gameObject);
        appleController.SetLocalPosition(new Vector3(0.042f, 0.39f, 0.352f));
        appleController.SetLocalRotation(Quaternion.Euler(-90, 0, 0));
        
    }
}