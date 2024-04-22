using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Метод для перехода на сцену Tutorial
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("Tutorial");
    }

    // Метод для перехода на сцену, имя которой передается как аргумент
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

    // Вспомогательный метод для проверки существования сцены
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
