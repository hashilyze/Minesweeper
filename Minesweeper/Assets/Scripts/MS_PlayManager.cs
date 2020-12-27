using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_PlayManager : MonoBehaviour {
        #region Public
        public MS_Grid MyGrid;
        public MS_GridGraphics MyGridGraphics;

        public Vector2Int Size;
        public int MineNum;

        public void Execute() {
            isExecute = true;
            IsFirst = true;

            MyGrid = new MS_Grid();
            MyGrid.Prepare(in Size);

            MS_GridGraphicResources.Load();
            MyGridGraphics.Prepare(Size);
        }
        public void Exit() {
            isExecute = false;
        }
        #endregion
        #region Private
        private bool isExecute;
        private bool IsFirst = true;

        private void Awake() {
            Execute();
        }
        private void Update() {
            if (isExecute == false) {
                return;
            }

            if (Input.GetMouseButtonUp(0)) { // Open tile
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (-Size.x * 0.5f < worldPos.x && worldPos.x < Size.x * 0.5f
                    && -Size.y * 0.5f < worldPos.y && worldPos.y < Size.y * 0.5f) {
                    Vector2Int offset = new Vector2Int(Mathf.RoundToInt(worldPos.x + Size.x * 0.5f - 0.5f), Mathf.RoundToInt(worldPos.y + Size.y * 0.5f - 0.5f));

                    if (IsFirst) {
                        IsFirst = false;
                        Vector2Int[] blocks = new Vector2Int[9];

                        int count = 0;
                        for (int x = -1; x <= 1; ++x) {
                            for (int y = -1; y <= 1; ++y) {
                                blocks[count++] = offset + new Vector2Int(x, y);
                            }
                        }
                        MyGrid.Generate(MineNum, blocks);
                    }

                    if (MyGrid.Open(offset, out List<Vector2Int> openOffsets)) {
                        MyGridGraphics.Open(MyGrid, openOffsets);
                    } else {
                        Debug.Log("Game Over!");
                    }
                }
            } else if (Input.GetMouseButtonUp(1)) { // Flag tile
                
            }
        }
        #endregion
    }
}