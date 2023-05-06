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
    private GameObject _object;
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

    public GameObject Object
    {
        get { return _object; }
        set { _object = value; }
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

    public LevelPlayerModel(int id, int levelScore, Vector3 initialPosition, Quaternion initialRotation)
    {
        _id = id;
        _levelScore = levelScore;
        _initialPosition = initialPosition;
        _initialRotation = initialRotation;
    }
}