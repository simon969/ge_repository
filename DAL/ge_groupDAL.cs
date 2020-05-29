using System;
using System.Collections.Generic;
using ge_repository.Models;
using ge_repository.DAL;

namespace ge_repository.DAL {

    public class ge_groupDAL : ige_groupDAL {
        public ge_DbContext context {get;set;}
        public ge_group group {get;set;}
        public ige_projectionDAL projection {get;set;}
        ge_groupDAL(ge_DbContext context,  ige_projectionDAL projection ) { 
            this.context = context;
            this.projection = projection; 
        }
        public IEnumerable<ge_data> getGroupData(Guid groupId){return null;}
        public IEnumerable<ge_project> getGroupProjects(Guid groupId){return null;}
        public ge_data getGroupById(Guid groupId){return null;}
        public void insertGroup(ge_group group){}
        public void deleteGroup(Guid groupId){}
        public void updateGroup(ge_group group){}
        public void addUser(ge_user u, string operations) {

            if (group != null) {
                if (group.users != null) {
                 ge_user_ops os = new ge_user_ops();
                    os.user =u;
                    os.operations = operations;
                    group.users.Add (os);
                }
            }
            

        }
        public string getUserOperations(string userId) {
        
        if (group != null) {
            if (group.users != null) {
                foreach  (ge_user_ops gu in group.users) {
                    if (gu.userId==userId) {
                        return gu.operations;
                    }
                } 
            }
        }
        return "";
        }

        public void Save(){}
        public void Dispose(){}
    }
}

