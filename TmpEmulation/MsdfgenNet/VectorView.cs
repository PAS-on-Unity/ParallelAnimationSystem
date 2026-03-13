using System.Collections;
using MsdfgenNet.Interop;

namespace MsdfgenNet;

public delegate T VectorViewElementFactory<out T>(IntPtr handle);

public class VectorView<T> : NativeObject, IReadOnlyList<T>
{
    public int Count => (int)Native.msdfgen_VectorView_count(Handle);

    private readonly VectorViewElementFactory<T> factory;
    private readonly UIntPtr elementSize;
    private readonly IntPtr dataPtr;
    
    internal VectorView(IntPtr handle, VectorViewElementFactory<T> factory) : base(handle)
    {
        this.factory = factory;
        
        elementSize = Native.msdfgen_VectorView_elementSize(Handle);
        unsafe
        {
            var countTemp = UIntPtr.Zero;
            fixed (IntPtr* dataPtrPtr = &dataPtr)
                Native.msdfgen_VectorView_data(Handle, new IntPtr(&countTemp), new IntPtr(dataPtrPtr));
        }
    }
    
    public T this[int index] => factory(GetElementHandle(index));

    private IntPtr GetElementHandle(int index)
    {
        if (index >= Count)
            throw new IndexOutOfRangeException();
        return dataPtr + (int)elementSize * index;
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
            yield return this[i];
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    protected override void FreeHandle(IntPtr handle)
    {
        Native.msdfgen_VectorView_destroy(handle);
    }
}