using System;

namespace DL.GameOfLife.Api.Models;

public class BoardModel
{
    string Id { get; set; } = string.Empty;
    List<BoardCellModel> Cells { get; set; } = new();
}
