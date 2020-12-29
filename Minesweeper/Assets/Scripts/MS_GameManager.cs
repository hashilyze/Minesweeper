using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_GameManager : GameManager {
        public MS_ModeSelectProcessor ModeSelectProcessor;
        public MS_PlayProcessor PlayProcessor;

        // Boot game-manager
        private void Awake() {
            // Load Resources
            MS_Resources.Load();
            
            // Load Processor
            ModeSelectProcessor = FindObjectOfType<MS_ModeSelectProcessor>();
            PlayProcessor = FindObjectOfType<MS_PlayProcessor>();

            ModeSelectProcessor.Deative();
            PlayProcessor.Deative();
        }
        private void Start() {
            ModeSelectProcessor.Ative();
            ModeSelectProcessor.OnCommited += (mode) => PlayProcessor.Ative(mode);
            ModeSelectProcessor.OnCommited += (mode) => ModeSelectProcessor.Deative();
            PlayProcessor.OnRestart += (() => PlayProcessor.Deative());
            PlayProcessor.OnRestart += (() => ModeSelectProcessor.Ative());
        }
    }
}

