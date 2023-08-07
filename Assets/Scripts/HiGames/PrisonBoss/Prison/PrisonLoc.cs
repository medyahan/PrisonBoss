using UnityEngine;

namespace HiGames.PrisonBoss.Prison
{
    public class PrisonLoc : MonoBehaviour
    {
        private bool _isFull;
        public bool IsFull
        {
            get => _isFull;
            set => _isFull = value;
        }
    }
}
