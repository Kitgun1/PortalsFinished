using UnityEngine;
using System.Collections;

public class TeleportObject : MonoBehaviour, ITeleportable
{
    private bool _isTeleported;

    public bool IsTeleported()
    {
        return _isTeleported;
    }

    public void OnTeleportEnd()
    {
        _isTeleported = false;
    }

    public void OnTeleportStart()
    {
        _isTeleported = true;

        StartCoroutine(onExit());
    }

    private IEnumerator onExit()
    {
        yield return new WaitForSeconds(0.2f);

        _isTeleported = false;
    }
}
