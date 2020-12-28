using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_GameManager : GameManager {
        public MS_ModeSelectProcessor ModeSelectProcessor;
        public MS_PlayProcessor PlayProcessor;

        private void Awake() {
            // Load Resources
            MS_Resources.Load();
            
            // Load Processor
            ModeSelectProcessor = FindObjectOfType<MS_ModeSelectProcessor>();
            PlayProcessor = FindObjectOfType<MS_PlayProcessor>();

            ModeSelectProcessor.gameObject.SetActive(false);
            PlayProcessor.gameObject.SetActive(false);
        }
        private void Start() {
            ModeSelectProcessor.Ative();
            ModeSelectProcessor.OnSelected += (x) => PlayProcessor.Ative(x);
            ModeSelectProcessor.OnSelected += (x) => ModeSelectProcessor.Deative();
        }
    }
}

