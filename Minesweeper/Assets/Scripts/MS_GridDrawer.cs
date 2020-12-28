using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_GridDrawer : MonoBehaviour {
        public static void DrawGrid(Vector2Int size) {
            s_instance.m_size = size;
            s_instance.m_tileHandle = new GameObject(TILE_HANDLE_NAME).transform;
            s_instance.m_tileHandle.transform.parent = s_instance.transform;
            s_instance.m_tileHandle.position = Vector2.zero;
            s_instance.m_tiles = new SpriteRenderer[size.x, size.y];

            s_instance.m_markHandle = new GameObject(MARK_HANDLE_NAME).transform;
            s_instance.m_markHandle.transform.parent = s_instance.transform;
            s_instance.m_markHandle.position = Vector2.zero;
            s_instance.m_marks = new SpriteRenderer[size.x, size.y];


            int objectID = 0;
            for (int y = 0, yLen = size.y; y != yLen; ++y) {
                for (int x = 0, xLen = size.x; x != xLen; ++x) {
                    Vector2 objectPos = new Vector2(x - (size.x - 1) * 0.5f, y - (size.y - 1) * 0.5f);

                    SpriteRenderer newTile = new GameObject($"Tile ({objectID})", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                    newTile.transform.position = objectPos;
                    newTile.transform.parent = s_instance.m_tileHandle;
                    newTile.sprite = MS_Resources.GetSprite($"Cover{(x + y) % 2 + 1}");
                    newTile.sortingOrder = 0;
                    s_instance.m_tiles[x, y] = newTile;

                    SpriteRenderer newMark = new GameObject($"Mark ({objectID})", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                    newMark.transform.position = objectPos;
                    newMark.transform.parent = s_instance.m_markHandle;
                    newTile.sortingOrder = -1;
                    s_instance.m_marks[x, y] = newMark;

                    ++objectID;
                }
            }
        }
        public static void DrawMark(Vector2Int offset, string name) {
            if(name != null) {
                s_instance.m_marks[offset.x, offset.y].sprite = MS_Resources.GetSprite(name);
            } else {
                s_instance.m_marks[offset.x, offset.y].sprite = null;
            }
            
        }
        public static void DrawMark(Vector2Int offset, int code) {
            if(code >= 0) {
                s_instance.m_marks[offset.x, offset.y].sprite = MS_Resources.GetSprite(code);
            } else {
                s_instance.m_marks[offset.x, offset.y].sprite = null;
            }
        }

        public static void SetCover(Vector2Int offset, bool open)
            => s_instance.m_tiles[offset.x, offset.y].sprite = MS_Resources.GetSprite((open ? "Ground" : "Cover") + ((offset.x + offset.y) % 2 + 1));


        public static void Clear() {
            if (s_instance.m_tileHandle != null) Destroy(s_instance.m_tileHandle.gameObject);
            s_instance.m_tiles = null;

            if (s_instance.m_markHandle != null) Destroy(s_instance.m_markHandle.gameObject);
            s_instance.m_marks = null;
        }
        public static void RedrawGrid() {
            for (int x = 0, xLen = s_instance.m_size.x; x != xLen; ++x) {
                for (int y = 0, yLen = s_instance.m_size.y; y != yLen; ++y) {
                    s_instance.m_tiles[x, y].sprite = null;
                    s_instance.m_marks[x, y].sprite = null;
                }
            }
        }

        private const string TILE_HANDLE_NAME = "Tile Handle";
        private const string MARK_HANDLE_NAME = "Mark Handle";

        private static MS_GridDrawer s_instance;

        private Vector2Int m_size;
        private Transform m_tileHandle;
        private SpriteRenderer[,] m_tiles;
        private Transform m_markHandle;
        private SpriteRenderer[,] m_marks;


        private void Awake() {
            s_instance = this;
        }
    }
}