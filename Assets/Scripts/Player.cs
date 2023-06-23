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
    private Animator _animator;

    private Vector3 _direction;
    private Vector3 _velocity;
    private bool _jumpingFlag = false;
    [SerializeField]
    private GameObject _playerModel;
    private bool _facingRight = true;
    private bool _lastFacingStatus = true;


    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if(_controller == null)
        {
            Debug.LogError("Character Controller is Null!");
        }

        _animator = GetComponentInChildren<Animator>();
        if(_animator == null)
        {
            Debug.LogError("Animator is Null!");
        }
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Making this GetAxisRaw stops smooth ramping up of values.  Will go from 0 to 1 or 0 to -1 instantly.
        
        // Rotate model to face correct direction
        if(horizontalInput > 0 && !_facingRight)
        {
            _facingRight = true;
            _playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        if (horizontalInput < 0 && _facingRight)
        {
            _facingRight = false;
            _playerModel.transform.localEulerAngles = new Vector3(0, -180, 0);
        }

        // Animation Info
        float absoluteSpeed = Mathf.Abs(horizontalInput); // Calculate absolute value of horizontal input
        _animator.SetFloat("Speed", absoluteSpeed); // Set "Speed" value in Animator - Used for idle/walking/jumping

        if (_controller.isGrounded == true)
        {
            _canWallJump = true;
            if(_jumpingFlag)// Only set animator if was jumping previously
            {
                _animator.SetBool("isJumping", false);
                _jumpingFlag = false; // Not jumping anymore
            }
            _direction = new Vector3(0, 0, horizontalInput);
            _velocity = _direction * _speed;

            if (Input.GetKeyDown(KeyCode.Space)) // Jumping
            {
                _jumpingFlag = true;
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
                _animator.SetBool("isJumping", _jumpingFlag);
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
