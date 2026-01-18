using UnityEngine;
using System.Collections;

public abstract class Popup : MonoBehaviour
{
    public void Open() {}

    public void Close()
    {
        Destroy(this);
    }
}