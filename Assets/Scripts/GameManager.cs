using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : SingletonBehaviour<GameManager>
{
    public Text turnText;
    public Text introCSTeamNameText, introSCSTeamNameText;
    public Text csTeamNameText, scsTeamNameText;
    public Text winnerText;
    public Text treasure2ScoreText, treasure3ScoreText, treasure5ScoreText, totalScoreText;

    [SerializeField]
    private MapManager mapManager;
    private int[] collectedTreasures = new int[3];
    private int[] thiefScore = { 0, 0 };

    public Image thiefCatchAlert, treasureCaptureAlert;
    public Image thiefErrorAlert, policeErrorAlert, calculatingImage;
    public Image thiefIcon, policeIcon;
    public GameObject roundEndPanel, winnerPanel, roundChangeIcon;


    private string csTeamName;
    private string scsTeamName;

    private bool isGameEnded = false;
    private int maxTurn = 3;
    private int turnCount;
    private bool isMoveEnded = false;

    private Police policeAI;
    private Thief thiefAI;

    private Thread policeThread, thiefThread;


    private Vector2[] initialTreasures;
    private PoliceInfo[] initialPolices;
    private ThiefInfo[] initialThieves;

    private MoveInfo[] nextPoliceMove, nextThiefMove;

    private bool isCSPolice = false;
    private int round = 0;

    private bool isGameStarted = false;

    public void AddThiefScore(List<int> treasures)
    {
        for(int i = 0; i < treasures.Count; i++)
        {
            collectedTreasures[treasures[i] == 2 ? 0 : treasures[i] == 3 ? 1 : 2]++;
            thiefScore[round % 2 == 0 ? 1 : 0] += treasures[i];
        }
    }

    private IEnumerator InitiateAgents()
    {
        int policeErrorCount = 0, thiefErrorCount = 0;
        while(policeErrorCount < 3 && thiefErrorCount < 3)
        {
            policeThread = new Thread(new ThreadStart(InitiatePolicePos));
            thiefThread = new Thread(new ThreadStart(InitiateThiefPos));

            calculatingImage.gameObject.SetActive(true);
            policeThread.Start();
            thiefThread.Start();
            yield return new WaitForSeconds(1);
            policeThread.Abort();
            thiefThread.Abort();
            calculatingImage.gameObject.SetActive(false);

            if (initialTreasures == null || initialPolices == null)
            {
                Debug.Log("Police error");
                policeErrorAlert.gameObject.SetActive(true);
                policeErrorCount++;
                yield return new WaitForSeconds(1);
                policeErrorAlert.gameObject.SetActive(false);
            }
            else if (initialThieves == null)
            {
                Debug.Log("Thief error");
                thiefErrorAlert.gameObject.SetActive(true);
                thiefErrorCount++;
                yield return new WaitForSeconds(1);
                thiefErrorAlert.gameObject.SetActive(false);
            }
            else
            {
                mapManager.InitiatePolice(initialPolices);
                mapManager.InitiateTreasure(initialTreasures);
                mapManager.InitiateThief(initialThieves);

                turnText.text = turnCount + "턴";
                isGameStarted = true;
                isMoveEnded = true;
                yield break;
            }
        }
        winnerPanel.SetActive(true);
        if (policeErrorCount == 3)
        {
            Debug.Log("Police lose");
            Debug.Log("Game end");
            winnerText.text = isCSPolice ? scsTeamName : csTeamName;
        }
        else if (thiefErrorCount == 3)
        {
            Debug.Log("Thief lose");
            Debug.Log("Game end");
            winnerText.text = isCSPolice ? csTeamName : scsTeamName;
        }
    }

    private IEnumerator MoveAgents()
    {
        isMoveEnded = false;
        nextPoliceMove = nextThiefMove = null;

        policeThread = new Thread(new ThreadStart(MovePolice));
        thiefThread = new Thread(new ThreadStart(MoveThief));

        calculatingImage.gameObject.SetActive(true);
        policeThread.Start();
        thiefThread.Start();
        yield return new WaitForSeconds(1);
        policeThread.Abort();
        thiefThread.Abort();
        calculatingImage.gameObject.SetActive(false);

        if (nextPoliceMove == null)
        {
            Debug.Log("Invalid police move");

            MoveInfo[] pos = new MoveInfo[6];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new MoveInfo();
            }

            pos[0].moveDir = MoveDir.Neutral;
            pos[1].moveDir = MoveDir.Neutral;
            pos[2].moveDir = MoveDir.Neutral;
            pos[3].moveDir = MoveDir.Neutral;
            pos[4].moveDir = MoveDir.Neutral;
            pos[5].moveDir = MoveDir.Neutral;

            pos[0].moveAngle = -1;
            pos[1].moveAngle = -1;
            pos[2].moveAngle = -1;
            pos[3].moveAngle = -1;
            pos[4].moveAngle = -1;
            pos[5].moveAngle = -1;

            nextPoliceMove = pos;
        }
        else if(nextThiefMove == null)
        {
            Debug.Log("Invalid thief move");

            MoveInfo[] pos = new MoveInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new MoveInfo();
            }

            pos[0].moveDir = MoveDir.Neutral;
            pos[1].moveDir = MoveDir.Neutral;
            pos[2].moveDir = MoveDir.Neutral;
            pos[3].moveDir = MoveDir.Neutral;

            nextThiefMove = pos;
        }
        turnCount--;
        yield return StartCoroutine(mapManager.MoveAgents(nextPoliceMove, nextThiefMove));

        turnText.text = turnCount + "턴";
        isMoveEnded = true;
    }

    public IEnumerator CatchThiefRoutine()
    {
        thiefCatchAlert.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        thiefCatchAlert.gameObject.SetActive(false);
    }

    public IEnumerator CaptureTreasureRoutine()
    {
        treasureCaptureAlert.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        treasureCaptureAlert.gameObject.SetActive(false);
    }

    public void InitiatePolicePos()
    {
        initialPolices = policeAI.InitialPolicePos();
        initialTreasures = policeAI.InitialTreasurePos();
    }

    public void InitiateThiefPos()
    {
        initialThieves = thiefAI.InitialThiefPos();
    }

    private void MovePolice()
    {
        nextPoliceMove = policeAI.NextPolicePos();
    }

    private void MoveThief()
    {
        nextThiefMove = thiefAI.NextThiefPos();
    }

    public void StartGame()
    {
        turnCount = maxTurn;
        csTeamNameText.text = csTeamName;
        scsTeamNameText.text = scsTeamName;

        isCSPolice = round % 2 == 0;

        string policeName = (isCSPolice ? csTeamName : scsTeamName) + ".PoliceAI";
        string thiefName = (isCSPolice ? scsTeamName : csTeamName) + ".ThiefAI";

        Debug.Log(policeName);
        Debug.Log(thiefName);

        policeAI = Activator.CreateInstance(Type.GetType(policeName)) as Police;
        thiefAI = Activator.CreateInstance(Type.GetType(thiefName)) as Thief;

        thiefCatchAlert.rectTransform.localPosition = new Vector3(245 * (isCSPolice ? -1 : 1), 0);
        treasureCaptureAlert.rectTransform.localPosition = new Vector3(245 * (isCSPolice ? 1 : -1), 0);

        policeErrorAlert.rectTransform.localPosition = new Vector3(245 * (isCSPolice ? -1 : 1), 0);
        thiefErrorAlert.rectTransform.localPosition = new Vector3(245 * (isCSPolice ? 1 : -1), 0);

        policeIcon.rectTransform.localPosition = new Vector3(780 * (isCSPolice ? -1 : 1), policeIcon.rectTransform.localPosition.y);
        thiefIcon.rectTransform.localPosition = new Vector3(780 * (isCSPolice ? 1 : -1), thiefIcon.rectTransform.localPosition.y);

        mapManager.InitiateMap();
        StartCoroutine(InitiateAgents());

    }

    public void RestartGame()
    {
        roundEndPanel.SetActive(false);
        mapManager.ClearGame();
        round++;
        isGameEnded = false;
        isMoveEnded = false;

        collectedTreasures = new int[3];
        thiefScore = new int[2];

        StartGame();
    }

    private IEnumerator RoundEndRoutine()
    {
        isGameEnded = true;
        Debug.Log("Round end");
        roundEndPanel.SetActive(true);
        treasure2ScoreText.text = "X   " + collectedTreasures[0] + "   =   " + (2 * collectedTreasures[0]) + "점";
        treasure3ScoreText.text = "X   " + collectedTreasures[1] + "   =   " + (3 * collectedTreasures[1]) + "점";
        treasure5ScoreText.text = "X   " + collectedTreasures[2] + "   =   " + (5 * collectedTreasures[2]) + "점";
        totalScoreText.text = "총점 : " + thiefScore[round % 2 == 0 ? 1 : 0] + "점";

        yield return new WaitForSeconds(5);
        if (round % 2 == 1)
        {
            roundEndPanel.SetActive(false);
            Debug.Log("Game end");
            winnerPanel.SetActive(true);
            Debug.Log(thiefScore[0]);
            Debug.Log(thiefScore[1]);
            if (thiefScore[0] > thiefScore[1])
            {
                Debug.Log(csTeamName + " win");
                winnerText.text = csTeamName;
            }
            else
            {
                Debug.Log(scsTeamName + " win");
                winnerText.text = scsTeamName;
            }
        }
        else
        {
            roundChangeIcon.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            roundChangeIcon.SetActive(false);
            RestartGame();
        }
    }

    public void RoundEnd()
    {
        StartCoroutine(RoundEndRoutine());
    }

    private void Start()
    {
        csTeamName = Resources.LoadAll("AI/CS")[0].name;
        scsTeamName = Resources.LoadAll("AI/SCS")[0].name;
        introCSTeamNameText.text = csTeamName;
        introSCSTeamNameText.text = scsTeamName;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStarted && !isGameEnded && isMoveEnded)
        {
            if (turnCount == 0)
            {
                RoundEnd();
            }
            else
            {
                StartCoroutine(MoveAgents());
            }
        }
    }

    void OnApplicationQuit()
    {
        if (policeThread != null && policeThread.IsAlive)
        {
            policeThread.Abort();
        }
        if (thiefThread != null && thiefThread.IsAlive)
        {
            thiefThread.Abort();
        }
    }
}
