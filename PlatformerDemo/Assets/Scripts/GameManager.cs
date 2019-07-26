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

    [SerializeField]
    private UIManager _uiManager = null;

    private GameObject _stage = null;
    [SerializeField]
    private GameObject _stagePrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        _stage = GameObject.Find("Stage");
    }

    // Update is called once per frame
    void Update()
    {
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
                SetGameSuccess();
                break;
            case "GameOver":
                SetGameOver();
                break;
            default:

                break;
        }
    }

    private void SetGameStart()
    {
        SetTimeScaleAndFixedDeltaTime(0);

        _uiManager.ShowPausePanel(true);

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.R))
        {
            SetPreGame();
        }
    }

    private void SetPreGame()
    {
        gameState = "PreGame";
        _countDownEndTime = Time.time + 3;
        _elapsedTime = 0.0f;
        _uiManager.ShowPausePanel(false);

        if (_stage != null)
        {
            Destroy(_stage);
            _stage = Instantiate(_stagePrefab);
        }

        _uiManager.UpdateLivesText(3);

        _uiManager.GetTotalCollectibleCount();
        _uiManager.UpdateCollectibleText(0);

        _uiManager.UpdateTimeText("00:00.00");
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

    }

    private void SetGameOver()
    {

    }

    public void SetTimeScaleAndFixedDeltaTime(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
