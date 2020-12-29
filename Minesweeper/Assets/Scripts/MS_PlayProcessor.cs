using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_PlayProcessor : MonoBehaviour {
        public MS_PlayUI PlayUI;
        public MS_ResultUI ResultUI;


        public System.Action<MS_GameState> OnGameComplete;
        public System.Action<MS_GameState> OnGameOver;
        public System.Action OnRestart;

        // Connection
        public void Ative(in MS_GameMode gameMode) {
            gameObject.SetActive(true);
            PlayUI.gameObject.SetActive(true);
            ResultUI.gameObject.SetActive(false);

            m_gameMode = gameMode;
            m_gameState = new MS_GameState(in gameMode.Size);
            m_tileNum = gameMode.Size.x * gameMode.Size.y;
            isPlay = true;

            m_grid.Prepare(in gameMode.Size);
            m_gridDrawer.Clear();
            m_gridDrawer.DrawGrid(in gameMode.Size);

            UpdateScene();
        }
        public void Deative() {
            gameObject.SetActive(false);
            PlayUI.gameObject.SetActive(false);
            ResultUI.gameObject.SetActive(false);
        }

        // Commnad
        [ContextMenu("Show Mines")]
        public void ShowMines() {
            if (m_gameState.IsFirst) return;

            foreach (Vector2Int mineOffset in m_grid.GetMineOffsets) {
                m_gridDrawer.DrawPiece(mineOffset, 0);
            }
        }
        public void Open(Vector2Int offset) {
            if (m_grid.IsOpen(offset)) return;

            if (m_gameState.IsFirst) {
                m_gameState.IsFirst = false;
                m_gameState.StartTime = Time.time;

                Vector2Int[] blocks = new Vector2Int[9];

                int count = 0;
                for (int x = -1; x <= 1; ++x) {
                    for (int y = -1; y <= 1; ++y) {
                        blocks[count++] = offset + new Vector2Int(x, y);
                    }
                }
                m_grid.Generate(m_gameMode.MineNum, blocks);
            }

            if (m_grid.Open(offset, out List<Vector2Int> openOffsets)) {
                for (int o = 0, oLen = openOffsets.Count; o != oLen; ++o) {
                    m_gridDrawer.SetCover(openOffsets[o], true);
                    if (0 < m_grid.GetTileID(openOffsets[o]) && m_grid.GetTileID(openOffsets[o]) <= 8) {
                        m_gridDrawer.DrawPiece(openOffsets[o], m_grid.GetTileID(openOffsets[o]));
                    }
                }
                m_gameState.OpenTileNum += openOffsets.Count;
                if (m_gameState.OpenTileNum == m_tileNum - m_gameMode.MineNum) {
                    EndGame("Congraulation!");
                    OnGameComplete?.Invoke(m_gameState);
                }
            } else {
                ShowMines();

                EndGame("Game Over!");
                OnGameOver?.Invoke(m_gameState);
            }
        }
        public void SetFlag(Vector2Int offset, bool active) {
            if (m_grid.IsOpen(offset)) return;

            if (active) {
                m_gridDrawer.DrawPiece(offset, "Flag");
                if (!m_gameState.IsFlags[offset.x, offset.y]) {
                    m_gameState.UsedFlagNum += 1;
                }
            } else {
                m_gridDrawer.DrawPiece(offset, null);
                if (m_gameState.IsFlags[offset.x, offset.y]) {
                    m_gameState.UsedFlagNum -= 1;
                }
            }
            m_gameState.IsFlags[offset.x, offset.y] = active;
        }



        private MS_Grid m_grid;
        private MS_GridDrawer m_gridDrawer;

        private MS_GameMode m_gameMode;
        private MS_GameState m_gameState;
        private bool isPlay;
        private int m_tileNum;



        private void Awake() {
            m_grid = new MS_Grid();
            m_gridDrawer = GetComponent<MS_GridDrawer>();

            ResultUI.Off.onClick.AddListener(() => ResultUI.gameObject.SetActive(false));
            PlayUI.Restart.onClick.AddListener(() => OnRestart?.Invoke());
        }
        private void Update() {
            if (isPlay == false) return;

            if (Input.GetMouseButtonUp(0)) { // Open tile
                if (TryMousePosToOffset(out Vector2Int offset)) {
                    Open(offset);
                }
            } else if (Input.GetMouseButtonUp(1)) { // Flag tile
                if(TryMousePosToOffset(out Vector2Int offset)) {
                    SetFlag(offset, !m_gameState.IsFlags[offset.x, offset.y]);
                }
            }
            
            if (m_gameState.IsFirst) return;
            UpdateScene();
        }
        
        private void UpdateScene() {
            PlayUI.TimeBoard.text = ((int)(Time.time - m_gameState.StartTime)).ToString();
            PlayUI.FlagBoard.text = (m_gameMode.MineNum - m_gameState.UsedFlagNum).ToString();
        }

        /// <summary>End the game</summary>
        private void EndGame(string message) {
            isPlay = false;

            ResultUI.Header.text = message;
            ResultUI.TimeBoard.text = ((int)(Time.time - m_gameState.StartTime)).ToString();
            ResultUI.gameObject.SetActive(true);
        }


        /// <summary>Convert mouse pos to grid offset</summary>
        /// <returns>If offset is valid return true, otherwise false</returns>
        private bool TryMousePosToOffset(out Vector2Int offset) {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (-m_gameMode.Size.x * 0.5f < worldPos.x && worldPos.x < m_gameMode.Size.x * 0.5f
                && -m_gameMode.Size.y * 0.5f < worldPos.y && worldPos.y < m_gameMode.Size.y * 0.5f) {

                offset = new Vector2Int(
                    Mathf.RoundToInt(worldPos.x + (m_gameMode.Size.x - 1) * 0.5f),
                    Mathf.RoundToInt(worldPos.y + (m_gameMode.Size.y - 1) * 0.5f)
                );
                return true;
            }
            offset = Vector2Int.zero;
            return false;
        }
    }
}
