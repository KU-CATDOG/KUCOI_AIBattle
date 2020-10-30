using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamExample
{
    public class ThiefAI : Thief
    {
        public override ThiefInfo[] InitialThiefPos(TileType[,] baseMap)
        {
            //도둑의 초기 위치를 설정하여 반환
            //반환되는 배열의 크기는 4이어야 하며, 배열의 크기가 4보다 큰 경우 첫 4번째 값까지만 사용됨
            //도둑의 위치는 반드시 탈출 구역 안에 있어야 함
            ThiefInfo[] pos = new ThiefInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new ThiefInfo();
            }


            
            return pos;
        }

        public override MoveInfo[] NextThiefPos(SightInfo[,] sight)
        {
            MoveInfo[] pos = new MoveInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new MoveInfo();
            }


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


            return pos;
        }

        public override Vector2[] InitialTreasurePos(TileType[,] baseMap)
        {
            Vector2[] pos = new Vector2[7];


            return pos;
        }

        public override MoveInfo[] NextPolicePos(SightInfo[,] sight)
        {
            MoveInfo[] pos = new MoveInfo[6];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new MoveInfo();
            }


            return pos;
        }
    }
}
