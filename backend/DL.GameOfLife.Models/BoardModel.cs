namespace DL.GameOfLife.Models;

public class BoardModel
{
    public string? Id { get; set; } = string.Empty;
    public List<BoardCellModel> Cells { get; set; } = new();
}
