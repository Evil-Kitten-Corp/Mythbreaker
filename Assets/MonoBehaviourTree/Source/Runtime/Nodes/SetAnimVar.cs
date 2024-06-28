using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode(name = "Tasks/Set Animator Variable")]
    public class SetAnimVar : Leaf
    {
        [SerializeField] private Type type;
        public AnimatorReference sourceAnim;
        public StringReference var;
        [SerializeField] public bool varBoolState;

        public override NodeResult Execute()
        {
            switch (type)
            {
                case Type.Trigger:
                    sourceAnim.Value.SetTrigger(var.Value);
                    break;
                case Type.Bool:
                    sourceAnim.Value.SetBool(var.Value, varBoolState);
                    break;
            }
            
            return NodeResult.success;
        }

        public override bool IsValid()
        {
            return !(sourceAnim == null || IsInvalid(sourceAnim));
        }

        private static bool IsInvalid(BaseVariableReference variable)
        {
            // Custom validation to allow null objects without warnings
            return (variable.isConstant)? false : variable.blackboard == null || string.IsNullOrEmpty(variable.key);
        }

        public enum Type
        {
            Trigger, Bool
        }
    }
}