using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTeamCS
{
    public class ThiefAI : Thief
    {
        public override ThiefInfo[] InitialThiefPos(TileType[,] baseMap)
        {
            ThiefInfo[] pos = new ThiefInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new ThiefInfo();
            }

            pos[0].mapPos = new Vector2(0, 1);
            pos[1].mapPos = new Vector2(0, 2);
            pos[2].mapPos = new Vector2(0, 3);
            pos[3].mapPos = new Vector2(1, 15);

            return pos;
        }

        public override MoveInfo[] NextThiefPos(SightInfo[,] sight, ThiefInfo[] thieves)
        {
            MoveInfo[] pos = new MoveInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new MoveInfo();
            }

            pos[0].moveDir = MoveDir.Right;
            pos[1].moveDir = MoveDir.Right;
            pos[2].moveDir = MoveDir.Right;
            pos[3].moveDir = MoveDir.Down;

            return pos;
        }
    }


    public class PoliceAI : Police
    {
        public override PoliceInfo[] InitialPolicePos(TileType[,] baseMap)
        {
            PoliceInfo[] pos = new PoliceInfo[6];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new PoliceInfo();
            }

            pos[0].mapPos = new Vector2(1, 3);
            pos[1].mapPos = new Vector2(1, 3);
            pos[2].mapPos = new Vector2(1, 3);
            pos[3].mapPos = new Vector2(1, 3);
            pos[4].mapPos = new Vector2(1, 3);
            pos[5].mapPos = new Vector2(1, 3);

            pos[0].angle = MoveAngle.Up;
            pos[1].angle = MoveAngle.Up;
            pos[2].angle = MoveAngle.Up;
            pos[3].angle = MoveAngle.Up;
            pos[4].angle = MoveAngle.Up;
            pos[5].angle = MoveAngle.Up;

            return pos;
        }

        public override Vector2[] InitialTreasurePos(TileType[,] baseMap)
        {
            Vector2[] pos = new Vector2[7];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new Vector2();
            }

            pos[0] = new Vector2(1, 1);
            pos[1] = new Vector2(4, 5);
            pos[2] = new Vector2(1, 9);
            pos[3] = new Vector2(4, 14);
            pos[4] = new Vector2(8, 1);
            pos[5] = new Vector2(8, 9);
            pos[6] = new Vector2(11, 5);

            return pos;
        }

        public override MoveInfo[] NextPolicePos(SightInfo[,] sight, PoliceInfo[] polices)
        {
            MoveInfo[] pos = new MoveInfo[6];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new MoveInfo();
            }

            pos[0].moveDir = MoveDir.Up;
            pos[1].moveDir = MoveDir.Up;
            pos[2].moveDir = MoveDir.Up;
            pos[3].moveDir = MoveDir.Up;
            pos[4].moveDir = MoveDir.Up;
            pos[5].moveDir = MoveDir.Up;

            pos[0].moveAngle = MoveAngle.Up;
            pos[1].moveAngle = MoveAngle.Up;
            pos[2].moveAngle = MoveAngle.Up;
            pos[3].moveAngle = MoveAngle.Up;
            pos[4].moveAngle = MoveAngle.Up;
            pos[5].moveAngle = MoveAngle.Up;

            return pos;
        }
    }
}
