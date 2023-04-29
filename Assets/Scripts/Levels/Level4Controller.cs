using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

/*
 * Controla o n�vel 4.
 * O n�vel consiste em trocar de bomba de jogador para jogador,
 * at� que o tempo acabe e quem tem a bomba perde.
 * O n�vel � constituido por v�rias rondas.
*/
public class Level4Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a refer�ncia do controlador de jogo
    private GameController _game;

    // vari�veis para guardar os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;
    private int _playerIDWithBomb = -1;

    // para os objetos do n�vel - bomba e power ups
    [SerializeField] private GameObject _bombPrefab;
    private GameObject _bombObject;
    private BombController _bombController;

    private List<GameObject> _powerUps = new List<GameObject>();
    [SerializeField] private GameObject _powerUp;
    private bool _freezeComponents = false;

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    private List<LevelPlayerModel> levelPlayers;

    // para definir a a��o dos jogadores neste n�vel
    private ThrowController _throwController;

    // para o tempo, rondas e pontua��o
    private TimerController _timer;

    [SerializeField] int _rounds;
    [SerializeField] int _roundPoints;
    [SerializeField] private Text _roundsComponent;
    [SerializeField] GameObject _popUpWinner;
    private int _currentRound = 0;

    private int _points;

    // para o painel de introdu��o do n�vel, que � mostrado antes do jogo come�ar
    [SerializeField] private GameObject _introPanel;

    // para o estado do n�vel
    LevelState _currentLevelState;


    /* PROPRIEDADES P�BLICAS */

    public bool CollisionOccurred
    {
        get { return _collisionOccurred; }
        set { _collisionOccurred = value; }
    }


    /* M�TODOS */

    void Start()
    {
        _game = GameController.Instance;

        // TEST: usar isto enquanto � testado apenas o n�vel atual (sem iniciar pelo menu)
        _game.Players = new List<PlayerModel>();
        _game.InitiateGame();

        levelPlayers = new List<LevelPlayerModel>();
        _timer = TimerController.Instance;

        foreach (PlayerModel player in _game.Players) {
            LevelPlayerModel levelPlayer = new LevelPlayerModel();

            levelPlayer.Id = player.id;
            levelPlayers.Add(levelPlayer);
        }

        _currentLevelState = LevelState.INTRO_LEVEL;

        TimerController.Freeze();

        _roundsComponent.text = _currentRound.ToString();

        int randomID = GenerateFirstPlayerToPlay();
        _playerIDWithBomb = randomID;

        // exibir objetos na cena
        CreateObjectInScene();
    }

    /*
     * � executado ao clicar no bot�o de iniciar, no painel de introdu��o do n�vel.
    */
    public void InitAfterIntro()
    {
        _currentLevelState = LevelState.START_ROUND;

        TimerController.Unfreeze();

        _currentRound++;
        _roundsComponent.text = _currentRound.ToString();

        Destroy(_introPanel);

        InvokeRepeating("SpawnPowerUp", 5f, 10f);
    }

    void NextRound()
    {
        _currentLevelState = LevelState.START_ROUND;

        TimerController.Unfreeze();
        _timer.Restart();

        _currentRound++;
        _roundsComponent.text = _currentRound.ToString();

    }

    void CreateObjectInScene()
    {
        SpawnPlayer1();
        SpawnPlayer2();

        AddActionToPlayer1();
        AddActionToPlayer2();

        SpawnBomb();
        AssignBomb();
    }

    void Update()
    {
        if (!_timer.hasFinished())
        {
            return;
        }
        
        if (_rounds == _currentRound && !_freezeComponents)
        {
            
            _freezeComponents = true;
            _popUpWinner.SetActive(true);

            CancelInvoke("CreatePowerUp");

            SetLevelPoints();

            GameObject[] popUpText = GameObject.FindGameObjectsWithTag("PopUpText");

            string textPoints = "";

            foreach (LevelPlayerModel levelPlayer in levelPlayers)
            {
                textPoints += "Jogador " + levelPlayer.Id + ": " + levelPlayer.LevelScore + "\n";
            }

            popUpText[0].GetComponent<TextMeshProUGUI>().text = textPoints;


            SetInitialPosition();

        } else if (!_freezeComponents) {

            float freezeTime = 5f;
            _freezeComponents = true;

            SetLevelPoints();

            _player1Object.GetComponent<PlayerController>().Freeze(freezeTime);
            _player2Object.GetComponent<PlayerController>().Freeze(freezeTime);

            Invoke("RestartRound", freezeTime);
        }
    }

    public void Init()
    {
        Time.timeScale = 1f;

        Destroy(GameObject.Find("IntroPanel"));

        InvokeRepeating("CreatePowerUp", 5f, 10f);
    }

    void RestartRound()
    {
        _timer.Restart();
        _currentRound++;
        _freezeComponents = false;

        SetInitialPosition();

        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("PowerUp");

        // Loop through each object and destroy it
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        _roundsComponent.text = _currentRound.ToString();
    }

    void CreatePowerUp()
    {
        System.Random rnd = new System.Random();
        int xValue = rnd.Next(72, 440);
        int zValue = rnd.Next(62, 430);

        Instantiate(_powerUp, new Vector3(xValue, _powerUp.transform.position.y, zValue), Quaternion.identity);
    }

    void SetLevelPoints() {

        int winnerId = _playerIDWithBomb == 1 ? 2 : 1;

        LevelPlayerModel levelPlayer = levelPlayers[winnerId - 1];

        levelPlayer.LevelScore += _roundPoints;

        string scoreText = winnerId == 1 ? "ScoreP1Text" : "ScoreP2Text";

        GameObject[] scoreTextComp = GameObject.FindGameObjectsWithTag(scoreText);
        scoreTextComp[0].GetComponent<TextMeshProUGUI>().text = levelPlayer.LevelScore.ToString();
    }

    void SetInitialPosition()
    {
        _player1Object.transform.position = levelPlayers[0].InitialPosition;
        _player2Object.transform.position = levelPlayers[1].InitialPosition;

        _player1Object.transform.rotation = levelPlayers[0].InitialRotation;
        _player2Object.transform.rotation = levelPlayers[1].InitialRotation;
    }

    void SpawnPlayer1()
    {
        _player1Object = Instantiate(_game.Players[0].prefab);
        levelPlayers[0].InitialPosition = _player1Object.transform.position;
        levelPlayers[0].InitialRotation = _player1Object.transform.rotation;
    }

    void SpawnPlayer2()
    {
        _player2Object = Instantiate(_game.Players[1].prefab);
        levelPlayers[1].InitialPosition = _player2Object.transform.position;
        levelPlayers[1].InitialRotation = _player2Object.transform.rotation;
    }

    /*
     * Adiciona o script da a��o ao objeto do jogador, para definir essa a��o ao personagem.
    */
    void AddActionToPlayer1()
    {
        _throwController = _player1Object.AddComponent<ThrowController>();
        _player1Object.GetComponent<PlayerController>().SetAction(_throwController, this);
    }

    void AddActionToPlayer2()
    {
        _throwController = _player2Object.AddComponent<ThrowController>();
        _player2Object.GetComponent<PlayerController>().SetAction(_throwController, this);
    }

    int GenerateFirstPlayerToPlay()
    {
        // previne que o Random n�o fique viciado
        Random.InitState(DateTime.Now.Millisecond);

        return Random.Range(1, 3);
    }

    public GameObject GetPlayerWithBomb()
    {
        if (_playerIDWithBomb == 1)
        {
            return _player2Object;
        }
        else
        {
            return _player1Object;
        }
    }

    public void ChangePlayerTurn()
    {
        if (_playerIDWithBomb == 1)
        {
            _playerIDWithBomb = 2;
        }
        else if (_playerIDWithBomb == 2)
        {
            _playerIDWithBomb = 1;
        }
    }

    void SpawnBomb()
    {
        _bombObject = Instantiate(_bombPrefab, _bombPrefab.transform.position, Quaternion.identity);
    }

    public void AssignBomb()
    {
        _bombController = _bombObject.GetComponent<BombController>();
        _bombController.SetPlayer(this.GetPlayerWithBomb());
        _bombController.SetPlayerAsParent(this.GetPlayerWithBomb());
        _bombController.SetLocalPosition(new Vector3(0.042f, 0.39f, 0.352f));
        _bombController.SetLocalRotation(Quaternion.Euler(270f, 0f, 0f));
        _bombController.SetLocalScale(new Vector3(85f, 85f, 85f));
    }

    void SpawnPowerUp()
    {
        System.Random rnd = new System.Random();
        int xValue = rnd.Next(72, 440);
        int zValue = rnd.Next(62, 430);

        Instantiate(_powerUp, new Vector3(xValue, _powerUp.transform.position.y, zValue), Quaternion.identity);
    }
}