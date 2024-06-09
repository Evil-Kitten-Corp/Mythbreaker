using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CheatBase
    {
        private string _cheatId;
        private string _cheatFormat;

        public string cheatId => _cheatId;
        public string cheatFormat => _cheatFormat;

        public CheatBase(string id, string format)
        {
            _cheatId = id;
            _cheatFormat = format;
        }
    }

    public class DebugCheat : CheatBase
    {
        private Action command;

        public DebugCheat(string id, string format, Action command) : base(id, format)
        {
            this.command = command;
        }

        public void Invoke()
        {
            command.Invoke();
        }
    }
}