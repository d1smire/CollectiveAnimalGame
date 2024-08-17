using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using PlayFab.ClientModels;
using PlayFab;
using System.Linq;

public class Arena : MonoBehaviour
{
    [SerializeField] private Fighter _player1Template;
    [SerializeField] private Fighter _player2Template;

    [SerializeField] private GameObject player2Cam;
    [SerializeField] private DataExchange dataExchange;

    private Fighter _player1;
    private Fighter _player2;

    [SerializeField] private Dictionary<string, Character> _characterParams;

    private Queue<Fighter> _readyToFight;

    private FighterSpawner _spawner;
    private PhotonManager _photonManager;
    private Notification _notification;
    private FighterData _fighterData;

    private Fighter _currentFighter;
    private int currentPlayerTurn;

    private void Awake()
    {
        _spawner = GetComponent<FighterSpawner>();
        _readyToFight = new Queue<Fighter>();
        currentPlayerTurn = 1;
    }

    private void Start()
    {
        _photonManager = GameObject.Find("PhotonService(Clone)").GetComponent<PhotonManager>();
        _fighterData = GameObject.Find("SaveFighterData(Clone)").GetComponent<FighterData>();
        _notification = GameObject.Find("Notification(Clone)").GetComponent<Notification>();

        dataExchange.InitializeArena();

        StartCoroutine(WaitForPlayers());
    }

    private IEnumerator WaitForPlayers()
    {
        while (PhotonNetwork.PlayerList.Length < 2)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        _characterParams = dataExchange.SetCharacterParams();
        dataExchange.SetAnimals();
        dataExchange.SetCameraAndPosition(player2Cam.transform);

        if (_fighterData.playernumber == 1)
        {
            _player1 = _spawner.SpawnPlayer1(_player1Template, _characterParams.Values.ElementAt(0));
            _player2 = _spawner.SpawnPlayer2(_player2Template, _characterParams.Values.ElementAt(1));
        }
        else if (_fighterData.playernumber == 2)
        {
            _player1 = _spawner.SpawnPlayer1(_player2Template, _characterParams.Values.ElementAt(1));
            _player2 = _spawner.SpawnPlayer2(_player1Template, _characterParams.Values.ElementAt(0));
        }


        _player1.SetTarget(_player2);
        _player2.SetTarget(_player1);

        InitialiseFighter(_player1);
        InitialiseFighter(_player2);

        StartCoroutine(Battle());
    }

    public void SetPlayer1(string name) 
    {
        string path = "ArenaPrefabs/" + name;
        Fighter player1 = Resources.Load<Fighter>(path);

        _player1Template = player1;
    }

    public void SetPlayer2(string name) 
    {
        string path = "ArenaPrefabs/" + name;
        Fighter player2 = Resources.Load<Fighter>(path);

        _player2Template = player2;
    }

    private void InitialiseFighter(Fighter fighter) // мб уберу нахуй
    {
        fighter.Died += OnDied;
    }

    private Fighter GetNextFighter()
    {
        if (_readyToFight.Count > 0)
        {
            return _readyToFight.Dequeue();
        }
        else
        {
            return null;
        }
    }

    private void OnDied(Fighter fighter) // можна просто говорить хто крутий хз зачем це
    {
        fighter.Died -= OnDied;
        DeleteFighter(fighter);
    }

    private void DeleteFighter(Fighter fighter) // xd
    {
        if (_player1 == fighter)
        {
            _player1 = null;
        }
        else if (_player2 == fighter)
        {
            _player2 = null;
        }
        _readyToFight.Clear();
    }

    //private void OnTurnMeterFilled(Fighter fighter)
    //{
    //    if (fighter == _player1) 
    //    {
    //        currentPlayerTurn = 1;
    //    }
    //    else if (fighter == _player2) 
    //    {
    //        currentPlayerTurn = 2;
    //    }
    //}

