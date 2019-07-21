using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private GameManager _gameManager = null;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.gameState != "GameStart" || _gameManager.gameState != "GamePaused")
        {
            this.transform.Rotate(0, 0, 2); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            if (playerScript != null)
            {
                playerScript.CollectCollectible();
            }
            Destroy(this.gameObject);
        }
    }
}
