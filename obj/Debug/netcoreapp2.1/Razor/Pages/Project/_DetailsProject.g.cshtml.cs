#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "39e7220fd9fd550a3a12f52b8b1347fbbaaf7524"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ge_repository.Pages.Project.Pages_Project__DetailsProject), @"mvc.1.0.view", @"/Pages/Project/_DetailsProject.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Pages/Project/_DetailsProject.cshtml", typeof(ge_repository.Pages.Project.Pages_Project__DetailsProject))]
namespace ge_repository.Pages.Project
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"39e7220fd9fd550a3a12f52b8b1347fbbaaf7524", @"/Pages/Project/_DetailsProject.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3f9885ed56a8217391a2bb21100b53de6ade68c4", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Project__DetailsProject : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ge_repository.Models.ge_project>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "../Group/Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            BeginContext(40, 28, true);
            WriteLiteral("\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(69, 40, false);
#line 4 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.name));

#line default
#line hidden
            EndContext();
            BeginContext(109, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(153, 36, false);
#line 7 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayFor(model => model.name));

#line default
#line hidden
            EndContext();
            BeginContext(189, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(233, 45, false);
#line 10 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.managerId));

#line default
#line hidden
            EndContext();
            BeginContext(278, 31, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n");
            EndContext();
#line 13 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
             if(@Model.manager!=null) {
              

#line default
#line hidden
            BeginContext(365, 24, false);
#line 14 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
         Write(Model.manager.FullName());

#line default
#line hidden
            EndContext();
#line 14 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
                                       ;  
            }

#line default
#line hidden
            BeginContext(409, 41, true);
            WriteLiteral("        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(451, 47, false);
#line 18 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.description));

#line default
#line hidden
            EndContext();
            BeginContext(498, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(542, 43, false);
#line 21 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayFor(model => model.description));

#line default
#line hidden
            EndContext();
            BeginContext(585, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(629, 44, false);
#line 24 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.keywords));

#line default
#line hidden
            EndContext();
            BeginContext(673, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(717, 40, false);
#line 27 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayFor(model => model.keywords));

#line default
#line hidden
            EndContext();
            BeginContext(757, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(801, 46, false);
#line 30 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.group.name));

#line default
#line hidden
            EndContext();
            BeginContext(847, 42, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n           ");
            EndContext();
            BeginContext(889, 135, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "73ca306ba2034b6f8604d4d82f592a0c", async() => {
                BeginContext(950, 14, true);
                WriteLiteral("\r\n            ");
                EndContext();
                BeginContext(965, 42, false);
#line 34 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayFor(model => model.group.name));

#line default
#line hidden
                EndContext();
                BeginContext(1007, 13, true);
                WriteLiteral("\r\n           ");
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
#line 33 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
                                            WriteLiteral(Model.groupId);

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
            BeginContext(1024, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1068, 46, false);
#line 38 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.start_date));

#line default
#line hidden
            EndContext();
            BeginContext(1114, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1158, 42, false);
#line 41 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayFor(model => model.start_date));

#line default
#line hidden
            EndContext();
            BeginContext(1200, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1244, 44, false);
#line 44 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.end_date));

#line default
#line hidden
            EndContext();
            BeginContext(1288, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1332, 40, false);
#line 47 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayFor(model => model.end_date));

#line default
#line hidden
            EndContext();
            BeginContext(1372, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1416, 43, false);
#line 50 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.pstatus));

#line default
#line hidden
            EndContext();
            BeginContext(1459, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1503, 39, false);
#line 53 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayFor(model => model.pstatus));

#line default
#line hidden
            EndContext();
            BeginContext(1542, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1586, 43, false);
#line 56 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.cstatus));

#line default
#line hidden
            EndContext();
            BeginContext(1629, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1673, 39, false);
#line 59 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayFor(model => model.cstatus));

#line default
#line hidden
            EndContext();
            BeginContext(1712, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1756, 52, false);
#line 62 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.otherDbConnectId));

#line default
#line hidden
            EndContext();
            BeginContext(1808, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1852, 48, false);
#line 65 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayFor(model => model.otherDbConnectId));

#line default
#line hidden
            EndContext();
            BeginContext(1900, 44, true);
            WriteLiteral("\r\n        </dd>\r\n         <dt>\r\n            ");
            EndContext();
            BeginContext(1945, 49, false);
#line 68 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayNameFor(model => model.esriConnectId));

#line default
#line hidden
            EndContext();
            BeginContext(1994, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(2038, 45, false);
#line 71 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Project\_DetailsProject.cshtml"
       Write(Html.DisplayFor(model => model.esriConnectId));

#line default
#line hidden
            EndContext();
            BeginContext(2083, 15, true);
            WriteLiteral("\r\n        </dd>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ge_repository.Models.ge_project> Html { get; private set; }
    }
}
#pragma warning restore 1591
