using System;
using System.Collections.Generic;
using DG.Tweening;
using HiGames.PrisonBoss.Game_Manager;
using HiGames.PrisonBoss.Prisoner;
using UnityEngine;

namespace HiGames.PrisonBoss
{
    public class CollectingPrisoner : MonoBehaviour
    {
        [Header("DISTANCE")]
        [SerializeField] private float _prisonerDistance;
        [SerializeField] private float _distanceToPlayer;

        [Header("COLLECT SPEED")]
        [SerializeField] private float _collectSpeed;
        [SerializeField] private float _refreshSpeed;

        public int PrisonerCount
        {
            get => GameManager.Instance.Prisoners.Count;
        }

        private void Start()
        {
        
        }

        private void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Prisoner"))
            {
                //CollectPrisoner(other.gameObject);
            }
        }

        private void CollectPrisoner(GameObject prisoner)
        {
            /*
            prisoner.transform.parent = transform;

            float zPos;

            if (PrisonerCount == 0)
            {
                zPos = _prisonerDistance;
            }
            else
            {
                zPos = GameManager.Instance.Prisoners[PrisonerCount - 1].transform.localPosition.z + _prisonerDistance;
            }
            
            var newQueuePos =  new Vector3(0, 0, zPos);

            AnimateCollecting(prisoner, newQueuePos, _collectSpeed);

            GameManager.Instance.Prisoners.Add(prisoner.GetComponent<PrisonerController>());
            */

            var prisonerPos = prisoner.transform.position;
            prisonerPos.z = transform.position.z + (PrisonerCount - 1) * _prisonerDistance * _distanceToPlayer;
        }

        private void AnimateCollecting(GameObject prisoner, Vector3 toPos, float speed)
        {
            prisoner.transform.DOLocalMove(toPos, speed).SetEase(Ease.InSine)
                .OnComplete(() => prisoner.transform.localPosition = toPos);
        }
        
        public void RefreshQueue()
        {
            List<PrisonerController> newQueue = new List<PrisonerController>();

            float zPos;
            
            for (int i = 0; i < PrisonerCount; i++)
            {
                if(i == 0)
                    zPos = _prisonerDistance;
                else
                    zPos = GameManager.Instance.Prisoners[PrisonerCount - 1].transform.localPosition.z + _prisonerDistance;
                
                var newQueuePos =  new Vector3(0, 0, zPos);
                
                AnimateCollecting(GameManager.Instance.Prisoners[i].gameObject, newQueuePos, _refreshSpeed);
                
                newQueue.Add(GameManager.Instance.Prisoners[i]);
                
                    
            }

            GameManager.Instance.Prisoners = newQueue;


        }
    }
}
