#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a4d7fa88d6fa7dbdd99c60100abc38edd23b47de"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ge_repository.Pages.UserOperations.Pages_UserOperations_Index), @"mvc.1.0.razor-page", @"/Pages/UserOperations/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/UserOperations/Index.cshtml", typeof(ge_repository.Pages.UserOperations.Pages_UserOperations_Index), null)]
namespace ge_repository.Pages.UserOperations
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 3 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 5 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#line 7 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\_ViewImports.cshtml"
using ge_repository;

#line default
#line hidden
#line 8 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\_ViewImports.cshtml"
using ge_repository.Data;

#line default
#line hidden
#line 9 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\_ViewImports.cshtml"
using ge_repository.Models;

#line default
#line hidden
#line 10 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\_ViewImports.cshtml"
using ge_repository.Authorization;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a4d7fa88d6fa7dbdd99c60100abc38edd23b47de", @"/Pages/UserOperations/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3f9885ed56a8217391a2bb21100b53de6ade68c4", @"/Pages/_ViewImports.cshtml")]
    public class Pages_UserOperations_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "./Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_PagingHeaderPartial", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "../Project/Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "../Group/Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "./Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "./Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_PagingFooterPartial", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(61, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
  
    ViewData["Title"] = "User Operations Index";

#line default
#line hidden
            BeginContext(120, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 8 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
 if (Model.project==null && Model.group==null) {

#line default
#line hidden
            BeginContext(172, 10, true);
            WriteLiteral("<h2>\r\n    ");
            EndContext();
            BeginContext(183, 45, false);
#line 10 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
Write(Html.DisplayFor(model => model.user.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(228, 7, true);
            WriteLiteral(",\r\n    ");
            EndContext();
            BeginContext(236, 46, false);
#line 11 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
Write(Html.DisplayFor(model => model.user.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(282, 38, true);
            WriteLiteral(" User Operations Assigned\r\n    </h2>\r\n");
            EndContext();
#line 13 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
}

#line default
#line hidden
            BeginContext(323, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 15 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
 if (Model.project!=null) {

#line default
#line hidden
            BeginContext(354, 14, true);
            WriteLiteral("    <h2>\r\n    ");
            EndContext();
            BeginContext(369, 44, false);
#line 17 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
Write(Html.DisplayFor(model => model.project.name));

#line default
#line hidden
            EndContext();
            BeginContext(413, 38, true);
            WriteLiteral(" User Operations Assigned\r\n    </h2>\r\n");
            EndContext();
#line 19 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
}

#line default
#line hidden
            BeginContext(454, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 21 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
 if (Model.group!=null) {

#line default
#line hidden
            BeginContext(483, 14, true);
            WriteLiteral("    <h2>\r\n    ");
            EndContext();
            BeginContext(498, 42, false);
#line 23 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
Write(Html.DisplayFor(model => model.group.name));

#line default
#line hidden
            EndContext();
            BeginContext(540, 38, true);
            WriteLiteral(" User Operations Assigned\r\n    </h2>\r\n");
            EndContext();
#line 25 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
}

#line default
#line hidden
            BeginContext(581, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 27 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
 if(@Model.groupId!=null && @Model.IsUserGroupAdmin()) {

#line default
#line hidden
            BeginContext(641, 72, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "bb05c6589dd84d60b1e7b6977044d536", async() => {
                BeginContext(699, 10, true);
                WriteLiteral("Create New");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-groupId", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 28 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                              WriteLiteral(Model.groupId);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["groupId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-groupId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["groupId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(713, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 29 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
}

#line default
#line hidden
            BeginContext(718, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(720, 39, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "39d7141ac91445a094560ebd5fc92f3f", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(759, 88, true);
            WriteLiteral("\r\n\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(848, 61, false);
#line 37 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.user_ops[0].user.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(909, 19, true);
            WriteLiteral(",\r\n                ");
            EndContext();
            BeginContext(929, 62, false);
#line 38 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.user_ops[0].user.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(991, 112, true);
            WriteLiteral("\r\n            </th>\r\n            <th></th>\r\n            \r\n            <th class=\"text-center\">\r\n                ");
            EndContext();
            BeginContext(1104, 60, false);
#line 43 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.user_ops[0].project.name));

#line default
#line hidden
            EndContext();
            BeginContext(1164, 102, true);
            WriteLiteral("\r\n            </th>   \r\n                      \r\n            <th class=\"text-center\">\r\n                ");
            EndContext();
            BeginContext(1267, 58, false);
#line 47 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.user_ops[0].group.name));

#line default
#line hidden
            EndContext();
            BeginContext(1325, 103, true);
            WriteLiteral("\r\n            </th>\r\n\r\n            <th>Container</th>\r\n            \r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1429, 63, false);
#line 53 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.user_ops[0].user_operations));

#line default
#line hidden
            EndContext();
            BeginContext(1492, 102, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n\r\n            </th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
            EndContext();
#line 61 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
 foreach (var item in Model.user_ops) {

#line default
#line hidden
            BeginContext(1635, 49, true);
            WriteLiteral("        <tr>\r\n             <td>\r\n                ");
            EndContext();
            BeginContext(1685, 48, false);
#line 64 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.user.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(1733, 19, true);
            WriteLiteral(",\r\n                ");
            EndContext();
            BeginContext(1753, 49, false);
#line 65 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.user.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(1802, 63, true);
            WriteLiteral("\r\n            </td>\r\n            <td></td>\r\n             <td>\r\n");
            EndContext();
#line 69 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                 if (item.projectId != null) { 

#line default
#line hidden
            BeginContext(1914, 20, true);
            WriteLiteral("                    ");
            EndContext();
            BeginContext(1934, 162, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4a08ed0ec3934ea6bbf3e2ac6467f15a", async() => {
                BeginContext(1998, 24, true);
                WriteLiteral("  \r\n                    ");
                EndContext();
                BeginContext(2023, 47, false);
#line 71 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.project.name));

#line default
#line hidden
                EndContext();
                BeginContext(2070, 22, true);
                WriteLiteral("\r\n                    ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 70 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                                                       WriteLiteral(item.projectId);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2096, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 73 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                   
                }  

#line default
#line hidden
            BeginContext(2140, 46, true);
            WriteLiteral("                </td>\r\n                <td> \r\n");
            EndContext();
#line 77 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                 if (item.projectId !=null) {

#line default
#line hidden
            BeginContext(2233, 20, true);
            WriteLiteral("                    ");
            EndContext();
            BeginContext(2253, 173, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7f8148bf821c4183a1a92e3bacc6e1f3", async() => {
                BeginContext(2322, 24, true);
                WriteLiteral("  \r\n                    ");
                EndContext();
                BeginContext(2347, 53, false);
#line 79 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.project.group.name));

#line default
#line hidden
                EndContext();
                BeginContext(2400, 22, true);
                WriteLiteral("\r\n                    ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 78 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                                                     WriteLiteral(item.project.group.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2426, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 81 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                }    

#line default
#line hidden
            BeginContext(2451, 15, true);
            WriteLiteral("               ");
            EndContext();
#line 82 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                if (item.groupId !=null) {

#line default
#line hidden
            BeginContext(2495, 20, true);
            WriteLiteral("                    ");
            EndContext();
            BeginContext(2515, 157, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8a5a32ebb80c471eaeaaec1708bca75b", async() => {
                BeginContext(2576, 24, true);
                WriteLiteral("  \r\n                    ");
                EndContext();
                BeginContext(2601, 45, false);
#line 84 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.group.name));

#line default
#line hidden
                EndContext();
                BeginContext(2646, 22, true);
                WriteLiteral("\r\n                    ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 83 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                                                     WriteLiteral(item.group.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2672, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 86 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                }

#line default
#line hidden
            BeginContext(2693, 23, true);
            WriteLiteral("                </td>\r\n");
            EndContext();
#line 88 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                 if(item.groupId != null) {

#line default
#line hidden
            BeginContext(2759, 16, true);
            WriteLiteral("  <td>Group</td>");
            EndContext();
#line 88 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                                                           }

#line default
#line hidden
            BeginContext(2778, 16, true);
            WriteLiteral("                ");
            EndContext();
#line 89 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                 if(item.projectId != null) {

#line default
#line hidden
            BeginContext(2823, 17, true);
            WriteLiteral(" <td>Project</td>");
            EndContext();
#line 89 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                                                              }

#line default
#line hidden
            BeginContext(2843, 34, true);
            WriteLiteral("            <td>\r\n                ");
            EndContext();
            BeginContext(2878, 50, false);
#line 91 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.user_operations));

#line default
#line hidden
            EndContext();
            BeginContext(2928, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2983, 53, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9997753eaa1a460eb47ac770431169aa", async() => {
                BeginContext(3028, 4, true);
                WriteLiteral("Edit");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 94 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                                       WriteLiteral(item.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3036, 20, true);
            WriteLiteral(" |\r\n                ");
            EndContext();
            BeginContext(3056, 57, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f572efe32769473b9fad46b4b044ce98", async() => {
                BeginContext(3103, 6, true);
                WriteLiteral("Delete");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 95 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
                                         WriteLiteral(item.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3113, 36, true);
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
            EndContext();
#line 98 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\UserOperations\Index.cshtml"
}

#line default
#line hidden
            BeginContext(3152, 28, true);
            WriteLiteral("\r\n    </tbody>\r\n</table>\r\n\r\n");
            EndContext();
            BeginContext(3180, 39, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "9de9b54f2d584f22b204ac0715291bec", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IAuthorizationService AuthorizationService { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ge_repository.Pages.UserOperations.IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ge_repository.Pages.UserOperations.IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ge_repository.Pages.UserOperations.IndexModel>)PageContext?.ViewData;
        public ge_repository.Pages.UserOperations.IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
