using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class Test_achieves_ : MonoBehaviour {
    #region 싱글톤
    private static Test_achieves_ _instance = null;

    public static Test_achieves_ Instance {
        get {
            if (_instance == null) {
                _instance = (Test_achieves_)FindObjectOfType(typeof(Test_achieves_));
                if (_instance == null) {
                    Debug.Log("There's no active ManagerClass object");
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
        // 기타 초기화
        InitGlobalFields();
    }
    #endregion

    #region Global Fields
    [SerializeField] Text txtLog;
    [SerializeField] Text txtScore;
    [SerializeField] Text txtIQ;
    [SerializeField] Text txtAchievement;
    bool isNew;
    int nScore;
    int nIQ;
    int nAchievement;

    bool[] SceneClear = new bool[3];

    public bool IsLogined => Social.localUser.authenticated;

    private void InitGlobalFields() {
        isNew = true;
        nScore = 0;
        nIQ = 0;
        nAchievement = 0;
    }
    #endregion

    #region Google Play 초기화 및 로그인
    private void Start() {
        CheckSocialLogin();
    }


    private void CheckSocialLogin() {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);

        // 구글 플레이 로그를 확인할려면 활성화
        PlayGamesPlatform.DebugLogEnabled = true;

        // 구글 플레이 활설화
        PlayGamesPlatform.Activate();

        SocialSignIn();

#elif UNITY_IOS
        SocialSingIn();
#endif
    }

    // 로그인
    public void SocialSignIn() {
        // 이미 인증된 사용자는 바로 로그인 성공됩니다.
        if (Social.localUser.authenticated) {
            txtLog.text = "name : " + Social.localUser.userName + "\n";
        }
        else {
            Social.localUser.Authenticate((bool success) => {
                if (success) {
                    txtLog.text = "name : " + Social.localUser.userName + "\n";
                }
                else {
                    txtLog.text = "Login Fail\n";
                }
            });
        }
    }

    // 로그아웃
    public void SocialSignOut() {
        // Active된 Social Plaform을 호출하여, PlayGamesPlatform 으로 대입시켜 SignOut 한다.
        ((PlayGamesPlatform)Social.Active).SignOut();
        // Active 여부 상관 없이 SignOut() 호출
        //PlayGamesPlatform.Instance.SignOut();
    }
    #endregion

    #region 버튼 동작 함수
    // LogIn
    public void OnBtnLoginClicked() {
        SocialSignIn();
    }

    // LogOut
    public void OnBtnLogOutclicked() {
        SocialSignOut();
    }

    public void OnBtnShowAchievementClicked() {
        Social.ShowAchievementsUI();
    }

    public void OnBtnQuitClicked() {
        Application.Quit();
    }


    public void OnBtnScene00() {
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_coverd_scenario_00, 100f, null);
    }

    public void OnBtnScene01() {
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_coverd_scenario_01, 100f, null);
    }

    public void OnBtnScene02() {
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_coverd_scenario_02, 100f, null);
    }

    public void OnBtnSceneCL() {
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_coverd_scenario_00, 0f, null);
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_coverd_scenario_01, 0f, null);
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_coverd_scenario_02, 0f, null);
    }
    #endregion
}