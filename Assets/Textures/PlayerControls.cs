using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Eiko.YaSDK;
using Eiko.YaSDK.Data;

public class PlayerControls : MonoBehaviour, ITeleportable
{
    public float speed = 6.0f;
    public float MaxSpeed;
    public float jumpSpeed = 8.0f;
    public float rotateSpeed = 10f;
    public float forceout;
    private float _startHeath;
    [SerializeField] float _health;
    [SerializeField] float _takeDistance;

    private Rigidbody _rigidbody;
    public Transform playerCamera;
    private bool _isTeleported;

    [SerializeField] private GameObject _placeForCatch;
    private bool _isCarring;

    [SerializeField] private ParticleSystem _carringEffect;
    private bool _isGrounded;

    [SerializeField] private AudioSource _carringSound;
    [SerializeField] private GameObject _menuPopUp;
    [SerializeField] private Animator _animator;

    [HideInInspector] public bool IsImpulse = false;


    private float _time = 0;

    public Animator Animator
    {
        set => _animator = value;
    }

    private YandexSDK _yandexSDK;

    [SerializeField] private Image _reflectionDamageImage;

    public bool IsMenuOpen;
    private bool _isJumpPressed = false;
    private bool _isDead;
    public bool IsMobile;

    private Joystick _joystick;
    private Joystick _cameraJoystick;
    private BoxCollider2D _collider;
    private MainCanvas _canvas;

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

        LvlTransition.Instance.OpenLvl();
        _yandexSDK = YandexSDK.instance;
        _startHeath = _health;
        rotateSpeed = PlayerPrefs.GetFloat("PlayerSensitivity", 4);

        IsMobile = GetComponent<CheckWebGLPlatform>().CheckIfMobile();
        _canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<MainCanvas>();
        _joystick = _canvas.joystick;
        _cameraJoystick = _canvas.CameraJoystick;
        _collider = _canvas._collider;
        if (!IsMobile)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void End()
    {
        _canvas.End();
    }

