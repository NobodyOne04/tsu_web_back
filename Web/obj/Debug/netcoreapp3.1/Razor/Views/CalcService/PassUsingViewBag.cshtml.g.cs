#pragma checksum "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "51b1d6457c5885ceee7712048721fd529bb4c18b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CalcService_PassUsingViewBag), @"mvc.1.0.view", @"/Views/CalcService/PassUsingViewBag.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"51b1d6457c5885ceee7712048721fd529bb4c18b", @"/Views/CalcService/PassUsingViewBag.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"243bef8901b38e9eef9e38f8c66b8f401f171c9b", @"/Views/_ViewImports.cshtml")]
    public class Views_CalcService_PassUsingViewBag : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
  
    ViewData["Title"] = "PassUsingViewBag";
    ViewData["h3"] = "Pass Using View Bag";

#line default
#line hidden
#nullable disable
            WriteLiteral("<!DOCTYPE html>\n<html>\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "51b1d6457c5885ceee7712048721fd529bb4c18b3441", async() => {
                WriteLiteral("\n    <title></title>\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "51b1d6457c5885ceee7712048721fd529bb4c18b4410", async() => {
                WriteLiteral("\n");
#nullable restore
#line 11 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
      
        var value = ViewBag.Value;
    

#line default
#line hidden
#nullable disable
                WriteLiteral("    <h6>Rand Value First : ");
#nullable restore
#line 14 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
                      Write(value.randValueFirst);

#line default
#line hidden
#nullable disable
                WriteLiteral("</h6>\n    <h6>Rand Value Second : ");
#nullable restore
#line 15 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
                       Write(value.randValueSecond);

#line default
#line hidden
#nullable disable
                WriteLiteral("</h6>\n    <h6>Add : </h6>");
#nullable restore
#line 16 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
               Write($"{value.randValueFirst} + {value.randValueSecond} = ");

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
                                                                        Write(value.randValueFirst + value.randValueSecond);

#line default
#line hidden
#nullable disable
                WriteLiteral("\n    <h6>Sub : </h6>");
#nullable restore
#line 17 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
               Write($"{value.randValueFirst} - {value.randValueSecond} = ");

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
                                                                        Write(value.randValueFirst - value.randValueSecond);

#line default
#line hidden
#nullable disable
                WriteLiteral("\n    <h6>Mult : </h6>");
#nullable restore
#line 18 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
                Write($"{value.randValueFirst} * {value.randValueSecond} = ");

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
                                                                         Write(value.randValueFirst * value.randValueSecond);

#line default
#line hidden
#nullable disable
                WriteLiteral("\n");
#nullable restore
#line 19 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
      
        string div;
        if (value.randValueSecond == 0)
        {
            div = "inf";
        }
        else
        {
            div = (value.randValueFirst / value.randValueSecond).ToString();
        }
    

#line default
#line hidden
#nullable disable
                WriteLiteral("    <h6>Div : </h6>");
#nullable restore
#line 30 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
               Write($"{value.randValueFirst} / {value.randValueSecond} = ");

#line default
#line hidden
#nullable disable
#nullable restore
#line 30 "/home/dev/back_labs/tsu_web_back/Web/Views/CalcService/PassUsingViewBag.cshtml"
                                                                       Write(div);

#line default
#line hidden
#nullable disable
                WriteLiteral("\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n</html>\n\n");
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
