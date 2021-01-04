using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    // 유니티에서 상품을 구별하기 위해 쓰는 표면적인 이름
    public const string ProductGold = "gold"; // Consumable
    public const string ProductSkin = "skin"; // UnConsumable
    public const string ProductSub = "sub"; // Subscription

    // 실제 스토어에 등록되어 있는 상품 코드(ID)들
    private const string _iOS_GoldId = "com.studio.app.gold";
    private const string _android_GoldId = "com.studio.app.gold";

    private const string _iOS_SkinId = "com.studio.app.skin";
    private const string _android_SkinId = "com.studio.app.skin";

    private const string _iOS_SubId = "com.studio.app.sub";
    private const string _android_SubId = "com.studio.app.sub";

    // 싱글톤
    #region 싱글톤
    private static IAPManager _instance = null;

    public static IAPManager Instance {
        get {
            if (_instance == null) {
                _instance = (IAPManager)FindObjectOfType(typeof(IAPManager));
                if (_instance == null) {
                    _instance = new GameObject("IAP Manager").AddComponent<IAPManager>();
                    return _instance;
                }
            }
            return _instance;
        }
    }
    #endregion

    // Store Controller / Store Extension Manager
    // ------------------------------------------------------------------------- //
    // Store Controller는 인앱 결제 모듈이 실행되는 순간 생성되는 오브젝트이다.  //
    // (정확히는 IAP 매니저 초기화 시 생성된다.)                                 //
    // 해당 모듈은 인앱 결제에 필요한 함수들을 제공해준다. (구매 과정 통제용)    //
    // Store Extension Manager는 멀티 플랫폼 구매를 구현할 때 필요한 함수를      //
    // 제공해 준다.                                                              //
    // ------------------------------------------------------------------------- //
    private IStoreController storeController; // 구매 과정을 제어하는 함수를 제공
    private IExtensionProvider storeExtensionProvider; // 여러 플랫폼을 위한 확장 처리를 제공

    // IAP 매니저 초기화 및 Contoller와 Extension Manager 캡처
    void Awake() {
    #region 싱글톤(Awake)
    if (_instance != null && _instance != this) {
            DestroyImmediate(gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    #endregion

    // IAP 초기화
    InitUnityIAP();
    }

    // 초기화 여부 확인용,외부에서는 읽기만 가능한 대리 접근 멤버 선언
    public bool IsInitialized => storeController != null && storeExtensionProvider != null;

    void InitUnityIAP() {
        // Unity 표준 구매 모듈(Unity가 기본으로 제공하는 스토어 설정)로 IAP 모듈 정의
        if (IsInitialized) return; // 중복 실행 방지

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // 상품 정의 빌더에 품목 추가
        builder.AddProduct(
            ProductGold, ProductType.Consumable, new IDs() {
                {_iOS_GoldId, AppleAppStore.Name },
                {_android_GoldId, GooglePlay.Name }
            }
        );

        builder.AddProduct(
            ProductSkin, ProductType.NonConsumable, new IDs() {
                {_iOS_SkinId, AppleAppStore.Name },
                {_android_SkinId, GooglePlay.Name }
            }
        );

        builder.AddProduct(
            ProductSub, ProductType.Subscription, new IDs() {
                {_iOS_SubId, AppleAppStore.Name },
                {_android_SubId, GooglePlay.Name }
            }
        );
        
        // 구매 모듈 초기 설정 -> storeController/ExtensionProvider 오브젝트가 생성 됨.
        UnityPurchasing.Initialize(this, builder);
        // 바로 OnInitialized() 가 실행된다.
    }
    
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        // 초기화 성공 시 실행 됨. 입력으로 생성된 controller 와 extentsionprovider 가 들어 온다.
        // 해당 오브젝트들을 캡쳐해준다.
        Debug.Log("유니티 IAP 초기화 성공");
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error) {
        // 초기화 실패 시 실행 됨. 
        Debug.Log($"유니티 IAP 초기화 실패 {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
        // 구매 완료 직전에 실행되는 함수(사실상 구매가 완료됐을 때 실행 됨)
        Debug.Log($"구매 성공 - ID : {args.purchasedProduct.definition.id}");
        // 참고: definition.storespecificid 는 앱스토어 특화 확인용 id, 범용 코드라서 id 사용

        if(args.purchasedProduct.definition.id == ProductGold) {
            Debug.Log("골드 상승 처리...");
        }
        else if(args.purchasedProduct.definition.id == ProductSkin) {
            Debug.Log("스킨 등록...");
        }
        else if (args.purchasedProduct.definition.id == ProductSkin) {
            Debug.Log("구독 서비스 시작...");
        }

        return PurchaseProcessingResult.Complete;  // Pending: 실패
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason) {
        // 구매 실패 시 실행 됨.
        Debug.LogWarning($"구매 실패 - {product.definition.id}, {reason}");
    }


    public void Purchase(string productId) {
        if (!IsInitialized) return; // 스토어 컨트롤러 초기화 확인

        var product = storeController.products.WithID(productId);
        
        // 구매 시도(구매 가능여부 판단)
        if(product != null && product.availableToPurchase) {
            Debug.Log($"구매 시도 - {product.definition.id}");
            storeController.InitiatePurchase(product); // 구매 시동을 걸어라(품목 준비해라)
        }
        else { // 품목이 구매 가능 상태가 아니면
            Debug.Log($"구매 시도 불가 - {productId}");
        }
    }
    
    // Mac / iOS 전용 (구매 복구: 필수 기능이다.)
    public void RestorePurchase() {
        if(!IsInitialized) return; // 스토어 컨트롤러 초기화 확인

        if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer) {
            Debug.Log("구매 복구 시도");

            // 구매 복구 기능은 ExtensionProvider 에서 받아서 써야한다.
            var applExt = storeExtensionProvider.GetExtension<IAppleExtensions>(); // 인터페이스를 받아온다.
            // 구매 복구
            applExt.RestoreTransactions(
                    result => Debug.Log($"구매 복구 시도 결과 - {result}")
                );
        }
    }

    // 구매 확인 함수
    public bool HadPurchased(string productId) {
        if (!IsInitialized) return false;

        var product = storeController.products.WithID(productId);

        if(product != null) {
            return product.hasReceipt; // 영수증이 있으면, 구매한것이다. Nonconsumable, subsript 상품에만 해당한다. consumable은 영수증이 없다.
                                       // 물약은 이전에 샀는지 안샀는지 알 필요 없잖아?
        }

        return false; // 상품 가져오기 실패 시
    }

}
