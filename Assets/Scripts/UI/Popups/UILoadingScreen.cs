using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : Popup
{
    [SerializeField] private List<Image> imagesDot;
    [SerializeField] private float dotAnimationInterval;


    private void Start()
    {
        // start dot animation
        StartCoroutine(DotAnimationCoroutine());
    }

    private IEnumerator DotAnimationCoroutine()
    {
        while (true)
        {
            // disable all dots
            foreach (var image in imagesDot)
            {
                image.enabled = false;
            }

            // wait before starting the animation cycle (again)
            yield return new WaitForSeconds(dotAnimationInterval);

            for (int i = 0; i < imagesDot.Count; i++)
            {
                // enable only the current dot
                imagesDot[i].enabled = true;
                // wait before enabling the next dot
                yield return new WaitForSeconds(dotAnimationInterval);
            }
        }
    }

}
