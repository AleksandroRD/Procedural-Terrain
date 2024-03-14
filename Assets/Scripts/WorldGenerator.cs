using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("World Generation")]
    [SerializeField]
    [Range(2,250)]
    private int chunkSize;
    [SerializeField]
    private float heightMultiplyer;
    [SerializeField]
    private int renderDistance;
    [SerializeField]
    private Material terrainMaterial;

    [SerializeField]
    private Transform player;
    private Vector2Int playerPos;
    private Vector2Int oldPlayerPos;
    
    private Dictionary<Vector2,WorldChunk> generatedChunks = new Dictionary<Vector2, WorldChunk>();
    private List<WorldChunk> loadedChunks = new List<WorldChunk>();
    [SerializeField]
    private NoiseSettings noiseSettings;
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

        for(int x = playerPos.x - renderDistance; x <= playerPos.x + renderDistance; x++){
            for(int y = playerPos.y - renderDistance; y <= playerPos.y + renderDistance; y++){
                Vector2 pos = new Vector2(x,y);

                if(generatedChunks.ContainsKey(pos)){
                    if(!generatedChunks[pos].ISLoaded()){
                        generatedChunks[pos].Load();
                    }
                }else{
                    generatedChunks[pos] = new (chunkSize,pos,transform,noiseSettings,heightMultiplyer,terrainMaterial);
                    loadedChunks.Add(generatedChunks[pos]);
                    generatedChunks[pos].Load();
                }
            }
        }

    }
}
