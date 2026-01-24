using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] private Transform _anchorStart;
    [SerializeField] private Transform _anchorEnd;

    public Transform AnchorStart { get { return _anchorStart; } }
    public Transform AnchorEnd { get { return _anchorEnd; } }
}
