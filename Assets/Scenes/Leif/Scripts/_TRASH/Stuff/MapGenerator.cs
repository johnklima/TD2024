using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Scenes.Leif.Scripts
{
    [Serializable]
    public class MapData
    {
        [Range(1, 1000)] public float heightMultiplier = 100;

        // public INT3 mapSize = new(10, 10, 10);
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
        public static MapGenerator Instance;
        public bool autoUpdate;
        [SerializeField] private MapData mapData = new();
        public GizmoSettings gizmoSettings;
        [Range(0, 1)] public float solidThreshold;
        public LayerMask interactableLayerMask;

        public Material material;

        private float[,] _noiseMap;

        private VoxelFactory[,,] grid;


        private void Start()
        {
        }


        private void Update()
        {
        }

//
// #if UNITY_EDITOR
//         private void OnDrawGizmos()
//         {
//             if (!gizmoSettings.drawGiz) return;
//
//             if (grid == null)
//                 grid = Generate3dVoxelArray(mapData);
//
//             var (xSize, ySize, zSize) = mapData.mapSize;
//             if (xSize <= 0 && ySize <= 0 && zSize <= 0) return;
//             for (var z = 0; z < zSize; z++)
//             for (var y = 0; y < ySize; y++)
//             for (var x = 0; x < xSize; x++)
//             {
//                 if (grid[x, y, z].noise / 10f > SolidThreshold)
//                     Gizmos.color = Color.Lerp(Color.black, Color.white, grid[x, y, z].noise / 10);
//                 switch (gizmoSettings.gizmoType)
//                 {
//                     case GizmoType.Cube:
//                         Gizmos.DrawCube(grid[x, y, z].worldPos,
//                             grid[x, y, z].size * gizmoSettings.gizmoScale);
//                         break;
//                     case GizmoType.WireCube:
//                         Gizmos.DrawWireCube(grid[x, y, z].worldPos,
//                             grid[x, y, z].size * gizmoSettings.gizmoScale);
//                         break;
//                     case GizmoType.Sphere:
//                         Gizmos.DrawSphere(grid[x, y, z].worldPos,
//                             grid[x, y, z].scale * gizmoSettings.gizmoScale);
//                         break;
//                     case GizmoType.WireSphere:
//                         Gizmos.DrawWireSphere(grid[x, y, z].worldPos,
//                             grid[x, y, z].scale * gizmoSettings.gizmoScale);
//                         break;
//                     case GizmoType.None:
//                         break;
//                     case GizmoType.Mesh:
//                         grid[x, y, z].SetVisible(grid[x, y, z].noise / 10f > SolidThreshold);
//                         break;
//                     default:
//                         throw new ArgumentOutOfRangeException();
//                 }
//
//                 if (gizmoSettings.drawHandles) Handles.Label(grid[x, y, z].worldPos, grid[x, y, z].noise + "");
//             }
//         }
// #endif

        private void OnValidate()
        {
        }

        private void OnVoxelStateDestroy(Int3 coords)
        {
            grid[coords.x, coords.y, coords.z] = null;
            OnVoxelStateChange(coords);
        }

        private void OnVoxelStateChange(Int3 coords)
        {
            var neighbours = GetVoxelNeighbours(grid, coords);
            foreach (var neighbour in neighbours)
            {
                if (neighbour == null) continue;
                var isSolid = neighbour.noise / 10f > SolidThreshold;
                neighbour.SetSolid(isSolid);
                neighbour.SetVisible(isSolid);
            }
        }


        public void ReGenerate()
        {
            StartCoroutine(CleanUpAndGenerate());
        }

        private IEnumerator CleanUpAndGenerate()
        {
            CleanUpHierarchy();
            yield return new WaitForEndOfFrame();
            if (_noiseMap == null) _noiseMap = Noise.GenerateNoiseMap(mapData);
            SolidThreshold = solidThreshold;
            grid = Generate3dVoxelArray(mapData);
            SetVoxelVisibility();
        }

        private void SetVoxelVisibility()
        {
            var (xSize, ySize, zSize) = mapData.mapSize;
            for (var z = 0; z < zSize; z++)
            for (var y = 0; y < ySize; y++)
            for (var x = 0; x < xSize; x++)
            {
                if (grid[x, y, z] == null) continue;
                var neighbours = GetVoxelNeighbours(grid, new Int3(x, y, z));
                grid[x, y, z].voxel.neighbours = new Voxel[neighbours.Length].ToList();
                var surroundedBy = 0;
                for (var i = 0; i < neighbours.Length; i++)
                {
                    grid[x, y, z].voxel.neighbours[i] = neighbours[i].voxel;
                    if (neighbours[i].isSolid)
                        surroundedBy++;
                }

                // 26 // 17 // 11 // 7
                if (neighbours.Length == 26 && surroundedBy == 26)
                    grid[x, y, z].SetVisible(false);
                else
                    grid[x, y, z].SetVisible(grid[x, y, z].noise / 10f > SolidThreshold);
            }
        }


        private VoxelFactory[] GetVoxelNeighbours(VoxelFactory[,,] array, Int3 coords, int area = 1)
        {
            var areaCubed = area * 3;
            var xLength = array.GetLength(0);
            var yLength = array.GetLength(1);
            var zLength = array.GetLength(2);

            var neighbours = new List<VoxelFactory>();
            for (var z = coords.z - area; z <= coords.z + area; z++)
            for (var y = coords.y - area; y <= coords.y + area; y++)
            for (var x = coords.x - area; x <= coords.x + area; x++)
            {
                if (x == coords.x && y == coords.y && z == coords.z) continue;
                if (x < 0 || y < 0 || z < 0) continue;
                if (x >= xLength || y >= yLength || z >= zLength) continue;
                var neighbour = array[x, y, z];
                if (!neighbours.Contains(neighbour)) neighbours.Add(neighbour);
            }

            return neighbours.ToArray();
        }

        private void CleanUpHierarchy()
        {
            foreach (Transform o in transform)
                if (Application.isEditor)
                    StartCoroutine(DestroyAtEndOfFrame(o.gameObject));
                else
                    Destroy(o.gameObject);
        }

        private IEnumerator DestroyAtEndOfFrame(GameObject go)
        {
            yield return new WaitForEndOfFrame();
            DestroyImmediate(go);
        }

        private VoxelFactory[,,] Generate3dVoxelArray(MapData mapData)
        {
            var voxelScale = mapData.gridScale;
            var (mapX, mapY, mapZ) = mapData.mapSize;
            var origin =
                Vector3.zero; //new Vector3(-mapX / 2f * voxelScale, -mapY / 2f * voxelScale, -mapZ / 2f * voxelScale);
            var halfScale = voxelScale / 2f;
            if (mapX <= 0 || mapY <= 0 || mapZ <= 0)
                throw new
                    Exception($"mapX:{mapX}, mapY:{mapY}, mapZ:{mapZ}; Neither can be zero (0)!");
            var noiseMap = Noise.GenerateNoiseMap(mapData);
            var voxelArray = new VoxelFactory[mapX, mapY, mapZ];
            for (var z = 0; z < mapZ; z++)
            for (var y = 0; y < mapY; y++)
            for (var x = 0; x < mapX; x++)
            {
                var worldPos = new Vector3(
                    origin.x + halfScale + x * voxelScale,
                    origin.y + halfScale + y * voxelScale,
                    origin.z + halfScale + z * voxelScale
                );

                var height = noiseMap[x, z] * mapData.heightMultiplier - y;
                var isSolid = height / 10f > SolidThreshold;
                var newFactory = new VoxelFactory(worldPos, voxelScale, height, new Int3(x, y, z), isSolid, transform,
                    material, interactableLayerMask);
                voxelArray[x, y, z] = newFactory;
                newFactory.voxel.RegisterEvents(OnVoxelStateChange, OnVoxelStateDestroy);
            }

            return voxelArray;
        }
    }
}