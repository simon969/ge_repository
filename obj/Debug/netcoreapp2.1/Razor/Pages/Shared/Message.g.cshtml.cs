#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\Message.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "609bdd3e0ee277cbbbd0842282fb01e259bf46af"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ge_repository.Pages.Shared.Pages_Shared_Message), @"mvc.1.0.razor-page", @"/Pages/Shared/Message.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/Shared/Message.cshtml", typeof(ge_repository.Pages.Shared.Pages_Shared_Message), null)]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"609bdd3e0ee277cbbbd0842282fb01e259bf46af", @"/Pages/Shared/Message.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3f9885ed56a8217391a2bb21100b53de6ade68c4", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Shared_Message : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\Message.cshtml"
  
   Layout = null;

#line default
#line hidden
            BeginContext(54, 1055, true);
            WriteLiteral(@"<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"">
<script src=""https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js""></script>
<script src=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js""></script>
 
<!-- Trigger the modal with a button -->
<!-- <button type=""button"" class=""btn btn-info btn-lg"" data-toggle=""modal"" data-target=""#myModal"">Open Modal</button>
<asp:Button ID=""ServerButton"" runat=""server"" Text=""Launch Modal Popup (Server)"" 
 OnClick=""ServerButton_Click"" /> -->
<!-- Modal -->

<div id=""myModal"" class=""modal fade"" role=""dialog"" data-toggle=""modal"">
    <div class=""modal-dialog"">

        <!-- Modal content-->
        <div class=""modal-content"">
            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal"">&times;</button>
                <h4 class=""modal-title"">Event Message</h4>
            </div>
            <div class=""modal-body"">
             ");
            WriteLiteral("<h1 class=\"text-danger\"></h1>\r\n");
            EndContext();
#line 27 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\Message.cshtml"
                 if (Model.log!=null) {
                    

#line default
#line hidden
#line 28 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\Message.cshtml"
                     if (Model.log.level == logLEVEL.Error) {

#line default
#line hidden
            BeginContext(1213, 99, true);
            WriteLiteral("                    <h2 class=\"text-danger\">An error occurred while processing your request.</h2>\r\n");
            EndContext();
#line 30 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\Message.cshtml"
                    }

#line default
#line hidden
            BeginContext(1335, 62, true);
            WriteLiteral("                <h3 class=\"text-danger\">\r\n                    ");
            EndContext();
            BeginContext(1398, 17, false);
#line 32 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\Message.cshtml"
               Write(Model.log.message);

#line default
#line hidden
            EndContext();
            BeginContext(1415, 25, true);
            WriteLiteral("\r\n                </h3>\r\n");
            EndContext();
#line 34 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\Message.cshtml"
                }

#line default
#line hidden
            BeginContext(1459, 63, true);
            WriteLiteral("                <h3 class=\"text-context\">\r\n                    ");
            EndContext();
            BeginContext(1523, 19, false);
#line 36 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\Message.cshtml"
               Write(ViewData["Context"]);

#line default
#line hidden
            EndContext();
            BeginContext(1542, 89, true);
            WriteLiteral("\r\n                </h3>\r\n\r\n                <h3 class=\"text-danger\">\r\n                    ");
            EndContext();
            BeginContext(1632, 15, false);
#line 40 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Pages\Shared\Message.cshtml"
               Write(ViewData["Msg"]);

#line default
#line hidden
            EndContext();
            BeginContext(1647, 447, true);
            WriteLiteral(@"
                </h3>
            </div>
            <div class=""modal-footer"">
                <button type=""button"" class=""btn btn-default"" data-dismiss=""modal"">Close</button>
            </div>
        </div>

    </div>
</div>
<script>
        $(function () {
            $(""#myModal"").modal();
        });
        $('#myModal').on('hide.bs.modal', function(){
        history.go(-1)
        //do your stuff
})
    </script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<MessageModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<MessageModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<MessageModel>)PageContext?.ViewData;
        public MessageModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
