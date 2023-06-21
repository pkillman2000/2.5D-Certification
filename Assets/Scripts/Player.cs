using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _gravity;
    private Vector3 _direction;
    private Vector3 _velocity;
    private float _yVelocity;

    CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("Character Controller is Null!");
        }
    }


    private void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (_controller.isGrounded == true) // Character on ground - Get movement input
        {
            _direction = new Vector3(0, 0, horizontalInput);
            _velocity = _direction * _speed;
        }
        else // Add gravity
        {
            _yVelocity -= _gravity;
        }

        _velocity.y = _yVelocity; // Add gravity if necessary

        _controller.Move(_velocity * Time.deltaTime); // Move character
    }
}
