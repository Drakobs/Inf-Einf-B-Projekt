using UnityEngine;

public class PauseMenu : Popup
{
    public void OnClickResume()
    {
        GameManager.Instance.ResumeGame();
        Close();
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
