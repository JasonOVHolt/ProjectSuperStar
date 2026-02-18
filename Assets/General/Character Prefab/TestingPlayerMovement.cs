using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TestingPlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private Vector2 _moveInput;
    private Vector3 _moveDirection;
    private Transform specificParent; // The target parent in the scene

    [Header("Movement Stats")]
    public float moveSpeed = 5.0f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    private float currentSpeed;
    private float _verticalVelocity;
    private bool _isJumpPressed;


    [Header("Rotation Stats")]
    public float rotationSpeed = 10.0f; // Higher = faster turns
    [SerializeField] GameObject model;

    [Header("Animation Smoothing")]
    public float animationSmoothTime = 0.1f;
    private float _smoothedSpeed;

    [Header("Ground Pound Settings")]
    public float poundSpeed = 20f;
    private bool _isPounding = false;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        specificParent = GameObject.Find("PlayerSpawner").transform;
        transform.parent = specificParent;

        for (int i = 0; i < specificParent.childCount; i++)
        {
            Transform child = specificParent.GetChild(i);
            if (child == transform)
            {
                transform.gameObject.name = "P" + (i + 1);
                ///SET MODEL FROM PLAYERPREFS
                ///INSTATIATE HERE
                break;
            }
        }

        _animator = GetComponentInChildren<Animator>();


    }

    // New Input System Message: Sent when Move keys/sticks are used
    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    // New Input System Message: Sent when Jump button is pressed
    public void OnJump(InputValue value)
    {
        if (_controller.isGrounded)
        {
            // Formula for jumping: velocity = sqrt(height * -2 * gravity)
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // New Ground Pound Logic
    public void OnGroundPound(InputValue value)
    {
        // Only allow ground pound if in the air and not already pounding
        if (!_controller.isGrounded && !_isPounding)
        {
            _isPounding = true;
            _verticalVelocity = -poundSpeed; // Slam downward instantly
            if (_animator) _animator.SetTrigger("GroundPound");
        }
    }

    private void Update()
    {
        ApplyMovement();
        if (!_isPounding) ApplyRotation();
        UpdateAnimation();
    }

    private void ApplyMovement()
    {
        Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);

        if (_controller.isGrounded)
        {
            if (_isPounding)
            {
                _isPounding = false; // Reset state when we hit floor
                // Optional: Add a screen shake or particle effect here!
            }

            if (_verticalVelocity < 0) _verticalVelocity = -2f;
        }
        else
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }

        // Disable horizontal movement while pounding for more impact
        _moveDirection = _isPounding ? Vector3.zero : move * moveSpeed;
        _moveDirection.y = _verticalVelocity;

        _controller.Move(_moveDirection * Time.deltaTime);
    }






    private void ApplyRotation()
    {
        // Only rotate if the player is actually moving
        if (_moveInput.sqrMagnitude > 0.01f)
        {
            // Calculate the target angle based on X and Y input
            float targetAngle = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg;

            // Create a rotation looking at that angle
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            // Smoothly rotate from current rotation to target rotation
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimation()
    {
        if (_animator == null) return;

        
        // Get the raw input strength (0 to 1)
        float targetSpeed = _moveInput.magnitude;

        // Smoothly transition the value so the animation doesn't "pop"
        _smoothedSpeed = Mathf.Lerp(_smoothedSpeed, targetSpeed, Time.deltaTime * 10f);

        _animator.SetFloat("Speed", _smoothedSpeed);
        _animator.SetBool("isGrounded", _controller.isGrounded);
        
    }




}
