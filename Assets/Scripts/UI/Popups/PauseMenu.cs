using UnityEngine;

public class PauseMenu : Popup
{
    public void OnClickResume()
    {
        GameManager.Instance.ResumeGame();
        Close();
    }
}
