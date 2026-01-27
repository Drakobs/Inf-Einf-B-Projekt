using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Grid tilemapGrid;
    [SerializeField] private float heightUnits;
    [SerializeField] private float visibleUnits;
    [SerializeField] private Transform anchorCamera;

    [Header("Movement")]
    [SerializeField] private SpeedConfigScriptableObject movementSpeedConfig;

    [Header("Background")]
    [SerializeField] List<SpriteRenderer> fogRenderers;
    [SerializeField] Vector2 fogPadding;
    [SerializeField] Sprite fogSprite;

    /// <summary>
    /// Current movement speed of the map
    /// </summary>
    public float MovementSpeed{ get { return _movementSpeed; } }
    private float _movementSpeed;

    /// <summary>
    /// Distance moved since level start
    /// </summary>
    public float MovedDistance { get { return _movedDistance; } }
    private float _movedDistance;

    /// <summary>
    /// Currently spawned sections in the map
    /// </summary>
    private List<Section> sections;

    /// <summary>
    /// Current pause state
    /// </summary>
    private bool isMoving;

    public event Action StartMovement;
    public event Action StopMovement;

    #region MonoBehaviour
    private void Start()
    {
        // register self on game manager
        GameManager.Instance.Map = this;

        // subscribe to GameManager events
        GameManager.Instance.LevelStarted += OnResume;
        GameManager.Instance.LevelEnded += OnPause;
        GameManager.Instance.Pause += OnPause;
        GameManager.Instance.Resume += OnResume;

        //position camera
        CameraController.Instance.PositionCamera(anchorCamera, tilemapGrid, visibleUnits);

        //set background fog
        PositionFog();
        
        //set initial paused state
        isMoving = GameManager.Instance.CurrentState == GameManager.GameState.Level;

        //set initial movement settings
        _movedDistance = 0f;
        _movementSpeed = movementSpeedConfig.GetSpeedAtDistance(_movedDistance);
    }

    // Update is called once per frame
    private void Update()
    {
        // stop execution if game is currently not in level state
        if (!isMoving) return;

        // calculates the current movement speed
        _movementSpeed = movementSpeedConfig.GetSpeedAtDistance(_movedDistance);
        // increase total moved distance
        _movedDistance += _movementSpeed * Time.deltaTime;
    }

    private void OnDestroy()
    {
        GameManager.Instance.LevelStarted -= OnResume;
        GameManager.Instance.LevelEnded -= OnPause;
        GameManager.Instance.Pause -= OnPause;
        GameManager.Instance.Resume -= OnResume;
    }
    #endregion

    #region Event Methods
    /// <summary>
    /// Executed when the level starts
    /// </summary>
    public void OnResume()
    {
        isMoving = true;

        StartMovement?.Invoke();
    }

    /// <summary>
    /// Executed when the level ends
    /// </summary>
    public void OnPause()
    {
        isMoving = false;

        StopMovement?.Invoke();
    }
    #endregion

    #region Background Fog
    private void PositionFog()
    {
        if (fogRenderers.Count == 0) return;

        Camera cam = CameraController.Instance.Camera;
        //calculate camera bounds
        var camHeight = cam.orthographicSize * 2f;
        var camWidth = camHeight * cam.aspect;
        //calculate needed fog scale
        var fogWidth = camWidth + fogPadding.x;
        var fogHeight = camHeight + fogPadding.y * 2f;
        var fogScale = new Vector3(fogWidth / fogSprite.bounds.size.x, fogHeight / fogSprite.bounds.size.y);

        var fogPosition = new Vector3(
            cam.transform.position.x - camWidth / 2f,
            cam.transform.position.y
            );

        foreach (var fogRenderer in fogRenderers)
        {
            //set sprite renderer size
            fogRenderer.transform.localScale = fogScale;
            //set fog position
            fogRenderer.transform.position = fogPosition;
        }
    }
    #endregion
}
