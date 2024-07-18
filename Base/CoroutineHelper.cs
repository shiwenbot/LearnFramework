﻿using UnityEngine;
using System.Collections;

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper instance;

    public static CoroutineHelper Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("CoroutineHelper");
                instance = obj.AddComponent<CoroutineHelper>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
}
