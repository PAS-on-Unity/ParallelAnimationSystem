namespace MsdfgenNet.Interop;

public abstract class NativeObject(IntPtr handle) : IDisposable
{
    internal IntPtr Handle { get; } = handle;
    
    private bool disposed;
    
    ~NativeObject()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        if (disposed)
            return;
        
        Dispose(true);
        GC.SuppressFinalize(this);
        
        disposed = true;
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;
        
        FreeHandle(handle);
    }

    protected abstract void FreeHandle(IntPtr handle);

    public override int GetHashCode()
    {
        return Handle.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is NativeObject nativeObject && Handle == nativeObject.Handle;
    }
    
    public static bool operator ==(NativeObject? left, NativeObject? right)
    {
        return left?.Handle == right?.Handle;
    }
    
    public static bool operator !=(NativeObject? left, NativeObject? right)
    {
        return left?.Handle != right?.Handle;
    }
}