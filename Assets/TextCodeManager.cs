using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        /*
         * Notes:
         *  - Need to check for conditions, put them in the same line
         *  - Of course add all the other code blocks
         *  - Need to connect this to the execution director? Or detect OnSnap. Get All instances of function definitions and start queue blocks
         *  - Handle a stack of while/if etc looking for closure (so we dont mess up indentation)
         *  - First block should be a StartQueue or Function block; handle that.
         *  - Handle function blocks as well
         */


        string text = string.Empty;
        int i = 0;
        int ind = 0; //Indentation
        int line = 0;
        while (i < blockList.Count) {
            TurtleCommand blockTurtleCommand;
            bool isCommandOrFlowControl = blockList[i].TryGetComponent<TurtleCommand>(out blockTurtleCommand);
            if (isCommandOrFlowControl)
            {

                switch (blockTurtleCommand.commandEnum)
                {
                    case (TurtleCommand.Command.IfBegin):
                        text += $"{line}\t{Indent(ind)}{key_clr}if{smb_clr}(";

                        //Check next block, see if its a condition
                        if (blockList.Count > i + 1)
                        {
                            TurtleCommand conditionCmd;
                            bool found = blockList[i].TryGetComponent<TurtleCommand>(out conditionCmd);
                            if (found && isCondition(conditionCmd.commandEnum))
                            {
                                text += GetString(conditionCmd.commandEnum);
                                i++; //Next block is already accounted for.
                            }
                        }

                        text += $"{smb_clr}):\n"; //close paranthesis, new line
                        ind++; //inside if scope

                        break;
                    case (TurtleCommand.Command.ElseIf):
                        ind--; //Assume we had an If before this
                        text += $"{line}\t{Indent(ind)}{key_clr}ElseIf{smb_clr}(";

                        //Check next block, see if its a condition
                        if (blockList.Count > i + 1)
                        {
                            TurtleCommand conditionCmd;
                            bool found = blockList[i].TryGetComponent<TurtleCommand>(out conditionCmd);
                            if (found && isCondition(conditionCmd.commandEnum))
                            {
                                text += GetString(conditionCmd.commandEnum);
                                i++; //Next block is already accounted for.
                            }
                        }

                        text += $"{smb_clr}):\n"; //close parenthesis, new line
                        ind++; //Enter ElseIf
                        break;

                    case (TurtleCommand.Command.Else):
                        ind--; //Assume we had an If before this
                        text += $"{line}\t{Indent(ind)}{key_clr}Else{smb_clr}:\n";
                        ind++; //Enter Else
                        break;

                    case (TurtleCommand.Command.IfEnd):
                        ind--; //We're exiting an if
                        text += text += $"{line}\t{Indent(ind)}{key_clr}EndIf{smb_clr}\n";
                        break;

                    case (TurtleCommand.Command.WhileBegin):
                        text += $"{line}\t{Indent(ind)}{key_clr}While{smb_clr}(";

                        //Check next block, see if its a condition
                        if (blockList.Count > i + 1)
                        {
                            TurtleCommand conditionCmd;
                            bool found = blockList[i].TryGetComponent<TurtleCommand>(out conditionCmd);
                            if (found && isCondition(conditionCmd.commandEnum))
                            {
                                text += GetString(conditionCmd.commandEnum);
                                i++; //Next block is already accounted for.
                            }
                        }

                        text += $"{smb_clr}):\n"; //close parenthesis, new line
                        ind++; //Enter While
                        break;

                    case (TurtleCommand.Command.WhileBreak):
                        text += $"{line}\t{Indent(ind)}{key_clr}Break\n";
                        break;

                    case (TurtleCommand.Command.WhileEnd):
                        ind--; //exiting While End
                        text += $"{line}\t{Indent(ind)}{key_clr}WhileEnd{smb_clr}\n";
                        break;

                    //Handle everything else as a single line, no control flow change
                    default:
                        text += $"{line}\t{Indent(ind)}{GetString(blockTurtleCommand.commandEnum)}\n";
                        break;
                }
                line++;
            }
        }
        return text;
    }

    private bool isCondition(TurtleCommand.Command command)
    {
        if ((int)command >= 11 && (int)command <= 15) { return true; }
        return false;
    }

    private string GetString(TurtleCommand.Command command)
    {
        switch (command)
        {
            //Conditions
            case (TurtleCommand.Command.ConditionTrue):
                return $"{key_clr}True";
            case (TurtleCommand.Command.ConditionFalse):
                return $"{key_clr}False";
            case (TurtleCommand.Command.ConditionFacingWall):
                return $"{tok_clr}turtle{smb_clr}.{tok_clr}isFacingWall{smb_clr}()";
            case (TurtleCommand.Command.ConditionFacingCliff):
                return $"{tok_clr}turtle{smb_clr}.{tok_clr}isFacingCliff{smb_clr}()";
            case (TurtleCommand.Command.ConditionFacingStepDown):
                return $"{tok_clr}turtle{smb_clr}.{tok_clr}isFacingStepDown{smb_clr}()";

            //Turtle Actions
            case (TurtleCommand.Command.MoveForward):
                return $"{tok_clr}turtle{smb_clr}.{tok_clr}MoveForward{smb_clr}()";
            case (TurtleCommand.Command.RotateLeft):
                return $"{tok_clr}turtle{smb_clr}.{tok_clr}RotateLeft{smb_clr}()";
            case (TurtleCommand.Command.RotateRight):
                return $"{tok_clr}turtle{smb_clr}.{tok_clr}RotateRight{smb_clr}()";
            case (TurtleCommand.Command.Jump):
                return $"{tok_clr}turtle{smb_clr}.{tok_clr}Jump{smb_clr}()";
        }
        return string.Empty;
    }
}
