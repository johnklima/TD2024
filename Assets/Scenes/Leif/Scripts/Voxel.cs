using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scenes.Leif.Scripts
{
    public enum GizmoType
    {
        Cube,
        WireCube,
        Sphere,
        WireSphere
    }

    public class Voxel
    {
        public Int3 coordinates;
        public GizmoType gizmoType;
        public float noise;
        public float scale;
        public Vector3 worldPos;

        public Voxel(Vector3 worldPos, float scale, float noise, Int3 coordinates, GizmoType gizmoType)
        {
            this.worldPos = worldPos;
            this.scale = scale;
            this.noise = noise;
            this.coordinates = coordinates;
            this.gizmoType = gizmoType;
        }

        public Vector3 size => Vector3.one * scale;

        public void DrawGizmos(GizmoSettings gizmoSettings)
        {
            if (noise / 10f < MapGenerator.SolidThreshold) return;
            Gizmos.color = Color.Lerp(Color.black, Color.white, noise / 10);

            switch (gizmoSettings.gizmoType)
            {
                case GizmoType.Cube:
                    Gizmos.DrawCube(worldPos, size * gizmoSettings.gizmoScale);
                    break;
                case GizmoType.WireCube:
                    Gizmos.DrawWireCube(worldPos, size * gizmoSettings.gizmoScale);
                    break;
                case GizmoType.Sphere:
                    Gizmos.DrawSphere(worldPos, scale * gizmoSettings.gizmoScale);
                    break;
                case GizmoType.WireSphere:
                    Gizmos.DrawWireSphere(worldPos, scale * gizmoSettings.gizmoScale);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (gizmoSettings.drawHandles) Handles.Label(worldPos, noise + "");
        }
    }
}