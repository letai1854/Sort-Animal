using System.Collections.Generic;
using UnityEngine;

public class MoveHistoryManager : SingletonMonoBehaviour<MoveHistoryManager>
{
    private Stack<MoveOperation> moveHistoryStack = new Stack<MoveOperation>();
    public void RecordMove(MoveOperation operation)
    {
        if (operation == null || operation.NumberOfAnimalsMoved == 0) return;
        moveHistoryStack.Push(operation);
    }
    public MoveOperation GetLastMoveForUndoAndPop()
    {
        if (moveHistoryStack.Count > 0)
        {
            MoveOperation op = moveHistoryStack.Pop();
            return op;
        }
        return null;
    }
    public bool CanUndo()
    {
        return moveHistoryStack.Count > 0;
    }
    public void ClearHistory()
    {
        moveHistoryStack.Clear();
    }
}
