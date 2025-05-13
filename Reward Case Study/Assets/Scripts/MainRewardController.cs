using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections; // Required for IEnumerator
using System; // Required for Action
using TMPro;

public class MainRewardController : MonoBehaviour
{
    [Header("UI")]
    public GameObject rewardPopup;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;

    [Header("Animations")]
    public StarRewardAnimator starAnimator;
    public Animator chickenAnimator;
    public string chickenDanceStateName = "Dance";

    [Header("Animations Parameters")]
    public float popupEnterDuration = 0.5f;
    public Ease popupEnterEase = Ease.OutBack;

    private int _finalScore = 0;
    private int _finalCoins = 0;
    private bool _isInitialized = false;

    void Start()
    {
        Debug.Log("[Unity] MainRewardController Start called.");
        if (rewardPopup != null)
        {
            rewardPopup.transform.localScale = Vector3.zero; 
            rewardPopup.SetActive(false); 
        }
        DOTween.Init(); 


        NativeBridge.Instance.NotifyUnityModuleReady();
        _isInitialized = true;
        Debug.Log("[Unity] Unity module is now ready.");

        TriggerRewardSequence("1");
    }
    

    public void TriggerRewardSequence(string rewardDetailsJson)
    {
        if (!_isInitialized)
        {

            return;
        }
        


        _finalScore = UnityEngine.Random.Range(50000, 1000000);
        _finalCoins = UnityEngine.Random.Range(100, 500);

        StartCoroutine(FullRewardCoroutine());
    }

    private IEnumerator FullRewardCoroutine()
    {

        if (rewardPopup == null || starAnimator == null || chickenAnimator == null || scoreText == null || coinText == null)
        {
            yield break; 
        }

        rewardPopup.SetActive(true);
        rewardPopup.transform.DOScale(Vector3.one, popupEnterDuration).SetEase(popupEnterEase);
        yield return new WaitForSeconds(popupEnterDuration);


        bool starsAnimationCompletedSignal = false;
        starAnimator.StartStarAnimation(() => {
            starsAnimationCompletedSignal = true;
        });

        chickenAnimator.Play(chickenDanceStateName);

        yield return new WaitUntil(() => starsAnimationCompletedSignal);

        int currentScore = 0;
        DOTween.To(() => currentScore, x => currentScore = x, _finalScore, 1.0f)
               .OnUpdate(() => scoreText.text = "SCORE\n" + currentScore.ToString("N0"))
               .SetTarget(scoreText);

        int currentCoins = 0;
        DOTween.To(() => currentCoins, x => currentCoins = x, _finalCoins, 1.0f)
               .OnUpdate(() => coinText.text = "COIN\n" + currentCoins.ToString())
               .SetTarget(coinText); 

        yield return new WaitForSeconds(1.1f); 
        NativeBridge.Instance.SendRewardAnimationCompleted();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            DOTween.PauseAll();
            if (chickenAnimator != null) chickenAnimator.speed = 0;
        }
        else
        {
            DOTween.PlayAll();
            if (chickenAnimator != null) chickenAnimator.speed = 1;
        }
    }
    
}