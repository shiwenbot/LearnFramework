using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootGame { 
    public class TaskPool<T> where T : TaskBase
    {
        private readonly LinkedList<T> m_WorkingTasks;
        private readonly LinkedList<T> m_WaitingTasks;

        public TaskPool()
        {
            m_WorkingTasks = new LinkedList<T>();
            m_WaitingTasks = new LinkedList<T>();
        }

        public void AddTask(T task)
        {
            LinkedListNode<T> current = m_WaitingTasks.Last;
            while (current != null)
            {
                if (task.Priority <= current.Value.Priority)
                {
                    break;
                }

                current = current.Previous;
            }

            if (current != null)
            {
                m_WaitingTasks.AddAfter(current, task);
            }
            else
            {
                m_WaitingTasks.AddFirst(task);
            }
        }


    }
}