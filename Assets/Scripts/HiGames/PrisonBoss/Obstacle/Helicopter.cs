using DG.Tweening;
using HiGames.PrisonBoss.Prisoner;
using UnityEngine;

namespace HiGames.PrisonBoss.Obstacle
{
    public class Helicopter : ObstacleBase
    {
        [SerializeField] private Transform _startPoint;
        
        protected override void AbductPrisoner(PrisonerController prisoner)
        {
            base.AbductPrisoner(prisoner);
            
            prisoner.IsEscaped = true;
            prisoner.gameObject.transform.position = _startPoint.position;
            
            prisoner.AnimateClimb();
            prisoner.transform.DOMove(_finishPoint.position, _exitTime).OnComplete((() => Destroy(prisoner.gameObject)));
            
        }
    }
}
