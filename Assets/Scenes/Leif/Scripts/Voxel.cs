using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scenes.Leif.Scripts
{
    public class Voxel
    {
        public float noise;
        public float scale;
        public Vector3 worldPos;

        public Voxel(Vector3 worldPos, float scale, float noise)
        {
            this.worldPos = worldPos;
            this.scale = scale;
            this.noise = noise;
        }

        public Vector3 size => Vector3.one * scale;

        public void DrawGizmos(bool handles = false)
        {
            if (noise / 10f < MapGenerator.SolidThreshold) return;
            Gizmos.color = Color.Lerp(Color.black, Color.white, noise / 10);
            Gizmos.DrawCube(worldPos, size * .95f);
            if (handles) Handles.Label(worldPos, noise + "");
        }
    }
}