using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Read CSV file and Generate Map
/// </summary>
public class MapGenerator : MonoBehaviour
{
    #region Attributes
    [SerializeField]
    private string _mapDataPath;
    [SerializeField]
    private GameObject _tilePrefab;
    [SerializeField]
    private GameObject _wallPrefab;
    [SerializeField]
    private GameObject _ghostBoxDoorPrefab;
    [SerializeField]
    private GameObject _bigCoinPrefab;
    [SerializeField]
    private GameObject _coinPrefab;
    [SerializeField]
    private float _cellOffset;

    public Vector2Int MapSize;
    public int[,] MapData;

    public TileBase[,] TileDatas;

    private Vector3 _pacmanSpawnPosition;
    private Vector2Int _pacmanSpawnPositionIndex;
    private List<Vector3> _ghostSpawnPositions;

    private List<Coin> _mapCoins = new List<Coin>();
    #endregion

    public void Initialize()
    {
        SetMapDataSize();
        ReadMapData();

        _ghostSpawnPositions = new List<Vector3>();

        float startX = -(MapSize.x * 0.5f) * _cellOffset;
        float startY = -(MapSize.y * 0.5f) * _cellOffset;

        TileDatas = new TileBase[MapSize.x, MapSize.y];
        for (int x = 0; x < MapData.GetLength(0); x++)
        {
            for (int y = 0; y < MapData.GetLength(1); y++)
            {
                Vector2 pos = new Vector2(startX + x * _cellOffset, startY + y * _cellOffset);
                GameObject cell = MapData[x, y] != ConvertIntEnum.ConvertCellTypeToInt(TileBase.TileType.Wall) ?
                                    Instantiate(_tilePrefab, pos, Quaternion.identity, transform) : Instantiate(_wallPrefab, pos, Quaternion.identity, transform);
                TileBase tileBase = cell.GetComponent<TileBase>();
                TileDatas[x, y] = tileBase;
                TileDatas[x, y].Initialize(this, MapData[x, y], x, y);

                if (MapData[x, y] == ConvertIntEnum.ConvertCellTypeToInt(TileBase.TileType.Coin))
                {
                    GameObject coin = Instantiate(_coinPrefab, pos, Quaternion.identity, transform);
                    _mapCoins.Add(coin.GetComponent<Coin>());
                }
                else if (MapData[x, y] == ConvertIntEnum.ConvertCellTypeToInt(TileBase.TileType.BigCoin))
                {
                    GameObject bigCoin = Instantiate(_bigCoinPrefab, pos, Quaternion.identity, transform);
                    _mapCoins.Add(bigCoin.GetComponent<Coin>());
                }
                else if (MapData[x, y] == ConvertIntEnum.ConvertCellTypeToInt(TileBase.TileType.GhostBoxDoor))
                {
                    GameObject ghostDoor = Instantiate(_ghostBoxDoorPrefab, pos, Quaternion.identity, transform);
                }
                else if(MapData[x,y] == ConvertIntEnum.ConvertCellTypeToInt(TileBase.TileType.PlayerSpawnPosition))
                {
                    _pacmanSpawnPosition = pos;
                    _pacmanSpawnPositionIndex = new Vector2Int(x, y);
                }
                else if (MapData[x, y] == ConvertIntEnum.ConvertCellTypeToInt(TileBase.TileType.GhostSpawnPosition))
                {
                    _ghostSpawnPositions.Add(pos);
                }
            }
        }
    }
    private void ReadMapData()
    {
        using (var reader = new StreamReader(Application.streamingAssetsPath + "/" + _mapDataPath + ".csv"))
        {
            int height = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    MapData[i, height] = int.Parse(values[i]);
                }
                height++;
            }
        }
    }
    private void SetMapDataSize()
    {
        MapSize = new Vector2Int();
        using (var reader = new StreamReader(Application.streamingAssetsPath + "/" + _mapDataPath + ".csv"))
        {
            int height = 0;
            while (!reader.EndOfStream)
            {
                height++;
                var line = reader.ReadLine();
                var values = line.Split(',');
                MapSize.x = values.Length;
            }

            MapSize.y = height;
        }
        MapData = new int[MapSize.x, MapSize.y];
    }

    public int[,] GetMapData()
    {
        return MapData;
    }

    public Vector2Int GetMapSize()
    {
        return MapSize;
    }

    public Vector3 GetPacmanSpawnPosition()
    {
        return _pacmanSpawnPosition;
    }

    public List<Vector3> GetGhostSpawnPositions()
    {
        return _ghostSpawnPositions;
    }

    public Vector2Int GetPacmanSpawnPositionIndex()
    {
        return _pacmanSpawnPositionIndex;
    }

    public float GetTileOffset()
    {
        return _cellOffset;
    }

    public TileBase.AStarValues[,] GetAStarValues()
    {
        TileBase.AStarValues[,] aStarValues = new TileBase.AStarValues[MapSize.x, MapSize.y];
        for (int x = 0; x < MapSize.x; x++)
        {
            for (int y = 0; y < MapSize.y; y++)
            {
                aStarValues[x, y] = TileDatas[x, y].GetAStarValue();
            }
        }

        return aStarValues;
    }

    public void DeleteCoin(Coin coin)
    {
        if(_mapCoins.Contains(coin))
        {
            _mapCoins.Remove(coin);
        }
    }

    public bool IsClear()
    {
        return _mapCoins.Count == 0;
    }

    public List<TileBase.AStarValues> GetNeighbours(TileBase.AStarValues[,] aStarValues, TileBase.AStarValues astarValue)
    {
        List<TileBase.AStarValues> neighbours = new List<TileBase.AStarValues>();

        // Left
        if (astarValue.Tile.IndexPosX - 1 >= 0)
        {
            neighbours.Add(aStarValues[astarValue.Tile.IndexPosX - 1, astarValue.Tile.IndexPosY]);
        }
        // Right
        if (astarValue.Tile.IndexPosX + 1 < MapSize.x)
        {
            neighbours.Add(aStarValues[astarValue.Tile.IndexPosX + 1, astarValue.Tile.IndexPosY]);
        }
        // Up
        if (astarValue.Tile.IndexPosY - 1 >= 0)
        {
            neighbours.Add(aStarValues[astarValue.Tile.IndexPosX, astarValue.Tile.IndexPosY - 1]);
        }
        // Down
        if (astarValue.Tile.IndexPosY + 1 < MapSize.y)
        {
            neighbours.Add(aStarValues[astarValue.Tile.IndexPosX, astarValue.Tile.IndexPosY + 1]);
        }

        return neighbours;
    }
}
