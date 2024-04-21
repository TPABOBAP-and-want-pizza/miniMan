using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public void LoadLobbyScene()
    {

        SceneManager.LoadScene("LoadindScreen", LoadSceneMode.Single); 

        SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);

        
    }
}