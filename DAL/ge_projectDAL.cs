using System;
using System.Collections.Generic;
using ge_repository.Models;
using ge_repository.DAL;

namespace ge_repository.DAL {

 public class ge_projectDAL : ige_projectDAL {

    public ge_DbContext context {get;set;}
    public ge_project project;
    public ige_projectionDAL projection {get;set;}
    ge_projectDAL(ge_DbContext context,  ige_projectionDAL projection ) { 
            this.context = context;
            this.projection = projection; 
     }    
        public IEnumerable<ge_data> getProjectData(Guid projectId){return null;}
        public IEnumerable<ge_project> getOfficeProjects(Guid officeId){return null;}
        public ge_data getProjectById(Guid projectId){return null;}
        public void insertProject(ge_project project){}
        public void deleteProject(Guid projectId) {}
        public void updateProject(ge_project project) {}
        public void Save(){}
        public void Dispose(){}
        
        public string getUserOperations(string userId) {
        
        if (project != null) {
            if (project.users != null) {
                foreach  (ge_user_ops pu in project.users) {
                    if (pu.userId==userId) {
                        return pu.operations;
                    }
                } 
            }
        }
        
        if (project != null) {
            if (project.group != null) {
                if (project.group != null) {
                    foreach  (ge_user_ops pu in project.users) {
                        if (pu.userId==userId) {
                            return pu.operations;
                        }
                    }
                }
            } 
        }

        return "";
        }

        public void addUser(ge_user u, string operations) {
            
            if (project != null) {
                if (project.users != null) {
                    ge_user_ops os = new ge_user_ops();
                    os.user =u;
                    os.operations =operations;
                    project.users.Add (os); 
                }
            }
            

        }

        public ge_data addNewData() {
        if (project != null) {
            if (project.data !=null) {
                ge_data newData = new ge_data();
                newData.projectId = project.Id;
                newData.locEast = project.locEast;
                newData.locNorth = project.locNorth;
                newData.createdDT = DateTime.UtcNow; 
                project.data.Add (newData);
                return newData;
            }
        }
        return null;
        }
    }
}
