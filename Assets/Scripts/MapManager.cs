using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private TileType[,] map;
    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private GameObject policeTile, thiefTile, treasure2Tile, treasure3Tile, treasure5Tile;

    private ThiefInfo[] thieves = new ThiefInfo[4];
    private PoliceInfo[] polices = new PoliceInfo[6];

    private Dictionary<Vector2, TreasureInfo> treasures = new Dictionary<Vector2, TreasureInfo>();

    public TileType GetTileCode(string tileName)
    {
        return (TileType)System.Enum.Parse(typeof(TileType), tileName);
    }

    private Vector2 OnBoardPos(Vector2 boardPos)
    {
        return boardPos - new Vector2(tileMap.size.x, tileMap.size.y) / 2 + new Vector2(0.5f, 0.5f);
    }

    private SightType MapToSight(TileType mapInfo)
    {
        SightType sightType;

        if(mapInfo == TileType.Exit || mapInfo == TileType.Wall)
        {
            sightType = SightType.Invisible;
        }
        else if (mapInfo == TileType.Treasure)
        {
            sightType = SightType.Treasure;
        }
        else //if (mapInfo == TileType.Empty)
        {
            sightType = SightType.Empty;
        }

        return sightType;
    }

    private Vector2 MoveDirToVector2(MoveDir moveDir)
    {
        if (moveDir == MoveDir.Up) return Vector2.up;
        else if (moveDir == MoveDir.Down) return Vector2.down;
        else if (moveDir == MoveDir.Left) return Vector2.left;
        else if (moveDir == MoveDir.Right) return Vector2.right;
        else return Vector2.zero;
    }

    public void InitiateMap()
    {
        map = new TileType[tileMap.size.x, tileMap.size.y];
        for (int x = 0; x < tileMap.size.x; x++)
        {
            for (int y = 0; y < tileMap.size.y; y++)
            {
                map[x, y] = GetTileCode(tileMap.GetTile(new Vector3Int(x - tileMap.size.x / 2, y - tileMap.size.y / 2, 0)).name);
            }
        }
    }

    private bool PolicesOnPos(Vector2 mapPos)
    {
        for (int i = 0; i < polices.Length; i++)
        {
            if (polices[i].mapPos == mapPos)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsTreasureNear(Vector2 mapPos)
    {
        return map[(int)mapPos.x + 1, (int)mapPos.y] == TileType.Treasure || map[(int)mapPos.x + 1, (int)mapPos.y + 1] == TileType.Treasure || map[(int)mapPos.x, (int)mapPos.y + 1] == TileType.Treasure ||
            map[(int)mapPos.x - 1, (int)mapPos.y + 1] == TileType.Treasure || map[(int)mapPos.x - 1, (int)mapPos.y] == TileType.Treasure || map[(int)mapPos.x - 1, (int)mapPos.y - 1] == TileType.Treasure ||
            map[(int)mapPos.x, (int)mapPos.y - 1] == TileType.Treasure || map[(int)mapPos.x + 1, (int)mapPos.y - 1] == TileType.Treasure;
    }

    public void MoveAgents(MoveInfo[] policeMoves, MoveInfo[] thiefMoves)
    {
        //Update virtual position
        for (int i = 0; i < polices.Length; i++)
        {
            Vector2 afterMovePos = polices[i].mapPos + MoveDirToVector2(policeMoves[i].moveDir);

            if(map[(int)afterMovePos.x, (int)afterMovePos.y] != TileType.Wall && map[(int)afterMovePos.x, (int)afterMovePos.y] != TileType.Exit && !IsTreasureNear(afterMovePos))
            {
                polices[i].mapPos = afterMovePos;
                polices[i].angle = policeMoves[i].moveAngle;
            }
            else
            {
                policeMoves[i].moveDir = MoveDir.Neutral;
            }
        }
        for (int i = 0; i < thieves.Length; i++)
        {
            if (thieves[i] != null)
            {
                Vector2 afterMovePos = thieves[i].mapPos + MoveDirToVector2(thiefMoves[i].moveDir);

                if(afterMovePos.x >= 0 && afterMovePos.x <= tileMap.size.x - 1 && afterMovePos.y >= 0 && afterMovePos.y <= tileMap.size.y - 1 && map[(int)afterMovePos.x, (int)afterMovePos.y] != TileType.Wall)
                {
                    thieves[i].mapPos = afterMovePos;
                }
                else
                {
                    thiefMoves[i].moveDir = MoveDir.Neutral;
                }
            }
        }

        //Check if thief was caught
        for(int i = 0; i < thieves.Length; i++)
        {
            if(thieves[i] != null)
            {
                if (map[(int)thieves[i].mapPos.x, (int)thieves[i].mapPos.y] != TileType.Exit &&
                    (PolicesOnPos(thieves[i].mapPos + Vector2.up) || PolicesOnPos(thieves[i].mapPos + Vector2.down) || PolicesOnPos(thieves[i].mapPos + Vector2.left) || PolicesOnPos(thieves[i].mapPos + Vector2.right)))
                {
                    Destroy(thieves[i].tileObject.gameObject);
                    thieves[i] = null;

                    Debug.Log("Thief was caught");
                }
            }
        }

        //Check if thief get treasure
        for (int i = 0; i < thieves.Length; i++)
        {
            if (thieves[i] != null)
            {
                if(map[(int)thieves[i].mapPos.x, (int)thieves[i].mapPos.y] == TileType.Treasure)
                {
                    thieves[i].value += treasures[thieves[i].mapPos].value;
                    map[(int)thieves[i].mapPos.x, (int)thieves[i].mapPos.y] = TileType.Empty;
                    Destroy(treasures[thieves[i].mapPos].tileObject);
                    treasures.Remove(thieves[i].mapPos);
                }
            }
        }

        //Check if thief return exit area with treasure
        for (int i = 0; i < thieves.Length; i++)
        {
            if (thieves[i] != null)
            {
                if (map[(int)thieves[i].mapPos.x, (int)thieves[i].mapPos.y] == TileType.Exit)
                {
                    GameManager.inst.AddThiefScore(thieves[i].value);
                    thieves[i].value = 0;
                }
            }
        }

        //Update real position
        for (int i = 0; i < polices.Length; i++)
        {
            polices[i].tileObject.transform.position = OnBoardPos(polices[i].mapPos);
            polices[i].tileObject.transform.rotation = Quaternion.Euler(0, 0, polices[i].angle);
        }
        for (int i = 0; i < thieves.Length; i++)
        {
            if(thieves[i] != null)
            {
                thieves[i].tileObject.transform.position = OnBoardPos(thieves[i].mapPos);
            }
        }
    }


    #region Police

    public void InitiatePolice()
    {
        PoliceInfo[] initialPolices = Police.inst.InitialPolicePos();
        for (int i = 0; i < 6; i++)
        {
            Vector2 policePos = initialPolices[i].mapPos;
            if (policePos.x <= 0 || policePos.y <= 0 || policePos.x >= tileMap.size.x - 1 || policePos.y >= tileMap.size.y - 1 || map[(int)policePos.x, (int)policePos.y] == TileType.Wall)
            {
                Debug.Log("Illegal police position at " + policePos);
            }
            else
            {
                polices[i] = initialPolices[i];
                polices[i].tileObject = Instantiate(policeTile, OnBoardPos(policePos), Quaternion.Euler(0, 0, initialPolices[i].angle));
            }
        }
    }

    public void InitiateTreasure()
    {
        Vector2[] initialTreasures = Police.inst.InitialTreasurePos();
        
        for(int i = 0; i < 7; i++)
        {
            Vector2 treasurePos = initialTreasures[i];
            if (treasurePos.x == 0 || treasurePos.y == 0 || treasurePos.x == tileMap.size.x - 1 || treasurePos.y == tileMap.size.y - 1 || map[(int)treasurePos.x, (int)treasurePos.y] == TileType.Wall)
            {
                Debug.Log("Illegal treasure position at " + treasurePos);
            }
            else
            {
                bool isPoliceNear = false;
                for(int j = 0; j < polices.Length; j++)
                {
                    Vector2 dist = polices[j].mapPos - treasurePos;
                    if(Mathf.Abs(dist.x) <= 1 && Mathf.Abs(dist.y) <= 1)
                    {
                        Debug.Log("Illegal treasure position at " + treasurePos);
                        isPoliceNear = true;
                        break;
                    }
                }
                if(!isPoliceNear)
                {
                    TreasureInfo newTreasureInfo = new TreasureInfo();
                    newTreasureInfo.mapPos = treasurePos;
                    newTreasureInfo.value = i <= 2 ? 2 : (i <= 5 ? 3 : 5);
                    newTreasureInfo.tileObject = Instantiate(i <= 2 ? treasure2Tile : (i <= 5 ? treasure3Tile : treasure5Tile), OnBoardPos(treasurePos), Quaternion.identity);
                    treasures.Add(treasurePos, newTreasureInfo);

                    map[(int)treasurePos.x, (int)treasurePos.y] = TileType.Treasure;
                }

            }
        }
    }

    private void GetPoliceSight()
    {
        //SightType[,] sightInfos = new SightType[tileMap.size.x, tileMap.size.y];
        //for (int x = 0; x < tileMap.size.x; x++)
        //{
        //    for (int y = 0; y < tileMap.size.y; y++)
        //    {
        //        sightInfos[x, y] = SightType.Invisible;
        //    }
        //}

        //for (int i = 0; i < 1; i++)
        //{
        //    Vector2 policePos = polices[i].mapPos;
        //    int sin = (int)Mathf.Sin(polices[i].angle * Mathf.Deg2Rad);
        //    int cos = (int)Mathf.Cos(polices[i].angle * Mathf.Deg2Rad);

        //    sightInfos[(int)policePos.x, (int)policePos.y] = MapToSight(map[(int)policePos.x, (int)policePos.y]);


        //    for(int x = 0; x < 3; x++)
        //    {
        //        for(int y = -1; y < 2; y++)
        //        {
        //            int realX = cos * x - sin * y + cos + (int)policePos.x;
        //            int realY = sin * x + cos * y + sin + (int)policePos.y;

        //            Debug.Log(realX + "," + realY);

        //        }
        //    }
        //}
    }

    #endregion

    #region Thief

    public void InitiateThief()
    {
        ThiefInfo[] initialThieves = Thief.inst.InitialThiefPos();
        for (int i = 0; i < 4; i++)
        {
            Vector2 thiefPos = initialThieves[i].mapPos;
            if (thiefPos.x != 0 && thiefPos.y != 0 && thiefPos.x != tileMap.size.x - 1 && thiefPos.y != tileMap.size.y - 1)
            {
                Debug.Log("Illegal thief position at " + thiefPos);
            }
            else
            {
                thieves[i] = initialThieves[i];
                thieves[i].tileObject = Instantiate(thiefTile, OnBoardPos(thiefPos), Quaternion.identity);
            }
        }
    }

    private void GetThiefSight(int index)
    {

    }

    #endregion

    private void Start()
    {
    }

    private void Update()
    {

    }
}

public class MoveInfo
{
    public MoveDir moveDir;
    public float moveAngle;
}

public class TileInfo : ScriptableObject
{
    public Vector2 mapPos;
    public GameObject tileObject;
}

public class PoliceInfo : TileInfo
{
    public float angle;
}

public class ThiefInfo : TileInfo
{
    public int value;
}

public class TreasureInfo : TileInfo
{
    public int value;

}
