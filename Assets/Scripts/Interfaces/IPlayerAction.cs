using UnityEngine;

/*
 * Define as propriedades e métodos que as classes das ações dos jogadores devem implementar.
*/
public interface IPlayerAction
{
    MonoBehaviour Level { get; set; }
    PlayerController Player { get; set; }
    Animator Animator { get; set; }

    void Start();
    void Enter();
    void Exit();
    void Collide(Collision collision);
}