using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TestA
{
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
            pos[0].mapPos = new Vector2(0, 1);
            pos[1].mapPos = new Vector2(0, 2);
            pos[2].mapPos = new Vector2(0, 3);
            pos[3].mapPos = new Vector2(1, 15);

            pos[0].value = 0;
            pos[1].value = 0;
            pos[2].value = 0;
            pos[3].value = 0;
            //For test

            return pos;
        }

        public override MoveInfo[] NextThiefPos()
        {
            MoveInfo[] pos = new MoveInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new MoveInfo();
            }


            //ForTest
            pos[0].moveDir = MoveDir.Right;
            pos[1].moveDir = MoveDir.Right;
            pos[2].moveDir = MoveDir.Right;
            pos[3].moveDir = MoveDir.Down;
            //ForTest



            return pos;
        }
    }



    public class PoliceAI : Police
    {
        public override PoliceInfo[] InitialPolicePos()
        {
            PoliceInfo[] pos = new PoliceInfo[6];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new PoliceInfo();
            }

            pos[0].mapPos = new Vector2(3, 1);
            pos[1].mapPos = new Vector2(3, 2);
            pos[2].mapPos = new Vector2(3, 3);
            pos[3].mapPos = new Vector2(3, 4);
            pos[4].mapPos = new Vector2(3, 5);
            pos[5].mapPos = new Vector2(3, 6);

            pos[0].angle = 0;
            pos[1].angle = 90;
            pos[2].angle = 180;
            pos[3].angle = 270;
            pos[4].angle = 0;
            pos[5].angle = 90;

            return pos;
        }

        public override Vector2[] InitialTreasurePos()
        {
            Vector2[] pos = new Vector2[7];

            pos[0] = new Vector2(1, 14);
            pos[1] = new Vector2(2, 14);
            pos[2] = new Vector2(3, 14);
            pos[3] = new Vector2(4, 14);
            pos[4] = new Vector2(5, 14);
            pos[5] = new Vector2(6, 14);
            pos[6] = new Vector2(7, 14);
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
}
