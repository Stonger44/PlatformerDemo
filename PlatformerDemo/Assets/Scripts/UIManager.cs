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
    private int _totalCollectibleCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        _totalCollectibleCount = GameObject.FindGameObjectsWithTag("Collectible").Length;

        _collectibleText.text = "Coins: 0/" + _totalCollectibleCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCollectibleText(int collectibles)
    {
        _collectibleText.text = "Coins: " + collectibles + "/" + _totalCollectibleCount;
    }

    public void UpdateLivesText(int lives)
    {
        _livesText.text = "Lives: " + lives;
    }
}
