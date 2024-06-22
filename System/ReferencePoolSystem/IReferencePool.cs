using QFramework;
using System;
using System.Collections.Generic;

namespace ShootGame
{
    public interface IReference
    {
        /// <summary>
        /// ��release�س��ӵ�ʱ�����
        /// </summary>
        void Clear();
    }

    public interface ReferencePoolSystem : ISystem
    {
        public T Acquire<T>() where T : class, IReference, new();
        public void Release(IReference reference);
    }

    public class ReferencePool : AbstractSystem, ReferencePoolSystem
    {
        private static readonly Dictionary<Type, ReferenceCollection> s_ReferenceCollections 
            = new Dictionary<Type, ReferenceCollection>();
        public static int Count { get { return s_ReferenceCollections.Count; } }

        //�����ó��л�ȡ����
        public T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }

        //�Ѷ���Ż����ó�
        public void Release(IReference reference)
        {
            if (reference == null)
            {
                throw new Exception("Reference is invalid.");
            }

            Type referenceType = reference.GetType();
            GetReferenceCollection(referenceType).Release(reference);
        }

        private static ReferenceCollection GetReferenceCollection(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new Exception("ReferenceType is invalid.");
            }

            ReferenceCollection referenceCollection = null;
            //����ֵ���û���ҵ����ͣ��ʹ���һ��
            if (!s_ReferenceCollections.TryGetValue(referenceType, out referenceCollection))
            {
                referenceCollection = new ReferenceCollection(referenceType);
                s_ReferenceCollections.Add(referenceType, referenceCollection);
            }

            return referenceCollection;
        }

        protected override void OnInit()
        {
            
        }
    }
}