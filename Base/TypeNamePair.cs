using System;
using System.Runtime.InteropServices;

namespace ShootGame
{
    [StructLayout(LayoutKind.Auto)]
    internal struct TypeNamePair : IEquatable<TypeNamePair>
    {
        private readonly Type m_Type;
        private readonly string m_Name;
        public TypeNamePair(Type type, string name)
        {
            m_Type = type;
            m_Name = name ?? string.Empty;
        }
        public bool Equals(TypeNamePair obj)
        {
            return obj is TypeNamePair && Equals((TypeNamePair)obj);
        }
    }
}