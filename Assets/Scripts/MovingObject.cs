using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 _pointA = new Vector3();
    [SerializeField]
    private Vector3 _pointB = new Vector3();
    [SerializeField]
    private float _speed = 5.0f;
    private string moveTowards = "";

    private GameManager _gameManager = null;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_gameManager.gameState != "GameStart" && _gameManager.gameState != "GamePaused")
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        if (this.transform.position == _pointA)
        {
            moveTowards = "PointB";
        }
        if (this.transform.position == _pointB)
        {
            moveTowards = "PointA";
        }

        if (moveTowards == "PointA")
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _pointA, Time.deltaTime * _speed);
        }
        if (moveTowards == "PointB")
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _pointB, Time.deltaTime * _speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
