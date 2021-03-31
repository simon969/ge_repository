#define SeedOnly
#if SeedOnly
using static ge_repository.Authorization.Constants;
using ge_repository.Authorization;
using ge_repository.Models;
using ge_repository.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using ge_repository.OtherDatabase;

namespace ge_repository.Data
{
    public static class SeedData
    {
        #region snippet_Initialize
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
             
            using (var context = new ge_DbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ge_DbContext>>()))
            {
                // For sample purposes we are seeding 2 users both with the same password.
                // The password is set with the following command:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                // SeedDB(context, @"C:\Users\ThomsonSJ\Documents\Visual Studio 2015\Projects\ge_repositoryAPI_SeedData");
                //  SeedLowerThamesCrossing(context);

                //FixLTC_OtherDatabase(context);
               //  FixUsers (context);
                var u6 = await EnsureUser(serviceProvider, testUserPw, "connor.caplen@aecom.com","Connor","Caplen","+441412021849");
                var u7= await EnsureUser(serviceProvider, testUserPw, "viktoria.nemeth@aecom.com","Viktoria","Nemeth","+447741566591");
            }
        }

        private static async Task EnsureUsers(IServiceProvider serviceProvider, string testUserPw) {

                var a1 = await EnsureUser(serviceProvider, testUserPw, "admin@aecom.com","Administrator","","+442086393568");
                await EnsureRole(serviceProvider, a1, Constants.ge_repositoryAdministratorRole);

                
                var m1 = await EnsureUser(serviceProvider, testUserPw, "tim.m.connolly@aecom.com","Tim","Connolly","+4402086393693");
                await EnsureRole(serviceProvider, m1, Constants.ge_repositoryManagerRole);
              
                var u1 = await EnsureUser(serviceProvider, testUserPw, "simon.thomson@aecom.com","Simon","Thomson","+442086393568");

                
                var m2 = await EnsureUser(serviceProvider, testUserPw, "mitesh.chandegra@aecom.com","Mitesh","Chandegra","+4402077985961");
                await EnsureRole(serviceProvider, m2, Constants.ge_repositoryManagerRole);
              
                var u2 = await EnsureUser(serviceProvider, testUserPw, "david.cheung@aecom.com","David","Cheung","+4402077985046");
                
               
                var m3 = await EnsureUser(serviceProvider, testUserPw, "aristeidis.zourmpakis@aecom.com","Aris","Zourmpakis","+441245771205");
                await EnsureRole(serviceProvider, m3, Constants.ge_repositoryManagerRole);
                                
                var u3 = await EnsureUser(serviceProvider, testUserPw, "lida.krimpeni@aecom.com","Lida","Krimpeni","+441245771210");
               
               
                var m4 = await EnsureUser(serviceProvider, testUserPw, "sarah.dagostino@aecom.com","Sarah","Dagostino","+4401256310515");
                await EnsureRole(serviceProvider, m4, Constants.ge_repositoryManagerRole);
                
                var u4 = await EnsureUser(serviceProvider, testUserPw, "pablo.bernardini@aecom.com","Pablo","Bernardini","+4402078214209");
               
                var m5 = await EnsureUser(serviceProvider, testUserPw, "paul.stewart@aecom.com","Paul","Stewart","+441727535638");
                await EnsureRole(serviceProvider, m3, Constants.ge_repositoryManagerRole);
                
                var u5 = await EnsureUser(serviceProvider, testUserPw, "charlie.chaplin@aecom.com","Charlie","Chaplin","+44208639356");

                var u6 = await EnsureUser(serviceProvider, testUserPw, "connor.caplen@aecom.com","Connor","Caplen","");
        }
        #endregion

        #region snippet_CreateRoles        
        private static async Task<string> getUserId (IServiceProvider serviceProvider, 
                                                    string UserName)
                                                    {
            var userManager = serviceProvider.GetService<UserManager<ge_user>>();

            var user = await userManager.FindByNameAsync(UserName);
            
            return user.Id;
            }
        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, 
                                                    string testUserPw, 
                                                    string email,
                                                    string firstname,
                                                    string lastname,
                                                    string phonenumber
                                                    )
        {
            var userManager = serviceProvider.GetService<UserManager<ge_user>>();

            var user = await userManager.FindByNameAsync(email);

            if (user == null)
            {
                user = new ge_user(firstname,lastname,email,phonenumber);
                var resp = await userManager.CreateAsync(user, testUserPw);

            }

            return user.Id;
        }
        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<ge_user>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }        
        #endregion
        
        public static void SeedDB2(ge_DbContext context, string source) {
            var u1 = context.Users.Where(user => user.UserName =="tim.m.connolly@aecom.com").Single();
            var u2 = context.Users.Where(user => user.UserName=="simon.thomson@aecom.com").Single();
            var u3 = context.Users.Where(a => a.UserName=="mitesh.chandegra@aecom.com").Single();      
            var u4 = context.Users.Where(a => a.UserName=="paul.stewart@aecom.com").Single(); 
            var u5 = context.Users.Where(a => a.UserName=="david.cheung@aecom.com").Single();
            var u6 = context.Users.Where(a => a.UserName=="sarah.dagostino@aecom.com").Single();;  
            var u7 = context.Users.Where(a => a.UserName=="pablo.bernardini@aecom.com").Single();;

            var g1 = new ge_group {
                Id = Guid.NewGuid(),
                name = "Primary Geotechnical Group",
                locName= "",
                createdDT = DateTime.Now,
                locAddress = "",
                locPostcode = "",
                datumProjection =  datumProjection.NONE,
                managerId = u4.Id,
                operations ="Read;",
                project_operations = "Create;Read;Update;Delete",
                projects = new List<ge_project>(), 
                users = new List<ge_user_ops>()
                };
            
            g1.users.Add(new ge_user_ops {userId = u1.Id,
                                             user_operations = "Read;Download;Create;Update;Delete;Approve;Admin"});
            g1.users.Add(new ge_user_ops {userId = u2.Id,
                                             user_operations = "Read;Download;Create;Update;Delete"});



        }
        
        public static void SeedDB(ge_DbContext context, string source)
        {
            if (context.ge_project.Any() && context.ge_data.Any())
            {
                 return;   // DB has been seeded
            } 
            
         try {          
            #region snippit_CreateOffices
            var manager1 = context.Users.Where(user => user.UserName =="tim.m.connolly@aecom.com").Single();
            var user1 = context.Users.Where(user => user.UserName=="simon.thomson@aecom.com").Single();
            
            var g1 = new ge_group {
                Id = Guid.NewGuid(),
                locName= "Croydon",
                createdDT =DateTime.Now,
                locAddress = "Sunley House, 4 Bedford Park, Surrey, Croydon",
                locPostcode = "CR0 2AP",
                datumProjection =  datumProjection.OSGB36NG,
                managerId = manager1.Id,
                operations ="Read;Update;Delete",
                project_operations = "Create;Read;Update;Delete",
                projects = new List<ge_project>(), 
                users = new List<ge_user_ops>()
                };
            
            g1.users.Add(new ge_user_ops {groupId= g1.Id,
                                             userId = manager1.Id,
                                             user_operations = "Read;Download;Create;Update;Delete;Approve;Admin"});
            g1.users.Add(new ge_user_ops {groupId=g1.Id,
                                             userId = user1.Id,
                                             user_operations = "Read;Download;Create;Update;Delete"});
            var manager2 = context.Users.Where(a => a.UserName=="mitesh.chandegra@aecom.com").Single();      
            var manager22 = context.Users.Where(a => a.UserName=="paul.stewart@aecom.com").Single(); 
            var user2 = context.Users.Where(a => a.UserName=="david.cheung@aecom.com").Single();

           var g2 = new ge_group {
                Id =  Guid.NewGuid(),
                locName= "Aldgate",
                createdDT =DateTime.Now,
                locAddress = "2 Leman Street, London",
                locPostcode = "E1 8FA",
                manager = manager2,
                datumProjection =  datumProjection.OSGB36NG,
                operations ="Read;Update;Delete",
                project_operations = "Create;Read;Update;Delete",
                projects = new List<ge_project>(), 
                users = new List<ge_user_ops>()
            };
            
            g2.users.Add(new ge_user_ops { userId = manager2.Id,
                                             user_operations = "Create;Read;Update;Delete;Reject;Approve;Admin"});
            g2.users.Add(new ge_user_ops { userId = user2.Id,
                                             user_operations = "Create;Read;Update;Delete"});
            g2.users.Add(new ge_user_ops { userId = manager22.Id,
                                             user_operations = "Create;Read;Update;Delete;Admin"});
        
           
            var manager3 = context.Users.Where(a => a.UserName=="aristeidis.zourmpakis@aecom.com").Single();
            var user3 = context.Users.Where(a => a.UserName=="lida.krimpeni@aecom.com").Single();
            
            var g3 = new ge_group {
                Id = Guid.NewGuid(),
                locName= "Chelmsford",
                createdDT =DateTime.Now,
                locAddress = "Saxon House, 27 Duke Street, Chelmsford",
                locPostcode = "CM1 1HT",
                managerId = manager3.Id,
                datumProjection =  datumProjection.OSGB36NG,
                operations ="Read;Update;Delete",
                project_operations = "Create;Read;Update;Delete",
                projects = new List<ge_project>(), 
                users = new List<ge_user_ops>()
            };
            
            g3.users.Add(new ge_user_ops {userId = manager3.Id,
                                             operations = "Create;Read;Update;Delete;Reject;Approve;Admin"});
            g3.users.Add(new ge_user_ops {userId = user3.Id,
                                             operations = "Create;Read;Update;Delete"});
 
            
            var manager4 = context.Users.Where(a => a.UserName=="sarah.dagostino@aecom.com").Single();;  
            var user4 = context.Users.Where(a => a.UserName=="pablo.bernardini@aecom.com").Single();;
            
            var g4 = new ge_group {
                Id = Guid.NewGuid(),
                locName= "Basingstoke",
                createdDT =DateTime.Now,
                locAddress = "Midpoint, Alencon Link, Hampshire, Basingstoke, ",
                locPostcode = "RG21 7PP",
                datumProjection =  datumProjection.OSGB36NG,
                managerId = manager4.Id,
                operations ="Read;Update;Delete",
                project_operations = "Read;Update;Delete",
                projects = new List<ge_project>(), 
                users = new List<ge_user_ops>()
            };

            g4.users.Add(new ge_user_ops {userId = manager4.Id,
                                             operations = "Create;Read;Update;Delete;Reject;Approve;Admin"});
            g4.users.Add(new ge_user_ops {userId = user4.Id,
                                             operations = "Create;Read;Update;Delete"});
            #endregion
            #region CreateProjects
            var p1 = new ge_project{
                    Id = Guid.NewGuid(),
                    locName = "Burton Upon Trent SWWM Appraisal",
                    locAddress = "Meadowside Drive, Burton, East Staffordshire, Staffordshire, West",
                    locPostcode = "DE14 1LD",
                    description = "Ground Investigation Data for Burton SWWM",
                    createdId = user1.Id,
                    keywords =" ",
                    start_date = new DateTime(2017,11,15),
                    createdDT = DateTime.Now,
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.ClientApproved,
                    locEast = 425388,
                    locNorth= 323033,
                    datumProjection =  datumProjection.OSGB36NG,
                    managerId = manager1.Id,
                    operations ="Read;Update;Delete",
                    data_operations = "Read;Create;Update;Delete",
                    data = new List<ge_data>(),
                    users = new List<ge_user_ops>()
            };

            p1.users.Add(new ge_user_ops {projectId=p1.Id,
                                             userId = manager1.Id,
                                             user_operations = "Create;Read;Update;Delete;Approve;Admin"});
            p1.users.Add(new ge_user_ops {projectId = p1.Id,
                                            userId = user1.Id,
                                            user_operations = "Create;Read;Update;Delete"});       

            p1.data.Add (new ge_data 
                {
                    createdId = user1.Id,
                    createdDT = DateTime.Now,
                    filename = "E7037A_Burton SWWM_Final Factual Report AGS4.ags",
                    fileext = ".ags", 
                    filetype ="text/plain",
                    filesize =948423,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.ClientApproved,
                    version= "P01.1",
                    vstatus= VersionStatus.Intermediate,
                    qstatus = QualitativeStatus.AECOMFactual,
                    description="AGS data downloaded from BGS website",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_string = File.ReadAllText(source + @"\E7037A_Burton SWWM\E7037A_Burton SWWM_Final Factual Report AGS4.ags")
                    }
                }
            );
            
          var p11 = new ge_project{
                    Id = Guid.NewGuid(),
                    name = "AGS Dictionary",
                    description = "Developement of AGS Reference Dictionary for converting from AGS to XML and SQL data formats",
                    createdId = user1.Id,
                    keywords ="ags, xml ",
                    start_date = new DateTime(2017,11,15),
                    createdDT = DateTime.Now,
                    pstatus = PublishStatus.Approved,
                    cstatus = ConfidentialityStatus.Owned,
                    locEast = 529565,
                    locNorth= 177615,
                    datumProjection =  datumProjection.OSGB36NG,
                    managerId = manager1.Id,
                    operations ="Read;Update;Delete",
                    data_operations = "Read;Update;Delete",
                    data = new List<ge_data>(),
                    users = new List<ge_user_ops>()
            };

            p11.users.Add(new ge_user_ops {projectId=p1.Id,
                                             userId = manager1.Id,
                                             user_operations= "Create;Read;Update;Delete;Approve;Admin"});
            p11.users.Add(new ge_user_ops {projectId = p1.Id,
                                            userId = user1.Id,
                                            user_operations = "Create;Read;Update;Delete"});       

            p11.data.Add (new ge_data 
                {
                    createdId = user1.Id,
                    createdDT = DateTime.Now,
                    filename = "DictionaryAgsml_0.0.14.xml",
                    fileext = ".xml", 
                    filetype ="text/plain",
                    filesize =948423,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Approved,
                    cstatus = ConfidentialityStatus.Owned,
                    version= "P01.1",
                    vstatus= VersionStatus.Final,
                    qstatus = QualitativeStatus.AECOMFactual,
                    description="AGS data dictionary",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_string = File.ReadAllText(source + @"\AGS Dictionary\DictionaryAgsml_0.0.14.xml")
                    }
                }
            );
             p11.data.Add (new ge_data 
                {
                    createdId = user1.Id,
                    createdDT = DateTime.Now,
                    filename = "DictionaryAgsml.xsl",
                    fileext = ".xsl", 
                    filetype ="text/plain",
                    filesize =2582,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Approved,
                    cstatus = ConfidentialityStatus.Owned,
                    version= "P01.1",
                    vstatus= VersionStatus.Final,
                    qstatus = QualitativeStatus.AECOMFactual,
                    description="AGS xml data template",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_string = File.ReadAllText(source + @"\AGS Dictionary\DictionaryAgsml.xsl")
                    }
                }
            );

            var p2 = new ge_project{
                name = "Tideway C410 HEAPS",
                description = "Ground Invetsigation Data close to the CSO shaft HEAPS",
                createdId = user2.Id,
                keywords ="London Clay;London Basin;Lambeth Group;Pressuremeter;Small Strain",
                start_date = new DateTime(2017,11,15),
                createdDT = DateTime.Now,
                pstatus = PublishStatus.Uncontrolled,
                cstatus = ConfidentialityStatus.ClientApproved,
                locEast = 529565,
                locNorth= 177615,
                managerId = manager2.Id,
                operations ="Read;Update;Delete",
                data_operations = "Read;Update;Delete",
                data = new List<ge_data>(),
                users = new List<ge_user_ops>()
                };
            
            p2.users.Add(new ge_user_ops {projectId = p2.Id,
                                             userId = manager2.Id,
                                             user_operations = "Create;Read;Update;Delete;Reject;Approve;Admin"});
            p2.users.Add(new ge_user_ops {projectId = p2.Id,
                                             userId = user2.Id,
                                             user_operations = "Create;Read;Update;Delete"});
            p2.users.Add(new ge_user_ops {projectId = p2.Id,
                                             userId = manager22.Id,
                                             user_operations = "Create;Read;Update;Delete;Approve;Admin"});                                 
            
            p2.data.Add (new ge_data 
                    {
                    createdId = user2.Id,
                    createdDT = DateTime.Now,
                    filename = "heaps_boreholes_3d_03-Geological Plan2.pdf",
                    fileext = ".pdf", 
                    filetype = "application/pdf",
                    filesize = 2177973,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.ClientApproved,
                    version= "P01.1",
                    vstatus= VersionStatus.Intermediate,
                    qstatus = QualitativeStatus.AECOMFactual,
                    description="Surfaces created from gINT Civil Tools for HEAPS",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_binary = File.ReadAllBytes(source + @"\Tideway HEAPS\heaps_boreholes_3d_03-Geological Plan.pdf")
                    }
                    }
                );

             p2.data.Add (new ge_data 
                    {
                    createdId = user2.Id,
                    createdDT = DateTime.Now,
                    filename = "4602-FLOJV-HEAPS-180-GG-MD-900001.gpj",
                    fileext = ".gpj",
                    filetype = "application/gINT", 
                    filesize = 19427328,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.ClientApproved,
                    version= "P01.1",
                    vstatus= VersionStatus.Intermediate,
                    qstatus = QualitativeStatus.AECOMFactual,
                    description="Tideway database exports of gINT database for HEAPS local GI data",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_binary = File.ReadAllBytes(source + @"\Tideway HEAPS\4602-FLOJV-HEAPS-180-GG-MD-900001.gpj")
                    }
                    }
                );
            
       
            var p3 = new ge_project {
                locName = "Ryhad Metro",
                name ="Ryad Metro",
                description = "Bridge 1-5 1A1P30",
                createdId = user3.Id,
                keywords ="Rock sockets",
                start_date = new DateTime(2018,06,15),
                createdDT = DateTime.Now,
                pstatus = PublishStatus.Uncontrolled,
                cstatus = ConfidentialityStatus.ClientApproved,
                datumProjection =  datumProjection.OSGB36NG,
                operations ="Read;Update;Delete",
                data_operations = "Read;Update;Delete",
                data = new List<ge_data>(),
                users = new List<ge_user_ops>()
            };
            
            p3.users.Add(new ge_user_ops {projectId = p3.Id,
                                             userId = manager3.Id,
                                             user_operations = "Create;Read;Update;Delete;Reject;Approve;Admin"});
            p3.users.Add(new ge_user_ops {projectId = p3.Id,
                                             userId = user3.Id,
                                             user_operations = "Create;Read;Update;Delete"});
            p3.data.Add (new ge_data 
                    {
                    createdId = user3.Id,
                    createdDT = DateTime.Now,
                    filename="M-BD4-1A1VDA-CSVD-EDR-100532_Updated with Orientation.dwg",
                    fileext = ".dwg", 
                    filetype = "application/autocad",
                    filesize = 19427328,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.ClientApproved,
                    version= "P01.1",
                    vstatus= VersionStatus.Intermediate,
                    qstatus = QualitativeStatus.AECOMFactual,
                    description="Tideway database exports of gINT database for HEAPS local GI data",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_binary = File.ReadAllBytes(source + @"\Ryhad Metro\M-BD4-1A1VDA-CSVD-EDR-100532_Updated with Orientation.dwg")
                    }
            });
            p3.data.Add (new ge_data 
                    {
                    createdId = user3.Id,
                    createdDT = DateTime.Now,
                    filename="1A1P30 and 1A1P31 Plaxis Embedded Pile Results 2017-03-09.xlsx",
                    fileext = ".xls", 
                    filetype = "application/excel",
                    filesize = 19427328,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.ClientApproved,
                    version= "P01.1",
                    vstatus= VersionStatus.Intermediate,
                    qstatus = QualitativeStatus.AECOMFactual,
                    description="Results from Plaxis Analysis",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_binary = File.ReadAllBytes(source + @"\Ryhad Metro\1A1P30 and 1A1P31 Plaxis Embedded Pile Results 2017-03-09.xlsx")
                    }
            }
            );

            var p31 = new ge_project {
                locName = "Silvertown Tunnel",
                name ="Silvertown Tunnel Development Tender Phase",
                description = "Tender design for Silvertown Tunnel",
                createdId = user3.Id,
                keywords ="Road Tunnel;Lambeth Group;London;Infrastructure;",
                start_date = new DateTime(2018,06,15),
                createdDT = DateTime.Now,
                pstatus = PublishStatus.Uncontrolled,
                cstatus = ConfidentialityStatus.ClientApproved,
                operations ="Read;Update;Delete",
                data_operations = "Read;Update;Delete",
                data = new List<ge_data>(),
                users = new List<ge_user_ops>()
            };
            
            p31.users.Add(new ge_user_ops {projectId = p3.Id,
                                             userId = manager3.Id,
                                             user_operations = "Create;Read;Update;Delete;Reject;Approve;Admin"});
            p31.users.Add(new ge_user_ops {projectId = p3.Id,
                                             userId = user3.Id,
                                             user_operations = "Create;Read;Update;Delete"});
            
            p31.data.Add (new ge_data 
                    {
                    createdId = user3.Id,
                    createdDT = DateTime.Now,
                    filename="20110770 - 2016-03-03 1540 - Final - 2.ags",
                    fileext = ".ags", 
                    filetype = "text/plain",
                    filesize = 19787,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Approved,
                    cstatus = ConfidentialityStatus.RequiresClientApproval,
                    version= "P01.1",
                    folder= "tender-data",
                    vstatus= VersionStatus.Final,
                    qstatus = QualitativeStatus.ThirdPartyFactual,
                    description="Silvertown AGS data recieved at tender",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_binary = File.ReadAllBytes(source + @"\Silvertown Tunnel\20110770 - 2016-03-03 1540 - Final - 2.ags")
                    }
            }
            );
            p31.data.Add (new ge_data 
                    {
                    createdId = user3.Id,
                    createdDT = DateTime.Now,
                    filename="TA7510F17.ags",
                    fileext = ".ags", 
                    filetype = "text/plain",
                    filesize = 8479000,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Approved,
                    cstatus = ConfidentialityStatus.RequiresClientApproval,
                    version= "P01.1",
                    folder= "tender_data",
                    vstatus= VersionStatus.Final,
                    qstatus = QualitativeStatus.ThirdPartyFactual,
                    description="Silvertown additional AGS data recieved at tender",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_binary = File.ReadAllBytes(source + @"\Silvertown Tunnel\TA7510F17.ags")
                    }
            }
            );      
            p31.data.Add (new ge_data 
                    {
                    createdId = user3.Id,
                    createdDT = DateTime.Now,
                    filename="SilvertownGI.xml",
                    fileext = ".xml", 
                    filetype = "text/plain",
                    filesize = 5653000,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Approved,
                    cstatus = ConfidentialityStatus.RequiresClientApproval,
                    version= "P01.1",
                    folder= "tender_data",
                    vstatus= VersionStatus.Final,
                    qstatus = QualitativeStatus.ThirdPartyFactual,
                    description="Silvertown additional AGS data recieved at tender",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_binary = File.ReadAllBytes(source + @"\Silvertown Tunnel\SilvertownGI.xml")
                    }
            }
            );

            var p4 = new ge_project {
                locName = "West Midlands to Crewe",
                name = "High Speed 2 Package 2A",
                description = "Historic Ground Investigation Data fro HS2 Package 2A",
                createdId = user4.Id,
                keywords ="Mercia Mudstone;Earthworks;Monitoring",
                start_date = new DateTime(2018,06,15),
                createdDT = DateTime.Now,
                pstatus = PublishStatus.Uncontrolled,
                cstatus = ConfidentialityStatus.ClientApproved,
                data = new List<ge_data>(),
                users = new List<ge_user_ops>(),
                operations ="Read;Update;Delete",
                data_operations = "Read;Update;Delete"
            };
            
            p4.users.Add(new ge_user_ops {projectId = p4.Id,
                                             userId = manager4.Id,
                                             user_operations = "Create;Read;Update;Delete;Reject;Approve;Admin"});
            p4.users.Add(new ge_user_ops {projectId = p4.Id,
                                             userId = user4.Id,
                                             user_operations = "Create;Read;Update;Delete"});

            p4.data.Add (new ge_data 
                    {
                    filename="HS2 PH2B AGS EXPORT 2018-12-18.ags",
                    fileext = ".ags",
                    filetype = "text/plain",
                    createdId =user4.Id,
                    createdDT = DateTime.Now,  
                    filesize = 2183000,
                    filedate = new DateTime(2018,06,28),
                    locEast = 425388.43,
                    locNorth = 323032.67,
                    pstatus = PublishStatus.Approved,
                    cstatus = ConfidentialityStatus.RequiresClientApproval,
                    version= "P01.1",
                    vstatus= VersionStatus.Intermediate,
                    qstatus = QualitativeStatus.ThirdPartyFactual,
                    description="Routewide historical borehole data compiled from BGS",
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_binary = File.ReadAllBytes(source + @"\HS2\HS2 PH2B AGS EXPORT 2018-12-18.ags")
                    }
            }
            );

            #endregion
            #region AddProjectsToOffices

            g1.projects.Add (p1);
            g1.projects.Add (p11);

            g2.projects.Add (p2);
            
            g3.projects.Add (p3);
            g3.projects.Add (p31);
            
            g4.projects.Add (p4);

            context.ge_group.Add (g1);
            context.ge_group.Add (g2);
            context.ge_group.Add (g3);
            context.ge_group.Add (g4);

