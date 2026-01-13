using System.Runtime.InteropServices;
using ParallelAnimationSystem.Core.Data;

namespace ParallelAnimationSystem.Core;

public struct ThemeColorState
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Buffer4<T> where T : unmanaged
    {
        public int Length => 4;
        
        private T element0;
        private T element1;
        private T element2;
        private T element3;
        
        public unsafe T this[int index]
        {
            get
            {
                if (index is < 0 or >= 4)
                    throw new IndexOutOfRangeException();

                fixed (T* ptr = &element0)
                    return ptr[index];
            }
            set
            {
                if (index is < 0 or >= 4)
                    throw new IndexOutOfRangeException();

                fixed (T* ptr = &element0)
                    ptr[index] = value;
            }
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Buffer9<T> where T : unmanaged
    {
        public int Length => 9;
        
        private T element0;
        private T element1;
        private T element2;
        private T element3;
        private T element4;
        private T element5;
        private T element6;
        private T element7;
        private T element8;
        
        public unsafe T this[int index]
        {
            get
            {
                if (index is < 0 or >= 9)
                    throw new IndexOutOfRangeException();

                fixed (T* ptr = &element0)
                    return ptr[index];
            }
            set
            {
                if (index is < 0 or >= 9)
                    throw new IndexOutOfRangeException();

                fixed (T* ptr = &element0)
                    ptr[index] = value;
            }
        }
    }

    public Buffer4<ColorRgb> Player;
    public Buffer9<ColorRgb> Object;
    public Buffer9<ColorRgb> Effect;
    public Buffer9<ColorRgb> ParallaxObject;
    public required ColorRgb Background;
    public required ColorRgb Gui;
    public required ColorRgb GuiAccent;
}