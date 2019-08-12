// https://www.skylinetechnologies.com/Blog/Skyline-Blog/March-2013/CLR-Functions-in-SQL-Server-A-Tutorial

using System;
using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace SqlClrObfuscation
{
    public static class ClrFunctions
    {

        //SQL Functions require an additional "SqlFunction" Attribute.
        //This attribute provides SQL server with additional meta data information it needs
        //regarding our custom function. In this example we are not accessing any data, and our
        //function is deterministic. So we let SQL know those facts about our function with
        //the DataAccess and IsDeterministic parameters of our attribute.
        //Additionally, SQL needs to know the name of a function it can defer to when it needs
        //to convert the object we have returned from our function into a structure that SQL
        //can understand. This is provided by the "FillRowMethodName" shown below.
        [SqlFunction(
            DataAccess = DataAccessKind.None,
            FillRowMethodName = "MyFillRowMethod"
            , IsDeterministic = true)
        ]
        //SQL Functions must be declared as Static. Table Valued functions must also
        //return a class that implements the IEnumerable interface. Most built in
        //.NET collections and arrays already implement this interface.
        public static IEnumerable ObfuscateAlphanumeric(string stringToSplit)
        {
            string[] elements = {stringToSplit };
            return elements;
        }

        //SQL needs to defer to user code to translate the an IEnumerable item into something
        //SQL Server can understand. In this case we convert our string to a SqlChar object...
        public static void MyFillRowMethod(Object theItem, out SqlChars results)
        {
            results = new SqlChars(theItem.ToString());
        }

    }
}