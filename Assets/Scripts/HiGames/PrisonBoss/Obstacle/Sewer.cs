using DG.Tweening;
using HiGames.PrisonBoss.Prisoner;
using UnityEngine;

namespace HiGames.PrisonBoss.Obstacle
{
    public class Sewer : ObstacleBase
    {
        protected override void AbductPrisoner(PrisonerController prisoner)
        {
            base.AbductPrisoner(prisoner);

            prisoner.IsEscaped = true;
            prisoner.GetComponent<Collider>().enabled = false;
            prisoner.AnimateFall();
            prisoner.transform.DOJump(_finishPoint.position, 5f, 1, _exitTime).OnComplete((() => Destroy(prisoner.gameObject)));
        }
    }
}
