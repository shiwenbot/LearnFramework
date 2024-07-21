using UnityEngine;

namespace ShootGame
{
    public class PlayerUIComponent : IEcsComponent
    {
        public SpriteRenderer spriteRenderer;
        Color color;       

        public PlayerUIComponent(GameObject player)
        {
            spriteRenderer = player.GetComponent<SpriteRenderer>();
            color = spriteRenderer.color;
        }

        /// <summary>
        /// 用颜色变暗模拟流血效果
        /// </summary>
        private void bleed()
        {
            color.r *= 0.8f;
            color.g *= 0.8f;
            color.b *= 0.8f;

            spriteRenderer.color = color;
        }

        public void Clear()
        {

        }
    }
}
