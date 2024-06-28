using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    public class AnimatorVariable : Variable<Animator>
    {
        protected override bool ValueEquals(Animator val1, Animator val2)
        {
            return val1 == val2;
        }
    }

    [System.Serializable]
    public class AnimatorReference : VariableReference<AnimatorVariable, Animator>
    {
        public AnimatorReference(VarRefMode mode = VarRefMode.EnableConstant)
        {
            SetMode(mode);
        }

        protected override bool isConstantValid
        {
            get { return constantValue != null; }
        }

        public Animator Value
        {
            get
            {
                return (useConstant)? constantValue : this.GetVariable().Value;
            }
            set
            {
                if (useConstant)
                {
                    constantValue = value;
                }
                else
                {
                    this.GetVariable().Value = value;
                }
            }
        }
    }
}