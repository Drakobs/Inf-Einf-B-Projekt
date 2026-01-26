using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public Camera Camera { get => _camera; }
    [SerializeField] private Camera _camera;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Adjusts the camera's orthographic size based on the desired number of visible cells on the given grid
    /// </summary>
    private void AdjustCameraSize(Grid grid, float visibleTiles)
    {
        // get the size of a tilemap tile in world units
        float tileHeight = grid.cellSize.y;

        // add two extra tiles to ensure full visibility
        float totalVisibleTiles = visibleTiles + 2;

        float visibleTilesHeight = totalVisibleTiles * tileHeight;

        _camera.orthographicSize = visibleTilesHeight / 2f;
    }

    /// <summary>
    /// Positions the camera
    /// </summary>
    public void PositionCamera(Transform leftEdgeAnchor, Grid grid, float visibleTiles)
    {
        AdjustCameraSize(grid, visibleTiles);

        float horizontalSize = _camera.orthographicSize * _camera.aspect;
        Vector3 cameraPosition = _camera.transform.position;
        cameraPosition.x = leftEdgeAnchor.position.x + horizontalSize;

        _camera.transform.position = cameraPosition;
    }
}
