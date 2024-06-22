using UnityEngine;

namespace ShootGame
{
    public class CollisionCheck : MonoBehaviour
    {
        public LayerMask TargetLayer;
        public int EnterCount;

        public bool IsTriggered { get { return EnterCount > 0; } }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsTargetLayer(collision.gameObject.layer))
            {
                EnterCount++;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsTargetLayer(collision.gameObject.layer))
            {
                EnterCount--;
            }
        }
        //判断是否为目标层，比如判断是否在地面时目标层为Ground
        private bool IsTargetLayer(int layer)
        {
            return (TargetLayer.value & 1 << layer) == 1 << layer;
        }
    }
}

