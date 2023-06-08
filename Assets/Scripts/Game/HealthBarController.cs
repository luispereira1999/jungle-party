using UnityEngine;


/// <summary>
/// Esta classe controla a barra da vida existente no n�vel 2.
/// </summary>s
public class HealthBarController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�veis para o dano causado pela ma�� e a quantidade de vida do jogador
    [SerializeField] private float _damage = 5f;

    // para as barras da vida de cada jogador
    [SerializeField] private GameObject _healthPlayer1;
    [SerializeField] private GameObject _healthPlayer2;

    // refer�ncia para a pr�pria inst�ncia desta classe
    private static HealthBarController _instance;


    /* PROPRIEDADES P�BLICAS */

    public static HealthBarController Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }


    /* M�TODOS */

    void Awake()
    {
        if (_instance != null)
        {
            return;
        }

        // guarda em mem�ria apenas uma inst�ncia desta classe
        _instance = this;
    }

    public void TakeDamage(int playerId)
    {
        GameObject healthBar = playerId == 1 ? _healthPlayer1 : _healthPlayer2;

        Vector3 currentScale = healthBar.transform.localScale;

        float healthValue = currentScale.x - _damage;

        if (healthValue < 0)
        {
            healthValue = 0;
        }

        // atualiza a barre de vida
        Vector3 newScale = new Vector3(healthValue, currentScale.y, currentScale.z);
        healthBar.transform.localScale = newScale;
    }

    public bool HasLoose(int playerId)
    {
      
        GameObject healthBar = playerId == 1 ? _healthPlayer1 : _healthPlayer2;

        Vector3 currentScale = healthBar.transform.localScale;

        return currentScale.x <= 0f;
    }
}