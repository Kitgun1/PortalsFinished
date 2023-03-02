using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK.Data;
using Eiko.YaSDK;

public class MarketItem : MonoBehaviour
{
    [SerializeField] private GunObject _gunObject;
    [SerializeField] private GameObject _GetButton;
    [SerializeField] private GameObject _SetButton;
    [SerializeField] private GameObject _SettedLable;
    [SerializeField] private Text _adsCountText;
    private MarketContainer _marketContainer;

    private void Start()
    {
        _marketContainer = MarketContainer.Instance;
        if (!_gunObject.IsTaken)
        {
            _GetButton.SetActive(true);
            _SetButton.SetActive(false);
            _SettedLable.SetActive(false);
            _adsCountText.text = _gunObject.AdWatching + "/" + _gunObject.AdToTake;
        }
        else
        {
            if (YandexPrefs.GetInt("ActiveGun", 0) == _gunObject.GunIndex)
            {
                _GetButton.SetActive(false);
                _SetButton.SetActive(false);
                _SettedLable.SetActive(true);
            }
            else
            {
                _GetButton.SetActive(false);
                _SetButton.SetActive(true);
                _SettedLable.SetActive(false);
            }
        }
    }

    public void WatchAdForGun()
    {
        YandexSDK.instance.ShowRewarded(_gunObject.GunIndex.ToString());
        YandexSDK.instance.onRewardedAdReward += GetGun;
        //YandexSDK.instance.onRewardedAdError += StopAd;
        //YandexSDK.instance.onRewardedAdClosed += StopAd;
    }

    public void StopAd(int num)
    {
        YandexSDK.instance.onRewardedAdReward -= GetGun;
        YandexSDK.instance.onRewardedAdError -= StopAd;
        YandexSDK.instance.onRewardedAdClosed -= StopAd;
    }

    public void StopAd(string str)
    {
        YandexSDK.instance.onRewardedAdReward -= GetGun;
        YandexSDK.instance.onRewardedAdError -= StopAd;
        YandexSDK.instance.onRewardedAdClosed -= StopAd;
    }

    public void GetGun(string str)
    {
        YandexSDK.instance.onRewardedAdReward -= GetGun;
        YandexSDK.instance.onRewardedAdError -= StopAd;
        YandexSDK.instance.onRewardedAdClosed -= StopAd;

        int adWatched = _gunObject.AdWatching;
        YandexPrefs.SetInt("GunAdWatched" + _gunObject.GunIndex, adWatched + 1);
        _gunObject.AdWatching = adWatched + 1;
        _adsCountText.text = _gunObject.AdWatching + "/" + _gunObject.AdToTake;

        if (_gunObject.AdWatching >= _gunObject.AdToTake)
        {
            YandexPrefs.SetInt("GunTaken" + _gunObject.GunIndex, 1);
            _gunObject.IsTaken = true;
            _GetButton.SetActive(false);
            _SetButton.SetActive(true);
            _SettedLable.SetActive(false);
        }
    }

    public void SetGun()
    {
        MarketItem[] items = _marketContainer._items;

        foreach (var item in items)
        {
            item.Disable();
        }

        YandexPrefs.SetInt("ActiveGun", _gunObject.GunIndex);
        _GetButton.SetActive(false);
        _SetButton.SetActive(false);
        _SettedLable.SetActive(true);
    }

    public void Disable()
    {
        if (!_gunObject.IsTaken)
        {
            _GetButton.SetActive(true);
            _SetButton.SetActive(false);
            _SettedLable.SetActive(false);
            _adsCountText.text = _gunObject.AdWatching + "/" + _gunObject.AdToTake;
        }
        else
        {
            _GetButton.SetActive(false);
            _SetButton.SetActive(true);
            _SettedLable.SetActive(false);
        }
    }
}