    void Update()
    {
        if (!IsMenuOpen)
        {
            if (!IsMobile)
            {
                MouseRotation();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!_isCarring)
                    {
                        TakeObject();
                    }
                    else
                    {
                        DropObject();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (IsMenuOpen)
                    {
                        _menuPopUp.SetActive(false);
                        Cursor.lockState = CursorLockMode.Locked;
                        IsMenuOpen = false;
                    }
                    else
                    {
                        _menuPopUp.SetActive(true);
                        Cursor.lockState = CursorLockMode.None;
                        IsMenuOpen = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.N))
                {
                    _yandexSDK.ShowRewarded("SkipLvl");
                    _yandexSDK.onRewardedAdReward += SkipLvl;
                    //StartCoroutine(OnFinishLvl());
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _isJumpPressed = true;
                }
            }
            else
            {
                //if (_canvas._collider.bounds.Contains(Input.mousePosition))
                //{
                //MobileRotation();
                //}
            }

            if (_health < _startHeath)
            {
                _health += Time.deltaTime / 1.5f;

                if (_reflectionDamageImage != null)
                {
                    _reflectionDamageImage.color = new Color(1, 0, 0,
                        Mathf.Lerp(0f, 0.4f, (float)Normalize((double)_health, 0d, 7d, 1d, 0d)));
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsMenuOpen)
                {
                    _menuPopUp.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    IsMenuOpen = false;
                }
                else
                {
                    _menuPopUp.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    IsMenuOpen = true;
                }
            }
        }
    }

    private void MobileRotation()
    {
        if (_collider.bounds.Contains(Input.mousePosition))
        {
            float ratX = _cameraJoystick.Horizontal;
            float ratY = _cameraJoystick.Vertical;

            transform.Rotate(0, ratX / 2 * rotateSpeed, 0);

            playerCamera.Rotate(-ratY / 2 * rotateSpeed, 0, 0);
            if (playerCamera.localRotation.eulerAngles.y != 0)
            {
                playerCamera.Rotate(ratY / 2 * rotateSpeed, 0, 0);
            }
        }
    }

    private void MouseRotation()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);

        playerCamera.Rotate(-Input.GetAxis("Mouse Y") * rotateSpeed, 0, 0);
        if (playerCamera.localRotation.eulerAngles.y != 0)
        {
            playerCamera.Rotate(Input.GetAxis("Mouse Y") * rotateSpeed, 0, 0);
        }
    }

    public void Pause()
    {
        _menuPopUp.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        IsMenuOpen = true;
    }

    public void SkipLvlN()
    {
        _yandexSDK.ShowRewarded("SkipLvl");
        _yandexSDK.onRewardedAdReward += SkipLvl;
    }

    public void SkipLvl()
    {
        _yandexSDK.ShowRewarded("SkipLvl");
        _yandexSDK.onRewardedAdReward += SkipLvl;
    }

    public void Jump()
    {
        _isJumpPressed = true;
    }

    public void Carring()
    {
        if (!_isCarring)
        {
            TakeObject();
        }
        else
        {
            DropObject();
        }
    }

    private void TakeObject()
    {
        if (!_isCarring)
        {
            Ray ray = playerCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if ((hit.collider.gameObject.layer == 14 || hit.collider.gameObject.layer == 7 ||
                     hit.collider.gameObject.layer == 17) &&
                    Vector3.Distance(hit.collider.transform.position, transform.position) <= _takeDistance)
                {
                    Debug.Log(Vector3.Distance(hit.collider.transform.position, transform.position));
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

    private void DropObject()
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

    private void SkipLvl(string str)
    {
        _yandexSDK.onRewardedAdReward -= SkipLvl;

        FinishLvl finishLvl = FindObjectOfType<FinishLvl>();
        finishLvl.StartFinishLvl();
        //StartCoroutine(OnFinishLvl());
    }

    private IEnumerator OnFinishLvl()
    {
        yield return new WaitForSeconds(1.5f);

        YandexPrefs.SetInt("CompleteLvl" + SceneManager.GetActiveScene().buildIndex, 1);
        LvlTransition.Instance.CloseLvl();

        yield return new WaitForSeconds(1.5f);

        if (SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void FixedUpdate()
    {
        MovementLogic();
        if (_isJumpPressed) JumpLogic();

        if (IsMobile) MobileRotation();
    }

    private void MovementLogic()
    {
        float moveHorizontal = 0, moveVertical = 0;

        if (IsMobile)
        {
            if (!_canvas._collider.bounds.Contains(Input.mousePosition))
            {
                moveHorizontal = _joystick.Horizontal;
                moveVertical = _joystick.Vertical;
            }
        }
        else
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");

            moveVertical = Input.GetAxisRaw("Vertical");
        }


        if (Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0)
        {
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        movement = transform.TransformDirection(movement);

        //if (Mathf.Abs(_rigidbody.velocity.x) > MaxSpeed || Mathf.Abs(_rigidbody.velocity.z) > MaxSpeed) return;
        if (Mathf.Abs(_rigidbody.velocity.x) < MaxSpeed &&
            Mathf.Abs(_rigidbody.velocity.y) < MaxSpeed &&
            Mathf.Abs(_rigidbody.velocity.z) < MaxSpeed)
        {
            IsImpulse = false;
        }

        if (IsImpulse) return;
//        print($"{Mathf.Abs(_rigidbody.velocity.x + _rigidbody.velocity.z)} ~ {speed * 2 + speed / 2}");
        if (Mathf.Abs(movement.x * speed) < speed - speed / 10 ||
            Mathf.Abs(movement.y * speed) < speed - speed / 10 ||
            Mathf.Abs(movement.z * speed) < speed - speed / 10)
        {
            _rigidbody.velocity = new Vector3(movement.x * speed, _rigidbody.velocity.y, movement.z * speed);
        }
        else
        {
            _rigidbody.AddForce(movement * speed, ForceMode.Force);
        }
        //rigidbody.velocity =
        //   transform.TransformDirection(new Vector3(moveHorizontal * speed, _rigidbody.velocity.y,
        //   moveVertical * speed)) + _impulseTemp;
    }

    private void JumpLogic()
    {
        if (_isGrounded)
        {
            _isJumpPressed = false;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpSpeed, _rigidbody.velocity.z);
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

    private IEnumerator OnReLoadScene()
    {
        LvlTransition.Instance.CloseLvl();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GetDamage(float damage, Transform damagePoint, float force)
    {
        _health -= damage;
        if (_health <= 0 && !_isDead)
        {
            _isDead = true;
            StartCoroutine(OnReLoadScene());
        }

        _rigidbody.AddForce((transform.position - damagePoint.position) * force, ForceMode.Force);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, _takeDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dead"))
        {
            StartCoroutine(OnReLoadScene());
        }
    }

    double Normalize(double val, double valMin, double valMax, double min, double max)
    {
        return (((val - valMin) / (valMax - valMin)) * (max - min)) + min;
    }
}