using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioController : MonoBehaviour
{
    public AudioClip deathAudioClip, healAudioClip, walkAudioClip, walkIndoorAudioClip;
    public AudioClip[] painAudioClips;
    private PlayerHealthSystem _playerHealthSystem;
    private PlayerInput _playerInput;
    private AudioSource _walkAudioSource, _extraAudioSource;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();

        _walkAudioSource = GetComponent<AudioSource>();
        _extraAudioSource = gameObject.AddComponent<AudioSource>();

        _walkAudioSource.playOnAwake = false;
        _extraAudioSource.playOnAwake = false;


        _walkAudioSource.clip = walkAudioClip;

        _playerHealthSystem = GetComponent<PlayerHealthSystem>();
        _playerHealthSystem.Die.AddListener(PlayDieAudioClip);
        _playerHealthSystem.onHeal.AddListener(PlayHealAudioClip);
        _playerHealthSystem.onDamage.AddListener(PlayDamageAudioClip);
    }

    private void Update()
    {
        WalkAudio();
    }

    private void PlayDamageAudioClip()
    {
        _extraAudioSource.Stop();
        var rand = Random.Range(0, painAudioClips.Length);
        _extraAudioSource.clip = painAudioClips[rand];
        _extraAudioSource.Play();
    }

    private void PlayHealAudioClip()
    {
        _extraAudioSource.Stop();
        _extraAudioSource.clip = healAudioClip;
        _extraAudioSource.Play();
    }

    private void PlayDieAudioClip()
    {
        _extraAudioSource.Stop();
        _extraAudioSource.clip = deathAudioClip;
        _extraAudioSource.Play();
    }

    public void WalkAudio()
    {
        if (_playerInput.IsWalking)
            _walkAudioSource.Play();
        if (_playerInput.IsWalking)
            _walkAudioSource.Pause();
    }
}