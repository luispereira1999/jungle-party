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

    public ProgressBarCircleController ProgressBar
    {
        get { return _progressBar; }
        set { _progressBar = value; }
    }

    public float CurrentTime
    {
        get { return _currentTime; }
        set { _currentTime = value; }
    }


    /* MÉTODOS */

    void Awake()
    {

    }

    void Start()
    {
        _currentTime = _startingTime;
        _progressBar.MaxValue = _currentTime;
        _progressBar.BarValue = _progressBar.MaxValue;
        Debug.Log("A:" + _progressBar.BarValue);
    }

    public void Restart()
    {
        _currentTime = _startingTime;
        _progressBar.MaxValue = _currentTime;
        _progressBar.BarValue = _progressBar.MaxValue;
    }

    public bool HasFinished()
    {
        return _currentTime <= 0f;
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

    public int GetTimeWithoutDecimals()
    {
        return Mathf.FloorToInt(_currentTime);
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