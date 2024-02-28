using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public UnityEvent OnUiButtonClicked;
    public AudioClip buttonPressAudioClip;
    private AudioSource _audioSource;

    private void Start()
    {
        if (buttonPressAudioClip == null) throw new Exception("No audio sound setup for UI buttons");

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.clip = buttonPressAudioClip;
        var buttons = FindObjectsOfType<Button>(true);

        foreach (var button in buttons) button.onClick.AddListener(ButtonClick);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void ButtonClick()
    {
        OnUiButtonClicked.Invoke();
        _audioSource.Play();
    }
}