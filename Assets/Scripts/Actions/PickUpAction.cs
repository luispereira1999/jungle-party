using UnityEngine;


/// <summary>
/// Trata da ação do jogador de lançar a maça no nível 2.
/// </summary>
public class PickUpAction : MonoBehaviour, IPlayerAction
{
    // Start is called before the first frame update
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
        // nada para ser implementado
    }

    public void Trigger(Collider collider)
    {
        // nada para ser implementado 
    }
}