using QFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame
{
    public class ReferenceCollection
    {
        Queue<IReference> m_References; //这里存的是对象
        Type m_ReferenceType; //对象的类型
        int m_AcquireReferenceCount; //获取了的/正在使用中的对象
        int m_ReleaseReferenceCount; //空闲的对象
        int m_TotalReferenceCount; //对象的总数

        public ReferenceCollection(Type referenceType)
        {
            m_References = new Queue<IReference>();
            m_ReferenceType = referenceType;           
            m_AcquireReferenceCount = 0;
            m_ReleaseReferenceCount = 0;
            m_TotalReferenceCount = 0;
        }

        public Type ReferenceType { get { return m_ReferenceType; } }
        public int AcquireReferenceCount { get {  return m_AcquireReferenceCount; } }
        public int ReleaseReferenceCount { get { return m_ReleaseReferenceCount; } }
        public int TotalReferenceCount { get { return m_TotalReferenceCount; } }
        public T Acquire<T>() where T : class, IReference, new()
        {
            if (typeof(T) != m_ReferenceType)
            {
                throw new Exception("Type is invalid.");
            }
           
            m_AcquireReferenceCount++;

            //如果队列中有对象，就从队列中取出
            if (m_References.Count > 0)
            {
                Debug.Log("从池中获取的对象");
                return (T)m_References.Dequeue();
            }

            //如果池中没有对象
            Debug.Log("池子空了，需要新建");
            return new T();
        }

        public void Release(IReference reference)
        {
            reference.Clear();
            if (m_References.Contains(reference))
            {
                throw new Exception("The reference has been released.");
            }
            Debug.Log("归还对象给池子");
            m_References.Enqueue(reference);

            m_ReleaseReferenceCount++;
            m_AcquireReferenceCount--;
        }
    }
}