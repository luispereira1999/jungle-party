using UnityEngine;

public class Level4Controller : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject bomb;
    public float spawnDistance = 2f;

    // start is called before the first frame update
    void Start()
    {

    }

    // update is called once per frame
    void Update()
    {

    }

    public void SpawnBomb()
    {
        // TODO

        // Exemplo de spawnar objeto:
        // GameObject newObject = Instantiate(bomb, transform.position + transform.forward * spawnDistance, transform.rotation);
        // newObject.SetActive(true);
    }

    public void SpawnPowerUp()
    {
        // TODO
    }
}