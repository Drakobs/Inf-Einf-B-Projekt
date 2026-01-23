using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] private float flyForce = 55f;

    private bool isFlying;

    public void OnFly()
    {
    }
}



