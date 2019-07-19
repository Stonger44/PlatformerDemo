using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float _timer = 0.0f;

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
        _timer = Time.time;

        _uiManager.UpdateTimeText(_timer);
    }
}
