namespace Codematic
{
    using LTP.CodeBuild;
    using LTP.DBFactory;
    using LTP.IDBO;
    using System;

    internal class ObjHelper
    {
        public static CodeBuilders CreatCB(string longservername)
        {
            return new CodeBuilders(CreatDbObj(longservername));
        }

        public static IDbObject CreatDbObj(string longservername)
        {
            DbSettings setting = DbConfig.GetSetting(longservername);
            IDbObject obj2 = DBOMaker.CreateDbObj(setting.DbType);
            obj2.DbConnectStr = setting.ConnectStr;
            return obj2;
        }

        public static IDbScriptBuilder CreatDsb(string longservername)
        {
            DbSettings setting = DbConfig.GetSetting(longservername);
            IDbScriptBuilder builder = DBOMaker.CreateScript(setting.DbType);
            builder.DbConnectStr = setting.ConnectStr;
            return builder;
        }
    }
}

