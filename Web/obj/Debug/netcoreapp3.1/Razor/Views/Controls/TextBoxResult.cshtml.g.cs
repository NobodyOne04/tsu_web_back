#pragma checksum "/home/dev/back_labs/tsu_web_back/Web/Views/Controls/TextBoxResult.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b39127077a8a866694393ab0da60979891873580"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Controls_TextBoxResult), @"mvc.1.0.view", @"/Views/Controls/TextBoxResult.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b39127077a8a866694393ab0da60979891873580", @"/Views/Controls/TextBoxResult.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7da9b87146674b93145a18d15ddfbde8744613bd", @"/Views/_ViewImports.cshtml")]
    public class Views_Controls_TextBoxResult : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Web.Forms.TextBoxViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "/home/dev/back_labs/tsu_web_back/Web/Views/Controls/TextBoxResult.cshtml"
  
    ViewBag.Title = "Text Box";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<h2>Text Box</h2>\n<dl class=\"dl-horizontal\">\n    <dt>Text</dt>\n    <dd>");
#nullable restore
#line 9 "/home/dev/back_labs/tsu_web_back/Web/Views/Controls/TextBoxResult.cshtml"
   Write(Model.Text);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\n</dl>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Web.Forms.TextBoxViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
