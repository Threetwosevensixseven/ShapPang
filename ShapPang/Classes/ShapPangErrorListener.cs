using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapPang.Classes
{
    class ShapPangErrorListener : BaseErrorListener
    {         
            private string errorCollection = "";


            public string ErrorCollection
            {
                get { return errorCollection; }               
            }

            public ShapPangErrorListener()
            {                                
            }

            public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
            {                
                throw new Exception("Unable to complete script execution: " + msg + " on line at :" + line.ToString());
            }
    }
}
