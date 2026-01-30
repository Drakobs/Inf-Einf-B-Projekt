using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region GameState Enum
    public enum GameState
    {
        StartMenu,
        Level,
        Paused,
        Loading
    }
    #endregion

    public static GameManager Instance { get; private set; }
    
    #region Properties
    public Player Player { get; set; }
    public Map Map { get; set; }
    public GameState CurrentState { get; private set; }
    #endregion

    #region Attributes
    private Coroutine restartCoroutine;
    #endregion

    #region Events
    // pause events
    public event Action Pause;
    public event Action Resume;
    // level events 
    public event Action LevelStarted;
    public event Action LevelEnded;
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
        // start the game with the start menu
        Restart(false);
    }


    private void StartGame()
    {
        // update game state
        CurrentState = GameState.StartMenu;
        // open start menu
        UIManager.Instance.OpenPopup<UIStartMenu>();
    }


    public void StartLevel()
    {
        // close all open popups
        UIManager.Instance.OpenPopup<UILevelHUD>(true);
        // update game state
        CurrentState = GameState.Level;
        LevelStarted?.Invoke();
        // subscribe to player death event
        Player.Died += GameOver;
    }

    public void PauseGame()
    {
        CurrentState = GameState.Paused;
        Pause?.Invoke();
        UIManager.Instance.OpenPopup<UIPauseMenu>();
    }

    public void ResumeGame()
    {
        CurrentState = GameState.Level;
        Resume?.Invoke();
    }

    public void GameOver()
    {
        LevelEnded?.Invoke();
        // open game over menu
        UIManager.Instance.OpenPopup<UIGameOverMenu>(true);
    }

    public void Restart(bool instantStart)
    {
        // check whether the restart coroutine is already running
        if (restartCoroutine != null) return;

        // start restart coroutine
        restartCoroutine = StartCoroutine(RestartCoroutine(instantStart));
    }

    private IEnumerator RestartCoroutine(bool instantStart)
    {
        // show loading screen
        UIManager.Instance.OpenPopup<UILoadingScreen>(true);
        // ensure visibility of loading screen
        yield return null;

        CurrentState = GameState.Loading;

        if (SceneManager.GetSceneByName("Game").isLoaded)
        {
            // unload existing game scene if necessary
            yield return SceneManager.UnloadSceneAsync("Game");
        }
        // load game scene
        yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);

        // wait a frame to ensure everything is initialized
        yield return null;

        if (instantStart)
        {
            // start level directly
            StartLevel();
        }
        else
        {
            // show main menu
            StartGame();
        }

        // mark coroutine as curently not running again
        restartCoroutine = null;
    }
}
