using UnityEngine;

/*
 * Armazenar informações de cada jogador.
 * Necessário para manter os dados dos jogadores (pontuação, etc.) entre cenas.
*/
public class PlayerModel
{
    public GameObject prefab;
    public float score;

    public PlayerModel(GameObject prefab, float score)
    {
        this.prefab = prefab;
        this.score = score;
    }
}