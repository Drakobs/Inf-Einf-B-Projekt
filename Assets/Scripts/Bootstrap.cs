using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private string persistentSceneName;
    [SerializeField] private string gameSceneName;

    private IEnumerator Start()
    {
        // load persistent scene
        yield return SceneManager.LoadSceneAsync(persistentSceneName, LoadSceneMode.Additive);

        // wait for UIManager to be initialized
        yield return new WaitUntil(() => GameManager.Instance != null);

        // unload bootstrap scene
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}

