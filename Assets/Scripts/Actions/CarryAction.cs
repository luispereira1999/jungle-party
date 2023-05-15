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
    private float _pushForce = 10f;

    // para saber se a jogador empurrou e tocou no outro jogador
    private bool carrying = false;
    private bool colliding = false;


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
        carrying = true;

        //if (carryingAndColliding)
        //{
        //    _animator.SetBool("isCarryingMove", true);
        //    carryingAndColliding = false;
        //}
    }

    public void Exit()
    {
        _animator.SetBool("isCarryingMove", false);
    }

    public void Collide(Collision collision)
    {
        Debug.Log("C");

        if (carrying && !colliding)
        {
            Debug.Log("D");
            //carrying = false;
        }

        //if (carryingAndColliding)
        //{
        //    Debug.Log("C");
        //    _animator.SetBool("isWalking", false);
        //    //Enter();
        //    PushPlayer(collision);
        //    carryingAndColliding = false;
        //}
    }

    public void PushPlayer(Collision collision)
    {
        //string currentPlayerTag = _player.GetCurrentPlayer().tag;

        //Vector3 direction = collision.contacts[0].point - transform.position;

        //direction = direction.normalized;

        //Rigidbody rigidbody = _player.GetComponent<Rigidbody>();

        //rigidbody.AddForce(direction * _pushForce, ForceMode.Impulse);
    }

    public void Collide()
    {
        if (carrying )
        {
            Debug.Log("B");

            // Calcula a dire��o oposta ao olhar atual do jogador
            Vector3 direcaoEmpurrao = transform.forward;

            GameObject opposite = _player.GetOppositePlayer();

            // Aplica o empurr�o ao Rigidbody do jogador
            opposite.GetComponent<Rigidbody>().AddForce(direcaoEmpurrao * 6f, ForceMode.Impulse);

            // Reseta a vari�vel da tecla pressionada
            carrying = false;
            colliding = false;
        }
    }
}