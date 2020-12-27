using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    /// <summary>
    /// Minesweeper Package Game Manager;
    /// GameManager alive until exit the game
    /// </summary>
    public class MS_GameManager : MonoBehaviour {
        private static MS_GameManager s_instance;

        private void Awake() {
            if (s_instance is null) {
                Initialize();
            }
        }
        private void Initialize() {
            s_instance = FindObjectOfType<MS_GameManager>();
            if(s_instance is null) {
                throw new System.NullReferenceException($"{nameof(MS_GameManager)} is null");
            }
            DontDestroyOnLoad(s_instance.gameObject);
        }
    }
}

