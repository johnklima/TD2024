using System;
using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MainMenu : MonoBehaviour
{
    public TMP_FontAsset font;
    public Transform mainCamera;
    public float cameraLerpSpeed = .1f;
    public float loadingScreenDuration = 10;
    public UnityEvent doneLoading = new();
    public Canvas canvas;
    public AudioClip uiButtonClickSound;

    public bool doLerp;
    private AudioSource _audioSource;

    private float floatAlpha = -.25f;

    private bool hasStarted;
    private Scene mainScene;
    private Vector3 start, end;
    private Quaternion startRot, endRot;

    private Transform targetCam;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        if (mainCamera == null) mainCamera = FindObjectOfType<Camera>().transform;
        start = mainCamera.position;
        startRot = mainCamera.rotation;

        var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
        var mainScene = SceneManager.LoadScene(2, parameters);

        var tmpTexts = FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (var tmpText in tmpTexts) tmpText.font = font;
        SetupAllButtonClickSound();
    }

    private void Update()
    {
        SetupTargetCam();

        if (!hasStarted) CursorLockHandler.ShowAndUnlockCursor();
        if (!doLerp) return;
        if (floatAlpha < 1) floatAlpha += Time.deltaTime * cameraLerpSpeed;
        if (floatAlpha < 0) return;

        mainCamera.position = Vector3.Lerp(start, end, floatAlpha);
        mainCamera.rotation = Quaternion.Lerp(startRot, endRot, floatAlpha);

        if (floatAlpha < 1) return;
        Debug.Log("Done loading: " + doneLoading.GetPersistentEventCount());

        doneLoading.Invoke();
        doLerp = false;
    }

    private void SetupAllButtonClickSound()
    {
        if (uiButtonClickSound == null) throw new Exception("No audio sound setup for UI buttons");
        var audioSource = FindObjectOfType<AudioSource>();
        _audioSource = audioSource.gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.clip = uiButtonClickSound;
        _audioSource.volume = audioSource.volume;


        var buttons = GetComponentsInChildren<Button>(true);
        foreach (var button in buttons) button.onClick.AddListener(_audioSource.Play);
    }

    private void SetupTargetCam()
    {
        if (targetCam == null)
        {
            var camBrain = FindObjectOfType<CinemachineBrain>();
            if (camBrain == null) return;
            targetCam = camBrain.transform;
            Debug.Log("setting targetCam: " + targetCam.position + " - " + targetCam.name);
            end = targetCam.position;
            endRot = targetCam.rotation;
        }
    }

    public void StartGame()
    {
        canvas.enabled = false;
        doLerp = true;
        hasStarted = true;
        // load loading screen additively, it automatically loads game scene 
        // deactivate canvas
        var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
        var scene = SceneManager.LoadScene(1, parameters);
        CursorLockHandler.HideAndLockCursor();
    }


    public void ExitGame()
    {
        StartCoroutine(TriggerExitGameDelayed());
    }

    private static IEnumerator TriggerExitGameDelayed()
    {
        Debug.Log("EXIT GAME");
        yield return new WaitForSecondsRealtime(1);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}