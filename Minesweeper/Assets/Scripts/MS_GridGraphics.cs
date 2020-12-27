using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_GridGraphics : MonoBehaviour {
        public void Prepare(Vector2Int size) {
            m_tileHandle = new GameObject("Tile Handle").transform;
            m_tileHandle.transform.parent = transform;
            m_tileHandle.position = Vector2.zero;
            m_tiles = new SpriteRenderer[size.x, size.y];

            m_markHandle = new GameObject("Mark Handle").transform;
            m_markHandle.transform.parent = transform;
            m_markHandle.position = Vector2.zero;
            m_marks = new SpriteRenderer[size.x, size.y];


            for (int y = 0; y != size.y; ++y) {
                for (int x = 0; x != size.x; ++x) {
                    int objectID = x + size.x * y;
                    Vector2 objectPos = new Vector2(x - (size.x - 0.5f) * 0.5f, y - (size.y - 1) * 0.5f);

                    SpriteRenderer newTile = new GameObject($"Tile ({objectID})", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                    newTile.transform.position = objectPos;
                    newTile.transform.parent = m_tileHandle;
                    newTile.sprite = MS_GridGraphicResources.GetTile((x + y) % 2);
                    newTile.color = MS_GridGraphicResources.CoverColor;
                    newTile.sortingOrder = 0;


                    m_tiles[x, y] = newTile;

                    SpriteRenderer newMark = new GameObject($"Mark ({objectID})", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                    newMark.transform.position = objectPos;
                    newMark.transform.parent = m_markHandle;
                    newTile.sortingOrder = -1;
                    m_marks[x, y] = newMark;
                }
            }
        }
        public void Open(MS_Grid grid, List<Vector2Int> openOffsets) {
            for (int o = 0; o != openOffsets.Count; ++o) {
                Vector2Int openOffset = openOffsets[o];
                int id = grid.GetTileID(openOffset);
                SpriteRenderer tile = m_tiles[openOffset.x, openOffset.y];
                SpriteRenderer mark = m_marks[openOffset.x, openOffset.y];

                tile.color = MS_GridGraphicResources.GroundColor;

                if (1 <= id && id <= 8) {
                    mark.sprite = MS_GridGraphicResources.GetNumber(id - 1);
                } else if (id == MS_Grid.MINE_ID) {
                    mark.sprite = MS_GridGraphicResources.Mine;
                }
            }
        }

        private Transform m_tileHandle;
        private SpriteRenderer[,] m_tiles;
        private Transform m_markHandle;
        private SpriteRenderer[,] m_marks;
    }
}

