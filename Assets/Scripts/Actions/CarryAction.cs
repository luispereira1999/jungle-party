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
    private float _pushForce = 5f;


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
        _animator.SetBool("isCarryingMove", true);
    }

    public void Exit()
    {
        _animator.SetBool("isCarryingMove", false);
    }

    public void Collide(Collision collision)
    {
        PushPlayer(collision);
    }

    public void PushPlayer(Collision collision)
    {
        // dire��o oposta de onde sofreu a colis�o do outro jogador
        Vector3 direction = transform.position - collision.transform.position;
        direction = direction.normalized;

        // aplica a for�a
        Player.GetComponent<Rigidbody>().AddForce(direction * _pushForce, ForceMode.Impulse);
    }

    public void Trigger(Collider collider)
    {
        _level3.OutOfArena = true;
        GameObject loser = Player.gameObject;

        if (loser.tag == "Player1")
        {
            _level3.PlayerOutID = 1;
        }
        else if (loser.tag == "Player2")
        {
            _level3.PlayerOutID = 2;
        }
    }
}