using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] private float flyForce = 55f;
    [SerializeField] Animator _anim;
    [SerializeField] private bool gameStarted;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float yVelocity;

    //Using update to give animator important values for correct animation
    private void Update()
    {
        Return_anim();
    }

    public void Fly()
    {
        //Start game after first fly() -> temporary solution for animation purposes
        if(gameStarted != true)
        {
            gameStarted = true;
        }

        _anim.SetBool("gameStarted", gameStarted);
        _rb.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
    }

    //Check Box Collider bottom -> Animation
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }

    private void Return_anim()
    {
        _anim.SetFloat("yVelocity", _rb.linearVelocity.y);
        _anim.SetBool("isGrounded", isGrounded);
    }
}



