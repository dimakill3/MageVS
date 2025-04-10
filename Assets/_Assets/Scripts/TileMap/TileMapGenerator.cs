using _Assets.Scripts.Core.Infrastructure.Configs;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace _Assets.Scripts.TileMap
{
    public class TileMapGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int startPosition = new(0, 0);
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase[] availableTiles;
        [SerializeField] private int borderTileWidth = 15;

        private int _mapWidth = 1;
        private int _mapHeight = 1;

        [Inject]
        private void Construct(GameConfig gameConfig)
        {
            _mapWidth = gameConfig.MapSize.x + borderTileWidth;
            _mapHeight = gameConfig.MapSize.y + borderTileWidth;
        }

        private void Awake() =>
            GenerateMap();

        private void GenerateMap()
        {
            if (tilemap == null)
            {
                Debug.LogError("Error: Tilemap isMissing!");
                return;
            }

            if (availableTiles == null || availableTiles.Length == 0)
            {
                Debug.LogError("Error: Where are no Tiles available!");
                return;
            }
            
            var randomSeed = Random.Range(0, 999999);
            Random.InitState(randomSeed);

            ClearTileMap();
            
            var actualStart = new Vector2Int(
                startPosition.x - _mapWidth / 2,
                startPosition.y - _mapHeight / 2
            );
            
            
            for (var x = 0; x < _mapWidth; x++)
                for (var y = 0; y < _mapHeight; y++)
                {
                    var position = new Vector3Int(actualStart.x + x, actualStart.y + y, 0);
                    var selectedTile = availableTiles[Random.Range(0, availableTiles.Length)];
                    tilemap.SetTile(position, selectedTile);
                }
        }

        [ContextMenu("ClearTileMap")]
        private void ClearTileMap() =>
            tilemap.ClearAllTiles();
    }
}