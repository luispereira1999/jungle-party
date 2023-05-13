using UnityEngine;


/// <summary>
/// Trata da a��o do jogador de empurrar no n�vel 3.
/// </summary>
public class CarryAction : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // refer�ncia ao n�vel 1
    private Level3Controller _level3;

    // refer�ncia ao controlador do jogador
    private PlayerController _player;

    // para controlar as anima��es
    private Animator _animator;

    // para a for�a que o jogador � empurrado 
    private float _force = 5f;


    /* PROPRIEDADES P�BLICAS */

    public MonoBehaviour Level
    {
        get { return _level3; }
        set { _level3 = (Level3Controller)value; }
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
    }

    public void Exit()
    {

    }

    public void Collide(Collision collision)
    {
        PushPlayer();
    }

    public void PushPlayer()
    {

    }
}