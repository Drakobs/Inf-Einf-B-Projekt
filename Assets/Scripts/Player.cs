using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] private float flyForce = 55f;
    [SerializeField] private float maxFlyForce = 30f;
    [SerializeField] Animator _anim;
    [SerializeField] private bool isFlying;

    private bool isHoldingFly;

    public void OnFly(InputAction.CallbackContext context)
    {
        if (context.performed) isHoldingFly = true;
        if (context.canceled) isHoldingFly = false;
        isFlying = true;
        _anim.SetBool("isGrounded", !isFlying); 
    }


    private void FixedUpdate()
    {
        if (isHoldingFly == true)
        {
            _rb.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
        }

        if (_rb.linearVelocity.y > maxFlyForce)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, maxFlyForce);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        isFlying = false; 
        _anim.SetBool("isGrounded", !isFlying);
    }
}



