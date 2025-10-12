namespace DL.GameOfLife.Models;

public class BoardModelRequest
{
    public List<BoardCellModel> Cells { get; set; } = new();
}

public class UpdateBoardModelRequest : BoardModelRequest
{
    public required string Id { get; set; }
}

public class BoardModelResponse
{
    public string? Id { get; set; } = string.Empty;
    public string? ParentId { get; set; } = string.Empty;
    public List<BoardCellModel> Cells { get; set; } = new();
    public IList<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
}

