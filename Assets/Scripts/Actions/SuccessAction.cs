using UnityEngine;

public class SuccessAction : MonoBehaviour, IPlayerAction
{
    // referencia ao nivel atual
    private MonoBehaviour _level;

    // referencia ao controlador do jogador
    private PlayerController _player;

    // para controlar as animacoes
    private Animator _animator;

    public MonoBehaviour Level
    {
        get { return _level; }
        set { _level = value; }
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

    public void Collide(Collision collision)
    {
        throw new System.NotImplementedException();
    }

    public void Enter()
    {
        _animator.SetBool("isSuccess", true);
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Trigger(Collider collider)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    public void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
