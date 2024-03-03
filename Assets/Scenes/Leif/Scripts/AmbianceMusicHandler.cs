using Unity.Collections;
using UnityEngine;

public class AmbianceMusicHandler : MonoBehaviour
{
    public AudioClip[] clips;
    [ReadOnly] public AudioClip currentClip;
    [ReadOnly] public float nextClipIn;
    private AudioSource _audioSource;
    private AudioClip nextClip;
    private float timeForNextClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        PlayRandomClip();
    }


    private void Update()
    {
        if (timeForNextClip <= 0)
            PlayRandomClip();
        timeForNextClip -= Time.deltaTime;
        nextClipIn = (int)timeForNextClip;
    }

    private void PlayRandomClip()
    {
        nextClip = clips[Random.Range(0, clips.Length)];
        _audioSource.clip = nextClip;
        currentClip = nextClip;
        Debug.Log($"Playing next music clip: {nextClip.name}, duration: {(int)nextClip.length}");
        timeForNextClip = (int)nextClip.length - 3;
        _audioSource.Play();
    }

    public void NextClip()
    {
        PlayRandomClip();
    }
}