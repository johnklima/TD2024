using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct Meshes
{
    [Header("Do not touch unless already broken")] [Tooltip("Prefab for the potion - do not touch")]
    public GameObject potion;

    [Tooltip("Prefab for the ingredient - do not touch")]
    public GameObject ingredient;
}

[Serializable]
public struct GizmoSettings
{
    public bool on, onlyWhenSelected;
}


public class Interactable : MonoBehaviour
{
    [Tooltip("Settings for gizmos")] public GizmoSettings gizmoSettings;

    [Tooltip("Pre-made object from 'Interactable Items' folder")]
    public BaseItem preMadeItem;

    [Tooltip("Enable to test events added to the TESTER gameObject")]
    public bool useTester = true;

    [Tooltip("The layer to interact with")]
    public LayerMask interactableLayerMask;

    [Tooltip("What key to press to trigger interaction")]
    public KeyCode key = KeyCode.E;

    [Tooltip("How far can away is item pickUpAble")]
    public float pickUpRadius = 5f;

    [Tooltip("Player controller camera, defaults to: Camera.main")]
    public Camera playerCamera;

    [Tooltip("Size and position of the triggerBox")]
    public TriggerBoxData triggerBoxData = new();

    [Tooltip("Events")] public InteractionEvents interactionEvents;

    [Tooltip("Do not alter unless already broken")] [SerializeField]
    public Meshes prefabData;

    public InteractableCamera interactableCamera;

    private RaycastHit _hit;

    // private Transform _playerCameraTransform;
    private bool _playerInRange;

    private GameObject _potion, _ingredient;


    private TriggerBox _triggerBox;

    private void Awake()
    {
        interactableCamera = InteractableManager.instance.Register(this);

        // ValidatePlayerCamera();
        // ValidateInteractableCamera();
        ValidateLayers();
        ValidateScriptableObject();
    }


    private void Start()
    {
        //* if pickUpRadius is greater than 0 (indicating that we want to use it)
        //* add a sphereCollider and set its values
        if (pickUpRadius > 0)
        {
            var sphere = gameObject.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = pickUpRadius;
        }
    }

    private void Update()
    {
        //* we shoot a ray from the camera
        //* it returns null or interactable
        //* interactable here is TriggerBox

        //* if:
        //* player is in range, and
        //* useKey is enabled, and
        //* useRaycast is enabled, and
        //* user presses <key>, and
        //* interactableCamera is looking at <I_Interactable>
        //* then:
        //* Interact() with interactable (TriggerBox)

        if (!_playerInRange) return;

        if (!Input.GetKeyDown(key)) return;
        if (!interactableCamera.TryDoRay(out var interactable)) return;
        interactable.Interact();
    }

    private void OnDrawGizmos()
    {
        if (!gizmoSettings.on) return;
        if (gizmoSettings.onlyWhenSelected) return;

        DrawGizmos();
    }

    private void OnDrawGizmosSelected()
    {
        if (!gizmoSettings.on) return;
        if (!gizmoSettings.onlyWhenSelected) return;
        DrawGizmos();
    }

    private void OnTriggerEnter(Collider other)
    {
        //todo check for player correctly
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        _playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //todo check for player correctly
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        _playerInRange = false;
    }


    private void OnValidate()
    {
        _triggerBox ??= GetComponentInChildren<TriggerBox>();
        triggerBoxData ??= new TriggerBoxData();
        if (triggerBoxData != null)
            _triggerBox.UpdateTriggerBox(this);
        ValidateScriptableObject();
    }

    private void DrawGizmos()
    {
        var position = transform.position;
        Gizmos.DrawWireSphere(position, pickUpRadius);
        var pos = position + triggerBoxData.localPos;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos, triggerBoxData.size);
    }


    private void ValidateScriptableObject()
    {
        if (!isActiveAndEnabled) return;
        if (preMadeItem == null) return;
        var i = prefabData.ingredient;
        var p = prefabData.potion;
        if (i == null || p == null)
            throw new Exception($"BROKEN: potion: {p} or ingredient: {i} - are not set correctly!");
        StartCoroutine(DoAtEndOfFrame(i, p));
        gameObject.name = preMadeItem.name;
    }

    private IEnumerator DoAtEndOfFrame(GameObject ingredient, GameObject potion)
    {
        var iType = preMadeItem.itemType;
        yield return new WaitForEndOfFrame();
        ingredient.SetActive(iType == ItemType.Ingredient);
        potion.SetActive(iType == ItemType.Potion);
    }

    private void ValidateLayers()
    {
        //* set all children (not grand-children) to ignore raycast
        gameObject.layer = 2;
        foreach (Transform tr in transform)
            tr.gameObject.layer = 2;
        //* set triggerBox to interactable
        _triggerBox.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
}