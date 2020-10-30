using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamName
{
    public class ThiefAI : Thief
    {
        /// <summary>
        /// 도둑의 초기 위치를 설정하여 반환합니다.
        /// </summary>
        /// <param name="baseMap">처음 맵의 상태</param>
        /// <returns>처음 도둑의 위치</returns>
        public override ThiefInfo[] InitialThiefPos(TileType[,] baseMap)
        {
            ThiefInfo[] pos = new ThiefInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new ThiefInfo();
            }

            //-------------------------------
            //
            // 이 부분을 구현해 주시면 됩니다.
            //
            //-------------------------------

            return pos;
        }

        /// <summary>
        /// 도둑의 다음 턴 이동 방향을 설정하여 반환합니다.
        /// </summary>
        /// <param name="sight">현재 도둑 시야</param>
        /// <returns>다음 턴 도둑의 이동 방향</returns>
        public override MoveInfo[] NextThiefPos(SightInfo[,] sight)
        {
            MoveInfo[] pos = new MoveInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new MoveInfo();
            }

            //-------------------------------
            //
            // 이 부분을 구현해 주시면 됩니다.
            //
            //-------------------------------

            return pos;
        }
    }

    public class PoliceAI : Police
    {
        /// <summary>
        /// 경찰의 초기 위치를 설정하여 반환합니다.
        /// </summary>
        /// <param name="baseMap">처음 맵의 상태</param>
        /// <returns>처음 경찰의 위치</returns>
        public override PoliceInfo[] InitialPolicePos(TileType[,] baseMap)
        {
            PoliceInfo[] pos = new PoliceInfo[6];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new PoliceInfo();
            }

            //-------------------------------
            //
            // 이 부분을 구현해 주시면 됩니다.
            //
            //-------------------------------

            return pos;
        }

        /// <summary>
        /// 보물의 초기 위치를 설정하여 반환합니다.
        /// </summary>
        /// <param name="baseMap">처음 맵의 상태</param>
        /// <returns>처음 보물의 위치</returns>
        public override Vector2[] InitialTreasurePos(TileType[,] baseMap)
        {
            Vector2[] pos = new Vector2[7];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new Vector2();
            }

            //-------------------------------
            //
            // 이 부분을 구현해 주시면 됩니다.
            //
            //-------------------------------

            return pos;
        }

        /// <summary>
        /// 경찰의 다음 턴 이동 방향과 시야 방향을 설정하여 반환합니다.
        /// </summary>
        /// <param name="sight">현재 경찰 시야</param>
        /// <returns>다음 턴 경찰 이동 방향과 시야 방향</returns>
        public override MoveInfo[] NextPolicePos(SightInfo[,] sight)
        {
            MoveInfo[] pos = new MoveInfo[6];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new MoveInfo();
            }

            //-------------------------------
            //
            // 이 부분을 구현해 주시면 됩니다.
            //
            //-------------------------------

            return pos;
        }
    }
}
