using UnityEngine;

public class GameOverMenu : Popup
{
    public void OnClickRestart()
    {
        GameManager.Instance.Restart(true);
    }

    public void OnClickMenu()
    {
        GameManager.Instance.Restart(false);
    }
}
