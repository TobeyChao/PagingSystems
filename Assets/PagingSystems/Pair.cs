using System;

[Serializable]
public class Pair<T, U>
{
    public Pair()
    {
        First = default;
        Second = default;
    }
    public Pair(T first, U second)
    {
        First = first;
        Second = second;
    }
    public T First = default;
    public U Second = default;
}