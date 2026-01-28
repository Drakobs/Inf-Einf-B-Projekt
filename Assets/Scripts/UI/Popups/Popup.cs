using UnityEngine;
using System.Collections;

public abstract class Popup : MonoBehaviour
{
    public virtual void Open() {}

    public void Close()
    {
        Destroy(gameObject);
    }
}