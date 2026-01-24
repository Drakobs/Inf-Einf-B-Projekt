using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region GameState Enum
    public enum GameState
    {
        StartMenu,
        Level,
        Paused
    }
    #endregion

    public static GameManager Instance { get; private set; }

    [SerializeField] private Player _player;

    #region Properties
    public GameState CurrentState { get; private set; }
    #endregion

    #region Events
    public event Action Paused;
    public event Action Resumed;
    public event Action LevelStarted;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        CurrentState = GameState.StartMenu;
        UIManager.Instance.OpenPopup<StartMenu>();
        CameraController.Instance.PositionCamera(MapManager.Instance.StartSection.AnchorCamera);
    }


    public void StartGame()
    {
        UIManager.Instance.CloseAllPopups();
        CurrentState = GameState.Level;
        LevelStarted?.Invoke();
    }

    public void PauseGame()
    {
        CurrentState = GameState.Paused;
        Paused?.Invoke();
    }

    public void ResumeGame()
    {
        CurrentState = GameState.Level;
        Resumed?.Invoke();
    }
}
