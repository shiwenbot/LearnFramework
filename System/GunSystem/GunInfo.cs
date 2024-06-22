using QFramework;
using UnityEngine;


namespace ShootGame
{
    public enum GunState
    {
        Idle,
        Shooting,
        Reload,
        Empty,
        CoolDown
    }

    // 这个类的构造器写在了IGunSystem中
    public class GunInfo
    {
        public BindableProperty<int> BulletLeft;
        public BindableProperty<string> GunName;
        public BindableProperty<GunState> GunState;
        public BindableProperty<int> BulletOutGun; //枪外子弹数量

        public void AddBullet()
        {
            BulletLeft.Value += Random.Range(1, 4);
        }
    }
}
