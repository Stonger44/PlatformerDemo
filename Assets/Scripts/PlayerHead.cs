using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platform") || other.CompareTag("MovingPlatform"))
        {
            _player.headIsTouchingPlatform = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Platform") || other.CompareTag("MovingPlatform"))
        {
            _player.headIsTouchingPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Platform") || other.CompareTag("MovingPlatform"))
        {
            _player.headIsTouchingPlatform = false;
        }
    }
}
