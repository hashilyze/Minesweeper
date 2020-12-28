using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_PlayProcessor : MonoBehaviour {
        public MS_Grid Grid;
        public MS_ResultUI ResultUI;
        public MS_PlayUI PlayUI;

        public void Ative(MS_GameMode gameMode) {
            gameObject.SetActive(true);
            startTime = Time.time;

            m_size = gameMode.Size;
            m_tileNum = m_size.x * m_size.y;
            m_mineNum = gameMode.MineNum;

            Grid = new MS_Grid();
            Grid.Prepare(in m_size);

            isFirst = true;
            openNum = 0;
            isFlags = new bool[m_size.x, m_size.y];
        }
        public void Deative() {
            gameObject.SetActive(false);
            ResultUI.gameObject.SetActive(false);
        }



        [Header("Game Mode")]
        private Vector2Int m_size;
        private int m_mineNum;
        private int m_tileNum;
        [Header("Game State")]
        private bool isFirst = true;
        private int openNum;
        private float startTime;
        private bool[,] isFlags;


        private void Awake() {
            ResultUI.Off.onClick.AddListener(() => ResultUI.gameObject.SetActive(false));
        }
        private void Update() {
            if (Input.GetMouseButtonUp(0)) { // Open tile
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (-m_size.x * 0.5f < worldPos.x && worldPos.x < m_size.x * 0.5f && -m_size.y * 0.5f < worldPos.y && worldPos.y < m_size.y * 0.5f) {
                    Vector2Int offset = new Vector2Int(Mathf.RoundToInt(worldPos.x + (m_size.x - 1) * 0.5f), Mathf.RoundToInt(worldPos.y + (m_size.y - 1) * 0.5f));
                    if (Grid.IsOpen(offset)) return;

                    if (isFirst) {
                        isFirst = false;
                        Vector2Int[] blocks = new Vector2Int[9];

                        int count = 0;
                        for (int x = -1; x <= 1; ++x) {
                            for (int y = -1; y <= 1; ++y) {
                                blocks[count++] = offset + new Vector2Int(x, y);
                            }
                        }
                        Grid.Generate(m_mineNum, blocks);
                    }

                    if (Grid.Open(offset, out List<Vector2Int> openOffsets)) {
                        for(int o = 0, oLen = openOffsets.Count; o != oLen; ++o) {
                            MS_GridDrawer.SetCover(openOffsets[o], true);
                            if (0 < Grid.GetTileID(openOffsets[o]) && Grid.GetTileID(openOffsets[o]) <= 8) {
                                MS_GridDrawer.DrawMark(openOffsets[o], Grid.GetTileID(openOffsets[o]));
                            }
                        }
                        openNum += openOffsets.Count;
                        if(openNum == m_tileNum - m_mineNum) {
                            ShowResult("Congraulation!");
                        }
                    } else {
                        foreach(Vector2Int mineOffset in Grid.GetMineOffsets) {
                            MS_GridDrawer.DrawMark(mineOffset, 0);
                        }
                        Deative();
                        ShowResult("Game Over!");
                    }
                }
            } else if (Input.GetMouseButtonUp(1)) { // Flag tile
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (-m_size.x * 0.5f < worldPos.x && worldPos.x < m_size.x * 0.5f && -m_size.y * 0.5f < worldPos.y && worldPos.y < m_size.y * 0.5f) {
                    Vector2Int offset = new Vector2Int(Mathf.RoundToInt(worldPos.x + (m_size.x - 1) * 0.5f), Mathf.RoundToInt(worldPos.y + (m_size.y - 1) * 0.5f));
                    if(Grid.IsOpen(offset) == false) {
                        if(isFlags[offset.x, offset.y]) {
                            MS_GridDrawer.DrawMark(offset, -1);
                        } else {
                            MS_GridDrawer.DrawMark(offset, "Flag");
                        }
                        isFlags[offset.x, offset.y] = !isFlags[offset.x, offset.y];
                    }
                    
                }
            }
        }

        private void ShowResult(string message) {
            ResultUI.Header.text = message;
            ResultUI.TimeBoard.text = ((int)(Time.time - startTime)).ToString();
            ResultUI.gameObject.SetActive(true);
        }
    }
}
