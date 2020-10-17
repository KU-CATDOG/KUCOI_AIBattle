using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField]
    private MapManager mapManager;
    [SerializeField]
    private int thiefScore = 0;

    public void AddThiefScore(int score)
    {
        thiefScore += score;
    }

    private void MoveAgents()
    {
        mapManager.MoveAgents(Police.inst.NextPolicePos(), Thief.inst.NextThiefPos());
    }

    private void MoveThief()
    {

    }

    private void MovePolice()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        mapManager.InitiateMap();
        mapManager.InitiatePolice();
        mapManager.InitiateTreasure();
        mapManager.InitiateThief();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveAgents();
        }
    }
}
