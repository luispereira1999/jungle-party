using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    /* ATRIBUTOS */

    // referência do jogador que tem a bomba
    private GameObject _player;


    private Rigidbody rb;
    //private Vector3 initialPosition;
    //private Quaternion initialRotation;
    private bool _isGrabbed = false;
    private bool _hasPickedUp = false;
    private bool _hasThrown = false;
    private Vector3 thrownDirection;

    public float throwForce = 10f;

    public bool IsGrabbed
    {
        get { return _isGrabbed; }
        set { _isGrabbed = value; }
    }

    public bool HasThrown
    {
        get { return _hasThrown; }
        set { _hasThrown = value; }
    }


    /* MÉTODOS */

    private void Start()
    {

    }

    private void Update()
    {
        
        if (_hasThrown)
        {
            transform.position += thrownDirection;
        }
        
    }

    public void SetPlayer(GameObject currentPlayer)
    {
        _player = currentPlayer;
    }

    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void SetLocalRotation(Quaternion rotation)
    {
        transform.localRotation = rotation;
    }

    public void SetLocalScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void SetPlayerAsParent(GameObject player)
    {
        transform.SetParent(player.transform);
    }

    public void throwApple()
    {

        thrownDirection = _player.transform.forward * 0.1f;

        _player.GetComponent<PlayerController>().SetThrownActions();

        _hasThrown = true;
        

        transform.SetParent(null);
        _player = null;

    }
}
