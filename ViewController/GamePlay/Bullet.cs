using QFramework;
using UnityEngine;

namespace ShootGame
{
    /// <summary>
    /// 每当Gun的shoot方法被调用的时候，就会实例化一个Bullet对象
    /// </summary>
    public class Bullet : MonoBehaviour, IController, IReference
    {
        private Rigidbody2D mRigidbody2D;  

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            //子弹发射
            mRigidbody2D.velocity = Vector2.right * 10 * Mathf.Sign(transform.localScale.x);
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            //击中敌人
            if (other.gameObject.CompareTag("Enemy"))
            {
                this.SendCommand<KillEnemyCommand>();
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor.Interface;
        }
        
        public void Release()
        {
            this.GetSystem<ReferencePoolSystem>().Release(this);
        }
        public void Clear()
        {
            mRigidbody2D = null;           
        }
    }
}
