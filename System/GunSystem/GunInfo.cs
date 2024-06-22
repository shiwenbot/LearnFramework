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

    // �����Ĺ�����д����IGunSystem��
    public class GunInfo
    {
        public BindableProperty<int> BulletLeft;
        public BindableProperty<string> GunName;
        public BindableProperty<GunState> GunState;
        public BindableProperty<int> BulletOutGun; //ǹ���ӵ�����

        public void AddBullet()
        {
            BulletLeft.Value += Random.Range(1, 4);
        }
    }
}
