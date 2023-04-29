using UnityEngine;

/*
 * Controla o relógio que existe em cada nível.
*/
public class TimerController : MonoBehaviour
{
    /* ATRIBUTOS */

    // variáveis para o tempo inicial e atual (o tempo é em segundos)
    [SerializeField] private float _startingTime;
    private float _currentTime = 0f;

    // barra de progresso do tempo
    [SerializeField] private ProgressBarCircleController _progressBar;


    /* MÉTODOS */

    void Start()
    {
        _currentTime = _startingTime;
        _progressBar.MaxValue = _currentTime;
        _progressBar.BarValue = _progressBar.MaxValue;
    }

    void Update()
    {
        _currentTime -= Time.deltaTime;

        // quando o tempo terminar
        if (_currentTime < 0f)
        {
            _currentTime = 0f;
        }

        // atualiza a barra de progresso
        int timeWithoutDecimals = GetTimeWithoutDecimals();
        _progressBar.UpdateValue(timeWithoutDecimals);
    }

    int GetTimeWithoutDecimals()
    {
        return Mathf.FloorToInt(_currentTime);
    }
}