using System.Collections.Generic;
using UnityEngine;

public class WorldChunk
{
    public Vector2Int Coordinates;
    public GameObject gameObject;

    Mesh terrainMesh;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    Vector2 sampleCentre;

    public HeightMap HeightMap;

    public WorldChunk(int size, Vector2Int coordinates, Transform parent, List<NoiseLayer> noiseLayers, List<DetailSettings> detailSettings, Material groundMaterial)
    {
        Coordinates = coordinates;

        gameObject = new GameObject("Terrain Chunk (x: " + coordinates.x + " y: " + coordinates.y + ")");
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        meshRenderer.material = groundMaterial;

        gameObject.transform.position = new Vector3(coordinates.x * size, 0, coordinates.y * size);
        gameObject.transform.parent = parent;

        sampleCentre = new(coordinates.x * size, coordinates.y * size);

        HeightMap = HeightMapGenerator.GenerateHeightMap(size + 1, noiseLayers, sampleCentre);
        terrainMesh = MeshGenerator.GenerateMeshData(HeightMap.Values).CreateMesh();
        DetailGenerator.CreateDetails(this, detailSettings);

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
