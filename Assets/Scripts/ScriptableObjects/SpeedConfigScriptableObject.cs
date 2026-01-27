using UnityEngine;

[CreateAssetMenu(fileName = "SpeedScriptableObject", menuName = "Scriptable Objects/SpeedConfig")]
public class SpeedConfigScriptableObject : ScriptableObject
{
    [Tooltip("Initial movement speed")]
    public float startSpeed;
    [Tooltip("Final movement speed")]
    public float endSpeed;

    [Space(10)]
    [Tooltip("Distance threshold in units on which the end speed should be reached")]
    public float endSpeedThreshold;

    [Space(10)]
    [Tooltip("Defines a curve that is used to control the growth in speed over distance")]
    public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public float GetSpeedAtDistance(float distance)
    {
        // check whether that max speed has been reached
        if (distance >= endSpeedThreshold)
        {
            return endSpeed;
        }

        // calculate speed based on curve
        float t = distance / endSpeedThreshold;
        float curveValue = speedCurve.Evaluate(t);
        return Mathf.Lerp(startSpeed, endSpeed, curveValue);
    }
}
