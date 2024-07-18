using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace ShootGame
{
    public class GameSceneManager
    {
        private static GameSceneManager m_Instance;
        public static GameSceneManager Instance
        {
            get
            {
                if (m_Instance == null) m_Instance = new();
                return m_Instance;
            }
        }
        enum PlayerState
        {
            SelectLevelFile,
            Playing
        }

        private PlayerState mCurrentLevelFile = PlayerState.SelectLevelFile;

        private string mLevelFilesFolder;

        private GameSceneManager()
        {
            mLevelFilesFolder = Application.persistentDataPath + "/LevelFiles";
        }

        public void LoadScene()
        {
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

                var levelItemGameObj = Object.Instantiate(levelItemPrefab);
                levelItemGameObj.transform.position = new Vector3(levelItemX, levelItemY, 0);
            }
        }
    }
}