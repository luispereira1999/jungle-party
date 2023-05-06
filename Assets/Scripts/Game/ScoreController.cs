using TMPro;
using UnityEngine;

/*
 * � respons�vel por tratar da pontua��o de cada jogador e exibir-la no ecr�.
*/
public class ScoreController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a pontua��o por ronda
    [SerializeField] private int _pointsPerRound;

    // para os componentes da UI - texto da pontua��o de cada jogador
    [SerializeField] private GameObject _scorePlayer1;
    [SerializeField] private GameObject _scorePlayer2;


    /* M�TODOS */

    public int AddScore()
    {
        return _pointsPerRound;
    }

    public void DisplayScoreObjectText(int winnerID, int score)
    {
        if (winnerID == 1)
        {
            _scorePlayer1.GetComponent<TextMeshProUGUI>().text = score.ToString();
        }
        else
        {
            _scorePlayer2.GetComponent<TextMeshProUGUI>().text = score.ToString();
        }
    }
}