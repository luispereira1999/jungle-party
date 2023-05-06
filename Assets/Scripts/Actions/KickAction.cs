using UnityEngine;

/*
 * Controla a ação do jogador de chutar a bola no nível 1.
*/
public class KickAction : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // referência ao nível 1
    private Level1Controller _level1;

    // referência ao controlador do jogador
    private PlayerController _player;

    // para controlar as animações
    private Animator _animator;

    // referência do controlador da bola
    private GameObject _ballObject;

    // para a força que a bola é empurrada ou chutada
    private float _force = 5f;


    /* PROPRIEDADES PÚBLICAS */

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


    /* MÉTODOS */

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

        // garante que a direção será sempre correta, independente da força aplicada
        direction = direction.normalized;

        Rigidbody rigidbodyBall = _ballObject.GetComponent<Rigidbody>();
        rigidbodyBall.AddForce(direction * _force, ForceMode.Impulse);
    }
}