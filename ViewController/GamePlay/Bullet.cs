using QFramework;
using UnityEngine;

namespace ShootGame
{
    /// <summary>
    /// ÿ��Gun��shoot���������õ�ʱ�򣬾ͻ�ʵ����һ��Bullet����
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
            //�ӵ�����
            mRigidbody2D.velocity = Vector2.right * 10 * Mathf.Sign(transform.localScale.x);
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            //���е���
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
