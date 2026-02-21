using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class TestingRigidbodyPlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _animator;
    private Vector2 _moveInput;

    [Header("Movement Stats")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f; // Rigidbody uses force/impulse for jumps
    private bool _isGrounded;
    private bool _isPounding = false;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Rotation Stats")]
    public float rotationSpeed = 10.0f;
    [SerializeField] GameObject model;

    [Header("Animation Smoothing")]
    private float _smoothedSpeed;

    [Header("Ground Pound Settings")]
    public float poundForce = 20f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();


        // Rigidbody setup for characters
        _rb.freezeRotation = true; // Prevents the player from falling over like a capsule
        _rb.interpolation = RigidbodyInterpolation.Interpolate; // Makes movement smooth
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (_isGrounded)
        {
            // Using Impulse for an immediate jump feel
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _animator.SetTrigger("Jump");
        }
    }

    public void OnGroundPound(InputValue value)
    {
        // 1. Only pound if in the air and not already pounding
        if (!_isGrounded && !_isPounding)
        {
            StartCoroutine(ExecuteGroundPound());
        }
    }

    private System.Collections.IEnumerator ExecuteGroundPound()
    {
        _isPounding = true;

        // 2. The "Hang Time": Freeze briefly in mid-air for impact
        _rb.linearVelocity = Vector3.zero;
        _rb.useGravity = false;

        if (_animator) _animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.15f); // Short pause before the drop

        // 3. The Slam: Use VelocityChange to ignore mass/current speed
        _rb.AddForce(Vector3.down * poundForce, ForceMode.VelocityChange);
    }

    private void Update()
    {
        CheckGrounded();
        if (!_isPounding) ApplyRotation();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void CheckGrounded()
    {
        // Use the Raycast method from the previous step—it's more reliable
        float rayLength = 0.5f;
        _isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, rayLength, groundLayer);

        // 5. Reset the physics state when we land
        if (_isGrounded && _isPounding)
        {
            _isPounding = false;
            _rb.useGravity = true; // Turn gravity back on!

            // Optional: Zero out velocity to prevent "sliding" on impact
            _rb.linearVelocity = Vector3.zero;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.1f, groundCheckDistance);
    }

    private void ApplyMovement()
    {
        // 4. LOCK horizontal movement during the pound
        if (_isPounding)
        {
            // Keep the downward velocity but kill X and Z
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
            return;
        }

        Vector3 targetVelocity = new Vector3(_moveInput.x * moveSpeed, _rb.linearVelocity.y, _moveInput.y * moveSpeed);
        _rb.linearVelocity = targetVelocity;
    }

    private void ApplyRotation()
    {
        if (_moveInput.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimation()
    {
        if (_animator == null) return;

        float targetSpeed = _moveInput.magnitude;
        _smoothedSpeed = Mathf.Lerp(_smoothedSpeed, targetSpeed, Time.deltaTime * 10f);

        _animator.SetFloat("Speed", _smoothedSpeed);
        _animator.SetBool("isGrounded", _isGrounded);
    }
}