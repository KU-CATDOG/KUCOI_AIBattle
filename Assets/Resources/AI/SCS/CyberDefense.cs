using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberDefense
{
    public class PoliceAI : Police
    {
        public override PoliceInfo[] InitialPolicePos()
        {
            PoliceInfo[] pos = new PoliceInfo[6];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new PoliceInfo();
            }

            //For test
            pos[0].mapPos = new Vector2(14, 1);
            pos[1].mapPos = new Vector2(14, 2);
            pos[2].mapPos = new Vector2(14, 3);
            pos[3].mapPos = new Vector2(14, 4);
            pos[4].mapPos = new Vector2(14, 5);
            pos[5].mapPos = new Vector2(14, 6);

            pos[0].angle = 0;
            pos[1].angle = 90;
            pos[2].angle = 180;
            pos[3].angle = 270;
            pos[4].angle = 0;
            pos[5].angle = 90;
            //For test

            return pos;
        }

        public override Vector2[] InitialTreasurePos()
        {
            Vector2[] pos = new Vector2[7];

            //For test
            pos[0] = new Vector2(14, 14);
            pos[1] = new Vector2(13, 14);
            pos[2] = new Vector2(12, 14);
            pos[3] = new Vector2(11, 14);
            pos[4] = new Vector2(10, 14);
            pos[5] = new Vector2(9, 14);
            pos[6] = new Vector2(8, 14);
            //For test
            return pos;
        }

        public override MoveInfo[] NextPolicePos()
        {
            MoveInfo[] pos = new MoveInfo[6];
            for (int i = 0; i < 6; i++)
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
    public class ThiefAI : Thief
    {
        public override ThiefInfo[] InitialThiefPos()
        {
            //도둑의 초기 위치를 설정하여 반환
            //반환되는 배열의 크기는 4이어야 하며, 배열의 크기가 4보다 큰 경우 첫 4번째 값까지만 사용됨
            //도둑의 위치는 반드시 탈출 구역 안에 있어야 함
            ThiefInfo[] pos = new ThiefInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new ThiefInfo();
            }

            //For test
            pos[0].mapPos = new Vector2(1, 15);
            pos[1].mapPos = new Vector2(2, 15);
            pos[2].mapPos = new Vector2(3, 15);
            pos[3].mapPos = new Vector2(4, 15);

            //For test

            return pos;
        }
        int i = 0;
        public override MoveInfo[] NextThiefPos()
        {
            MoveInfo[] pos = new MoveInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new MoveInfo();
            }


            //ForTest
            if(i == 0)
            {
                pos[0].moveDir = MoveDir.Down;
                pos[1].moveDir = MoveDir.Down;
                pos[2].moveDir = MoveDir.Down;
                pos[3].moveDir = MoveDir.Down;
            }
            else if (i == 1)
            {
                pos[0].moveDir = MoveDir.Up;
                pos[1].moveDir = MoveDir.Up;
                pos[2].moveDir = MoveDir.Up;
                pos[3].moveDir = MoveDir.Up;
            }
            else
            {
                pos[0].moveDir = MoveDir.Down;
                pos[1].moveDir = MoveDir.Down;
                pos[2].moveDir = MoveDir.Down;
                pos[3].moveDir = MoveDir.Down;
            }
            //ForTest
            i++;


            return pos;
        }
    }


}
