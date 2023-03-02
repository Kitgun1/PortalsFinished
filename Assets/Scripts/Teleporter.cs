using System;
using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Teleporter Other;
    public bool IsTeleporting;
    public Transform TeleportPoint;

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void Teleport(Transform obj)
    {
        // Position
        Vector3 localPos = TeleportPoint.worldToLocalMatrix.MultiplyPoint3x4(obj.position);
        //localPos = new Vector3(-localPos.x, localPos.y, -localPos.z);
        obj.position = Other.TeleportPoint.transform.localToWorldMatrix.MultiplyPoint3x4(localPos);

        Rigidbody ct = obj.GetComponent<Rigidbody>();
        float forceOut = Mathf.Abs(ct.velocity.y) < 3f ? 1f : obj.GetComponent<PlayerControls>().forceout;
        Vector3 directionForce = Other.TeleportPoint.transform.forward * Mathf.Abs(ct.velocity.y) * forceOut;
        directionForce = new Vector3(Mathf.Min(Mathf.Abs(directionForce.x), 35f) * Mathf.Sign(directionForce.x),
            Mathf.Min(Mathf.Abs(directionForce.y), 35f) * Mathf.Sign(directionForce.y),
            Mathf.Min(Mathf.Abs(directionForce.z), 35f) * Mathf.Sign(directionForce.z)
            );

        // Rotation
        Quaternion difference = new Quaternion(0, Other.transform.rotation.y, 0, Other.transform.rotation.w) * Quaternion.Inverse(new Quaternion(0, transform.rotation.y, 0, transform.rotation.w) * Quaternion.Euler(0, 180, 0));
        obj.rotation = difference * obj.rotation;

        ct.AddForce(directionForce, ForceMode.Impulse);

        Turret turret = obj.GetComponent<Turret>();
        if (turret != null)
        {
            turret.Kill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ITeleportable teleportObject = other.GetComponent<ITeleportable>();

        if (teleportObject == null) return;

        if (!teleportObject.IsTeleported())
        {
            Other.IsTeleporting = true;
            teleportObject.OnTeleportStart();
            Teleport(other.transform);
            StartCoroutine(OnExit());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private IEnumerator OnExit()
    {
        yield return new WaitForSeconds(0.2f);

        IsTeleporting = false;
    }
}
