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
    private float _gravity = 0.5f;
    [SerializeField]
    private float _terminalVelocity = -50.0f;
    [SerializeField]
    private float _jumpPower = 15.0f;
    [SerializeField]
    private float _perfectDoubleJumpPower = 20.0f;

    private float _yVelocity = 0.0f;
    private float _xVelocity = 0.0f;
    
    private bool _canJump = false;
    private bool _canDoubleJump = false;

    private float _midairControlReEnabledTime = 0.0f;

    public int collectibles = 0;

    private GameManager _gameManager = null;
    private UIManager _uiManager = null;

    private int _lives = 3;
    private Vector3 _startingPosition = new Vector3(0, 3, 0);

    // Start is called before the first frame update
    void Start()
    {
        _controller = this.GetComponent<CharacterController>();

        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        if (_uiManager != null)
        {
            _uiManager.UpdateLivesText(_lives);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.gameState == "PreGame" || _gameManager.gameState == "GameRunning" || _gameManager.gameState == "GameSuccess")
        {
            Move(); 
        }

        if (this.transform.position.y < _lowerBounds)
        {
            DamagePlayer();
        }
    }

    public void Move()
    {
        float horizontalInput = 0;
        Vector3 moveDirection;
        Vector3 velocity;

        if (_gameManager.gameState == "GameRunning" || _gameManager.gameState == "GameSuccess")
        {
            horizontalInput = Input.GetAxis("Horizontal") * 1.5f;
        }


        if (_midairControlReEnabledTime != 0 && _midairControlReEnabledTime > Time.time)
        {
            //player cannot change directions in midair
        }
        else
        {
            if (_controller.isGrounded)
            {
                if (horizontalInput != 0)
                    _xVelocity = horizontalInput * 1.25f;
            }
            else
            {
                if (horizontalInput != 0)
                {
                    _xVelocity += horizontalInput;

                    if (_xVelocity > 1.5f)
                    {
                        _xVelocity = 1.5f;
                    }
                    if (_xVelocity < -1.5f)
                    {
                        _xVelocity = -1.5f;
                    }



                    //if ((_xVelocity > 0 && horizontalInput < 0) || (_xVelocity < 0 && horizontalInput > 0))
                    //{
                    //    _xVelocity += horizontalInput;
                    //}
                    //else if ((_xVelocity > 0 && horizontalInput > 0) || (_xVelocity < 0 && horizontalInput < 0))
                    //{
                    //    _xVelocity = horizontalInput;
                    //}
                }
            }
        }

        moveDirection = new Vector3(_xVelocity, 0, 0);
        velocity = moveDirection * _speed;

        if (_controller.isGrounded)
        {
            _yVelocity = -5f;

            if (_xVelocity < 0)
            {
                _xVelocity += 0.05f;

                if (_xVelocity > 0)
                    _xVelocity = 0;
            }
            if (_xVelocity > 0)
            {
                _xVelocity -= 0.05f;

                if (_xVelocity < 0)
                    _xVelocity = 0;
            }

            _canJump = true;

            if (Input.GetKeyDown(KeyCode.Space) && (_gameManager.gameState == "GameRunning" || _gameManager.gameState == "GameSuccess"))
            {
                _yVelocity = _jumpPower;
                _canJump = false;
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && (_canJump || _canDoubleJump) && (_gameManager.gameState == "GameRunning" || _gameManager.gameState == "GameSuccess"))
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

        velocity.y = _yVelocity;

        _controller.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_controller.isGrounded)
        {
            if (hit.normal.y < 0.1f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _yVelocity = _jumpPower;
                    _xVelocity = hit.normal.x * 1.5f;

                    _midairControlReEnabledTime = Time.time + 1;
                }
                else
                {
                    float xNormal = hit.normal.x;

                    if (xNormal > 0)
                        _xVelocity = -0.1f;
                    if (xNormal < 0)
                        _xVelocity = 0.1f;
                } 
            }
        }
    }

    public void CollectCollectible()
    {
        collectibles++;

        if (_uiManager != null)
        {
            _uiManager.UpdateCollectibleText(collectibles);
        }
    }

    public void DamagePlayer()
    {
        if (_lives > 0)
        {
            _lives--;
            _uiManager.UpdateLivesText(_lives);
        }

        if (_lives == 0)
        {
            _gameManager.SetEndGame();
        }
        else
        {
            this.transform.position = _startingPosition;
            _yVelocity = 0.0f;
            _xVelocity = 0.0f;
        }
    }
}
