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
    private GameObject policeTile, thiefTile;

    private ThiefInfo[] thieves = new ThiefInfo[4];
    private PoliceInfo[] polices = new PoliceInfo[6];
    private TreasureInfo[] treasures = new TreasureInfo[7];

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

    #region Police

    public void InitiatePolice()
    {
        PoliceInfo[] initialPolices = Police.inst.InitialPolicePos();
        for (int i = 0; i < 6; i++)
        {
            Vector2 policePos = initialPolices[i].mapPos;
            if (policePos.x == 0 || policePos.y == 0 || policePos.x == tileMap.size.x - 1 || policePos.y == tileMap.size.y - 1)
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

    private void GetPoliceSight()
    {
        SightType[,] sightInfos = new SightType[tileMap.size.x, tileMap.size.y];
        for (int x = 0; x < tileMap.size.x; x++)
        {
            for (int y = 0; y < tileMap.size.y; y++)
            {
                sightInfos[x, y] = SightType.Invisible;
            }
        }

        for (int i = 0; i < 1; i++)
        {
            Vector2 policePos = polices[i].mapPos;
            int sin = (int)Mathf.Sin(polices[i].angle * Mathf.Deg2Rad);
            int cos = (int)Mathf.Cos(polices[i].angle * Mathf.Deg2Rad);

            sightInfos[(int)policePos.x, (int)policePos.y] = MapToSight(map[(int)policePos.x, (int)policePos.y]);


            for(int x = 0; x < 3; x++)
            {
                for(int y = -1; y < 2; y++)
                {
                    int realX = cos * x - sin * y + cos + (int)policePos.x;
                    int realY = sin * x + cos * y + sin + (int)policePos.y;

                    Debug.Log(realX + "," + realY);

                }
            }
        }
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
        map = new TileType[tileMap.size.x, tileMap.size.y];
        for (int x = 0; x < tileMap.size.x; x++)
        {
            for (int y = 0; y < tileMap.size.y; y++)
            {
                map[x, y] = GetTileCode(tileMap.GetTile(new Vector3Int(x - tileMap.size.x / 2, y - tileMap.size.y / 2, 0)).name);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            GetPoliceSight();
        }
    }
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
