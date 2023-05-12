using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Trata de cada ronda de um nível.
/// Permite atualizar o número de rondas, atualizar a UI e determinar se é a última ronda.
/// </summary>
public class RoundController : MonoBehaviour
{
    // ATRIBUTOS PRIVADOS

    // variáveis sobre as rondas
    private int _currentRound = 0;
    [SerializeField] private int _numberOfRounds;

    // referências para a UI - texto da ronda atual e objeto que mostra o texto de introdução da próxima ronda
    [SerializeField] private Text _roundsComponent;
    [SerializeField] private GameObject _nextRoundIntro;


    // PROPRIEDADES PÚBLICAS

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


    // MÉTODOS

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