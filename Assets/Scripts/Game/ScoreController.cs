using UnityEngine;

/*
 * É responsável por tratar da pontuação de cada jogador.
*/
public class ScoreController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // para a pontuação do jogador
    private int _score;


    /* MÉTODOS */

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