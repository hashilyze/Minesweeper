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
        public int GetTileID(in Vector2Int offset) => m_tileIDs[offset.x, offset.y];
        /// <summary>Get offset of mines</summary>
        public IEnumerable<Vector2Int> GetMineOffsets => m_mineOffsets;
        /// <summary>Whether open tile of given offset</summary>
        public bool IsOpen(in Vector2Int offset) => m_isOpens[offset.x, offset.y];



        public void Prepare(in Vector2Int size) {
            m_size = size;
            m_tileIDs = new int[m_size.x, m_size.y];
            m_isOpens = new bool[m_size.x, m_size.y];
            m_mineOffsets = null;
            m_floodQueue.Clear();
        }
        public void Generate(int mineNum, Vector2Int start, int seed) {
            HashSet<Vector2Int> startBlocks = new HashSet<Vector2Int>();
            m_mineOffsets = new Vector2Int[mineNum];

            // Generate blocks
            for (int y = -1; y <= 1; ++y) {
                for (int x = -1; x <= 1; ++x) {
                    Vector2Int startBlock = start + new Vector2Int(x, y);
                    if (0 <= startBlock.x && 0 <= startBlock.y && startBlock.x < m_size.x && startBlock.y < m_size.y) {
                        startBlocks.Add(startBlock);
                    }
                }
            }
            // Generate mines
            using (var offsetGenerator = MakeOffsetGenerator(m_size, seed)) {
                int cursor = 0;
                while (mineNum > 0 && offsetGenerator.MoveNext()) {
                    Vector2Int mineOffset = offsetGenerator.Current;

                    // Check current offset is blocking
                    if (startBlocks.Contains(mineOffset)) continue;

                    // Set mine
                    m_tileIDs[mineOffset.x, mineOffset.y] = MINE_ID;
                    m_mineOffsets[cursor++] = mineOffset;
                    --mineNum;
                }
            }
            // Generate hints
            for (int m = 0, mLen = m_mineOffsets.Length; m != mLen; ++m) {
                for (int n = 0, nLen = NEIGHBOUR_OFFSETS.Length; n != nLen; ++n) {
                    Vector2Int neighbourOffset = m_mineOffsets[m] + NEIGHBOUR_OFFSETS[n];

                    if (0 <= neighbourOffset.x && 0 <= neighbourOffset.y && neighbourOffset.x < m_size.x && neighbourOffset.y < m_size.y) {
                        if (m_tileIDs[neighbourOffset.x, neighbourOffset.y] != MINE_ID) {
                            m_tileIDs[neighbourOffset.x, neighbourOffset.y] += 1;
                        }
                    }
                }
            }
        }
        public bool Open(Vector2Int offset, out List<Vector2Int> openOffsets) {
            openOffsets = new List<Vector2Int>();

            // Open the mine
            if (m_tileIDs[offset.x, offset.y] == MINE_ID) {
                m_isOpens[offset.x, offset.y] = true;
                openOffsets.Add(offset);
                return false;
            }

            m_floodQueue.Enqueue(offset);

            while (m_floodQueue.Count > 0) {
                Vector2Int openOffset = m_floodQueue.Dequeue();
                // Skip because already oepned
                if (m_isOpens[openOffset.x, openOffset.y]) continue;

                openOffsets.Add(openOffset);
                m_isOpens[openOffset.x, openOffset.y] = true;

                // Open neighbours, which are not mine
                if (m_tileIDs[openOffset.x, openOffset.y] == NULL_ID) {
                    for (int n = 0, nLen = NEIGHBOUR_OFFSETS.Length; n != nLen; ++n) {
                        Vector2Int neighbourOffset = openOffset + NEIGHBOUR_OFFSETS[n];

                        if (0 <= neighbourOffset.x && neighbourOffset.x < m_size.x && 0 <= neighbourOffset.y && neighbourOffset.y < m_size.y) {
                            if (m_isOpens[neighbourOffset.x, neighbourOffset.y] == false) {
                                m_floodQueue.Enqueue(neighbourOffset);
                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion
        #region Private
        private static readonly Vector2Int[] NEIGHBOUR_OFFSETS = GetNeighbours_Square(1);

        private readonly Queue<Vector2Int> m_floodQueue = new Queue<Vector2Int>();
        private Vector2Int m_size;
        private int[,] m_tileIDs;
        private bool[,] m_isOpens;
        private Vector2Int[] m_mineOffsets;


        /// <summary>Make offset-generator which yield randomly offsets in grid</summary>
        /// <param name="size">AABB size of grid</param>
        private static IEnumerator<Vector2Int> MakeOffsetGenerator(Vector2Int size, int seed) {
            int leftOffsetNum = size.x * size.y;
            Vector2Int[] offsets = new Vector2Int[leftOffsetNum];

            // Initialize sample
            int cursor = 0;
            for (int y = 0, yLen = size.y; y != yLen; ++y) {
                for (int x = 0, xLen = size.x; x != xLen; ++x) {
                    offsets[cursor++] = new Vector2Int(x, y);
                }
            }

            System.Random random = new System.Random(seed);
            while (leftOffsetNum > 0) {
                int target = random.Next(0, leftOffsetNum);
                yield return offsets[target];

                Vector2Int tmp = offsets[target];
                offsets[target] = offsets[--leftOffsetNum];
                offsets[leftOffsetNum] = tmp;
            }
        }

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
        #endregion
    }
}