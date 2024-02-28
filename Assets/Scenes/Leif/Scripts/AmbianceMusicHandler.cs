using UnityEngine;

public class AmbianceMusicHandler : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource _audioSource;
    private AudioClip nextClip;

    private float timeForNextClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        nextClip = clips[0];
        _audioSource.clip = nextClip;
        timeForNextClip = nextClip.length;
        _audioSource.Play();
    }

    private void Update()
    {
        if (timeForNextClip <= 0)
            PlayRandomClip();
        timeForNextClip -= Time.deltaTime;
    }

    private void PlayRandomClip()
    {
        nextClip = clips[Random.Range(0, clips.Length)];
        _audioSource.clip = nextClip;
        Debug.Log($"Playing next music clip: {nextClip.name}, duration: {nextClip.length}");
        timeForNextClip = nextClip.length;
        _audioSource.Play();
    }
}