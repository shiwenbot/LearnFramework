using QFramework;
using UnityEngine;

namespace ShootGame{
    public class Player : MonoBehaviour, IController
    {
        private Rigidbody2D mRigidbody2D;
        private bool mJump = false; //角色跳跃
        private CollisionCheck mCollisionCheck;
        private Gun gun;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mCollisionCheck = transform.Find("GroundCheck").GetComponent<CollisionCheck>();
            gun = transform.Find("Gun").GetComponent<Gun>();
        }

        private void Update()
        {
            //角色跳跃
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mJump = true;
            }
            //按J键开枪
            if (Input.GetKeyDown(KeyCode.J))
            {
                gun.Shoot();
            }
            if (Input.GetKeyDown(KeyCode.R)) 
            {
                gun.Reload();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                this.SendCommand<SwitchGunCommand>();
            }
        }

        private void FixedUpdate()
        {
            //角色左右移动
            float move = Input.GetAxis("Horizontal");
            //转向
            if (move * transform.localScale.x < 0)
            {
                var localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }          

            mRigidbody2D.velocity = new Vector2(move * 10, mRigidbody2D.velocity.y);

            //跳跃
            var grounded = mCollisionCheck.IsTriggered; //只有在地面的时候才可以跳跃
            if (mJump && grounded)
            {
                mRigidbody2D.AddForce(new Vector2(0, 300));
                mJump = false;
            }
        }

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor.Interface;
        }
    }
}
