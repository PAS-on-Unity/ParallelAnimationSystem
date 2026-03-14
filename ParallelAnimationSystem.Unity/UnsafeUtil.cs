namespace ParallelAnimationSystem.Unity;

public static class UnsafeUtil
{
    public static unsafe int SizeOf<T>() where T : unmanaged
    {
        return sizeof(T);
    }
}