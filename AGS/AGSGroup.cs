using System;
using System.Collections.Generic;


namespace ge_repository.AGS {

    public interface IAGSGroup {

        string GroupName();
        int setValues(string[] header, string[] values) ;


    }
    public interface IAGSTable {

    }

    public class AGSGroup : IAGSGroup{
        private string _groupName {get;set;}

        public string GroupName(){
            return _groupName;
        }
        
        public AGSGroup(string name) {
            _groupName = name;
        }

        public virtual int setValues(string[] header, string[] values) {

        return -1;

        }

    }

    public class AGSTable<T> : IAGSTable {
        public List<string> headers {get;set;}
        public List<string> units {get;set;} 
        public List<string> types {get;set;} 
        public List<T> values {get;set;}
    } 

}


