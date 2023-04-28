using System;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    public Portal Red;
    public Portal Blue;
    public Animation _animation;
    public ParticleSystem _RedParticle;
    public ParticleSystem _BlueParticle;
    [SerializeField] private AudioSource _portalShootSound;
    private PlayerControls _player;
    [SerializeField] LayerMask _mask;
    private Camera _camera;
    [SerializeField] private LayerMask _maskForForward;
    [SerializeField] private LayerMask _maskForBehind;

    private void Start()
    {
        _camera = Camera.main;
        _player = GetComponent<PlayerControls>();
    }

    public void ShootBlue()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _mask))
        {
            if (hit.collider.gameObject.layer != 12) return;
            Collider[] overlapBox = Physics.OverlapBox(hit.point + hit.normal * 0.3F, new Vector3(1F, 1.8F, 0.2F) / 2,
                Quaternion.LookRotation(hit.normal), _maskForForward);

            if (overlapBox.Length == 0)
            {
                Transform blue;
                (blue = Blue.transform).rotation = Quaternion.LookRotation(hit.normal);
                blue.position = hit.point + blue.forward * 0.3f;
                _BlueParticle.Play();
                OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                if (portalPlace != null) portalPlace.portal = Blue;

                _portalShootSound.Play();
            }
        }

        _animation.Play();
    }

    public void ShootRed()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, _mask))
        {
            if (hit.collider.gameObject.layer != 12) return;

            Collider[] overlapBox = Physics.OverlapBox(hit.point + hit.normal * 0.3F, new Vector3(1F, 1.8F, 0.2F) / 2,
                Quaternion.LookRotation(hit.normal), _maskForForward);

            if (overlapBox.Length == 0)
            {
                Transform red;
                (red = Red.transform).rotation = Quaternion.LookRotation(hit.normal);
                red.position = hit.point + red.forward * 0.3f;
                _RedParticle.Play();
                OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                if (portalPlace != null) portalPlace.portal = Red;

                _portalShootSound.Play();
            }
        }

        _animation.Play();
    }

    private List<Vector3> _corners = new List<Vector3>(8);

    private void OnDrawGizmos()
    {
        if (_corners.Count < 4) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_corners[0], 0.05F);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_corners[1], 0.05F);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_corners[2], 0.05F);
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(_corners[3], 0.05F);
        Gizmos.color = new Color(0.29f, 0.41f, 1f);
        Gizmos.DrawWireSphere(_corners[4], 0.05F);
        Gizmos.color = new Color(1f, 0.35f, 0.25f);
        Gizmos.DrawWireSphere(_corners[5], 0.05F);
        Gizmos.color = new Color(0.65f, 1f, 0.3f);
        Gizmos.DrawWireSphere(_corners[6], 0.05F);
        Gizmos.color = new Color(0.8f, 1f, 0.75f);
        Gizmos.DrawWireSphere(_corners[7], 0.05F);

        for (int i = 0; i < 8; i++)
        {
            if (i < 4)
            {
                Gizmos.color = new Color(1f, 0f, 0.95f);
                Gizmos.DrawLine(_corners[i], _corners[i] + Red.transform.forward * .2f);
            }
            else
            {
                Gizmos.color = new Color(0.49f, 0f, 1f);
                Gizmos.DrawLine(_corners[i], _corners[i] - Red.transform.forward * .2f);
            }
        }
    }

    private void Update()
    {
        if (_player.IsMenuOpen || _player.IsMobile) return;
        if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) return;

        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _mask))
        {
            if (hit.collider.gameObject.layer != 12) return;

            if (Input.GetMouseButtonDown(1))
            {
                Quaternion lastRotation = Red.transform.rotation;
                Vector3 lastPosition = Red.transform.position;

                Transform redTransform;
                (redTransform = Red.transform).rotation = Quaternion.LookRotation(hit.normal);
                redTransform.position = hit.point + redTransform.forward * 0.3f;
                OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                if (portalPlace != null) portalPlace.portal = Red;

                _corners = GetPortalCorners(Red);
                Red.ColliderChecker.enabled = false;

                // если что-то мешает, возвращаем назад
                if (!IsAllObstacleBehind(Red) || IsObstacleForward(Red))
                {
                    (redTransform = Red.transform).rotation = lastRotation;
                    redTransform.position = lastPosition;
                    _RedParticle.Play();
                    //portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                    //if (portalPlace != null) portalPlace.portal = Red;
                }
                else
                {
                    _RedParticle.Play();
                }

                Red.ColliderChecker.enabled = true;


                //}
            }
            else
            {
                Quaternion lastRotation = Red.transform.rotation;
                Vector3 lastPosition = Red.transform.position;
                
                Transform blueTransform;
                (blueTransform = Blue.transform).rotation = Quaternion.LookRotation(hit.normal);
                blueTransform.position = hit.point + blueTransform.forward * 0.3f;
                OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                if (portalPlace != null) portalPlace.portal = Blue;

                _corners = GetPortalCorners(Blue);
                Blue.ColliderChecker.enabled = false;

                // если что-то мешает, возвращаем назад
                if (!IsAllObstacleBehind(Red) || IsObstacleForward(Red))
                {
                    (blueTransform = Blue.transform).rotation = lastRotation;
                    blueTransform.position = lastPosition;
                    _BlueParticle.Play();
                    //portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                    //if (portalPlace != null) portalPlace.portal = Blue;
                }
                else
                {
                    _BlueParticle.Play();
                }


                Blue.ColliderChecker.enabled = true;
            }

            _portalShootSound.Play();
        }

        _animation.Play();
    }

    private bool IsAllObstacleBehind(Portal portal)
    {
        portal.ColliderChecker.enabled = false;
        int count = 0;
        for (int i = 4; i < 8; i++)
        {
            Vector3 direction = _corners[i] - portal.transform.forward;
            Debug.DrawLine(_corners[i] - portal.transform.forward * 0.03f, direction, Color.cyan, 15);
            if (Physics.Raycast(_corners[i] - portal.transform.forward * 0.03f, direction - _corners[i],
                    out RaycastHit hitAngle, 1F, _maskForBehind))
            {
                print(count + " - " + hitAngle.collider.name);
                count++;
            }
        }

        portal.ColliderChecker.enabled = true;
        if (count == 4)
        {
            return true;
        }

        return false;
    }

    private bool IsObstacleForward(Portal portal)
    {
        portal.ColliderChecker.enabled = false;

        for (int i = 0; i < 4; i++)
        {
            Vector3 direction = _corners[i] + portal.transform.forward;
            Debug.DrawLine(_corners[i] - portal.transform.forward * 0.03f, direction, new Color(1f, 0.92f, 0.81f), 15);
            if (Physics.Raycast(_corners[i] - portal.transform.forward * 0.03f, direction - _corners[i],
                    out RaycastHit hitAngle, 1F, _maskForForward))
            {
                print(" - " + hitAngle.collider.name);
                portal.ColliderChecker.enabled = true;
                return true;
            }
        }

        portal.ColliderChecker.enabled = true;
        return false;
    }

    private static List<Vector3> GetPortalCorners(Portal portal)
    {
        BoxCollider collider = portal.ColliderChecker;
        Vector3 size = collider.size;
        return new List<Vector3>(8)
        {
            portal.transform.TransformPoint(
                collider.center + new Vector3(size.x, size.y, size.z) * 0.5f), // 0
            portal.transform.TransformPoint(
                collider.center + new Vector3(-size.x, size.y, size.z) * 0.5f), // 4
            portal.transform.TransformPoint(
                collider.center + new Vector3(size.x, -size.y, size.z) * 0.5f), // 2
            portal.transform.TransformPoint(
                collider.center + new Vector3(-size.x, -size.y, size.z) * 0.5f), // 6
            portal.transform.TransformPoint(
                collider.center + new Vector3(size.x, size.y, -size.z) * 0.5f), // 1
            portal.transform.TransformPoint(
                collider.center + new Vector3(-size.x, size.y, -size.z) * 0.5f), // 5
            portal.transform.TransformPoint(
                collider.center + new Vector3(size.x, -size.y, -size.z) * 0.5f), // 3
            portal.transform.TransformPoint(
                collider.center + new Vector3(-size.x, -size.y, -size.z) * 0.5f) // 7
        };
    }
}