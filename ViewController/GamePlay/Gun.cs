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
            mBullet = transform.Find("Bullet").GetComponent<Bullet>();//这里最终获取到的是Bullet脚本
            mGunInfo = this.GetSystem<IGunSystem>().CurrentGun;
            mMaxBulletCount = new MaxBulletCountQuery(this.GetSystem<IGunSystem>().CurrentGun.GunName.Value).Do();
        }

        public void Shoot()
        {
            // 子弹数量大于0 且 枪的状态是Idle
            if (mGunInfo.BulletLeft.Value > 0 && mGunInfo.GunState.Value == GunState.Idle)
            {
                //这里要从池子中获取一个子弹对象
                var bullet = this.GetSystem<ReferencePoolSystem>().Acquire<Bullet>();
                if(bullet == null)
                {
                    bullet = Instantiate(mBullet.transform, mBullet.transform.position, mBullet.transform.rotation).GetComponent<Bullet>();
                }
                bullet.transform.position = mBullet.transform.position;
                bullet.transform.localScale = mBullet.transform.lossyScale;
                bullet.gameObject.SetActive(true);

                //一秒后销毁子弹
                bullet.Invoke("Release", 1.0f);

                // 更新子弹数量
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
