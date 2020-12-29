using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    [SerializeField]
    public struct MS_GameState {
        public MS_GameState(in Vector2Int size) {
            IsFirst = true;
            IsFlags = new bool[size.x, size.y];
            StartTime = Time.time;
            UsedFlagNum = 0;
            OpenTileNum = 0;
        }

        public bool IsFirst;
        public bool[,] IsFlags;
        public float StartTime;

        public int UsedFlagNum;
        public int OpenTileNum;
    }
}