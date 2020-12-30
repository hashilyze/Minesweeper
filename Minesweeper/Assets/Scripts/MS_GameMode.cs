using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    [System.Serializable]
    public struct MS_GameMode {
        ///<summary>Identitiy of game mode</summary>
        public string name;
        /// <summary>AABB size of grid</summary>
        public Vector2Int Size;
        /// <summary>The number of mines</summary>
        public int MineNum;
    }
}


