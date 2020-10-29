using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Police
{
    public abstract Vector2[] InitialTreasurePos();
    public abstract PoliceInfo[] InitialPolicePos();
    public abstract MoveInfo[] NextPolicePos();
}
