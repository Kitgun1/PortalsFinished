using UnityEngine;

public class CheckWebGLPlatform : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool IsMobile();
#endif

    public bool CheckIfMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        var isMobile = IsMobile();
#else
        var isMobile = false;
#endif

        return isMobile;
    }
}
