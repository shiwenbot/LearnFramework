using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootGame
{
    public class UIGameOver : MonoBehaviour
    {
        private Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            // 字体大小
            fontSize = 80,
            // 居中
            alignment = TextAnchor.MiddleCenter
        });

        private Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            // 字体大小
            fontSize = 40,
            // 居中
            alignment = TextAnchor.MiddleCenter
        });


        private void OnGUI()
        {
            var labelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, Screen.height * 0.5f, 400, 100);

            GUI.Label(labelRect, "游戏失败", mLabelStyle.Value);

            var buttonRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, Screen.height * 0.5f + 150, 200, 100);

            if (GUI.Button(buttonRect, "回到首页", mButtonStyle.Value))
            {
                SceneManager.LoadScene("GameStart");
            }
        }
    }
}