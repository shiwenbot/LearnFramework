using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootGame
{
    public class UIGameOver : MonoBehaviour
    {
        private Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            // �����С
            fontSize = 80,
            // ����
            alignment = TextAnchor.MiddleCenter
        });

        private Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            // �����С
            fontSize = 40,
            // ����
            alignment = TextAnchor.MiddleCenter
        });


        private void OnGUI()
        {
            var labelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, Screen.height * 0.5f, 400, 100);

            GUI.Label(labelRect, "��Ϸʧ��", mLabelStyle.Value);

            var buttonRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, Screen.height * 0.5f + 150, 200, 100);

            if (GUI.Button(buttonRect, "�ص���ҳ", mButtonStyle.Value))
            {
                SceneManager.LoadScene("GameStart");
            }
        }
    }
}