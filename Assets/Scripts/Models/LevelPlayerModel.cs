using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayerModel
{
    private int _levelScore;
    private int _id;
    private bool _hasBomb;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    public int LevelScore
    {
        get { return _levelScore; }
        set { _levelScore = value; }
    }

    public int Id { 
        get { return _id; } 
        set {  _id = value; } 
    }

    public bool HasBomb { 
        get { return _hasBomb; } 
        set { _hasBomb = value; } 
    }

    public Vector3 InitialPosition {
        get { return _initialPosition; }
        set { _initialPosition = value; }
    }

    public Quaternion InitialRotation { 
        get { return _initialRotation; } 
        set { _initialRotation = value; } 
    }
}
