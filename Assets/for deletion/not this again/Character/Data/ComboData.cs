using System;
using System.Collections.Generic;
using not_this_again.Enums;
using TriInspector;
using UnityEngine;

namespace not_this_again.Character.Data
{
    [Serializable]
    public struct ComboData
    {
        [Title("Combo")]
        public string rowName;
        public string comboName;
        public List<Keystroke> comboInputs;
        public List<AnimationClip> comboClips;
    }
}