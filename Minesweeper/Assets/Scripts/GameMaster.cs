using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMaster : MonoBehaviour {
    private static GameMaster s_instance;

    private void Awake() {
        if (s_instance is null) {
            LoadInstance();
        }
    }

    private void LoadInstance() {
        s_instance = FindObjectOfType<GameMaster>();
        DontDestroyOnLoad(s_instance.gameObject);
    }

    public string GameName;
    [ContextMenu("Execute game")]
    private void ExecuteGame() {
        SceneManager.LoadScene(GameName);
    }
}
