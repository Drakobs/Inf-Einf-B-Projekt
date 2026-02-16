using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UILevelHUD : Popup
{
    [SerializeField] private TMP_Text labelDistance;

    private Map map;
    private bool isPaused;

    private void Start()
    {
        isPaused = GameManager.Instance.CurrentState == GameManager.GameState.Paused;
        map = GameManager.Instance.Map;

        // subscribe to GameManager events
        GameManager.Instance.Pause += OnPause;
        GameManager.Instance.Resume += OnResume;

        // set initial distance
        labelDistance.text = "0";
    }

    private void Update()
    {
        if (isPaused) return;

        // update distance label
        int movedDistance = (int)map.MovedDistance;
        labelDistance.text = movedDistance.ToString();
    }

    private void OnDestroy()
    {
        // unsubscribe from GameManager events
        GameManager.Instance.Pause -= OnPause;
        GameManager.Instance.Resume -= OnResume;
    }

    #region Event Methods
    private void OnPause()
    {
        isPaused = true;
    }

    private void OnResume()
    {
        isPaused = false;
    }
    #endregion

    public void OnClickPause()
    {
        GameManager.Instance.PauseGame();
    }
}
