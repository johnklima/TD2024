using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class ThrowingHandler : MonoBehaviour
{
    private static readonly int ThrowAnimTrigger = Animator.StringToHash("Throw");
    public float aimCoolDown;
    public LineRendererSettings lineRendererSettings;

    public UnityEvent<DraggableItem> OnThrowing = new(); //! autoListener: InventoryController.RemoveItem 

    public float testAngle;

    public float throwDelay = 1f;
    private bool _aimPrevFram, _fire;
    private Animator _animator;
    private bool _canAim = true;

    //TODO reduce count in UI/inventories
    private CannonBall1 _cannonBall;
    private Transform _doRayCam;

    private RaycastHit _hit;
    private InventoryController _inventoryController;
    private InventoryDisplay _inventoryDisplay;
    private ItemManager _itemManager;
    private LineRenderer _lineRenderer;
    private Transform _mainCam;
    private GameObject _newThrowable;


    private float _offsetAlpha;
    private PlayerController _playerController;
    private PlayerInput _playerInput;
    private IEnumerator coroutine;

    private bool isLoadedAdditively;

    private void Start()
    {
        var mainMenu = FindObjectOfType<MainMenu>();
        if (mainMenu != null) isLoadedAdditively = true;
        if (isLoadedAdditively) return;

        _inventoryDisplay = FindObjectOfType<InventoryDisplay>();
        if (_inventoryDisplay == null) ThrowError("Make sure there is a <InventoryDisplay> in the scene");

        _playerInput = FindObjectOfType<PlayerInput>();
        if (_playerInput == null) ThrowError("Make sure there is a <PlayerInput> in the scene");

        _playerController = GetComponentInParent<PlayerController>();
        if (_playerController == null)
            ThrowError("Make sure there is a <PlayerController> in the scene");
        _inventoryController = GetComponentInParent<InventoryController>();
        if (_inventoryController == null)
            ThrowError("Make sure there is a <InventoryController> in the scene");

        _itemManager = FindObjectOfType<ItemManager>();
        if (_itemManager == null)
            ThrowError("Make sure there is a <ItemManager> in the scene");

        OnThrowing.AddListener(_inventoryController.RemoveItem);

        _animator = GetComponentInParent<Animator>();


        SetupLineRenderer();
        _canAim = true;
        if (Camera.main != null)
            _mainCam = Camera.main.transform;
        else ThrowError("No camera set as MainCamera");
    }

    private void Update()
    {
        if (isLoadedAdditively) return;
        if (_newThrowable != null) _newThrowable.transform.position = transform.position;
        if (!PlayerInput.playerHasControl) return;
        var selectedItem = _inventoryDisplay.selectedItem;
        if (selectedItem == null) return;

        // if we dont have selected item, we have nothing to throw
        // else we do ray,
        var didRayHit = DoRay(out _hit);
        var canMoveAndAim = _playerController.canWalkWhileAiming;
        // check if player was holding mouse1 previous frame, and released this frame
        if (_aimPrevFram && Input.GetMouseButtonUp(0)) //! order matters #1 
        {
            //set fire to true, if we hit something with raycast
            _fire = didRayHit;
        }
        else if (_aimPrevFram && Input.GetMouseButton(1)) //! order matters #2
        {
            // player cancel aim
            _aimPrevFram = false;
            if (!canMoveAndAim)
                _playerInput.SetPlayerInputState(true);
            StartCoroutine(AimCoolDown(aimCoolDown / 2f));
        }

        // if player is not in cooldown and presses mouse down this frame
        if (_canAim && Input.GetMouseButtonDown(0)) //! order matters #3
        {
            // player is aiming
            _aimPrevFram = true;
            if (!canMoveAndAim)
                _playerInput.SetPlayerInputState(false);
        }

        // if everything up to now says we can fire, set the wheels in motion!
        if (_fire) //! order matters #4
        {
            //do fire
            _fire = false;
            _animator.SetTrigger(ThrowAnimTrigger);
            ExecuteThrow(selectedItem);
        }

        _lineRenderer.enabled = _aimPrevFram;

        if (!_aimPrevFram) return; //! order matters #5
        // do aim
        var pulseFreq = lineRendererSettings.pulseFreq;
        _offsetAlpha += Time.deltaTime * pulseFreq;
        if (_offsetAlpha >= 100) _offsetAlpha = 0;
        _lineRenderer.sharedMaterial.mainTextureOffset = new Vector2(_offsetAlpha, 0);
        var res = _lineRenderer.positionCount;
        for (var i = 0; i < res; i++)
            if (_hit.point != Vector3.zero)
            {
                var pos = transform.position;
                var dst = Vector3.Distance(pos, _hit.point);
                pos += lineRendererSettings.startPosOffset;
                pos = Vector3.Lerp(pos, _hit.point, i / (float)res);
                pos.y += Mathf.Sin(i * lineRendererSettings.sineModA) * (dst * lineRendererSettings.sineModB);
                _lineRenderer.SetPosition(i, pos);
            }
    }


    private void OnValidate()
    {
        if (isLoadedAdditively) return;

        SetupLineRenderer();
        var res = _lineRenderer.positionCount;
        for (var i = 0; i < res; i++)
        {
            var pos = Vector3.Lerp(
                lineRendererSettings.testStart,
                lineRendererSettings.testEnd,
                i / (float)res);
            pos.y += Mathf.Sin(i * lineRendererSettings.sineModA) * lineRendererSettings.sineModB;
            _lineRenderer.SetPosition(i, pos);
        }
    }

    private void ThrowError(string e)
    {
        Debug.Log("Disabling: " + name);
        enabled = false;
        throw new Exception(e);
    }

    private void ExecuteThrow(DraggableItem selectedItem)
    {
        _playerInput.DisablePlayerInputForDuration(aimCoolDown);
        _canAim = _aimPrevFram = false; // exit next frame
        StartCoroutine(AimCoolDown()); // set cooldown
        var throwable = _itemManager.GetActiveGameObject(selectedItem.item);
        if (throwable == null) ThrowError("Something wrong");
        _newThrowable = Instantiate(throwable); // make object
        //TODO CHANGE THING
        var item = _newThrowable.GetComponent<Item>();
        item.isInteractionOneShot = true; // thrown items can only be picked back up again
        item.rigidbody.isKinematic = false;
        // item itself handles breaking on collision
        _newThrowable.SetActive(true); // make sure its active
        _newThrowable.transform.position = transform.position; // move to hand pos
        var newCannon = _newThrowable.AddComponent<CannonBall1>(); // add john physics
        StartCoroutine(LaunchDelayed(newCannon, _hit.point, testAngle)); // launch
        OnThrowing.Invoke(selectedItem); // update hotBar
    }

    private IEnumerator LaunchDelayed(CannonBall1 cannon, Vector3 target, float angle)
    {
        yield return new WaitForSeconds(throwDelay);
        _newThrowable = null;
        cannon.Launch(target, angle); // launch
    }

    private void SetupLineRenderer()
    {
        if (_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.material = lineRendererSettings.lineRendererMaterial;
        var res = _lineRenderer.positionCount = lineRendererSettings.resolution;
    }


    private IEnumerator AimCoolDown(float t = 0)
    {
        if (t == 0) t = aimCoolDown;
        yield return new WaitForSeconds(t);
        _animator.ResetTrigger(ThrowAnimTrigger);
        _canAim = true;
    }

    private bool DoRay(out RaycastHit hit)
    {
        if (_doRayCam == null)
            _doRayCam = Camera.main.transform;
        if (_doRayCam == null)
            ThrowError("Cannot find Camera.Main, make sure there is always one active Camera.main");
        var ray = new Ray(_doRayCam.position, _doRayCam.forward);
        return Physics.Raycast(ray, out hit, 1000);
    }
}

public static class ExtensionMethods
{
    public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}