using UnityEngine;


public class AppleController : MonoBehaviour
{
    /* ATRIBUTOS */

    // refer�ncia do jogador que tem a ma��
    private GameObject _player;

    private bool _isGrabbed = false;
    private bool _hasThrown = false;
    private Vector3 _thrownDirection;

    public float _throwForce = 10f;

    private ThrowLvl2Action _throwLvl2Action;


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


    /* M�TODOS */

    private void Update()
    {
        if (_hasThrown)
        {
            transform.position += _thrownDirection;
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

    public void ThrowApple()
    {
        _thrownDirection = _player.transform.forward * 0.1f;

        _throwLvl2Action = _player.GetComponent<ThrowLvl2Action>();
        _throwLvl2Action.SetThrownActions();

        _hasThrown = true;

        transform.SetParent(null);
        _player = null;
    }
}