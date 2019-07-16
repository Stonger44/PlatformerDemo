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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCollectibleText(int collectibles)
    {
        _collectibleText.text = "Coins: " + collectibles;
    }

    public void UpdateLivesText(int lives)
    {
        _livesText.text = "Lives: " + lives;
    }
}
