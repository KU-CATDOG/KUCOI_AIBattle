using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : SingletonBehaviour<Thief>
{
    public ThiefInfo[] InitialThiefPos()
    {
        //도둑의 초기 위치를 설정하여 반환
        //반환되는 배열의 크기는 4이어야 하며, 배열의 크기가 4보다 큰 경우 첫 4번째 값까지만 사용됨
        //도둑의 위치는 반드시 탈출 구역 안에 있어야 함
        ThiefInfo[] pos = new ThiefInfo[4];
        for (int i = 0; i < 4; i++)
        {
            pos[i] = new ThiefInfo();
        }

        pos[0].mapPos = new Vector2(0, 0);
        pos[1].mapPos = new Vector2(0, 1);
        pos[2].mapPos = new Vector2(0, 3);
        pos[3].mapPos = new Vector2(0, 4);

        pos[0].value = 0;
        pos[1].value = 0;
        pos[2].value = 0;
        pos[3].value = 0;

        return pos;
    }


    public MoveInfo[] NextThiefPos()
    {
        MoveInfo[] pos = new MoveInfo[4];
        for (int i = 0; i < 4; i++)
        {
            pos[i] = new MoveInfo();
        }


        //ForTest
        pos[0].moveDir = MoveDir.Up;
        pos[1].moveDir = MoveDir.Up;
        pos[2].moveDir = MoveDir.Neutral;
        pos[3].moveDir = MoveDir.Up;
        //ForTest



        return pos;
    }

}
