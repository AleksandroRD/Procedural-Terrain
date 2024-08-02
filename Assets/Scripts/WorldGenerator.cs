using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField][Range(2, 250)] private int _chunkSize;
    [SerializeField] private int _renderDistance;
    [SerializeField] bool _randomSeed;
    [SerializeField] int _seed;
    [SerializeField] private Transform _player;

    [Header("Water")]
    [SerializeField] float _waterLevelHeight;
    [SerializeField] private Transform _waterMesh;

    [Header("HeightMap")]
    [SerializeField] private List<NoiseLayer> _noiseLayers;

    [Header("Visual Settings")]
    [SerializeField] private Material _terrainMaterial;
    [SerializeField] private List<DetailSettings> _detailSettings;

    private Dictionary<Vector2, WorldChunk> _generatedChunks = new Dictionary<Vector2, WorldChunk>();
    private List<WorldChunk> _loadedChunks = new List<WorldChunk>();

    private Vector2Int _playerPos;
    private Vector2Int _oldPlayerPos;

    public void Start()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }

        GenerateSeed();
        UpdateChuncks();
        UpdateWater();
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
        float scale = (_renderDistance * 2 + 1) * (_chunkSize * 0.1f);
        _waterMesh.localScale = new Vector3(scale, scale, scale);
        _waterMesh.position = new Vector3(_playerPos.x * _chunkSize, _waterLevelHeight, _playerPos.y * _chunkSize);
    }

    public void UpdateChuncks()
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
                    _terrainMaterial
                );

                _loadedChunks.Add(_generatedChunks[pos]);
            }
        }
    }

    void GenerateSeed()
    {
        System.Random rand = _randomSeed ? new System.Random(Random.Range(0, 99999999)) : new System.Random(_seed);

        foreach (var layer in _noiseLayers)
        {
            layer.NoiseSettings.Seed = rand.Next();
        }
    }
    void OnValidate()
    {
        foreach (NoiseLayer item in _noiseLayers)
        {
            item.NoiseSettings.Validate();
        }
    }

    public void PreviewMap()
    {

        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
        GenerateSeed();
        UpdateChuncks();
        UpdateWater();

        _generatedChunks = new Dictionary<Vector2, WorldChunk>();
        _loadedChunks = new List<WorldChunk>();

        _playerPos = new Vector2Int();
        _oldPlayerPos = new Vector2Int();
    }
}
