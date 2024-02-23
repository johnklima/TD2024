using System;
using UnityEngine;

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