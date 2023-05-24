using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // uma vez que existem 2 objetos com este script (os 2 jogadores),
        // é necessário verificar se já ocorreu a colisão num jogador,
        // para que o mesmo código não seja executado no outro jogador

        /*
        if (_level2.CollisionOccurred)
        {
            _level2.CollisionOccurred = false;
            return;
        }

        string currentPlayerTag = _player.GetCurrentPlayer().tag;

        if (!_level2.GetPlayerWithBomb().CompareTag(currentPlayerTag))
        {
            _level2.ChangePlayerTurn();
            _level2.AssignBomb();
            _level2.CollisionOccurred = true;

            _player.Freeze(_player.GetFreezingTime());

            _animator.SetBool("isWalking", false);
            _player.IsWalking = false;
        }*/
    }

    public void onTriggerCollision(Collider collider)
    {
        throw new System.NotImplementedException();
    }
}
