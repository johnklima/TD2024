using System;
using UnityEngine;

namespace Scenes.Leif.Scripts
{
    [Serializable]
    public struct INT2
    {
        [Range(1, 100)] public int x;
        [Range(1, 100)] public int y;

        public INT2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }
    }

    [Serializable]
    public struct INT3
    {
        [Range(1, 100)] public int x;
        [Range(1, 100)] public int y;
        [Range(1, 100)] public int z;

        public INT3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }
    }

    [Serializable]
    public struct Int3
    {
        public int x;
        public int y;
        public int z;

        public Int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"x:{x}, y:{y}, z:{z}";
        }

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }
    }
}