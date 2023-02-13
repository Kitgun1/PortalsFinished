using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Eiko.YaSDK;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private List<Sprite> _ruSprites;
    [SerializeField] private List<Sprite> _enSprites;
    [SerializeField] private List<Image> _images;

    private void Start()
    {
        if (YandexSDK.instance.Lang == "ru")
        {
            for (int i = 0; i < _images.Count; i++)
            {
                _images[i].sprite = _ruSprites[i];
            }
        }
        else
        {
            for (int i = 0; i < _images.Count; i++)
            {
                _images[i].sprite = _enSprites[i];
            }
        }
    }
}
