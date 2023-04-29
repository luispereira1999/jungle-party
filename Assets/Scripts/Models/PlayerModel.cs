using UnityEngine;

/*
 * Armazenar informa��es de cada jogador.
 * Necess�rio para manter os dados dos jogadores (pontua��o, etc.) entre cenas.
*/
public class PlayerModel
{
    public GameObject prefab;
    public float score;
    public int id;

    public PlayerModel(GameObject prefab, float score, int id)
    {
        this.prefab = prefab;
        this.score = score;
        this.id = id;
    }
}