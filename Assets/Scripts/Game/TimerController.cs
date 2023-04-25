using UnityEngine;
using UnityEngine.UI;

/*
 * Controla o rel�gio que existe em cada n�vel.
*/
public class TimerController : MonoBehaviour
{
    // ATRIBUTOS

    // vari�veis para o tempo iniciar e o atual 
    [SerializeField] private float _startingTime;
    private float _currentTime = 0f;

    // a label de texto onde ser� mostrado o tempo
    private Text _countdownText;


    // M�TODOS

    void Awake()
    {
        _countdownText = GetComponent<Text>();
        _currentTime = _startingTime;
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

    string convertToDisplay(float value)
    {
        if (value < 10)
        {
            return "0" + value.ToString();
        }

        return value.ToString();
    }
}