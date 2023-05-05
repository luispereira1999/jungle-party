using UnityEngine;

/*
 * Armazenar informa��es de cada jogador.
 * Necess�rio para manter os dados dos jogadores (pontua��o, etc.) entre cenas.
*/
public class GamePlayerModel
{
    public GameObject prefab;
    public float score;
    public int id;

    public GamePlayerModel(GameObject prefab, float score, int id)
    {
        this.prefab = prefab;
        this.score = score;
        this.id = id;
    }
}