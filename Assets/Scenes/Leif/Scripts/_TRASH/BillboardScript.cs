using System;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    [SerializeField] private Camera cameraToLookAt;
    [SerializeField] private AxisConstraint _constraint;

    private bool deactivated;

    private float scaleAlpha;

    private void Awake()
    {
        if (cameraToLookAt == null) cameraToLookAt = Camera.main;
    }

    private void Update()
    {
        ScaleOut();
        if (deactivated) return;
        if (cameraToLookAt == null) return;

        var targetPosition = cameraToLookAt.transform.position;

        // transform.LookAt(targetPosition);

        var direction = transform.position - targetPosition;
        if (direction == Vector3.zero) return;

        if (direction.magnitude >= 1) // clamp moveDir to avoid diagonal speedBoost
            direction = direction.normalized;

        if (_constraint.IsActive)
            direction = new Vector3(
                _constraint.x ? 0 : direction.x,
                _constraint.y ? 0 : direction.y,
                _constraint.z ? 0 : direction.z
            );

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void Deactivate()
    {
        deactivated = true;
    }

    private void ScaleOut()
    {
        if (!deactivated) return;

        // scaled object to 0 and deactivates
        if (scaleAlpha <= 1) scaleAlpha += Time.deltaTime;

        transform.localScale = Vector3.one * (1 - scaleAlpha);

        if (scaleAlpha < 1) return;
        gameObject.SetActive(false);
    }

    [Serializable]
    public class AxisConstraint
    {
        public bool x, y, z;
        public bool IsActive => x || y || z;
    }
}