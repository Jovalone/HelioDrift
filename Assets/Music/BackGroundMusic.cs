using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    public int TrackSelector;
    public int TrackHistory = -1;

    void Start()
    {
        SelectRandomTrack();
    }

    void Update()
    {
        if(audioSource.isPlaying == false)
        {
            SelectRandomTrack();
        }
    }

    void SelectRandomTrack()
    {
        TrackSelector = Random.Range(0, audioClips.Length);

        if (TrackSelector != TrackHistory)
        {
            audioSource.clip = audioClips[TrackSelector];
            TrackHistory = TrackSelector;
            audioSource.Play();
        }
    }
}
