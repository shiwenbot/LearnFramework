using System.Collections.Generic;
using System;
using UnityEngine;

namespace ShootGame
{
    public class BuffComponent : IEcsComponent
    {
        private List<BuffBase> _buffs;

        public BuffComponent()
        {
            _buffs = new List<BuffBase>();
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

        public void AddBuff(BuffBase buff)
        {
            //新建buff并加入到list中
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
