#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "57ee186e6332057816a1fcc3f402b6037f3514f4"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"57ee186e6332057816a1fcc3f402b6037f3514f4", @"/Views/ge_log/ReadData.cshtml")]
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
            BeginContext(179, 54, true);
            WriteLiteral("\r\n<h2>Details</h2>\r\n\r\n    <h4>Logger File</h4>\r\n      ");
            EndContext();
            BeginContext(234, 84, false);
#line 14 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
 Write(Html.ActionLink("View Original File", "View", "ge_data",  new { id = Model.dataId }));

#line default
#line hidden
            EndContext();
            BeginContext(318, 85, true);
            WriteLiteral(" \r\n    <h1> </h1>\r\n<hr />\r\n    <dl class=\"dl-horizontal\">\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(404, 46, false);
#line 19 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayNameFor(model => model.fileHeader));

#line default
#line hidden
            EndContext();
            BeginContext(450, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(494, 42, false);
#line 22 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayFor(model => model.fileHeader));

#line default
#line hidden
            EndContext();
            BeginContext(536, 45, true);
            WriteLiteral("\r\n        </dd>  \r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(582, 47, false);
#line 25 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayNameFor(model => model.fieldHeader));

#line default
#line hidden
            EndContext();
            BeginContext(629, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(673, 43, false);
#line 28 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayFor(model => model.fieldHeader));

#line default
#line hidden
            EndContext();
            BeginContext(716, 45, true);
            WriteLiteral("\r\n        </dd>  \r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(762, 53, false);
#line 31 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayNameFor(model => model.readingAggregates));

#line default
#line hidden
            EndContext();
            BeginContext(815, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(859, 49, false);
#line 34 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(Html.DisplayFor(model => model.readingAggregates));

#line default
#line hidden
            EndContext();
            BeginContext(908, 99, true);
            WriteLiteral("\r\n        </dd> \r\n        <dt>\r\n            Save Status\r\n        </dt>\r\n        <dd> \r\n            ");
            EndContext();
            BeginContext(1008, 18, false);
#line 40 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
       Write(ViewBag.fileStatus);

#line default
#line hidden
            EndContext();
            BeginContext(1026, 118, true);
            WriteLiteral("\r\n        </dd>  \r\n    </dl>\r\n    <table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1145, 56, false);
#line 47 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.file_headers[0].name));

#line default
#line hidden
            EndContext();
            BeginContext(1201, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1257, 63, false);
#line 50 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.file_headers[0].search_text));

#line default
#line hidden
            EndContext();
            BeginContext(1320, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1376, 57, false);
#line 53 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.file_headers[0].value));

#line default
#line hidden
            EndContext();
            BeginContext(1433, 67, true);
            WriteLiteral("\r\n            </th>\r\n        </tr>\r\n    </thead>    \r\n    <tbody>\r\n");
            EndContext();
#line 58 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
     foreach (var header in Model.file_headers) {

#line default
#line hidden
            BeginContext(1551, 48, true);
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(1600, 41, false);
#line 61 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.name));

#line default
#line hidden
            EndContext();
            BeginContext(1641, 40, true);
            WriteLiteral("\r\n            </td>\r\n             <td>\r\n");
            EndContext();
#line 64 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                 if(header.search_text!=null) {
                

#line default
#line hidden
            BeginContext(1747, 48, false);
#line 65 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.search_text));

#line default
#line hidden
            EndContext();
#line 65 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                                                                 
                }

#line default
#line hidden
            BeginContext(1816, 54, true);
            WriteLiteral("            </td>\r\n             <td>\r\n                ");
            EndContext();
            BeginContext(1871, 42, false);
#line 69 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.value));

#line default
#line hidden
            EndContext();
            BeginContext(1913, 36, true);
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
            EndContext();
#line 72 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
    }

#line default
#line hidden
            BeginContext(1956, 116, true);
            WriteLiteral("    </tbody>\r\n    </table>\r\n    <table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2073, 55, false);
#line 79 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].id));

#line default
#line hidden
            EndContext();
            BeginContext(2128, 56, true);
            WriteLiteral("\r\n            </th>\r\n             <th>\r\n                ");
            EndContext();
            BeginContext(2185, 57, false);
#line 82 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].name));

#line default
#line hidden
            EndContext();
            BeginContext(2242, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2298, 60, false);
#line 85 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].db_name));

#line default
#line hidden
            EndContext();
            BeginContext(2358, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2414, 59, false);
#line 88 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].source));

#line default
#line hidden
            EndContext();
            BeginContext(2473, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2529, 58, false);
#line 91 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].units));

#line default
#line hidden
            EndContext();
            BeginContext(2587, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2643, 61, false);
#line 94 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.field_headers[0].comments));

#line default
#line hidden
            EndContext();
            BeginContext(2704, 67, true);
            WriteLiteral("\r\n            </th>\r\n        </tr>\r\n    </thead>    \r\n    <tbody>\r\n");
            EndContext();
#line 99 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
     foreach (var header in Model.field_headers) {

#line default
#line hidden
            BeginContext(2823, 48, true);
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2872, 39, false);
#line 102 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.id));

#line default
#line hidden
            EndContext();
            BeginContext(2911, 56, true);
            WriteLiteral("\r\n            </td>\r\n             <td>\r\n                ");
            EndContext();
            BeginContext(2968, 41, false);
#line 105 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.name));

#line default
#line hidden
            EndContext();
            BeginContext(3009, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3065, 44, false);
#line 108 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.db_name));

#line default
#line hidden
            EndContext();
            BeginContext(3109, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3165, 43, false);
#line 111 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.source));

#line default
#line hidden
            EndContext();
            BeginContext(3208, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3264, 42, false);
#line 114 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.units));

#line default
#line hidden
            EndContext();
            BeginContext(3306, 58, true);
            WriteLiteral("\r\n            </td>\r\n               <td>\r\n                ");
            EndContext();
            BeginContext(3365, 45, false);
#line 117 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => header.comments));

#line default
#line hidden
            EndContext();
            BeginContext(3410, 36, true);
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
            EndContext();
#line 120 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
    }

#line default
#line hidden
            BeginContext(3453, 30, true);
            WriteLiteral("    </tbody>\r\n    </table>\r\n\r\n");
            EndContext();
#line 124 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
     if (Model.file_array != null) {

#line default
#line hidden
            BeginContext(3521, 112, true);
            WriteLiteral("        <table class=\"table\">\r\n            <thead>\r\n            <tr>\r\n                <th>\r\n                    ");
            EndContext();
            BeginContext(3634, 54, false);
#line 129 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
               Write(Html.DisplayNameFor(model => model.file_array[0].name));

#line default
#line hidden
            EndContext();
            BeginContext(3688, 25, true);
            WriteLiteral("\r\n                </th>\r\n");
            EndContext();
#line 131 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                     foreach(var value in Model.file_array[0].values) {

#line default
#line hidden
            BeginContext(3786, 46, true);
            WriteLiteral("                    <th>\r\n                    ");
            EndContext();
            BeginContext(3833, 39, false);
#line 133 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
               Write(Html.DisplayNameFor(modelItem => value));

#line default
#line hidden
            EndContext();
            BeginContext(3872, 29, true);
            WriteLiteral("\r\n                    </th>\r\n");
            EndContext();
#line 135 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                    }

#line default
#line hidden
            BeginContext(3924, 66, true);
            WriteLiteral("            </tr>\r\n            </thead>    \r\n            <tbody>\r\n");
            EndContext();
#line 139 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                 foreach (var row in Model.file_array) { 

#line default
#line hidden
            BeginContext(4049, 84, true);
            WriteLiteral("                    <tr>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(4134, 38, false);
#line 142 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                       Write(Html.DisplayFor(modelItem => row.name));

#line default
#line hidden
            EndContext();
            BeginContext(4172, 33, true);
            WriteLiteral("\r\n                        </td>\r\n");
            EndContext();
#line 144 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                         foreach (var value in row.values) { 

#line default
#line hidden
            BeginContext(4268, 58, true);
            WriteLiteral("                        <td>\r\n                            ");
            EndContext();
            BeginContext(4327, 35, false);
#line 146 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                       Write(Html.DisplayFor(modelItem => value));

#line default
#line hidden
            EndContext();
            BeginContext(4362, 33, true);
            WriteLiteral("\r\n                        </td>\r\n");
            EndContext();
#line 148 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                        }

#line default
#line hidden
            BeginContext(4422, 27, true);
            WriteLiteral("                    </tr>\r\n");
            EndContext();
#line 150 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
                   
                }

#line default
#line hidden
            BeginContext(4489, 40, true);
            WriteLiteral("            </tbody>\r\n        </table>\r\n");
            EndContext();
#line 154 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
    }

#line default
#line hidden
            BeginContext(4536, 192, true);
            WriteLiteral(" <div class=\"table-wrapper-scroll-y my-custom-scrollbar\">   \r\n <table class=\"table table-bordered table-striped mb-0\">\r\n    <thead>\r\n        <tr>\r\n           <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(4729, 63, false);
#line 160 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].ReadingDatetime));

#line default
#line hidden
            EndContext();
            BeginContext(4792, 66, true);
            WriteLiteral("\r\n            </th>\r\n           <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(4859, 56, false);
#line 163 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Duration));

#line default
#line hidden
            EndContext();
            BeginContext(4915, 67, true);
            WriteLiteral("\r\n            </th>\r\n            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(4983, 54, false);
#line 166 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value1));

#line default
#line hidden
            EndContext();
            BeginContext(5037, 67, true);
            WriteLiteral("\r\n            </th>\r\n            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(5105, 54, false);
#line 169 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value2));

#line default
#line hidden
            EndContext();
            BeginContext(5159, 67, true);
            WriteLiteral("\r\n            </th>\r\n            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(5227, 54, false);
#line 172 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value3));

#line default
#line hidden
            EndContext();
            BeginContext(5281, 68, true);
            WriteLiteral("\r\n            </th>\r\n             <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(5350, 54, false);
#line 175 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value4));

#line default
#line hidden
            EndContext();
            BeginContext(5404, 68, true);
            WriteLiteral("\r\n            </th> \r\n            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(5473, 54, false);
#line 178 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value5));

#line default
#line hidden
            EndContext();
            BeginContext(5527, 67, true);
            WriteLiteral("\r\n            </th>\r\n            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(5595, 54, false);
#line 181 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value6));

#line default
#line hidden
            EndContext();
            BeginContext(5649, 70, true);
            WriteLiteral("\r\n            </th>   \r\n            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(5720, 54, false);
#line 184 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value7));

#line default
#line hidden
            EndContext();
            BeginContext(5774, 21, true);
            WriteLiteral("\r\n            </th>\r\n");
            EndContext();
#line 186 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
             if (Model.field_headers.Count > 7) {

#line default
#line hidden
            BeginContext(5846, 46, true);
            WriteLiteral("            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(5893, 54, false);
#line 188 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value8));

#line default
#line hidden
            EndContext();
            BeginContext(5947, 68, true);
            WriteLiteral("\r\n            </th>\r\n             <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(6016, 54, false);
#line 191 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value9));

#line default
#line hidden
            EndContext();
            BeginContext(6070, 67, true);
            WriteLiteral("\r\n            </th>\r\n            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(6138, 55, false);
#line 194 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value10));

#line default
#line hidden
            EndContext();
            BeginContext(6193, 68, true);
            WriteLiteral("\r\n            </th>\r\n             <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(6262, 55, false);
#line 197 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value11));

#line default
#line hidden
            EndContext();
            BeginContext(6317, 68, true);
            WriteLiteral("\r\n            </th> \r\n            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(6386, 55, false);
#line 200 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value12));

#line default
#line hidden
            EndContext();
            BeginContext(6441, 67, true);
            WriteLiteral("\r\n            </th>\r\n            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(6509, 55, false);
#line 203 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value13));

#line default
#line hidden
            EndContext();
            BeginContext(6564, 70, true);
            WriteLiteral("\r\n            </th>   \r\n            <th scope=\"col\">\r\n                ");
            EndContext();
            BeginContext(6635, 55, false);
#line 206 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayNameFor(model => model.readings[0].Value14));

#line default
#line hidden
            EndContext();
            BeginContext(6690, 21, true);
            WriteLiteral("\r\n            </th>\r\n");
            EndContext();
#line 208 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
            }

#line default
#line hidden
            BeginContext(6726, 42, true);
            WriteLiteral("        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
            EndContext();
#line 212 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
     foreach (var reading in Model.readings) {

#line default
#line hidden
            BeginContext(6816, 48, true);
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(6865, 53, false);
#line 215 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.ReadingDatetime));

#line default
#line hidden
            EndContext();
            BeginContext(6918, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(6974, 46, false);
#line 218 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Duration));

#line default
#line hidden
            EndContext();
            BeginContext(7020, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7076, 44, false);
#line 221 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value1));

#line default
#line hidden
            EndContext();
            BeginContext(7120, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7176, 44, false);
#line 224 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value2));

#line default
#line hidden
            EndContext();
            BeginContext(7220, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7276, 44, false);
#line 227 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value3));

#line default
#line hidden
            EndContext();
            BeginContext(7320, 56, true);
            WriteLiteral("\r\n            </td> \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7377, 44, false);
#line 230 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value4));

#line default
#line hidden
            EndContext();
            BeginContext(7421, 57, true);
            WriteLiteral("\r\n            </td>  \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7479, 44, false);
#line 233 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value5));

#line default
#line hidden
            EndContext();
            BeginContext(7523, 56, true);
            WriteLiteral("\r\n            </td> \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7580, 44, false);
#line 236 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value6));

#line default
#line hidden
            EndContext();
            BeginContext(7624, 57, true);
            WriteLiteral("\r\n            </td>  \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7682, 44, false);
#line 239 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value7));

#line default
#line hidden
            EndContext();
            BeginContext(7726, 22, true);
            WriteLiteral("\r\n            </td> \r\n");
            EndContext();
#line 241 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
             if (Model.field_headers.Count > 7) {

#line default
#line hidden
            BeginContext(7799, 35, true);
            WriteLiteral("             <td>\r\n                ");
            EndContext();
            BeginContext(7835, 44, false);
#line 243 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value8));

#line default
#line hidden
            EndContext();
            BeginContext(7879, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(7935, 44, false);
#line 246 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value9));

#line default
#line hidden
            EndContext();
            BeginContext(7979, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(8035, 45, false);
#line 249 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value10));

#line default
#line hidden
            EndContext();
            BeginContext(8080, 56, true);
            WriteLiteral("\r\n            </td> \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(8137, 45, false);
#line 252 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value11));

#line default
#line hidden
            EndContext();
            BeginContext(8182, 57, true);
            WriteLiteral("\r\n            </td>  \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(8240, 45, false);
#line 255 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value12));

#line default
#line hidden
            EndContext();
            BeginContext(8285, 56, true);
            WriteLiteral("\r\n            </td> \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(8342, 45, false);
#line 258 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value13));

#line default
#line hidden
            EndContext();
            BeginContext(8387, 57, true);
            WriteLiteral("\r\n            </td>  \r\n            <td>\r\n                ");
            EndContext();
            BeginContext(8445, 45, false);
#line 261 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
           Write(Html.DisplayFor(modelItem => reading.Value14));

#line default
#line hidden
            EndContext();
            BeginContext(8490, 35, true);
            WriteLiteral("\r\n            </td>              \r\n");
            EndContext();
#line 263 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
            }

#line default
#line hidden
            BeginContext(8540, 15, true);
            WriteLiteral("        </tr>\r\n");
            EndContext();
#line 265 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
    }

#line default
#line hidden
            BeginContext(8562, 43, true);
            WriteLiteral("    </tbody>\r\n</table>\r\n</div>\r\n\r\n<p>\r\n    ");
            EndContext();
            BeginContext(8606, 54, false);
#line 271 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
Write(Html.ActionLink("Save", "Edit", new { id = Model.Id }));

#line default
#line hidden
            EndContext();
            BeginContext(8660, 8, true);
            WriteLiteral(" |\r\n    ");
            EndContext();
            BeginContext(8669, 40, false);
#line 272 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\ge_log\ReadData.cshtml"
Write(Html.ActionLink("Back to List", "Index"));

#line default
#line hidden
            EndContext();
            BeginContext(8709, 190, true);
            WriteLiteral("\r\n</p>\r\n\r\n\r\n$(document).ready(function () {\r\n$(\'#dtVerticalScrollExample\').DataTable({\r\n\"scrollY\": \"200px\",\r\n\"scrollCollapse\": true,\r\n});\r\n$(\'.dataTables_length\').addClass(\'bs-select\');\r\n});");
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
