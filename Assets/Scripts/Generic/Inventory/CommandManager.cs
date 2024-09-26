using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void Execute();
    void Undo();
}
public class MoveCommand : ICommand
{
    private Transform ObjectToMove;
    private Vector3 displacement;
    public MoveCommand(Transform obj, Vector3 displacement)
    {
        this.ObjectToMove = obj;
        this.displacement = displacement;
    }
    public void Execute() { ObjectToMove.position += displacement; }
    public void Undo() { ObjectToMove.position -= displacement; }
}

public class CommandManager : MonoBehaviour
{
    private Stack<ICommand> commandHistory = new Stack<ICommand>();     //스택 형태로 커맨드 관리 

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);   
    }

    public void UndoLastCommand()
    {
        if (commandHistory.Count > 0)
        {
            ICommand lastCommand = commandHistory.Pop();
            lastCommand.Undo();
        }
    }
}
