using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : TileBase
{
    public Sprite WallSprite;

    public override void Initialize(MapGenerator mapGenerator, int type, int x, int y)
    {
        _currentTileType = ConvertIntEnum.ConvertIntToCellType(type);
        IndexPosX = x;
        IndexPosY = y;
        _mapGenerator = mapGenerator;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = WallSprite;
        CanWalk = false;
    }
}
