using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    [RequireComponent(typeof(MS_GridDrawer))]
    public class MS_ModeSelectProcessor : MonoBehaviour {
        public MS_ModeSelectUI SelectUI;
        public MS_GameMode[] GameModes;
        public int SelectedMode => m_selectedMode;
        

        public System.Action<MS_GameMode> OnCommited;
        public System.Action<MS_GameMode> OnChanged;


        // Connection
        public void Ative() {
            gameObject.SetActive(true);
            SelectUI.gameObject.SetActive(true);

            ChangMode(0);
        }
        public void Deative() {
            gameObject.SetActive(false);
            SelectUI.gameObject.SetActive(false);
        }

        // Command
        public void CommitMode() {
            OnCommited?.Invoke(GameModes[m_selectedMode]);
        }
        public void ChangMode(int mode) {
            m_selectedMode = mode;
            UpdateScene();
            OnChanged?.Invoke(GameModes[mode]);
        }



        private MS_GridDrawer m_gridPreviewer;
        private int m_selectedMode;
        

        private void Awake() {
            m_gridPreviewer = GetComponent<MS_GridDrawer>();

            // Binding Events
            SelectUI.Select.onClick.AddListener(() => CommitMode());
            SelectUI.Right.onClick.AddListener(() => ChangMode((m_selectedMode + 1 + GameModes.Length) % GameModes.Length));
            SelectUI.Left.onClick.AddListener(() => ChangMode((m_selectedMode - 1 + GameModes.Length) % GameModes.Length));
        }

        private void UpdateScene() {
            SelectUI.Name.text = GameModes[m_selectedMode].name;
            m_gridPreviewer.Clear();
            m_gridPreviewer.DrawGrid(GameModes[m_selectedMode].Size);
        }
    }
}