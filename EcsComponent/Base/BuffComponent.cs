using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ShootGame
{
    public class BuffComponent : IEcsComponent
    {
        private List<BuffBase> _buffs;

        public BuffComponent()
        {
            _buffs = new List<BuffBase>();
            EventManager.Instance.RegisterEvent<BuffBase, bool>("NoxianMightInit", AddBuff);
        }

        public void Tick(float deltatime)
        {
            foreach (var buff in _buffs)
            {
                if (buff.m_buffState == BuffState.Active) { buff.OnUpdate(deltatime); }
            }
        }

        public void CommitBuff<T>() where T : BuffBase, new()
        {
            //如果buff在list中不存在，调用addBuff
            //如果存在且是inactive的，设置为active，调用onEnter
            for(int i = 0; i < _buffs.Count; i++)
            {
                //如果存了这种buff了
                if (_buffs[i].GetType() == typeof(T))
                {
                    //如果这个buff是激活状态
                    if(_buffs[i].m_buffState == BuffState.Active)
                    {
                        _buffs[i].OnRefresh();
                        return;
                    }
                    //如果是未激活状态
                    else
                    {
                        _buffs[i].OnEnter();
                        return;
                    }                  
                }                             
            }
            //没存过的话就创建 
            BuffBase buff = ReferencePool.Acquire<T>();
            buff.OnInit();
            buff.OnEnter();
            _buffs.Add(buff);
        }

        /// <summary>
        /// 用法：比如流血到5层了，需要添加血怒buff
        /// </summary>
        /// <param name="buff"></param>
        public void AddBuff(BuffBase buff, bool isCaster)
        {
            // 获取 buff 的类型
            Type buffType = buff.GetType();

            // 使用反射来调用 CommitBuff<T>
            MethodInfo method = typeof(BuffComponent).GetMethod(nameof(CommitBuff), BindingFlags.Public | BindingFlags.Instance);
            MethodInfo genericMethod = method.MakeGenericMethod(buffType);
            genericMethod.Invoke(this, null);

            buff.Initialize(isCaster);
        }

        public T GetBuff<T>() where T : BuffBase
        {
            for(int i =  0; i < _buffs.Count; i++)
            {
                if (_buffs[i].GetType() == typeof(T)) return (T)_buffs[i];
            }
            //throw new Exception("没找到要获取的buff");
            return null;
        }
        public void Clear()
        {
            
        }
    }
}
