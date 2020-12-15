using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIAP : MonoBehaviour
{
    #region 싱글톤
    private static TestIAP _instance = null;

    public static TestIAP Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<TestIAP>();
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

    void HandleClick(string targetProductId) {
        // 이미 구매한 상품인지 확인
        if(targetProductId == IAPManager.ProductSkin || targetProductId == IAPManager.ProductSub) {
            if(IAPManager.Instance.HadPurchased(targetProductId)){
                Debug.Log("이미 구매한 상품");
                return;
            }
        }

        IAPManager.Instance.Purchase(targetProductId);
    }

    public void OnbtnPurchaseGold() {
        HandleClick("gold");
    }

    public void OnbtnPurchaseSkin() {
        HandleClick("skin");
    }

    public void OnbtnPurchaseSub() {
        HandleClick("sub");
    }
}
