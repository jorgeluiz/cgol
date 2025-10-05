using System;

namespace DL.GameOfLife.Domain.Entities;

public class Board
{
    public string Id { get; set; } = string.Empty;
    public string? ParentId { get; set; } = string.Empty;
    public List<BoardCell> Cells { get; set; } = new();
}
