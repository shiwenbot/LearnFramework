using QFramework;
using System.Collections.Generic;

namespace ShootGame
{
    public interface IGunConfigModel : IModel
    {
        GunConfigItem GetItemByName(string name);
    }

    public class GunConfigItem
    {
        public GunConfigItem(string name, int bulletMaxCount, float attack, float frequency, float shootDistance,
            bool needBullet, float reloadSeconds, string description)
        {
            Name = name;
            BulletMaxCount = bulletMaxCount;
            Attack = attack;
            Frequency = frequency;
            ShootDistance = shootDistance;
            NeedBullet = needBullet;
            ReloadSeconds = reloadSeconds;
            Description = description;
        }
        public string Name { get; set; }
        public int BulletMaxCount { get; set; }
        public float Attack { get; set; }
        public float Frequency { get; set; }
        public float ShootDistance { get; set; }
        public bool NeedBullet { get; set; }
        public float ReloadSeconds { get; set; }
        public string Description { get; set; }
    }

    public class GunConfigModel : AbstractModel, IGunConfigModel
    {
        private Dictionary<string, GunConfigItem> mItems = new Dictionary<string, GunConfigItem>()
        {
            { "��ǹ", new GunConfigItem("��ǹ", 7, 1, 1, 0.5f, false, 3, "Ĭ��ǿ") },
            { "���ǹ", new GunConfigItem("���ǹ", 30, 1, 6, 0.34f, true, 3, "��") },
            { "��ǹ", new GunConfigItem("��ǹ", 50, 3, 3, 1f, true, 1, "��һ��������") },
            { "�ѻ�ǹ", new GunConfigItem("�ѻ�ǹ", 12, 6, 1, 1f, true, 5, "������׼+��������") },
            { "���Ͳ", new GunConfigItem("���Ͳ", 1, 5, 1, 1f, true, 4, "����+��ը") },
            { "����ǹ", new GunConfigItem("����ǹ", 1, 1, 1, 0.5f, true, 1, "һ�η��� 6 ~ 12 ���ӵ�") },
        };
        protected override void OnInit()
        {
            
        }
        public GunConfigItem GetItemByName(string name)
        {
            return mItems[name];
        }
    }
}
