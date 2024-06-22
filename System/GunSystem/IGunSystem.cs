using QFramework;
using System.Collections.Generic;
using System.Linq;

namespace ShootGame
{
    public interface IGunSystem : ISystem
    {
        GunInfo CurrentGun { get; }
        void PickGun(string gunName, int BulletLeft, int BulletOutGun);
        void SwitchGun();
    }

    public class GunSystem : AbstractSystem, IGunSystem
    {
        private Queue<GunInfo> mGunInfos = new Queue<GunInfo>();
        public GunInfo CurrentGun { get; } = new GunInfo()
        {
            BulletLeft = new BindableProperty<int>(3),
            GunName = new BindableProperty<string>("��ǹ"),
            GunState = new BindableProperty<GunState>(GunState.Idle),
            BulletOutGun = new BindableProperty<int>(10) //�ӵ����
        };
        
        public void PickGun(string gunName, int BulletLeft, int BulletOutGun)
        {
            //����񵽵ĺ͵�ǰʹ�õ���ͬһ��ǹ����ֻ�����ӵ�����
            if (CurrentGun.GunName.Value == gunName)
            {
                CurrentGun.BulletOutGun.Value += BulletOutGun;
                CurrentGun.BulletLeft.Value += BulletLeft;
            }
            //����˵�񵽵�ǹ�������ڿ�����Ѿ����ڣ���ֻ�����ӵ�����
            else if (mGunInfos.Any(info => info.GunName.Value == gunName))
            {
                var gunInfo = mGunInfos.First(info => info.GunName.Value == gunName);
                gunInfo.BulletOutGun.Value += BulletOutGun;
                gunInfo.BulletLeft.Value += BulletLeft;
            }
            //�����Ȼ��浱ǰʹ�õ�ǹ�����л����µ�ǹ
            else
            {
                var prevGun = new GunInfo()
                {
                    GunName = new BindableProperty<string>()
                    {
                        Value = CurrentGun.GunName.Value
                    },
                    BulletLeft = new BindableProperty<int>()
                    {
                        Value = CurrentGun.BulletLeft.Value
                    },
                    BulletOutGun = new BindableProperty<int>()
                    {
                        Value = CurrentGun.BulletOutGun.Value
                    },
                    GunState = new BindableProperty<GunState>()
                    {
                        Value = CurrentGun.GunState.Value
                    }
                };
                mGunInfos.Enqueue(prevGun);

                //���µ�ǰʹ�õ�ǹ
                CurrentGun.GunName.Value = gunName;
                CurrentGun.BulletLeft.Value = BulletLeft;
                CurrentGun.BulletOutGun.Value = BulletOutGun;
                CurrentGun.GunState.Value = GunState.Idle;

                //֪ͨview
                this.SendEvent(new OnCurrentGunChanged()
                {
                    GunName = gunName
                });
            }

        }

        public void SwitchGun()
        {
            if (mGunInfos.Count > 0)
            {
                var nextGunInfo = mGunInfos.Dequeue();

                // ���Ƶ�ǰ��ǹе��Ϣ
                var currentGunInfo = new GunInfo
                {
                    GunName = new BindableProperty<string>()
                    {
                        Value = CurrentGun.GunName.Value
                    },
                    BulletLeft = new BindableProperty<int>()
                    {
                        Value = CurrentGun.BulletLeft.Value
                    },
                    BulletOutGun = new BindableProperty<int>()
                    {
                        Value = CurrentGun.BulletOutGun.Value
                    },
                    GunState = new BindableProperty<GunState>()
                    {
                        Value = CurrentGun.GunState.Value
                    }
                };

                // ����
                mGunInfos.Enqueue(currentGunInfo);

                // ��ǹ����Ϊ��ǰǹ
                CurrentGun.GunName.Value = nextGunInfo.GunName.Value;
                CurrentGun.BulletLeft.Value = nextGunInfo.BulletLeft.Value;
                CurrentGun.BulletOutGun.Value = nextGunInfo.BulletOutGun.Value;
                CurrentGun.GunState.Value = GunState.Idle;

                // ���ͻ�ǹ�¼�
                this.SendEvent(new OnCurrentGunChanged()
                {
                    GunName = nextGunInfo.GunName.Value
                });
            }
        }

        protected override void OnInit()
        {
            
        }
    }

    public class OnCurrentGunChanged
    {
        public string GunName { get; set; }
    }
}