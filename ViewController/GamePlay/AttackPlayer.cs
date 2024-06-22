using QFramework;
using UnityEngine;

namespace ShootGame
{
    public class AttackPlayer : MonoBehaviour, IController
    {
        public IArchitecture GetArchitecture()
        {
            return ShootingEditor.Interface;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                this.SendCommand<HurtPlayerCommand>();
            }
        }
    }
}