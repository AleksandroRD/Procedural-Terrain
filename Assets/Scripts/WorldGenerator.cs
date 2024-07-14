using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField][Range(2, 250)] private int _chunkSize;
    [SerializeField] bool _randomSeed;
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;
    [SerializeField] private int _renderDistance;
    [SerializeField] private Material _terrainMaterial;
    [SerializeField] private Transform _player;

    [Header("Noise Settings")]
    [SerializeField] private List<NoiseSettings> _noiseLayers;

    private Dictionary<Vector2, WorldChunk> _generatedChunks = new Dictionary<Vector2, WorldChunk>();
    private List<WorldChunk> _loadedChunks = new List<WorldChunk>();

    private Vector2Int _playerPos;
    private Vector2Int _oldPlayerPos;

    void Start()
    {
        if(_randomSeed)
        {
            foreach (var layer in _noiseLayers)
            {
                layer.seed = Random.Range(0,99999999);
            }
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

            UpdateChuncks();
        }

        _oldPlayerPos = _playerPos;

    }

    void UpdateChuncks()
    {

        foreach (WorldChunk chunk in _loadedChunks)
        {
            if (Vector2Int.Distance(chunk.coordinates, _playerPos) > _renderDistance)
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
                    _minHeight,
                    _maxHeight,
                    _terrainMaterial
                );

                _loadedChunks.Add(_generatedChunks[pos]);
            }
        }

    }
}
