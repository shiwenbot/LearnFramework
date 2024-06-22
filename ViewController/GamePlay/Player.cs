using QFramework;
using UnityEngine;

namespace ShootGame{
    public class Player : MonoBehaviour, IController
    {
        private Rigidbody2D mRigidbody2D;
        private bool mJump = false; //��ɫ��Ծ
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
            //��ɫ��Ծ
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mJump = true;
            }
            //��J����ǹ
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
            //��ɫ�����ƶ�
            float move = Input.GetAxis("Horizontal");
            //ת��
            if (move * transform.localScale.x < 0)
            {
                var localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }          

            mRigidbody2D.velocity = new Vector2(move * 10, mRigidbody2D.velocity.y);

            //��Ծ
            var grounded = mCollisionCheck.IsTriggered; //ֻ���ڵ����ʱ��ſ�����Ծ
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
