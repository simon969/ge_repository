#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "99ac6abd4cf9d04740109992f14512701c6ddc28"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ge_repository.Pages.Shared.Pages_Shared__DetailsBase), @"mvc.1.0.view", @"/Pages/Shared/_DetailsBase.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Pages/Shared/_DetailsBase.cshtml", typeof(ge_repository.Pages.Shared.Pages_Shared__DetailsBase))]
namespace ge_repository.Pages.Shared
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"99ac6abd4cf9d04740109992f14512701c6ddc28", @"/Pages/Shared/_DetailsBase.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3f9885ed56a8217391a2bb21100b53de6ade68c4", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Shared__DetailsBase : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ge_repository.Models._ge_base>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 1, true);
            WriteLiteral(" ");
            EndContext();
            BeginContext(43, 67, true);
            WriteLiteral("        \r\n        <div id=_DetailsBase>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(111, 46, false);
#line 5 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
       Write(Html.DisplayNameFor(model => model.operations));

#line default
#line hidden
            EndContext();
            BeginContext(157, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(201, 42, false);
#line 8 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
       Write(Html.DisplayFor(model => model.operations));

#line default
#line hidden
            EndContext();
            BeginContext(243, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(287, 45, false);
#line 11 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
       Write(Html.DisplayNameFor(model => model.createdId));

#line default
#line hidden
            EndContext();
            BeginContext(332, 33, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>  \r\n");
            EndContext();
#line 14 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
             if (@Model.created!=null) {
             

#line default
#line hidden
            BeginContext(421, 24, false);
#line 15 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
        Write(Model.created.FullName());

#line default
#line hidden
            EndContext();
#line 15 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
                                      ;
            }

#line default
#line hidden
            BeginContext(463, 41, true);
            WriteLiteral("        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(505, 45, false);
#line 19 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
       Write(Html.DisplayNameFor(model => model.createdDT));

#line default
#line hidden
            EndContext();
            BeginContext(550, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(594, 41, false);
#line 22 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
       Write(Html.DisplayFor(model => model.createdDT));

#line default
#line hidden
            EndContext();
            BeginContext(635, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(679, 44, false);
#line 25 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
       Write(Html.DisplayNameFor(model => model.editedId));

#line default
#line hidden
            EndContext();
            BeginContext(723, 31, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n");
            EndContext();
#line 28 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
             if (@Model.edited!=null) {
                

#line default
#line hidden
            BeginContext(812, 23, false);
#line 29 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
           Write(Model.edited.FullName());

#line default
#line hidden
            EndContext();
#line 29 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
                                        ;
            }

#line default
#line hidden
            BeginContext(853, 41, true);
            WriteLiteral("        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(895, 44, false);
#line 33 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
       Write(Html.DisplayNameFor(model => model.editedDT));

#line default
#line hidden
            EndContext();
            BeginContext(939, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(983, 40, false);
#line 36 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\_DetailsBase.cshtml"
       Write(Html.DisplayFor(model => model.editedDT));

#line default
#line hidden
            EndContext();
            BeginContext(1023, 31, true);
            WriteLiteral("\r\n        </dd>\r\n        </div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ge_repository.Models._ge_base> Html { get; private set; }
    }
}
#pragma warning restore 1591
