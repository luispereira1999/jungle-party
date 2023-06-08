using UnityEngine;


/// <summary>
/// Trata da a��o do jogador de lan�ar a ma��o no n�vel 2.
/// </summary>
public class ThrowLvl2Action : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // refer�ncia ao n�vel 2
    private Level2Controller _level2;

    // refer�ncia ao controlador do jogador
    private PlayerController _player;

    // para controlar as anima��es
    private Animator _animator;

    // para a ma�� atual que o jogador apanhou
    private GameObject _apple;


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
        if (_apple != null)
        {
            AppleController appleController = _apple.GetComponent<AppleController>();
            appleController.ThrowApple();

            _apple = null;
        }
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
        if (_apple == null)
        {
            _apple = collider.gameObject;
            AppleController appleController = _apple.GetComponent<AppleController>();

            if (appleController.HasThrown)
            {
                _level2.PlaySplashSound();

                GameObject healthBarCtrl = GameObject.FindGameObjectWithTag("HealthBarCtrl");
                healthBarCtrl.GetComponent<HealthBarController>().TakeDamage(_player.PlayerID);

                Destroy(collider.gameObject);
            }
            else
            {
                appleController.SetPlayer(_player.gameObject);
                appleController.SetPlayerAsParent(_player.gameObject);
                appleController.SetLocalPosition(new Vector3(0.042f, 0.39f, 0.352f));
                appleController.SetLocalRotation(Quaternion.Euler(-90, 0, 0));
            }
        }
    }

    public void SetThrownActions()
    {
        _apple = null;
        _animator.Play("Throw", -1, 0.1f);
        _animator.Update(0f);
    }
}