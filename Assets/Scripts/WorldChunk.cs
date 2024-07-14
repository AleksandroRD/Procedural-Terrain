using System.Collections.Generic;
using UnityEngine;

public class WorldChunk
{
    public Vector2Int coordinates;
    public GameObject gameObject;

    Mesh terrainMesh;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    Vector2 sampleCentre;

    HeightMap heightMap;

    public WorldChunk(int size, Vector2Int coordinates, Transform parent, List<NoiseSettings> featureNoiseLayers, float minHeight, float maxHeight, Material groundMaterial)
    {
        this.coordinates = coordinates;

        gameObject = new GameObject("Terrain Chunk (x: " + coordinates.x + " y: " + coordinates.y + ")");
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        meshRenderer.material = groundMaterial;

        gameObject.transform.position = new Vector3(coordinates.x * size, 0, coordinates.y * size);
        gameObject.transform.parent = parent;

        sampleCentre = new(coordinates.x * size, coordinates.y * size);

        heightMap = HeightMapGenerator.GenerateHeightMap(size + 1, featureNoiseLayers, minHeight, maxHeight, sampleCentre);
        terrainMesh = MeshGenerator.GenerateMeshData(heightMap.values).CreateMesh();

        meshCollider.sharedMesh = terrainMesh;
        meshFilter.sharedMesh = terrainMesh;
    }

    // public void Load()
    // {
        
    // }

    // public bool IsLoaded()
    // {
        
    // }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public bool IsVisible()
    {
        return gameObject.activeSelf;
    }

}
