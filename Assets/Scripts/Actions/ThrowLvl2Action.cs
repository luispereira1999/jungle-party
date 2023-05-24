using UnityEngine;


/// <summary>
/// Trata da ação do jogador de lançar a bomba no nível 4.
/// </summary>
public class ThrowLvl2Action : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // referência ao nível 4
    private Level2Controller _level2;

    // referência ao controlador do jogador
    private PlayerController _player;

    // para controlar as animações
    private Animator _animator;


    /* PROPRIEDADES PÚBLICAS */

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


    /* MÉTODOS */

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