using System.ComponentModel.DataAnnotations;

namespace LXGaming.FloodAnalytics.Models; 

public class Traffic {
    
    [Key]
    public long Id { get; init; }

    [Required, MaxLength(40)]
    public string TorrentId { get; init; } = null!;

    [Required]
    public long BytesDone { get; init; }
    
    [Required]
    public long SizeBytes { get; init; }
    
    [Required]
    public decimal PercentComplete { get; init; }
    
    [Required]
    public decimal Ratio { get; init; }
    
    [Required]
    public long DownRate { get; init; }
    
    [Required]
    public long DownTotal { get; init; }
    
    [Required]
    public long UpRate { get; init; }
    
    [Required]
    public long UpTotal { get; init; }
    
    [Required]
    public int PeersConnected { get; init; }
    
    [Required]
    public int PeersTotal { get; init; }
    
    [Required]
    public int SeedsConnected { get; init; }
    
    [Required]
    public int SeedsTotal { get; init; }
    
    [Required]
    public DateTime CreatedAt { get; init; }
    
    public virtual Torrent Torrent { get; init; } = null!;
}