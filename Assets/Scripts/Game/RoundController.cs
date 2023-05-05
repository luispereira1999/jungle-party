using UnityEngine;
using UnityEngine.UI;

/*
 * Controla cada ronda do n�vel.
 * Mostrando a ronda atual e a introdu��o da pr�xima ronda.
*/
public class RoundController : MonoBehaviour
{
    // ATRIBUTOS PRIVADOS

    private int _currentRound = 0;
    [SerializeField] private int _numberOfRounds;
    [SerializeField] private int _pointsPerRound;
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

    public int PointsPerRound
    {
        get { return _pointsPerRound; }
        set { _pointsPerRound = value; }
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
}