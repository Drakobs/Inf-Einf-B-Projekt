using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class Popup : MonoBehaviour
{
    [Header("Popup Default Settings")]
    [SerializeField] protected GameObject defaultSelectedElement;

    public virtual void Open() 
    {
        if (defaultSelectedElement != null)
        {
            StartCoroutine(SelectFirstElement(defaultSelectedElement));
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Selects the first element in the popup
    /// </summary>
    protected IEnumerator SelectFirstElement(GameObject elementToSelect)
    {
        // wait one frame to ensure that the UI elements are properly initialized
        yield return null;

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(elementToSelect);
    }
}