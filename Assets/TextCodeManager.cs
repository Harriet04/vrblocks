using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCodeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string key_clr = "<color=#000000>"; //Color for keywords like if, else, else if, etc
    string smb_clr = "<color=#000000>";
    string tok_clr = "<color=#000000>"; //Color for tokens (class names, instances, etc)
    string val_clr = "<color=#000000>"; //Color for values (floats, ints, etc)
    string cls_clr = "</color>";

    string Indent(int level) => new string(' ', level * 4);
    
    string ConstructText(List<GameObject> blockList)
    {
        string text = string.Empty;
        int i = 0;
        int ind = 0; //Indentation
        int line = 0;
        while (i < blockList.Count) {
            TurtleCommand blockTurtleCommand;
            bool isCommandOrFlowControl = blockList[i].TryGetComponent<TurtleCommand>(out blockTurtleCommand);
            if (isCommandOrFlowControl)
            {
                if(line != 0) { text += "\n"; } //New line
                //text += $"{line}"
                text += $"{Indent(ind)}";
                line++;
                switch (blockTurtleCommand.commandEnum)
                {
                    case (TurtleCommand.Command.MoveForward):
                        text += $"{key_clr}turtle{smb_clr}.{key_clr}MoveForward{smb_clr}()";
                        break;
                    case (TurtleCommand.Command.RotateRight):
                        text += $"{key_clr}turtle{smb_clr}.{key_clr}RotateRight{smb_clr}()";
                        break;
                    case (TurtleCommand.Command.RotateLeft):
                        text += $"{key_clr}turtle{smb_clr}.{key_clr}RotateLeft{smb_clr}()";
                        break;
                    case (TurtleCommand.Command.Jump):
                        text += $"{key_clr}turtle{smb_clr}.{key_clr}Jump{smb_clr}()";
                        break;
                    //IfBegin,
                    //ElseIf,
                    //Else,
                    //IfEnd,
                    //WhileBegin,
                    //WhileBreak,
                    //WhileEnd,
                    //ConditionTrue,
                    //ConditionFalse,
                    //ConditionFacingWall,
                    //ConditionFacingCliff,
                    //ConditionFacingStepDown,
                    //CommandError

                    default:
                        break;
                }
            }
        }
        return text;
    }

}
