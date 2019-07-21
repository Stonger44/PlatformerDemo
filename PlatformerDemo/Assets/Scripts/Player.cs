using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField]
    private float _lowerBounds = -100f;

    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _gravity = 1.0f;
    [SerializeField]
    private float _terminalVelocity = -50.0f;
    [SerializeField]
    private float _jumpPower = 15.0f;
    [SerializeField]
    private float _perfectDoubleJumpPower = 20.0f;

    private float _yVelocity = 0.0f;
    private bool _canJump = false;
    private bool _canDoubleJump = false;

    [SerializeField]
    private int _collectibles = 0;

    private UIManager _uiManager = null;

    private int _lives = 3;
    private Vector3 _startingPosition = new Vector3(0, 3, 0);

    // Start is called before the first frame update
    void Start()
    {
        _controller = this.GetComponent<CharacterController>();
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();

        if (_uiManager != null)
        {
            _uiManager.UpdateLivesText(_lives);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (this.transform.position.y < _lowerBounds)
        {
            DamagePlayer();
        }
    }

    public void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, 0);
        Vector3 velocity = moveDirection * _speed;

        if (_controller.isGrounded)
        {
            _yVelocity = -5f;
            _canJump = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpPower;
                _canJump = false;
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && (_canJump || _canDoubleJump))
            {
                if (_yVelocity <= 3.0f && _yVelocity > 0.0f)
                {
                    _yVelocity = _perfectDoubleJumpPower;
                }
                else
                {
                    _yVelocity = _jumpPower;
                }

                _canJump = false;
                _canDoubleJump = false;
            }

            _yVelocity -= _gravity;

            if (_yVelocity < _terminalVelocity)
            {
                _yVelocity = _terminalVelocity;
            }
        }

        //save and set value of _yVelocity for the next frame update
        velocity.y = _yVelocity;

        _controller.Move(velocity * Time.deltaTime);
    }

    public void CollectCollectible()
    {
        _collectibles++;

        if (_uiManager != null)
        {
            _uiManager.UpdateCollectibleText(_collectibles);
        }
    }

    public void DamagePlayer()
    {
        _lives--;
        _uiManager.UpdateLivesText(_lives);

        if (_lives < 1)
        {
            //End game
            Debug.Log("GAME OVER");
            SceneManager.LoadScene("Level00");
        }
        else
        {
            this.transform.position = _startingPosition;
            _yVelocity = 0.0f;
        }
    }
}
