using System;
using Corgibytes.Freshli.Cli.DataModel;
using Corgibytes.Freshli.Cli.Extensions;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Corgibytes.Freshli.Cli.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace Corgibytes.Freshli.Cli.Functionality.Analysis;

public class PrepareCacheForAnalysisActivity : IApplicationActivity
{
    public PrepareCacheForAnalysisActivity(string repositoryUrl = "", string? repositoryBranch = null,
        string historyInterval = "", CommitHistory useCommitHistory = CommitHistory.AtInterval,
        RevisionHistoryMode revisionHistoryMode = RevisionHistoryMode.AllRevisions)
    {
        RepositoryUrl = repositoryUrl;
        RepositoryBranch = repositoryBranch;
        HistoryInterval = historyInterval;
        UseCommitHistory = useCommitHistory;
        RevisionHistoryMode = revisionHistoryMode;
    }

    public string RepositoryUrl { get; init; }
    public string? RepositoryBranch { get; init; }
    public CommitHistory UseCommitHistory { get; init; }
    public RevisionHistoryMode RevisionHistoryMode { get; init; }

    // TODO: Research how to use a value class here instead of a string
    public string HistoryInterval { get; init; }

    public void Handle(IApplicationEventEngine eventClient)
    {
        var configuration = eventClient.ServiceProvider.GetRequiredService<IConfiguration>();
        var cacheManager = new CacheManager(configuration);
        Console.Out.WriteLine(CliOutput.CachePrepareCommandRunner_Run_Preparing_cache, configuration.CacheDir);
        try
        {
            cacheManager.Prepare().ToExitCode();
            var cacheDb = cacheManager.GetCacheDb();
            cacheDb.SaveAnalysis(new CachedAnalysis(RepositoryUrl, RepositoryBranch, HistoryInterval,
                UseCommitHistory, RevisionHistoryMode));
            eventClient.Fire(new CachePreparedForAnalysisEvent
            {
                RepositoryUrl = RepositoryUrl,
                RepositoryBranch = RepositoryBranch,
                HistoryInterval = HistoryInterval,
                UseCommitHistory = UseCommitHistory,
                RevisionHistoryMode = RevisionHistoryMode
            });
        }
        catch (CacheException e)
        {
            Console.Error.WriteLine(e.Message);
        }
    }
}