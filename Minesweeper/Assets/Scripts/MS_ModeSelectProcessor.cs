using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_ModeSelectProcessor : MonoBehaviour {
        public MS_GameMode[] GameModes;
        public int SelectedMode => m_mode;
        public MS_ModeSelectUI SelectUI;


        public System.Action<MS_GameMode> OnSelected;

        public void Ative() {
            gameObject.SetActive(true);
            SelectUI.gameObject.SetActive(true);
        }
        public void Deative() {
            gameObject.SetActive(false);
            SelectUI.gameObject.SetActive(false);
        }

        public void SelectMode(int mode) => OnSelected?.Invoke(GameModes[mode]);
        public void SelectMode(in MS_GameMode gameMode) => OnSelected?.Invoke(gameMode);

        public void UpdateUI(int mode) {
            SelectUI.Name.text = GameModes[mode].name;
            m_mode = mode;
            MS_GridDrawer.Clear();
            MS_GridDrawer.DrawGrid(GameModes[mode].Size);
        }



        private int m_mode;


        private void Awake() {
            SelectUI.Select.onClick.AddListener(() => SelectMode(GameModes[m_mode]));
            SelectUI.Right.onClick.AddListener(() => UpdateUI((m_mode + 1 + GameModes.Length) % GameModes.Length));
            SelectUI.Left.onClick.AddListener(() => UpdateUI((m_mode - 1 + GameModes.Length) % GameModes.Length));

            UpdateUI(0);
        }
    }
}