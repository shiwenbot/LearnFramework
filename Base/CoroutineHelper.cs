using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper instance;
    private Queue<IEnumerator> mCoroutineQueue = new Queue<IEnumerator>();
    //当前
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

    private IEnumerator mCurrentWork = null;
    public void EnqueueWork(IEnumerator work)
    {
        mCoroutineQueue.Enqueue(work);
    }

    void Update()
    {
        while (mCoroutineQueue.Count > 30)
            DoWork();

        //如果协程队列>20个,则一次执行3个语块
        if (mCoroutineQueue.Count > 20)
            DoWork();

        //如果协程队列>10个,则一次执行2个语块
        if (mCoroutineQueue.Count > 10)
            DoWork();

        DoWork();
    }

    void DoWork()
    {
        if (mCurrentWork == null && mCoroutineQueue.Count == 0)
            return;

        if (mCurrentWork == null)
        {
            mCurrentWork = mCoroutineQueue.Dequeue();
        }

        //这个协程片段是否执行完毕
        bool mElementFinish = !mCurrentWork.MoveNext();

        if (mElementFinish)
        {
            mCurrentWork = null;
        }
        //如果协程里面嵌套协程
        else if (mCurrentWork.Current is IEnumerator)
        {
            mCurrentWork = (mCurrentWork.Current as IEnumerator);
        }
    }
}
