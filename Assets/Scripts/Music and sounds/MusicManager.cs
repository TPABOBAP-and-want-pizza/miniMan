using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public AudioClip[] musicTracks;
    private AudioSource audioSource;
    private bool isMusicPlaying = true;
    private Coroutine musicCoroutine;  // Ссылка на корутину

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (musicTracks.Length > 0)
        {
            musicCoroutine = StartCoroutine(PlayMusic());
        }
        else
        {
            Debug.LogError("No music tracks found for MusicManager.");
        }
    }

    private IEnumerator PlayMusic()
    {
        while (isMusicPlaying)
        {
            AudioClip randomTrack = musicTracks[Random.Range(0, musicTracks.Length)];
            audioSource.clip = randomTrack;
            audioSource.Play();
            yield return new WaitForSeconds(randomTrack.length);
        }
    }

    public void ToggleMusic(bool isMusicOn)
    {
        isMusicPlaying = isMusicOn;
        if (!isMusicOn)
        {
            if (musicCoroutine != null)
            {
                StopCoroutine(musicCoroutine);
                musicCoroutine = null;
            }
            audioSource.Stop();
        }
        else
        {
            if (musicCoroutine == null)
            {
                musicCoroutine = StartCoroutine(PlayMusic());
            }
        }
    }

    // Добавленный метод для проверки состояния музыки
    public bool IsMusicPlaying()
    {
        return isMusicPlaying;
    }


}
