using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField][Range(2, 250)] private int _chunkSize;
    [SerializeField] bool _randomSeed;
    [SerializeField] int _seed;
    [SerializeField] float _waterLevelHeight;
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;
    [SerializeField] private int _renderDistance;
    [SerializeField] private Material _terrainMaterial;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _waterMesh;

    [Header("Noise Settings")]
    [SerializeField] private List<NoiseSettings> _noiseLayers;

    [Header("Detail Settings")]
    [SerializeField] private List<DetailSettings> _detailSettings;

    private Dictionary<Vector2, WorldChunk> _generatedChunks = new Dictionary<Vector2, WorldChunk>();
    private List<WorldChunk> _loadedChunks = new List<WorldChunk>();

    private Vector2Int _playerPos;
    private Vector2Int _oldPlayerPos;

    void Start()
    {
        float scale = (_renderDistance * 2 + 1) * (_chunkSize * 0.1f);

        _waterMesh.localScale = new Vector3(scale, scale, scale);
        _waterMesh.position = new Vector3(0,_waterLevelHeight,0);

        System.Random rand = _randomSeed ? new System.Random(_seed) : new System.Random(Random.Range(0,99999999));

        foreach (var layer in _noiseLayers)
        {
            layer.Seed = rand.Next();
        }

        UpdateChuncks();
    }

    void FixedUpdate()
    {
        int x = Mathf.RoundToInt(_player.position.x / _chunkSize);
        int y = Mathf.RoundToInt(_player.position.z / _chunkSize);

        _playerPos = new(x, y);
        
        if (_playerPos != _oldPlayerPos)
        {
            UpdateWater();
            UpdateChuncks();
        }

        _oldPlayerPos = _playerPos;
    }

    private void UpdateWater()
    {
        _waterMesh.position = new Vector3(_playerPos.x * _chunkSize, _waterLevelHeight, _playerPos.y * _chunkSize);
    }

    void UpdateChuncks()
    {

        foreach (WorldChunk chunk in _loadedChunks)
        {
            if (Vector2Int.Distance(chunk.Coordinates, _playerPos) > _renderDistance)
            {
                chunk.SetVisible(false);
            }
        }

        for (int x = _playerPos.x - _renderDistance; x <= _playerPos.x + _renderDistance; x++)
        {
            for (int y = _playerPos.y - _renderDistance; y <= _playerPos.y + _renderDistance; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if (_generatedChunks.ContainsKey(pos))
                {
                    if (!_generatedChunks[pos].IsVisible())
                    {
                        _generatedChunks[pos].SetVisible(true);
                    }

                    continue;
                }

                _generatedChunks[pos] = new(_chunkSize,
                    pos,
                    transform,
                    _noiseLayers,
                    _detailSettings,
                    _minHeight,
                    _maxHeight,
                    _terrainMaterial
                );

                _loadedChunks.Add(_generatedChunks[pos]);
            }
        }

    }
}
