using UnityEngine;

/*
 * Controla o relógio que existe em cada nível.
*/
public class TimerController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // variáveis para o tempo inicial e atual (o tempo é em segundos)
    [SerializeField] private float _startingTime;
    private float _currentTime = 0f;

    // barra de progresso do tempo
    [SerializeField] private ProgressBarCircleController _progressBar;


    /* PROPRIEDADES PÚBLICAS */

    public float CurrentTime
    {
        get { return _currentTime; }
        set { _currentTime = value; }
    }


    /* MÉTODOS */

    void Start()
    {
        _currentTime = _startingTime;
        _progressBar.MaxValue = _currentTime;
        _progressBar.BarValue = _progressBar.MaxValue;
    }

    public void Restart()
    {
        _currentTime = _startingTime;
        _progressBar.MaxValue = _currentTime;
        _progressBar.BarValue = _progressBar.MaxValue;
    }

    void Update()
    {
        if (IsFrozen())
        {
            return;
        }

        _currentTime -= Time.deltaTime;

        // atualiza a barra de progresso
        int timeWithoutDecimals = GetTimeWithoutDecimals();
        _progressBar.UpdateValue(timeWithoutDecimals);
    }

    public int GetTimeWithoutDecimals()
    {
        return Mathf.FloorToInt(_currentTime);
    }

    bool IsFrozen()
    {
        return Time.timeScale == 0f;
    }

    public static void Freeze()
    {
        Time.timeScale = 0f;
    }

    public static void Unfreeze()
    {
        Time.timeScale = 1f;
    }
}