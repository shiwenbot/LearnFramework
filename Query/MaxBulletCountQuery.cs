using QFramework;

namespace ShootGame
{
    public class MaxBulletCountQuery : IBelongToArchitecture, ICanGetModel
    {
        private readonly string mGunName;
        public MaxBulletCountQuery(string gunName)
        {
            mGunName = gunName;
        }
        public int Do()
        {
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            var gunConfigItem = gunConfigModel.GetItemByName(mGunName);
            return gunConfigItem.BulletMaxCount;
        }
        public IArchitecture GetArchitecture()
        {
            return ShootingEditor.Interface;
        }
    }
}