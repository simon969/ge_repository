using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
namespace ge_repository.Extensions { 
public abstract class Enumeration : IComparable
{
    public string Name { get; private set; }

    public int Id { get; private set; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var fields = typeof(T).GetFields(BindingFlags.Public |
                                         BindingFlags.Static |
                                         BindingFlags.DeclaredOnly);

        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    public override bool Equals(object obj)
    {
        var otherValue = obj as Enumeration;

        if (otherValue == null)
            return false;

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

    // Other utility methods ...
}
public class dataLayout : Enumeration {
        public static dataLayout NotIdentified =  new dataLayout(-1, "NotIdentified");
        public static dataLayout DataLogger101 = new dataLayout(1, "DataLogger101");
        public static dataLayout DataDipper101 = new dataLayout (2,"DataDipper101");
        public dataLayout(int id, string name)
            :base(id,name) {}
    }
}
