using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Police
{
    public abstract Vector2[] InitialTreasurePos(TileType[,] baseMap);
    public abstract PoliceInfo[] InitialPolicePos(TileType[,] baseMap);
    public abstract MoveInfo[] NextPolicePos(SightInfo[,] sight, PoliceInfo[] polices);
}
