using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load scene using name, or reload the active scene
/// </summary>
public class LoadScene : MonoBehaviour
{
    public string sceneName;
    public float delay = 5f;
    
    public void LoadSceneUsingName()
    {
        StartCoroutine(TransitionScene());
    }

    IEnumerator TransitionScene()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);

    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
