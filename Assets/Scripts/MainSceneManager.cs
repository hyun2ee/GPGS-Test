using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    #region 싱글톤
    private static MainSceneManager _instance = null;

    public static MainSceneManager Instance {
        get {
            if (_instance == null) {
                _instance = (MainSceneManager)FindObjectOfType(typeof(MainSceneManager));
                if (_instance == null) {
                    Debug.Log("There's no active MainSceneManager object");
                }
            }
            return _instance;
        }
    }

    void Awake() {
        if (_instance != null && _instance != this) {
            DestroyImmediate(gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public Button obj_btnGame;
    public Button obj_btnStore;

    [Header("Windows")]
    public GameObject obj_SimpleGame;
    public GameObject obj_SimpleStore;

    public void OnBtnGame() {
        obj_SimpleStore.SetActive(false);
        obj_SimpleGame.SetActive(true);
    }

    public void OnBtnStore() {
        obj_SimpleStore.SetActive(true);
        obj_SimpleGame.SetActive(false);
    }
}
