using System;


// https://www.pluralsight.com/guides/property-copying-between-two-objects-using-reflection
namespace ge_repository.repositories {

public class PropertyCopier<TParent, TChild> where TParent : class
                                            where TChild : class
{
    public static void Copy(TParent parent, TChild child)
    {
        var parentProperties = parent.GetType().GetProperties();
        var childProperties = child.GetType().GetProperties();

        foreach (var parentProperty in parentProperties)
        {
            foreach (var childProperty in childProperties)
            {
                if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                {
                    childProperty.SetValue(child, parentProperty.GetValue(parent));
                    break;
                }
            }
        }
    }
}
public class PropertyCopier<TEntity> where TEntity : class                                            
{
    public void Copy(TEntity to, TEntity from)
    {
        var fromProperties = from.GetType().GetProperties();
        var toProperties = to.GetType().GetProperties();

        foreach (var fromProperty in fromProperties)
        {
            foreach (var toProperty in toProperties)
            {
                if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                {
                    toProperty.SetValue(to, fromProperty.GetValue(from));
                    break;
                }
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class MatchParentAttribute : Attribute
{
    public readonly string ParentPropertyName;
    public MatchParentAttribute(string parentPropertyName)
    {
        ParentPropertyName = parentPropertyName;
    }
}
public class PersonMatch
{
    [MatchParent("Name")]
    public string NameMatch { get; set; }
    [MatchParent("Lastname")]
    public string LastnameMatch { get; set; }
}
public class PropertyMatcher<TParent, TChild> where TParent : class
                                                  where TChild : class
{
    public static void GenerateMatchedObject(TParent parent, TChild child)
    {
        var childProperties = child.GetType().GetProperties();
        foreach (var childProperty in childProperties)
        {
            var attributesForProperty = childProperty.GetCustomAttributes(typeof(MatchParentAttribute), true);
            var isOfTypeMatchParentAttribute = false;

            MatchParentAttribute currentAttribute = null;

            foreach (var attribute in attributesForProperty)
            {
                if (attribute.GetType() == typeof(MatchParentAttribute))
                {
                    isOfTypeMatchParentAttribute = true;
                    currentAttribute = (MatchParentAttribute) attribute;
                    break;
                }
            }

            if (isOfTypeMatchParentAttribute)
            {
                var parentProperties = parent.GetType().GetProperties();
                object parentPropertyValue = null;
                foreach (var parentProperty in parentProperties)
                {
                    if (parentProperty.Name == currentAttribute.ParentPropertyName)
                    {
                        if (parentProperty.PropertyType== childProperty.PropertyType)
                        {
                            parentPropertyValue = parentProperty.GetValue(parent);
                        }
                    }
                }
                childProperty.SetValue(child, parentPropertyValue);
            }
        }
    }
}

public static class ObjectExtensionMethods
{
    public static void CopyPropertiesFrom(this object self, object parent)
    {
        var fromProperties = parent.GetType().GetProperties();
        var toProperties = self.GetType().GetProperties();

        foreach (var fromProperty in fromProperties)
        {
            foreach (var toProperty in toProperties)
            {
                if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                {
                    toProperty.SetValue(self, fromProperty.GetValue(parent));
                    break;
                }
            }
        }
    }

    public static void MatchPropertiesFrom(this object self, object parent)
    {
        var childProperties = self.GetType().GetProperties();
        foreach (var childProperty in childProperties)
        {
            var attributesForProperty = childProperty.GetCustomAttributes(typeof(MatchParentAttribute), true);
            var isOfTypeMatchParentAttribute = false;

            MatchParentAttribute currentAttribute = null;

            foreach (var attribute in attributesForProperty)
            {
                if (attribute.GetType() == typeof(MatchParentAttribute))
                {
                    isOfTypeMatchParentAttribute = true;
                    currentAttribute = (MatchParentAttribute)attribute;
                    break;
                }
            }

            if (isOfTypeMatchParentAttribute)
            {
                var parentProperties = parent.GetType().GetProperties();
                object parentPropertyValue = null;
                foreach (var parentProperty in parentProperties)
                {
                    if (parentProperty.Name == currentAttribute.ParentPropertyName)
                    {
                        if (parentProperty.PropertyType== childProperty.PropertyType)
                        {
                            parentPropertyValue = parentProperty.GetValue(parent);
                        }
                    }
                }
                childProperty.SetValue(self, parentPropertyValue);
            }
        }
    }
}

}
