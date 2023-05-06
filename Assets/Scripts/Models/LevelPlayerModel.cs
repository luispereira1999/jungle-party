using UnityEngine;

/*
 * Diferente da classe GamePlayerModel que armazena as informações de cada jogador de forma geral, de nível para nível.
 * Nesta classe são apenas armazenadas as informações no respetivo nível.
*/
public class LevelPlayerModel
{
    // ATRIBUTOS PRIVADOS

    private int _id;
    private int _levelScore;
    private bool _hasBomb;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;


    // PROPRIEDADES PÚBLICAS

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