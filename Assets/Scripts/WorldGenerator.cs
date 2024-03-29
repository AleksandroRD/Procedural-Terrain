using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] [Range(2,250)] private int chunkSize;
    [SerializeField] private float heightMultiplyer;
    [SerializeField] private int renderDistance;
    [SerializeField] private Material terrainMaterial;
    [SerializeField] private Transform player;
    
    [Header("Noise Settings")]
    [SerializeField] private NoiseSettings baseNoiseSettings;
    [SerializeField] private NoiseSettings featureNoiseSettings;
    [SerializeField] private AnimationCurve featureNoiseInfluence;

    private Dictionary<Vector2,WorldChunk> generatedChunks = new Dictionary<Vector2, WorldChunk>();
    private List<WorldChunk> loadedChunks = new List<WorldChunk>();

    private Vector2Int playerPos;
    private Vector2Int oldPlayerPos;

    void Start(){
        UpdateChuncks();
    }

    void Update(){

        int x = Mathf.RoundToInt(player.position.x / chunkSize);
        int y = Mathf.RoundToInt(player.position.z / chunkSize);

        playerPos = new(x,y);

        if(playerPos != oldPlayerPos){
            
            UpdateChuncks();
        }

        oldPlayerPos = playerPos;

    }

    void UpdateChuncks(){

        foreach(WorldChunk chunk in loadedChunks){
            if(Vector2Int.Distance(chunk.coordinates,playerPos) > renderDistance){
                chunk.Unload();
            }
        }

        for(int x = playerPos.x - renderDistance; x <= playerPos.x + renderDistance; x++){
            for(int y = playerPos.y - renderDistance; y <= playerPos.y + renderDistance; y++){
                Vector2Int pos = new Vector2Int(x,y);

                if(generatedChunks.ContainsKey(pos)){
                    if(!generatedChunks[pos].ISLoaded()){
                        generatedChunks[pos].Load();
                    }
                }else{
                    generatedChunks[pos] = new (chunkSize,pos,transform,baseNoiseSettings,featureNoiseSettings,featureNoiseInfluence,heightMultiplyer,terrainMaterial);
                    loadedChunks.Add(generatedChunks[pos]);
                    generatedChunks[pos].Load();
                }
            }
        }

    }
}
