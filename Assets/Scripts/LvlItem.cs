using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Eiko.YaSDK.Data;

public class LvlItem : MonoBehaviour
{
    [SerializeField] private Button _lvlButton;
    [SerializeField] private int _sceneIndex;
    [SerializeField] private GameObject _lock;
    [SerializeField] private Text _text;
    [SerializeField] private bool _isTitorial;

    private void Start()
    {
        _lvlButton.onClick.AddListener(LoadScene);
        YandexPrefs.Init();
        if (!_isTitorial) _text.text = (_sceneIndex - 1).ToString();

        if (_sceneIndex != 1)
        {
            if (YandexPrefs.GetInt("CompleteLvl" + (_sceneIndex - 1), 0) == 1)
            {
                _lvlButton.interactable = true;
                _lock.SetActive(false);
            }
            else
            {
                _lvlButton.interactable = false;
                _lock.SetActive(true);
            }
        }
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneWait());
    }

    private IEnumerator LoadSceneWait()
    {
        LvlTransition.Instance.CloseLvl();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(_sceneIndex);
    }
}
