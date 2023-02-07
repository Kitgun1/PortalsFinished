using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    [SerializeField] private PlayerControls _player;

    public void LoadMenu()
    {
        StartCoroutine(OnLoadScene());
    }

    private IEnumerator OnLoadScene()
    {
        LvlTransition.Instance.CloseLvl();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _player._isMenuOpen = false;
    }
}
