using UnityEngine;

public class LimiteArena : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou no trigger é o jogador
        if (other.CompareTag("Player1"))
        {
            Debug.Log("aa");
            // Ativa um flag no  jogador indicando que ele está dentro da arena
            other.GetComponent<PlayerController>().estaDentroDaArena = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica se o objeto que saiu do trigger é o jogador
        if (other.CompareTag("Player1"))
        {
            Debug.Log("bb");

            // Desativa o flag indicando que o jogador está dentro da arena
            other.GetComponent<PlayerController>().estaDentroDaArena = false;
        }
    }
}