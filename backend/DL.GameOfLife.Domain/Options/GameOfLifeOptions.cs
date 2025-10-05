namespace DL.GameOfLife.Domain.Options;

public class GameOfLifeOptions
{
    public int StatesIncrementLimit { get; set; }
    public int ColumnStartOffset { get; set; }
    public int ColumnEndOffset { get; set; }
    public int RowEndOffset { get; set; }
    public int RowStartOffset { get; set; }
}
