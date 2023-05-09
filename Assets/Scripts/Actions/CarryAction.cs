using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryAction : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // refer�ncia ao n�vel 1
    private Level3Controller _level3;

    // refer�ncia ao controlador do jogador
    private PlayerController _player;

    // para controlar as anima��es
    private Animator _animator;

    // refer�ncia do controlador da bola
    private GameObject _ballObject;

    // para a for�a que o jogador � empurrado 
    private float _force = 5f;


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
        
    }

    
}

