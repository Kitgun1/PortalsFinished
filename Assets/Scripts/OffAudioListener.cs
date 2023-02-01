using UnityEngine;

public class OffAudioListener : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("Music", 1) == 0)
        {
            AudioListener.volume = 0;
        }
    }
}
