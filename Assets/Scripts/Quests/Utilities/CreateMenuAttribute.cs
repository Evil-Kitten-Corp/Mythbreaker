﻿using System;

namespace Quests.Utilities
{
    public class CreateMenuAttribute : Attribute 
    {
        public string Path { get; }
        public int Priority { get; }

        public CreateMenuAttribute (string path, int priority = 0) 
        {
            Path = path;
            Priority = priority;
        }
    }
}