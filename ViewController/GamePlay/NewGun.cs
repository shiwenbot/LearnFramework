using QFramework;
using UnityEngine;

namespace ShootGame
{
    public class NewGun : MonoBehaviour, IController
    {
        public string Name;
        public int BulletLeft;
        public int BulletOutGun;

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor.Interface;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                this.SendCommand(new PickGunCommand(Name, BulletLeft, BulletOutGun));
            }
        }
    }
}
