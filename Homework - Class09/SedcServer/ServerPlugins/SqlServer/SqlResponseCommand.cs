﻿namespace ServerPlugins.SqlServer
{
    internal enum SqlResponseCommand
    {
        Error,
        GeneralInfo,
        TableList,
        TableData,
        TableSchema,
        TableDataID
    }
}