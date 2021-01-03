using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#if UNITY_IOS || UNITY_EDITOR_OSX
using UnityEngine.SocialPlatforms.GameCenter;
#endif

// 소셜 참고용 스크립트2
// 게임 동작을 위한 버튼만 호출한다.
// 참고 바람.

public class TestA : MonoBehaviour
{
    [SerializeField] Text txtLog;
    bool isNew;
    int nScore;
    int nIQ;
    int nAchievement;
    [SerializeField] Text txtScore;
    [SerializeField] Text txtIQ;
    [SerializeField] Text txtAchievement;

    void Awake()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#endif 
        isNew = true;
        nScore = 0;
        nIQ = 0;
        nAchievement = 0;
    }

    public void OnBtnLoginClicked()
    {
        //이미 인증된 사용자는 바로 로그인 성공됩니다.
        if (Social.localUser.authenticated)
        {
            Debug.Log(Social.localUser.userName);
            txtLog.text = "name : " + Social.localUser.userName + "\n";
        }
        else
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log(Social.localUser.userName);
                    txtLog.text = "name : " + Social.localUser.userName + "\n";
#if UNITY_IOS
                    GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
                }
                else
                {
                    Debug.Log("Login Fail");
                    txtLog.text = "Login Fail\n";
                }
            });
    }

    public void OnBtnLogoutClicked()
    {
        // Active된 Social Plaform을 호출하여, PlayGamesPlatform 으로 대입시켜 SignOut 한다.
#if UNITY_ANDOID
        ((PlayGamesPlatform)Social.Active).SignOut();
#endif
        // Active 여부 상관 없이 SignOut() 호출
        //PlayGamesPlatform.Instance.SignOut();
    }

    public void OnBtnShowAchievementClicked()
    {
        Social.ShowAchievementsUI();
    }

    public void OnBtnQuitClicked()
    {
        Application.Quit();
    }

#region SimpleGame
    public void OnBtnPlayGameClicked()
    {
        if (isNew)
            //초보 탈출 (표준 유형은 progress 값을 바로 100으로 넣어주면 됩니다.)
            Social.ReportProgress(GPGSIds.achievement, 100.0f, (bool success) =>
            {
                if (success)
                {
                    isNew = false;
                    Debug.Log("초보 탈출 획득 성공");
                    txtLog.text = "초보 탈출 획득 성공";
                    //단계별 업적의 경우 ReportProgress 보다 IncrementAchievement 함수를 사용하는 게 좋습니다.
                    //Social.ReportProgress(GPGSIds.achievement, 0.5f, null);
#if UNITY_ANDOID
                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement__4, 1, null);

                    PlayGamesPlatform.Instance.LoadAchievements((UnityEngine.SocialPlatforms.IAchievement[] achieves) =>
                    {
                        Debug.Log(achieves[0].id + "!!!!!!!!!!!");
                    });
#endif
                    nAchievement++;
                    txtAchievement.text = "달성한 업적 수 : " + nAchievement;
                    //마지막 인자에 null을 넣어주면 콜백을 받지 않습니다.
                }
                else
                {
                    Debug.Log("초보 탈출 획득 실패");
                    txtLog.text = "초보 탈출 획득 실패";
                }
            });
    }

    public void OnBtnGetScoreClicked() {
        int nStep = 1;
        nScore += 50;
        txtScore.text = "점수 : " + nScore;

        if (nScore.Equals(1000)) {
#if UNITY_ANDOID
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement__1000, nStep, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("점수 획득 업적 달성" + nScore);
                    txtLog.text = "점수 획득 업적 달성" + nScore;
                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement__4, 1, null);
                    nAchievement++;
                    txtAchievement.text = "달성한 업적 수 : " + nAchievement;
                }
                else
                {
                    Debug.Log("점수 획득 업적 달성 실패");
                    txtLog.text = "점수 획득 업적 달성 실패";
                    nScore = 950;
                    txtScore.text = "점수 : " + nScore;
                }
            });
#endif
        }
        //1000점을 10단계로 했으니 1단계당 100점이므로 100점마다 단계를 올려줍니다.
        else if (nScore < 1000 && (nScore % 100).Equals(0)) {
#if UNITY_ANDOID
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement__1000, nStep, (bool success) => {
                if (success) {
                    Debug.Log("점수 획득 성공" + nScore);
                    txtLog.text = "점수 획득 성공" + nScore;
                }
                else {
                    Debug.Log("점수 획득 실패");
                    txtLog.text = "점수 획득 실패";
                    nScore -= 50;
                    txtScore.text = "점수 : " + nScore;
                }
            });
#endif
        }
    }

    public void OnBtnHiddenClicked()
    {
        int nStep = 1;
        nIQ += 50;
        txtIQ.text = "IQ : " + nIQ;

        //처음으로 이 버튼을 찾으면 천재가 된다.
        if (nIQ.Equals(50))
        {
            //천재 (표준 유형은 progress 값을 바로 100으로 넣어주면 된다.)
            Social.ReportProgress(GPGSIds.achievement_2, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("천재 획득 성공");
                    txtLog.text = "천재 획득 성공";
#if UNITY_ANDOID
                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement__4, 1, null);
#endif
                    nAchievement++;
                    txtAchievement.text = "달성한 업적 수 : " + nAchievement;
                }
                else
                {
                    Debug.Log("천재 획득 실패");
                    txtLog.text = "천재 획득 실패";
                    nIQ = 0;
                    txtIQ.text = "IQ : " + nIQ;
                }
            });
            //동시에 IQ 200 업적을 공개하기로 한다.
            //IQ 200 (progress 값을 바로 0으로 넣어주면 숨김 업적이 공개가 된다.)
            Social.ReportProgress(GPGSIds.achievement__200, 0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("IQ 공개 성공");
                    txtLog.text = "IQ 공개 성공";
                }
                else
                {
                    Debug.Log("IQ 공개 실패");
                    txtLog.text = "IQ 공개 실패";
                }
            });
        }

        //200점을 2단계로 했으니 1단계당 100점이므로 100점마다 단계를 올려준다.
        if (nIQ <= 200 && (nIQ % 100).Equals(0)) {
#if UNITY_ANDOID
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement__200, nStep, (bool success) => {
                if (success) {
                    if (nIQ.Equals(200)) {
                        Debug.Log("IQ 200 업적 획득");
                        txtLog.text = "IQ 200 업적 획득";
                        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement__4, 1, null);
                        nAchievement++;
                        txtAchievement.text = "달성한 업적 수 : " + nAchievement;
                    }
                    else {
                        Debug.Log("IQ 올리기 성공" + nScore);
                        txtLog.text = "IQ 올리기 성공" + nScore;
                    }
                }
                else {

                    if (nIQ.Equals(200)) {
                        Debug.Log("IQ 200 업적 획득 실패");
                        txtLog.text = "IQ 200 업적 획득 실패";
                        nIQ = 150;
                    }
                    else {
                        Debug.Log("IQ 올리기 실패");
                        txtLog.text = "IQ 올리기 실패";
                        nIQ -= 50;
                    }
                }
            });
#endif
        }

    }
#endregion
}
