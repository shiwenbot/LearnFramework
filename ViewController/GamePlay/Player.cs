using QFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShootGame
{
    public class Player : Entity, IController
    {
        private Rigidbody2D mRigidbody2D;
        private bool mJump = false; //角色跳跃
        private CollisionCheck mCollisionCheck;
        private Gun gun;

        //ECS
        protected List<FsmState<Player>> stateList;
        private IFsm<Player> fsm;

        //tmp
        public GameObject healthBar;
        public GameObject buffStack;

        private void Awake()
        {           
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mCollisionCheck = transform.Find("GroundCheck").GetComponent<CollisionCheck>();
            gun = transform.Find("Gun").GetComponent<Gun>();
        }

        private void Start()
        {
            PrintBug();
            healthBar.SetActive(true);
            buffStack.SetActive(true);
            stateList = new List<FsmState<Player>>() { new IdleState(), new MoveState() };
            fsm = FsmManager.Instance.CreateFsm<Player>("Player", this, stateList);
            fsm.Start<IdleState>();

            //获取左上角血条，buff状态栏和角色颜色
            Image hp = GameObject.Find("Hp").GetComponent<Image>();
            GameObject buffUIObject = GameObject.Find("BuffStack");
            SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
            
            HealthBarComponent healthBarComponent = ReferencePool.Acquire<HealthBarComponent>();
            healthBarComponent.Initialize(hp, buffUIObject, spriteRenderer);
            List<IEcsComponent> ecsComponents = new List<IEcsComponent> { new BuffComponent(), healthBarComponent };
            //通知EcsComponentManager，把component添加到array或者创建新的array
            EventManager.Instance.SendEvent<Entity, List<IEcsComponent>>("IntializeEntity", this, ecsComponents);
            EventManager.Instance.SendEvent<Entity>("AddEntity", this); //通知BleedBuffSystem这个对象会有流血效果
        }

        public void PrintBug()
        {
            Debug.Log("我是bug");
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.SendCommand(new ApplyBleedBuffCommand(this));
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

            mRigidbody2D.velocity = new Vector2(move * 50, mRigidbody2D.velocity.y);

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
