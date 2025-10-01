using System;

namespace DL.GameOfLife.Domain.Entities;

public class BoardCell
{
    public int RowNumber { get; set; }
    public int ColumnNumber { get; set; }
    public bool IsAlive { get; set; }
}
