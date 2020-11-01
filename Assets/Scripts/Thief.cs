using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Thief
{
    public abstract ThiefInfo[] InitialThiefPos(TileType[,] baseMap);
    public abstract MoveInfo[] NextThiefPos(SightInfo[,] sight, ThiefInfo[] thieves);
}
