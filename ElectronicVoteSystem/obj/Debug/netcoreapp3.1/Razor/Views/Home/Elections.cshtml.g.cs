#pragma checksum "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e7eb5506bf7cbc8f86eba5ab9a478957a18718a2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Elections), @"mvc.1.0.view", @"/Views/Home/Elections.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\_ViewImports.cshtml"
using ElectronicVoteSystem;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\_ViewImports.cshtml"
using ElectronicVoteSystem.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
using System.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
using Microsoft.EntityFrameworkCore;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e7eb5506bf7cbc8f86eba5ab9a478957a18718a2", @"/Views/Home/Elections.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b1b7d3aa13ef8d455f311b5001b5e3769a9feb37", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Elections : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<ElectronicVoteSystem.Models.Election>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "BallotPapers", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("list-group-item list-group-item-action text-center disabled  "), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("list-group-item list-group-item-action text-center"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#nullable restore
#line 4 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
  
    ViewData["Title"] = "Elections";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Electciones</h1>\r\n\r\n<div class=\"list-group\">\r\n    <a href=\"#\" class=\"list-group-item list-group-item-action active\"> Elecciones disponibles </a>\r\n\r\n\r\n\r\n\r\n\r\n");
#nullable restore
#line 17 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
     foreach (var election in Model)
    {
        using (ElectronicVotingContext db = new ElectronicVotingContext())
        {
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 21 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
             if (db.BallotPaper.Include(b => b.Candidate).Where(c => c.Candidate.Status == true).Include(b => b.Election).Where(e => e.ElectionId == election.Id).ToList().Count < 2)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e7eb5506bf7cbc8f86eba5ab9a478957a18718a25844", async() => {
#nullable restore
#line 23 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
                                                                                                                                      Write(election.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral(" --- No se puede proceder con la votación, solo tiene un candidato");
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
#line 23 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
                                           WriteLiteral(election.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 24 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
            }
            else
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e7eb5506bf7cbc8f86eba5ab9a478957a18718a28778", async() => {
#nullable restore
#line 27 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
                                                                                                                               Write(election.Name);

#line default
#line hidden
#nullable disable
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
#line 27 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
                                               WriteLiteral(election.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 28 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 28 "C:\Users\heili\OneDrive\C# II\ElectronicVoteSystem\ElectronicVoteSystem\Views\Home\Elections.cshtml"
             
        }


    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<ElectronicVoteSystem.Models.Election>> Html { get; private set; }
    }
}
#pragma warning restore 1591
