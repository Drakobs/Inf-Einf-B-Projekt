using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] private float flyForce = 55f;
    [SerializeField] private float maxFlyForce = 30f;
    [SerializeField] Animator _anim;
    [SerializeField] private bool isFlying;
    [SerializeField] private bool gameStarted;
    [SerializeField] private float yVelocity_rb;
    private bool isHoldingFly;

    private bool isAlive = true;

    public event Action Died;


    private void Awake()
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

    private void Update()
    {
        animUpdate();
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
        gameStarted = true;
        _anim.SetBool("isGrounded", !isFlying);
        _anim.SetBool("gameStarted", gameStarted);
    }



    private void animUpdate()
    {
        yVelocity_rb = _rb.linearVelocity.y;
        _anim.SetFloat("yVelocity", yVelocity_rb);
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



