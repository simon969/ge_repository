#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Test\drag_n_drop.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e314d2adc17a7c74b2566d9c870d7914e5ed7f94"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Test_drag_n_drop), @"mvc.1.0.view", @"/Views/Test/drag_n_drop.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Test/drag_n_drop.cshtml", typeof(AspNetCore.Views_Test_drag_n_drop))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e314d2adc17a7c74b2566d9c870d7914e5ed7f94", @"/Views/Test/drag_n_drop.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9430c914460544b2bbe4f0e24f2bead87399a60e", @"/Views/_ViewImports.cshtml")]
    public class Views_Test_drag_n_drop : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 437, true);
            WriteLiteral(@"<div id=""drop-area"">
  <form class=""my-form"">
    <p>Upload multiple files with the file dialog or by dragging and dropping images onto the dashed region</p>
    <input type=""file"" id=""fileElem"" multiple accept=""image/*"" onchange=""handleFiles(this.files)"">
    <label class=""button"" for=""fileElem"">Select some files</label>
  </form>
  <progress id=""progress-bar"" max=100 value=0></progress>
  <div id=""gallery"" /></div>
</div>
");
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
