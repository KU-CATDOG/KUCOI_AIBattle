using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private MapManager mapManager;

    // Start is called before the first frame update
    void Start()
    {
        mapManager.InitiatePolice();
        mapManager.InitiateThief();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
