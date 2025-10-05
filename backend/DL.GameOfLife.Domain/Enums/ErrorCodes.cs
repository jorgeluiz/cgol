using System.ComponentModel;

namespace DL.GameOfLife.Domain.Enums;

public enum ErrorCodes
{
    /// <summary>
    /// Internal server error
    /// </summary>
    [Description("An internal error occoured when the system tried to process your request")]
    ERR_0000,
    /// <summary>
    /// Bad request
    /// </summary>
    [Description("There is something wrong with the request you sent to be processed by the system")]
    ERR_0001,
    /// <summary>
    /// Not Found
    /// </summary>
    [Description("Game not found")]
    ERR_0002
}
