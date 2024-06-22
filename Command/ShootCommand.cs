using QFramework;

namespace ShootGame
{
    public class ShootCommand : AbstractCommand
    {
        // �������static����ֹÿ�ο�ǹ��newһ��Command����
        public static readonly ShootCommand Instance = new ShootCommand();

        //��ǹ���ӵ���������
        protected override void OnExecute()
        {
            var GunSystem = this.GetSystem<IGunSystem>();
            GunSystem.CurrentGun.BulletLeft.Value--; //�ӵ���������
            GunSystem.CurrentGun.GunState.Value = GunState.Shooting; //ǹ֧״̬��Ϊ�����
            var gunConfigItem = this.GetModel<IGunConfigModel>()
                .GetItemByName(GunSystem.CurrentGun.GunName.Value);

            var timeSystem = this.GetSystem<ITimeSystem>();

            //�����һ��ʱ���ǹ֧״̬��ΪIdle
            timeSystem.AddDelayTask(1 / gunConfigItem.Frequency, () => 
            { 
                GunSystem.CurrentGun.GunState.Value = GunState.Idle;
                //����ӵ�����Ϊ0���ҵ����л����ӵ������Զ�װ��
                if (GunSystem.CurrentGun.BulletLeft.Value == 0 &&
                        GunSystem.CurrentGun.BulletOutGun.Value > 0)
                {
                    this.SendCommand<ReloadCommand>();
                }
            });
        }
    }
}