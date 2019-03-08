public abstract class Singleton<T> where T : new()
{

    private static T instance;
    private static readonly object syslock = new object();

    public static T getInstance()
    {

        if (instance == null)
        {
            lock (syslock) {
                instance = new T();
            }

        }
        return instance;
    }

}

