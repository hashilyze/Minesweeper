using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    [RequireComponent(typeof(GridDrawer))]
    public class MS_GridDrawer : MonoBehaviour {
        public GridDrawer RawDrawer => m_gridDrawer;

        public void DrawGrid(in Vector2Int size) {
            m_gridDrawer.DrawGrid(in size, (offset) => MS_Resources.GetSprite($"Cover{(offset.x + offset.y) % 2 + 1}"));
        }

        public void DrawPiece(in Vector2Int offset, string name) {
            if(name != null) {
                m_gridDrawer.GetPiece(in offset).sprite = MS_Resources.GetSprite(name);
            } else {
                m_gridDrawer.GetPiece(in offset).sprite = null;
            }
        }
        public void DrawPiece(in Vector2Int offset, int code) {
            if(code >= 0) {
                m_gridDrawer.GetPiece(in offset).sprite = MS_Resources.GetSprite(code);
            } else {
                m_gridDrawer.GetPiece(in offset).sprite = null;
            }
        }

        public void SetCover(in Vector2Int offset, bool open) {
            m_gridDrawer.GetTile(offset).sprite = MS_Resources.GetSprite((open ? "Ground" : "Cover") + ((offset.x + offset.y) % 2 + 1));
        }

        public void Clear() {
            m_gridDrawer.Clear();
        }

        public void RedrawGrid() {
            m_gridDrawer.RedrawGrid((offset) => MS_Resources.GetSprite($"Cover{(offset.x + offset.y) % 2 + 1}"));
        }



        private GridDrawer m_gridDrawer;


        private void Awake() {
            m_gridDrawer = GetComponent<GridDrawer>();
        }
    }
}