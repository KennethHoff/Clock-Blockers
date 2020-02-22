//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;
//using UnityEngine;

//public struct ActionCategorization
//{
//    public String name;
//    public List<String> parameters;
//}

//public class ActionDebugging
//{
//    private void DebugActions(CharacterAction[] actionArray)
//    {
//        var checkAmounts = CheckActionAmounts(actionArray);
//        Debug.Log(checkAmounts.Count);

//        foreach (var checkAmount in checkAmounts)
//        {
//            Debug.Log(checkAmount.name + ": " + checkAmount.parameters.Count);
//        }
//    }

//    private List<ActionCategorization> CheckActionAmounts(CharacterAction[] actionArray)
//    {
//        var tempTesting = new List<ActionCategorization>();

//        foreach (var characterAction in actionArray)
//        {
//            var actionString = characterAction.method;
//            var funcName = Regex.Match(actionString, "(?<=func: ).*?(?=params: )").ToString(); // Get function, by string.
//            var para = Regex.Match(actionString, "(?<=params: ).*").ToString(); // Get parameters, by string.

//            var added = false;
//            for (int i = 0; i < tempTesting.Count; i++)
//            {
//                if (tempTesting[i].name == funcName)
//                {
//                    tempTesting[i].parameters.Add(para);
//                    added = true;
//                }
//            }

//            if (!added)
//            {
//                var newTestList = new List<String>();
//                newTestList.Add(para);

//                var newTesting = new ActionCategorization { name = funcName, parameters = newTestList };
//                tempTesting.Add(newTesting);
//            }
//        }

//        return tempTesting;
//    }
//}