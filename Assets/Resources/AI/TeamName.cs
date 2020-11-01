using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ** namespace 이름은 반드시 파일명과 동일해야합니다.
namespace TeamName
{
    /// <summary>
    /// 도둑 AI
    /// </summary>
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
            // (Vector2) ThiefInfo.mapPos : 맵 상의 좌표
            //
            //-------------------------------

            return pos;
        }

        /// <summary>
        /// 도둑의 다음 턴 이동 방향을 설정하여 반환합니다.
        /// </summary>
        /// <param name="sight">현재 도둑 시야</param>
        /// <param name="thieves">현재 도둑 정보</param>
        /// <returns>다음 턴 도둑의 이동 방향</returns>
        public override MoveInfo[] NextThiefPos(SightInfo[,] sight, ThiefInfo[] thieves)
        {
            MoveInfo[] pos = new MoveInfo[4];
            for (int i = 0; i < 4; i++)
            {
                pos[i] = new MoveInfo();
            }

            //-------------------------------
            //
            // 이 부분을 구현해 주시면 됩니다.
            // (MoveDir) MoveInfo.moveDir : 이동 방향
            // ** 만약 도둑이 잡혔을 경우, thives 배열에서 그 도둑의 원소는 null입니다.
            // ** 도둑이 null인 경우에 대한 예외 처리를 반드시 할 것을 권장드립니다.
            // ex) 3번 도둑이 잡혔을 경우 thieves[3] == null
            //
            //-------------------------------

            return pos;
        }
    }

    /// <summary>
    /// 경찰 AI
    /// </summary>
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
            // (MoveAngle) PoliceInfo.angle : 바라보는 방향
            // (Vector2) PoliceInfo.mapPos : 맵 상 좌표
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
            // 0 ~ 2번은 2점, 3 ~ 5번은 3점, 6번은 5점 짜리 보물입니다.
            //
            //-------------------------------

            return pos;
        }

        /// <summary>
        /// 경찰의 다음 턴 이동 방향과 시야 방향을 설정하여 반환합니다.
        /// </summary>
        /// <param name="sight">현재 경찰 시야</param>
        /// <param name="polices">현재 경찰 정보</param>
        /// <returns>다음 턴 경찰 이동 방향과 시야 방향</returns>
        public override MoveInfo[] NextPolicePos(SightInfo[,] sight, PoliceInfo[] polices)
        {
            MoveInfo[] pos = new MoveInfo[6];
            for (int i = 0; i < 6; i++)
            {
                pos[i] = new MoveInfo();
            }

            //-------------------------------
            //
            // 이 부분을 구현해 주시면 됩니다.
            // (MoveDir) MoveInfo.moveDir : 이동 방향
            // (MoveAngle) MoveInfo.moveAngle : 바라보는 방향
            //
            //-------------------------------

            return pos;
        }
    }
}
