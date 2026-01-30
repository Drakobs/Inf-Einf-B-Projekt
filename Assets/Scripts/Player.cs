using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _anim;
    [SerializeField] public ParticleSystem broomFX;

    [Header("Debug Values")]
    [SerializeField] private float flyForce = 35f;
    [SerializeField] private float maxFlyForce = 20f;
    [SerializeField] private bool isFlying;
    [SerializeField] private bool gameStarted;
    [SerializeField] private float yVelocity_rb;
    private bool isHoldingFly;
    private bool isAlive = true;
    public event Action Died;

    //needed for playerCatchUp
    private float originX;
    private float catchUpSpeed = 0.5f;
    private float delayTimer = 0f;
    private float horizontalPosOffsetFloat;


    //needed for playerRotation
    public float tiltAmount = 15f;  // Maximum angle to tilt
    public float tiltSpeed = 5f;    // How fast the tilt happens


    private void Start()
    {
        GameManager.Instance.Player = this;
        originX = transform.position.x;
    }

    //Getting Input action
    public void OnFly(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            isHoldingFly = true;
        }
        if (context.canceled) 
        {
            isHoldingFly = false;
        }
            isFlying = true;
            _anim.SetBool("isGrounded", !isFlying); 
    }

    //Animations get called
    private void Update()
    {
        animUpdate();
        playerRotation();
    }

    //Physics
    private void FixedUpdate()
    {
        //user is holding flyaction -> fly
        if (isHoldingFly == true)
        {
            _rb.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
            broomFX.Play();
        }

        //player should not be able to accelerate to infinity
        if (_rb.linearVelocity.y > maxFlyForce)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, maxFlyForce);
        }

    //Resetting the position of the Player after he is pushed by world
    playerCatchUp();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        isFlying = false; 
        gameStarted = true;
        _anim.SetBool("isGrounded", !isFlying);
        _anim.SetBool("gameStarted", gameStarted);
        broomFX.Stop();
        //reset rotation when not flying & rotation is not 0 -> not working
    }



    private void animUpdate()
    {
        yVelocity_rb = _rb.linearVelocity.y;
        _anim.SetFloat("yVelocity", yVelocity_rb);
        //having the player rotate while having positive y-force


    }

    private void playerCatchUp()
    {
        //determine if player is to the left(value -) or right(value +)
        horizontalPosOffsetFloat = Mathf.Abs(transform.position.x - originX);

        //determine if pos is left or right of origin
        //Abs -> Absolute Value
        if(horizontalPosOffsetFloat > 0.1f)
        {
            //Delay the recovery for 2 Seconds
            delayTimer += Time.deltaTime;

            if(delayTimer >= 2f)
            {
                float transitionX = Mathf.Lerp(transform.position.x, originX, Time.deltaTime * catchUpSpeed);
                transform.position = new Vector3 (transitionX, transform.position.y, transform.position.z);
            }

        }
        else{
            //reset the timer after transition is over
            delayTimer = 0f;
        }

    }

    private void playerRotation()
    {
        float targetZAngle = 0f;

        //determine if velocity is - or +
        if (_rb.linearVelocity.y > 0.1f) 
        {
            targetZAngle = tiltAmount;
        }
        else if (_rb.linearVelocity.y < -0.1f)
        {
            targetZAngle = -tiltAmount;
        }
        //transform to euler angle
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZAngle);
        //transform angle over time
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
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



