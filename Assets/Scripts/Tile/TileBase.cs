using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    public enum TileType
    {
        EMPTY = 4,
        Coin = 0,
        Wall = 1,
        BigCoin = 2,
        GhostBoxDoor = 5,
        GhostSpawnPosition = 6,
        PlayerSpawnPosition = 9,
    }

    protected TileType _currentTileType;
    public int IndexPosX;
    public int IndexPosY;
    protected SpriteRenderer _spriteRenderer;
    protected MapGenerator _mapGenerator;

    public Sprite BlankSprite;

    public bool CanWalk;

    [System.Serializable]
    public class AStarValues : IHeapItem<AStarValues>
    {
        public TileBase Tile;

        private TileBase.AStarValues _parentTile;
        private int _gCost;
        private int _hCost;
        private int _heapIndex;

        public int HeapIndex
        {
            get
            {
                return _heapIndex;
            }
            set
            {
                _heapIndex = value;
            }
        }

        public AStarValues(TileBase tile)
        {
            Tile = tile;
        }

        public int GetGCost()
        {
            return _gCost;
        }

        public void SetGCost(int cost)
        {
            _gCost = cost;
        }

        public void SetHCost(int cost)
        {
            _hCost = cost;
        }

        public void SetParent(TileBase.AStarValues parent)
        {
            _parentTile = parent;
        }

        public TileBase.AStarValues GetParent()
        {
            return _parentTile;
        }

        public float GetFCost()
        {
            return _gCost + _hCost;
        }

        public float GetHCost()
        {
            return _hCost;
        }

        public int CompareTo(AStarValues other)
        {
            int compare = GetFCost().CompareTo(other.GetFCost());
            if (compare == 0)
            {
                compare = GetHCost().CompareTo(other.GetHCost());
            }

            return -compare;
        }
    }

    public virtual void Initialize(MapGenerator mapGenerator, int type, int x, int y)
    {
        _currentTileType = ConvertIntEnum.ConvertIntToCellType(type);
        IndexPosX = x;
        IndexPosY = y;
        _mapGenerator = mapGenerator;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = BlankSprite;
        CanWalk = true;
    }

    public AStarValues GetAStarValue()
    {
        return new AStarValues(this);
    }
}
