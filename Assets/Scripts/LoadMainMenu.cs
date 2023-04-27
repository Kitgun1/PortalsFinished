using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    [SerializeField] private PlayerControls _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
    }

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

    private IEnumerator OnReloadScene()
    {
        LvlTransition.Instance.CloseLvl();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Reload()
    {
        StartCoroutine(OnReloadScene());
    }

    public void LockCursor()
    {
        _player.IsMenuOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
