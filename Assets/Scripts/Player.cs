using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _gravity = 1.0f;
    [SerializeField]
    private float _jumpHeight = 15.0f;
    private float _yVelocity;
    private bool _canDoubleJump = false;
    private bool _canWallJump = false;
    Vector3 _wallSurfaceNormal;
    [SerializeField]
    private float _pushVelocity;

    private Vector3 _direction;
    private Vector3 _velocity;


    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if(_controller == null)
        {
            Debug.LogError("Character Controller is null!");
        }
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (_controller.isGrounded == true)
        {
            _canWallJump = true;
            _direction = new Vector3(0, 0, horizontalInput);
            _velocity = _direction * _speed;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _canWallJump == false) // Jump
            {
                if (_canDoubleJump == true) // Double Jump
                {
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && _canWallJump == true) // Wall Jump
            {
                _yVelocity += _jumpHeight;
                _velocity = _wallSurfaceNormal * _speed;
                _canWallJump = false;
            }

            _yVelocity -= _gravity;
        }

        _velocity.y = _yVelocity;

        if (_controller.enabled == true)
        {
            _controller.Move(_velocity * Time.deltaTime);
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_controller.isGrounded && hit.gameObject.tag == "Wall") // Wall Jumping
        {
            _canWallJump = true;
            _wallSurfaceNormal = hit.normal;
        }

        if (hit.gameObject.tag == "Movable") // Pushing Movables
        {
            Rigidbody rigidbody = hit.collider.attachedRigidbody;
            if (rigidbody == null || rigidbody.isKinematic)
            {
                Debug.LogWarning("Moveable Rigidbody Problems!");
                return;
            }

            Vector3 pushDir = new Vector3(0, 0, hit.moveDirection.z);
            rigidbody.velocity = pushDir * _pushVelocity;
        }
    }
}
