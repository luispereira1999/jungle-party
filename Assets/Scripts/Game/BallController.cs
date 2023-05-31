using UnityEngine;


/// <summary>
/// Esta classe controla a bola existente no n�vel 1.
/// </summary>
public class BallController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a f�sica (movimento e velocidade da bola)
    private Rigidbody _rigidbody;

    // para saber quem marcou golo
    private bool _player1Scored;
    private bool _player2Scored;

    // para as refer�ncia das linhas de golo de cada baliza
    [SerializeField] private GameObject _goalLinePlayer1;
    [SerializeField] private GameObject _goalLinePlayer2;


    /* PROPRIEDADES P�BLICAS */

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


    /* M�TODOS */

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // colis�o com alguma parede da arena - impede que a bola saia da arena
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Goal"))
        {
            // obt�m o vetor da dire��o oposta, � posi��o que a bola bateu na parede
            Vector3 oppositeDirection = transform.position - collision.collider.ClosestPoint(transform.position);

            // normalizar o vetor, garante que a dire��o ser� sempre correta independente da for�a aplicada
            oppositeDirection = oppositeDirection.normalized;

            // aplicar uma for�a oposta para manter a bola dentro da arena
            _rigidbody.AddForce(oppositeDirection * 10f, ForceMode.Impulse);
        }

        Debug.Log("_goalLinePlayer1.tag:" + _goalLinePlayer1.tag);
        //Debug.Log("collision.gameObject.tag:" + collision.gameObject.tag);
        // colis�o com a linha de golo - deteta que foi golo
        if (collision.gameObject.CompareTag(_goalLinePlayer1.tag))
        {
            _player2Scored = true;
        }
        else if (collision.gameObject.CompareTag(_goalLinePlayer2.tag))
        {
            _player1Scored = true;
        }
    }

    public bool IsGoalScored()
    {
        return _player1Scored || _player2Scored ? true : false;
    }
}