//            var user5 = context.Users.Where(a => a.UserName=="charlie.chaplin@aecom.com").Single();
//            var admin = context.Users.Where(a => a.UserName=="admin@contoso.com").Single();       

            context.SaveChanges();
            #endregion

            }   catch (Exception e) {
              Console.WriteLine(e.Message); 
         }
        } 
        public static void AddFile (ge_project project, string filename, ge_user user) {

            if (File.Exists(filename)) {
                
                System.IO.FileInfo fi = new FileInfo(filename);

                project.data.Add (new ge_data 
                {
                    createdId = user.Id,
                    createdDT = DateTime.Now,
                    filename = fi.Name,
                    fileext = fi.Extension, 
                    filetype ="text/plain",
                    filesize = fi.Length,
                    filedate = fi.LastWriteTime,
                    encoding = "ascii",
                    datumProjection = datumProjection.NONE,
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.RequiresClientApproval,
                    version= "P01.1",
                    vstatus= VersionStatus.Intermediate,
                    qstatus = QualitativeStatus.AECOMFactual,
                    description=fi.Name,
                    operations ="Read;Update;Delete",
                    file = new ge_data_file {
                        data_string = File.ReadAllText(filename)
                    }
                }
            );

            }
        }
        public static void FixUsers(ge_DbContext context) {
        
            var user1 = context.Users.Where(a => a.UserName=="WeiJian.Ng@aecom.com").Single();
            user1.FirstName = "WeiJian";
            user1.LastName = "Ng";
            context.SaveChanges(); 

         
        }
        public static void FixLTC_OtherDatabase(ge_DbContext context) {

        dbConnectDetails cd1 = new dbConnectDetails() {
                Type = gINTTables.DB_DATA_TYPE, 
                Server = "EUDTC1PSQLW001",
                Database ="ltc_ags4",
                UserId = "ltc_ags4",
                Password = "9Lz+j#yEfa$P6*&S",
                ProjectId = 3
        };
         
         dbConnectDetails cd2 = new dbConnectDetails() {
                Type = logTables.DB_DATA_TYPE,
                Server = "EUDTC1PSQLW001",
                Database ="ltc_monitoring",
                UserId = "ltc_monitoring",
                Password = "Wu8Nx+c7?FTEHz5u",
            };   
            
            context.SaveChanges();
        }
        public static void SeedLowerThamesCrossing(ge_DbContext context) {
          var manager1 = context.Users.Where(a => a.UserName=="simon.thomson@aecom.com").Single();      
            var user1 = context.Users.Where(a => a.UserName=="WeiJian.Ng@aecom.com").Single();
           
            var g1 = new ge_group {
                Id = Guid.NewGuid(),
                locName= "Lower Thames Crossing Ground Investigation",
                createdDT =DateTime.Now,
                locAddress = "",
                locPostcode = "",
                datumProjection =  datumProjection.NONE,
                managerId = manager1.Id,
                operations ="Read;Update;Delete",
                project_operations = "Create;Read;Update;Delete",
                projects = new List<ge_project>(), 
                users = new List<ge_user_ops>()
                };
            
            g1.users.Add(new ge_user_ops {groupId = g1.Id,
                                             userId = manager1.Id,
                                             user_operations = "Read;Download;Create;Update;Delete;Approve;Admin"});
            g1.users.Add(new ge_user_ops {groupId = g1.Id,
                                             userId = user1.Id,
                                             user_operations = "Read;Download;Create;Update;Delete"});
           
           dbConnectDetails cd1 = new dbConnectDetails(){
                        Type = gINTTables.DB_DATA_TYPE,
                        Server = "EUDTC1PSQLW001",
                        Database ="ltc_ags4",
                        DataSource = "172.17.136.103",
                        UserId = "ltc_ags4",
                        Password = "9Lz+j#yEfa$P6*&S"
            };

            #region CreateProjects
            var p1 = new ge_project{
                    Id = Guid.NewGuid(),
                    locName = "Lower Thames Crossing Package A",
                    locAddress = "",
                    locPostcode = "",
                    description = "Monitoring Data for Lower Thames Crossing Package A",
                    keywords = "", 
                    createdId = user1.Id, 
                    createdDT = DateTime.Now, 
                    editedId=user1.Id,
                    editedDT = DateTime.Now,
                    start_date = new DateTime(2017,11,15),
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.RequiresClientApproval,
                    datumProjection =  datumProjection.NONE,
                    managerId = manager1.Id,
                    operations ="Read;Update;Delete",
                    data_operations = "Read;Create;Update;Delete",
                   data = new List<ge_data>(),
                    users = new List<ge_user_ops>()
            };
      
            p1.users.Add(new ge_user_ops {projectId=p1.Id,
                                             userId = manager1.Id,
                                             user_operations = "Create;Read;Update;Delete;Approve;Admin"});
            p1.users.Add(new ge_user_ops {projectId = p1.Id,
                                            userId = user1.Id,
                                            user_operations = "Create;Read;Update;Delete"});       

                       
            var p2 = new ge_project{
                    Id = Guid.NewGuid(),
                    locName = "Lower Thames Crossing Package B",
                    locAddress = "",
                    locPostcode = "",
                    description = "Monitoring Data for Lower Thames Crossing Package B",
                    createdId = user1.Id, 
                    createdDT = DateTime.Now, 
                    editedId=user1.Id,
                    editedDT = DateTime.Now,
                    keywords ="",
                    start_date = new DateTime(2017,11,15),
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.Owned,
                    datumProjection =  datumProjection.NONE,
                    managerId = manager1.Id,
                    operations ="Read;Update;Delete",
                    data_operations = "Read;Update;Delete",
                    data = new List<ge_data>(),
                    users = new List<ge_user_ops>()
            };
            p2.users.Add(new ge_user_ops {projectId=p1.Id,
                                             userId = manager1.Id,
                                             user_operations= "Create;Read;Update;Delete;Approve;Admin"});
            p2.users.Add(new ge_user_ops {projectId = p1.Id,
                                            userId = user1.Id,
                                            user_operations = "Create;Read;Update;Delete"});       
            
            var p3 = new ge_project{
                    Id = Guid.NewGuid(),
                    locName = "Lower Thames Crossing Package C",
                    locAddress = "",
                    locPostcode = "",
                    description = "Monitoring Data for Lower Thames Crossing Package C",
                    createdId = user1.Id, 
                    createdDT = DateTime.Now, 
                    editedId=user1.Id,
                    editedDT = DateTime.Now,
                    keywords ="",
                    start_date = new DateTime(2017,11,15),
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.Owned,
                    datumProjection =  datumProjection.NONE,
                    managerId = manager1.Id,
                    operations ="Read;Update;Delete",
                    data_operations = "Read;Update;Delete",
                    data = new List<ge_data>(),
                    users = new List<ge_user_ops>()
            };
      
            p3.users.Add(new ge_user_ops {projectId=p3.Id,
                                             userId = manager1.Id,
                                             user_operations= "Create;Read;Update;Delete;Approve;Admin"});
            p3.users.Add(new ge_user_ops {projectId = p3.Id,
                                            userId = user1.Id,
                                            user_operations = "Create;Read;Update;Delete"});       
            
            string folder = @"\\eu.aecomnet.com\euprojectvol\UKCRD1-TI\Projects\14\geotech1\Projects\Lower Thames Crossing\Package C\02 WIP\LTM Data Round0";
            
            AddFile(p3,folder + "\\" + "Logger_Data 2019-11-14 to 2019-12-16.csv", user1);
            AddFile(p3,folder + "\\" + "Package B Round0 Long Term Monitoring data 2020-01-31.csv", user1);
            
            
            var p4 = new ge_project{
                    Id = Guid.NewGuid(),
                    locName = "Lower Thames Crossing Package D",
                    locAddress = "",
                    locPostcode = "",
                    description = "Monitoring Data for Lower Thames Crossing Package D",
                    createdId = user1.Id, 
                    createdDT = DateTime.Now, 
                    editedId=user1.Id,
                    editedDT = DateTime.Now,
                    keywords ="",
                    start_date = new DateTime(2017,11,15),
                    pstatus = PublishStatus.Uncontrolled,
                    cstatus = ConfidentialityStatus.Owned,
                    datumProjection =  datumProjection.NONE,
                    managerId = manager1.Id,
                    operations ="Read;Update;Delete",
                    data_operations = "Read;Update;Delete",
                    data = new List<ge_data>(),
                    users = new List<ge_user_ops>()
            };
      
            p4.users.Add(new ge_user_ops {projectId=p4.Id,
                                             userId = manager1.Id,
                                             user_operations= "Create;Read;Update;Delete;Approve;Admin"});
            p4.users.Add(new ge_user_ops {projectId = p4.Id,
                                            userId = user1.Id,
                                            user_operations = "Create;Read;Update;Delete"});  
             #endregion
            
            g1.projects.Add (p1);
            g1.projects.Add (p2);
            g1.projects.Add (p3);
            g1.projects.Add (p4);

            context.ge_group.Add (g1);
            context.SaveChanges();  
    } 


  
    }

}
#endif