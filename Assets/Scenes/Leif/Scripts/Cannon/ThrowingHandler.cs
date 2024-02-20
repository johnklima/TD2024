using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class LineRendererSettings
{
    public int resolution = 10;
    public Material lineRendererMaterial;
    public float sineModA = 1, sineModB = 1;
    public float pulseFreq = 1;
    public Vector3 testStart = new(0, 0, 0);
    public Vector3 testEnd = new(0, 0, 0);
    public Vector3 startPosOffset = new(0, 0, 0);
}

[RequireComponent(typeof(LineRenderer))]
public class ThrowingHandler : MonoBehaviour
{
    public float aimCoolDown;
    public LineRendererSettings lineRendererSettings;

    public UnityEvent<DraggableItem> OnThrowing = new(); //! listener: InventoryController.RemoveItem 

    public float testAngle;
    private bool _aimPrevFram, _fire;
    private bool _canAim = true;

    //TODO reduce count in UI/inventories
    private CannonBall1 _cannonBall;
    private Transform _doRayCam;

    private RaycastHit _hit;
    private InventoryController _inventoryController;
    private LineRenderer _lineRenderer;
    private Transform _mainCam;
    private PlayerController _playerController;
    private InventoryDisplay _uIInventoryDisplay;

    private float offsetAlpha;

    private void Start()
    {
        _uIInventoryDisplay = FindObjectOfType<InventoryDisplay>();
        if (_uIInventoryDisplay == null) throw new Exception("Make sure there is a <UIInventoryManager> in the scene");
        _playerController = GetComponentInParent<PlayerController>();
        _inventoryController = GetComponentInParent<InventoryController>();
        if (_inventoryController == null)
            throw new Exception("Make sure there is a <InventoryController> in the scene");
        OnThrowing.AddListener(_inventoryController.RemoveItem);

        SetupLineRenderer();
        _canAim = true;
        if (Camera.main != null)
            _mainCam = Camera.main.transform;
        else throw new Exception("No camera set as MainCamera");
    }

    private void Update()
    {
        var selectedItem = _uIInventoryDisplay.selectedItem;
        if (selectedItem == null) return;
        // if we dont have selected item, we have nothing to throw
        // else we do ray,
        var didRayHit = DoRay(out _hit);
        if (Input.GetMouseButtonUp(0)) Debug.Log("VAR");
        // check if player was holding mouse1 previous frame, and released this frame
        if ((_aimPrevFram && Input.GetMouseButtonUp(0)) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            //set fire to true, if we hit something with raycast
            _fire = didRayHit;
        }
        else if (_aimPrevFram && Input.GetMouseButton(1))
        {
            // cancel aim
            _aimPrevFram = false;
            StartCoroutine(AimCoolDown(aimCoolDown / 2f)); //todo
        }

        // if player is not in cooldown and presses mouse down this frame
        if (_canAim && Input.GetMouseButtonDown(0))
            _aimPrevFram = true;

        if (_fire)
        {
            Debug.Log("fire");
            //do fire
            _fire = _aimPrevFram = false; // exit next frame
            var newThrowable = Instantiate(selectedItem.item.GetActiveGameObject()); // make object
            var i = newThrowable.GetComponent<Item>();
            i.isOneShot = true; // thrown items can only be picked back up again
            // item itself handles breaking on collision
            newThrowable.SetActive(true); // make sure its active
            newThrowable.transform.position = transform.position; // move to hand pos
            var newCannon = newThrowable.AddComponent<CannonBall1>(); // add john physics
            newCannon.Launch(_hit.point, testAngle); // launch
            OnThrowing.Invoke(selectedItem); // update hotBar
            StartCoroutine(AimCoolDown()); // set cooldown
            Debug.Log("fire2");
        }

        // do aim
        _lineRenderer.enabled = _aimPrevFram;
        if (!_aimPrevFram) return;
        var pulseFreq = lineRendererSettings.pulseFreq;
        offsetAlpha += Time.deltaTime * pulseFreq;
        if (offsetAlpha >= 100) offsetAlpha = 0;
        _lineRenderer.sharedMaterial.mainTextureOffset = new Vector2(offsetAlpha, 0);
        var res = _lineRenderer.positionCount;
        for (var i = 0; i < res; i++)
            if (_hit.point != Vector3.zero)
            {
                var tPos = transform.position + lineRendererSettings.startPosOffset;
                var pos = Vector3.Lerp(tPos, _hit.point, i / (float)res);
                pos.y += Mathf.Sin(i * lineRendererSettings.sineModA) * lineRendererSettings.sineModB;

                // var normalizedI = ExtensionMethods.Remap(i, 0, res, 0, 1);
                // if (normalizedI > .5f) normalizedI = 1 - normalizedI;
                // pos.y += Vector3.Distance(tPos, _hit.point) * normalizedI;
                _lineRenderer.SetPosition(i, pos);
            }
    }

    private void OnValidate()
    {
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
        _canAim = true;
    }

    private bool DoRay(out RaycastHit hit)
    {
        if (_doRayCam == null)
            _doRayCam = Camera.main.transform;
        if (_doRayCam == null)
            throw new Exception("Cannot find Camera.Main, make sure there is always one active Camera.main");
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