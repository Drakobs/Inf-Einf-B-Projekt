using System.Collections;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float parallaxAmount;
    [SerializeField] private Camera cam;

    private float startingPosition;
    private float spriteLength;


    private void Start()
    {
        //starting x position of middle sprite
        startingPosition = transform.position.x;
        //length of middle sprite
        spriteLength = spriteRenderer.bounds.size.x;
    }


    private void Update()
    {
        Vector3 position = cam.transform.position;
        float temp = position.x * (1 - parallaxAmount);
        float distance = position.x * parallaxAmount;

        Vector3 NewPosition = new Vector3(startingPosition + distance, transform.position.y, transform.position.z);

        transform.position = NewPosition;

        if (temp > startingPosition + (spriteLength / 2))
        {
            startingPosition += spriteLength;
        }
        else if (temp < startingPosition - (spriteLength / 2))
        {
            startingPosition -= spriteLength;
        }
    }
}