using QFramework;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace ShootGame
{
    public static class GameEntry
    {
        private static readonly LinkedList<FrameworkComponent> s_FrameworkComponents = new LinkedList<FrameworkComponent>();
        public static FrameworkComponent GetComponent(Type type)
        {
            LinkedListNode<FrameworkComponent> current = s_FrameworkComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    return current.Value;
                }
                current = current.Next;
            }
            return null;
        }

        internal static void RegisterComponent(FrameworkComponent frameworkComponent)
        {
            Type type = frameworkComponent.GetType();

            LinkedListNode<FrameworkComponent> current = s_FrameworkComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    Debug.Log("Component is already exists");
                    return;
                }
                current = current.Next;
            }

            s_FrameworkComponents.AddLast(frameworkComponent);
        }
    }
}