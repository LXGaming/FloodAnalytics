using System.ComponentModel.DataAnnotations;

namespace LXGaming.FloodAnalytics.Models; 

public class Torrent {

    [Key, MaxLength(40)]
    public string Id { get; init; } = null!;

    [Required, MaxLength(255)]
    public string Name { get; init; } = null!;

    [Required]
    public string[] Trackers { get; init; } = null!;
    
    [Required]
    public DateTime CreatedAt { get; init; }

    [Required]
    public DateTime UpdatedAt { get; set; }
}