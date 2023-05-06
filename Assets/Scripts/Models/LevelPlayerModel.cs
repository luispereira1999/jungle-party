using UnityEngine;

/*
 * Diferente da classe GamePlayerModel que armazena as informa��es de cada jogador de forma geral, de n�vel para n�vel.
 * Nesta classe s�o apenas armazenadas as informa��es no respetivo n�vel.
*/
public class LevelPlayerModel
{
    // ATRIBUTOS PRIVADOS

    private int _id;
    private int _levelScore;
    private bool _hasBomb;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;


    // PROPRIEDADES P�BLICAS

    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }

    public int LevelScore
    {
        get { return _levelScore; }
        set { _levelScore = value; }
    }

    public bool HasBomb
    {
        get { return _hasBomb; }
        set { _hasBomb = value; }
    }

    public Vector3 InitialPosition
    {
        get { return _initialPosition; }
        set { _initialPosition = value; }
    }

    public Quaternion InitialRotation
    {
        get { return _initialRotation; }
        set { _initialRotation = value; }
    }


    // CONSTRUTOR

    public LevelPlayerModel(int id, int levelScore)
    {
        _id = id;
        _levelScore = levelScore;
    }
}