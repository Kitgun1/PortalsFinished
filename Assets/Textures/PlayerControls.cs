using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour, ITeleportable
{
    public float speed = 6.0f;
    public float MaxSpeed;
    public float jumpSpeed = 8.0f;
    public float rotateSpeed = 10f;
    public float forceout;

    private Rigidbody _rigidbody;
    public Transform playerCamera;

    private bool _isTeleported;

    [SerializeField] private GameObject _placeForCatch;
    private bool _isCarring;

    [SerializeField] private ParticleSystem _carringEffect;

    private bool _isGrounded;
    [SerializeField] private float _isGgroundedDistance;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private AudioSource _carringSound;

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

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        LvlTransition.Instance.OpenLvl();
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);

        playerCamera.Rotate(-Input.GetAxis("Mouse Y") * rotateSpeed, 0, 0);
        if (playerCamera.localRotation.eulerAngles.y != 0)
        {
            playerCamera.Rotate(Input.GetAxis("Mouse Y") * rotateSpeed, 0, 0);
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (!_isCarring)
            {
                Ray ray = playerCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.layer == 7 && Vector3.Distance(hit.collider.transform.position, transform.position) <= 3)
                    {
                        hit.collider.transform.SetParent(_placeForCatch.transform);
                        hit.collider.transform.GetComponent<CarryObject>().SetCarry(_placeForCatch.transform);
                        hit.collider.GetComponent<Rigidbody>().useGravity = false;
                        _carringEffect.Play();
                        _isCarring = true;
                        _carringSound.Play();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(2))
        {
            if (_isCarring)
            {
                Transform child = _placeForCatch.transform.GetChild(0);

                TeleportObject teleport = child.GetComponent<TeleportObject>();

                child.GetComponent<Rigidbody>().useGravity = true;
                child.GetComponent<CarryObject>().BreakCarry();
                child.transform.parent = null;
                _isCarring = false;
                _carringEffect.Stop();
                if (teleport != null)
                {
                    teleport.OnTeleportEnd();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(OnLoadScene());
        }
    }

    private void FixedUpdate()
    {
        MovementLogic();
        JumpLogic();
    }

    private void MovementLogic()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        movement = transform.TransformDirection(movement);

        if (Mathf.Abs(_rigidbody.velocity.x) >= MaxSpeed || Mathf.Abs(_rigidbody.velocity.z) >= MaxSpeed) return;

        _rigidbody.AddForce(movement * speed, ForceMode.Force);

    }

    private void JumpLogic()
    {
        if (Input.GetAxisRaw("Jump") > 0)
        {
            if (_isGrounded)
            {
                _rigidbody.AddForce(transform.up * jumpSpeed);
            }
        }
    }

    private IEnumerator onExit()
    {
        yield return new WaitForSeconds(0.4f);

        _isTeleported = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    void OnCollisionExit(Collision collision)
    {
        IsGroundedUpate(collision, false);
    }

    private void OnCollisionStay(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            _isGrounded = value;
        }
    }

    private IEnumerator OnLoadScene()
    {
        LvlTransition.Instance.CloseLvl();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }
}
