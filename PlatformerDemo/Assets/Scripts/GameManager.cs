using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float _elapsedTime = 0.0f;

    private string _timerText = "";

    private UIManager _uiManager = null;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
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
}
