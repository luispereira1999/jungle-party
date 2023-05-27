using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScene : MonoBehaviour
{

    /* ATRIBUTOS PRIVADOS */

    // variável para a referência do controlador de jogo
    private GameController _gameController;

    // referencia ao n�vel atual
    private MonoBehaviour _finalScene;

    // refer�ncia ao controlador do jogador
    private PlayerController _player;

    // variáveis sobre os jogadores
    private List<LevelPlayerModel> _levelPlayers = new();

    // referência do controlador da pontuação
    [SerializeField] private ScoreController _scoreController;

    // para os componentes da UI - painel de final de jogo

    [SerializeField] private GameObject _finishedGamePanel;

    // para controlar as animações
    private Animator _animator;

    /* MÉTODOS DO MONOBEHAVIOUR */

    public MonoBehaviour Level
    {
        get { return _finalScene; }
        set { _finalScene = value; }
    }
    public PlayerController Player
    {
        get { return _player; }
        set { _player = value; }
    }
    public Animator Animator
    {
        get { return _animator; }
        set { _animator = value; }
    }
    
    void Start()
    {

        _animator = GetComponent<Animator>();
       

        _gameController = GameController.Instance;

        // TEST: usar isto enquanto é testado apenas o nível atual (sem iniciar pelo menu)
        _gameController.GamePlayers = new();
        _gameController.InitiateGame();

        // armazenar dados de cada jogador neste nível,
        // sabendo que um jogo tem vários níveis e já existem dados que passam de nível para nível, como a pontuação
        CreatePlayersDataForLevel();

        DisplayObjectInScene();
    }

    void Update()
    {
       _animator.SetBool("isSucess", true);
    }

    /* MÉTODOS DO FINALSCENECONTROLLER */

    void CreatePlayersDataForLevel()
    {
        foreach (GamePlayerModel gamePlayer in _gameController.GamePlayers)
        {
            LevelPlayerModel levelPlayer = new(gamePlayer.ID, 0, gamePlayer.Prefab.transform.position, gamePlayer.Prefab.transform.rotation);
            _levelPlayers.Add(levelPlayer);
        }
    }

    void DisplayObjectInScene()
    {
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        _levelPlayers[0].Object = Instantiate(_gameController.GamePlayers[0].Prefab);
        _levelPlayers[1].Object = Instantiate(_gameController.GamePlayers[1].Prefab);
    }

    /// <summary>
    /// Atribui os pontos do marcador e atualiza no ecrã.
    /// </summary>
    void UpdateScore(int scorerID)
    {
        _levelPlayers[scorerID - 1].LevelScore += _scoreController.AddScore();
        _scoreController.DisplayScoreObjectText(scorerID, _levelPlayers[scorerID - 1].LevelScore);
    }

    public void Quit()
    {
        TimerController.Unfreeze();

        string sceneName = "MenuScene";
        SceneManager.LoadScene(sceneName);
    }
}