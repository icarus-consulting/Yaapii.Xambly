//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\mtu.ICARUS-CONSULT\Documents\Projekte\Yaapii.Xembly\src\Yaapii.Xml.Xembly\\Antlr4\Xembly.g by ANTLR 4.7

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


using System.Xml;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xembly;
using Yaapii.Xml.Xembly.Arg;
using Yaapii.Xml.Xembly.Error;
using Yaapii.Xml.Xembly.Directive;


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IXemblyListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public partial class XemblyBaseListener : IXemblyListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="XemblyParser.directives"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDirectives([NotNull] XemblyParser.DirectivesContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="XemblyParser.directives"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDirectives([NotNull] XemblyParser.DirectivesContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="XemblyParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDirective([NotNull] XemblyParser.DirectiveContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="XemblyParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDirective([NotNull] XemblyParser.DirectiveContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="XemblyParser.argument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArgument([NotNull] XemblyParser.ArgumentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="XemblyParser.argument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArgument([NotNull] XemblyParser.ArgumentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="XemblyParser.label"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLabel([NotNull] XemblyParser.LabelContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="XemblyParser.label"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLabel([NotNull] XemblyParser.LabelContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}