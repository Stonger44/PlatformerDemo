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
    private TextMeshProUGUI _pregameCountDownText = null;

    [SerializeField]
    private GameObject _pausePanel = null;
    [SerializeField]
    private GameObject _endGamePanel = null;

    [SerializeField]
    private TextMeshProUGUI _endGameTextOne = null;
    [SerializeField]
    private TextMeshProUGUI _endGameTextTwo = null;
    [SerializeField]
    private TextMeshProUGUI _endGameTextThree = null;
    [SerializeField]
    private TextMeshProUGUI _endGameTextFour = null;


    public int totalCollectibleCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        totalCollectibleCount = GetTotalCollectibleCount();

        UpdateCollectibleText(0);
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
        _collectibleText.text = "Coins: " + collectibles + "/" + totalCollectibleCount;
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
        _pausePanel.SetActive(showPausePanel);
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

    public void ShowEndGamePanel(bool showEndGamePanel)
    {
        _endGamePanel.SetActive(showEndGamePanel);
    }

    public void UpdateEndGamePanel(string endGameResult)
    {
        switch (endGameResult)
        {
            case "Success":

                break;
            case "NewBestTime":

                break;
            case "GameOver":

                break;
            default:

                break;
        }
    }
}
