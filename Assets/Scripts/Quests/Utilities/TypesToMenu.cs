using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Quests.Utilities
{
    public class TypesToMenu<T> 
    {
        public class TypeEntry 
        {
            public Type Type;
            public string Path;
            public int Priority;
        }

        public List<TypeEntry> Lines { get; }

        public TypesToMenu() 
        {
            Lines = GetTypeEntries();
        }

        private static List<TypeEntry> GetTypeEntries() 
        {
            var list = (from assembly in AppDomain.CurrentDomain.GetAssemblies() 
                from type in assembly.GetTypes() 
                where type.IsSubclassOf(typeof(T)) && !type.IsAbstract 
                let attr = type.GetCustomAttribute<CreateMenuAttribute>() 
                select new TypeEntry
                {
                    Type = type, Path = attr?.Path ?? type.FullName, Priority = attr?.Priority ?? 0,
                })
                .ToList();

            return list
                .OrderByDescending(t => t.Priority)
                .ToList();
        }
    }
}