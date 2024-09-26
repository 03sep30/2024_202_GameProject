using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CommandManager CommandManager;   
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            ICommand moveRight = new MoveCommand(transform, Vector3.right);
            CommandManager.ExecuteCommand(moveRight);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ICommand moveLeft = new MoveCommand(transform, Vector3.left);
            CommandManager.ExecuteCommand(moveLeft);
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            CommandManager.UndoLastCommand();
        }        
    }
}
