using System.Collections.Generic;
using UnityEngine;


/*
 * Controla o nível 3.
 * O nível consiste em empurrar o adversário para fora do ringue.
 * O nível é constituido por várias rondas.
*/

public class Level3Controller : MonoBehaviour
{

    /* ATRIBUTOS PRIVADOS */

    // variável para a referência do controlador de jogo
    private GameController _game;

    // variáveis para guardar os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;


    // Start is called before the first frame update
    void Start()
    {
        _game = GameController.Instance;

        // TEST: usar isto enquanto é testado apenas o nível atual (sem iniciar pelo menu)
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