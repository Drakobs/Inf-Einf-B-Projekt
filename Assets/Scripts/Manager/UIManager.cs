using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private string popupsResourcesPath;
    [SerializeField] private Transform popupsContainer;
    
    private List<Popup> popups = new List<Popup>();


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

    #region Popup Management
    /// <summary>
    /// Loads and opens a popup
    /// </summary>
    /// <typeparam name="PopupType">Type of the popup to open</typeparam>
    /// <param name="closeOtherPopups">Whether to close all other popups when opening this one</param>
    /// <returns></returns>
    public PopupType OpenPopup<PopupType>(bool closeOtherPopups = false)
        where PopupType : Popup
    {
        if (closeOtherPopups)
        {
            CloseAllPopups();
        }

        // load popup prefab
        PopupType popupPrefab = Resources.Load<PopupType>($"{popupsResourcesPath}/{typeof(PopupType).Name}");
        if (popupPrefab == null)
        {
            // popup not found
            Debug.LogWarning($"Popup couldn't be found: {typeof(PopupType).Name}");
            return null;
        }
        
        // instantiate popup
        PopupType popup = Instantiate(popupPrefab, popupsContainer);
        // open the instantiated popup
        popup.Open();
        
        return popup;
    }

    public void CloseAllPopups()
    {
        popups.Clear();
        foreach (Transform child in popupsContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ClosePopup(Popup popup)
    {
        popups.Remove(popup);
        popup.Close();
    }
    #endregion
}
