using System.Collections;
using TMPro;
using UnityEngine;

public class UINumberAnimator : MonoBehaviour
{
    [SerializeField] private TMP_Text labelNumber;
    [SerializeField] private float animationDuration;

    private int currentValue;
    private Coroutine animationCoroutine;

    /// <summary>
    /// Sets the displayed numeric value, optionally using an animation.
    /// </summary>
    /// <param name="value">the numeric value to display</param>
    /// <param name="animate">true to animate the value change using default animation settings; false to update the value immediately</param>
    public void SetValue(int value, bool animate = true)
    {
        if (!animate)
        {
            // immediately set the value without animation
            labelNumber.text = value.ToString();
            return;
        }

        SetValue(value, this.animationDuration);
    }

    /// <summary>
    /// Animates a transition to the given value
    /// </summary>
    /// <param name="value">value to animate to</param>
    /// <param name="animationDuration">duration of the aniation</param>
    public void SetValue(int value, float animationDuration)
    {
        if (animationCoroutine != null)
        {
            // stop active animation coroutine
            StopCoroutine(animationCoroutine);
        }
        // start new animation coroutine
        animationCoroutine = StartCoroutine(AnimationCoroutine(value, animationDuration));
    }

    /// <summary>
    /// Animates the numeric label from its current value to the specified target value over the given duration.
    /// </summary>
    /// <param name="targetValue">the final integer value to display at the end of the animation.</param>
    /// <param name="duration">the duration, in seconds, over which the animation occurs.</param>
    private IEnumerator AnimationCoroutine(int targetValue, float duration)
    {
        // save starting value
        int startValue = currentValue;
        // update displayed value every frame over the duration
        for (float elapsedTime = 0f; elapsedTime < duration; elapsedTime += Time.deltaTime)
        {
            // calculate current progress
            float t = Mathf.Clamp01(elapsedTime / duration);
            // calculate current interpolated value based on progress
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, t));
            // update label text
            labelNumber.text = currentValue.ToString();
            // wait until next frame
            yield return null;
        }
        // set final value
        labelNumber.text = targetValue.ToString();
    }
}
