using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.Leif.Scripts
{
    [Serializable]
    public class MapData
    {
        [Range(1, 1000)] public float heightMultiplier = 100;
        public INT3 mapSize = new(10, 10, 10);
        [Range(0, 5)] public float gridScale = 1;
        public NoiseData noiseData = new();
    }

    [Serializable]
    public class GizmoSettings
    {
        public bool drawGiz, drawHandles;
        public GizmoType gizmoType;
        [Range(0, 1)] public float gizmoScale = 1;
    }

    public class MapGenerator : MonoBehaviour
    {
        public static float SolidThreshold = 0.1f;
        [SerializeField] private MapData mapData = new();
        [FormerlySerializedAs("gizSettings")] public GizmoSettings gizmoSettings;
        [Range(0, 1)] public float solidThreshold;
        private float[,] _noiseMap;

        private Voxel[,,] grid;


        private void Start()
        {
        }


        private void Update()
        {
        }

        private void OnDrawGizmos()
        {
            if (!gizmoSettings.drawGiz) return;
            if (grid == null) grid = Generate3dGrid(mapData.mapSize, mapData.gridScale);
            var xSize = grid.GetLength(0);
            var ySize = grid.GetLength(1);
            var zSize = grid.GetLength(2);

            if (xSize <= 0 && ySize <= 0 && zSize <= 0) return;

            for (var y = 0; y < ySize; y++)
            for (var z = 0; z < zSize; z++)
            for (var x = 0; x < xSize; x++)
                grid[x, y, z].DrawGizmos(gizmoSettings);
        }


        private void OnValidate()
        {
            grid = null;
            if (_noiseMap == null) _noiseMap = Noise.GenerateNoiseMap(mapData);
            SolidThreshold = solidThreshold;
        }

        private Voxel[,,] Generate3dGrid(INT3 mapDimensions, float voxelScale)
        {
            var (mapX, mapY, mapZ) = mapDimensions;

            var bottomLeftRear = new Vector3(-mapX / 2f * voxelScale, -mapY / 2f * voxelScale, -mapZ / 2f * voxelScale);

            var halfScale = voxelScale / 2f;

            if (mapX <= 0 || mapY <= 0 || mapZ <= 0)
                throw new
                    Exception($"mapX:{mapX}, mapY:{mapY}, mapZ:{mapZ}; Neither can be zero (0)!");
            var noiseMap = Noise.GenerateNoiseMap(mapData);

            var voxelArray = new Voxel[mapX, mapY, mapZ];

            for (var y = 0; y < mapY; y++)
            for (var z = 0; z < mapZ; z++)
            for (var x = 0; x < mapX; x++)
            {
                var worldPos = new Vector3(
                    bottomLeftRear.x + halfScale + x * voxelScale,
                    bottomLeftRear.y + halfScale + y * voxelScale,
                    bottomLeftRear.z + halfScale + z * voxelScale
                );

                var height = noiseMap[x, z] * mapData.heightMultiplier - y;


                voxelArray[x, y, z] =
                    new Voxel(worldPos, voxelScale, height, new Int3(x, y, z), gizmoSettings.gizmoType);
            }

            return voxelArray;
        }
    }
}