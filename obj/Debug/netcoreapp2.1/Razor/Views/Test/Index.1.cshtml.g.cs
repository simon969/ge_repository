#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Test\Index.1.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "09dee6f964ee7723cf502034231d8d881b99b5d6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Test_Index_1), @"mvc.1.0.view", @"/Views/Test/Index.1.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Test/Index.1.cshtml", typeof(AspNetCore.Views_Test_Index_1))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09dee6f964ee7723cf502034231d8d881b99b5d6", @"/Views/Test/Index.1.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9430c914460544b2bbe4f0e24f2bead87399a60e", @"/Views/_ViewImports.cshtml")]
    public class Views_Test_Index_1 : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Test\Index.1.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
            BeginContext(45, 1142, true);
            WriteLiteral(@"<h1>Upload Files</h1>
<div class=""row"">
    <section>
        <form method=""post"" enctype=""multipart/form-data""
              asp-controller=""ge_data"" asp-action=""Post"">
             <div class=""form-group"">
                <div class=""col-md-10"">
                    <p>Project Id:</p>
                    <input type=""text"" name=""ProjectId"" value=""BEC5E41A-3765-4024-AB9E-08D6545F8FADM""/>
                </div>
            </div>  
            <div class=""form-group"">
                <div class=""col-md-10"">
                    <p>Upload one or more files using this form:</p>
                    <input type=""file"" name=""files"" multiple />
                </div>
            </div>
            <div class=""form-group"">
                <div class=""col-md-10"">
                    <input type=""submit"" value=""Upload"" />
                </div>
            </div>
        </form>
    </section>
</div>
<hr />
<h3>Other Samples</h3>
<ul>
    <li><a asp-controller=""Profile"" asp-action=""Index"">Pro");
            WriteLiteral("file Sample</a></li>\r\n    <li><a asp-controller=\"Streaming\" asp-action=\"Index\">Streaming Upload Sample</a></li>\r\n</ul>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
