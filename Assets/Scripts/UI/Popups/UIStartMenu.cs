using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UIStartMenu : Popup
{
    [SerializeField] private TMP_Text labelBlinking;
    [SerializeField] private float blinkDuration;

    [Header("Score Displays")]
    [SerializeField] private TMP_Text labelLastScore;
    [SerializeField] private TMP_Text labelHighScore;

    [Header("Version Display")]
    [SerializeField] private TMP_Text labelVersion;

    public void Start()
    {
        StartCoroutine(Blink());

        // display saved scores
        labelLastScore.text = $"{PlayerPrefs.GetInt("LastScore", 0)}";
        labelHighScore.text = $"{PlayerPrefs.GetInt("HighScore", 0)}";

        labelVersion.text = $"v.{Application.version}";
    }

    public void OnClickStart()
    {
        GameManager.Instance.StartLevel();
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator Blink()
    {
        float elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            // keep elapsed time between 0 and blinkDuration
            elapsedTime %= blinkDuration;
            // caluclate input for sinus function to complete one full cycle in blinkDuration seconds
            float sinusInput = (elapsedTime / blinkDuration) * Mathf.PI * 2f;
            // calculate alpha using a normalized sinus value
            float normalizedSinusValue = (Mathf.Sin(sinusInput) + 1f) / 2f;
            // calculate alpha value between 0 and 1
            float alpha = Mathf.Lerp(0f, 1f, normalizedSinusValue);
            // set alpha value
            labelBlinking.color = new Color(labelBlinking.color.r, labelBlinking.color.g, labelBlinking.color.b, alpha);
            // wait for next frame
            yield return null;
        }
    }
}
