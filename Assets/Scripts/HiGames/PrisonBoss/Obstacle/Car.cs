using System.Collections.Generic;
using DG.Tweening;
using HiGames.PrisonBoss.Prisoner;
using UnityEngine;

namespace HiGames.PrisonBoss.Obstacle
{
    public class Car : ObstacleBase
    {
        [Header("SEAT LOCATION")] 
        [SerializeField] private List<Transform> _seatLocs = new List<Transform>();
        
        [Header("ANIMATOR")] 
        [SerializeField] private Animator _anim;
        
        
        private int _currentPrisonerCount;

        protected override void AbductPrisoner(PrisonerController prisoner)
        {
            base.AbductPrisoner(prisoner);
            
            if (_currentPrisonerCount < _seatLocs.Count)
            {
                prisoner.IsEscaped = true;
                prisoner.gameObject.transform.position = _seatLocs[_currentPrisonerCount].position;
                prisoner.transform.parent = transform;
                
                _currentPrisonerCount++;
            }
            else // Drive Car
            {
                //transform.DOMove(_finishPoint.position, _exitTime);
            }
            
        }
    }
    
    
    
}
