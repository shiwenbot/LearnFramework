using QFramework;
using System;
using UnityEngine;

namespace ShootGame
{
    public class UIController : MonoBehaviour, IController
    {
        private IPlayerModel playerModel;
        private IStateSystem stateSystem;
        private IGunSystem gunSystem;
        private int mMaxBulletCount;

        private void Awake()
        {
            playerModel = this.GetModel<IPlayerModel>();
            stateSystem = this.GetSystem<IStateSystem>();
            gunSystem = this.GetSystem<IGunSystem>();

            mMaxBulletCount = new MaxBulletCountQuery(gunSystem.CurrentGun.GunName.Value).Do(); //��ѯ�����������

            this.RegisterEvent<OnCurrentGunChanged>(e =>
            {
                mMaxBulletCount = new MaxBulletCountQuery(gunSystem.CurrentGun.GunName.Value).Do(); //��ѯ�����������
            }).UnRegisterWhenGameObjectDestroyed(gameObject);           
        }

        private void OnGUI()
        {
            //ǹе���
            GUI.Label(new Rect(10, 10, 300, 100), "HP: " + playerModel.HP.Value + "/3", mLabelStyle.Value);
            GUI.Label(new Rect(10, 60, 300, 100), "Bullet left: " + gunSystem.CurrentGun.BulletLeft.Value + "/" + mMaxBulletCount, mLabelStyle.Value);           
            GUI.Label(new Rect(10, 110, 300, 100), "Bullet in Screen: " + gunSystem.CurrentGun.BulletOutGun.Value, mLabelStyle.Value);
            GUI.Label(new Rect(10, 160, 300, 100), "Gun State: " + gunSystem.CurrentGun.GunState.Value, mLabelStyle.Value);

            GUI.Label(new Rect(Screen.width - 300 - 10, 10, 300, 100), "Kill count: " + stateSystem.killCount.Value, mLabelStyle.Value);
        }

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor.Interface;
        }

        /// <summary>
        /// �Զ��������С
        /// �����أ�mLabelStyle.Value ��һ�ε��õ�ʱ��Ż�ִ�к�ߴ���ȥ��ί��
        /// </summary>
        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 20
        });

        // �ͷ���Դ
        private void OnDestroy()
        {
            playerModel = null;
            stateSystem = null;
            gunSystem = null;
        }
    }
}