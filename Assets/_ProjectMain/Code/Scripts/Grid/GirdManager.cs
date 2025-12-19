using System.Collections.Generic;
using UnityEngine;

public class GirdManager : MonoBehaviour
{
    [SerializeField] ShapeStorage shapeStorage;
    [SerializeField] public int colums = 0;
    [SerializeField] public int rows = 0;
    [SerializeField] public float squaresGap = 0.1f;
    [SerializeField] public GameObject gridSquare;
    [SerializeField] public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    [SerializeField] public float squareScale = 0.5f;
    [SerializeField] public float everySquareOffset = 0.0f;
    [SerializeField] public SquareTextureData squareTextureData;

    private Vector2 _offSet = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();

    private LineIndicator lineIndicator;

    private Config.SquareColor currentActiveSquareColor = Config.SquareColor.NotSet;
    private List<Config.SquareColor> colorsInTheGrid = new List<Config.SquareColor>();

    [Header("Sound SFX")]
    [SerializeField] private AudioClip placeShapeSound; 
    [SerializeField] private AudioClip lineCompleteSound; 
    [SerializeField] private AudioClip comboSound;
    [SerializeField] private AudioClip loseSound;
    private AudioSource audioSource;
    void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor += OnUpdateSquareColor;
        GameEvents.CheckIfPlayerLost += CheckIfPlayerLost;
    }
    void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor -= OnUpdateSquareColor;
        GameEvents.CheckIfPlayerLost -= CheckIfPlayerLost;
    }
    void Start()
    {
        lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
        currentActiveSquareColor = squareTextureData.activeSquareTextures[0].squareColor;
        //AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void OnUpdateSquareColor(Config.SquareColor color)
    {
        currentActiveSquareColor = color;
    }

    private List<Config.SquareColor> GetAllSquareColorsInTheGrid()
    {
        List<Config.SquareColor> colors = new List<Config.SquareColor>();
        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.SquraeOccupied)
            {
                var color = gridSquare.GetCurrentColor();
                if (colors.Contains(color) == false)
                {
                    colors.Add(color);
                }
            }

        }
        return colors;
    }
    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPositions();
    }
    private void SpawnGridSquares()
    {
        int square_index = 0;
        for (var row = 0; row < rows; ++row)
        {
            for (var colum = 0; colum < colums; ++colum)
            {
                _gridSquares.Add(Instantiate(gridSquare));
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = square_index;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(lineIndicator.GetGridSquareIndex(square_index) % 2 == 0);
                square_index++;
            }
        }
    }
    private void SetGridSquaresPositions()
    {
        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        var square_react = _gridSquares[0].GetComponent<RectTransform>();

        _offSet.x = square_react.rect.width * square_react.transform.localScale.x + everySquareOffset;
        _offSet.y = square_react.rect.height * square_react.transform.localScale.y + everySquareOffset;

        foreach (GameObject square in _gridSquares)
        {
            if (column_number + 1 > colums)
            {
                square_gap_number.x = 0; //go to next column
                column_number = 0;
                row_number++;
                row_moved = false;
            }

            var pos_x_offset = _offSet.x * column_number + (square_gap_number.x * squaresGap);
            var pos_y_offset = _offSet.y * row_number + (square_gap_number.y * squaresGap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGap;
            }
            if (row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squaresGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);

            column_number++;
        }
    }
    private void CheckIfShapeCanBePlaced()
    {
        List<int> squareIndexes = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.Selected && !gridSquare.SquraeOccupied)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
                //gridSquare.ActivateSquare();
            }
        }
        Shape currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return;

        if (currentSelectedShape.totalSquareNumber == squareIndexes.Count)
        {
            foreach (var squareIndex in squareIndexes)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard(currentActiveSquareColor);
            }

            if (placeShapeSound != null) //SFX
            {
                audioSource.PlayOneShot(placeShapeSound);
            }

            int shapeLeft = 0;
            foreach (var shape in shapeStorage.shapesList)
            {
                if (shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
                {
                    shapeLeft++;
                }
            }

            if (shapeLeft == 0)
            {
                GameEvents.RequestNewShape();
            }
            else
            {
                GameEvents.SetShapeInactive();
            }
            CheckIfAnyLineIsCompleted();

        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }
    }
    private void CheckIfAnyLineIsCompleted()
    {
        List<int[]> lines = new List<int[]>();

        //Columns
        foreach (int colum in lineIndicator.columIndexes)
        {
            lines.Add(lineIndicator.GetVerticalLine(colum));
        }

        //Rows
        for (int row = 0; row < 9; row++)
        {
            List<int> data = new List<int>(9);
            for (int index = 0; index < 9; index++)
            {
                data.Add(lineIndicator.lineData[row, index]);
            }

            lines.Add(data.ToArray());
        }
        //Square
        for (int square = 0; square < 9; square++)
        {
            List<int> data = new List<int>(9);
            for (int index = 0; index < 9; index++)
            {
                data.Add(lineIndicator.square_data[square, index]);
            }
            lines.Add(data.ToArray());
        }

        //This fuction needs to be called before CheckIfSquareAreCompleted.
        colorsInTheGrid = GetAllSquareColorsInTheGrid();

        int completedLines = CheckIfSquareAreCompleted(lines);
       if (completedLines == 1)
        {
            // Sound with line complete
            if (lineCompleteSound != null)
            {
                audioSource.PlayOneShot(lineCompleteSound);
            }
        }
        else if (completedLines >= 2)
        {
            // Sound for comboline
            if (comboSound != null)
            {
                audioSource.PlayOneShot(comboSound);
            }
            GameEvents.ShowCongralationWriting();
        }
        //Score
        int totalScore = 10 * completedLines;
        int bonusScore = ShouldPlayColorBonusAnimation();
        GameEvents.AddScores(totalScore + bonusScore);
        GameEvents.CheckIfPlayerLost();
    }
    private int ShouldPlayColorBonusAnimation()
    {
        var colorsInTheGridAfterLineRemove = GetAllSquareColorsInTheGrid();
        List<Config.SquareColor> clearedColors = new List<Config.SquareColor>();
        int bonusScore = 0;

        // Kiểm tra tất cả các màu trong lưới trước khi xóa dòng
        foreach (var squareColor in colorsInTheGrid)
        {
            // Nếu màu không còn trong lưới sau khi xóa dòng và không phải màu hiện tại
            if (!colorsInTheGridAfterLineRemove.Contains(squareColor) && squareColor != currentActiveSquareColor)
            {
                clearedColors.Add(squareColor);
                bonusScore += 50; // Cộng 50 điểm cho mỗi màu bị xóa
            }
        }

        // Gọi ShowBonusScreen với danh sách các màu bị xóa
        if (clearedColors.Count > 0)
        {
            GameEvents.ShowBonusScreen(clearedColors);
        }

        return bonusScore;
        // Config.SquareColor colorToPlayBonusFor = Config.SquareColor.NotSet;

        // foreach (var squareColor in colorsInTheGrid)
        // {
        //     if (colorsInTheGridAfterLineRemove.Contains(squareColor) == false)
        //     {
        //         colorToPlayBonusFor = squareColor;
        //     }
        // }
        // if (colorToPlayBonusFor == Config.SquareColor.NotSet)
        // {
        //     //Debug.Log("Cannot fine Color for bonus");
        //     return 0;
        // }

        // //Should never play bonus for the current color.
        // if (colorToPlayBonusFor == currentActiveSquareColor)
        // {
        //     return 0;
        // }

        // GameEvents.ShowBonusScreen(colorToPlayBonusFor);

        // return 50; //score
        //TO DO: fix error Eat 2 values at the same time but only calculate one
    }
    private int CheckIfSquareAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();

        int totalLinesCompleted = 0;

        foreach (int[] line in data)
        {
            bool checkLineCompleted = true;
            foreach (int squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (comp.SquraeOccupied == false)
                {
                    checkLineCompleted = false;
                }
            }

            if (checkLineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach (var line in completedLines)
        {
            var completed = false;
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.DeactivateSquare();
                completed = true;
            }
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }
            if (completed)
            {
                totalLinesCompleted++;
            }
        }

        return totalLinesCompleted;
    }

    private void CheckIfPlayerLost()
    {
        var validShape = 0;
        for (int index = 0; index < shapeStorage.shapesList.Count; index++)
        {
            var shape = shapeStorage.shapesList[index];
            if (shape != null && shape.IsAnyOfShapeSquareActive() && CheckIfShapeCanBePlaceOnGrid(shape))
            {
                //shape.ActivateShape(); //turn off
                validShape++;
            }
        }
        if (validShape == 0)
        {
            //GAME OVER
            GameEvents.GameOver(false);
            audioSource.PlayOneShot(loseSound);
            Debug.Log("GameOver, pls try");
        }
    }

    private bool CheckIfShapeCanBePlaceOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.CurrentShapeData;
        int shapeColums = currentShapeData.columns;
        int shapeRows = currentShapeData.rows;

        //All indexes of filled up squares.
        List<int> originalShapeFilledUpSquares = new List<int>();
        int squaresIndex = 0;

        for (int rowIndex = 0; rowIndex < shapeRows; rowIndex++)
        {
            for (int columIndex = 0; columIndex < shapeColums; columIndex++)
            {
                if (currentShapeData.board[rowIndex].column[columIndex])
                {
                    originalShapeFilledUpSquares.Add(squaresIndex);
                }

                squaresIndex++;
            }
        }
        if (currentShape.totalSquareNumber != originalShapeFilledUpSquares.Count)
        {
            Debug.Log("Error");
        }

        var squareList = GetAllSquaresCombination(shapeColums, shapeRows);
        bool canBePlaced = false;
        foreach (var number in squareList)
        {
            bool shapeCanBePlaceOnTheBoard = true;
            foreach (var squareIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if (comp.SquraeOccupied) // true
                {
                    shapeCanBePlaceOnTheBoard = false;
                }
            }
            if (shapeCanBePlaceOnTheBoard)
            {
                canBePlaced = true;
            }
        }

        return canBePlaced;
    }
    private List<int[]> GetAllSquaresCombination(int columns, int rows)
    {
        if (lineIndicator == null || lineIndicator.lineData == null)
        {
            Debug.LogError("LineIndicator or lineData is null.");
            return new List<int[]>();
        }

        var squareList = new List<int[]>();
        int gridRows = this.rows; //Rows form GridManager
        int gridColumns = this.colums; //Colums form GridManager

        for (int lastRowIndex = 0; lastRowIndex + rows <= gridRows; lastRowIndex++)
        {
            for (int lastColumnIndex = 0; lastColumnIndex + columns <= gridColumns; lastColumnIndex++)
            {
                var rowData = new List<int>();
                for (int row = lastRowIndex; row < lastRowIndex + rows; row++)
                {
                    for (int column = lastColumnIndex; column < lastColumnIndex + columns; column++)
                    {
                        if (row >= 0 && row < gridRows && column >= 0 && column < gridColumns)
                        {
                            rowData.Add(lineIndicator.lineData[row, column]);
                        }
                        else
                        {
                            Debug.LogWarning($"Invalid grid index: [{row}, {column}]");
                        }
                    }
                }
                squareList.Add(rowData.ToArray());
            }
        }

        return squareList;
    }
}
