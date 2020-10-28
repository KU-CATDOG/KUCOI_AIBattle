using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField]
    private Text turnText;

    [SerializeField]
    private MapManager mapManager;
    [SerializeField]
    private int thiefScore = 0;

    private bool isGameEnded = false;
    private int turnCount = 0;
    private int maxTurn = 50;

    private Police policeAI;
    private Thief thiefAI;

    private Thread policeThread, thiefThread;

    private MoveInfo[] nextPoliceMove, nextThiefMove;

    private float turnTimer = 0;
    private float turnDelay = 1;

    private bool isGameStarted = false;

    public void AddThiefScore(int score)
    {
        thiefScore += score;
    }

    private void MoveAgents()
    {
        nextPoliceMove = nextThiefMove = null;

        policeThread = new Thread(new ThreadStart(MovePolice));
        thiefThread = new Thread(new ThreadStart(MoveThief));

        policeThread.Start();
        thiefThread.Start();
        Thread.Sleep(1000);
        policeThread.Abort();
        thiefThread.Abort();

        if (nextPoliceMove == null)
        {
            Debug.Log("Invalid police move");
            GameEnd();
        }
        else if(nextThiefMove == null)
        {
            Debug.Log("Invalid thief move");
            GameEnd();
        }
        else
        {
            mapManager.MoveAgents(nextPoliceMove, nextThiefMove);
        }
    }

    public void GameEnd()
    {
        isGameEnded = true;
        //isGameStarted = false;
        Debug.Log("Game end");
    }

    public Vector2[] InitiateTreasurePos()
    {
        return policeAI.InitialTreasurePos();
    }

    public PoliceInfo[] InitiatePolicePos()
    {
        return policeAI.InitialPolicePos();
    }

    public MoveInfo[] GetNextPolicePos()
    {
        return policeAI.NextPolicePos();
    }


    public ThiefInfo[] InitiateThiefPos()
    {
        return thiefAI.InitialThiefPos();
    }

    public MoveInfo[] GetNextThiefPos()
    {
        return thiefAI.NextThiefPos();
    }




    private void MovePolice()
    {
        nextPoliceMove = policeAI.NextPolicePos();
    }

    private void MoveThief()
    {
        nextThiefMove = thiefAI.NextThiefPos();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    int round = 0;

    private void StartGame()
    {
        string policeName = Resources.LoadAll("AI/" + (round % 2 == 0 ? "1" : "2"))[0].name + ".PoliceAI";
        string thiefName = Resources.LoadAll("AI/" + (round % 2 == 0 ? "2" : "1"))[0].name + ".ThiefAI";

        Debug.Log(policeName + thiefName);

        policeAI = Activator.CreateInstance(Type.GetType(policeName)) as Police;
        thiefAI = Activator.CreateInstance(Type.GetType(thiefName)) as Thief;

        mapManager.InitiateMap();
        mapManager.InitiatePolice();
        mapManager.InitiateTreasure();
        mapManager.InitiateThief();

        turnText.text = turnCount + "턴";
        turnTimer = Time.time;
        isGameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameStarted && Input.GetKeyDown(KeyCode.A))
        {
            StartGame();
        }

        else if (isGameStarted && !isGameEnded)
        {
            if (turnCount == maxTurn)
            {
                GameEnd();
            }
            else if (Time.time - turnTimer > turnDelay)
            {

                MoveAgents();
                turnCount++;
                turnText.text = turnCount + "턴";
                turnTimer = Time.time;
            }
        }
        else if (isGameEnded && Input.GetKeyDown(KeyCode.A))
        {
            mapManager.ClearGame();
            turnCount = 0;
            thiefScore = 0;
            round++;
            isGameEnded = false;
            StartGame();
        }
    }

    void OnApplicationQuit()
    {
        if (policeThread.IsAlive)
        {
            policeThread.Abort();
        }
        if (thiefThread.IsAlive)
        {
            thiefThread.Abort();
        }
    }
}
