using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_Utility {
        public static Vector2Int[] GetNeighbours_Square(int range) {
            Vector2Int[] result = new Vector2Int[range * (range + 1) * 4];

            int cursor = 0;
            for (int y = -range; y <= range; ++y) {
                for (int x = -range; x <= range; ++x) {
                    if (x != 0 || y != 0) {
                        result[cursor++] = new Vector2Int(x, y);
                    }
                }
            }

            return result;
        }
        public static Vector2Int[] GetNeighbours_Cross(int range) {
            Vector2Int[] result = new Vector2Int[range * 4];

            int cursor = 0;
            for(int r = 1; r <= range; ++r) {
                result[cursor++] = new Vector2Int(r, 0);    // Right
                result[cursor++] = new Vector2Int(0, r);    // Up
                result[cursor++] = new Vector2Int(-r, 0);   // Left
                result[cursor++] = new Vector2Int(0, -r);   // Down
            }
            return result;
        }
        public static Vector2Int[] GetNeighbours_X(int range) {
            Vector2Int[] result = new Vector2Int[range * 4];

            int cursor = 0;
            for (int r = 1; r <= range; ++r) {
                result[cursor++] = new Vector2Int(r, r);    // Up-Right
                result[cursor++] = new Vector2Int(-r, r);   // Up-Left
                result[cursor++] = new Vector2Int(-r, -r);  // Down-Left
                result[cursor++] = new Vector2Int(r, -r);   // Down-Right
            }
            return result;
        }
    }
}

