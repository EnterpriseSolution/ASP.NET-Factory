using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Flextronics.Applications.Library.Utility;

namespace Flextronics.Applications.Library.Schema
{
    public class DBOMaker
    {
        private static Cache cache = new Cache();

        public static IDbObject CreateDbObj(string dbTypename)
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            string library = assem.GetName().Name; 
            string typeName = "Flextronics.Applications.Library.Schema.";
            if (dbTypename == "SQL2000")
                typeName += "SQLServer2000DbObject";
            if (dbTypename == "SQL2005")
                typeName += "SQLServer2005DbObject";

            return (IDbObject)CreateObject(library, typeName); 
        }

        private static object CreateObject(string path, string TypeName)
        {
            object obj2 = cache.GetObject(TypeName);
            if (obj2 == null)
            {
                try
                {
                    obj2 = Assembly.Load(path).CreateInstance(TypeName);
                    cache.SaveCache(TypeName, obj2);
                }
                catch (Exception exception)
                {
                    string message = exception.Message;
                }
            }
            return obj2;
        }

        public static IDbScriptBuilder CreateScript(string dbTypename)
        {
            string typeName = "Flextronics.Applications.Library.Schema.";
            if (dbTypename == "SQL2000")
                typeName += "SQLServer2000DbScriptBuilder";

            Assembly assem = Assembly.GetExecutingAssembly();
            string library = assem.GetName().Name;
            return (IDbScriptBuilder)CreateObject(library, typeName);

        }
    }

}