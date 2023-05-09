using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryAction : MonoBehaviour, IPlayerAction
{
    /* ATRIBUTOS PRIVADOS */

    // referência ao nível 1
    private Level3Controller _level3;

    // referência ao controlador do jogador
    private PlayerController _player;

    // para controlar as animações
    private Animator _animator;

    // referência do controlador da bola
    private GameObject _ballObject;

    // para a força que o jogador é empurrado 
    private float _force = 5f;


    /* PROPRIEDADES PÚBLICAS */

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


    /* MÉTODOS */

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

