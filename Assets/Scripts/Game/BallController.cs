using UnityEngine;


/// <summary>
/// Esta classe controla a bola existente no nível 1.
/// </summary>
public class BallController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // variável para a física (movimento e velocidade da bola)
    private Rigidbody _rigidbody;

    // para saber quem marcou golo
    private bool _player1Scored;
    private bool _player2Scored;

    // para as referência das linhas de golo de cada baliza
    [SerializeField] private GameObject _goalLinePlayer1;
    [SerializeField] private GameObject _goalLinePlayer2;


    /* PROPRIEDADES PÚBLICAS */

    public bool Player1Scored
    {
        get { return _player1Scored; }
        set { _player1Scored = value; }
    }

    public bool Player2Scored
    {
        get { return _player2Scored; }
        set { _player2Scored = value; }
    }


    /* MÉTODOS */

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider collider)
    {
        // colisão com alguma parede da arena - impede que a bola saia da arena
        if (collider.CompareTag("Wall") || collider.CompareTag("Goal"))
        {
            // obtém o vetor da direção oposta, à posição que a bola bateu na parede
            Vector3 oppositeDirection = transform.position - collider.ClosestPoint(transform.position);

            // normalizar o vetor, garante que a direção será sempre correta independente da força aplicada
            oppositeDirection = oppositeDirection.normalized;

            // aplicar uma força oposta para manter a bola dentro da arena
            _rigidbody.AddForce(oppositeDirection * 10f, ForceMode.Impulse);
        }

        // colisão com a linha de golo - deteta que foi golo
        if (collider.CompareTag(_goalLinePlayer1.tag))
        {
            _player2Scored = true;
        }
        else if (collider.CompareTag(_goalLinePlayer2.tag))
        {
            _player1Scored = true;
        }
    }

    public bool IsGoalScored()
    {
        return _player1Scored || _player2Scored ? true : false;
    }
}