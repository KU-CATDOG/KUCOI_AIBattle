using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private MapInfo[,] map;
    public Tilemap tileMap;


    public MapInfo GetTileCode(string tileName)
    {
        return (MapInfo)System.Enum.Parse(typeof(MapInfo), tileName);
    }

    // Start is called before the first frame update
    void Start()
    {
        map = new MapInfo[tileMap.size.x, tileMap.size.y];
        for(int y = 0; y < tileMap.size.y; y++)
        {
            for(int x = 0; x < tileMap.size.x; x++)
            {
                map[x, y] = GetTileCode(tileMap.GetTile(new Vector3Int(x - tileMap.size.x / 2, y - tileMap.size.y / 2, 0)).name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
