namespace StorageMonster.Util
{
    public sealed class Tuple<T1,T2>
    {
        public T1 Object1 { get; private set; }
        public T2 Object2 { get; private set; }

        public Tuple(T1 object1, T2 object2)
        {
            Object1 = object1;
            Object2 = object2;
        }
    }
}
