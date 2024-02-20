using System;
using Unity.Mathematics;
using UnityEngine;

public class WaterfallMe : MonoBehaviour
{
    public GameObject node;
    public MeshFilter meshFilter;
    public float speed = 1, freq = 1, amplitude = 1, waterfallDistance = 10;
    public float alpha;
    public float alphaZ;
    public Vector3 endPos;
    public Vector3 pos;
    public Vector3 startPos;

    private void Start()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        return;
        if (node == null) return;
        if (meshFilter == null) return;
        if (alphaZ < 1)
        {
            alphaZ += Time.deltaTime * speed;
            // pos.z += Mathf.Sin(alphaZ * amplitude) * freq;
            pos.y += Mathf.Sin(alphaZ * amplitude) * freq;
            if (alphaZ >= 1)
            {
                startPos = pos;
                endPos = startPos + Vector3.down * waterfallDistance;
            }
        }

        if (alphaZ >= 1)
        {
            alpha += Time.deltaTime * speed;
            pos.y = Vector3.Lerp(startPos, endPos, alpha).y;
        }

        if (alpha >= 1)
        {
            alpha = 0;
            alphaZ = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (node == null) return;
        if (meshFilter == null) return;
        Gizmos.DrawMesh(meshFilter.sharedMesh, pos, quaternion.identity, Vector3.one * 100);
    }
}