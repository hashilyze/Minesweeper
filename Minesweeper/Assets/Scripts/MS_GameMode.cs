using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    [System.Serializable]
    public struct MS_GameMode{
        public string name;
        public Vector2Int Size;
        public int MineNum;
    }
}


