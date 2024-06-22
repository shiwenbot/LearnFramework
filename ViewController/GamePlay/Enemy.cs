using UnityEngine;

namespace ShootGame
{
    public class Enemy : MonoBehaviour
    {
        private Rigidbody2D mRigidbody2D;
        private CollisionCheck mWallCheck;
        private CollisionCheck mGroundCheck;
        private CollisionCheck mFallCheck;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mWallCheck = transform.Find("WallCheck").GetComponent<CollisionCheck>();
            mGroundCheck = transform.Find("GroundCheck").GetComponent<CollisionCheck>();
            mFallCheck = transform.Find("FallCheck").GetComponent<CollisionCheck>();
        }

        private void FixedUpdate()
        {
            var localScale = transform.localScale.x;

            // 如果检测到墙壁或者掉落，就转向
            if (mGroundCheck.IsTriggered && mFallCheck.IsTriggered && !mWallCheck.IsTriggered)
            {
                mRigidbody2D.velocity = new Vector2(localScale * 10, mRigidbody2D.velocity.y);
            }
            else
            {
                var tmp = transform.localScale;
                tmp.x *= -1;
                transform.localScale = tmp;
            }
        }
    }
}