using UnityEngine;

/*
 * Controla a a��o do jogador de chutar a bola no n�vel 1.
*/
public class KickAction : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // refer�ncia ao n�vel 1
    private Level1Controller _level1;

    // refer�ncia ao controlador do jogador
    private PlayerController _player;

    // para controlar as anima��es
    private Animator _animator;

    // refer�ncia do controlador da bola
    private GameObject _ballObject;

    // para a for�a que a bola � empurrada ou chutada
    private float _force = 5f;


    /* PROPRIEDADES P�BLICAS */

    public MonoBehaviour Level
    {
        get { return _level1; }
        set { _level1 = (Level1Controller)value; }
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
        _ballObject = GameObject.FindGameObjectsWithTag("Ball")[0];
    }

    public void Enter()
    {
        _animator.SetBool("isKicking", true);
    }

    public void Exit()
    {
        _animator.SetBool("isKicking", false);
    }

    public void Collide(Collision collision)
    {
        PushBall(collision);
    }

    public void PushBall(Collision collision)
    {
        Vector3 direction = collision.contacts[0].point - transform.position;

        // garante que a dire��o ser� sempre correta, independente da for�a aplicada
        direction = direction.normalized;

        Rigidbody rigidbodyBall = _ballObject.GetComponent<Rigidbody>();
        rigidbodyBall.AddForce(direction * _force, ForceMode.Impulse);
    }
}