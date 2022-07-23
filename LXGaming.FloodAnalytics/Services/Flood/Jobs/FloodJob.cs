using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using LXGaming.FloodAnalytics.Services.InfluxDb;
using Quartz;

namespace LXGaming.FloodAnalytics.Services.Flood.Jobs;

[DisallowConcurrentExecution, PersistJobDataAfterExecution]
public class FloodJob : IJob {

    public static readonly JobKey JobKey = JobKey.Create(nameof(FloodJob));
    private readonly FloodService _floodService;
    private readonly InfluxDbService _influxDbService;

    public FloodJob(FloodService floodService, InfluxDbService influxDbService) {
        _floodService = floodService;
        _influxDbService = influxDbService;
    }

    public async Task Execute(IJobExecutionContext context) {
        var torrentListSummary = await _floodService.EnsureAuthenticatedAsync(_floodService.GetTorrentsAsync());
        if (torrentListSummary?.Torrents == null || torrentListSummary.Torrents.Count == 0) {
            return;
        }

        var points = new List<PointData>(torrentListSummary.Torrents.Count);
        foreach (var (_, value) in torrentListSummary.Torrents) {
            var point = PointData
                .Measurement("torrent")
                .Tag("id", value.Hash)
                .Tag("name", value.Name)
                .Tag("trackers", string.Join(',', value.TrackerUris!))
                .Field("bytes_done", value.BytesDone)
                .Field("size_bytes", value.SizeBytes)
                .Field("percent_complete", value.PercentComplete)
                .Field("ratio", value.Ratio)
                .Field("down_rate", value.DownRate)
                .Field("down_total", value.DownTotal)
                .Field("up_rate", value.UpRate)
                .Field("up_total", value.UpTotal)
                .Field("peers_connected", value.PeersConnected)
                .Field("peers_total", value.PeersTotal)
                .Field("seeds_connected", value.SeedsConnected)
                .Field("seeds_total", value.SeedsTotal)
                .Timestamp(context.ScheduledFireTimeUtc ?? context.FireTimeUtc, WritePrecision.S);

            points.Add(point);
        }

        await _influxDbService.Client.GetWriteApiAsync().WritePointsAsync(points, _influxDbService.Bucket, _influxDbService.Organization);
    }
}