using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int positionIndex;
    [HideInInspector]
    public Board board;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void SetupGem(Vector2Int position, Board theBoard)
    {
        positionIndex = position;
        board = theBoard;
    }
}
