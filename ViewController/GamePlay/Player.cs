using QFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShootGame
{
    public class Player : Entity, IController
    {
        private Rigidbody2D mRigidbody2D;
        private bool mJump = false; //��ɫ��Ծ
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

            //��ȡ���Ͻ�Ѫ����buff״̬���ͽ�ɫ��ɫ
            Image hp = GameObject.Find("Hp").GetComponent<Image>();
            GameObject buffUIObject = GameObject.Find("BuffStack");
            SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
            
            HealthBarComponent healthBarComponent = ReferencePool.Acquire<HealthBarComponent>();
            healthBarComponent.Initialize(hp, buffUIObject, spriteRenderer);
            List<IEcsComponent> ecsComponents = new List<IEcsComponent> { new BuffComponent(), healthBarComponent };
            //֪ͨEcsComponentManager����component��ӵ�array���ߴ����µ�array
            EventManager.Instance.SendEvent<Entity, List<IEcsComponent>>("IntializeEntity", this, ecsComponents);
            EventManager.Instance.SendEvent<Entity>("AddEntity", this); //֪ͨBleedBuffSystem������������ѪЧ��
        }

        public void PrintBug()
        {
            Debug.Log("����bug");
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.SendCommand(new ApplyBleedBuffCommand(this));
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

            mRigidbody2D.velocity = new Vector2(move * 50, mRigidbody2D.velocity.y);

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
