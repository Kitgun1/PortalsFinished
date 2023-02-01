using UnityEngine;

public class SoundOffButton : MonoBehaviour
{
    public void Toggle()
    {
        if (PlayerPrefs.GetInt("Music", 1) == 0)
        {
            OnSound();
        }
        else
        {
            OffSound();
        }
    }

    public void OffSound()
    {
        PlayerPrefs.SetInt("Music", 0);
        AudioListener.volume = 0;
    }

    public void OnSound()
    {
        PlayerPrefs.SetInt("Music", 1);
        AudioListener.volume = 1;
    }
}
