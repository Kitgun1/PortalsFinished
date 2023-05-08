using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
            Quaternion lastRotation = Blue.transform.rotation;
            Vector3 lastPosition = Blue.transform.position;
            Transform blueTransform;
            (blueTransform = Blue.transform).rotation = Quaternion.LookRotation(hit.normal);
            blueTransform.position = hit.point + blueTransform.forward * 0.3f;
            OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
            if (portalPlace != null) portalPlace.portal = Blue;

            for (int i = 0; i < 40; i++)
            {
                _corners = GetPortalCorners(Blue);
                Blue.ColliderChecker.enabled = false;

                var obstacles = IsAllObstacleBehind(Blue);
                bool isAllObstacleBehind = obstacles.Item1;
                bool isObstacleForward = IsObstacleForward(Blue);


                if (obstacles.Item2.Item2.Length == 4)
                {
                    (blueTransform = Blue.transform).rotation = lastRotation;
                    blueTransform.position = lastPosition;
                    Blue.ColliderChecker.enabled = true;
                    break;
                }

                if (isAllObstacleBehind && !isObstacleForward)
                {
                    break;
                }

                if (isObstacleForward) // спереди что-то есть => возвращаем назад
                {
                    (blueTransform = Blue.transform).rotation = lastRotation;
                    blueTransform.position = lastPosition;
                    Blue.ColliderChecker.enabled = true;
                    break;
                }

                // взади чего-то нет && нет препятсвий спереди => двигаем
                if (!isAllObstacleBehind && !isObstacleForward)
                {
                    int[] indexesNoHit = obstacles.Item2.Item2;
                    Vector3 direction = GetDirectionToObstacle(indexesNoHit, Blue);

                    blueTransform.position += direction * 0.05F;
                    if (portalPlace != null) portalPlace.portal = Blue;
                    Debug.DrawLine(Blue.transform.position + new Vector3(0, 0, Random.Range(-0.01f, 0.01f)),
                        hit.point + blueTransform.forward * 0.1f + direction * 0.2F,
                        new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 2);
                }

                if (!isAllObstacleBehind && i == 40)
                {
                    (blueTransform = Red.transform).rotation = lastRotation;
                    blueTransform.position = lastPosition;
                    Red.ColliderChecker.enabled = true;
                }

                Red.ColliderChecker.enabled = true;
            }

            _RedParticle.Play();
            _portalShootSound.Play();
        }

        _animation.Play();
    }

    public void ShootRed()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, _mask))
        {
            if (hit.collider.gameObject.layer != 12) return;

            Quaternion lastRotation = Red.transform.rotation;
            Vector3 lastPosition = Red.transform.position;
            Transform redTransform;
            (redTransform = Red.transform).rotation = Quaternion.LookRotation(hit.normal);
            redTransform.position = hit.point + redTransform.forward * 0.3f;
            OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
            if (portalPlace != null) portalPlace.portal = Red;

            for (int i = 0; i < 40; i++)
            {
                _corners = GetPortalCorners(Red);
                Red.ColliderChecker.enabled = false;

                var obstacles = IsAllObstacleBehind(Red);
                bool isAllObstacleBehind = obstacles.Item1;
                bool isObstacleForward = IsObstacleForward(Red);
                if (obstacles.Item2.Item2.Length == 4)
                {
                    (redTransform = Red.transform).rotation = lastRotation;
                    redTransform.position = lastPosition;
                    Red.ColliderChecker.enabled = true;
                    break;
                }

                if (isAllObstacleBehind && !isObstacleForward)
                {
                    break;
                }

                if (isObstacleForward) // спереди что-то есть => возвращаем назад
                {
                    (redTransform = Red.transform).rotation = lastRotation;
                    redTransform.position = lastPosition;
                    Red.ColliderChecker.enabled = true;
                    break;
                }

                // взади чего-то нет && нет препятсвий спереди => двигаем
                if (!isAllObstacleBehind && !isObstacleForward)
                {
                    int[] indexesNoHit = obstacles.Item2.Item2;
                    Vector3 direction = GetDirectionToObstacle(indexesNoHit, Red);

                    redTransform.position += direction * 0.05F;
                    if (portalPlace != null) portalPlace.portal = Red;
                    Debug.DrawLine(Red.transform.position + new Vector3(0, 0, Random.Range(-0.01f, 0.01f)),
                        hit.point + redTransform.forward * 0.1f + direction * 0.2F,
                        new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 2);
                }

                if (!isAllObstacleBehind && i == 40)
                {
                    (redTransform = Red.transform).rotation = lastRotation;
                    redTransform.position = lastPosition;
                    Red.ColliderChecker.enabled = true;
                }

                Red.ColliderChecker.enabled = true;
            }

            _RedParticle.Play();
            _portalShootSound.Play();
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

                for (int i = 0; i < 40; i++)
                {
                    _corners = GetPortalCorners(Red);
                    Red.ColliderChecker.enabled = false;

                    var obstacles = IsAllObstacleBehind(Red);
                    bool isAllObstacleBehind = obstacles.Item1;
                    bool isObstacleForward = IsObstacleForward(Red);
                    print($"{obstacles.Item2.Item2.Length}");
                    if (obstacles.Item2.Item2.Length == 4)
                    {
                        redTransform.rotation = lastRotation;
                        redTransform.position = lastPosition;
                        Red.ColliderChecker.enabled = true;
                        break;
                    }

                    if (isAllObstacleBehind && !isObstacleForward)
                    {
                        break;
                    }

                    if (isObstacleForward) // спереди что-то есть => возвращаем назад
                    {
                        redTransform .rotation = lastRotation;
                        redTransform.position = lastPosition;
                        Red.ColliderChecker.enabled = true;
                        break;
                    }

                    // взади чего-то нет && нет препятсвий спереди => двигаем
                    if (!isAllObstacleBehind && !isObstacleForward)
                    {
                        int[] indexesNoHit = obstacles.Item2.Item2;
                        Vector3 direction = GetDirectionToObstacle(indexesNoHit, Red);

                        redTransform.position += direction * 0.05F;
                        if (portalPlace != null) portalPlace.portal = Red;
                        Debug.DrawLine(Red.transform.position + new Vector3(0, 0, Random.Range(-0.01f, 0.01f)),
                            hit.point + redTransform.forward * 0.1f + direction * 0.2F,
                            new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 2);
                    }

                    if (!isAllObstacleBehind && i == 40)
                    {
                        redTransform.rotation = lastRotation;
                        redTransform.position = lastPosition;
                        Red.ColliderChecker.enabled = true;
                    }

                    Red.ColliderChecker.enabled = true;
                }

                _RedParticle.Play();
            }
            else
            {
                Quaternion lastRotation = Blue.transform.rotation;
                Vector3 lastPosition = Blue.transform.position;
                Transform redTransform;
                (redTransform = Blue.transform).rotation = Quaternion.LookRotation(hit.normal);
                redTransform.position = hit.point + redTransform.forward * 0.3f;
                OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                if (portalPlace != null) portalPlace.portal = Blue;

                for (int i = 0; i < 40; i++)
                {
                    _corners = GetPortalCorners(Blue);
                    Blue.ColliderChecker.enabled = false;

                    var obstacles = IsAllObstacleBehind(Blue);
                    bool isAllObstacleBehind = obstacles.Item1;
                    bool isObstacleForward = IsObstacleForward(Blue);

                    if (obstacles.Item2.Item2.Length == 4)
                    {
                        (redTransform = Blue.transform).rotation = lastRotation;
                        redTransform.position = lastPosition;
                        Blue.ColliderChecker.enabled = true;
                        break;
                    }

                    if (isAllObstacleBehind && !isObstacleForward)
                    {
                        break;
                    }

                    if (isObstacleForward) // спереди что-то есть => возвращаем назад
                    {
                        (redTransform = Blue.transform).rotation = lastRotation;
                        redTransform.position = lastPosition;
                        Blue.ColliderChecker.enabled = true;
                        break;
                    }

                    // взади чего-то нет && нет препятсвий спереди => двигаем
                    if (!isAllObstacleBehind && !isObstacleForward)
                    {
                        int[] indexesNoHit = obstacles.Item2.Item2;
                        Vector3 direction = GetDirectionToObstacle(indexesNoHit, Blue);

                        redTransform.position += direction * 0.05F;
                        if (portalPlace != null) portalPlace.portal = Blue;
                        Debug.DrawLine(Blue.transform.position + new Vector3(0, 0, Random.Range(-0.01f, 0.01f)),
                            hit.point + redTransform.forward * 0.1f + direction * 0.2F,
                            new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 2);
                    }

                    if (!isAllObstacleBehind && i == 40)
                    {
                        (redTransform = Blue.transform).rotation = lastRotation;
                        redTransform.position = lastPosition;
                        Blue.ColliderChecker.enabled = true;
                    }

                    Blue.ColliderChecker.enabled = true;
                }

                _RedParticle.Play();
            }

            _portalShootSound.Play();
        }

        _animation.Play();
    }

    private Vector3 GetDirectionToObstacle(int[] indexesNoHits, Portal portal)
    {
        Transform portalPos = portal.transform;

        Vector3 direction = Vector3.zero;
        if (indexesNoHits.Length == 3)
        {
            for (int i = 4; i <= 7; i++)
            {
                if (!indexesNoHits.Contains(i))
                {
                    direction = i switch
                    {
                        4 => portalPos.right + portalPos.up,
                        5 => -portalPos.right + portalPos.up,
                        6 => portalPos.right + -portalPos.up,
                        7 => -portalPos.right + -portalPos.up,
                        _ => direction
                    };
                }
            }
        }
        else if (indexesNoHits.Length == 1)
        {
            direction = indexesNoHits[0] switch
            {
                4 => -portalPos.up + portalPos.right,
                5 => -portalPos.up + -portalPos.right,
                6 => portalPos.right + portalPos.up,
                7 => -portalPos.right + portalPos.up,
                _ => direction
            };
        }
        else if (indexesNoHits.Length == 2)
        {
            if (indexesNoHits.Contains(4) && indexesNoHits.Contains(5))
            {
                direction = -portalPos.up;
            }
            else if (indexesNoHits.Contains(5) && indexesNoHits.Contains(7))
            {
                direction = portalPos.right;
            }
            else if (indexesNoHits.Contains(6) && indexesNoHits.Contains(7))
            {
                direction = portalPos.up;
            }
            else if (indexesNoHits.Contains(6) && indexesNoHits.Contains(4))
            {
                direction = -portalPos.right;
            }
            else if (indexesNoHits.Contains(4) && indexesNoHits.Contains(7))
            {
                direction = -portalPos.up + -portalPos.right;
            }
            else if (indexesNoHits.Contains(5) && indexesNoHits.Contains(6))
            {
                direction = -portalPos.up + portalPos.right;
            }
        }

        //print(direction + " - Direction Move Portal");
        return direction;
    }

    private (bool, (Vector3[], int[])) IsAllObstacleBehind(Portal portal, Vector3 offset = default(Vector3))
    {
        List<Vector3> noHitsCornersList = new List<Vector3>(4);
        List<int> noHitsCornersIndexList = new List<int>(4);

        portal.ColliderChecker.enabled = false;
        int count = 0;
        for (int i = 4; i < 8; i++)
        {
            Vector3 endPosition = _corners[i] - portal.transform.forward + offset;

            if (Physics.Raycast(_corners[i] - portal.transform.forward * 0.03f, endPosition - _corners[i],
                    out RaycastHit hitAngle, 1F, _maskForBehind))
            {
                count++;
            }
            else
            {
                noHitsCornersList.Add(_corners[i]);
                noHitsCornersIndexList.Add(i);
            }
        }

        portal.ColliderChecker.enabled = true;
        if (count == 4)
        {
            return (true, (noHitsCornersList.ToArray(), noHitsCornersIndexList.ToArray()));
        }

        return (false, (noHitsCornersList.ToArray(), noHitsCornersIndexList.ToArray()));
    }

    private bool IsObstacleForward(Portal portal)
    {
        portal.ColliderChecker.enabled = false;

        for (int i = 0; i < 4; i++)
        {
            Vector3 direction = _corners[i] + portal.transform.forward;
            Debug.DrawLine(_corners[i], direction, new Color(1f, 0f, 0.03f), 15);
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