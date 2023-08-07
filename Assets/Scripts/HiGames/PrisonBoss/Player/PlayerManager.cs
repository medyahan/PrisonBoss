using System.Collections.Generic;
using DG.Tweening;
using HiGames.PrisonBoss.Event;
using HiGames.PrisonBoss.Game_Manager;
using HiGames.PrisonBoss.Upgrade;
using UnityEngine;

namespace HiGames.PrisonBoss.Player
{
    public class PlayerManager : MonoBehaviour
    {
        // INSTANCE
        public static PlayerManager Instance;
        
        [Header("SPEED VALUE")] 
        [SerializeField] private float _playerSpeed;
        [SerializeField] private float _clampBounds;
        private GameObject _offsetObj;
        private Vector3 _offset, _newPos;
        
        [SerializeField] private float _forwardSpeed;
        [SerializeField] private float _rotateSpeed;
        
        [Header("JOYSTICK")] 
        [SerializeField] private Joystick _joystick;

        [Header("RIGIDBODY")] 
        [SerializeField] private Rigidbody _rb;
        public Rigidbody Rb => _rb;

        [Header("MESH")] 
        [SerializeField] private SkinnedMeshRenderer _playerMesh;
        public SkinnedMeshRenderer PlayerMesh => _playerMesh;
        public Material PlayerMaterial
        {
            get => _playerMesh.material;
            set => _playerMesh.material = value;
        }
        
        [Header("COLLIDER")] 
        [SerializeField] private CapsuleCollider _playerCollider;
        public CapsuleCollider PlayerCollider => _playerCollider;
        [SerializeField] private BoxCollider _carCollider;
        public BoxCollider CarCollider => _carCollider;

        [Header("ANIMATOR")] 
        [SerializeField] private Animator _anim;
        public Animator Anim
        {
            get => _anim;
            set => _anim = value;
        }
        
        [Header("POLICE LEVELS")] 
        [SerializeField] private List<GameObject> _policeLevels = new List<GameObject>();
        
        // BOOL
        private bool _isPoliceCar;
        
        private void Awake()
        {
            Instance = this;
            Input.multiTouchEnabled = false; // MULTI TOUCH DISABLE
        }

        private void Start()
        {
            _offsetObj = new GameObject();
            _offsetObj.name = "OffsetObj";
            
            EventManager.Instance.OnPoliceUpgraded += UpdatePolice;
            EventManager.Instance.OnMovementChanged += ChangePolice;
            
            GetPolice();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _offsetObj.transform.position = new Vector3(GetXPos(), 0, transform.position.z);
                _offset = transform.position - _offsetObj.transform.position;
            }
            
            if (!GameManager.IsGameStarted)
                return;

            
            if (GameManager.Instance.IsRunner)
            {
                if(!_isPoliceCar)
                    _anim.SetFloat("Velocity", 0.6f);
                RunnerMovement();
            }
            else
            {
                _anim.SetFloat("Velocity", _rb.velocity.magnitude);
                JoystickMovement();
            }
        }

        private void RunnerMovement()
        {
            ResetPlayerSpeed();
            
            transform.position += new Vector3(0, 0, _playerSpeed * Time.deltaTime);
            //transform.position = new Vector3(transform.position.x, 0, transform.position.z);


            if (Input.GetMouseButton(0))
            {
                _offsetObj.transform.position = new Vector3(GetXPos(), 0, transform.position.z);
                _newPos = _offsetObj.transform.position + _offset;
                _newPos.y = 0;
                _newPos.z = transform.position.z;
                _newPos.x = Mathf.Clamp(_newPos.x, -_clampBounds, _clampBounds);
                transform.position = Vector3.Lerp(transform.position, _newPos, Time.deltaTime * 15f);
                //transform.position = _newPos;
            }
        }

        private void JoystickMovement()
        {

            if (_joystick.Vertical != 0 || _joystick.Horizontal != 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(
                    _joystick.Horizontal * _rotateSpeed * Time.deltaTime, 0,
                    _joystick.Vertical * _rotateSpeed * Time.deltaTime));
                    
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
                
            }
            _rb.velocity = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical) * _forwardSpeed;
        
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                //_isEscaped = true;
                ShakePoliceCar();
            }

        }
        
        private float GetXPos() // Mouse positiona gore X position degerini dondurur
        {
            float calc = (Input.mousePosition.x * 16) / Screen.width;
            calc -= 8f;
            return calc;
        }
        
        private void ResetPlayerSpeed()
        {
            _rb.velocity = Vector3.zero;
        }

        private void UpdatePolice()
        {
            if (UpgradeManager.Instance.PoliceLevelNo == 1) // Araba olduysak
            {
                _isPoliceCar = true;
                GameManager.Instance.DistanceToPlayer = 5f;
                //_carCollider.enabled = true;
            }
            else
            {
                _isPoliceCar = false;
                //_carCollider.enabled = false;
            }
        }

        private void ChangePolice()
        {
            if (!_isPoliceCar)
                return;

            if (GameManager.Instance.IsRunner)
            {
                _policeLevels[0].SetActive(false);
                _policeLevels[1].SetActive(true);
                _carCollider.enabled = true;
            }
            else
            {
                _policeLevels[0].SetActive(true);
                _policeLevels[1].SetActive(false);
                _carCollider.enabled = false;

            }

        }

        private void ShakePoliceCar()
        {
            //transform.DOShakeRotation(1, 1);
            //transform.doh
        }

        private void GetPolice()
        {
            foreach (var policeLevel in _policeLevels)
            {
                policeLevel.SetActive(false);
            }

            if (GameManager.Instance.IsRunner)
            {
                _policeLevels[UpgradeManager.Instance.PoliceLevelNo].SetActive(true);
                
                if (UpgradeManager.Instance.PoliceLevelNo == 1) // Araba olduysak
                {
                    _isPoliceCar = true;
                    GameManager.Instance.DistanceToPlayer = 5f;
                    _carCollider.enabled = true;
                }
                else
                {
                    _isPoliceCar = false;
                    _carCollider.enabled = false;
                }
            }

            else
            {
                _policeLevels[0].SetActive(true);
                
                if (UpgradeManager.Instance.PoliceLevelNo == 1) // Araba olduysak
                {
                    _isPoliceCar = true;
                    GameManager.Instance.DistanceToPlayer = 5f;
                }
                else
                {
                    _isPoliceCar = false;
                }
            }


        }
    }
}
