﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiit_passport.Models;
[Table("session_numbers")]
public class SessionNumber(string sessionId, int number, Status status, string name) : Interfaces.ISessionNumbers
{
    [Key]
    [Required]
    [Column("session_id")]
    public string SessionId { get; set; } = sessionId;

    [Required]
    [Column("number")]
    public int Number { get; set; } = number;

    [Required]
    [Column("status")]
    public Status Status { get; set; } = status;
    
    [Required]
    [Column("name")]
    public string Name { get; set; } = name;
    
    public void Update(SessionNumber sessionNumber)
    {
        Number = sessionNumber.Number;
        Status = sessionNumber.Status;
    }
}
