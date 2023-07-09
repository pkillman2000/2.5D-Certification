using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    // Movement
    [Header("Movement")]
    [SerializeField]
    private float _walkingSpeed = 5;
    [SerializeField]
    private float _runningSpeed = 8; //*** TODO ***//
    [SerializeField]
    private float _jumpHeight = 400;
    [SerializeField]
    private float _doubleJumpHeight = 600;
    [SerializeField]
    private float _climbingLadderSpeed = 1;
    
    private float _currentSpeed = 0;
    private Input_Actions _inputActions;
    private Rigidbody _rigidbody;
    public bool _isGrounded;
    public bool _canMove = true;

    // Animation
    [Header("Animation")]
    [SerializeField]
    private GameObject _playerModel;
    private Animator _animator;
    private Vector3 _currentIdlePosition;
    private bool _isLedgeClimb = false;
    private Vector3 _modelDirection;
    private bool _isClimbingLadder = false;
    private bool _isRolling = false;

    private void Start()
    {

        _inputActions = new Input_Actions();
        if(_inputActions == null)
        {
            Debug.LogError("Player Input Actions is Null!");
        }
        else
        {
            _inputActions.Player.Enable();
        }

        _animator = GetComponentInChildren<Animator>();
        if(_animator == null)
        {
            Debug.LogError("Animator is Null!");
        }

        _rigidbody = GetComponent<Rigidbody>();
        {
            if(_rigidbody == null)
            {
                Debug.LogWarning("Rigidbody is Null!");
            }
        }

        // Walk
        _inputActions.Player.Walk.started += Walk_started;
        _inputActions.Player.Walk.canceled += Walk_canceled;

        // Jump
        _inputActions.Player.Jump.performed += Jump_performed;

        // Double Jump
        _inputActions.Player.DoubleJump.performed += DoubleJump_performed;

        // Ledge Grab
        _inputActions.Player.Climb.performed += Climb_performed;

        // Roll
        _inputActions.Player.Roll.performed += Roll_performed;

    }


    private void Update()
    {
        if(_canMove)
        {
            CalculateMovement();
        }

        if(_isClimbingLadder)
        {
            ClimbingLadder();
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(0, 0, _currentSpeed);
    }

    // Movement
    private void Walk_started(InputAction.CallbackContext obj)
    {
        _currentSpeed = _walkingSpeed * obj.ReadValue<float>() * Time.deltaTime;
        if(_isRolling) // Slow motion for rolling animation
        {
            _currentSpeed = _currentSpeed / 2;
        }
        if (_canMove)
        {
            if (_isGrounded)// Only animate walking when on ground.
            {
                float absoluteSpeed = Mathf.Abs(_currentSpeed); // Walking Animation
                _animator.SetFloat("Speed", absoluteSpeed);
            }
            else
            {
            }

            if (_currentSpeed > 0) // Rotate model to correct direction
            {
                //_facingRight = true;
                _playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (_currentSpeed < 0)
            {
                _playerModel.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            _animator.SetFloat("Speed", 0);
        }
    }

    private void Walk_canceled(InputAction.CallbackContext obj)
    {
        _currentSpeed = 0;
        _animator.SetFloat("Speed", 0);
    }

    // Jumping
    private void Jump_performed(InputAction.CallbackContext obj) // Jump
    {
        if(_isGrounded && _canMove) 
        {
            _rigidbody.AddForce(Vector3.up * _jumpHeight);
        }
    }

    private void DoubleJump_performed(InputAction.CallbackContext obj) // Double Jump
    {
        if (_isGrounded)
        {            
            _rigidbody.AddForce(Vector3.up * _doubleJumpHeight);
        }
    }

    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Platform")
        {
            _isGrounded = true;
            _animator.SetBool("isJumping", false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Platform")
        {
            _isGrounded = true;
            _animator.SetBool("isJumping", false);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform") 
        {
            _isGrounded = false;
            if(!_isClimbingLadder)
            {
                _animator.SetBool("isJumping", true);
            }
        }
    }

    // Ledge Grab and Climb

    private void Climb_performed(InputAction.CallbackContext obj)
    {
        if(_isLedgeClimb)
        {
            ClimbLedge();
        }
    }

    public void LedgeGrab(Vector3 snapToPosition, Vector3 idlePosition)
    {
        _currentIdlePosition = idlePosition;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        
        _canMove = false;
        _isLedgeClimb = true;
        this.transform.position = snapToPosition;
        _animator.SetFloat("Speed", 0);
        _animator.SetBool("isLedgeGrab", true);
        _animator.SetBool("isJumping", false);
    }

    public void ClimbLedge()
    {
        _animator.SetTrigger("ClimbUp");
        _animator.SetBool("isLedgeGrab", false);
        _animator.SetBool("isJumping", false);
    }

    public void ClimbToIdle()
    {
        _animator.ResetTrigger("ClimbUp");
        _rigidbody.useGravity = true;
        _canMove = true;
        this.transform.position = _currentIdlePosition;
    }
    // End Ledge Grab and Climb

    // Ladder Climb
    public void ClimbLadder()
    {
        Vector3 currentPosition = this.transform.position;
        _isClimbingLadder = true;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _animator.SetFloat("Speed", 0);
        _animator.SetBool("isJumping", false);
        _canMove = false;
        _modelDirection = _playerModel.transform.localEulerAngles;

        _animator.SetBool("isClimbingLadder", true);
        
        if(_modelDirection.y == 0)
        {
            _modelDirection.y = 180;
        }
        else
        {
            _modelDirection.y = 0;
        }        
        _playerModel.transform.localEulerAngles = _modelDirection;
        this.transform.position = currentPosition;
    }

    public void ClimbingLadder()
    {
        transform.Translate(0, _climbingLadderSpeed * Time.deltaTime, 0);
    }

    public void ClimbToTopOfLadder(Vector3 idlePosition)
    {
        if(_isClimbingLadder)
        {
            _currentIdlePosition = idlePosition;
            _rigidbody.velocity = Vector3.zero;
            _modelDirection = _playerModel.transform.localEulerAngles;
            _animator.SetTrigger("isTopOfLadder");
            if (_modelDirection.y == 0)
            {
                _modelDirection.y = 180;
            }
            else
            {
                _modelDirection.y = 0;
            }
            _playerModel.transform.localEulerAngles = _modelDirection;
        }
    }

    public void ClimbLadderToIdle()
    {
        _animator.SetBool("isClimbingLadder", false);
        _rigidbody.useGravity = true;
        _canMove = true;
        _isClimbingLadder = false;
        this.transform.position = _currentIdlePosition;
    }
    // End Climb Ladder

    // Rolling
    private void Roll_performed(InputAction.CallbackContext obj)
    {
        _animator.SetTrigger("Roll");
        _isRolling = true;
    }

    public void EndRoll()
    {
        _animator.ResetTrigger("Roll");
        _isRolling = false;
    }
    // End Rolling
}
