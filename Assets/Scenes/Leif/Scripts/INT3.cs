using System;
using UnityEngine;

namespace Scenes.Leif.Scripts
{
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
}