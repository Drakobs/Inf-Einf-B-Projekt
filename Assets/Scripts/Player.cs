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
}



