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
        if(worldChunk != null || transform.childCount != 0){
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        worldChunk = new WorldChunk(size,new Vector2(0,0),transform,noiseSettings,heightMultiplyer,terrainMaterial);
        worldChunk.Load();

        plane.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = TextureGenerator.GenerateNoiseTexture(Noise.GeneratePerlinNoise(size,noiseSettings,offSet));
    }

    void OnValidate(){
        //GenerateSimpleTerrain();
    }

}
