using UnityEngine;

/*
 * É responsável por tratar da pontuação de cada jogador.
*/
public class ScoreController : MonoBehaviour
{
    private int _score;

    void SetScore(int score)
    {
        _score = score;
    }

    int GetScore()
    {
        return _score;
    }

    void UpdateScore(int score)
    {
        _score = score;
    }
}