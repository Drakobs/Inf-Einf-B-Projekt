using UnityEngine;

public class UIGameOverMenu : Popup
{
    [Header("GameOver Menu Settings")]
    [SerializeField] private UINumberAnimator scoreAnimator;

    private void Start()
    {
        // animate the score display
        int finalScore = (int)GameManager.Instance.Map.MovedDistance;
        scoreAnimator.SetValue(finalScore);
    }
    public void OnClickRestart()
    {
        GameManager.Instance.Restart(true);
    }

    public void OnClickMenu()
    {
        GameManager.Instance.Restart(false);
    }
}