    private void CalculateTurnMeter(Fighter player1, Fighter player2)
    {
        float speed1 = player1.GetSpeedValue();
        float speed2 = player2.GetSpeedValue();

        if (speed1 >= speed2)
        {
            currentPlayerTurn = 1;
            _readyToFight.Enqueue(player1);
            _readyToFight.Enqueue(player2);
        }
        else
        {
            currentPlayerTurn = 2;
            _readyToFight.Enqueue(player2);
            _readyToFight.Enqueue(player1);
        }
    }

    //private void CalculateAdditionalMove(float speed1, float speed2) // якщо буде то краще рахувати результат на сервері
    //{
    //    float chance = (speed1 - speed2) / 2.0f;

    //    if (Random.value < chance / 100.0f)
    //    {
    //        _readyToFight.Enqueue(_player1);
    //    }
    //}

    private void ResetPlayer() 
    {
        _currentFighter.ui.skillNumber = 0;
    }

    private IEnumerator Battle()
    {
        while (_player1 != null && _player2 != null)
        {
            _currentFighter = GetNextFighter(); 

            if (_currentFighter != null)
            {
                if (currentPlayerTurn == 1)
                {
                    _currentFighter.ui.IsInteract(true);

                    _notification.SomeInformation(InfoStatus.Default, "Current player turn is: " + currentPlayerTurn);

                    yield return StartCoroutine(WaitForSkillSelection(_currentFighter));
                }
                else 
                {
                    _currentFighter.ui.IsInteract(false);

                    _notification.SomeInformation(InfoStatus.Default, "Current player turn is: " + currentPlayerTurn);

                    yield return StartCoroutine(WaitForEnemyMove(_currentFighter));
                }
            }

            yield return new WaitForSeconds(1f);

            if (_player1 != null && _player2 != null)
            {
                CalculateTurnMeter(_player1, _player2);
            }
        }
        if (_player1 != null)
        {
            Debug.Log("Battle win by player 1");
            //fighterData.ClearData();
            _photonManager.OnLeftRoom();
        }
        else
        {
            Debug.Log("Battle win by player 2");
            //fighterData.ClearData();
            _photonManager.OnLeftRoom();
        }
    }

    private void CharacterMove()
    {
        switch (_currentFighter.ui.skillNumber)
        {
            case 1:
                StartCoroutine(ExecuteSkill(_currentFighter.FirstSkill()));
                break;
            case 2:
                StartCoroutine(ExecuteSkill(_currentFighter.SecondSkill()));
                break;
            case 3:
                StartCoroutine(ExecuteSkill(_currentFighter.ThirdSkill()));
                break;
        }
    }

    private IEnumerator ExecuteSkill(Coroutine skillCoroutine)
    {
        yield return skillCoroutine;
        
        ResetPlayer();
        SwitchTurn();
    }

    private IEnumerator WaitForSkillSelection(Fighter fighter)
    {
        yield return new WaitUntil(() => fighter.ui.skillNumber > 0);

        //PhotonView.RPC("SetSkillNumber", RpcTarget.AllBuffered, fighterData.playernumber, fighter.ui.skillNumber);

        CharacterMove();
    }

    private IEnumerator WaitForEnemyMove(Fighter fighter)
    {
        yield return new WaitUntil(() => fighter.ui.skillNumber > 0);

        // прийняти данние і дать їх короче переписать ФУЛЛ класс

        CharacterMove();
    }

    private void SwitchTurn()
    {
        currentPlayerTurn = currentPlayerTurn == 1 ? 2 : 1;
        Debug.Log("Current player turn: " + currentPlayerTurn);
    }

    private void SetSkillNumber(int playernumber, int number)
    {
        _currentFighter.ui.skillNumber = number;
        Debug.Log("Currentfighter number: " + _currentFighter.ui.skillNumber);
    }

    //private void CalculateWinLose(int playerNumber) 
    //{
    //    switch (playerNumber) 
    //    {
    //        case 1:
    //            fighterData.ClearData();
    //            _photonManager.OnLeftRoom();
    //            break;
    //        case 2:
    //            fighterData.ClearData();
    //            _photonManager.OnLeftRoom();
    //            break;
    //    }
    //}
}
