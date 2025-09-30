using System;

namespace DL.GameOfLife.Api.Models;

public class BoardCellModel
{
    int RowNumber { get; set; }
    int ColumnNumber { get; set; }
    bool IsDead { get; set; }
}
