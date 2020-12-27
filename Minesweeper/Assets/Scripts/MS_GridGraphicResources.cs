using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper {
    public class MS_GridGraphicResources : MonoBehaviour {
        #region Public
        public static Color CoverColor => s_instance.m_coverColor;
        public static Color GroundColor => s_instance.m_groundColor;

        public static Sprite GetTile(int id) => s_instance.m_tileSprites[id];
        public static Sprite GetNumber(int id) => s_instance.m_numberSprites[id];
        public static Sprite Mine => s_instance.m_mineSprite;
        public static Sprite Flag => s_instance.m_flagSprite;


        public static void Load() {
            if ((s_instance = FindObjectOfType<MS_GridGraphicResources>()) is null) {
                throw new System.NullReferenceException($"Instance of <{nameof(MS_GridGraphicResources)}> is null");
            }
            for (int t = 0; t != TILE_PATHS.Length; ++t) {
                if ((s_instance.m_tileSprites[t] = Resources.Load<Sprite>(ROOT_PATH + TILE_PATHS[t])) is null) {
                    throw new System.NullReferenceException($"Property <{nameof(m_tileSprites)}> is null");
                }
            }
            for (int n = 0; n != NUMBER_PATHS.Length; ++n) {
                if ((s_instance.m_numberSprites[n] = Resources.Load<Sprite>(ROOT_PATH + NUMBER_PATHS[n])) is null) {
                    throw new System.NullReferenceException($"Property <{nameof(m_numberSprites)}> is null");
                }
            }
            if ((s_instance.m_mineSprite = Resources.Load<Sprite>(ROOT_PATH + MINE_PATH)) is null) {
                throw new System.NullReferenceException($"Property <{nameof(m_mineSprite)}> is null");
            }
            if ((s_instance.m_flagSprite = Resources.Load<Sprite>(ROOT_PATH + FLAG_PATH)) is null) {
                throw new System.NullReferenceException($"Property <{nameof(m_flagSprite)}> is null");
            }
        }
        #endregion

        #region Private
        private static readonly string ROOT_PATH = "Sprites/";

        private static readonly string[] TILE_PATHS = { "Tile1", "Tile2" };
        private static readonly string[] NUMBER_PATHS = { "Num_1", "Num_2", "Num_3", "Num_4", "Num_5", "Num_6", "Num_7", "Num_8", "Num_9" };
        private static readonly string MINE_PATH = "Mine";
        private static readonly string FLAG_PATH = "Flag";

        private static readonly int TILE_NUM = TILE_PATHS.Length;
        private static readonly int NUMBER_NUM = NUMBER_PATHS.Length;

        private static MS_GridGraphicResources s_instance;

        [SerializeField] private Color m_coverColor = new Color(167f, 218f, 71f, 255f);
        [SerializeField] private Color m_groundColor = new Color(228f, 195f, 159f, 255f);

        private readonly Sprite[] m_tileSprites = new Sprite[TILE_NUM];
        private readonly Sprite[] m_numberSprites = new Sprite[NUMBER_NUM];
        private Sprite m_mineSprite;
        private Sprite m_flagSprite;
        #endregion
    }
}
