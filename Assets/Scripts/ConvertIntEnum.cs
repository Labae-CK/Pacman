using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConvertIntEnum 
{
    private static Dictionary<int, TileBase.TileType> IntKeyCellTypeConvert = new Dictionary<int, TileBase.TileType>();
    private static Dictionary<TileBase.TileType, int> CellTypeKeyIntConvert = new Dictionary<TileBase.TileType, int>();

    public static TileBase.TileType ConvertIntToCellType(int type)
    {
        if(!IntKeyCellTypeConvert.ContainsKey(type))
        {
            IntKeyCellTypeConvert.Add(type, (TileBase.TileType)type);
        }
        return IntKeyCellTypeConvert[type];
    }

    public static int ConvertCellTypeToInt(TileBase.TileType type)
    {
        if (!CellTypeKeyIntConvert.ContainsKey(type))
        {
            CellTypeKeyIntConvert.Add(type, (int)type);
        }
        return CellTypeKeyIntConvert[type];
    }
}
