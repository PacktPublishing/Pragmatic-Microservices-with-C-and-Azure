﻿using System.ComponentModel.DataAnnotations;

namespace Codebreaker.GameAPIs.Models;

public class Move(Guid id, int moveNumber)
{
    [Required]
    public Guid Id { get; private set; } = id;

    /// <summary>
    /// The move number for this move within the associated game.
    /// </summary>
    [Required]
    public int MoveNumber { get; private set; } = moveNumber;

    /// <summary>
    /// The guess pegs from the user for this move.
    /// </summary>
    [Required]
    public required string[] GuessPegs { get; init; }
    /// <summary>
    /// The result from the analyzer for this move based on the associated game that contains the move.
    /// </summary>
    [Required]
    public required string[] KeyPegs { get; init; }

    public override string ToString() => $"{MoveNumber}. " +
        $"{string.Join('#',GuessPegs)} : " +
        $"{string.Join('#', KeyPegs)}";
}
