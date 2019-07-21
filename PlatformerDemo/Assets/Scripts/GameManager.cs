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

    [SerializeField]
    private UIManager _uiManager = null;
    [SerializeField]
    private GameObject _pausePanel = null;

    // Start is called before the first frame update
    void Start()
    {

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
                SetPreGame();
                break;
            case "GameRunning":
                SetGameRunning();
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

        _pausePanel.SetActive(true);

        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState = "PreGame";
            _pausePanel.SetActive(false);
        }
    }

    private void SetPreGame()
    {
        SetTimeScaleAndFixedDeltaTime(1);
    }

    private void SetGameRunning()
    {
        SetTimeScaleAndFixedDeltaTime(1);

        UpdateTimer();

        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState = "GamePaused";
            _pausePanel.SetActive(true);
        }
    }

    private void SetGamePaused()
    {
        SetTimeScaleAndFixedDeltaTime(0);

        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState = "GameRunning";
            _pausePanel.SetActive(false);
        }
    }

    private void SetGameSuccess()
    {

    }

    private void SetGameOver()
    {

    }

    private void UpdatePreGameCountDown()
    {
        string preGameCountDownText = "3";



        _uiManager.UpdatePreGameCountDowntext(preGameCountDownText);
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

    public void SetTimeScaleAndFixedDeltaTime(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
