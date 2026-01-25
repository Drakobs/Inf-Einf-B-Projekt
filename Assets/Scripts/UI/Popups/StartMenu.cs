using UnityEngine;
using UnityEngine.UI;

public class StartMenu : Popup
{
    #region Popup Methods
    #endregion

    public void OnClickStart()
    {
        GameManager.Instance.StartLevel();
    }
}
