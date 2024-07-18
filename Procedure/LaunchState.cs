using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using XLua;

namespace ShootGame
{
    public class LaunchState : FsmState<ProcedureManager>
    {
        bool downloadVersionFile = false;
        bool writeMD5 = false;
        private byte[] downloadedFileData;
        private Dictionary<string, string> localMD5Map;

        IFsm<ProcedureManager> m_fsm;

        public override void OnInit(IFsm<ProcedureManager> fsm)
        {
            base.OnInit(fsm);
            m_fsm = fsm;
        }
        public override void OnEnter(IFsm<ProcedureManager> fsm)
        {
            base.OnEnter(fsm);            

            string url = "http://172.16.111.199:81/md5list.txt";
            CoroutineHelper.Instance.StartCoroutine(DownloadVersioinFile(url));
            //CoroutineHelper.Instance.StartCoroutine(GenerateMD5Coroutine("Assets/AssetBundle", "Assets/Config/md5list.txt"));
        }

        public override void OnUpdate(IFsm<ProcedureManager> fsm)
        {
            if (!downloadVersionFile)
            {
                return;
            }
            List<string> differingFiles = CompareFilesWithLocal("Assets/Config/md5list.txt");
            if (differingFiles.Count > 0) //如果有不同的文件
            {              
                m_fsm.SetData<VarList<string>>("differingFiles", differingFiles);
                ChangeState<HotFixState>(fsm);
            }
        }
        public override void OnLeave(IFsm<ProcedureManager> fsm) { }
        public override void OnDestroy(IFsm<ProcedureManager> fsm) { }

        private IEnumerator DownloadVersioinFile(string url)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                downloadedFileData = request.downloadHandler.data; //存一下云端的版本数据文件
            }
            downloadVersionFile = true;
        }

        private IEnumerator GenerateMD5Coroutine(string directoryPath, string outputPath)
        {
            directoryPath = Path.Combine(Application.dataPath, directoryPath.Substring(7)); // 去掉"Assets/"前缀并转换为绝对路径
            outputPath = Path.Combine(Application.dataPath, outputPath.Substring(7)); // 去掉"Assets/"前缀并转换为绝对路径

            if (!Directory.Exists(directoryPath))
            {
                Debug.LogError("Directory does not exist: " + directoryPath);
                yield break;
            }

            StringBuilder sb = new StringBuilder();

            // 获取目录及子目录中的所有文件
            string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                if (!filePath.EndsWith(".meta"))
                {
                    string md5Hash = GetMD5HashFromFile(filePath);
                    string relativePath = "Assets" + filePath.Substring(Application.dataPath.Length).Replace("\\", "/");
                    sb.AppendLine($"{relativePath} {md5Hash}");
                }

                // 每处理一个文件后等待一帧，以防止卡住主线程
                yield return null;
            }

            // 写入输出文件
            File.WriteAllText(outputPath, sb.ToString());
            Debug.Log($"MD5 hashes have been written to {outputPath}");
        }

        private string GetMD5HashFromFile(string filePath)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = MD5.Create();
                byte[] fileHash = md5.ComputeHash(file);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in fileHash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public List<string> CompareFilesWithLocal(string localMD5ListPath)
        {
            List<string> differingFiles = new List<string>();

            if (downloadedFileData == null || downloadedFileData.Length == 0)
            {
                Debug.LogError("No downloaded data to compare.");
                return differingFiles;
            }

            // Load local MD5 list
            LoadLocalMD5List(localMD5ListPath);

            // Convert byte array to string
            string downloadedContent = System.Text.Encoding.UTF8.GetString(downloadedFileData);
            // Split by new lines
            string[] lines = downloadedContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(' ');
                if (parts.Length != 2)
                {
                    Debug.LogWarning($"Invalid line format: {line}");
                    continue;
                }

                string relativePath = parts[0];
                string downloadedMD5 = parts[1];

                if (localMD5Map.TryGetValue(relativePath, out string localMD5))
                {
                    if (!localMD5.Equals(downloadedMD5, StringComparison.OrdinalIgnoreCase))
                    {
                        differingFiles.Add(relativePath);
                    }
                }
                else
                {
                    differingFiles.Add(relativePath); // Local file does not exist in MD5 list
                }
            }

            return differingFiles;
        }

        private void LoadLocalMD5List(string localMD5ListPath)
        {
            localMD5Map = new Dictionary<string, string>();

            if (!File.Exists(localMD5ListPath))
            {
                Debug.LogError($"Local MD5 list file not found: {localMD5ListPath}");
                return;
            }

            string[] lines = File.ReadAllLines(localMD5ListPath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(' ');
                if (parts.Length != 2)
                {
                    Debug.LogWarning($"Invalid line format in local MD5 list: {line}");
                    continue;
                }

                string relativePath = parts[0];
                string md5Hash = parts[1];
                localMD5Map[relativePath] = md5Hash;
            }
        }


    }
}