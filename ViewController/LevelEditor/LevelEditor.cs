using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace ShootGame
{
    class LevelItemInfo
    {
        public float X;
        public float Y;
        public string Name;
    }
    public class LevelEditor : MonoBehaviour
    {
        /// <summary>
        /// ����ģʽ
        /// </summary>
        public enum OperateMode
        {
            Draw,
            Erase
        }

        /// <summary>
        /// ��ˢģʽ
        /// </summary>
        public enum BrushType 
        {
            Ground,
            Hero
        }

        private OperateMode mCurrentOperateMode = OperateMode.Draw;
        private BrushType mCurrentBrushType = BrushType.Ground; // +

        private readonly Lazy<GUIStyle> mModeLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 30,
            alignment = TextAnchor.MiddleCenter
        });

        private readonly Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 30,
        });

        private readonly Lazy<GUIStyle> mRightButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button) // +
        {
            fontSize = 25
        });

        private void OnGUI()
        {
            var modeLabelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, 35, 200, 50);
            // ��ʾ��ǰģʽ
            if (mCurrentOperateMode == OperateMode.Draw)
            {
                GUI.Label(modeLabelRect, mCurrentOperateMode + ":" + mCurrentBrushType, mModeLabelStyle.Value);
            }
            else
            {
                GUI.Label(modeLabelRect, mCurrentOperateMode.ToString(), mModeLabelStyle.Value);
            }

            var drawButtonRect = new Rect(10, 10, 150, 40);
            if (GUI.Button(drawButtonRect, "����", mButtonStyle.Value))
            {
                mCurrentOperateMode = OperateMode.Draw;
            }

            var eraseButtonRect = new Rect(10, 60, 150, 40);
            if (GUI.Button(eraseButtonRect, "��Ƥ", mButtonStyle.Value))
            {
                mCurrentOperateMode = OperateMode.Erase;
            }

            if (mCurrentOperateMode == OperateMode.Draw)
            {
                var groundButtonRect = new Rect(Screen.width - 110, 10, 100, 40);
                if (GUI.Button(groundButtonRect, "�ؿ�", mRightButtonStyle.Value))
                {
                    mCurrentBrushType = BrushType.Ground;
                }

                var heroButtonRect = new Rect(Screen.width - 110, 60, 100, 40);
                if (GUI.Button(heroButtonRect, "����", mRightButtonStyle.Value))
                {
                    mCurrentBrushType = BrushType.Hero;
                }
            }

            var saveButtonRect = new Rect(Screen.width - 110, Screen.height - 50, 100, 40);
            if (GUI.Button(saveButtonRect, "����", mRightButtonStyle.Value))
            {

                List<LevelItemInfo> infos = new List<LevelItemInfo>(transform.childCount);
                // �Ѽ�
                foreach (Transform child in transform)
                {
                    infos.Add(new LevelItemInfo()
                    {
                        X = child.position.x,
                        Y = child.position.y,
                        Name = child.name
                    });
                }

                XmlDocument document = new XmlDocument();

                var declaration = document.CreateXmlDeclaration("1.0", "UTF-8", "");
                document.AppendChild(declaration);

                var level = document.CreateElement("Level"); // ���ڵ�
                document.AppendChild(level);

                foreach (var levelItemInfo in infos)
                {
                    var levelItem = document.CreateElement("LevelItem");
                    levelItem.SetAttribute("name", levelItemInfo.Name);
                    levelItem.SetAttribute("x", levelItemInfo.X.ToString());
                    levelItem.SetAttribute("y", levelItemInfo.Y.ToString());
                    level.AppendChild(levelItem);
                }

                // -+
                var levelFilesFolder = Application.persistentDataPath + "/LevelFiles";
                Debug.Log(levelFilesFolder);

                if (!Directory.Exists(levelFilesFolder))
                {
                    Directory.CreateDirectory(levelFilesFolder);
                }

                var levelFilePath = levelFilesFolder + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";

                // �� xml д�뵽�ļ�·��
                document.Save(levelFilePath);
            }
        }

        public SpriteRenderer EmptyHighlight;

        /// <summary>
        /// �Ƿ�ɻ���
        /// </summary>
        private bool mCanDraw;

        /// <summary>
        /// ���ָ�������
        /// </summary>
        private GameObject mCurrentObjectMouseOn;

        private void Update()
        {
            var mousePosition = Input.mousePosition;
            var worldMousePos = Camera.main.ScreenToWorldPoint(mousePosition);

            // ���������ת��Ϊ�������꣬ʵ�ַ����Ч��
            worldMousePos.x = Mathf.Floor(worldMousePos.x + 0.5f);
            worldMousePos.y = Mathf.Floor(worldMousePos.y + 0.5f);

            worldMousePos.z = 0;

            //������ͽ��水ť�н����Ļ�������ʱ����ʾ�����飬�����ڵ����ť��ʱ�򲻻ử���߲����ؿ�
            if (GUIUtility.hotControl == 0)
            {
                EmptyHighlight.gameObject.SetActive(true);
            }
            else
            {
                EmptyHighlight.gameObject.SetActive(false);
            }

            //������λ�ú͸������λ����ͬ��ʲô������
            if (Math.Abs(EmptyHighlight.transform.position.x - worldMousePos.x) < 0.1f
                && Math.Abs(EmptyHighlight.transform.position.y - worldMousePos.y) < 0.1f) // -+
            {
                
            }
            else
            {
                // ���ø������λ��
                var emptyHighlightPos = worldMousePos;
                emptyHighlightPos.z = -1;
                EmptyHighlight.transform.position = emptyHighlightPos;

                // ��������
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);

                var hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity);

                // ����ײ˵�����еؿ飬�������ڵ�ǰλ�û����µĵؿ�
                if (hit.collider)
                {
                    if (mCurrentOperateMode == OperateMode.Draw)
                    {
                        EmptyHighlight.color = new Color(1, 0, 0, 0.5f); // ��ɫ�����ܻ���
                    }
                    else
                    {
                        EmptyHighlight.color = new Color(1, 0.5f, 0, 0.5f); // ��ɫ����ɲ���
                    }
                    mCanDraw = false;
                    mCurrentObjectMouseOn = hit.collider.gameObject;
                }
                else
                {
                    if (mCurrentOperateMode == OperateMode.Draw)
                    {
                        EmptyHighlight.color = new Color(1, 1, 1, 0.5f); // ��ɫ������Ի���
                    }
                    else
                    {
                        EmptyHighlight.color = new Color(0, 0, 1, 0.5f); // ��ɫ������Ƥ״̬
                    }

                    mCanDraw = true;
                    mCurrentObjectMouseOn = null;
                }
                
            }
            //��������϶�����ʱ����Ի���/�����ؿ�
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && GUIUtility.hotControl == 0)
            {
                if (mCanDraw && mCurrentOperateMode == OperateMode.Draw)
                {
                    if (mCurrentBrushType == BrushType.Ground) // +
                    {
                        var groundPrefab = Resources.Load<GameObject>("Ground");
                        var groundGameObj = Instantiate(groundPrefab, transform);
                        groundGameObj.transform.position = worldMousePos;
                        groundGameObj.name = "Ground";

                        mCanDraw = false;
                    }
                    else if (mCurrentBrushType == BrushType.Hero)
                    {
                        // ��ͬ���õؿ�
                        var groundPrefab = Resources.Load<GameObject>("Ground");
                        var groundGameObj = Instantiate(groundPrefab, transform);
                        groundGameObj.transform.position = worldMousePos;
                        groundGameObj.name = "Player";

                        // ����ɫ��������
                        groundGameObj.GetComponent<SpriteRenderer>().color = Color.cyan;

                        mCanDraw = false;
                    }
                }
                else if (mCurrentObjectMouseOn && mCurrentOperateMode == OperateMode.Erase)
                {
                    Destroy(mCurrentObjectMouseOn);
                    mCurrentObjectMouseOn = null;
                }
            }
        }
    }
}
