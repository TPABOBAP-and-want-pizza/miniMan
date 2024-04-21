using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_toggle : MonoBehaviour
{
    [SerializeField] AudioSource music;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMusic()
    {
        music.Play();
    }

    public void OffMusic()
    {
        music.Stop();
    }
}
