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
    private GameController _gameController;

    // variáveis sobre os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;

    // para o modelo de dados do jogador referente ao nível
    private List<LevelPlayerModel> levelPlayers = new();


    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameController.Instance;

        // TEST: usar isto enquanto é testado apenas o nível atual (sem iniciar pelo menu)
        _gameController.GamePlayers = new();
        _gameController.InitiateGame();



        // exibir objetos na cena
        SpawnPlayer1();
        SpawnPlayer2();



    }
    void SpawnPlayer1()
    {
        _player1Object = Instantiate(_gameController.GamePlayers[0].Prefab);
    }

    void SpawnPlayer2()
    {
        _player2Object = Instantiate(_gameController.GamePlayers[1].Prefab);
    }

    void Update()
    {

    }

    /*
     * É executado quando é clicado o botão de próximo nível, no painel de fim de nível.
    */
    public void FinishLevel()
    {
        _gameController.NextLevel(levelPlayers[0].LevelScore, levelPlayers[1].LevelScore);
    }
}