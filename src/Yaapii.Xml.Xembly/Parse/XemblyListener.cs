//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Xembly.g by ANTLR 4.7

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


    
    import java.util.Collection;
    import java.util.LinkedList;

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="XemblyParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public interface IXemblyListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="XemblyParser.directives"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDirectives([NotNull] XemblyParser.DirectivesContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="XemblyParser.directives"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDirectives([NotNull] XemblyParser.DirectivesContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="XemblyParser.directive"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDirective([NotNull] XemblyParser.DirectiveContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="XemblyParser.directive"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDirective([NotNull] XemblyParser.DirectiveContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="XemblyParser.argument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgument([NotNull] XemblyParser.ArgumentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="XemblyParser.argument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgument([NotNull] XemblyParser.ArgumentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="XemblyParser.label"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLabel([NotNull] XemblyParser.LabelContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="XemblyParser.label"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLabel([NotNull] XemblyParser.LabelContext context);
}
