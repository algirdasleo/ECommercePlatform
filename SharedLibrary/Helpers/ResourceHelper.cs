using System.Resources;
using System.Reflection;
using System;
using System.ComponentModel;

namespace SharedLibrary.Helpers
{
    public static class ResourceHelper
    {
        private static readonly ResourceManager resourceManager = new ResourceManager(
            "SharedLibrary.Resources.SQLQueries", 
            Assembly.GetExecutingAssembly()
        );

    
        public static string GetQuery(string queryName)
        {
            return resourceManager.GetString(queryName) ?? throw new Exception($"Query {queryName} not found");
        }
    }
}