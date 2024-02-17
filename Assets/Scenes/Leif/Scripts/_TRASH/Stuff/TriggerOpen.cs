using UnityEngine;

public class TriggerOpen : MonoBehaviour
{
    public float lerpSpeed = 1;
    public BoxCollider doorBlocker;
    private SkinnedMeshRenderer _meshRenderer;
    private float lerpAlpha;
    private bool playerInRange;

    private void Start()
    {
        _meshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        if (lerpAlpha < 1) lerpAlpha += Time.deltaTime * lerpSpeed;
        var val = lerpAlpha;
        if (!playerInRange) val = 1 - lerpAlpha;
        _meshRenderer.SetBlendShapeWeight(0, val * 100);
        if (lerpAlpha < 1) return;
        doorBlocker.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        lerpAlpha = 0;
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        lerpAlpha = 0;
        playerInRange = false;
        doorBlocker.enabled = true;
    }
}