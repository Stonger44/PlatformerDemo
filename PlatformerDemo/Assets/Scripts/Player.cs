using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _gravity = 0.75f;
    [SerializeField]
    private float _jumpHeight = 15.0f;

    private float _yVelocity = 0.0f;
    private bool _canDoubleJump = false;

    [SerializeField]
    private int _collectibles = 0;

    private UIManager _uiManager = null;

    // Start is called before the first frame update
    void Start()
    {
        _controller = this.GetComponent<CharacterController>();
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }

    public void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, 0);
        Vector3 velocity = moveDirection * _speed;

        if (_controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump)
            {
                if (_yVelocity <= 3.0f && _yVelocity > 0.0f)
                {
                    _yVelocity = 20;
                    Debug.Log("DOUBLE JUMP!!");
                }
                else
                {
                    _yVelocity = _jumpHeight;
                }

                _canDoubleJump = false;
            }
            _yVelocity -= _gravity;
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
}
