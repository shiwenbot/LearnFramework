using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootGame
{
    public class UIGamePass : MonoBehaviour
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
            var labelWidth = 400;
            var labelHeight = 100;
            var labelPosition = new Vector2(Screen.width - labelWidth, Screen.height - labelHeight) * 0.5f;
            var labelSize = new Vector2(labelWidth, labelHeight);
            var labelRect = new Rect(labelPosition, labelSize);
            GUI.Label(labelRect, "��Ϸͨ��", mLabelStyle.Value);

            var buttonWidth = 200;
            var buttonHeight = 100;
            var buttonPosition = new Vector2(Screen.width - buttonWidth, Screen.height - buttonHeight + 300) * 0.5f;
            var buttonSize = new Vector2(buttonWidth, buttonHeight);
            var buttonRect = new Rect(buttonPosition, buttonSize);

            if (GUI.Button(buttonRect, "�ص���ҳ", mButtonStyle.Value))
            {
                SceneManager.LoadScene("GameStart");
            }
        }
    }
}