using UnityEngine;

/*
 * Controla a ação do jogador de chutar a bola no nível 1.
*/
public class KickController : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // referência ao nível 1
    private Level1Controller _level1;

    // para controlar as animações
    private Animator _animator;


    /* PROPRIEDADES PÚBLICAS */

    // propriedade para ter acesso ao controlador do jogador
    public PlayerController Player { get; set; }

    // para ter acesso ao nível 1
    public MonoBehaviour Level
    {
        get { return _level1; }
        set { _level1 = (Level1Controller)value; }
    }


    /* MÉTODOS */

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