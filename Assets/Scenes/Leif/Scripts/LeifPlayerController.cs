using UnityEngine;

[RequireComponent(typeof(Rigidbody),
    typeof(CapsuleCollider))]
public class LeifPlayerController : CameraController
{
    [SerializeField] private float playerSpeed = 10, movementMultiplier = 0.1f;
    [SerializeField] private float jumpForce;

    private RaycastHit hit;

    private Rigidbody mRigidbody;

    public RaycastHit Hit => hit;

    protected override void Start()
    {
        if (cameraToControl == null)
            cameraToControl = Camera.main;
        mRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        InputHandler();
        DoRay();
    }

    private void FixedUpdate()
    {
        if (!playerHasControl) return;
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");
        var transforms = transform;
        var moveDirection = transforms.forward * verticalMovement + transforms.right * horizontalMovement;

        if (Input.GetKeyDown(KeyCode.Space)) moveDirection += Vector3.up * jumpForce;

        mRigidbody.AddForce(moveDirection.normalized * (playerSpeed * movementMultiplier), ForceMode.Acceleration);
    }

    private void DoRay()
    {
        if (!playerHasControl) return;
        var ray = cameraToControl.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 100));
        if (!Physics.Raycast(ray, out hit, interactableLayerMask)) return;
        if (!Input.GetMouseButtonDown(0)) return;
        if (hit.transform.gameObject.TryGetComponent(out Voxel voxel))
        {
            Debug.Log("hit block, interacted");
            voxel.Interact();
        }
        // destroy block;
    }
}