#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6715ac277fa343443be5b8d45578c65a319c4676"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ge_repository.Pages.User.Pages_User_Index), @"mvc.1.0.razor-page", @"/Pages/User/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/User/Index.cshtml", typeof(ge_repository.Pages.User.Pages_User_Index), null)]
namespace ge_repository.Pages.User
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 3 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6715ac277fa343443be5b8d45578c65a319c4676", @"/Pages/User/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3f9885ed56a8217391a2bb21100b53de6ade68c4", @"/Pages/_ViewImports.cshtml")]
    public class Pages_User_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "./Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "./Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "./Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 4 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
            BeginContext(131, 29, true);
            WriteLiteral("\r\n<h2>Index</h2>\r\n\r\n<p>\r\n    ");
            EndContext();
            BeginContext(160, 35, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f8f19b28c0864f31bcbb828422fe0eaa", async() => {
                BeginContext(181, 10, true);
                WriteLiteral("Create New");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(195, 92, true);
            WriteLiteral("\r\n</p>\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(288, 54, false);
#line 17 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(342, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(398, 53, false);
#line 20 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].LastName));

#line default
#line hidden
            EndContext();
            BeginContext(451, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(507, 57, false);
#line 23 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].LastLoggedIn));

#line default
#line hidden
            EndContext();
            BeginContext(564, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(620, 53, false);
#line 26 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].UserName));

#line default
#line hidden
            EndContext();
            BeginContext(673, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(729, 63, false);
#line 29 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].NormalizedUserName));

#line default
#line hidden
            EndContext();
            BeginContext(792, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(848, 50, false);
#line 32 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].Email));

#line default
#line hidden
            EndContext();
            BeginContext(898, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(954, 60, false);
#line 35 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].NormalizedEmail));

#line default
#line hidden
            EndContext();
            BeginContext(1014, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1070, 59, false);
#line 38 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].EmailConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(1129, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1185, 57, false);
#line 41 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].PasswordHash));

#line default
#line hidden
            EndContext();
            BeginContext(1242, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1298, 58, false);
#line 44 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].SecurityStamp));

#line default
#line hidden
            EndContext();
            BeginContext(1356, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1412, 61, false);
#line 47 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].ConcurrencyStamp));

#line default
#line hidden
            EndContext();
            BeginContext(1473, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1529, 56, false);
#line 50 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(1585, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1641, 65, false);
#line 53 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].PhoneNumberConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(1706, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1762, 61, false);
#line 56 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].TwoFactorEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(1823, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1879, 55, false);
#line 59 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].LockoutEnd));

#line default
#line hidden
            EndContext();
            BeginContext(1934, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1990, 59, false);
#line 62 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].LockoutEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(2049, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(2105, 62, false);
#line 65 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.users[0].AccessFailedCount));

#line default
#line hidden
            EndContext();
            BeginContext(2167, 86, true);
            WriteLiteral("\r\n            </th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
            EndContext();
#line 71 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
 foreach (var item in Model.users) {

#line default
#line hidden
            BeginContext(2291, 48, true);
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2340, 44, false);
#line 74 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(2384, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2440, 43, false);
#line 77 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(2483, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2539, 47, false);
#line 80 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.LastLoggedIn));

#line default
#line hidden
            EndContext();
            BeginContext(2586, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2642, 43, false);
#line 83 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.UserName));

#line default
#line hidden
            EndContext();
            BeginContext(2685, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2741, 53, false);
#line 86 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.NormalizedUserName));

#line default
#line hidden
            EndContext();
            BeginContext(2794, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2850, 40, false);
#line 89 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.Email));

#line default
#line hidden
            EndContext();
            BeginContext(2890, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2946, 50, false);
#line 92 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.NormalizedEmail));

#line default
#line hidden
            EndContext();
            BeginContext(2996, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3052, 49, false);
#line 95 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.EmailConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(3101, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3157, 47, false);
#line 98 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.PasswordHash));

#line default
#line hidden
            EndContext();
            BeginContext(3204, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3260, 48, false);
#line 101 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.SecurityStamp));

#line default
#line hidden
            EndContext();
            BeginContext(3308, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3364, 51, false);
#line 104 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.ConcurrencyStamp));

#line default
#line hidden
            EndContext();
            BeginContext(3415, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3471, 46, false);
#line 107 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(3517, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3573, 55, false);
#line 110 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.PhoneNumberConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(3628, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3684, 51, false);
#line 113 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.TwoFactorEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(3735, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3791, 45, false);
#line 116 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.LockoutEnd));

#line default
#line hidden
            EndContext();
            BeginContext(3836, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3892, 49, false);
#line 119 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.LockoutEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(3941, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3997, 52, false);
#line 122 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.AccessFailedCount));

#line default
#line hidden
            EndContext();
            BeginContext(4049, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(4104, 53, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b1baed9def8646b7a981b458ba7a7f81", async() => {
                BeginContext(4149, 4, true);
                WriteLiteral("Edit");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 125 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
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
            BeginContext(4157, 20, true);
            WriteLiteral(" |\r\n                ");
            EndContext();
            BeginContext(4177, 59, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c377a754d00d4ab9bf5afe1541ddc649", async() => {
                BeginContext(4225, 7, true);
                WriteLiteral("Details");
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
#line 126 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
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
            BeginContext(4236, 20, true);
            WriteLiteral(" |\r\n                ");
            EndContext();
            BeginContext(4256, 57, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "144a850f899247b789d9bf0f73f99dd7", async() => {
                BeginContext(4303, 6, true);
                WriteLiteral("Delete");
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
#line 127 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
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
            BeginContext(4313, 36, true);
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
            EndContext();
#line 130 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Index.cshtml"
}

#line default
#line hidden
            BeginContext(4352, 24, true);
            WriteLiteral("    </tbody>\r\n</table>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ge_repository.Pages.User.IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ge_repository.Pages.User.IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ge_repository.Pages.User.IndexModel>)PageContext?.ViewData;
        public ge_repository.Pages.User.IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
