using System;
using System.Collections.Generic;
using ge_repository.Models;
using ge_repository.Authorization;

//https://social.technet.microsoft.com/wiki/contents/articles/36287.repository-pattern-in-asp-net-core.aspx?Redirected=true

namespace ge_repository.DAL
{
    public interface ige_dataDAL : IDisposable {
        IEnumerable<ge_data> getOfficeData(Guid officeId);
        IEnumerable<ge_data> getProjectData(Guid projectId);
        ge_data getDataById(Guid Id);
        ge_data_file getBigDataById(Guid Id);
        void insertData(ge_data data);
        void insertBigData(ge_data_file data);
        void deleteData(int dataId);
        void updateData(ge_data data);
        void Save();
    }

    public interface ige_projectionDAL : IDisposable {
        bool calcXYZ_fromLatLong();
        bool calcLatLongH_fromXYZ(); 
        bool calcEN_fromMapRef();  
        bool calcMapRef_fromEN(); 
        bool calcLatLong_fromEN(); 
        bool calcEN_fromLatLong(); 
        bool updateAll(string s1);
        string getMessage();
        Constants.datumProjection datumProjection();
    }

        public interface ige_groupDAL : IDisposable
        {
        IEnumerable<ge_data> getGroupData(Guid groupId);
        IEnumerable<ge_project> getGroupProjects(Guid groupId);
        ge_data getGroupById(Guid groupId);
        void insertGroup(ge_group group);
        void deleteGroup(Guid groupId);
        void updateGroup(ge_group group);
        void addUser (ge_user user, string operations);
        string getUserOperations(string UserId);
        void Save();
        }

        public interface ige_projectDAL : IDisposable
        {
        IEnumerable<ge_data> getProjectData(Guid projectId);
        IEnumerable<ge_project> getOfficeProjects(Guid officeId);
        ge_data getProjectById(Guid projectId);
        void insertProject(ge_project project);
        void deleteProject(Guid projectId);
        void updateProject(ge_project project);
        void addUser (ge_user user, string operations);
        void Save();
        string getUserOperations(string UserId);
        }

        public interface ige_userDAL : IDisposable
        {
        IEnumerable<ge_data> getUserData(string userId);
        IEnumerable<ge_project> getUserProjects(string userId);
        IEnumerable<ge_group> getUserOffices(string userId);

        ge_data getUserById(string userId);
        void insertUser(ge_user user);
        void deleteUser(string userId);
        void updateUser(ge_user user);
        ge_user addUser (string Email, string FirstName, string Surname);
        void Save();

    }
        public interface ige_eventDAL : IDisposable
        {
        IEnumerable<ge_event> getUserEvents(string userId);
        ge_event addEvent(string userId, string message,string Return_URL, logLEVEL Level);
        }

        public interface ige_transformDAL : IDisposable {
        IEnumerable<ge_transform> getOfficeTransform(Guid officeId);
        IEnumerable<ge_transform> getProjectTransform(Guid projectId);
        ge_transform getTransform(Guid Id);
        void insertTransform(ge_transform transform);
        void deleteTransform(Guid Id);
        void updateTransform(ge_transform transform);
        void Save();
    }
}
