/// <summary>
/// 맵 한 칸의 종류
/// </summary>
public enum TileType { 
    /// <summary>
    /// 빈 칸
    /// </summary>
    Empty,
    /// <summary>
    /// 벽
    /// </summary>
    Wall,
    /// <summary>
    /// 탈출구
    /// </summary>
    Exit,
    /// <summary>
    /// 보물
    /// </summary>
    Treasure,
    /// <summary>
    /// 시야가 보이는 타일(Empty 혹은 Treasure)
    /// </summary>
    Sight }

/// <summary>
/// 시야의 종류
/// </summary>
public enum SightType { 
    /// <summary>
    /// 시야에 아무것도 없음
    /// </summary>
    Empty, 
    /// <summary>
    /// 볼 수 없음
    /// </summary>
    Invisible, 
    /// <summary>
    /// 시야에 보물이 존재
    /// </summary>
    Treasure, 
    /// <summary>
    /// 시야에 도둑이 존재
    /// </summary>
    Thief }

/// <summary>
/// 이동 방향
/// </summary>
public enum MoveDir { 
    /// <summary>
    /// 위 (0, 1)
    /// </summary>
    Up,
    /// <summary>
    /// 아래 (0, -1)
    /// </summary>
    Down,
    /// <summary>
    /// 왼쪽 (-1, 0)
    /// </summary>
    Left,
    /// <summary>
    /// 오른쪽 (1, 0)
    /// </summary>
    Right,
    /// <summary>
    /// 중립 (0, 0)
    /// </summary>
    Neutral }

/// <summary>
/// 보는 방향
/// </summary>
public enum MoveAngle { 
    /// <summary>
    /// 위 (90도)
    /// </summary>
    Up = 90,
    /// <summary>
    /// 아래 (270도)
    /// </summary>
    Down = 270, 
    /// <summary>
    /// 왼쪽 (180도)
    /// </summary>
    Left = 180,
    /// <summary>
    /// 오른쪽 (0도)
    /// </summary>
    Right = 0, 
    /// <summary>
    /// 각도 유지, 사용 금지
    /// </summary>
    Null = -1 }
