using UnityEngine;

public class LimiteArena : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou no trigger � o jogador
        if (other.CompareTag("Player1"))
        {
            Debug.Log("aa");
            // Ativa um flag no  jogador indicando que ele est� dentro da arena
            other.GetComponent<PlayerController>().estaDentroDaArena = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica se o objeto que saiu do trigger � o jogador
        if (other.CompareTag("Player1"))
        {
            Debug.Log("bb");

            // Desativa o flag indicando que o jogador est� dentro da arena
            other.GetComponent<PlayerController>().estaDentroDaArena = false;
        }
    }
}