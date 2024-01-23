using UnityEngine;

namespace Scenes.Leif.Scripts
{
    public enum GizmoType
    {
        None,
        Mesh,
        Cube,
        WireCube,
        Sphere,
        WireSphere
    }

    public class VoxelFactory
    {
        private readonly Transform parent;
        private BoxCollider boxCollider;
        public Int3 coordinates;
        public LayerMask interactableLayerMask;
        public bool isSolid;
        public bool isVisible;
        public Material material;
        public GameObject meshObject;
        private MeshRenderer meshRenderer;
        public string name;
        public float noise;
        public float scale;
        public Voxel voxel;
        public Vector3 worldPos;

        public VoxelFactory(
            Vector3 worldPos,
            float scale,
            float noise,
            Int3 coordinates,
            bool isSolid,
            Transform parent,
            Material material,
            LayerMask interactableLayerMask
        )
        {
            this.worldPos = worldPos;
            this.scale = scale;
            this.noise = noise;
            this.coordinates = coordinates;
            this.material = material;
            this.parent = parent;
            this.interactableLayerMask = interactableLayerMask;
            name = $"Voxel (x:{coordinates.x},y:{coordinates.y},z:{coordinates.z}) (#{parent.childCount})";
            GenerateMeshObject();
            isVisible = this.isSolid = isSolid;
        }

        public Vector3 size => Vector3.one * scale;


        public void SetSolid(bool solid)
        {
            if (boxCollider)
                boxCollider.enabled = solid;
            isSolid = solid;
            voxel.isSolid = isSolid;
            voxel.isVisible = isVisible;
            voxel.noise = noise;
        }


        public void SetVisible(bool visible)
        {
            isVisible = visible;
            voxel.isSolid = isSolid;
            voxel.isVisible = isVisible;
            voxel.noise = noise;
            meshRenderer.enabled = isVisible;
        }

        private void GenerateMeshObject()
        {
            meshObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            meshObject.name = name;
            meshObject.transform.parent = parent;
            meshObject.transform.position = worldPos;
            meshObject.transform.localScale = Vector3.one * scale;
            meshObject.layer = interactableLayerMask;
            //? Renderer/Mesh/Collider
            meshRenderer = meshObject.GetComponent<MeshRenderer>();
            meshRenderer.material = material;
            boxCollider = meshObject.GetComponent<BoxCollider>();

            voxel = meshObject.AddComponent<Voxel>();
            voxel.coordinates = coordinates;
            voxel.isSolid = isSolid;
            voxel.isVisible = isVisible;
            voxel.noise = noise;
        }
    }
}