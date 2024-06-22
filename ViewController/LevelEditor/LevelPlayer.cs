using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace ShootGame
{
    public class LevelPlayer : MonoBehaviour
    {

        enum PlayerState
        {
            SelectLevelFile,
            Playing
        }

        private PlayerState mCurrentLevelFile = PlayerState.SelectLevelFile;

        private string mLevelFilesFolder;
        private void Awake()
        {
            mLevelFilesFolder = Application.persistentDataPath + "/LevelFiles";
            //获取文件夹路径中的第一个xml文件，读取
            var filePath = Directory.GetFiles(mLevelFilesFolder).FirstOrDefault(f => f.EndsWith("xml"));
            if (filePath != null)
            {
                var xml = File.ReadAllText(filePath);
                ParseAndRun(xml);
                mCurrentLevelFile = PlayerState.Playing;
            }
        }


        void ParseAndRun(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);

            var levelNode = document.SelectSingleNode("Level");

            foreach (XmlElement levelItemNode in levelNode.ChildNodes)
            {
                var levelItemName = levelItemNode.Attributes["name"].Value;
                var levelItemX = int.Parse(levelItemNode.Attributes["x"].Value);
                var levelItemY = int.Parse(levelItemNode.Attributes["y"].Value);

                var levelItemPrefab = Resources.Load<GameObject>(levelItemName);

                var levelItemGameObj = Instantiate(levelItemPrefab, transform);
                levelItemGameObj.transform.position = new Vector3(levelItemX, levelItemY, 0);
            }
        }

        /*private void OnGUI()
        {
            if (mCurrentLevelFile == PlayerState.SelectLevelFile)
            {
                int y = 10;

                foreach (var filePath in Directory.GetFiles(mLevelFilesFolder).Where(f => f.EndsWith("xml")))
                {
                    var fileName = Path.GetFileName(filePath);

                    if (GUI.Button(new Rect(10, y, 150, 40), fileName))
                    {
                        var xml = File.ReadAllText(filePath);
                        ParseAndRun(xml);
                        mCurrentLevelFile = PlayerState.Playing;
                    }
                    y += 50;
                }
            }
        }*/
    }
}