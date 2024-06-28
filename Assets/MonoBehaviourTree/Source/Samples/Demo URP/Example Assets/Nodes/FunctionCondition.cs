using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FinalScripts.Refactored;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode(name = "Conditions/Function Condition")]
    public class FunctionCondition : Condition
    {
        public EnemyBT scriptToCheck;
        public string function;

        public override bool Check()
        {
            if (scriptToCheck == null || string.IsNullOrEmpty(function))
            {
                Debug.LogError("scriptToCheck or function is not set.");
                return false;
            }
            
            Type scriptType = scriptToCheck.GetType();
            MethodInfo boolMethod = scriptType.GetMethod(function, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (boolMethod == null)
            {
                Debug.LogError($"Method {function} not found in {scriptType}.");
                return false;
            }

            if (boolMethod.ReturnType != typeof(bool))
            {
                Debug.LogError($"Method {function} does not return a boolean value.");
                return false;
            }

            try
            {
                return (bool)boolMethod.Invoke(scriptToCheck, null);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error invoking method {function}: {ex.Message}");
                return false;
            }
        }
    }
}