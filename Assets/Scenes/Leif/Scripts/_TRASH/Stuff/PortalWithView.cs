using System;
using UnityEngine;

public class PortalWithView : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PortalTrigger portalTrigger;
    [SerializeField] private PortalTrigger exitTrigger;
    [SerializeField] private Vector3 portalBoundsSize;
    [SerializeField] private Camera entranceCamera, exitCamera, mainCam;
    private Transform _entrancePortalTransform, _exitPortalTransform;
    private Vector3 _mainCamLocalPosition, _mainCamPosition, _mainCamRotationEuler;

    private Transform _mainCamTransform;

    private PortalSide _playerAtEntrance;
    private Vector3 _playerPosition, _playerRotationEuler;

    private BoxCollider _portalBoundsTrigger;

    private void Awake()
    {
    }

    private void Update()
    {
        if (_playerAtEntrance != PortalSide.None)
            CopyPlayerCameraPosition();

        _playerPosition = playerTransform.position;
        _playerRotationEuler = playerTransform.rotation.eulerAngles;
        _mainCamRotationEuler = _mainCamTransform.rotation.eulerAngles;
        _mainCamPosition = _mainCamTransform.position;
        _mainCamLocalPosition = _mainCamTransform.localPosition;
    }

    public void OnTriggerExit()
    {
        _playerAtEntrance = PortalSide.None;
        exitTrigger.MeshCollider.enabled = true;
        portalTrigger.MeshCollider.enabled = true;
    }

    private void OnValidate()
    {
        mainCam = Camera.main; //todo get better ref to camera
        if (mainCam != null)
            _mainCamTransform = mainCam.transform;

        if (!playerTransform)
            playerTransform = FindObjectOfType<LeifPlayerController>().transform;
        SetupChildrenComponents();
    }

    private void CopyPlayerCameraPosition()
    {
        // get mainCam pos relative to entrance / exit
        var entranceCamRelativePos = _entrancePortalTransform.InverseTransformPoint(_mainCamPosition);
        var exitCamInverse = _exitPortalTransform.InverseTransformPoint(_mainCamPosition);
        // apply it relative to the opposite entrance / exit
        var exitBoundsRelativePos = _exitPortalTransform.TransformPoint(entranceCamRelativePos);
        var entranceBoundsRelativePos = _entrancePortalTransform.TransformPoint(exitCamInverse);


        exitBoundsRelativePos.y = _mainCamLocalPosition.y;
        entranceBoundsRelativePos.y = _mainCamLocalPosition.y;
        switch (_playerAtEntrance)
        {
            // copy playerCam pos to the exit cam
            case PortalSide.Entrance:
                exitCamera.transform.position = exitBoundsRelativePos;
                exitCamera.transform.rotation = Quaternion.Euler(_mainCamRotationEuler.x,
                    _playerRotationEuler.y + 180, 180);
                break;
            // copy playerCam pos to the entrance cam
            case PortalSide.Exit:
                entranceCamera.transform.position = entranceBoundsRelativePos;
                entranceCamera.transform.rotation = Quaternion.Euler(_mainCamRotationEuler.x,
                    _playerRotationEuler.y + 180, 180);
                break;
            case PortalSide.None:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetupChildrenComponents()
    {
        if (!_entrancePortalTransform)
            _entrancePortalTransform = portalTrigger.transform;
        if (!_exitPortalTransform)
            _exitPortalTransform = exitTrigger.transform;

        if (_portalBoundsTrigger == null)
            _portalBoundsTrigger = _entrancePortalTransform.GetComponentInChildren<BoxCollider>();

        _portalBoundsTrigger.size = portalBoundsSize;
        _portalBoundsTrigger.center = new Vector3(0, portalBoundsSize.y / 2, 0);

        var triggers = GetComponentsInChildren<PortalTrigger>();
        foreach (var trigger in triggers)
        {
            //de-register for safety
            trigger.OnEnter -= OnEntranceTriggerEnter;
            trigger.OnEnter -= OnExitTriggerEnter;
            trigger.OnExit -= OnTriggerExit;
            trigger.OnCollision -= OnPortalCollision;

            //register new events
            trigger.OnCollision += OnPortalCollision;
            trigger.OnExit += OnTriggerExit;
            if (trigger.PortalType == PortalType.Entrance)
                trigger.OnEnter += OnEntranceTriggerEnter;
            else
                trigger.OnEnter += OnExitTriggerEnter;
        }
    }


    public void OnPortalCollision(PortalType portalType)
    {
        portalTrigger.MeshCollider.enabled = false;
        exitTrigger.MeshCollider.enabled = false;
        var startTransform = _entrancePortalTransform;
        var endTransform = _exitPortalTransform;
        if (portalType == PortalType.Exit)
        {
            startTransform = _exitPortalTransform;
            endTransform = _entrancePortalTransform;
        }

        var start = startTransform.InverseTransformPoint(_playerPosition);
        var end = endTransform.TransformPoint(start);
        playerTransform.position = end;
    }

    public void OnEntranceTriggerEnter()
    {
        exitTrigger.MeshCollider.enabled = false;
        _playerAtEntrance = PortalSide.Entrance;
    }

    public void OnExitTriggerEnter()
    {
        portalTrigger.MeshCollider.enabled = false;
        _playerAtEntrance = PortalSide.Exit;
    }

    private enum PortalSide
    {
        None,
        Entrance,
        Exit
    }
}