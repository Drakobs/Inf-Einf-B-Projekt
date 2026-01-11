using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] private float flyForce = 55f;

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



