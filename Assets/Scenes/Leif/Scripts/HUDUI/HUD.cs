using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public UnityEvent OnUiButtonClicked;
    [Range(0, 1f)] public float volume = .5f;
    public AudioClip buttonPressAudioClip, hotBarItemDragAudioClip;
    private AudioSource _audioSource;

    private void Start()
    {
        if (buttonPressAudioClip == null) throw new Exception("No audio sound setup for UI buttons");

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.volume = volume;

        var buttons = FindObjectsOfType<Button>(true);

        foreach (var button in buttons) button.onClick.AddListener(ButtonClick);
    }

    private void ButtonClick()
    {
        PlaySound(buttonPressAudioClip);
        OnUiButtonClicked.Invoke();
    }

    public void ItemDragged()
    {
        PlaySound(hotBarItemDragAudioClip);
        OnUiButtonClicked.Invoke();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) throw new Exception(clip + " clip not found");
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}