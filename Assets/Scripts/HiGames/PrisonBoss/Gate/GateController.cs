using System;
using DG.Tweening;
using HiGames.PrisonBoss.Event;
using HiGames.PrisonBoss.Game_Manager;
using HiGames.PrisonBoss.Player;
using UnityEngine;

namespace HiGames.PrisonBoss.Gate
{
    public class GateController : MonoBehaviour
    {
        private bool _controlled;
        [SerializeField] private Animator _anim;
        [SerializeField] private bool _isPrisonEntrance;


        private void Start()
        {
            if(_isPrisonEntrance)
                ControlDoor();
        }

        private void Update()
        {
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Control") && !_controlled)
            {
                _controlled = true;
                ChangeMovement();
                
                if(_isPrisonEntrance)
                    ControlDoor();
                
                EventManager.Instance.MovementChanged();
            }
        }
        
        private void ChangeMovement()
        {
            if (GameManager.Instance.IsRunner)
            {
                GameManager.Instance.IsRunner = false;
            }
            else
            {
                ResetPlayer();
                GameManager.Instance.IsRunner = true;
            }
                
        }

        private void ResetPlayer()
        {
            //PlayerManager.Instance.transform.rotation = quaternion.identity;
            PlayerManager.Instance.transform.DORotateQuaternion(Quaternion.identity, 1f);
            GameManager.Instance.Prisoners.Clear();
        }


        private void ControlDoor()
        {
            if (GameManager.Instance.IsRunner)
            {
                // kapıyı aç
                _anim.SetBool("IsOpen", true);
            }
            else
            {
                 // kapıyı kapa
                 _anim.SetBool("IsClose", true);
            }
        }
    }
}
