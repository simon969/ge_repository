using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace ge_repository.AGS {

    public interface IAGSGroup {

        string GroupName();
        int set_values(string[] header, string[] values) ;
        string ValueToCSV (string[] include_order, string delimeter, string encapsulation);
        string HeaderToCSV (string[] include_order, string delimeter, string encapsulation);
        string UnitToCSV (string[] include_order, string delimeter, string encapsulation);
        string TypeToCSV (string[] include_order, string delimeter, string encapsulation);

    }
    public interface IAGSTable {

    }

    public class AGSGroup : IAGSGroup {
        private string _groupName {get;set;}

        public string GroupName(){
            return _groupName;
        }
        
        public AGSGroup(string name) {
            _groupName = name;
        }

        public virtual int set_values(string[] header, string[] values) {return -1;}
        public string ValueToCSV (string[] include_order, string delimeter=",", string encapsulation = "\"") {return "";}
        public string HeaderToCSV (string[] include_order, string delimeter=",", string encapsulation = "\"") {return "";}
        public string UnitToCSV (string[] include_order, string delimeter=",", string encapsulation = "\"") {return "";}
        public string TypeToCSV (string[] include_order, string delimeter=",", string encapsulation = "\"") {return "";}
    }

    public class AGSTable<T> : IAGSTable {
        public List<string> headers {get;set;}
        public List<string> units {get;set;} 
        public List<string> types {get;set;} 
        public List<T> values {get;set;}
    }


    // public class AGSMetadataDetails: DefaultMetadataDetails {



    // }        
    // public class AGSModelMetadataProvider : DefaultModelMetadataProvider {
    
    // // https://buildstarted.com/2010/09/14/creating-your-own-modelmetadataprovider-to-handle-custom-attributes/
        
    // protected override ModelMetadata CreateModelMetadata(
    //                          IEnumerable<Attribute> attributes,
    //                          Type containerType,
    //                          Func<object> modelAccessor,
    //                          Type modelType,
    //                          string propertyName) {

    //     var data = base.CreateMetadata(
    //                          attributes, 
    //                          containerType, 
    //                          modelAccessor, 
    //                          modelType, 
    //                          propertyName);

    //     var tooltip = attributes
    //             .SingleOrDefault(a => typeof(TooltipAttribute) == a.GetType());
    //     if (tooltip != null) data.AdditionalValues
    //             .Add("Tooltip", ((TooltipAttribute)tooltip).Tooltip);


    //     return data;
    // }
    // }
//public class CustomMetadataAttribure : Attribute, IMetadataAware
// {
//     public string Key { get; set; }
//     public CustomMetadataAttribure(string key) => this.Key = key;
//     public void OnMetadataCreated(ModelMetadata metadata)
//     {
//         //set properties of metadata
//     }
// }
    
    
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class AGSAttribute : Attribute
    {
        public AGSAttribute () { }
        public AGSAttribute (string name, object value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public object Value { get; private set; }
    }

     
    // https://www.codeproject.com/Tips/1243346/In-MVC-Core-Passing-Custom-Attribute-from-Model-Cl
    public class AGSMetadataProvider : IDisplayMetadataProvider, IValidationMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            // var tooltip = context.DisplayMetadata.AdditionalValues
            //      .SingleOrDefault(a => typeof(TooltipAttribute) == a.GetType());
            // if (tooltip != null) context.DisplayMetadata.AdditionalValues
            //      .Add("Tooltip", ((TooltipAttribute)tooltip).Tooltip);
            
            // var propertyAttributes = context.Attributes;
            // var modelMetadata = context.DisplayMetadata;
            // var propertyName = context.Key.Name;

           if (context.PropertyAttributes != null)
           {

               foreach (object propAttr in context.PropertyAttributes)
               {
                   AGSAttribute addMetaAttr = propAttr as AGSAttribute;
                   if (addMetaAttr != null)
                   {
                       context.DisplayMetadata.AdditionalValues.Add
                                     (addMetaAttr.Name, addMetaAttr.Value);
                   }
               }
           }
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
        }
    }
}



