using UnityEngine;
using UnityEngine.InputSystem;

public class WizardMovement : MonoBehaviour
{
    public static WizardMovement Instance { get; private set; }
    Rigidbody2D _rb;
    private PlayerInput _playerInput; 
    private float flyForce = 55f;

    private void Awake(){
        //Singleton
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {

    }

    public void Fly()
    {
        Debug.Log("Fly() in Movement is called");
        _rb.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
    }
    public void DropFlyForce()
    {
        Debug.Log("DropFlyForce in Movement is called");
        if (_rb.linearVelocity.y > 0)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y / 2);
        }
    }
}



