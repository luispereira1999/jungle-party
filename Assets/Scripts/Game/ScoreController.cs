using UnityEngine;

/*
 * � respons�vel por tratar da pontua��o de cada jogador.
*/
public class ScoreController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // para a pontua��o do jogador
    private int _score;


    /* M�TODOS */

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