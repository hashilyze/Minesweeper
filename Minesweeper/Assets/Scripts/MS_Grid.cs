using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_Grid {
        #region Public
        // ID of tiles
        public const int MINE_ID = -1;
        public const int NULL_ID = 0;
        public const int NUM_START_ID = 1;
        public const int NUM_END_ID = 8;

        /// <summary>Size of grid</summary>
        public Vector2Int Size => m_size;
        /// <summary>Get tile id of given offset</summary>
        public int GetTileID(Vector2Int offset) => m_tileIDs[offset.x, offset.y];
        /// <summary>Get offset of mines</summary>
        public IEnumerable<Vector2Int> GetMineOffsets => m_mineOffsets;
        /// <summary>Whether open tile of given offset</summary>
        public bool IsOpen(Vector2Int offset) => m_isOpens[offset.x, offset.y];



        /// <summary>
        /// Prepare to generate grid;
        /// Properties are undefined until generate
        /// </summary>
        public void Prepare(in Vector2Int size) {
            m_size = size;
            m_tileIDs = new int[m_size.x, m_size.y];
            m_isOpens = new bool[m_size.x, m_size.y];

            m_offsetGenerator = MakeOffsetGenerator(size);
        }
        /// <summary>Generate grid</summary>
        /// <param name="mineNum">The number of mines; its range is [0, num(tiles - blocks)]</param>
        /// <param name="blockOffsets">Offset of blocks that mines are not placed; all of offsets should be inside the grid</param>
        public void Generate(int mineNum, IEnumerable<Vector2Int> blockOffsets) {
            HashSet<Vector2Int> blockSet = new HashSet<Vector2Int>(blockOffsets);
            m_mineOffsets = new Vector2Int[mineNum];

            // Generate mines
            using (m_offsetGenerator) {
                int cursor = 0;
                while (mineNum > 0 && m_offsetGenerator.MoveNext()) {
                    Vector2Int mineOffset = m_offsetGenerator.Current;

                    // Check current offset is blocking
                    if (blockSet.Contains(mineOffset)) {
                        continue;
                    }
                    // Set mine
                    m_tileIDs[mineOffset.x, mineOffset.y] = MINE_ID;
                    m_mineOffsets[cursor++] = mineOffset;
                    --mineNum;
                }
            }
            m_offsetGenerator = null;

            // Generate hints
            for (int m = 0, mLen = m_mineOffsets.Length; m != mLen; ++m) {
                for (int n = 0, nLen = NEIGHBOUR_OFFSETS.Length; n != nLen; ++n) {
                    Vector2Int neighbourOffset = m_mineOffsets[m] + NEIGHBOUR_OFFSETS[n];

                    if (0 <= neighbourOffset.x && neighbourOffset.x < m_size.x
                        && 0 <= neighbourOffset.y && neighbourOffset.y < m_size.y) {
                        if (m_tileIDs[neighbourOffset.x, neighbourOffset.y] != MINE_ID) {
                            m_tileIDs[neighbourOffset.x, neighbourOffset.y] += 1;
                        }
                    }
                }
            }
        }
        /// <summary>Open tile from given offset</summary>
        /// <param name="offset">Offset to open; it should be inside the grid</param>
        /// <param name="openOffsets">Offsets of opened tiles</param>
        /// <returns>If open mine return false, otherwise true</returns>
        public bool Open(Vector2Int offset, out List<Vector2Int> openOffsets) {
            openOffsets = new List<Vector2Int>();

            // Open the mine
            if (m_tileIDs[offset.x, offset.y] == MINE_ID) {
                m_isOpens[offset.x, offset.y] = true;
                openOffsets.Add(offset);
                return false;
            }

            bool[,] isVisit = new bool[m_size.x, m_size.y];
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(offset);

            while (queue.Count > 0) {
                Vector2Int openOffset = queue.Dequeue();
                openOffsets.Add(openOffset);
                isVisit[openOffset.x, openOffset.y] = true;
                m_isOpens[openOffset.x, openOffset.y] = true;

                // Stop to flood if tile is not null
                if (m_tileIDs[openOffset.x, openOffset.y] != NULL_ID) {
                    continue;
                }
                // Open neighbours, which are not mine
                for (int n = 0, len = NEIGHBOUR_OFFSETS.Length; n != len; ++n) {
                    Vector2Int neighbourOffset = openOffset + NEIGHBOUR_OFFSETS[n];

                    if (0 <= neighbourOffset.x && neighbourOffset.x < m_size.x && 0 <= neighbourOffset.y && neighbourOffset.y < m_size.y) {
                        if (isVisit[neighbourOffset.x, neighbourOffset.y] == false) {
                            queue.Enqueue(neighbourOffset);
                        }
                    }
                }

            }
            return true;
        }
        #endregion
        #region Private
        public static readonly Vector2Int[] NEIGHBOUR_OFFSETS = MS_Utility.GetNeighbours_Square(1);

        private Vector2Int m_size;
        private int[,] m_tileIDs;
        private bool[,] m_isOpens;
        private Vector2Int[] m_mineOffsets;
        private IEnumerator<Vector2Int> m_offsetGenerator;



        /// <summary>Make generator which yield offset in grid randomly</summary>
        private static IEnumerator<Vector2Int> MakeOffsetGenerator(Vector2Int size) {
            int tileNum = size.x * size.y;
            Vector2Int[] offsets = new Vector2Int[tileNum];
            // Initialize sample
            int cursor = 0;
            for (int y = 0, yLen = size.y; y != yLen; ++y) {
                for (int x = 0, xLen = size.x; x != xLen; ++x) {
                    offsets[cursor++] = new Vector2Int(x, y);
                }
            }
            
            while (tileNum > 0) {
                int target = Random.Range(0, tileNum);
                yield return offsets[target];

                Vector2Int tmp = offsets[target];
                offsets[target] = offsets[--tileNum];
                offsets[tileNum] = tmp;
            }
        }
        #endregion
    }
}