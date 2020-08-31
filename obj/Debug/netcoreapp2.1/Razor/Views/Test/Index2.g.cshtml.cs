#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Test\Index2.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fbb8e910a7edd602cffd0a8699c89a84835e90d0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Test_Index2), @"mvc.1.0.view", @"/Views/Test/Index2.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Test/Index2.cshtml", typeof(AspNetCore.Views_Test_Index2))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fbb8e910a7edd602cffd0a8699c89a84835e90d0", @"/Views/Test/Index2.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9430c914460544b2bbe4f0e24f2bead87399a60e", @"/Views/_ViewImports.cshtml")]
    public class Views_Test_Index2 : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Scripts/jquery.filedrop.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 4, true);
            WriteLiteral("    ");
            EndContext();
#line 1 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Test\Index2.cshtml"
      
        ViewBag.Title = "Index";
    

#line default
#line hidden
            BeginContext(49, 652, true);
            WriteLiteral(@"     
    <h2>Drag & Drop file upload </h2>
    <div id=""dropArea"">
        Drop your files here
    </div>
    <h4>Uploaded files : </h4>
    <ul class=""list-group"" id=""uploadList"">
     
    </ul>
     
    <style>
        #dropArea{
            background:#b5b5b5;
            border:black dashed 1px;
            height:50px;
            text-align:center;
            color:#fff;
            padding-top:12px;
        }
        .active-drop{
            background:#77bafa !important;
            border:solid 2px blue !important;
            opacity:.5;
            color:black !important;
        }
    </style>
     
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(722, 14, true);
                WriteLiteral("    \r\n        ");
                EndContext();
                BeginContext(736, 52, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b23e1810a0b34a22b4439ba3ea5a9090", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(788, 141, true);
                WriteLiteral("\r\n        <script type=\"text/javascript\">\r\n            $(function () {\r\n                $(\'#dropArea\').filedrop({\r\n                    url: \'");
                EndContext();
                BeginContext(930, 25, false);
#line 36 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Test\Index2.cshtml"
                     Write(Url.Action("UploadFiles"));

#line default
#line hidden
                EndContext();
                BeginContext(955, 1094, true);
                WriteLiteral(@"',
                    allowedfiletypes: ['image/jpeg', 'image/png', 'image/gif'],
                    allowedfileextensions: ['.jpg', '.jpeg', '.png', '.gif'],
                    paramname: 'files',
                    maxfiles: 5,
                    maxfilesize: 5, // in MB
                    dragOver: function () {
                        $('#dropArea').addClass('active-drop');
                    },
                    dragLeave: function () {
                        $('#dropArea').removeClass('active-drop');
                    },
                    drop: function () {
                        $('#dropArea').removeClass('active-drop');
                    },
                    afterAll: function (e) {
                        $('#dropArea').html('file(s) uploaded successfully');
                    },
                    uploadFinished: function (i, file, response, time) {
                        $('#uploadList').append('<li class=""list-group-item"">'+file.name+'</li>')
            ");
                WriteLiteral("        }\r\n                })\r\n            })\r\n        </script>\r\n    ");
                EndContext();
            }
            );
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
