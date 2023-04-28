using UnityEngine;
using UnityEngine.UI;

/*
 * Controla o relógio que existe em cada nível.
*/
public class TimerController : MonoBehaviour
{
    // ATRIBUTOS

    // variáveis para o tempo iniciar e o atual 
    [SerializeField] private float _startingTime;
    private float _currentTime = 0f;

    // a label de texto onde será mostrado o tempo
    private Text _countdownText;

    // barra de progresso do tempo
    [SerializeField] ProgressBarCircle _ProgressBar;


    // MÉTODOS

    void Awake()
    {
        _countdownText = GetComponent<Text>();
        _currentTime = _startingTime;
    }

    void Start()
    {
        _ProgressBar.BarValue = 0;
    }

    void Update()
    {
        _currentTime -= 1 * Time.deltaTime;

        if (_currentTime < 0)
        {
            _currentTime = 0f;
        }

        float minutes = Mathf.FloorToInt(_currentTime / 60);
        float seconds = Mathf.FloorToInt(_currentTime % 60);

        _countdownText.text = convertToDisplay(minutes) + ":" + convertToDisplay(seconds);
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.G))
        {
            _ProgressBar.BarValue += 1;
        }

        if (Input.GetKey(KeyCode.F))
        {
            _ProgressBar.BarValue -= 1;
        }
    }

    string convertToDisplay(float value)
    {
        if (value < 10)
        {
            return "0" + value.ToString();
        }

        return value.ToString();
    }
}