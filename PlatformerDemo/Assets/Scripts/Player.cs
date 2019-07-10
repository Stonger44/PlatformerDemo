using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _gravity = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, 0);
        Vector3 velocity = moveDirection * _speed;

        if (_controller.isGrounded)
        {

        }
        else
        {
            velocity.y -= _gravity;
        }

        _controller.Move(velocity * Time.deltaTime);
    }
}
