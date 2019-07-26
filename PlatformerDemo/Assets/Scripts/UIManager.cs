using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _livesText = null;
    [SerializeField]
    private TextMeshProUGUI _collectibleText = null;
    [SerializeField]
    private TextMeshProUGUI _timeText = null;
    [SerializeField]
    private GameObject _pausePanel = null;
    [SerializeField]
    private TextMeshProUGUI _pregameCountDownText = null;


    private int _totalCollectibleCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _totalCollectibleCount = GetTotalCollectibleCount();

        _collectibleText.text = "Coins: 0/" + _totalCollectibleCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetTotalCollectibleCount()
    {
        return GameObject.FindGameObjectsWithTag("Collectible").Length;
    }

    public void UpdateCollectibleText(int collectibles)
    {
        _collectibleText.text = "Coins: " + collectibles + "/" + _totalCollectibleCount;
    }

    public void UpdateLivesText(int lives)
    {
        _livesText.text = "Lives: " + lives;
    }

    public void UpdateTimeText(string elapsedTime)
    {
        _timeText.text = "Time: " + elapsedTime;
    }

    public void ShowPausePanel(bool showPausePanel)
    {
        if (showPausePanel)
            _pausePanel.SetActive(true);
        else
            _pausePanel.SetActive(false);
    }

    public void UpdatePreGameCountDowntext(string preGameCountDownText)
    {
        if (preGameCountDownText == "GO!!")
        {
            StartCoroutine(ClearPreGameCountDownText_Routine(preGameCountDownText));
        }
        else
        {
            _pregameCountDownText.text = preGameCountDownText;
        }
    }

    public IEnumerator ClearPreGameCountDownText_Routine(string preGameCountDownText)
    {
        _pregameCountDownText.text = preGameCountDownText;

        yield return new WaitForSeconds(3);

        _pregameCountDownText.text = "";
    }
}
