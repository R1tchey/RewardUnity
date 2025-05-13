using UnityEngine;

public class NativeBridge
{
    private static NativeBridge _instance;
    public static NativeBridge Instance => _instance ?? (_instance = new NativeBridge());

    private const string AndroidActivityClassName = "com.unity3d.player.UnityPlayer";
    private const string AndroidActivityStaticMethodName = "currentActivity";


    private const string AndroidCallback_OnAnimationCompleted = "onUnityRewardAnimationCompleted";
    private const string AndroidCallback_OnUnityModuleReady = "onUnityModuleReady";


    public void NotifyUnityModuleReady()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass(AndroidActivityClassName))
        {
            using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>(AndroidActivityStaticMethodName))
            {
                activity.Call(AndroidCallback_OnUnityModuleReady);
            }
        }
#endif
    }

    public void SendRewardAnimationCompleted()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass(AndroidActivityClassName))
        {
            using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>(AndroidActivityStaticMethodName))
            {
                activity.Call(AndroidCallback_OnAnimationCompleted);
            }
        }
#endif
    }
}