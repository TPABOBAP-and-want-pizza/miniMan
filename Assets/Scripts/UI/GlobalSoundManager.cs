using UnityEngine;

public class GlobalSoundManager : MonoBehaviour
{
    // Предположим, что все источники музыки имеют тэг "Music"
    public void ToggleSounds(bool enable)
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (!audioSource.CompareTag("Music1"))
            {
                audioSource.mute = enable;
            }
        }
    }
}
