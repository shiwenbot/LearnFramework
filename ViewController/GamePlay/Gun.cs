using QFramework;
using UnityEngine;

namespace ShootGame
{
    public class Gun : MonoBehaviour, IController
    {
        private Bullet mBullet;
        private GunInfo mGunInfo;
        private int mMaxBulletCount;

        private void Awake()
        {
            mBullet = transform.Find("Bullet").GetComponent<Bullet>();//�������ջ�ȡ������Bullet�ű�
            mGunInfo = this.GetSystem<IGunSystem>().CurrentGun;
            mMaxBulletCount = new MaxBulletCountQuery(this.GetSystem<IGunSystem>().CurrentGun.GunName.Value).Do();
        }

        public void Shoot()
        {
            // �ӵ���������0 �� ǹ��״̬��Idle
            if (mGunInfo.BulletLeft.Value > 0 && mGunInfo.GunState.Value == GunState.Idle)
            {
                //����Ҫ�ӳ����л�ȡһ���ӵ�����
                var bullet = this.GetSystem<ReferencePoolSystem>().Acquire<Bullet>();
                if(bullet == null)
                {
                    bullet = Instantiate(mBullet.transform, mBullet.transform.position, mBullet.transform.rotation).GetComponent<Bullet>();
                }
                bullet.transform.position = mBullet.transform.position;
                bullet.transform.localScale = mBullet.transform.lossyScale;
                bullet.gameObject.SetActive(true);

                //һ��������ӵ�
                bullet.Invoke("Release", 1.0f);

                // �����ӵ�����
                this.SendCommand(ShootCommand.Instance);
            }               
        }

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor.Interface;
        }

        public void Reload()
        {
            if (mGunInfo.BulletLeft.Value < mMaxBulletCount &&
                mGunInfo.BulletOutGun.Value > 0 &&
                mGunInfo.GunState.Value == GunState.Idle) // +
            {
                this.SendCommand<ReloadCommand>();
            }
        }
    }
}
