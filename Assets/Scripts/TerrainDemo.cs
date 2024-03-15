using UnityEngine;

public class TerrainDemo : MonoBehaviour
{  
    [Header("Hoise")]
    //general settings
    [SerializeField]
     [Range(2,250)]
    private int size;
    [SerializeField]
    float heightMultiplyer;
    [SerializeField]
    private Vector2 offSet;
    [SerializeField]
    private NoiseSettings noiseSettings;
    [SerializeField]
    private NoiseSettings featureNoiseSettings;
    [SerializeField]
    private AnimationCurve featureNoiseInfluence;
    
    [Header("Material")]
    [SerializeField]
    private Material terrainMaterial;

    public GameObject plane;
    private WorldChunk worldChunk;
    void Start()
    {
        GenerateSimpleTerrain();
    }
    
    public void GenerateSimpleTerrain(){
        worldChunk = new WorldChunk(size,new Vector2Int(0,0),transform,noiseSettings,featureNoiseSettings,featureNoiseInfluence,heightMultiplyer,terrainMaterial);
        worldChunk.Load();

        plane.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = TextureGenerator.GenerateNoiseTexture(Noise.GeneratePerlinNoise(size,featureNoiseSettings,offSet));
    }

    void OnValidate(){
        //GenerateSimpleTerrain();
    }

}
