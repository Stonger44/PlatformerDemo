using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //GameStart
    //PreGame
    //GameRunning
    //GamePaused
    //GameSuccess
    //GameOver
    public string gameState = "GameStart";

    private float _elapsedTime = 0.0f;
    private string _timerText = "";

    private float _countDownEndTime = 0;
    private string _preGameCountDownText = "";

    private float _endGameSlowMoEndTime = 0;
    private bool _gameFinished = false;

    private Player _playerScript = null;

    [SerializeField]
    private UIManager _uiManager = null;

    private GameObject _stage = null;
    [SerializeField]
    private GameObject _stagePrefab = null;

    private float _bestTimeFloat = 0.0f;
    private string _bestTimeText = "";

    // Start is called before the first frame update
    void Start()
    {
        _stage = GameObject.Find("Stage");
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        PlayerPrefs.DeleteAll();
        _bestTimeFloat = PlayerPrefs.GetFloat("BestTime");
        _bestTimeText = PlayerPrefs.GetString("BestTimeText");

        if (_bestTimeFloat == 0 && _bestTimeText == "")
        {
            SetPlayerPrefBestTime(44, "00:44.00");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerScript == null)
        {
            _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        switch (gameState)
        {
            case "GameStart":
                SetGameStart();
                break;
            case "PreGame":
                UpdatePreGame();
                break;
            case "GameRunning":
                UpdateGameRunning();
                break;
            case "GamePaused":
                SetGamePaused();
                break;
            case "GameSuccess":
                UpdateGameSuccess();
                break;
            case "GameOver":
                SetGameOver();
                break;
            default:

                break;
        }
    }

    private void SetPlayerPrefBestTime(float bestTimeFloat, string bestTimeText)
    {
        PlayerPrefs.SetFloat("BestTime", bestTimeFloat);
        PlayerPrefs.SetString("BestTimeText", bestTimeText);

        _bestTimeFloat = PlayerPrefs.GetFloat("BestTime");
        _bestTimeText = PlayerPrefs.GetString("BestTimeText");
    }

    private void SetGameStart()
    {
        SetTimeScaleAndFixedDeltaTime(0);

        _uiManager.ShowPausePanel(true);
        _uiManager.ShowEndGamePanel(false);

        _uiManager.UpdateBestTime(_bestTimeText);

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.R))
        {
            SetPreGame();
        }
    }

    private void SetPreGame()
    {
        gameState = "PreGame";

        _gameFinished = false;

        SetTimeScaleAndFixedDeltaTime(1);

        _countDownEndTime = Time.time + 3;
        _elapsedTime = 0.0f;
        _uiManager.ShowPausePanel(false);
        _uiManager.ShowEndGamePanel(false);

        if (_stage != null)
        {
            Destroy(_stage);
            _stage = Instantiate(_stagePrefab);
            _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        _uiManager.UpdateLivesText(3);

        _uiManager.GetTotalCollectibleCount();
        _uiManager.UpdateCollectibleText(0);

        _uiManager.UpdateTimeText("00:00.00");

        _uiManager.UpdateBestTime(_bestTimeText);
    }

    private void UpdatePreGame()
    {
        SetTimeScaleAndFixedDeltaTime(1);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetPreGame();
        }

        UpdatePreGameCountDown();
    }

    private void UpdatePreGameCountDown()
    {
        float timeLeft = _countDownEndTime - Time.time;

        if (timeLeft >= 2)
        {
            _preGameCountDownText = "3";
        }
        else if (timeLeft < 2 && timeLeft >= 1)
        {
            _preGameCountDownText = "2";
        }
        else if (timeLeft < 1 && timeLeft >= 0)
        {
            _preGameCountDownText = "1";
        }
        else
        {
            _preGameCountDownText = "GO!!";
            gameState = "GameRunning";
        }

        _uiManager.UpdatePreGameCountDowntext(_preGameCountDownText);
    }

    private void UpdateGameRunning()
    {
        SetTimeScaleAndFixedDeltaTime(1);

        UpdateTimer();

        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState = "GamePaused";
            _uiManager.ShowPausePanel(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetPreGame();
        }

        if (_playerScript.collectibles == _uiManager.totalCollectibleCount)
        {
            SetGameSuccess();
        }
    }

    private void UpdateTimer()
    {
        _elapsedTime += Time.deltaTime;

        //I don't understand this very well
        string strMinutes = Mathf.Floor((_elapsedTime % 3600) / 60).ToString("00");
        float seconds = (_elapsedTime % 60);

        string strSeconds = "";
        if (seconds < 10)
            strSeconds = "0" + seconds.ToString("f2");
        else
            strSeconds = seconds.ToString("f2");

        _timerText = strMinutes + ":" + strSeconds;

        _uiManager.UpdateTimeText(_timerText);
    }

    private void SetGamePaused()
    {
        SetTimeScaleAndFixedDeltaTime(0);

        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState = "GameRunning";
            _uiManager.ShowPausePanel(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetPreGame();
        }
    }

    private void SetGameSuccess()
    {
        gameState = "GameSuccess";
    }

    private void SetGameOver()
    {

    }

    public void SetTimeScaleAndFixedDeltaTime(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void UpdateGameSuccess()
    {
        if (_gameFinished != true)
        {
            GoEndGameSlowMo();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetPreGame();
        }
    }

    private void GoEndGameSlowMo()
    {
        if (_endGameSlowMoEndTime == 0)
        {
            _endGameSlowMoEndTime = Time.time + 0.5f;
        }

        float timeLeft = _endGameSlowMoEndTime - Time.time;

        if (timeLeft > 0.4f)
        {
            SetTimeScaleAndFixedDeltaTime(0.8f);
        }
        else if (timeLeft > 0.3f)
        {
            SetTimeScaleAndFixedDeltaTime(0.6f);
        }
        else if (timeLeft >= 0.2f)
        {
            SetTimeScaleAndFixedDeltaTime(0.4f);
        }
        else if (timeLeft >= 0.1f)
        {
            SetTimeScaleAndFixedDeltaTime(0.2f);
        }
        else
        {
            SetEndGame();
        }

    }

    private void SetEndGame()
    {
        _gameFinished = true;

        SetTimeScaleAndFixedDeltaTime(0);
        _endGameSlowMoEndTime = 0.0f;

        if (_elapsedTime < _bestTimeFloat)
        {
            SetPlayerPrefBestTime(_elapsedTime, _timerText);

            if (_timerText == _bestTimeText)
            {
                _uiManager.UpdateEndGamePanel(_timerText, _bestTimeText, "NewBestTime");
            }
        }
        else
        {
            _uiManager.UpdateEndGamePanel(_timerText, _bestTimeText, "Success");
        }

        _uiManager.ShowEndGamePanel(true);
    }
}
