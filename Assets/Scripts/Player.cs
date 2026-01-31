using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _anim;
    [SerializeField] ParticleSystem broomFX;
    [SerializeField] PlayerInput input;
    [SerializeField] private Broom Broom;

    [Header("Debug Values")]
    [SerializeField] private float flyForce = 35f;
    [SerializeField] private float maxFlyForce = 20f;
    [SerializeField] private bool isFlying;
    [SerializeField] private float yVelocity_rb;
    private bool isHoldingFly;
    private bool isAlive = true;
    public event Action Died;

    //used for animator
    private bool isPaused;
    private bool gameMenu;

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
        //resetting the speed of anim at start
        _anim.speed = 1f;
        isPaused = GameManager.Instance.CurrentState == GameManager.GameState.Paused;
        GameManager.Instance.Player = this;
        originX = transform.position.x;
        GameManager.Instance.Pause += OnPause;
        GameManager.Instance.Resume += OnResume;
        GameManager.Instance.LevelStarted += OnLevel;
    }

    //Getting Input action
    public void OnFly(InputAction.CallbackContext context)
    {
        // check whether the input came from a mouse
        if (context.control.device is Mouse)
        {
            // prevent flying when clicking on UI
            if (EventSystem.current.IsPointerOverGameObject()) return;
        }

        if (context.performed) 
        {
            isHoldingFly = true;
        }
        if (context.canceled) 
        {
            isHoldingFly = false;
        }
        AnimOnFly();
    }

    //Animations get called
    private void Update()
    {
        if(isPaused)
        {
            return;
        }
        animUpdate();
        playerRotation();
    }

    //Physics
    private void FixedUpdate()
    {
         if(isPaused)
        {
            return;
        }
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

    private void OnDestroy()
    {
        GameManager.Instance.Pause -= OnPause;
        GameManager.Instance.Resume -= OnResume;
        GameManager.Instance.LevelStarted -= OnLevel;
    }

    //collider on bottom of player -> not flying anymore
    public void OnTriggerEnter2D(Collider2D collision)
    {
        isFlying = false; 
        _anim.SetBool("isGrounded", !isFlying);
        broomFX.Stop();
        Broom.isFlying = false;
    }



    private void animUpdate()
    {
        yVelocity_rb = _rb.linearVelocity.y;
        _anim.SetFloat("yVelocity", yVelocity_rb);
        //check every frame if game is in level -> otherwise _anim -> idle
        gameMenu = GameManager.GameState.Paused == GameManager.Instance.CurrentState;
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

            if(delayTimer >= 3f)
            {
                float transitionX = Mathf.Lerp(transform.position.x, originX, Time.deltaTime * catchUpSpeed);
                transform.position = new Vector3 (transitionX, transform.position.y, transform.position.z);
            }

        }
        if(horizontalPosOffsetFloat < 0.1f)
        {
            transform.position = new Vector3 (originX, transform.position.y, transform.position.z);  
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



    private void AnimOnFly()
    {
        isFlying = true;
        _anim.SetBool("isGrounded", !isFlying); 
        Broom.isFlying = true;
    }

    //GAME EVENTS
    public void Kill()
    {
        //check if already eliminated
        if (!isAlive) return;

        isAlive = false;
        _rb.freezeRotation = false;
        _anim.speed = 0f;
        Died?.Invoke();
        Broom.rb_simulated = true;
    }

    //idle animation in Menu
    private void OnLevel()
    {
        _anim.SetBool("gameStarted", !gameMenu);
    }
    
    private void OnPause()
    {
        isPaused = true;
        _rb.simulated = false;
        input.DeactivateInput();
        _anim.speed = 0f;
        broomFX.Stop();
    }
    private void OnResume()
    {
        isPaused = false;
        _rb.simulated = true;
        input.ActivateInput();
        _anim.speed = 1f;
        broomFX.Play();
    }
}





