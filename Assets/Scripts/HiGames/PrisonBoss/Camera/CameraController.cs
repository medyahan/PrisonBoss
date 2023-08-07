using DG.Tweening;
using HiGames.PrisonBoss.Event;
using HiGames.PrisonBoss.Game_Manager;
using HiGames.PrisonBoss.Player;
using UnityEngine;

namespace HiGames.PrisonBoss
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _runnerSmoothness;
        [SerializeField] private float _joystickSmoothness;
        [SerializeField] private float _changeTime;
        [SerializeField] private Vector3 _runnerPos;
        [SerializeField] private Vector3 _joystickPos;
        [SerializeField] private Transform _mainCamera;

        private Vector3 _offset;
        
        private void Start()
        {
            EventManager.Instance.OnMovementChanged += ChangeCamera;

            _offset = transform.position - PlayerManager.Instance.transform.position;

        }

        private void Update()
        {
            if(GameManager.Instance.IsRunner)
            { 
                FollowForRunner();
            }
                
            else
            {
                FollowForJoystick();
            }
            
        }

        private void FollowForRunner()
        {
            Vector3 newPos = transform.position;
            newPos.z = (PlayerManager.Instance.transform.position + _offset).z;
            //newPos.x = Mathf.Lerp(newPos.x, 0, _followSmoothness * 2f * Time.deltaTime);
            //Vector3 smoothPos = transform.position;
            Vector3 smoothPos = Vector3.Lerp(transform.position, newPos, _runnerSmoothness);
            transform.position = smoothPos;
        }
        
        private void FollowForJoystick()
        {
            //Vector3 newPos = transform.position;
            //newPos.z = (PlayerManager.Instance.transform.position + _offset).z;
            //newPos.x = PlayerManager.Instance.transform.position.x;
            Vector3 smoothPos = Vector3.Lerp(transform.position, PlayerManager.Instance.transform.position + _offset, _joystickSmoothness * Time.deltaTime);
            transform.position = smoothPos;
        }

        private void ChangeCamera()
        {
            if(GameManager.Instance.IsRunner)
            {
                _mainCamera.DOLocalMove(_runnerPos, _changeTime).OnComplete((() => _mainCamera.localPosition = _runnerPos));
                transform.DOMoveX(0, _changeTime);
                //_mainCamera.localPosition = Vector3.Lerp(_mainCamera.localPosition, _runnerPos, _smoothness * Time.deltaTime);
            }
                
            else
            {
                _mainCamera.DOLocalMove(_joystickPos, _changeTime).OnComplete((() => _mainCamera.localPosition = _joystickPos));
                transform.DOMoveX((PlayerManager.Instance.transform.position + _offset).x, _changeTime);
                //_mainCamera.localPosition = Vector3.Lerp(_mainCamera.localPosition, _joystickPos, _smoothness * Time.deltaTime);
            }
        }
    }
}
