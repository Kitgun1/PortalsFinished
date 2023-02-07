using UnityEngine;

public class SoundOffButton : MonoBehaviour
{
    [SerializeField] private GameObject _buttonOn;
    [SerializeField] private GameObject _buttonOff;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Music", 1) == 1)
        {
            _buttonOn.SetActive(true);
            _buttonOff.SetActive(false);
        }
        else
        {
            _buttonOff.SetActive(true);
            _buttonOn.SetActive(false);
        }
    }

    public void Toggle()
    {
        if (PlayerPrefs.GetInt("Music", 1) == 0)
        {
            OnSound();
            _buttonOn.SetActive(true);
            _buttonOff.SetActive(false);
        }
        else
        {
            OffSound();
            _buttonOn.SetActive(false);
            _buttonOff.SetActive(true);
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
