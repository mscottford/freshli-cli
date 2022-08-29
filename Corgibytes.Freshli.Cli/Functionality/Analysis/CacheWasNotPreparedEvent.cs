using System;
using Corgibytes.Freshli.Cli.CommandRunners.Cache;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Corgibytes.Freshli.Cli.Functionality.Analysis;

public class CacheWasNotPreparedEvent : ErrorEvent
{
    public IServiceProvider ServiceProvider { get; init; } = null!;

    public string CacheDirectory { get; init; } = null!;
    public string RepositoryUrl { get; init; } = null!;
    public string? RepositoryBranch { get; init; }
    public string HistoryInterval { get; init; } = null!;
    public string GitPath { get; init; } = null!;

    public override void Handle(IApplicationActivityEngine eventClient)
    {
        eventClient.Dispatch(new PrepareCacheActivity(CacheDirectory, RepositoryUrl, RepositoryBranch, HistoryInterval)
        {
            CacheDirectory = CacheDirectory,
            RepositoryUrl = RepositoryUrl,
            RepositoryBranch = RepositoryBranch,
            HistoryInterval = HistoryInterval,
            GitPath = GitPath
        });

        eventClient.Dispatch(new RestartAnalysisActivity(
            ServiceProvider.GetRequiredService<ICacheManager>(),
            ServiceProvider.GetRequiredService<IHistoryIntervalParser>()
        )
        {
            CacheDirectory = CacheDirectory,
            RepositoryUrl = RepositoryUrl,
            RepositoryBranch = RepositoryBranch,
            HistoryInterval = HistoryInterval,
            GitPath = GitPath
        });
    }
}
