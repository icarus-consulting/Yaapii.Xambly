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

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public partial class XemblyParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, DIGIT=14, COMMA=15, COLON=16, SEMICOLON=17, 
		TEXT=18, SPACE=19;
	public const int
		RULE_directives = 0, RULE_directive = 1, RULE_argument = 2, RULE_label = 3;
	public static readonly string[] ruleNames = {
		"directives", "directive", "argument", "label"
	};

	private static readonly string[] _LiteralNames = {
		null, "'XPATH'", "'SET'", "'XSET'", "'ATTR'", "'ADD'", "'ADDIF'", "'REMOVE'", 
		"'STRICT'", "'UP'", "'PI'", "'PUSH'", "'POP'", "'CDATA'", null, "','", 
		"':'", "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, "DIGIT", "COMMA", "COLON", "SEMICOLON", "TEXT", "SPACE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "Xembly.g"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static XemblyParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}


	    @Override
	    public void emitErrorMessage(String msg) {
	        throw new ParsingException(msg);
	    }

		public XemblyParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public XemblyParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}
	public partial class DirectivesContext : ParserRuleContext {
		public Collection<Directive> ret;
		public DirectiveContext _directive;
		public ITerminalNode Eof() { return GetToken(XemblyParser.Eof, 0); }
		public DirectiveContext[] directive() {
			return GetRuleContexts<DirectiveContext>();
		}
		public DirectiveContext directive(int i) {
			return GetRuleContext<DirectiveContext>(i);
		}
		public ITerminalNode[] SEMICOLON() { return GetTokens(XemblyParser.SEMICOLON); }
		public ITerminalNode SEMICOLON(int i) {
			return GetToken(XemblyParser.SEMICOLON, i);
		}
		public LabelContext[] label() {
			return GetRuleContexts<LabelContext>();
		}
		public LabelContext label(int i) {
			return GetRuleContext<LabelContext>(i);
		}
		public ITerminalNode[] COLON() { return GetTokens(XemblyParser.COLON); }
		public ITerminalNode COLON(int i) {
			return GetToken(XemblyParser.COLON, i);
		}
		public DirectivesContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_directives; } }
		public override void EnterRule(IParseTreeListener listener) {
			IXemblyListener typedListener = listener as IXemblyListener;
			if (typedListener != null) typedListener.EnterDirectives(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IXemblyListener typedListener = listener as IXemblyListener;
			if (typedListener != null) typedListener.ExitDirectives(this);
		}
	}

	[RuleVersion(0)]
	public DirectivesContext directives() {
		DirectivesContext _localctx = new DirectivesContext(Context, State);
		EnterRule(_localctx, 0, RULE_directives);
		 _localctx.ret =  new LinkedList<Directive>(); 
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 19;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__1) | (1L << T__2) | (1L << T__3) | (1L << T__4) | (1L << T__5) | (1L << T__6) | (1L << T__7) | (1L << T__8) | (1L << T__9) | (1L << T__10) | (1L << T__11) | (1L << T__12) | (1L << DIGIT))) != 0)) {
				{
				{
				State = 11;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==DIGIT) {
					{
					State = 8; label();
					State = 9; Match(COLON);
					}
				}

				State = 13; _localctx._directive = directive();
				State = 14; Match(SEMICOLON);
				 _localctx.ret.add(_localctx._directive.ret); 
				}
				}
				State = 21;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 22; Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class DirectiveContext : ParserRuleContext {
		public IDirective ret;
		public ArgumentContext _argument;
		public ArgumentContext name;
		public ArgumentContext value;
		public ArgumentContext target;
		public ArgumentContext data;
		public ArgumentContext[] argument() {
			return GetRuleContexts<ArgumentContext>();
		}
		public ArgumentContext argument(int i) {
			return GetRuleContext<ArgumentContext>(i);
		}
		public ITerminalNode COMMA() { return GetToken(XemblyParser.COMMA, 0); }
		public DirectiveContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_directive; } }
		public override void EnterRule(IParseTreeListener listener) {
			IXemblyListener typedListener = listener as IXemblyListener;
			if (typedListener != null) typedListener.EnterDirective(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IXemblyListener typedListener = listener as IXemblyListener;
			if (typedListener != null) typedListener.ExitDirective(this);
		}
	}

	[RuleVersion(0)]
	public DirectiveContext directive() {
		DirectiveContext _localctx = new DirectiveContext(Context, State);
		EnterRule(_localctx, 2, RULE_directive);
		try {
			State = 71;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case T__0:
				EnterOuterAlt(_localctx, 1);
				{
				State = 24; Match(T__0);
				State = 25; _localctx._argument = argument();

				        try {
				            _localctx.ret =  new XpathDirective(_localctx._argument.ret.toString());
				        } catch (final XmlContentException ex) {
				            throw new ParsingException(ex);
				        }
				    
				}
				break;
			case T__1:
				EnterOuterAlt(_localctx, 2);
				{
				State = 28; Match(T__1);
				State = 29; _localctx._argument = argument();

				        try {
				            _localctx.ret =  new SetDirective(_localctx._argument.ret.toString());
				        } catch (final XmlContentException ex) {
				            throw new ParsingException(ex);
				        }
				    
				}
				break;
			case T__2:
				EnterOuterAlt(_localctx, 3);
				{
				State = 32; Match(T__2);
				State = 33; _localctx._argument = argument();

				        try {
				            _localctx.ret =  new XsetDirective(_localctx._argument.ret.toString());
				        } catch (final XmlContentException ex) {
				            throw new ParsingException(ex);
				        }
				    
				}
				break;
			case T__3:
				EnterOuterAlt(_localctx, 4);
				{
				State = 36; Match(T__3);
				State = 37; _localctx.name = argument();
				State = 38; Match(COMMA);
				State = 39; _localctx.value = argument();

				        try {
				            _localctx.ret =  new AttrDirective(_localctx.name.ret.toString(), _localctx.value.ret.toString());
				        } catch (final XmlContentException ex) {
				            throw new ParsingException(ex);
				        }
				    
				}
				break;
			case T__4:
				EnterOuterAlt(_localctx, 5);
				{
				State = 42; Match(T__4);
				State = 43; _localctx._argument = argument();

				        try {
				            _localctx.ret =  new AddDirective(_localctx._argument.ret.toString());
				        } catch (final XmlContentException ex) {
				            throw new ParsingException(ex);
				        }
				    
				}
				break;
			case T__5:
				EnterOuterAlt(_localctx, 6);
				{
				State = 46; Match(T__5);
				State = 47; _localctx._argument = argument();

				        try {
				            _localctx.ret =  new AddIfDirective(_localctx._argument.ret.toString());
				        } catch (final XmlContentException ex) {
				            throw new ParsingException(ex);
				        }
				    
				}
				break;
			case T__6:
				EnterOuterAlt(_localctx, 7);
				{
				State = 50; Match(T__6);

				        _localctx.ret =  new RemoveDirective();
				    
				}
				break;
			case T__7:
				EnterOuterAlt(_localctx, 8);
				{
				State = 52; Match(T__7);
				State = 53; _localctx._argument = argument();

				        _localctx.ret =  new StrictDirective(Integer.parseInt(_localctx._argument.ret.toString()));
				    
				}
				break;
			case T__8:
				EnterOuterAlt(_localctx, 9);
				{
				State = 56; Match(T__8);

				        _localctx.ret =  new UpDirective();
				    
				}
				break;
			case T__9:
				EnterOuterAlt(_localctx, 10);
				{
				State = 58; Match(T__9);
				State = 59; _localctx.target = argument();
				State = 60; _localctx.data = argument();

				        try {
				            _localctx.ret =  new PiDirective(_localctx.target.ret.toString(), _localctx.data.ret.toString());
				        } catch (final XmlContentException ex) {
				            throw new ParsingException(ex);
				        }
				    
				}
				break;
			case T__10:
				EnterOuterAlt(_localctx, 11);
				{
				State = 63; Match(T__10);

				        _localctx.ret =  new PushDirective();
				    
				}
				break;
			case T__11:
				EnterOuterAlt(_localctx, 12);
				{
				State = 65; Match(T__11);

				        _localctx.ret =  new PopDirective();
				    
				}
				break;
			case T__12:
				EnterOuterAlt(_localctx, 13);
				{
				State = 67; Match(T__12);
				State = 68; _localctx._argument = argument();

				        try {
				            _localctx.ret =  new CdataDirective(_localctx._argument.ret.toString());
				        } catch (final XmlContentException ex) {
				            throw new ParsingException(ex);
				        }
				    
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ArgumentContext : ParserRuleContext {
		public Object ret;
		public IToken _TEXT;
		public ITerminalNode TEXT() { return GetToken(XemblyParser.TEXT, 0); }
		public ArgumentContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_argument; } }
		public override void EnterRule(IParseTreeListener listener) {
			IXemblyListener typedListener = listener as IXemblyListener;
			if (typedListener != null) typedListener.EnterArgument(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IXemblyListener typedListener = listener as IXemblyListener;
			if (typedListener != null) typedListener.ExitArgument(this);
		}
	}

	[RuleVersion(0)]
	public ArgumentContext argument() {
		ArgumentContext _localctx = new ArgumentContext(Context, State);
		EnterRule(_localctx, 4, RULE_argument);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 73; _localctx._TEXT = Match(TEXT);
			 _localctx.ret =  (_localctx._TEXT!=null?_localctx._TEXT.Text:null); 
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class LabelContext : ParserRuleContext {
		public ITerminalNode[] DIGIT() { return GetTokens(XemblyParser.DIGIT); }
		public ITerminalNode DIGIT(int i) {
			return GetToken(XemblyParser.DIGIT, i);
		}
		public LabelContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_label; } }
		public override void EnterRule(IParseTreeListener listener) {
			IXemblyListener typedListener = listener as IXemblyListener;
			if (typedListener != null) typedListener.EnterLabel(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IXemblyListener typedListener = listener as IXemblyListener;
			if (typedListener != null) typedListener.ExitLabel(this);
		}
	}

	[RuleVersion(0)]
	public LabelContext label() {
		LabelContext _localctx = new LabelContext(Context, State);
		EnterRule(_localctx, 6, RULE_label);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 77;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			do {
				{
				{
				State = 76; Match(DIGIT);
				}
				}
				State = 79;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			} while ( _la==DIGIT );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x3', '\x15', 'T', '\x4', '\x2', '\t', '\x2', '\x4', '\x3', 
		'\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', '\x5', '\x3', 
		'\x2', '\x3', '\x2', '\x3', '\x2', '\x5', '\x2', '\xE', '\n', '\x2', '\x3', 
		'\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\a', '\x2', '\x14', 
		'\n', '\x2', '\f', '\x2', '\xE', '\x2', '\x17', '\v', '\x2', '\x3', '\x2', 
		'\x3', '\x2', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x5', '\x3', 'J', '\n', '\x3', 
		'\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x5', '\x6', '\x5', 
		'P', '\n', '\x5', '\r', '\x5', '\xE', '\x5', 'Q', '\x3', '\x5', '\x2', 
		'\x2', '\x6', '\x2', '\x4', '\x6', '\b', '\x2', '\x2', '\x2', '^', '\x2', 
		'\x15', '\x3', '\x2', '\x2', '\x2', '\x4', 'I', '\x3', '\x2', '\x2', '\x2', 
		'\x6', 'K', '\x3', '\x2', '\x2', '\x2', '\b', 'O', '\x3', '\x2', '\x2', 
		'\x2', '\n', '\v', '\x5', '\b', '\x5', '\x2', '\v', '\f', '\a', '\x12', 
		'\x2', '\x2', '\f', '\xE', '\x3', '\x2', '\x2', '\x2', '\r', '\n', '\x3', 
		'\x2', '\x2', '\x2', '\r', '\xE', '\x3', '\x2', '\x2', '\x2', '\xE', '\xF', 
		'\x3', '\x2', '\x2', '\x2', '\xF', '\x10', '\x5', '\x4', '\x3', '\x2', 
		'\x10', '\x11', '\a', '\x13', '\x2', '\x2', '\x11', '\x12', '\b', '\x2', 
		'\x1', '\x2', '\x12', '\x14', '\x3', '\x2', '\x2', '\x2', '\x13', '\r', 
		'\x3', '\x2', '\x2', '\x2', '\x14', '\x17', '\x3', '\x2', '\x2', '\x2', 
		'\x15', '\x13', '\x3', '\x2', '\x2', '\x2', '\x15', '\x16', '\x3', '\x2', 
		'\x2', '\x2', '\x16', '\x18', '\x3', '\x2', '\x2', '\x2', '\x17', '\x15', 
		'\x3', '\x2', '\x2', '\x2', '\x18', '\x19', '\a', '\x2', '\x2', '\x3', 
		'\x19', '\x3', '\x3', '\x2', '\x2', '\x2', '\x1A', '\x1B', '\a', '\x3', 
		'\x2', '\x2', '\x1B', '\x1C', '\x5', '\x6', '\x4', '\x2', '\x1C', '\x1D', 
		'\b', '\x3', '\x1', '\x2', '\x1D', 'J', '\x3', '\x2', '\x2', '\x2', '\x1E', 
		'\x1F', '\a', '\x4', '\x2', '\x2', '\x1F', ' ', '\x5', '\x6', '\x4', '\x2', 
		' ', '!', '\b', '\x3', '\x1', '\x2', '!', 'J', '\x3', '\x2', '\x2', '\x2', 
		'\"', '#', '\a', '\x5', '\x2', '\x2', '#', '$', '\x5', '\x6', '\x4', '\x2', 
		'$', '%', '\b', '\x3', '\x1', '\x2', '%', 'J', '\x3', '\x2', '\x2', '\x2', 
		'&', '\'', '\a', '\x6', '\x2', '\x2', '\'', '(', '\x5', '\x6', '\x4', 
		'\x2', '(', ')', '\a', '\x11', '\x2', '\x2', ')', '*', '\x5', '\x6', '\x4', 
		'\x2', '*', '+', '\b', '\x3', '\x1', '\x2', '+', 'J', '\x3', '\x2', '\x2', 
		'\x2', ',', '-', '\a', '\a', '\x2', '\x2', '-', '.', '\x5', '\x6', '\x4', 
		'\x2', '.', '/', '\b', '\x3', '\x1', '\x2', '/', 'J', '\x3', '\x2', '\x2', 
		'\x2', '\x30', '\x31', '\a', '\b', '\x2', '\x2', '\x31', '\x32', '\x5', 
		'\x6', '\x4', '\x2', '\x32', '\x33', '\b', '\x3', '\x1', '\x2', '\x33', 
		'J', '\x3', '\x2', '\x2', '\x2', '\x34', '\x35', '\a', '\t', '\x2', '\x2', 
		'\x35', 'J', '\b', '\x3', '\x1', '\x2', '\x36', '\x37', '\a', '\n', '\x2', 
		'\x2', '\x37', '\x38', '\x5', '\x6', '\x4', '\x2', '\x38', '\x39', '\b', 
		'\x3', '\x1', '\x2', '\x39', 'J', '\x3', '\x2', '\x2', '\x2', ':', ';', 
		'\a', '\v', '\x2', '\x2', ';', 'J', '\b', '\x3', '\x1', '\x2', '<', '=', 
		'\a', '\f', '\x2', '\x2', '=', '>', '\x5', '\x6', '\x4', '\x2', '>', '?', 
		'\x5', '\x6', '\x4', '\x2', '?', '@', '\b', '\x3', '\x1', '\x2', '@', 
		'J', '\x3', '\x2', '\x2', '\x2', '\x41', '\x42', '\a', '\r', '\x2', '\x2', 
		'\x42', 'J', '\b', '\x3', '\x1', '\x2', '\x43', '\x44', '\a', '\xE', '\x2', 
		'\x2', '\x44', 'J', '\b', '\x3', '\x1', '\x2', '\x45', '\x46', '\a', '\xF', 
		'\x2', '\x2', '\x46', 'G', '\x5', '\x6', '\x4', '\x2', 'G', 'H', '\b', 
		'\x3', '\x1', '\x2', 'H', 'J', '\x3', '\x2', '\x2', '\x2', 'I', '\x1A', 
		'\x3', '\x2', '\x2', '\x2', 'I', '\x1E', '\x3', '\x2', '\x2', '\x2', 'I', 
		'\"', '\x3', '\x2', '\x2', '\x2', 'I', '&', '\x3', '\x2', '\x2', '\x2', 
		'I', ',', '\x3', '\x2', '\x2', '\x2', 'I', '\x30', '\x3', '\x2', '\x2', 
		'\x2', 'I', '\x34', '\x3', '\x2', '\x2', '\x2', 'I', '\x36', '\x3', '\x2', 
		'\x2', '\x2', 'I', ':', '\x3', '\x2', '\x2', '\x2', 'I', '<', '\x3', '\x2', 
		'\x2', '\x2', 'I', '\x41', '\x3', '\x2', '\x2', '\x2', 'I', '\x43', '\x3', 
		'\x2', '\x2', '\x2', 'I', '\x45', '\x3', '\x2', '\x2', '\x2', 'J', '\x5', 
		'\x3', '\x2', '\x2', '\x2', 'K', 'L', '\a', '\x14', '\x2', '\x2', 'L', 
		'M', '\b', '\x4', '\x1', '\x2', 'M', '\a', '\x3', '\x2', '\x2', '\x2', 
		'N', 'P', '\a', '\x10', '\x2', '\x2', 'O', 'N', '\x3', '\x2', '\x2', '\x2', 
		'P', 'Q', '\x3', '\x2', '\x2', '\x2', 'Q', 'O', '\x3', '\x2', '\x2', '\x2', 
		'Q', 'R', '\x3', '\x2', '\x2', '\x2', 'R', '\t', '\x3', '\x2', '\x2', 
		'\x2', '\x6', '\r', '\x15', 'I', 'Q',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
