using UnityEngine;
using UnityEngine.UI;

public class UIStartMenu : Popup
{
    #region Popup Methods
    #endregion

    public void OnClickStart()
    {
        GameManager.Instance.StartLevel();
    }
}
