#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "322ad7823d9673d2ce4664aa473f21c21518e538"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ge_log_ReadData), @"mvc.1.0.view", @"/Views/ge_log/ReadData.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/ge_log/ReadData.cshtml", typeof(AspNetCore.Views_ge_log_ReadData))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\_ViewImports.cshtml"
using ge_repository.Extensions;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"322ad7823d9673d2ce4664aa473f21c21518e538", @"/Views/ge_log/ReadData.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9430c914460544b2bbe4f0e24f2bead87399a60e", @"/Views/_ViewImports.cshtml")]
    public class Views_ge_log_ReadData : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ge_repository.OtherDatabase.ge_log_file>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(48, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
  
    Layout = "~/Pages/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(104, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(179, 61, true);
            WriteLiteral("\r\n<h2>Details</h2>\r\n\r\n<div>\r\n    <h4>Logger File</h4>\r\n      ");
            EndContext();
            BeginContext(241, 84, false);
#line 15 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
 Write(Html.ActionLink("View Original File", "View", "ge_data",  new { id = Model.dataId }));

#line default
#line hidden
            EndContext();
            BeginContext(325, 85, true);
            WriteLiteral(" \r\n    <h1> </h1>\r\n<hr />\r\n    <dl class=\"dl-horizontal\">\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(411, 46, false);
#line 20 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayNameFor(model => model.fileHeader));

#line default
#line hidden
            EndContext();
            BeginContext(457, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(501, 42, false);
#line 23 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayFor(model => model.fileHeader));

#line default
#line hidden
            EndContext();
            BeginContext(543, 45, true);
            WriteLiteral("\r\n        </dd>  \r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(589, 47, false);
#line 26 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayNameFor(model => model.fieldHeader));

#line default
#line hidden
            EndContext();
            BeginContext(636, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(680, 43, false);
#line 29 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayFor(model => model.fieldHeader));

#line default
#line hidden
            EndContext();
            BeginContext(723, 45, true);
            WriteLiteral("\r\n        </dd>  \r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(769, 53, false);
#line 32 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayNameFor(model => model.readingAggregates));

#line default
#line hidden
            EndContext();
            BeginContext(822, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(866, 49, false);
#line 35 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayFor(model => model.readingAggregates));

#line default
#line hidden
            EndContext();
            BeginContext(915, 99, true);
            WriteLiteral("\r\n        </dd> \r\n        <dt>\r\n            Save Status\r\n        </dt>\r\n        <dd> \r\n            ");
            EndContext();
            BeginContext(1015, 18, false);
#line 41 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(ViewBag.fileStatus);

#line default
#line hidden
            EndContext();
            BeginContext(1033, 118, true);
            WriteLiteral("\r\n        </dd>  \r\n    </dl>\r\n    <table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1152, 56, false);
#line 48 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.file_headers[0].name));

#line default
#line hidden
            EndContext();
            BeginContext(1208, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1264, 63, false);
#line 51 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.file_headers[0].search_text));

#line default
#line hidden
            EndContext();
            BeginContext(1327, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1383, 57, false);
#line 54 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.file_headers[0].value));

#line default
#line hidden
            EndContext();
            BeginContext(1440, 67, true);
            WriteLiteral("\r\n            </th>\r\n        </tr>\r\n    </thead>    \r\n    <tbody>\r\n");
            EndContext();
#line 59 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
     foreach (var header in Model.file_headers) {

#line default
#line hidden
            BeginContext(1558, 48, true);
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(1607, 41, false);
#line 62 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.name));

#line default
#line hidden
            EndContext();
            BeginContext(1648, 40, true);
            WriteLiteral("\r\n            </td>\r\n             <td>\r\n");
            EndContext();
#line 65 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                 if(header.search_text!=null) {
                

#line default
#line hidden
            BeginContext(1754, 48, false);
#line 66 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.search_text));

#line default
#line hidden
            EndContext();
#line 66 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                                                                 
                }

#line default
#line hidden
            BeginContext(1823, 54, true);
            WriteLiteral("            </td>\r\n             <td>\r\n                ");
            EndContext();
            BeginContext(1878, 42, false);
#line 70 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.value));

#line default
#line hidden
            EndContext();
            BeginContext(1920, 36, true);
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
            EndContext();
#line 73 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
    }

#line default
#line hidden
            BeginContext(1963, 116, true);
            WriteLiteral("    </tbody>\r\n    </table>\r\n    <table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2080, 55, false);
#line 80 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].id));

#line default
#line hidden
            EndContext();
            BeginContext(2135, 56, true);
            WriteLiteral("\r\n            </th>\r\n             <th>\r\n                ");
            EndContext();
            BeginContext(2192, 57, false);
#line 83 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].name));

#line default
#line hidden
            EndContext();
            BeginContext(2249, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2305, 60, false);
#line 86 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].db_name));

#line default
#line hidden
            EndContext();
            BeginContext(2365, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2421, 59, false);
#line 89 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].source));

#line default
#line hidden
            EndContext();
            BeginContext(2480, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2536, 58, false);
#line 92 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].units));

#line default
#line hidden
            EndContext();
            BeginContext(2594, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2650, 61, false);
#line 95 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].comments));

#line default
#line hidden
            EndContext();
            BeginContext(2711, 67, true);
            WriteLiteral("\r\n            </th>\r\n        </tr>\r\n    </thead>    \r\n    <tbody>\r\n");
            EndContext();
#line 100 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
     foreach (var header in Model.field_headers) {

#line default
#line hidden
            BeginContext(2830, 48, true);
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2879, 39, false);
#line 103 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.id));

#line default
#line hidden
            EndContext();
            BeginContext(2918, 56, true);
            WriteLiteral("\r\n            </td>\r\n             <td>\r\n                ");
            EndContext();
            BeginContext(2975, 41, false);
#line 106 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.name));

#line default
#line hidden
            EndContext();
            BeginContext(3016, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3072, 44, false);
#line 109 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.db_name));

#line default
#line hidden
            EndContext();
            BeginContext(3116, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3172, 43, false);
#line 112 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.source));

#line default
#line hidden
            EndContext();
            BeginContext(3215, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3271, 42, false);
#line 115 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.units));

#line default
#line hidden
            EndContext();
            BeginContext(3313, 58, true);
            WriteLiteral("\r\n            </td>\r\n               <td>\r\n                ");
            EndContext();
            BeginContext(3372, 45, false);
#line 118 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.comments));

#line default
#line hidden
            EndContext();
            BeginContext(3417, 36, true);
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
            EndContext();
#line 121 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
    }

#line default
#line hidden
            BeginContext(3460, 30, true);
            WriteLiteral("    </tbody>\r\n    </table>\r\n\r\n");
            EndContext();
#line 125 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
     if (Model.file_array != null) {

#line default
#line hidden
            BeginContext(3528, 112, true);
            WriteLiteral("        <table class=\"table\">\r\n            <thead>\r\n            <tr>\r\n                <th>\r\n                    ");
            EndContext();
            BeginContext(3641, 54, false);
#line 130 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
               Write(Html.DisplayNameFor(model => model.file_array[0].name));

#line default
#line hidden
            EndContext();
            BeginContext(3695, 25, true);
            WriteLiteral("\r\n                </th>\r\n");
            EndContext();
#line 132 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                     foreach(var value in Model.file_array[0].values) {

#line default
#line hidden
            BeginContext(3793, 46, true);
            WriteLiteral("                    <th>\r\n                    ");
            EndContext();
            BeginContext(3840, 39, false);
#line 134 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
               Write(Html.DisplayNameFor(modelItem => value));

#line default
#line hidden
            EndContext();
            BeginContext(3879, 29, true);
            WriteLiteral("\r\n                    </th>\r\n");
            EndContext();
#line 136 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                    }

#line default
#line hidden
            BeginContext(3931, 66, true);
            WriteLiteral("            </tr>\r\n            </thead>    \r\n            <tbody>\r\n");
            EndContext();
#line 140 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                 foreach (var row in Model.file_array) { 

#line default
#line hidden
            BeginContext(4056, 84, true);
            WriteLiteral("                    <tr>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(4141, 38, false);
#line 143 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                       Write(Html.DisplayFor(modelItem => row.name));

#line default
#line hidden
            EndContext();
            BeginContext(4179, 33, true);
            WriteLiteral("\r\n                        </td>\r\n");
            EndContext();
#line 145 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                         foreach (var value in row.values) { 

#line default
#line hidden
            BeginContext(4275, 58, true);
            WriteLiteral("                        <td>\r\n                            ");
            EndContext();
            BeginContext(4334, 35, false);
#line 147 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                       Write(Html.DisplayFor(modelItem => value));

#line default
#line hidden
            EndContext();
            BeginContext(4369, 33, true);
            WriteLiteral("\r\n                        </td>\r\n");
            EndContext();
#line 149 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                        }

#line default
#line hidden
            BeginContext(4429, 27, true);
            WriteLiteral("                    </tr>\r\n");
            EndContext();
#line 151 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                   
                }

#line default
#line hidden
            BeginContext(4496, 40, true);
            WriteLiteral("            </tbody>\r\n        </table>\r\n");
            EndContext();
#line 155 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
    }

#line default
#line hidden
            BeginContext(4543, 84, true);
            WriteLiteral("<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(4628, 63, false);
#line 160 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].ReadingDatetime));

#line default
#line hidden
            EndContext();
            BeginContext(4691, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(4747, 56, false);
#line 163 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Duration));

#line default
#line hidden
            EndContext();
            BeginContext(4803, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(4859, 54, false);
#line 166 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value1));

#line default
#line hidden
            EndContext();
            BeginContext(4913, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(4969, 54, false);
#line 169 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value2));

#line default
#line hidden
            EndContext();
            BeginContext(5023, 56, true);
            WriteLiteral("\r\n            </th>\r\n             <th>\r\n                ");
            EndContext();
            BeginContext(5080, 54, false);
#line 172 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value3));

#line default
#line hidden
            EndContext();
            BeginContext(5134, 56, true);
            WriteLiteral("\r\n            </th>\r\n             <th>\r\n                ");
            EndContext();
            BeginContext(5191, 54, false);
#line 175 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value4));

#line default
#line hidden
            EndContext();
            BeginContext(5245, 56, true);
            WriteLiteral("\r\n            </th> \r\n            <th>\r\n                ");
            EndContext();
            BeginContext(5302, 54, false);
#line 178 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value5));

#line default
#line hidden
            EndContext();
            BeginContext(5356, 56, true);
            WriteLiteral("\r\n            </th>\r\n             <th>\r\n                ");
            EndContext();
            BeginContext(5413, 54, false);
#line 181 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value6));

#line default
#line hidden
            EndContext();
            BeginContext(5467, 58, true);
            WriteLiteral("\r\n            </th>   \r\n            <th>\r\n                ");
            EndContext();
            BeginContext(5526, 54, false);
#line 184 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value7));

#line default
#line hidden
            EndContext();
            BeginContext(5580, 21, true);
            WriteLiteral("\r\n            </th>\r\n");
            EndContext();
#line 186 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
             if (Model.field_headers.Count > 7) {

#line default
#line hidden
            BeginContext(5652, 34, true);
            WriteLiteral("            <th>\r\n                ");
            EndContext();
            BeginContext(5687, 54, false);
#line 188 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value8));

#line default
#line hidden
            EndContext();
            BeginContext(5741, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(5797, 54, false);
#line 191 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value9));

#line default
#line hidden
            EndContext();
            BeginContext(5851, 56, true);
            WriteLiteral("\r\n            </th>\r\n             <th>\r\n                ");
            EndContext();
            BeginContext(5908, 55, false);
#line 194 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value10));

#line default
#line hidden
            EndContext();
            BeginContext(5963, 56, true);
            WriteLiteral("\r\n            </th>\r\n             <th>\r\n                ");
            EndContext();
            BeginContext(6020, 55, false);
#line 197 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value11));

#line default
#line hidden
            EndContext();
            BeginContext(6075, 56, true);
            WriteLiteral("\r\n            </th> \r\n            <th>\r\n                ");
            EndContext();
            BeginContext(6132, 55, false);
#line 200 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value12));

#line default
#line hidden
            EndContext();
            BeginContext(6187, 56, true);
            WriteLiteral("\r\n            </th>\r\n             <th>\r\n                ");
            EndContext();
            BeginContext(6244, 55, false);
#line 203 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value13));

#line default
#line hidden
            EndContext();
            BeginContext(6299, 58, true);
            WriteLiteral("\r\n            </th>   \r\n            <th>\r\n                ");
            EndContext();
            BeginContext(6358, 55, false);
#line 206 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value14));

#line default
#line hidden
            EndContext();
            BeginContext(6413, 21, true);
            WriteLiteral("\r\n            </th>\r\n");
            EndContext();
#line 208 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
            }

#line default
#line hidden
            BeginContext(6449, 42, true);
            WriteLiteral("        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
            EndContext();
#line 212 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
     foreach (var reading in Model.readings) {

#line default
#line hidden
            BeginContext(6539, 48, true);
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(6588, 53, false);
#line 215 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.ReadingDatetime));

#line default
#line hidden
            EndContext();
            BeginContext(6641, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(6697, 46, false);
#line 218 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Duration));

#line default
#line hidden
            EndContext();
            BeginContext(6743, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(6799, 44, false);
#line 221 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value1));

#line default
#line hidden
            EndContext();
            BeginContext(6843, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(6899, 44, false);
#line 224 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value2));

#line default
#line hidden
            EndContext();
            BeginContext(6943, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(6999, 44, false);
#line 227 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value3));

#line default
#line hidden
            EndContext();
            BeginContext(7043, 56, true);
            WriteLiteral("\r\n            </td> \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7100, 44, false);
#line 230 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value4));

#line default
#line hidden
            EndContext();
            BeginContext(7144, 57, true);
            WriteLiteral("\r\n            </td>  \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7202, 44, false);
#line 233 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value5));

#line default
#line hidden
            EndContext();
            BeginContext(7246, 56, true);
            WriteLiteral("\r\n            </td> \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7303, 44, false);
#line 236 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value6));

#line default
#line hidden
            EndContext();
            BeginContext(7347, 57, true);
            WriteLiteral("\r\n            </td>  \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7405, 44, false);
#line 239 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value7));

#line default
#line hidden
            EndContext();
            BeginContext(7449, 22, true);
            WriteLiteral("\r\n            </td> \r\n");
            EndContext();
#line 241 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
             if (Model.field_headers.Count > 7) {

#line default
#line hidden
            BeginContext(7522, 35, true);
            WriteLiteral("             <td>\r\n                ");
            EndContext();
            BeginContext(7558, 44, false);
#line 243 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value8));

#line default
#line hidden
            EndContext();
            BeginContext(7602, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7658, 44, false);
#line 246 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value9));

#line default
#line hidden
            EndContext();
            BeginContext(7702, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7758, 45, false);
#line 249 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value10));

#line default
#line hidden
            EndContext();
            BeginContext(7803, 56, true);
            WriteLiteral("\r\n            </td> \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7860, 45, false);
#line 252 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value11));

#line default
#line hidden
            EndContext();
            BeginContext(7905, 57, true);
            WriteLiteral("\r\n            </td>  \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7963, 45, false);
#line 255 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value12));

#line default
#line hidden
            EndContext();
            BeginContext(8008, 56, true);
            WriteLiteral("\r\n            </td> \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(8065, 45, false);
#line 258 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value13));

#line default
#line hidden
            EndContext();
            BeginContext(8110, 57, true);
            WriteLiteral("\r\n            </td>  \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(8168, 45, false);
#line 261 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value14));

#line default
#line hidden
            EndContext();
            BeginContext(8213, 35, true);
            WriteLiteral("\r\n            </td>              \r\n");
            EndContext();
#line 263 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
            }

#line default
#line hidden
            BeginContext(8263, 15, true);
            WriteLiteral("        </tr>\r\n");
            EndContext();
#line 265 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
    }

#line default
#line hidden
            BeginContext(8285, 45, true);
            WriteLiteral("    </tbody>\r\n</table>\r\n\r\n\r\n</div>\r\n<p>\r\n    ");
            EndContext();
            BeginContext(8331, 54, false);
#line 272 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
Write(Html.ActionLink("Save", "Edit", new { id = Model.Id }));

#line default
#line hidden
            EndContext();
            BeginContext(8385, 8, true);
            WriteLiteral(" |\r\n    ");
            EndContext();
            BeginContext(8394, 40, false);
#line 273 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
Write(Html.ActionLink("Back to List", "Index"));

#line default
#line hidden
            EndContext();
            BeginContext(8434, 6, true);
            WriteLiteral("\r\n</p>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ge_repository.OtherDatabase.ge_log_file> Html { get; private set; }
    }
}
#pragma warning restore 1591
