using System.Collections.Generic;
using UnityEngine;

public class EnvironmentParallaxLayer : MovementLayer
{
    [SerializeField] private float parallaxAmount;

    protected override void Move()
    {
        var movementVector = new Vector3(map.MovementSpeed  * Time.deltaTime * parallaxAmount, 0f, 0f);
        Move(movementVector);
    }
}
