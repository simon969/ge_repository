#pragma checksum "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4f88c4f8c4cdb9e8f3fd0c22323b8a70b704fef7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_OperationRequest), @"mvc.1.0.view", @"/Views/Shared/OperationRequest.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/OperationRequest.cshtml", typeof(AspNetCore.Views_Shared_OperationRequest))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4f88c4f8c4cdb9e8f3fd0c22323b8a70b704fef7", @"/Views/Shared/OperationRequest.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9430c914460544b2bbe4f0e24f2bead87399a60e", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_OperationRequest : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ge_repository.Models.operation_request>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(47, 1317, true);
            WriteLiteral(@"
<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"">
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
                <h4 class=""modal-title"">Operational Request Details</h4>
            </div>
            <div class=""modal-body""");
            WriteLiteral(@">
             <h1 class=""text-danger""></h1>
             <div>
                 <table class=""table"">
                    <tr> 
                        <td>
                           User Name
                        </td>  
                        <td>
                            ");
            EndContext();
            BeginContext(1365, 22, false);
#line 31 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._user.FullName());

#line default
#line hidden
            EndContext();
            BeginContext(1387, 61, true);
            WriteLiteral(" \r\n                        </td>\r\n                    </tr>\r\n");
            EndContext();
#line 34 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                 if (Model._group != null) {

#line default
#line hidden
            BeginContext(1494, 84, true);
            WriteLiteral("                    <tr>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(1579, 47, false);
#line 37 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Html.DisplayNameFor(model => model._group.name));

#line default
#line hidden
            EndContext();
            BeginContext(1626, 92, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td> \r\n                            ");
            EndContext();
            BeginContext(1719, 17, false);
#line 40 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._group.name);

#line default
#line hidden
            EndContext();
            BeginContext(1736, 142, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>\r\n                          ");
            EndContext();
            BeginContext(1879, 61, false);
#line 45 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                     Write(Html.DisplayNameFor(model => model._group.project_operations));

#line default
#line hidden
            EndContext();
            BeginContext(1940, 93, true);
            WriteLiteral("\r\n                        </td> \r\n                        <td> \r\n                            ");
            EndContext();
            BeginContext(2034, 31, false);
#line 48 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._group.project_operations);

#line default
#line hidden
            EndContext();
            BeginContext(2065, 141, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                    <td>\r\n                             ");
            EndContext();
            BeginContext(2207, 63, false);
#line 53 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                        Write(Html.DisplayNameFor(model => model._group_user.user_operations));

#line default
#line hidden
            EndContext();
            BeginContext(2270, 91, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(2362, 33, false);
#line 56 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._group_user.user_operations);

#line default
#line hidden
            EndContext();
            BeginContext(2395, 61, true);
            WriteLiteral("\r\n                        </td> \r\n                    </tr>\r\n");
            EndContext();
#line 59 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                }

#line default
#line hidden
            BeginContext(2475, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 61 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                 if(Model._project != null) {

#line default
#line hidden
            BeginContext(2524, 85, true);
            WriteLiteral("                    <tr>\r\n                        <td>\r\n                             ");
            EndContext();
            BeginContext(2610, 49, false);
#line 64 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                        Write(Html.DisplayNameFor(model => model._project.name));

#line default
#line hidden
            EndContext();
            BeginContext(2659, 92, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td> \r\n                            ");
            EndContext();
            BeginContext(2752, 19, false);
#line 67 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._project.name);

#line default
#line hidden
            EndContext();
            BeginContext(2771, 145, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>\r\n                             ");
            EndContext();
            BeginContext(2917, 60, false);
#line 72 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                        Write(Html.DisplayNameFor(model => model._project.data_operations));

#line default
#line hidden
            EndContext();
            BeginContext(2977, 92, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td> \r\n                            ");
            EndContext();
            BeginContext(3070, 30, false);
#line 75 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._project.data_operations);

#line default
#line hidden
            EndContext();
            BeginContext(3100, 145, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>\r\n                             ");
            EndContext();
            BeginContext(3246, 65, false);
#line 80 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                        Write(Html.DisplayNameFor(model => model._project_user.user_operations));

#line default
#line hidden
            EndContext();
            BeginContext(3311, 92, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td> \r\n                            ");
            EndContext();
            BeginContext(3404, 35, false);
#line 83 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._project_user.user_operations);

#line default
#line hidden
            EndContext();
            BeginContext(3439, 61, true);
            WriteLiteral(" \r\n                        </td>\r\n                    </tr>\r\n");
            EndContext();
#line 86 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                }

#line default
#line hidden
            BeginContext(3519, 16, true);
            WriteLiteral("                ");
            EndContext();
#line 87 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                 if(@Model._data != null) {

#line default
#line hidden
            BeginContext(3564, 86, true);
            WriteLiteral("                    <tr> \r\n                        <td>\r\n                             ");
            EndContext();
            BeginContext(3651, 50, false);
#line 90 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                        Write(Html.DisplayNameFor(model => model._data.filename));

#line default
#line hidden
            EndContext();
            BeginContext(3701, 92, true);
            WriteLiteral(" \r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(3794, 20, false);
#line 93 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._data.filename);

#line default
#line hidden
            EndContext();
            BeginContext(3814, 144, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(3959, 52, false);
#line 98 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Html.DisplayNameFor(model => model._data.operations));

#line default
#line hidden
            EndContext();
            BeginContext(4011, 92, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td> \r\n                            ");
            EndContext();
            BeginContext(4104, 22, false);
#line 101 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._data.operations);

#line default
#line hidden
            EndContext();
            BeginContext(4126, 61, true);
            WriteLiteral(" \r\n                        </td>\r\n                    </tr>\r\n");
            EndContext();
#line 104 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                }                    

#line default
#line hidden
            BeginContext(4226, 16, true);
            WriteLiteral("                ");
            EndContext();
#line 105 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                 if(@Model._requested_object =="group") {

#line default
#line hidden
            BeginContext(4285, 89, true);
            WriteLiteral("                    <tr>    \r\n                        <td>\r\n                             ");
            EndContext();
            BeginContext(4375, 55, false);
#line 108 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                        Write(Html.DisplayNameFor(model => model._effectiveGroup_ops));

#line default
#line hidden
            EndContext();
            BeginContext(4430, 91, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(4522, 25, false);
#line 111 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._effectiveGroup_ops);

#line default
#line hidden
            EndContext();
            BeginContext(4547, 60, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n");
            EndContext();
#line 114 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                }

#line default
#line hidden
            BeginContext(4626, 16, true);
            WriteLiteral("                ");
            EndContext();
#line 115 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                 if(@Model._requested_object =="project") {

#line default
#line hidden
            BeginContext(4687, 87, true);
            WriteLiteral("                    <tr>    \r\n                        <td>\r\n                           ");
            EndContext();
            BeginContext(4775, 57, false);
#line 118 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                      Write(Html.DisplayNameFor(model => model._effectiveProject_ops));

#line default
#line hidden
            EndContext();
            BeginContext(4832, 91, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(4924, 27, false);
#line 121 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._effectiveProject_ops);

#line default
#line hidden
            EndContext();
            BeginContext(4951, 60, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n");
            EndContext();
#line 124 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                }

#line default
#line hidden
            BeginContext(5030, 16, true);
            WriteLiteral("                ");
            EndContext();
#line 125 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                 if (@Model._requested_object =="data") {

#line default
#line hidden
            BeginContext(5089, 89, true);
            WriteLiteral("                    <tr>    \r\n                        <td>\r\n                             ");
            EndContext();
            BeginContext(5179, 54, false);
#line 128 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                        Write(Html.DisplayNameFor(model => model._effectiveData_ops));

#line default
#line hidden
            EndContext();
            BeginContext(5233, 91, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(5325, 24, false);
#line 131 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._effectiveData_ops);

#line default
#line hidden
            EndContext();
            BeginContext(5349, 60, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n");
            EndContext();
#line 134 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                }

#line default
#line hidden
            BeginContext(5428, 16, true);
            WriteLiteral("                ");
            EndContext();
#line 135 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                 if (@Model._requested_ops != "") {

#line default
#line hidden
            BeginContext(5481, 198, true);
            WriteLiteral("                    <tr>\r\n                        <td>\r\n                            Requested Operations \r\n                        </td>\r\n                        <td>  \r\n                            ");
            EndContext();
            BeginContext(5680, 20, false);
#line 141 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                       Write(Model._requested_ops);

#line default
#line hidden
            EndContext();
            BeginContext(5700, 191, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>\r\n                            Request Result\r\n                        </td>\r\n");
            EndContext();
#line 148 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                         if (@Model._requested_result==false) {

#line default
#line hidden
            BeginContext(5956, 116, true);
            WriteLiteral("                            <td>    \r\n                                FAIL\r\n                            </td>     \r\n");
            EndContext();
#line 152 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                        } else {

#line default
#line hidden
            BeginContext(6106, 114, true);
            WriteLiteral("                            <td>    \r\n                                PASS\r\n                            </td>   \r\n");
            EndContext();
#line 156 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                        }                     

#line default
#line hidden
            BeginContext(6268, 27, true);
            WriteLiteral("                    </tr>\r\n");
            EndContext();
#line 158 "C:\Users\thomsonsj\Documents\Visual Studio Code\ge_repository\Views\Shared\OperationRequest.cshtml"
                }

#line default
#line hidden
            BeginContext(6314, 470, true);
            WriteLiteral(@"                </table>
            </div>
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
    </script>
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ge_repository.Models.operation_request> Html { get; private set; }
    }
}
#pragma warning restore 1591
