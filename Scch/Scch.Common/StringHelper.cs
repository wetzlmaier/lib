using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace Scch.Common
{
    public static class StringHelper
    {
        public static char Semicolon => ';';

        public static char Tabulator => '\t';

        public static char Comma => ',';

        public static char[] CommaSemicolon
        {
            get
            {
                return new[] { Comma, Semicolon };
            }
        }

        public static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }
    }
}
