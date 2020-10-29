using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Thief
{
    public abstract ThiefInfo[] InitialThiefPos();
    public abstract MoveInfo[] NextThiefPos();
}
