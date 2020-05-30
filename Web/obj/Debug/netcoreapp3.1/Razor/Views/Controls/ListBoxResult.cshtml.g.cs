#pragma checksum "/home/dev/back_labs/tsu_web_back/Web/Views/Controls/ListBoxResult.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c2fe8cd9ae4e7c3dc7bdd49301380373ca0c333c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Controls_ListBoxResult), @"mvc.1.0.view", @"/Views/Controls/ListBoxResult.cshtml")]
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
#nullable restore
#line 1 "/home/dev/back_labs/tsu_web_back/Web/Views/Controls/ListBoxResult.cshtml"
using Web.Forms;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c2fe8cd9ae4e7c3dc7bdd49301380373ca0c333c", @"/Views/Controls/ListBoxResult.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"243bef8901b38e9eef9e38f8c66b8f401f171c9b", @"/Views/_ViewImports.cshtml")]
    public class Views_Controls_ListBoxResult : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Web.Forms.ListBoxViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "/home/dev/back_labs/tsu_web_back/Web/Views/Controls/ListBoxResult.cshtml"
  
    ViewBag.Title = "List Box";
    var allMonths = ViewBag.AllMonths as Month[];
    var montsMap = allMonths.ToDictionary(x => x.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<h2>List Box</h2>\n<dl class=\"dl-horizontal\">\n    <dt>Months</dt>\n");
#nullable restore
#line 12 "/home/dev/back_labs/tsu_web_back/Web/Views/Controls/ListBoxResult.cshtml"
     if (Model.Months != null && Model.Months.Length > 0)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <dd>");
#nullable restore
#line 14 "/home/dev/back_labs/tsu_web_back/Web/Views/Controls/ListBoxResult.cshtml"
       Write(String.Join(", ", Model.Months.Select(x => montsMap[x].Name)));

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\n");
#nullable restore
#line 15 "/home/dev/back_labs/tsu_web_back/Web/Views/Controls/ListBoxResult.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <dd>(none)</dd>\n");
#nullable restore
#line 19 "/home/dev/back_labs/tsu_web_back/Web/Views/Controls/ListBoxResult.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</dl>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Web.Forms.ListBoxViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
