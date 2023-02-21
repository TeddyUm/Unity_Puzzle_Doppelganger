using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
    none,
    left,
    right,
    up,
    down
}

public class dot : MonoBehaviour
{
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    public bool isMatched = false;
    public int prevColumn;
    public int prevRow;

    [Header("Swipe Stuff")]
    public SwipeDirection swipeDir;
    public float swipeAngle = 0;
    public float swipeResist = .5f;

    private FindMatches findMatches;
    private Board board;
    private GameObject otherDot;
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private Vector2 tempPos;

    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        swipeDir = SwipeDirection.none;
    }

    void Update()
    {
        targetX = column;
        targetY = row;
        SetPosition(Mathf.Abs(targetX - transform.position.x) > .1, new Vector2(targetX, transform.position.y));
        SetPosition(Mathf.Abs(targetY - transform.position.y) > .1, new Vector2(transform.position.x, targetY));
    }


    //    FindMatches();

    //    if (isMatched)
    //    {
    //        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
    //        mySprite.color = new Color(1, 1, 1, 0.2f);
    //    }

    //    targetX = column;
    //    targetY = row;
    //    if(Mathf.Abs(targetX - transform.position.x) > 0.1f)
    //    {
    //        tempPos = new Vector2(targetX, transform.position.y);
    //        transform.position = Vector2.Lerp(transform.position, tempPos, 0.6f);
    //        if(board.allDots[column, row] != gameObject)
    //        {
    //            board.allDots[column, row] = gameObject;
    //        }
    //    }
    //    else
    //    {
    //        tempPos = new Vector2(targetX, transform.position.y);
    //        transform.position = tempPos;
    //    }

    //    if (Mathf.Abs(targetY - transform.position.y) > .1)
    //    {
    //        tempPos = new Vector2(transform.position.x, targetY);
    //        transform.position = Vector2.Lerp(transform.position, tempPos, 0.6f);
    //        if (board.allDots[column, row] != gameObject)
    //        {
    //            board.allDots[column, row] = gameObject;
    //        }
    //    }
    //    else
    //    {
    //        tempPos = new Vector2(transform.position.x, targetY);
    //        transform.position = tempPos;
    //    }
    //}

    private void SetPosition(bool IsLongAway, Vector2 tempPos)
    {
        if (!IsLongAway) transform.position = tempPos;
        else
        {
            transform.position = Vector2.Lerp(transform.position, tempPos, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
    }

    private void OnMouseDown()
    {
        if (board.currentState == GameState.move)
        {
            firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPos.y - firstTouchPos.y) > swipeResist || Mathf.Abs(finalTouchPos.x - firstTouchPos.x) > swipeResist)
        {
            board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x) * Mathf.Rad2Deg;// 180 / Mathf.PI;
            MovePieces();
            board.currentDot = this;
        }
        else
        {
            board.currentState = GameState.move;
            swipeAngle = 0;
        }
    }
    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            MovePiecesActual(Vector2.right);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            MovePiecesActual(Vector2.up);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            MovePiecesActual(Vector2.left);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            MovePiecesActual(Vector2.down);
        }
        else
        {
            board.currentState = GameState.move;
        }
    }
    void MovePiecesActual(Vector2 direction)
    {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];
        prevRow = row;
        prevColumn = column;

        if (otherDot != null)
        {
            MovePiecesDirection(direction);
            otherDot.GetComponent<dot>().MovePiecesDirection(direction * (-1));
            StartCoroutine(CheckMoveCo());
        }
        else
        {
            board.currentState = GameState.move;
        }
    }
    void MovePiecesDirection(Vector2 direction)
    {
        column += (int)direction.x;
        row += (int)direction.y;

        if (direction == Vector2.left) swipeDir = SwipeDirection.left;
        if (direction == Vector2.right) swipeDir = SwipeDirection.right;
        if (direction == Vector2.up) swipeDir = SwipeDirection.up;
        if (direction == Vector2.down) swipeDir = SwipeDirection.down;
    }
    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<dot>().isMatched)
            {
                otherDot.GetComponent<dot>().row = row;
                otherDot.GetComponent<dot>().column = column;
                row = prevRow;
                column = prevColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyMatches();
            }
        }
    }
}
