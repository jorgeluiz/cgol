using System.ComponentModel;

namespace DL.GameOfLife.Domain.Enums;

public enum ErrorCodes
{
    /// <summary>
    /// Internal server error
    /// </summary>
    [Description("An internal error occurred while processing your request")]
    ERR_0000,
    /// <summary>
    /// Bad request
    /// </summary>
    [Description("There was an error in the request you sent to the system.")]
    ERR_0001,
    /// <summary>
    /// Not Found
    /// </summary>
    [Description("Game not found")]
    ERR_0002,
    /// <summary>
    /// State progression limit reached
    /// </summary>
    [Description("State progression limit reached. The system returned the last valid state")]
    ERR_0003
}
