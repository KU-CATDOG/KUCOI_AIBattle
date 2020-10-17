using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : SingletonBehaviour<Police>
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

    public PoliceInfo[] InitialPolicePos()
    {
        //경찰의 초기 위치를 설정하여 반환
        //반환되는 배열의 크기는 6이어야 하며, 배열의 크기가 6보다 큰 경우 첫 6번째 값까지만 사용됨
        PoliceInfo[] pos = new PoliceInfo[6];
        for (int i = 0; i < 6; i++)
        {
            pos[i] = new PoliceInfo();
        }

        pos[0].mapPos = new Vector2(1, 1);
        pos[1].mapPos = new Vector2(2, 2);
        pos[2].mapPos = new Vector2(3, 3);
        pos[3].mapPos = new Vector2(4, 4);
        pos[4].mapPos = new Vector2(5, 5);
        pos[5].mapPos = new Vector2(6, 6);

        pos[0].angle = 0;
        pos[1].angle = 90;
        pos[2].angle = 180;
        pos[3].angle = 270;
        pos[4].angle = 0;
        pos[5].angle = 90;

        return pos;
    }

    public MoveInfo[] NextPolicePos()
    {
        MoveInfo[] pos = new MoveInfo[6];
        for(int i = 0; i < 6; i++)
        {
            pos[i] = new MoveInfo();
        }


        //ForTest
        pos[0].moveDir = MoveDir.Up;
        pos[1].moveDir = MoveDir.Up;
        pos[2].moveDir = MoveDir.Up;
        pos[3].moveDir = MoveDir.Up;
        pos[4].moveDir = MoveDir.Up;
        pos[5].moveDir = MoveDir.Up;

        pos[0].moveAngle = 270;
        pos[1].moveAngle = 180;
        pos[2].moveAngle = 90;
        pos[3].moveAngle = 0;
        pos[4].moveAngle = 270;
        pos[5].moveAngle = 180;
        //ForTest



        return pos;
    }
}
