using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using XLua;

namespace ShootGame
{
    public class HotFixState : FsmState<ProcedureManager>
    {
        LuaEnv luaEnv = null;

        IFsm<ProcedureManager> m_fsm;
        List<string> differingFiles;

        private string baseURL = "http://172.16.111.199:81/";
        private string localFolderPath = "D:/unity/2D shoot/Assets/AssetBundle/";

        bool finishHotfix = false;

        public override void OnInit(IFsm<ProcedureManager> fsm)
        {
            base.OnInit(fsm);
            m_fsm = fsm;
        }

        public override void OnEnter(IFsm<ProcedureManager> fsm)
        {            
            base.OnEnter(fsm);
            differingFiles = m_fsm.GetData<VarList<string>>("differingFiles");

            CoroutineHelper.Instance.StartCoroutine(HotFixCoroutine(differingFiles));
        }

        public override void OnUpdate(IFsm<ProcedureManager> fsm)
        {
            base.OnUpdate(fsm);
            if (!finishHotfix) return;

            /**/
            luaEnv = new LuaEnv();
            luaEnv.AddLoader(CustomLoader);
            luaEnv.DoString("require 'main'");
            ChangeState<GameState>(fsm);
        }

        private IEnumerator HotFixCoroutine(List<string> differingFiles)
        {
            foreach (string fileName in differingFiles)
            {
                string downloadURL = baseURL + fileName;
                string localFilePath = Path.Combine(localFolderPath, fileName);

                UnityWebRequest request = UnityWebRequest.Get(downloadURL);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error downloading file {fileName}: {request.error}");
                }
                else
                {
                    byte[] fileData = request.downloadHandler.data;

                    // Save downloaded file to local path
                    File.WriteAllBytes(localFilePath, fileData);
                    Debug.Log($"File {fileName} downloaded and saved to {localFilePath}");
                }
            }
            yield return new WaitForSeconds(2f); //tmp
            finishHotfix = true;
        }

        private byte[] CustomLoader(ref string filePath)
        {
            // 自定义路径
            string customPath = Path.Combine(@"D:\unity\2D shoot\Assets\AssetBundle", filePath + ".lua.txt");
            if (File.Exists(customPath))
            {
                Debug.Log("Loading file from: " + customPath);
                return File.ReadAllBytes(customPath);
            }
            else
            {
                Debug.LogError("File not found: " + customPath);
                return null;
            }
        }
    }
}