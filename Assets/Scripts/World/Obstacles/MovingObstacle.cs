using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public enum MovementType
    {
        Pendulum,
        PingPong,
    }

    [Header("Movement Settings")]
    [Tooltip("Speed, in which the obstacle moves; must be positive")]
    [SerializeField, Min(0f)] float movementSpeed;
    [Tooltip("Movement target vector relative to the initial local position of the game object")]
    [SerializeField] private Vector2 moveTarget;
    [SerializeField] private MovementType movementType;

    private bool isMoving;
    private Vector3 origin;
    private float elapsedTime;

    private void Start()
    {
        elapsedTime = 0f;

        GameManager.Instance.Pause += OnPause;
        GameManager.Instance.Resume += OnResume;

        origin = transform.localPosition;
        isMoving = GameManager.Instance.CurrentState == GameManager.GameState.Level;
    }

    private void OnDestroy()
    {
        GameManager.Instance.Pause -= OnPause;
        GameManager.Instance.Resume -= OnResume;
    }

    private void Update()
    {
        if (!isMoving) return;
        
        elapsedTime += Time.deltaTime;

        // check whether a valid target is set
        if (moveTarget == Vector2.zero) return;

        Vector3 offset = movementType switch
        {
            MovementType.Pendulum => CalculatePendulumOffset(),
            MovementType.PingPong => CalculatePingPongOffset(),
            _ => Vector3.zero
        };

        transform.localPosition = origin + offset;
    }

    /// <summary>
    /// Calculates the current positional offset for pendulum-movement
    /// </summary>
    /// <returns>calculated offset</returns>
    private Vector3 CalculatePendulumOffset()
    {
        Vector3 direction = moveTarget;
        // get the distance to move
        float distance = direction.magnitude;
        // normalize vector to get only the direction
        direction.Normalize();

        // calculate the current speed of movement
        float currentSpeed = Mathf.PI * movementSpeed / distance;

        // calculate an interpolation factor betweeen 0 an 1 using sine function for smooth movement
        float interpollationFactor = (Mathf.Sin(elapsedTime * currentSpeed) + 1f) * 0.5f;
        // return the calculated offset
        return direction * distance * interpollationFactor;
    }

    /// <summary>
    /// Calculates the current offset for ping-pong-movement
    /// </summary>
    /// <returns>calculated offset</returns>
    private Vector3 CalculatePingPongOffset()
    {
        Vector3 direction = moveTarget;
        // get the distance to move
        float distance = direction.magnitude;
        // normalize vector to get only the direction
        direction.Normalize();
        // calculate the travelled distance using PingPong function
        float travelledDistance = Mathf.PingPong(elapsedTime * movementSpeed, distance);
        // return the calculated offset
        return direction * travelledDistance;
    }

    #region Event Methods
    public void OnPause()
    {
        isMoving = false;
    }

    public void OnResume()
    {
        isMoving = true;
    }
    #endregion

#if UNITY_EDITOR
    /// <summary>
    /// Visualize the movement line in the editor
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 from = Application.isPlaying ? origin : transform.localPosition;
        Vector3 to = from + (Vector3)moveTarget;
        Gizmos.DrawLine(from, to);
    }
#endif
}
