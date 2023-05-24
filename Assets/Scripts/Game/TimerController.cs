using UnityEngine;


/// <summary>
/// Controla o relógio que existe em cada nível.
/// </summary>
public class TimerController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // variáveis para o tempo inicial e atual (o tempo é em segundos)
    [SerializeField] private float _startingTime;
    private float _currentTime = 0f;

    private AudioSource clockTicking;

    // referência para a barra de progresso do tempo visível no nível
    [SerializeField] private ProgressBarCircleController _progressBar;

    // para guardar uma instância única desta classe
    private static TimerController _instance;

    // para saber se o relógio está ativado ou em pausa
    private bool _isPause = false;


    /* PROPRIEDADES PÚBLICAS */

    public static TimerController Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public float CurrentTime
    {
        get { return _currentTime; }
        set { _currentTime = value; }
    }


    /* MÉTODOS */

    void Awake()
    {
        if (_instance != null)
        {
            return;
        }

        // guarda em memória apenas uma instância desta classe
        _instance = this;
    }

    void Start()
    {
        clockTicking = GetComponent<AudioSource>();
        _currentTime = _startingTime;
        SetInitialTime();
        
    }

    public void SetInitialTime()
    {
        
        _progressBar.MaxValue = _currentTime;
        _progressBar.BarValue = _progressBar.MaxValue;
    }

    void Update()
    {
        if (IsFrozen() || IsOnPause())
        {
            return;
        }

        _currentTime -= Time.deltaTime;

        if (HasFinished())
        {
            clockTicking.Stop();
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

    public bool HasFinished()
    {
        return _currentTime <= 0f;
    }

    bool IsFrozen()
    {
        StopSound();
        return Time.timeScale == 0f;
    }

    public bool IsOnPause()
    {
        return _isPause == true;
    }

    public void Play()
    {
        PlaySound();
        _isPause = false;
    }

    public void Pause()
    {
        StopSound();
        _isPause = true;
    }

    public static void Freeze()
    {
        Time.timeScale = 0f;
    }

    public static void Unfreeze()
    {
        Time.timeScale = 1f;
    }

    public void PlaySound() {
        clockTicking.Play();
    }

    public void StopSound()
    {
        clockTicking.Stop();
    }
}