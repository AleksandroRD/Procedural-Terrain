using UnityEngine;

public class WorldChunk
{
    public Vector2 coordinates;
    GameObject meshObject;

    Mesh terrainMesh;
    MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	MeshCollider meshCollider;

	Vector2 sampleCentre;

    NoiseSettings noiseSettings;

    float heightMultiplyer;
    float[,] heightMap;

    public WorldChunk(int size,Vector2 coordinates,Transform parent,NoiseSettings noiseSettings,float heightMultiplyer,Material material){
        this.coordinates = coordinates;
        this.noiseSettings = noiseSettings;

        meshObject = new GameObject("Terrain Chunk");
		meshRenderer = meshObject.AddComponent<MeshRenderer>();
		meshFilter = meshObject.AddComponent<MeshFilter>();
		meshCollider = meshObject.AddComponent<MeshCollider>();
		meshRenderer.material = material;

        meshObject.transform.position = new Vector3(coordinates.x * size,0,coordinates.y * size);
		meshObject.transform.parent = parent;
        SetVisible(false);

        sampleCentre = new(coordinates.x * size, coordinates.y * size);

        heightMap = Noise.GeneratePerlinNoise(size+1, noiseSettings, sampleCentre);
        terrainMesh = MeshGenerator.GenerateMeshData(heightMap,heightMultiplyer).CreateMesh();
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
        meshObject.SetActive(visible);
    }

    private bool IsVisible() {
		return meshObject.activeSelf;
	}


}
