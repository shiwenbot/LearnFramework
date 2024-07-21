using System.Collections.Generic;
using System;
using UnityEngine;

namespace ShootGame
{
    public interface IComponentArray
    {
        Type ComponentType { get; }
    }


    /// <summary>
    /// 用连续内存记录所有component，比如说在轮询所有英雄的buff时，CPU就会把一整块buffComponent拿进内存
    /// 用一个字典记录entity和它的component在list中的位置
    /// 如果一个entity被移除了，那么在list中的component的数据也就无效了，就把要删除位置的数据和末尾交换，再更新字典
    /// </summary>
    public class ComponentArray<T> : IComponentArray, IReference where T : class, IEcsComponent, new()
    {
        private List<T> components = new List<T>();
        private Dictionary<Entity, int> indexMap = new Dictionary<Entity, int>();
        private Dictionary<int, Entity> reverseIndexMap = new Dictionary<int, Entity>();

        public Type ComponentType => typeof(T);

        public T GetComponent(Entity entity)
        {           
            // 如果map中不存在这个key，调用Add
            if (!indexMap.ContainsKey(entity))
            {
                var newComponent = ReferencePool.Acquire<T>();
                AddComponent(entity, newComponent);
            }

            // 根据map找到index，然后去list中取数据
            if (indexMap.TryGetValue(entity, out int index))
            {
                return components[index];
            }
            throw new Exception("无法获取组件");
        }

        public void AddComponent(Entity entity, T component)
        {
            //如果map中已经存在key了，报错
            if(indexMap.ContainsKey(entity))
            {
                throw new Exception("Entity已经创建了这个类型的Component");
            }
            //如果不存在，正常插入到list，把index记录到map
            int index = components.Count;
            components.Add(component);
            indexMap[entity] = index;
            reverseIndexMap[index] = entity;
        }

        public void RemoveComponent(Entity entity, T component)
        {
            //在map中获取到当前的index，把当前index的内容和末尾的内容调换，不要忘记更新map的值
            if (!indexMap.ContainsKey(entity))
            {
                throw new Exception("Entity不存在这个类型的Component");
            }

            int index = indexMap[entity];
            int lastIndex = components.Count - 1;
            //如果要移除的元素不是数组中最后一个
            if(index !=  lastIndex)
            {
                components[index] = components[lastIndex];

                Entity lastEntity = reverseIndexMap[lastIndex];
                indexMap[lastEntity] = index;
                reverseIndexMap[index] = lastEntity;               
            }
            components.RemoveAt(lastIndex);
            indexMap.Remove(entity);
            reverseIndexMap.Remove(lastIndex);
        }

        public void Clear()
        {
            
        }
    }
}
