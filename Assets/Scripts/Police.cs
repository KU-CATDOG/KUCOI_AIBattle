using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : MonoBehaviour
{
    public Vector2[] InitialTreasurePos()
    {
        //보물의 초기 위치를 설정하여 반환
        //반환되는 배열의 크기는 7이어야 하며, 배열의 크기가 7보다 큰 경우 첫 7번째 값까지만 사용됨
        //0번, 1번, 2번은 2점짜리 보물의 위치
        //3번, 4번, 5번은 3점짜리 보물의 위치
        //6번은 5점짜리 보물의 위치
        return null;
    }

    public Vector2[] InitialPolicePos()
    {
        //경찰의 초기 위치를 설정하여 반환
        //반환되는 배열의 크기는 6이어야 하며, 배열의 크기가 6보다 큰 경우 첫 6번째 값까지만 사용됨
        return null;
    }
}
