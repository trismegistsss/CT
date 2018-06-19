using System.Collections.Generic;
using System.Diagnostics;
using CT.Enums;
using UnityEngine;

namespace CT.Config
{
    public class GameConfig
    {
        #region UI

        // HUD
        public const float SPEED_MOVE_PANELS = 1f;
        public const float Y_PANELS_OUT = 500f;

        // Gamestart panel 
        public const float DELAY_TIME_OPEN_LOGO = 1f;
        public const float SPEED_MOVE_LOGO = 2f;
        public const float Y_GAME_NAME_OUT = 500f;

        #endregion

        // Gameplay
        public const int START_LEVEL = 1;
        public const int SCORE_TO_NEXT_LEVEL = 10;

        public const int START_LIVE = 2;

        public const float MAX_ENEMY_IN_GAME = 10f;
        public static readonly Vector3 DEFAULT_GAMEOBJECTS_POSITION = new Vector3(0f,0f,-1f);


        public static int GetCountEnemyInGame(int level)
        {
            if (level<2)
                return 2;
            if (level < 4)
                return 3;
            if (level < 6)
                return 4;
            if (level < 7)
                return 5;
            return 6;
        }

        public static List<EnemyType> GetAvelablyEnemyInGame(int level)
        {
            var len = new List<EnemyType>();
            if (level > 0)
                len.Add(EnemyType.TankGreen);
            if (level > 1)
                len.Add(EnemyType.TankEllo);
            if (level > 2)
                len.Add(EnemyType.TankBlue);
            if (level > 3)
                len.Add(EnemyType.TankPurple);
            if (level > 4)
                len.Add(EnemyType.TankRed);

            return len;
        }
    }
}
