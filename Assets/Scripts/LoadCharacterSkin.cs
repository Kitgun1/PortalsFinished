using UnityEngine;
using Eiko.YaSDK.Data;

public class LoadCharacterSkin : MonoBehaviour
{
    [SerializeField] CharacterGameItem[] _characterGameItems;
    private PlayerControls _playerControls;

    private void Start()
    {
        _playerControls = GetComponent<PlayerControls>();

        foreach (var item in _characterGameItems)
        {
            if (YandexPrefs.GetInt("ActiveChar", 0) == item.characterObject.CharIndex)
            {
                item.gameObject.SetActive(true);
                _playerControls.Animator = item.animator;
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
