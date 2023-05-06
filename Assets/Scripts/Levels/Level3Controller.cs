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
    private GameController _gameController;

    // vari�veis sobre os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;

    // para o modelo de dados do jogador referente ao n�vel
    private List<LevelPlayerModel> levelPlayers = new();


    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameController.Instance;

        // TEST: usar isto enquanto � testado apenas o n�vel atual (sem iniciar pelo menu)
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
     * � executado quando � clicado o bot�o de pr�ximo n�vel, no painel de fim de n�vel.
    */
    public void FinishLevel()
    {
        _gameController.NextLevel(levelPlayers[0].LevelScore, levelPlayers[1].LevelScore);
    }
}