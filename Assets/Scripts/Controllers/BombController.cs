using UnityEngine;

/*
 * Esta classe controla a bomba existente no nível 4.
 * Quando a primeira bomba é iniciada no nível ou na ronda, é inserida como objeto "filho" de um objeto "jogador".
 * Ao trocar a bomba de jogador, esta passa novamente como um objeto "filho" para o objeto do outro jogador
*/
public class BombController : MonoBehaviour
{
    private GameObject _player;

    public void SetPlayer(GameObject player)
    {
        _player = player;
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