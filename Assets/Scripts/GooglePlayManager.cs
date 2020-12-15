using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Text;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class GooglePlayManager : MonoBehaviour {
    #region 싱글톤
    private static GooglePlayManager _instance = null;

    public static GooglePlayManager Instance {
        get {
            if (_instance == null) {
                _instance = (GooglePlayManager)FindObjectOfType(typeof(GooglePlayManager));
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
    }
    #endregion

    #region Local fields
    [SerializeField] Text textinfo;

    #endregion

    #region 구글플레이 초기화 및 로그인
    private IEnumerator Start() //동적도 할당을 해주어야 한다.
    {
        yield return new WaitForSeconds(2.0f);
        CheckSocialLogin();
        yield return new WaitForSeconds(2.0f);
        ShowAchievement();
    }

    public bool isProcessing {
        get;
        private set;
    }
    public string loadedData {
        get;
        private set;
    }

    public bool IsLogined => Social.localUser.authenticated;

    private void CheckSocialLogin() {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        //.EnableSavedGames() // 저장된 게임 활성화 : 안쓰시니까 주석처리
        .Build();

        PlayGamesPlatform.InitializeInstance(config);

        // 구글 플레이 로그를 확인할려면 활성화
        PlayGamesPlatform.DebugLogEnabled = false;

        // 구글 플레이 활성화
        PlayGamesPlatform.Activate();

        // 로그인 안돼있을 경우 로그인 호출
        if (PlayGamesPlatform.Instance.IsAuthenticated() == false)
            SocialSignIn();

#elif UNITY_IOS
        SocialSignIn();
#endif
    }

    // 로그인
    public void SocialSignIn() {
        Social.localUser.Authenticate((success) => {
            if (success) {
                Debug.Log("로그인 성공");
                textinfo.text = "로그인 성공";
#if UNITY_IOS
                GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
            }
            else {
                Debug.Log("로그인 실패");
                textinfo.text = "로그인 실패";
            }

        });
    }

    #endregion

    #region 리더보드
    public void ShowMainLeaderBoard() {
        if (Social.localUser.authenticated == false) {
            Social.localUser.Authenticate((bool success) => {
                if (success) {
                    // Sign In 성공
                }
                else {
                    // Sign In 실패 
                    return;
                }
            });
        }

#if UNITY_ANDROID
        //PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_sum_of_all_stage_score);
#elif UNITY_IOS
        GameCenterPlatform.ShowLeaderboardUI(GPGSIds.leaderboard_sum_of_all_stage_score, UnityEngine.SocialPlatforms.TimeScope.AllTime);
#endif
    }
    #endregion

    #region 업적
    public void ShowAchievement() {
        if (Social.localUser.authenticated == false) {
            Social.localUser.Authenticate((bool success) => {
                if (success) {
                    // Sign In 성공
                    return;
                }
                else {
                    // Sign In 실패 처리
                    return;
                }
            });
        }

        Social.ShowAchievementsUI();
    }

    #endregion
}