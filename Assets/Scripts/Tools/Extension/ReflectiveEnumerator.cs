using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ReflectiveEnumerator
{
    static ReflectiveEnumerator() { }

    public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class/*, IComparable<T>*/
    {
        //List<T> objects = new List<T>();
        //foreach (Type type in
        //    Assembly.GetAssembly(typeof(T)).GetTypes()
        //    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
        //{
        //    objects.Add((T)Activator.CreateInstance(type, constructorArgs));
        //}
        //objects.Sort();
        //return objects;

        return Assembly.GetAssembly(typeof(T))
                       .GetTypes()
                       .Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(T)));
    }
}