grammar ShapPang;

/*
 * Parser Rules
 */
compileUnit
	: (element (NEWLINE|EOF))* 
	;

element: 
	elementname = ID '(' (description = STRINGLITERAL)? ')' NEWLINE '{' NEWLINE ((assign|givendeclaration|derivationdeclaration) NEWLINE)* '}';
givendeclaration: ID ('(' description = STRINGLITERAL')')?;
assign: ID '=' expressionToEval = expression;
expression: left= expression operator='*' right=expression #ExpressionMultiply
| left = expression operator=('+' |'-') right=expression #ExpressionAddMinus
| left = expression operator= '\\' right = expression #ExpressionDivide
| ID #ExpressionReference
| DECIMAL #ExpressionConstant
| INT #ExpressionConstant;

derivationdeclaration: ID '(' description = STRINGLITERAL ')' NEWLINE '{' NEWLINE
((assign) NEWLINE)*
'}'
 ;

COMMENT: '//';
INT: (NUMBERS)+;
DECIMAL: (NUMBERS)+('.'(NUMBERS)+)?;
NUMBERS: [0-9];
STRINGLITERAL : '"' ~["\r\n]* '"'; 

/*
 * Lexer Rules
 */
ID: ([A-Za-z.])+;
NEWLINE: '\r'? '\n';
fragment A : [aA];
fragment B : [bB];
fragment C : [cC];
fragment D : [dD];
fragment E : [eE];
fragment F : [fF];
fragment G : [gG];
fragment H : [hH];
fragment I : [iI];
fragment J : [jJ];
fragment K : [kK];
fragment L : [lL];
fragment M : [mM];
fragment N : [nN];
fragment O : [oO];
fragment P : [pP];
fragment Q : [qQ];
fragment R : [rR];
fragment S : [sS];
fragment T : [tT];
fragment U : [uU];
fragment V : [vV];
fragment W : [wW];
fragment X : [xX];
fragment Y : [yY];
fragment Z : [zZ];

WS
	:	' ' -> channel(HIDDEN)
	;