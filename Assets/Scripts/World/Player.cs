using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] private float flyForce = 55f;
    [SerializeField] private float maxFlyForce = 30f;
    [SerializeField] Animator _anim;

    private bool isFlying;
    private bool isHoldingFly;
    private bool isAlive = true;

    public event Action Died;


    private void Start()
    {
        GameManager.Instance.Player = this;
    }

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

    public void Kill()
    {
        //check if already eliminated
        if (!isAlive) return;

        isAlive = false;
        _rb.freezeRotation = false;
        Died?.Invoke();
    }
}



