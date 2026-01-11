using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _flyAction;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        //Get Input and Action
        _playerInput = GetComponent<PlayerInput>();
        _flyAction = _playerInput.actions.FindAction("Fly");
    }

    void FixedUpdate()
    {
        // 5. Check if the action exists and is being pressed
        if (_flyAction != null && _flyAction.IsPressed())
        {
            WizardMovement.Instance.Fly();
            Debug.Log("InputManager: Fly() gets called");
        }

        //better movement feeling -> yforce = 0, when not pressed anymore
        if (_flyAction.IsPressed() != true)
        {
            WizardMovement.Instance.DropFlyForce();
        }
    }
}