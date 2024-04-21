using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public Toggle musicToggle;

    private void Start()
    {
        if (musicToggle != null)
        {
            musicToggle.onValueChanged.AddListener(OnToggleMusic);
        }
    }

    private void OnToggleMusic(bool isMusicOn)
    {
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.ToggleMusic(isMusicOn);
        }
    }
}
