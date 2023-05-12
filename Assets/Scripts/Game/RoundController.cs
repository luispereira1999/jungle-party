using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Trata de cada ronda de um n�vel.
/// Permite atualizar o n�mero de rondas, atualizar a UI e determinar se � a �ltima ronda.
/// </summary>
public class RoundController : MonoBehaviour
{
    // ATRIBUTOS PRIVADOS

    // vari�veis sobre as rondas
    private int _currentRound = 0;
    [SerializeField] private int _numberOfRounds;

    // refer�ncias para a UI - texto da ronda atual e objeto que mostra o texto de introdu��o da pr�xima ronda
    [SerializeField] private Text _roundsComponent;
    [SerializeField] private GameObject _nextRoundIntro;


    // PROPRIEDADES P�BLICAS

    public int CurrentRound
    {
        get { return _currentRound; }
        set { _currentRound = value; }
    }

    public int NumberOfRounds
    {
        get { return _numberOfRounds; }
        set { _numberOfRounds = value; }
    }


    // M�TODOS

    public void NextRound()
    {
        _currentRound++;
    }

    public void DisplayCurrentRound()
    {
        _roundsComponent.text = _currentRound.ToString();
    }

    public void DisplayNextRoundIntro()
    {
        _nextRoundIntro.SetActive(true);
        _nextRoundIntro.GetComponent<Text>().text = "Ronda: " + _currentRound.ToString();
    }

    public void DisableNextRoundIntro()
    {
        _nextRoundIntro.SetActive(false);
    }

    public bool IsLastRound()
    {
        return _currentRound == _numberOfRounds;
    }
}