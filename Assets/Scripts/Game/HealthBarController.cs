using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HealthBarController : MonoBehaviour
{

    [SerializeField] private float Damage = 5f;
    [SerializeField] private float HealthAmount = 40f;

    [SerializeField] private GameObject _healthPlayer1;
    [SerializeField] private GameObject _healthPlayer2;

    private static HealthBarController _instance;

    // Start is called before the first frame update

    public static HealthBarController Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    void Awake()
    {
        if (_instance != null)
        {
            return;
        }

        // guarda em memória apenas uma instância desta classe
        _instance = this;
    }

    public void TakeDamage(int playerId)
    {
        GameObject healthBar = playerId == 1 ? _healthPlayer1 : _healthPlayer2;

        Vector3 currentScale = healthBar.transform.localScale;

        float healthValue = currentScale.x - Damage;

        if (healthValue < 0)
        {
            healthValue = 0;
        }

        // Calculate the new scale with the desired width
        Vector3 newScale = new Vector3(healthValue, currentScale.y, currentScale.z);

        // Set the new scale to the game object
        healthBar.transform.localScale = newScale;
    }


    public bool HasLoose(int playerId)
    {

        GameObject healthBar = playerId == 1 ? _healthPlayer1 : _healthPlayer2;

        Vector3 currentScale = healthBar.transform.localScale;

        return currentScale.x <= 0f;


    }
}
