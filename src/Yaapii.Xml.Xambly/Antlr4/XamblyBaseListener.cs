//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Daten\GITHUB\Yaapii.Xambly\src\Yaapii.Xml.Xambly\\Antlr4\Xambly.g by ANTLR 4.6

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
using Yaapii.Xml.Xambly;
using Yaapii.Xml.Xambly.Arg;
using Yaapii.Xml.Xambly.Error;
using Yaapii.Xml.Xambly.Directive;


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IXamblyListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6")]
[System.CLSCompliant(false)]
public partial class XamblyBaseListener : IXamblyListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="XamblyParser.directives"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDirectives([NotNull] XamblyParser.DirectivesContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="XamblyParser.directives"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDirectives([NotNull] XamblyParser.DirectivesContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="XamblyParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDirective([NotNull] XamblyParser.DirectiveContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="XamblyParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDirective([NotNull] XamblyParser.DirectiveContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="XamblyParser.argument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArgument([NotNull] XamblyParser.ArgumentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="XamblyParser.argument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArgument([NotNull] XamblyParser.ArgumentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="XamblyParser.label"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLabel([NotNull] XamblyParser.LabelContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="XamblyParser.label"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLabel([NotNull] XamblyParser.LabelContext context) { }

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
