using UnityEngine;

public class StartSection : Section
{
    [SerializeField] private Transform _cameraAnchor;
    public Transform AnchorCamera { get { return _cameraAnchor; } }
}
