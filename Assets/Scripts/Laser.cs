using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer _laserLine;
    public bool IsActive = false;
    [SerializeField] private LayerMask _mask;

    private void Start()
    {
        _laserLine = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (IsActive)
        {
            _laserLine.SetPosition(0, transform.position);
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.layer);
                _laserLine.SetPosition(1, hit.point);

                if (hit.collider.gameObject.layer == 14)
                {
                    Ray ray2 = new Ray(hit.transform.position, hit.transform.GetChild(0).transform.forward);
                    RaycastHit hit2;

                    if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, _mask))
                    {
                        _laserLine.SetPosition(2, hit.transform.position);
                        _laserLine.SetPosition(3, hit2.point);

                        if (hit2.collider.gameObject.layer == 17)
                        {
                            Turret turret = hit2.transform.GetComponent<Turret>();
                            turret.Kill();
                        }
                    }
                }
                else
                {
                    _laserLine.SetPosition(2, hit.point);
                    _laserLine.SetPosition(3, hit.point);
                }
            }
        }
        else
        {
            _laserLine.SetPosition(0, new Vector3(1000, 1000, 1000));
            _laserLine.SetPosition(1, new Vector3(1000, 1000, 1000));
            _laserLine.SetPosition(2, new Vector3(1000, 1000, 1000));
            _laserLine.SetPosition(3, new Vector3(1000, 1000, 1000));
        }
    }
}
