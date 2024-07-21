using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace ShootGame
{
    public class EcsComponentManager : FrameworkComponent
    {
        protected override void Awake()
        {
            base.Awake();
            EventManager.Instance.RegisterEvent<Entity, List<IEcsComponent>>("IntializeEntity", OnEntityInit);
            EventManager.Instance.RegisterEvent<Entity, List<IEcsComponent>>("DestroyEntity", OnEntityDestroy);
        }

        protected override void Update()
        {
            foreach (var componentArray in componentArrays)
            {
                
            }
        }

        private List<IComponentArray> componentArrays = new List<IComponentArray>();

        public ComponentArray<T> GetArray<T>() where T : class, IEcsComponent, new()
        {
            foreach (var array in componentArrays)
            {
                if (array is ComponentArray<T> componentArray)
                {
                    return componentArray;
                }
            }

            // 创建新的 ComponentArray<T> 实例
            var newArray = ReferencePool.Acquire<ComponentArray<T>>();
            componentArrays.Add(newArray);
            return newArray;
        }

        /// <summary>
        /// 委托，当有一个新的Entity被创建的时候，自动更新
        /// 例如，诺手加入游戏，需要找到HPcomponentArray和BleedComponentArray
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="component"></param>
        public void OnEntityInit(Entity entity, List<IEcsComponent> components)
        {
            foreach (var component in components)
            {
                Type componentType = component.GetType();
                MethodInfo method = typeof(EcsComponentManager).GetMethod(nameof(AddComponentToArray), BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo genericMethod = method.MakeGenericMethod(componentType);
                genericMethod.Invoke(this, new object[] { entity, component });

                //如果这个ecsComponent是buff的话，需要通知buffComponentManager
                if(componentType == typeof(BuffComponent))
                {
                    Debug.Log("Send Event");
                    EventManager.Instance.SendEvent("Initialize buffComponent", (BuffComponent)component);
                }
            }
        }

        public void OnEntityDestroy(Entity entity, List<IEcsComponent> components)
        {
            foreach (var component in components)
            {
                Type componentType = component.GetType();
                MethodInfo method = typeof(EcsComponentManager).GetMethod(nameof(RemoveComponentFromArray), BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo genericMethod = method.MakeGenericMethod(componentType);
                genericMethod.Invoke(this, new object[] { entity, component });
            }
        }

        private void AddComponentToArray<T>(Entity entity, T component) where T : class, IEcsComponent, new()
        {
            var array = GetArray<T>();
            array.AddComponent(entity, component);
        }

        private void RemoveComponentFromArray<T>(Entity entity, T component) where T : class, IEcsComponent, new()
        {
            var array = GetArray<T>();
            array.RemoveComponent(entity, component);
        }
    }
}
