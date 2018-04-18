using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System
{
    public class XsltContext : System.Xml.Xsl.XsltContext
    {
        public XsltContext()
        {
            Initialize();
        }

        public XsltContext(System.Xml.NameTable nameTable)
            : base(nameTable)
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterFunction("my", "distinct-values", typeof(List<int>));
        }

        public override string LookupNamespace(string prefix)
        {
            return base.LookupNamespace(prefix);
        }

        public override int CompareDocument(string baseUri, string nextbaseUri)
        {
            return string.CompareOrdinal(baseUri, nextbaseUri);
        }

        public override bool PreserveWhitespace(System.Xml.XPath.XPathNavigator node)
        {
            return false;
        }

        public void RegisterFunction(string prefix, string name, Type function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            if (name == null)
                throw new ArgumentNullException("name");

            functions[prefix + ":" + name] = function;
        }

        Dictionary<string, Type> functions = new Dictionary<string, Type>();

        public override System.Xml.Xsl.IXsltContextFunction ResolveFunction(string prefix, string name, System.Xml.XPath.XPathResultType[] argTypes)
        {
            Type functionType = null;

            if (functions.TryGetValue(prefix + ":" + name, out functionType))
            {
                System.Xml.Xsl.IXsltContextFunction function = Activator.CreateInstance(functionType) as System.Xml.Xsl.IXsltContextFunction;

                return function;
            }

            return null;
        }

        public override System.Xml.Xsl.IXsltContextVariable ResolveVariable(string prefix, string name)
        {
            return null;
        }

        public override bool Whitespace
        {
            get
            {
                return false;
            }
        }

        internal static string GetValue(object v)
        {
            if (v == null)
                return null;

            if (v is System.Xml.XPath.XPathNodeIterator)
            {
                foreach (System.Xml.XPath.XPathNavigator n in v as System.Xml.XPath.XPathNodeIterator)
                    return n.Value;
            }

            return Convert.ToString(v);
        }

    }

    class StringCompare : System.Xml.Xsl.IXsltContextFunction
    {
        public System.Xml.XPath.XPathResultType[] ArgTypes
        {
            get
            {
                return new System.Xml.XPath.XPathResultType[]
            {
                System.Xml.XPath.XPathResultType.String,
                System.Xml.XPath.XPathResultType.String,
                System.Xml.XPath.XPathResultType.String
            };
            }
        }

        public object Invoke(System.Xml.Xsl.XsltContext xsltContext, object[] args, System.Xml.XPath.XPathNavigator docContext)
        {
            string arg1 = XsltContext.GetValue(args[0]);
            string arg2 = XsltContext.GetValue(args[1]);

            string locale = "en-US";

            if (args.Length > 2)
                locale = XsltContext.GetValue(args[2]);

            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo(locale);

            return string.Compare(arg1, arg2, false, culture);
        }

        public int Maxargs
        {
            get
            {
                return 3;
            }
        }

        public int Minargs
        {
            get
            {
                return 2;
            }
        }

        public System.Xml.XPath.XPathResultType ReturnType
        {
            get
            {
                return System.Xml.XPath.XPathResultType.Number;
            }
        }
    }
}