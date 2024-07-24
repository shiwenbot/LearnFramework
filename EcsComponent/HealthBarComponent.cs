using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShootGame
{
    public class HealthBarComponent : IEcsComponent
    {
        private Image hpImg;
        private GameObject m_buffUIObject;
        private SpriteRenderer m_spriteRenderer;

        public float maxHp = 100f;
        public float curHp;
        
        public HealthBarComponent()
        {          
            curHp = maxHp;
            EventManager.Instance.RegisterEvent<int>("OnBleedBuffExit", OnBleedBuffExit);
            EventManager.Instance.RegisterEvent<Color>("OnNoxianMightChanged", ChangePlayerColor);
        }

        private void ChangePlayerColor(Color color)
        {
            m_spriteRenderer.color = color;
        }

        private void OnBleedBuffExit(int stack)
        {
            m_buffUIObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = stack.ToString();
        }

        public void UpdateHealthBar(float damageReceived, int bleedBuffStack)
        {
            curHp = Mathf.Max(curHp - damageReceived, 0);
            hpImg.fillAmount = curHp / maxHp;

            m_buffUIObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = bleedBuffStack.ToString();
        }

        internal void Initialize(Image hp, GameObject buffUIObject, SpriteRenderer spriteRenderer)
        {
            hpImg = hp;
            m_buffUIObject = buffUIObject;
            m_spriteRenderer = spriteRenderer;
        }

        public void Clear()
        {

        }
    }
}
