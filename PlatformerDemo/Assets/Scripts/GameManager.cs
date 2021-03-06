﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseScript
{
    /*----------Sound Effects----------*\

        -New High Score: FX6, FX7
        -EndGameSuccess: FX5
        -Clear High Score: FX11
        -Collect Coin: FX41
        -Jumps: Jump7?, Jump13?, Jump12?
        -BGM: LAZZULI LUVS_instrument?!, Music/Dance002?, Music/Dance006?, Darksider
        -CountDown Blips: 

    \*----------Sound Effects----------*/


    /*----------Game States----------*\

        -GameStart
        -PreGame
        -GameRunning
        -GamePaused
        -GameSuccess

    \*----------Game States----------*/
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

    [SerializeField]
    private MainCamera _mainCameraScript = null;
    private bool _setCameraToPlayerPosition = false;
    [SerializeField]
    private Vector3 _cameraMapPosition = new Vector3(0, 10.75f, -60);
    [SerializeField]
    private Vector3 _cameraPlayerPosition = new Vector3(0, 0, -20);

    private float _bestTimeFloat = 0.0f;
    private string _bestTimeText = "";

    private AudioSource _backGroundMusic = null;


    private AudioSource _soundEffect = null;

    [SerializeField]
    private AudioClip _countDownBlip = null;
    [SerializeField]
    private AudioClip _countDownBlipFinal = null;
    [SerializeField]
    private AudioClip _resetScoreSound = null;
    [SerializeField]
    private AudioClip _endGameSuccessTune = null;
    [SerializeField]
    private AudioClip _endGameNewHighScoreTune = null;
    [SerializeField]
    private AudioClip _endGameFailureTune = null;

    private float _canPauseOrResetGameTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        _backGroundMusic = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        if (_backGroundMusic != null && _backGroundMusic.isPlaying)
            _backGroundMusic.Stop();

        _soundEffect = this.GetComponent<AudioSource>();
        if (_soundEffect == null)
            Debug.Log("Sound Effect Audio Source is null!");

        _stage = GameObject.Find("Stage");
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        _bestTimeFloat = PlayerPrefs.GetFloat("BestTime");
        _bestTimeText = PlayerPrefs.GetString("BestTimeText");

        if (_bestTimeFloat == 0 && _bestTimeText == "")
        {
            ResetBestTime();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerScript == null)
        {
            _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        if (_mainCameraScript == null)
        {
            _mainCameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();
        }

        switch (gameState)
        {
            case "GameStart":
                UpdateGameStart();
                break;
            case "PreGame":
                UpdatePreGame();
                break;
            case "GameRunning":
                UpdateGameRunning();
                break;
            case "GamePaused":
                UpdateGamePaused();
                break;
            case "GameSuccess":
                UpdateGameSuccess();
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

    private void ResetBestTime()
    {
        SetPlayerPrefBestTime(60, "01:00.00");
        _uiManager.UpdateBestTime(_bestTimeText);
        PlayAudioClip(_soundEffect, _resetScoreSound);
    }

    private void UpdateGameStart()
    {
        SetTimeScaleAndFixedDeltaTime(0);

        _uiManager.ShowPausePanel(true);
        _uiManager.ShowEndGamePanel(false);

        _uiManager.UpdateBestTime(_bestTimeText);

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.R))
        {
            _mainCameraScript.SetCameraPosition(_cameraPlayerPosition);

            SetPreGame();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ResetBestTime();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_setCameraToPlayerPosition == true)
            {
                _mainCameraScript.SetCameraPosition(_cameraPlayerPosition);
            }
            else
            {
                _mainCameraScript.SetCameraPosition(_cameraMapPosition);
            }

            _setCameraToPlayerPosition = !_setCameraToPlayerPosition;
        }
    }

    private void SetPreGame()
    {
        gameState = "PreGame";

        _gameFinished = false;

        SetTimeScaleAndFixedDeltaTime(1);

        _backGroundMusic.Stop();

        _countDownEndTime = Time.time + 3;
        _elapsedTime = 0.0f;
        _uiManager.ShowPausePanel(false);
        _uiManager.ShowEndGamePanel(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Destroy(player);
        }

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

        UpdatePreGameCountDown();
    }

    private void UpdatePreGameCountDown()
    {
        _soundEffect.clip = _countDownBlip;

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
        }

        _uiManager.UpdatePreGameCountDowntext(_preGameCountDownText);

        if (_soundEffect.isPlaying == false)
        {
            if (timeLeft >= 0)
            {
                PlayAudioClip(_soundEffect, _countDownBlip);
                StartCoroutine(StopAudioSource_Routine(_soundEffect, 1));
            }
            else
            {
                PlayAudioClip(_soundEffect, _countDownBlipFinal);
                _backGroundMusic.Play();
                gameState = "GameRunning";
                _canPauseOrResetGameTime = Time.time + 3.0f;
            }
        }
    }

    private void UpdateGameRunning()
    {
        SetTimeScaleAndFixedDeltaTime(1);

        UpdateTimer();

        if (Time.time > _canPauseOrResetGameTime)
        {
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

        if (_playerScript.collectibles == _uiManager.totalCollectibleCount)
        {
            gameState = "GameSuccess";
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

    private void UpdateGamePaused()
    {
        SetTimeScaleAndFixedDeltaTime(0);

        _backGroundMusic.Pause();

        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState = "GameRunning";
            _backGroundMusic.UnPause();
            _uiManager.ShowPausePanel(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetPreGame();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ResetBestTime();
        }
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
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SetPreGame();
            }
        }
    }

    private void GoEndGameSlowMo()
    {
        if (_endGameSlowMoEndTime == 0)
        {
            _endGameSlowMoEndTime = Time.time + 0.25f;
        }

        float timeLeft = _endGameSlowMoEndTime - Time.time;

        if (timeLeft > 0.25f)
        {
            SetTimeScaleAndFixedDeltaTime(0.8f);
        }
        else if (timeLeft > 0.2f)
        {
            SetTimeScaleAndFixedDeltaTime(0.6f);
        }
        else if (timeLeft >= 0.15f)
        {
            SetTimeScaleAndFixedDeltaTime(0.4f);
        }
        else if (timeLeft >= 0.1f)
        {
            SetTimeScaleAndFixedDeltaTime(0.2f);
        }
        else if (timeLeft >= 0.05f)
        {
            SetTimeScaleAndFixedDeltaTime(0.1f);
        }
        else if (timeLeft >= 0.025f)
        {
            SetTimeScaleAndFixedDeltaTime(0.05f);
        }
        else
        {
            SetEndGame();
        }

    }

    public void SetEndGame()
    {
        AudioClip endGameAudioClip = null; 

        _gameFinished = true;
        gameState = "GameSuccess";

        SetTimeScaleAndFixedDeltaTime(0);
        _backGroundMusic.Stop();
        _endGameSlowMoEndTime = 0.0f;

        if (_playerScript.collectibles < _uiManager.totalCollectibleCount)
        {
            _uiManager.UpdateEndGamePanel(_timerText, _bestTimeText, "GameOver");
            endGameAudioClip = _endGameFailureTune;
        }
        else if (_elapsedTime < _bestTimeFloat)
        {
            SetPlayerPrefBestTime(_elapsedTime, _timerText);

            if (_timerText == _bestTimeText)
            {
                _uiManager.UpdateEndGamePanel(_timerText, _bestTimeText, "NewBestTime");
                endGameAudioClip = _endGameNewHighScoreTune;
            }
        }
        else
        {
            _uiManager.UpdateEndGamePanel(_timerText, _bestTimeText, "Success");
            endGameAudioClip = _endGameSuccessTune;
        }

        _uiManager.ShowEndGamePanel(true);

        PlayAudioClip(_soundEffect, endGameAudioClip);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
