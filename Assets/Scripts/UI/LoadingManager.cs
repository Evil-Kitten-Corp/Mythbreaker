using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadSceneAsync("Combat"));
    }

    IEnumerator LoadSceneAsync(string sceneId)
    {
        yield return new WaitForSeconds(1);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
