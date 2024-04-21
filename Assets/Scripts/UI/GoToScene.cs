using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Импортируем SceneManager

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
        SceneManager.LoadScene("Tutorial"); // Используем имя сцены, которую нужно загрузить
    }
}
