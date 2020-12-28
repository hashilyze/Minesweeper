using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Minesweeper {
    public class MS_Resources : MonoBehaviour {
        /// <summary>Get resource by code</summary>
        public static Sprite GetSprite(int code) => s_instance.m_codeToResources[code];
        /// <summary>Get resource by name</summary>
        public static Sprite GetSprite(string name) => s_instance.m_codeToResources[s_instance.m_nameToCodes[name]];

        /// <summary>Convert resource name to code</summary>
        public static int NameToCode(string name) => s_instance.m_nameToCodes[name];
        /// <summary>Convert resource code to name</summary>
        public static string CodeToName(int code) => s_instance.m_codeToResources[code].name;

        /// <summary>Load resources</summary>
        public static void Load() {
            Sprite[] sprites = Resources.LoadAll<Sprite>(ROOT_PATH);
            s_instance.m_codeToResources = new Sprite[sprites.Length];

            for (int s = 0, sLen = sprites.Length; s != sLen; ++s) {
                Sprite sprite = sprites[s];
                string[] keys = sprite.name.Split('-');
                int code = int.Parse(keys[0]);
                s_instance.m_codeToResources[code] = sprite;
                s_instance.m_nameToCodes[keys[1]] = code;
            }
        }


        /// <summary>Root path of resources</summary>
        private const string ROOT_PATH = "Sprites";
        private static MS_Resources s_instance;

        /// <summary>Resource register mapping by code</summary>
        private Sprite[] m_codeToResources;
        /// <summary>Dictory mapping name to code</summary>
        private readonly Dictionary<string, int> m_nameToCodes = new Dictionary<string, int>();


        private void Awake() {
            s_instance = this;
        }
    }
}
