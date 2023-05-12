using UnityEngine;


/// <summary>
/// Esta classe controla a bomba existente no n�vel 4.
/// Quando a primeira bomba � iniciada no n�vel ou na ronda, � inserida como objeto "filho" de um objeto "jogador".
/// Ao trocar a bomba de jogador, esta passa novamente como um objeto "filho" para o objeto do outro jogador.
/// </summary>
public class BombController : MonoBehaviour
{
    /* ATRIBUTOS */

    // refer�ncia do jogador que tem a bomba
    private GameObject _player;


    /* M�TODOS */

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
}