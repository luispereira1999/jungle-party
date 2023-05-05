using UnityEngine;

/*
 * Armazenar informações de cada jogador.
 * Necessário para manter os dados dos jogadores (pontuação, etc.) entre cenas.
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