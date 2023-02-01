using UnityEngine;
using UnityEngine.UI;

public class ChangeSensitivity : MonoBehaviour
{
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void ChangeSins()
    {
        PlayerPrefs.SetFloat("PlayerSensitivity", _slider.value);
    }
}
