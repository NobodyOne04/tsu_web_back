#pragma checksum "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "39e87d11585cfe2af1f2af5ccbda68146c095d93"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ForumMessages_Details), @"mvc.1.0.view", @"/Views/ForumMessages/Details.cshtml")]
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
#line 3 "/home/dev/back_labs/tsu_web_back/Web/Views/_ViewImports.cshtml"
using Web.Models.AccountViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "/home/dev/back_labs/tsu_web_back/Web/Views/_ViewImports.cshtml"
using Web.Models.ManageViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "/home/dev/back_labs/tsu_web_back/Web/Views/_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"39e87d11585cfe2af1f2af5ccbda68146c095d93", @"/Views/ForumMessages/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"51e107879ebb7343e2ff892d1a05aa50005a835e", @"/Views/_ViewImports.cshtml")]
    public class Views_ForumMessages_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Web.Models.ForumMessage>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
#nullable restore
#line 3 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
  
    ViewData["Title"] = "Details";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<h2>Details</h2>\n\n<div>\n    <h4>ForumMessage</h4>\n    <hr />\n    <dl class=\"dl-horizontal\">\n        <dt>\n            ");
#nullable restore
#line 14 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Created));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
#nullable restore
#line 17 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
       Write(Html.DisplayFor(model => model.Created));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
#nullable restore
#line 20 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Modified));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
#nullable restore
#line 23 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
       Write(Html.DisplayFor(model => model.Modified));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
#nullable restore
#line 26 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Text));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
#nullable restore
#line 29 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
       Write(Html.DisplayFor(model => model.Text));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
#nullable restore
#line 32 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ApplicationUser));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
#nullable restore
#line 35 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
       Write(Html.DisplayFor(model => model.ApplicationUser.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </dd>\n        <dt>\n            ");
#nullable restore
#line 38 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ForumTopic));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </dt>\n        <dd>\n            ");
#nullable restore
#line 41 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
       Write(Html.DisplayFor(model => model.ForumTopic.ApplicationUserId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n        </dd>\n    </dl>\n</div>\n<div>\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "39e87d11585cfe2af1f2af5ccbda68146c095d937279", async() => {
                WriteLiteral("Edit");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 46 "/home/dev/back_labs/tsu_web_back/Web/Views/ForumMessages/Details.cshtml"
                           WriteLiteral(Model.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(" |\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "39e87d11585cfe2af1f2af5ccbda68146c095d939380", async() => {
                WriteLiteral("Back to List");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n</div>\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Web.Models.ForumMessage> Html { get; private set; }
    }
}
#pragma warning restore 1591
