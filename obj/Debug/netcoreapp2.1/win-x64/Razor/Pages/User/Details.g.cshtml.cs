#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5e198fac7484af700475921cb117a116dca96804"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ge_repository.Pages.User.Pages_User_Details), @"mvc.1.0.razor-page", @"/Pages/User/Details.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/User/Details.cshtml", typeof(ge_repository.Pages.User.Pages_User_Details), null)]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5e198fac7484af700475921cb117a116dca96804", @"/Pages/User/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3f9885ed56a8217391a2bb21100b53de6ade68c4", @"/Pages/_ViewImports.cshtml")]
    public class Pages_User_Details : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "./Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "./Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            BeginContext(53, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
  
    ViewData["Title"] = "Details";

#line default
#line hidden
            BeginContext(98, 121, true);
            WriteLiteral("\r\n<h2>Details</h2>\r\n\r\n<div>\r\n    <h4>ge_user</h4>\r\n    <hr />\r\n    <dl class=\"dl-horizontal\">\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(220, 53, false);
#line 15 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(273, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(317, 49, false);
#line 18 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(366, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(410, 52, false);
#line 21 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(462, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(506, 48, false);
#line 24 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(554, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(598, 56, false);
#line 27 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.LastLoggedIn));

#line default
#line hidden
            EndContext();
            BeginContext(654, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(698, 52, false);
#line 30 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.LastLoggedIn));

#line default
#line hidden
            EndContext();
            BeginContext(750, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(794, 52, false);
#line 33 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.UserName));

#line default
#line hidden
            EndContext();
            BeginContext(846, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(890, 48, false);
#line 36 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.UserName));

#line default
#line hidden
            EndContext();
            BeginContext(938, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(982, 62, false);
#line 39 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.NormalizedUserName));

#line default
#line hidden
            EndContext();
            BeginContext(1044, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1088, 58, false);
#line 42 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.NormalizedUserName));

#line default
#line hidden
            EndContext();
            BeginContext(1146, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1190, 49, false);
#line 45 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.Email));

#line default
#line hidden
            EndContext();
            BeginContext(1239, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1283, 45, false);
#line 48 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.Email));

#line default
#line hidden
            EndContext();
            BeginContext(1328, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1372, 59, false);
#line 51 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.NormalizedEmail));

#line default
#line hidden
            EndContext();
            BeginContext(1431, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1475, 55, false);
#line 54 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.NormalizedEmail));

#line default
#line hidden
            EndContext();
            BeginContext(1530, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1574, 58, false);
#line 57 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.EmailConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(1632, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1676, 54, false);
#line 60 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.EmailConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(1730, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1774, 56, false);
#line 63 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.PasswordHash));

#line default
#line hidden
            EndContext();
            BeginContext(1830, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1874, 52, false);
#line 66 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.PasswordHash));

#line default
#line hidden
            EndContext();
            BeginContext(1926, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1970, 57, false);
#line 69 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.SecurityStamp));

#line default
#line hidden
            EndContext();
            BeginContext(2027, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2071, 53, false);
#line 72 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.SecurityStamp));

#line default
#line hidden
            EndContext();
            BeginContext(2124, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(2168, 60, false);
#line 75 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.ConcurrencyStamp));

#line default
#line hidden
            EndContext();
            BeginContext(2228, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2272, 56, false);
#line 78 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.ConcurrencyStamp));

#line default
#line hidden
            EndContext();
            BeginContext(2328, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(2372, 55, false);
#line 81 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(2427, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2471, 51, false);
#line 84 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(2522, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(2566, 64, false);
#line 87 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.PhoneNumberConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(2630, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2674, 60, false);
#line 90 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.PhoneNumberConfirmed));

#line default
#line hidden
            EndContext();
            BeginContext(2734, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(2778, 60, false);
#line 93 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.TwoFactorEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(2838, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2882, 56, false);
#line 96 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.TwoFactorEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(2938, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(2982, 54, false);
#line 99 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.LockoutEnd));

#line default
#line hidden
            EndContext();
            BeginContext(3036, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(3080, 50, false);
#line 102 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.LockoutEnd));

#line default
#line hidden
            EndContext();
            BeginContext(3130, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(3174, 58, false);
#line 105 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.LockoutEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(3232, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(3276, 54, false);
#line 108 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.LockoutEnabled));

#line default
#line hidden
            EndContext();
            BeginContext(3330, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(3374, 61, false);
#line 111 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ge_user.AccessFailedCount));

#line default
#line hidden
            EndContext();
            BeginContext(3435, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(3479, 57, false);
#line 114 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
       Write(Html.DisplayFor(model => model.ge_user.AccessFailedCount));

#line default
#line hidden
            EndContext();
            BeginContext(3536, 47, true);
            WriteLiteral("\r\n        </dd>\r\n    </dl>\r\n</div>\r\n<div>\r\n    ");
            EndContext();
            BeginContext(3583, 62, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5e685637fb8a46b38820c5e94f066f94", async() => {
                BeginContext(3637, 4, true);
                WriteLiteral("Edit");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 119 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\User\Details.cshtml"
                           WriteLiteral(Model.ge_user.Id);

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
            BeginContext(3645, 8, true);
            WriteLiteral(" |\r\n    ");
            EndContext();
            BeginContext(3653, 38, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e654196d35fb4c7aaa77cc465a04c068", async() => {
                BeginContext(3675, 12, true);
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
            BeginContext(3691, 10, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ge_repository.Pages.User.DetailsModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ge_repository.Pages.User.DetailsModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ge_repository.Pages.User.DetailsModel>)PageContext?.ViewData;
        public ge_repository.Pages.User.DetailsModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
