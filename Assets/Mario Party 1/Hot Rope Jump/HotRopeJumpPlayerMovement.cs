using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class HotRopeJumpPlayerMovement : MonoBehaviour
{

    [SerializeField] Rigidbody _rb;
    bool _isGrounded;
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Animator _animator;

    public void OnJump(InputValue value)
    {
        if (_isGrounded)
        {
            // Using Impulse for an immediate jump feel
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _animator.SetTrigger("Jump");
        }
    }

    private void Update()
    {
        CheckGrounded();
        UpdateAnimation();
    }

    private void CheckGrounded()
    {
        // Use the Raycast method from the previous step—it's more reliable
        float rayLength = 0.5f;
        _isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, rayLength, groundLayer);

    }


    private void UpdateAnimation()
    {
        if (!_animator) return;

        // Keep the animator informed about ground status for transitions
        _animator.SetBool("isGrounded", _isGrounded);
    }
}
