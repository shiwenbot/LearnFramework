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
            GunName = new BindableProperty<string>("手枪"),
            GunState = new BindableProperty<GunState>(GunState.Idle),
            BulletOutGun = new BindableProperty<int>(10) //子弹库存
        };
        
        public void PickGun(string gunName, int BulletLeft, int BulletOutGun)
        {
            //如果捡到的和当前使用的是同一把枪，则只增加子弹数量
            if (CurrentGun.GunName.Value == gunName)
            {
                CurrentGun.BulletOutGun.Value += BulletOutGun;
                CurrentGun.BulletLeft.Value += BulletLeft;
            }
            //或者说捡到的枪的类型在库存中已经存在，则只增加子弹数量
            else if (mGunInfos.Any(info => info.GunName.Value == gunName))
            {
                var gunInfo = mGunInfos.First(info => info.GunName.Value == gunName);
                gunInfo.BulletOutGun.Value += BulletOutGun;
                gunInfo.BulletLeft.Value += BulletLeft;
            }
            //否则先缓存当前使用的枪，再切换到新的枪
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

                //更新当前使用的枪
                CurrentGun.GunName.Value = gunName;
                CurrentGun.BulletLeft.Value = BulletLeft;
                CurrentGun.BulletOutGun.Value = BulletOutGun;
                CurrentGun.GunState.Value = GunState.Idle;

                //通知view
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

                // 复制当前的枪械信息
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

                // 缓存
                mGunInfos.Enqueue(currentGunInfo);

                // 新枪设置为当前枪
                CurrentGun.GunName.Value = nextGunInfo.GunName.Value;
                CurrentGun.BulletLeft.Value = nextGunInfo.BulletLeft.Value;
                CurrentGun.BulletOutGun.Value = nextGunInfo.BulletOutGun.Value;
                CurrentGun.GunState.Value = GunState.Idle;

                // 发送换枪事件
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