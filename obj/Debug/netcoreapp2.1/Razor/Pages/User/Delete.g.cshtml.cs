#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c1eaba3f24a94021c60b62da0ba6c9db7d39a693"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ge_repository.Pages.User.Pages_User_Delete), @"mvc.1.0.razor-page", @"/Pages/User/Delete.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/User/Delete.cshtml", typeof(ge_repository.Pages.User.Pages_User_Delete), null)]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c1eaba3f24a94021c60b62da0ba6c9db7d39a693", @"/Pages/User/Delete.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3f9885ed56a8217391a2bb21100b53de6ade68c4", @"/Pages/_ViewImports.cshtml")]
    public class Pages_User_Delete : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "./Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(52, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
  
    ViewData["Title"] = "Delete";

#line default
#line hidden
            BeginContext(96, 168, true);
            WriteLiteral("\r\n<h2>Delete</h2>\r\n\r\n<h3>Are you sure you want to delete this?</h3>\r\n<div>\r\n    <h4>ge_user</h4>\r\n    <hr />\r\n    <dl class=\"dl-horizontal\">\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(265, 53, false);
#line 16 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(318, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(362, 49, false);
#line 19 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(411, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(455, 52, false);
#line 22 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(507, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(551, 48, false);
#line 25 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(599, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(643, 56, false);
#line 28 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.LastLoggedIn));

#line default
#line hidden
            EndContext();
            BeginContext(699, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(743, 52, false);
#line 31 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.LastLoggedIn));

#line default
#line hidden
            EndContext();
            BeginContext(795, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(839, 52, false);
#line 34 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.UserName));

#line default
#line hidden
            EndContext();
            BeginContext(891, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(935, 48, false);
#line 37 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.UserName));

#line default
#line hidden
            EndContext();
            BeginContext(983, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1027, 62, false);
#line 40 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.NormalizedUserName));

#line default
#line hidden
            EndContext();
            BeginContext(1089, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1133, 58, false);
#line 43 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.NormalizedUserName));

#line default
#line hidden
            EndContext();
            BeginContext(1191, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1235, 49, false);
#line 46 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.Email));

#line default
#line hidden
            EndContext();
            BeginContext(1284, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1328, 45, false);
#line 49 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.Email));

#line default
#line hidden
            EndContext();
            BeginContext(1373, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1417, 59, false);
#line 52 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.NormalizedEmail));

#line default
#line hidden
            EndContext();
            BeginContext(1476, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1520, 55, false);
#line 55 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.NormalizedEmail));

#line default
#line hidden
            EndContext();
            BeginContext(1575, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1619, 58, false);
#line 58 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.EmailConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(1677, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1721, 54, false);
#line 61 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.EmailConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(1775, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1819, 56, false);
#line 64 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.PasswordHash));

#line default
#line hidden
            EndContext();
            BeginContext(1875, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1919, 52, false);
#line 67 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.PasswordHash));

#line default
#line hidden
            EndContext();
            BeginContext(1971, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(2015, 57, false);
#line 70 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.SecurityStamp));

#line default
#line hidden
            EndContext();
            BeginContext(2072, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2116, 53, false);
#line 73 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.SecurityStamp));

#line default
#line hidden
            EndContext();
            BeginContext(2169, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(2213, 60, false);
#line 76 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.ConcurrencyStamp));

#line default
#line hidden
            EndContext();
            BeginContext(2273, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2317, 56, false);
#line 79 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.ConcurrencyStamp));

#line default
#line hidden
            EndContext();
            BeginContext(2373, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(2417, 55, false);
#line 82 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(2472, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2516, 51, false);
#line 85 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(2567, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(2611, 64, false);
#line 88 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.PhoneNumberConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(2675, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2719, 60, false);
#line 91 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.PhoneNumberConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(2779, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(2823, 60, false);
#line 94 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.TwoFactorEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(2883, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2927, 56, false);
#line 97 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.TwoFactorEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(2983, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(3027, 54, false);
#line 100 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.LockoutEnd));

#line default
#line hidden
            EndContext();
            BeginContext(3081, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(3125, 50, false);
#line 103 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.LockoutEnd));

#line default
#line hidden
            EndContext();
            BeginContext(3175, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(3219, 58, false);
#line 106 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.LockoutEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(3277, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(3321, 54, false);
#line 109 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.LockoutEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(3375, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(3419, 61, false);
#line 112 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.AccessFailedCount));

#line default
#line hidden
            EndContext();
            BeginContext(3480, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(3524, 57, false);
#line 115 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.AccessFailedCount));

#line default
#line hidden
            EndContext();
            BeginContext(3581, 38, true);
            WriteLiteral("\r\n        </dd>\r\n    </dl>\r\n    \r\n    ");
            EndContext();
            BeginContext(3619, 209, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "321ca84719f54671a432e2475b1f8789", async() => {
                BeginContext(3639, 10, true);
                WriteLiteral("\r\n        ");
                EndContext();
                BeginContext(3649, 44, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "96d4467c382544e985c5f571d3d5991d", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 120 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Delete.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.ge_user.Id);

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(3693, 84, true);
                WriteLiteral("\r\n        <input type=\"submit\" value=\"Delete\" class=\"btn btn-default\" /> |\r\n        ");
                EndContext();
                BeginContext(3777, 38, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "640e1f49077341b087b2b7346343f227", async() => {
                    BeginContext(3799, 12, true);
                    WriteLiteral("Back to List");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(3815, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3828, 10, true);
            WriteLiteral("\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ge_repository.Pages.User.DeleteModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ge_repository.Pages.User.DeleteModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ge_repository.Pages.User.DeleteModel>)PageContext?.ViewData;
        public ge_repository.Pages.User.DeleteModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
