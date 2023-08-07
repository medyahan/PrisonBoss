using System.Collections.Generic;
using DG.Tweening;
using HiGames.PrisonBoss.Game_Manager;
using HiGames.PrisonBoss.Prisoner;
using HiGames.PrisonBoss.UI;
using UnityEngine;

namespace HiGames.PrisonBoss.Obstacle
{
    public class ObstacleBase : MonoBehaviour
    {
        [Header("FINISH LOCATION")] 
        [SerializeField] protected Transform _finishPoint;
        
        [Header("TIME")] 
        [SerializeField] protected float _exitTime;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PrisonerController>() != null)
            {
                AbductPrisoner(other.gameObject.GetComponent<PrisonerController>());
            }
        }
        
        protected virtual void AbductPrisoner(PrisonerController prisoner)
        {
            GameManager.Instance.Prisoners.Remove(prisoner);
            UIManager.Instance.UpdatePrisonerCountText();
            GameManager.Instance.RefreshPrisonerIndex();
            
        }

    }
}
