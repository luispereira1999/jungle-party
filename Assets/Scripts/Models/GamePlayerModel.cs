using UnityEngine;

/*
 * Armazenar informa��es de cada jogador.
 * Necess�rio para manter os dados dos jogadores (pontua��o, etc.) entre cenas.
*/
public class GamePlayerModel
{
    // ATRIBUTOS PRIVADOS

    private int _id;
    private GameObject _prefab;
    private int _globalScore;


    // PROPRIEDADES P�BLICAS

    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }

    public GameObject Prefab
    {
        get { return _prefab; }
        set { _prefab = value; }
    }

    public int GlobalScore
    {
        get { return _globalScore; }
        set { _globalScore = value; }
    }


    // CONSTRUTOR

    public GamePlayerModel(int id, GameObject prefab, int globalScore)
    {
        _id = id;
        _prefab = prefab;
        _globalScore = globalScore;
    }
}