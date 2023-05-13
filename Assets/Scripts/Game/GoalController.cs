using UnityEngine;


/// <summary>
/// Esta classe controla cada baliza existente no n�vel 1.
/// </summary>
public class GoalController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a velocidade que a baliza se move
    [SerializeField] private float speed;

    // para a dist�ncia do movimento tanto � esquerda como � direita
    [SerializeField] private float _distance;

    // para a posi��o central do movimento (por padr�o � a posi��o inicial)
    private float _center;

    // para a dire��o atual do movimento (1.0 � direita | -1.0 � esquerda)
    private float _direction = 1.0f;

    // para ativar ou desativar o movimento
    private bool _activeMoviemnt = true;


    /* M�TODOS */

    void Start()
    {
        // define a posi��o inicial (centro da baliza)
        _center = transform.position.x;
    }

    void Update()
    {
        if (_activeMoviemnt)
        {
            // move o objeto ao longo do eixo X
            transform.position += Vector3.right * speed * _direction * Time.deltaTime;

            // se passou o limite � direita - definir para o alvo � esquerda e mudar para essa dire��o
            if (transform.position.x > _center + _distance && _direction == 1.0f)
            {
                _direction = -1.0f;
            }
            // se passou o limite � esquerda - definir para o alvo � direita e mudar para essa dire��o
            else if (transform.position.x < _center - _distance && _direction == -1.0f)
            {
                _direction = 1.0f;
            }
        }
    }
}