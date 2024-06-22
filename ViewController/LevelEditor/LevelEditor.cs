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
        /// 操作模式
        /// </summary>
        public enum OperateMode
        {
            Draw,
            Erase
        }

        /// <summary>
        /// 笔刷模式
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
            // 显示当前模式
            if (mCurrentOperateMode == OperateMode.Draw)
            {
                GUI.Label(modeLabelRect, mCurrentOperateMode + ":" + mCurrentBrushType, mModeLabelStyle.Value);
            }
            else
            {
                GUI.Label(modeLabelRect, mCurrentOperateMode.ToString(), mModeLabelStyle.Value);
            }

            var drawButtonRect = new Rect(10, 10, 150, 40);
            if (GUI.Button(drawButtonRect, "绘制", mButtonStyle.Value))
            {
                mCurrentOperateMode = OperateMode.Draw;
            }

            var eraseButtonRect = new Rect(10, 60, 150, 40);
            if (GUI.Button(eraseButtonRect, "橡皮", mButtonStyle.Value))
            {
                mCurrentOperateMode = OperateMode.Erase;
            }

            if (mCurrentOperateMode == OperateMode.Draw)
            {
                var groundButtonRect = new Rect(Screen.width - 110, 10, 100, 40);
                if (GUI.Button(groundButtonRect, "地块", mRightButtonStyle.Value))
                {
                    mCurrentBrushType = BrushType.Ground;
                }

                var heroButtonRect = new Rect(Screen.width - 110, 60, 100, 40);
                if (GUI.Button(heroButtonRect, "主角", mRightButtonStyle.Value))
                {
                    mCurrentBrushType = BrushType.Hero;
                }
            }

            var saveButtonRect = new Rect(Screen.width - 110, Screen.height - 50, 100, 40);
            if (GUI.Button(saveButtonRect, "保存", mRightButtonStyle.Value))
            {

                List<LevelItemInfo> infos = new List<LevelItemInfo>(transform.childCount);
                // 搜集
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

                var level = document.CreateElement("Level"); // 根节点
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

                // 将 xml 写入到文件路径
                document.Save(levelFilePath);
            }
        }

        public SpriteRenderer EmptyHighlight;

        /// <summary>
        /// 是否可绘制
        /// </summary>
        private bool mCanDraw;

        /// <summary>
        /// 鼠标指向的物体
        /// </summary>
        private GameObject mCurrentObjectMouseOn;

        private void Update()
        {
            var mousePosition = Input.mousePosition;
            var worldMousePos = Camera.main.ScreenToWorldPoint(mousePosition);

            // 把鼠标坐标转换为整数坐标，实现方格的效果
            worldMousePos.x = Mathf.Floor(worldMousePos.x + 0.5f);
            worldMousePos.y = Mathf.Floor(worldMousePos.y + 0.5f);

            worldMousePos.z = 0;

            //如果鼠标和界面按钮有交互的话，就暂时不显示高亮块，这样在点击按钮的时候不会画或者擦除地块
            if (GUIUtility.hotControl == 0)
            {
                EmptyHighlight.gameObject.SetActive(true);
            }
            else
            {
                EmptyHighlight.gameObject.SetActive(false);
            }

            //如果鼠标位置和高亮块的位置相同，什么都不做
            if (Math.Abs(EmptyHighlight.transform.position.x - worldMousePos.x) < 0.1f
                && Math.Abs(EmptyHighlight.transform.position.y - worldMousePos.y) < 0.1f) // -+
            {
                
            }
            else
            {
                // 设置高亮块的位置
                var emptyHighlightPos = worldMousePos;
                emptyHighlightPos.z = -1;
                EmptyHighlight.transform.position = emptyHighlightPos;

                // 发出射线
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);

                var hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity);

                // 有碰撞说明是有地块，不可以在当前位置绘制新的地块
                if (hit.collider)
                {
                    if (mCurrentOperateMode == OperateMode.Draw)
                    {
                        EmptyHighlight.color = new Color(1, 0, 0, 0.5f); // 红色代表不能绘制
                    }
                    else
                    {
                        EmptyHighlight.color = new Color(1, 0.5f, 0, 0.5f); // 橙色代表可擦除
                    }
                    mCanDraw = false;
                    mCurrentObjectMouseOn = hit.collider.gameObject;
                }
                else
                {
                    if (mCurrentOperateMode == OperateMode.Draw)
                    {
                        EmptyHighlight.color = new Color(1, 1, 1, 0.5f); // 白色代表可以绘制
                    }
                    else
                    {
                        EmptyHighlight.color = new Color(0, 0, 1, 0.5f); // 蓝色代表橡皮状态
                    }

                    mCanDraw = true;
                    mCurrentObjectMouseOn = null;
                }
                
            }
            //点击或者拖动鼠标的时候可以绘制/擦除地块
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
                        // 先同样用地块
                        var groundPrefab = Resources.Load<GameObject>("Ground");
                        var groundGameObj = Instantiate(groundPrefab, transform);
                        groundGameObj.transform.position = worldMousePos;
                        groundGameObj.name = "Player";

                        // 用青色代替主角
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
