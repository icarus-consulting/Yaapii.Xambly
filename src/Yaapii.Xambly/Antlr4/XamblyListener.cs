//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Projekte_VisualStudio\ContainsInGitHub\Yaapii.Xembly\src\Yaapii.Xambly\\Antlr4\Xambly.g by ANTLR 4.6

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
using Yaapii.Xambly;
using Yaapii.Xambly.Arg;
using Yaapii.Xambly.Error;
using Yaapii.Xambly.Directive;

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="XamblyParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6")]
[System.CLSCompliant(false)]
public interface IXamblyListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="XamblyParser.directives"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDirectives([NotNull] XamblyParser.DirectivesContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="XamblyParser.directives"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDirectives([NotNull] XamblyParser.DirectivesContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="XamblyParser.directive"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDirective([NotNull] XamblyParser.DirectiveContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="XamblyParser.directive"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDirective([NotNull] XamblyParser.DirectiveContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="XamblyParser.argument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgument([NotNull] XamblyParser.ArgumentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="XamblyParser.argument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgument([NotNull] XamblyParser.ArgumentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="XamblyParser.label"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLabel([NotNull] XamblyParser.LabelContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="XamblyParser.label"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLabel([NotNull] XamblyParser.LabelContext context);
}
