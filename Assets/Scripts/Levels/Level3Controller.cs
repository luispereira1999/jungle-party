using System.Collections.Generic;
using UnityEngine;


/*
 * Controla o n�vel 3.
 * O n�vel consiste em empurrar o advers�rio para fora do ringue.
 * O n�vel � constituido por v�rias rondas.
*/

public class Level3Controller : MonoBehaviour
{

    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a refer�ncia do controlador de jogo
    private GameController _game;

    // vari�veis para guardar os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;


    // Start is called before the first frame update
    void Start()
    {
        _game = GameController.Instance;

        // TEST: usar isto enquanto � testado apenas o n�vel atual (sem iniciar pelo menu)
        _game.Players = new List<GamePlayerModel>();
        _game.InitiateGame();



        // exibir objetos na cena
        SpawnPlayer1();
        SpawnPlayer2();



    }
    void SpawnPlayer1()
    {
        _player1Object = Instantiate(_game.Players[0].prefab);
    }

    void SpawnPlayer2()
    {
        _player2Object = Instantiate(_game.Players[1].prefab);
    }

    // Update is called once per frame
    void Update()
    {

    }
}