using DG.Tweening;
using HiGames.PrisonBoss.Event;
using HiGames.PrisonBoss.Game_Manager;
using HiGames.PrisonBoss.Player;
using HiGames.PrisonBoss.UI;
using UnityEngine;

namespace HiGames.PrisonBoss.Prisoner
{
    public class PrisonerController : MonoBehaviour
    {
        private bool _inQueue;
        private bool _inPrison;
        private bool _isEscaped;
        public bool IsEscaped
        {
            get => _isEscaped;
            set => _isEscaped = value;
        }
        private int _prisonerIndex;
        
        [Header("ANIMATOR")] 
        [SerializeField] private Animator _anim;

        public int PrisonerIndex
        {
            get => _prisonerIndex;
            set => _prisonerIndex = value;
        }
        
        private void Update()
        {
            if (_inQueue && GameManager.Instance.IsRunner && !_inPrison && !_isEscaped)
            {
                _anim.SetFloat("Velocity", 0.6f);
                FollowPlayer();
            }

            if (_inQueue && _inPrison)
            {
                // yerleştirme işlemi
                _anim.SetFloat("Velocity", 0f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && GameManager.Instance.IsRunner && !_inQueue)
            {
                _inQueue = true;
                
                GameManager.Instance.Prisoners.Add(this);
                UIManager.Instance.UpdatePrisonerCountText();
                
                _prisonerIndex = GameManager.Instance.Prisoners.Count - 1;
                
                gameObject.tag = "Player";
            }
            
            if (other.gameObject.CompareTag("Obstacle"))
            {
                //_isEscaped = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Gate"))
            {
                _inPrison = true;

                PutInPrison();
            }
        }

        private void FollowPlayer()
        {
            var prisonerPos = transform.position;
            prisonerPos.z = PlayerManager.Instance.transform.position.z + _prisonerIndex * GameManager.Instance.PrisonerDistance + GameManager.Instance.DistanceToPlayer;

            //transform.position = Vector3.Lerp(transform.position, prisonerPos, GameManager.Instance.CollectSpeed);
            //prisonerPos.x = PlayerManager.Instance.transform.position.x;
            
            //transform.position = Vector3.Lerp(transform.position, prisonerPos, GameManager.Instance.CurlingSmoothness * _prisonerIndex);
            
            transform.DOMoveZ(prisonerPos.z, GameManager.Instance.CollectSpeed);
            transform.DOMoveX(PlayerManager.Instance.transform.position.x, GameManager.Instance.CurlingSmoothness * _prisonerIndex);
        }

        public void AnimateClimb()
        {
            _anim.SetFloat("Velocity", 0f);
            _anim.SetBool("IsClimbing", true);
        }
        
        public void AnimateFall()
        {
            _anim.SetFloat("Velocity", 0f);
            _anim.SetBool("IsFalling", true);
        }

        private void PutInPrison()
        {
            GameManager.Instance.Prisoners.Remove(this);
            UIManager.Instance.UpdatePrisonerCountText();
            GetComponent<CapsuleCollider>().enabled = false;

            if (GameManager.Instance.PrisonersInPrison.Count < GameManager.Instance.CurrentPrison.PrisonerLocs.Count)
            {
                EventManager.Instance.CashCollected(10, transform.position);
                GameManager.Instance.PrisonersInPrison.Add(this);
                
                FindPrisonerLocation();
            }
            else
            {
                // kapasite dolu ise
                Vector3 currentPos = transform.position;
                transform.DOMove(new Vector3(currentPos.x -20, currentPos.y, currentPos.z - 20), 2f);
            }
                
        }

        private void FindPrisonerLocation()
        {
            foreach (var loc in GameManager.Instance.CurrentPrison.PrisonerLocs)
            {
                if (!loc.IsFull)
                {
                    transform.DOMove(loc.transform.position, 2f);
                    loc.IsFull = true;
                    break;
                }
                
            }
        }
        
        
    }
}
