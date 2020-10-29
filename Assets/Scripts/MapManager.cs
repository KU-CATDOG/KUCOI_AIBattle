﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private TileType[,] map;
    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private GameObject policeTile, thiefTile, treasure2Tile, treasure3Tile, treasure5Tile;

    [SerializeField]
    private Sprite[] policeSprites;

    private ThiefInfo[] thieves = new ThiefInfo[4];
    private PoliceInfo[] polices = new PoliceInfo[6];

    [SerializeField]
    private Image thiefCatchAlert, treasureCaptureAlert;

    private int thiefCount;

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
        bool IsTreasureCaptured = false;
        bool isThiefCaught = false;

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
                if (map[(int)thieves[i].mapPos.x, (int)thieves[i].mapPos.y] != TileType.Exit && (PolicesOnPos(thieves[i].mapPos + Vector2.up) || PolicesOnPos(thieves[i].mapPos + Vector2.down)
                    || PolicesOnPos(thieves[i].mapPos + Vector2.left) || PolicesOnPos(thieves[i].mapPos + Vector2.right) || PolicesOnPos(thieves[i].mapPos)))
                {
                    Destroy(thieves[i].tileObject.gameObject);
                    thieves[i] = null;
                    thiefCount--;

                    isThiefCaught = true;

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
                    GameManager.inst.AddThiefScore(thieves[i].value);
                    thieves[i].value = 0;
                }
            }
        }



        Dictionary<Vector2, List<int>> dupPolicePos = new Dictionary<Vector2, List<int>>();

        for (int i = 0; i < polices.Length; i++)
        {
            if(!dupPolicePos.ContainsKey(polices[i].mapPos))
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
            for(int i = 0; i < dupPoliceCount; i++)
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
                if(thieves[child.Value[i]] != null)
                {
                    thieves[child.Value[i]].tileObject.transform.position = OnBoardPos(thieves[child.Value[i]].mapPos) + new Vector2(-0.5f + (float)(i + 1) / (dupThiefCount + 1), 0);
                }
            }
        }



        //If there is no thief, game over
        if (thiefCount == 0 || treasures.Count == 0)
        {
            GameManager.inst.GameEnd();
        }
        else
        {
            thiefCatchAlert.gameObject.SetActive(isThiefCaught);
            treasureCaptureAlert.gameObject.SetActive(IsTreasureCaptured);
            if (isThiefCaught && IsTreasureCaptured)
            {
                thiefCatchAlert.rectTransform.localPosition = new Vector3(-245, 0);
                treasureCaptureAlert.rectTransform.localPosition = new Vector3(245, 0);
            }
            else if(isThiefCaught)
            {
                thiefCatchAlert.rectTransform.localPosition = Vector3.zero;
            }
            else if(IsTreasureCaptured)
            {
                treasureCaptureAlert.rectTransform.localPosition = Vector3.zero;
            }
        }
    }

    public void ClearGame()
    {
        for (int i = 0; i < polices.Length; i++)
        {
            Destroy(polices[i].tileObject.gameObject);
            polices[i] = null;
        }
        Debug.Log(treasures.Count);
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
    }

    #region Police

    public void InitiatePolice()
    {
        PoliceInfo[] initialPolices = GameManager.inst.InitiatePolicePos();
        for (int i = 0; i < polices.Length; i++)
        {
            Vector2 policePos = initialPolices[i].mapPos;
            if (policePos.x <= 0 || policePos.y <= 0 || policePos.x >= tileMap.size.x - 1 || policePos.y >= tileMap.size.y - 1 || map[(int)policePos.x, (int)policePos.y] == TileType.Wall)
            {
                Debug.Log("Illegal police position at " + policePos);
                GameManager.inst.GameEnd();
            }
            else
            {
                polices[i] = initialPolices[i];
                polices[i].tileObject = Instantiate(policeTile, OnBoardPos(policePos), Quaternion.identity);

                int spriteIndex = (int)initialPolices[i].angle / 90;
                polices[i].tileObject.GetComponent<SpriteRenderer>().sprite = policeSprites[spriteIndex];
            }
        }
    }

    public void InitiateTreasure()
    {
        Vector2[] initialTreasures = GameManager.inst.InitiateTreasurePos();
        
        for(int i = 0; i < 7; i++)
        {
            Vector2 treasurePos = initialTreasures[i];
            if (treasurePos.x == 0 || treasurePos.y == 0 || treasurePos.x == tileMap.size.x - 1 || treasurePos.y == tileMap.size.y - 1 || map[(int)treasurePos.x, (int)treasurePos.y] == TileType.Wall)
            {
                Debug.Log("Illegal treasure position at " + treasurePos);
                GameManager.inst.GameEnd();
                return;
            }
            else
            {
                for(int j = 0; j < polices.Length; j++)
                {
                    Vector2 dist = polices[j].mapPos - treasurePos;
                    if(Mathf.Abs(dist.x) <= 1 && Mathf.Abs(dist.y) <= 1)
                    {
                        Debug.Log("Illegal treasure position at " + treasurePos);
                        GameManager.inst.GameEnd();
                        return;
                    }
                }
                TreasureInfo newTreasureInfo = new TreasureInfo();
                newTreasureInfo.mapPos = treasurePos;
                newTreasureInfo.value = i <= 2 ? 2 : (i <= 5 ? 3 : 5);
                newTreasureInfo.tileObject = Instantiate(i <= 2 ? treasure2Tile : (i <= 5 ? treasure3Tile : treasure5Tile), OnBoardPos(treasurePos), Quaternion.identity);
                treasures.Add(treasurePos, newTreasureInfo);

                map[(int)treasurePos.x, (int)treasurePos.y] = TileType.Treasure;
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
        ThiefInfo[] initialThieves = GameManager.inst.InitiateThiefPos();
        for (int i = 0; i < thieves.Length; i++)
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

        thiefCount = thieves.Length;
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
