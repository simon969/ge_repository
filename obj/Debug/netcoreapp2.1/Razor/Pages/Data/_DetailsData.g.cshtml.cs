#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9ec960bcd182ee36ec565b84b69c3f7ebc96c2f3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ge_repository.Pages.Data.Pages_Data__DetailsData), @"mvc.1.0.view", @"/Pages/Data/_DetailsData.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Pages/Data/_DetailsData.cshtml", typeof(ge_repository.Pages.Data.Pages_Data__DetailsData))]
namespace ge_repository.Pages.Data
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9ec960bcd182ee36ec565b84b69c3f7ebc96c2f3", @"/Pages/Data/_DetailsData.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3f9885ed56a8217391a2bb21100b53de6ade68c4", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Data__DetailsData : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ge_repository.Models.ge_data>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "../Project/Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            BeginContext(0, 1, true);
            WriteLiteral(" ");
            EndContext();
            BeginContext(39, 29, true);
            WriteLiteral(" \r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(69, 48, false);
#line 4 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.project.name));

#line default
#line hidden
            EndContext();
            BeginContext(117, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(160, 144, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "42df8eb147bd477aae2834788b42ff99", async() => {
                BeginContext(225, 15, true);
                WriteLiteral("\r\n             ");
                EndContext();
                BeginContext(241, 44, false);
#line 8 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
        Write(Html.DisplayFor(model => model.project.name));

#line default
#line hidden
                EndContext();
                BeginContext(285, 15, true);
                WriteLiteral("\r\n             ");
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
#line 7 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
                                               WriteLiteral(Model.projectId);

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
            BeginContext(304, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(348, 44, false);
#line 12 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.filename));

#line default
#line hidden
            EndContext();
            BeginContext(392, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(436, 40, false);
#line 15 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayFor(model => model.filename));

#line default
#line hidden
            EndContext();
            BeginContext(476, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(520, 44, false);
#line 18 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.filesize));

#line default
#line hidden
            EndContext();
            BeginContext(564, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(608, 40, false);
#line 21 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayFor(model => model.filesize));

#line default
#line hidden
            EndContext();
            BeginContext(648, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(692, 44, false);
#line 24 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.filetype));

#line default
#line hidden
            EndContext();
            BeginContext(736, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(780, 40, false);
#line 27 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayFor(model => model.filetype));

#line default
#line hidden
            EndContext();
            BeginContext(820, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(864, 44, false);
#line 30 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.encoding));

#line default
#line hidden
            EndContext();
            BeginContext(908, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(952, 40, false);
#line 33 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayFor(model => model.encoding));

#line default
#line hidden
            EndContext();
            BeginContext(992, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1036, 44, false);
#line 36 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.filedate));

#line default
#line hidden
            EndContext();
            BeginContext(1080, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1124, 40, false);
#line 39 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayFor(model => model.filedate));

#line default
#line hidden
            EndContext();
            BeginContext(1164, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1208, 47, false);
#line 42 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.description));

#line default
#line hidden
            EndContext();
            BeginContext(1255, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1299, 43, false);
#line 45 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayFor(model => model.description));

#line default
#line hidden
            EndContext();
            BeginContext(1342, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1386, 44, false);
#line 48 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.keywords));

#line default
#line hidden
            EndContext();
            BeginContext(1430, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1474, 40, false);
#line 51 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayFor(model => model.keywords));

#line default
#line hidden
            EndContext();
            BeginContext(1514, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1558, 43, false);
#line 54 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.cstatus));

#line default
#line hidden
            EndContext();
            BeginContext(1601, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1645, 39, false);
#line 57 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayFor(model => model.cstatus));

#line default
#line hidden
            EndContext();
            BeginContext(1684, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1728, 43, false);
#line 60 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.pstatus));

#line default
#line hidden
            EndContext();
            BeginContext(1771, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1815, 39, false);
#line 63 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayFor(model => model.pstatus));

#line default
#line hidden
            EndContext();
            BeginContext(1854, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1898, 43, false);
#line 66 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayNameFor(model => model.qstatus));

#line default
#line hidden
            EndContext();
            BeginContext(1941, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1985, 39, false);
#line 69 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Data\_DetailsData.cshtml"
       Write(Html.DisplayFor(model => model.qstatus));

#line default
#line hidden
            EndContext();
            BeginContext(2024, 15, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ge_repository.Models.ge_data> Html { get; private set; }
    }
}
#pragma warning restore 1591
