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
    private int mapSizeX, mapSizeY;
    [SerializeField]
    private GameObject policeTile, thiefTile, treasure2Tile, treasure3Tile, treasure5Tile;

    [SerializeField]
    private Tile sightTile, floorTile;

    [SerializeField]
    private Sprite[] policeSprites;

    private ThiefInfo[] thieves = new ThiefInfo[4];
    private PoliceInfo[] polices = new PoliceInfo[6];
    [SerializeField]
    private GameObject[] caughtThieves = new GameObject[4];

    private int thiefCount;

    private Dictionary<Vector2, TreasureInfo> treasures = new Dictionary<Vector2, TreasureInfo>();

    private SightRay policeSightRule;

    public TileType[,] baseMapGetter { get { return (TileType[,])map.Clone(); } }

    public TileType GetTileCode(string tileName)
    {
        return (tileName != "WallBottom" && tileName != "WallTop") ? (TileType)System.Enum.Parse(typeof(TileType), tileName) : TileType.Wall;
    }

    private Vector2 OnBoardPos(Vector2 boardPos)
    {
        return boardPos - new Vector2(mapSizeX, mapSizeY) / 2 + new Vector2(0.5f, 0.5f);
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

        mapSizeX = tileMap.size.x;
        mapSizeY = tileMap.size.y;
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                map[x, y] = GetTileCode(tileMap.GetTile(new Vector3Int(x - mapSizeX / 2, y - mapSizeY / 2, 0)).name);
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


    public IEnumerator MoveAgents(MoveInfo[] policeMoves, MoveInfo[] thiefMoves)
    {
        bool IsTreasureCaptured = false;
        bool isThiefCaught = false;

        //Update virtual position
        for (int i = 0; i < polices.Length; i++)
        {
            Vector2 afterMovePos = polices[i].mapPos + MoveDirToVector2(policeMoves[i].moveDir);

            if(policeMoves[i].moveAngle != MoveAngle.Null && 
                map[(int)afterMovePos.x, (int)afterMovePos.y] != TileType.Wall && map[(int)afterMovePos.x, (int)afterMovePos.y] != TileType.Exit && !IsTreasureNear(afterMovePos))
            {
                polices[i].mapPos = afterMovePos;
                polices[i].angle = (int)policeMoves[i].moveAngle;
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

                if(afterMovePos.x >= 0 && afterMovePos.x <= mapSizeX - 1 && afterMovePos.y >= 0 && afterMovePos.y <= mapSizeY - 1 && map[(int)afterMovePos.x, (int)afterMovePos.y] != TileType.Wall)
                {
                    thieves[i].mapPos = afterMovePos;
                }
                else
                {
                    thiefMoves[i].moveDir = MoveDir.Neutral;
                }
            }
        }





        Dictionary<Vector2, List<int>> dupPolicePos = new Dictionary<Vector2, List<int>>();

        for (int i = 0; i < polices.Length; i++)
        {
            if (!dupPolicePos.ContainsKey(polices[i].mapPos))
            {
                List<int> indexList = new List<int>();
                dupPolicePos.Add(polices[i].mapPos, indexList);
            }
            dupPolicePos[polices[i].mapPos].Add(i);
        }

        Dictionary<Vector2, List<int>> dupThiefPos = new Dictionary<Vector2, List<int>>();

        for (int i = 0; i < thieves.Length; i++)
        {
            if (thieves[i] != null)
            {
                if (!dupThiefPos.ContainsKey(thieves[i].mapPos))
                {
                    List<int> indexList = new List<int>();
                    dupThiefPos.Add(thieves[i].mapPos, indexList);
                }
                dupThiefPos[thieves[i].mapPos].Add(i);
            }
        }

        //Update real position
        foreach (var child in dupPolicePos)
        {
            int dupPoliceCount = child.Value.Count;
            for (int i = 0; i < dupPoliceCount; i++)
            {
                polices[child.Value[i]].tileObject.transform.position = OnBoardPos(polices[child.Value[i]].mapPos) + new Vector2(-0.5f + (float)(i + 1) / (dupPoliceCount + 1), 0);

                int spriteIndex = (int)polices[child.Value[i]].angle / 90;
                polices[child.Value[i]].tileObject.GetComponent<SpriteRenderer>().sprite = policeSprites[spriteIndex];
            }
        }

        foreach (var child in dupThiefPos)
        {
            int dupThiefCount = child.Value.Count;
            for (int i = 0; i < dupThiefCount; i++)
            {
                if (thieves[child.Value[i]] != null)
                {
                    thieves[child.Value[i]].tileObject.transform.position = OnBoardPos(thieves[child.Value[i]].mapPos) + new Vector2(-0.5f + (float)(i + 1) / (dupThiefCount + 1), 0);
                }
            }
        }


        SightInfo[,] currentSight = GetPoliceSight();
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (map[x, y] == TileType.Empty || map[x, y] == TileType.Treasure)
                {
                    tileMap.SetTile(new Vector3Int(x - mapSizeX / 2, y - mapSizeY / 2, 0), currentSight[x, y].isVisible ? sightTile : floorTile);
                }
            }
        }


        yield return new WaitForSeconds(1);

        //Check if thief was caught
        for (int i = 0; i < thieves.Length; i++)
        {
            if(thieves[i] != null)
            {
                if (map[(int)thieves[i].mapPos.x, (int)thieves[i].mapPos.y] != TileType.Exit && (PolicesOnPos(thieves[i].mapPos + Vector2.up) || PolicesOnPos(thieves[i].mapPos + Vector2.down)
                    || PolicesOnPos(thieves[i].mapPos + Vector2.left) || PolicesOnPos(thieves[i].mapPos + Vector2.right) || PolicesOnPos(thieves[i].mapPos)))
                {
                    Destroy(thieves[i].tileObject.gameObject);
                    thieves[i] = null;
                    thiefCount--;

                    isThiefCaught = true;
                    caughtThieves[thieves.Length - thiefCount - 1].SetActive(true);

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
                    thieves[i].treasures.Add(treasures[thieves[i].mapPos].value);
                    map[(int)thieves[i].mapPos.x, (int)thieves[i].mapPos.y] = TileType.Empty;
                    Destroy(treasures[thieves[i].mapPos].tileObject);
                    treasures.Remove(thieves[i].mapPos);

                    IsTreasureCaptured = true;
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
                    GameManager.inst.AddThiefScore(thieves[i].treasures);
                    thieves[i].treasures.Clear();
                }
            }
        }


        if (isThiefCaught)
        {
            yield return StartCoroutine(GameManager.inst.CatchThiefRoutine());
        }
        if (IsTreasureCaptured)
        {
            yield return StartCoroutine(GameManager.inst.CaptureTreasureRoutine());
        }

        //If there is no thief or treasure, game over
        if (thiefCount == 0 || treasures.Count == 0)
        {
            GameManager.inst.RoundEnd();
        }
    }

    public void ClearGame()
    {
        for (int i = 0; i < polices.Length; i++)
        {
            Destroy(polices[i].tileObject.gameObject);
            polices[i] = null;
        }

        if(treasures.Count != 0)
        {
            TreasureInfo[] remainings = new TreasureInfo[treasures.Count];
            treasures.Values.CopyTo(remainings, 0);
            for (int i = 0; i < treasures.Count; i++)
            {
                Destroy(remainings[i].tileObject);
            }
        }
        treasures.Clear();
        for (int i = 0; i < thieves.Length; i++)
        {
            if(thieves[i] != null)
            {
                Destroy(thieves[i].tileObject.gameObject);
                thieves[i] = null;
            }
        }
        for(int x = 0; x < mapSizeX; x++)
        {
            for(int y = 0; y < mapSizeY; y++)
            {
                if(map[x, y] == TileType.Empty || map[x, y] == TileType.Treasure)
                {
                    tileMap.SetTile(new Vector3Int(x - mapSizeX / 2, y - mapSizeY / 2, 0), floorTile);
                }
            }
        }
    }

    #region Police

    public bool CheckInitialPolicePossible(PoliceInfo[] initialPolices)
    {
        for (int i = 0; i < polices.Length; i++)
        {
            Vector2 policePos = initialPolices[i].mapPos;
            if (policePos.x <= 0 || policePos.y <= 0 || policePos.x >= mapSizeX - 1 || policePos.y >= mapSizeY - 1 || map[(int)policePos.x, (int)policePos.y] == TileType.Wall)
            {
                Debug.Log("Illegal police position at " + policePos);
                return false;
            }
        }

        return true;
    }

    public void InitiatePolice(PoliceInfo[] initialPolices)
    {
        Dictionary<Vector2, List<int>> dupPolicePos = new Dictionary<Vector2, List<int>>();

        for (int i = 0; i < initialPolices.Length; i++)
        {
            if (!dupPolicePos.ContainsKey(initialPolices[i].mapPos))
            {
                List<int> indexList = new List<int>();
                dupPolicePos.Add(initialPolices[i].mapPos, indexList);
            }
            dupPolicePos[initialPolices[i].mapPos].Add(i);
        }

        foreach (var child in dupPolicePos)
        {
            int dupPoliceCount = child.Value.Count;
            for (int i = 0; i < dupPoliceCount; i++)
            {
                Vector2 policePos = initialPolices[child.Value[i]].mapPos;
                polices[child.Value[i]] = initialPolices[child.Value[i]];
                polices[child.Value[i]].tileObject = Instantiate(policeTile, OnBoardPos(polices[child.Value[i]].mapPos) + new Vector2(-0.5f + (float)(i + 1) / (dupPoliceCount + 1), 0), Quaternion.identity);

                int spriteIndex = (int)polices[child.Value[i]].angle / 90;
                polices[child.Value[i]].tileObject.GetComponent<SpriteRenderer>().sprite = policeSprites[spriteIndex];
            }
        }

        SightInfo[,] currentSight = GetPoliceSight();
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (map[x, y] == TileType.Empty || map[x, y] == TileType.Treasure)
                {
                    tileMap.SetTile(new Vector3Int(x - mapSizeX / 2, y - mapSizeY / 2, 0), currentSight[x, y].isVisible ? sightTile : floorTile);
                }
            }
        }
    }


    public bool CheckInitialTreasurePossible(Vector2[] initialTreasures, PoliceInfo[] initialPolices)
    {
        for (int i = 0; i < 7; i++)
        {
            Vector2 treasurePos = initialTreasures[i];
            if (treasurePos.x == 0 || treasurePos.y == 0 || treasurePos.x == mapSizeX - 1 || treasurePos.y == mapSizeY - 1 || map[(int)treasurePos.x, (int)treasurePos.y] == TileType.Wall)
            {
                Debug.Log("Illegal treasure position at " + treasurePos);
                return false;
            }
            else
            {
                for (int j = 0; j < polices.Length; j++)
                {
                    Vector2 dist = initialPolices[j].mapPos - treasurePos;
                    if (Mathf.Abs(dist.x) <= 1 && Mathf.Abs(dist.y) <= 1)
                    {
                        Debug.Log("Illegal treasure position at " + treasurePos);
                        return false;
                    }
                }

                for (int j = 0; j < i; j++)
                {
                    Vector2 dist = treasurePos - initialTreasures[j];
                    if (Mathf.Abs(dist.x) + Mathf.Abs(dist.y) < 7)
                    {
                        Debug.Log("Illegal treasure position at " + treasurePos);
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void InitiateTreasure(Vector2[] initialTreasures)
    {        
        for(int i = 0; i < 7; i++)
        {
            Vector2 treasurePos = initialTreasures[i];

            TreasureInfo newTreasureInfo = new TreasureInfo();
            newTreasureInfo.mapPos = treasurePos;
            newTreasureInfo.value = i <= 2 ? 2 : (i <= 5 ? 3 : 5);
            newTreasureInfo.tileObject = Instantiate(i <= 2 ? treasure2Tile : (i <= 5 ? treasure3Tile : treasure5Tile), OnBoardPos(treasurePos), Quaternion.identity);
            treasures.Add(treasurePos, newTreasureInfo);

            map[(int)treasurePos.x, (int)treasurePos.y] = TileType.Treasure;
        }
    }

    public SightInfo[,] GetPoliceSight()
    {
        if(policeSightRule == null)
        {
            policeSightRule =
                new SightRay(0, 0, new SightRay[]
                {
                    new SightRay(1, 0, new SightRay[]
                    {
                        new SightRay(0, -1, new SightRay[]
                        {
                            new SightRay(1, 0)
                        }),
                        new SightRay(1, 0, new SightRay[]
                        {
                            new SightRay(1, 0)
                        }),
                        new SightRay(0, 1, new SightRay[]
                        {
                            new SightRay(1, 0)
                        })
                    })
                });
        }
        SightInfo[,] sightInfos = new SightInfo[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                sightInfos[x, y] = new SightInfo();
            }
        }

        for (int i = 0; i < polices.Length; i++)
        {
            Vector2Int policePos = new Vector2Int((int)polices[i].mapPos.x, (int)polices[i].mapPos.y);
            int sin = (int)Mathf.Sin(polices[i].angle * Mathf.Deg2Rad);
            int cos = (int)Mathf.Cos(polices[i].angle * Mathf.Deg2Rad);

            PoliceSightDFS(sightInfos, policePos, sin, cos, policeSightRule);
        }

        return sightInfos;
    }
    private void PoliceSightDFS(SightInfo[,] infos, Vector2Int pos, int sin, int cos, SightRay ray)
    {
        Vector2Int nextPos = new Vector2Int(cos * ray.ray.x - sin * ray.ray.y + pos.x, sin * ray.ray.x + cos * ray.ray.y + pos.y);

        if (nextPos.x >= 0 && nextPos.x < mapSizeX &&
            nextPos.y >= 0 && nextPos.y < mapSizeY &&
            map[nextPos.x, nextPos.y] != TileType.Exit && map[nextPos.x, nextPos.y] != TileType.Wall)
        {
            int enemy = 0, treasure = 0;
            foreach(ThiefInfo thief in thieves)
            {
                if (thief != null && (int)thief.mapPos.x == nextPos.x && (int)thief.mapPos.y == nextPos.y)
                {
                    enemy++;
                    foreach (int value in thief.treasures)
                    {
                        treasure += value;
                    }
                }
            }
            if(treasures.TryGetValue(new Vector2(nextPos.x, nextPos.y), out TreasureInfo o))
            {
                treasure += o.value;
            }
            infos[nextPos.x, nextPos.y] = new SightInfo(true, enemy, treasure);

            foreach (SightRay child in ray.next)
            {
                PoliceSightDFS(infos, nextPos, sin, cos, child);
            }
        }
    }

    #endregion

    #region Thief

    public bool CheckInitialThiefPossible(ThiefInfo[] initialThieves)
    {
        for (int i = 0; i < thieves.Length; i++)
        {
            Vector2 thiefPos = initialThieves[i].mapPos;
            if (thiefPos.x != 0 && thiefPos.y != 0 && thiefPos.x != mapSizeX - 1 && thiefPos.y != mapSizeY - 1)
            {
                Debug.Log("Illegal thief position at " + thiefPos);
                return false;
            }
        }
        return true;
    }

    public void InitiateThief(ThiefInfo[] initialThieves)
    {
        Dictionary<Vector2, List<int>> dupThiefPos = new Dictionary<Vector2, List<int>>();

        for (int i = 0; i < initialThieves.Length; i++)
        {
            if (initialThieves[i] != null)
            {
                if (!dupThiefPos.ContainsKey(initialThieves[i].mapPos))
                {
                    List<int> indexList = new List<int>();
                    dupThiefPos.Add(initialThieves[i].mapPos, indexList);
                }
                dupThiefPos[initialThieves[i].mapPos].Add(i);
            }
        }

        foreach (var child in dupThiefPos)
        {
            int dupThiefCount = child.Value.Count;
            for (int i = 0; i < dupThiefCount; i++)
            {
                Vector2 thiefPos = initialThieves[child.Value[i]].mapPos;
                thieves[child.Value[i]] = initialThieves[child.Value[i]];
                thieves[child.Value[i]].tileObject = Instantiate(thiefTile, OnBoardPos(thieves[child.Value[i]].mapPos) + new Vector2(-0.5f + (float)(i + 1) / (dupThiefCount + 1), 0), Quaternion.identity);
            }
        }

        thiefCount = thieves.Length;
    }

    public SightInfo[,] GetThiefSight()
    {
        SightInfo[,] sightInfos = new SightInfo[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                sightInfos[x, y] = new SightInfo();
            }
        }
        for (int i = 0; i < thieves.Length; i++)
        {
            if(thieves[i] != null)
            {
                Vector2Int thiefPos = new Vector2Int((int)thieves[i].mapPos.x, (int)thieves[i].mapPos.y);

                Vector2Int minCoord = new Vector2Int(Mathf.Clamp(thiefPos.x - 2, 0, mapSizeX - 1), Mathf.Clamp(thiefPos.y - 2, 0, mapSizeY - 1));
                Vector2Int maxCoord = new Vector2Int(Mathf.Clamp(thiefPos.x + 2, 0, mapSizeX - 1), Mathf.Clamp(thiefPos.y + 2, 0, mapSizeY - 1));
                for (int x = minCoord.x; x <= maxCoord.x; x++)
                {
                    for (int y = minCoord.y; y <= maxCoord.y; y++)
                    {
                        if (map[x, y] != TileType.Wall)
                        {
                            int enemy = 0, treasure = 0;
                            foreach (PoliceInfo police in polices)
                            {
                                if ((int)police.mapPos.x == x && (int)police.mapPos.y == y)
                                {
                                    enemy++;
                                }
                            }
                            if (treasures.TryGetValue(new Vector2(x, y), out TreasureInfo o))
                            {
                                treasure += o.value;
                            }
                            sightInfos[x, y] = new SightInfo(true, enemy, treasure);
                        }
                    }
                }
            }
        }
        return sightInfos;
    }

    #endregion
}

public class MoveInfo
{
    public MoveDir moveDir;
    public MoveAngle moveAngle;
}

public class TileInfo
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
    public List<int> treasures;

    public ThiefInfo()
    {
        treasures = new List<int>();
    }
}

public class TreasureInfo : TileInfo
{
    public int value;
}
/// <summary>
/// 아군 유닛이 시야범위를 통해 특정 칸을 관찰한 결과
/// </summary>
public class SightInfo
{
    /// <summary>
    /// 아군 유닛이 해당 칸을 관찰했는가
    /// </summary>
    public bool isVisible { get { return _isVisible; } }

    /// <summary>
    /// 해당 칸에 있는 상대 유닛의 개수
    /// </summary>
    public int enemyAgentNum { get { return _enemyAgentNum; } }

    /// <summary>
    /// 해당 칸에 있는 보물의 양, 도둑이 소유중인 보물도 관측됨
    /// </summary>
    public int treasureNum { get { return _treasureNum; } }

    private bool _isVisible;
    private int _enemyAgentNum;
    private int _treasureNum;

    public SightInfo (bool visible, int agent, int treasure)
    {
        _isVisible = visible;
        _enemyAgentNum = agent;
        _treasureNum = treasure;
    }
    public SightInfo()
    {
        _isVisible = false;
        _enemyAgentNum = 0;
        _treasureNum = 0;
    }
}


public class SightRay
{
    public Vector2Int ray;
    public List<SightRay> next;

    public SightRay(int x, int y)
    {
        ray = new Vector2Int(x, y);
        next = new List<SightRay>();
    }
    public SightRay(int x, int y, SightRay[] list)
    {
        ray = new Vector2Int(x, y);
        next = new List<SightRay>(list);
    }
}
