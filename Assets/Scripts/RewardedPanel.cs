using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK.Data;

public class RewardedPanel : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _closeBtn;
    private GunObject _gunObject;

    public void OpenPanel(GunObject gunObject, FinishLvl finishLvl)
    {
        _icon.sprite = gunObject.sprite;
        _gunObject = gunObject;
        
        _closeBtn.onClick.AddListener(finishLvl.SetLoadable);
        _closeBtn.onClick.AddListener(ClosePanel);

        YandexPrefs.SetInt("GunTaken" + _gunObject.GunIndex, 1);
        _gunObject.IsTaken = true;
    }

    public void SetSkin()
    {
        YandexPrefs.SetInt("ActiveGun", _gunObject.GunIndex);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
