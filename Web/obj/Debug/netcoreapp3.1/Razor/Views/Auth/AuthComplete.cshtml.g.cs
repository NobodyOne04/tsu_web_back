#pragma checksum "/home/dev/back_labs/tsu_web_back/Web/Views/Auth/AuthComplete.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6501149c45b4062a329c3614f68e75a1aea8ab3e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Auth_AuthComplete), @"mvc.1.0.view", @"/Views/Auth/AuthComplete.cshtml")]
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
#nullable restore
#line 1 "/home/dev/back_labs/tsu_web_back/Web/Views/_ViewImports.cshtml"
using Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/home/dev/back_labs/tsu_web_back/Web/Views/_ViewImports.cshtml"
using Web.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6501149c45b4062a329c3614f68e75a1aea8ab3e", @"/Views/Auth/AuthComplete.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"243bef8901b38e9eef9e38f8c66b8f401f171c9b", @"/Views/_ViewImports.cshtml")]
    public class Views_Auth_AuthComplete : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Web.Models.ViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
#nullable restore
#line 2 "/home/dev/back_labs/tsu_web_back/Web/Views/Auth/AuthComplete.cshtml"
  
    ViewData["Title"] = "Results";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<h2>Sign Up Result</h2>\n");
            WriteLiteral("\n<ul style=\"list-style-type: none\">\n    <li>\n        ");
#nullable restore
#line 11 "/home/dev/back_labs/tsu_web_back/Web/Views/Auth/AuthComplete.cshtml"
   Write(Html.DisplayFor(item => item.UserInfoModel.FirstName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </li>\n    <li>\n        ");
#nullable restore
#line 14 "/home/dev/back_labs/tsu_web_back/Web/Views/Auth/AuthComplete.cshtml"
   Write(Html.DisplayFor(item => item.UserInfoModel.SecondName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </li>\n    <li>\n        ");
#nullable restore
#line 17 "/home/dev/back_labs/tsu_web_back/Web/Views/Auth/AuthComplete.cshtml"
   Write(Html.DisplayFor(item => item.UserInfoModel.Day));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </li>\n    <li>\n        ");
#nullable restore
#line 20 "/home/dev/back_labs/tsu_web_back/Web/Views/Auth/AuthComplete.cshtml"
   Write(Html.DisplayFor(item => item.UserInfoModel.Month));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </li>\n    <li>\n        ");
#nullable restore
#line 23 "/home/dev/back_labs/tsu_web_back/Web/Views/Auth/AuthComplete.cshtml"
   Write(Html.DisplayFor(item => item.UserInfoModel.Year));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </li>\n    <li>\n        ");
#nullable restore
#line 26 "/home/dev/back_labs/tsu_web_back/Web/Views/Auth/AuthComplete.cshtml"
   Write(Html.DisplayFor(item => item.PrivateUserInfoModel.email));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </li>\n    <li>\n        ");
#nullable restore
#line 29 "/home/dev/back_labs/tsu_web_back/Web/Views/Auth/AuthComplete.cshtml"
   Write(Html.DisplayFor(item => item.PrivateUserInfoModel.password));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </li>\n</ul>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Web.Models.ViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
