using UnityEngine;

/*
 * Controla a a��o do jogador de chutar a bola no n�vel 1.
*/
public class KickController : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // refer�ncia ao n�vel 1
    private Level1Controller _level1;

    // para controlar as anima��es
    private Animator _animator;


    /* PROPRIEDADES P�BLICAS */

    // propriedade para ter acesso ao controlador do jogador
    public PlayerController Player { get; set; }

    // para ter acesso ao n�vel 1
    public MonoBehaviour Level
    {
        get { return _level1; }
        set { _level1 = (Level1Controller)value; }
    }


    /* M�TODOS */

    public void Start()
    {
        _animator = GetComponent<Animator>();
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

    }
}