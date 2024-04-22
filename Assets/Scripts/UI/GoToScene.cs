using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    [SerializeField] string sceneName;

    // Метод для перехода на сцену Tutorial
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("Tutorial");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            LoadSceneByName(sceneName);
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        // Проверяем, существует ли сцена с таким именем
        if (SceneExists(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Сцена с именем \"" + sceneName + "\" не найдена. Проверьте правильность написания.");
        }
    }

    private bool SceneExists(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return false;
        }

        // Проверяем наличие сцены в списке загруженных сцен
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (sceneName == System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)))
            {
                return true;
            }
        }
        return false;
    }
}
