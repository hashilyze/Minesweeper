using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_GameManager : GameManager {
        public MS_SelectProcessor SelectProcessor;
        public MS_PlayProcessor PlayProcessor;


        private void Awake() {
            // Load Resources
            MS_Resources.Load();
            
            // Load Processor
            SelectProcessor = FindObjectOfType<MS_SelectProcessor>();
            PlayProcessor = FindObjectOfType<MS_PlayProcessor>();

            SelectProcessor.Deative();
            PlayProcessor.Deative();
        }
        private void Start() {
            SelectProcessor.OnCommited += (mode) => PlayProcessor.Ative(mode);
            SelectProcessor.OnCommited += (mode) => SelectProcessor.Deative();

            PlayProcessor.OnRestart += (() => PlayProcessor.Deative());
            PlayProcessor.OnRestart += (() => SelectProcessor.Ative());

            SelectProcessor.Ative();
        }
    }
}

