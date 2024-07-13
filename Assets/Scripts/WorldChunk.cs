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

    public WorldChunk(int size, Vector2Int coordinates, Transform parent, NoiseSettings baseNoiseSettings, List<NoiseSettings> featureNoiseLayers, float heightMultiplyer,float maxHeight, Material material){
        this.coordinates = coordinates;

        gameObject = new GameObject("Terrain Chunk");
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshFilter = gameObject.AddComponent<MeshFilter>();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		meshRenderer.material = material;

        gameObject.transform.position = new Vector3(coordinates.x * size,0,coordinates.y * size);
		gameObject.transform.parent = parent;

        SetVisible(false);

        sampleCentre = new(coordinates.x * size, coordinates.y * size);

        heightMap = HeightMapGenerator.GenerateHeightMap(size+1, baseNoiseSettings, featureNoiseLayers, heightMultiplyer, maxHeight, sampleCentre);
        terrainMesh = MeshGenerator.GenerateMeshData(heightMap.values).CreateMesh();

        meshCollider.sharedMesh = terrainMesh;
        meshFilter.sharedMesh = terrainMesh;
    }

    public void Load(){
        SetVisible(true);
    }

    public void Unload(){
        SetVisible(false);
    }

    public bool ISLoaded(){
        return IsVisible();
    }
    private void SetVisible(bool visible){
        gameObject.SetActive(visible);
    }

    private bool IsVisible() {
		return gameObject.activeSelf;
	}

}
