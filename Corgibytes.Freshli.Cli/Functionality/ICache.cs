using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Cli.DataModel;
using Corgibytes.Freshli.Cli.Functionality.Git;
using PackageUrl;

namespace Corgibytes.Freshli.Cli.Functionality;

public interface ICacheDb
{
    public Guid SaveAnalysis(CachedAnalysis analysis);
    public CachedAnalysis? RetrieveAnalysis(Guid id);
    public int AddHistoryStopPoint(CachedHistoryStopPoint historyStopPoint);
    public int AddPackageLibYear(CachedPackageLibYear packageLibYear);
    public CachedGitSource? RetrieveCachedGitSource(CachedGitSourceId id);
    public CachedHistoryStopPoint? RetrieveHistoryStopPoint(int historyStopPointId);
    public CachedPackageLibYear? RetrievePackageLibYear(int packageLibYearId);
    public List<CachedPackage> RetrieveCachedReleaseHistory(PackageURL packageUrl);
    public void StoreCachedReleaseHistory(List<CachedPackage> packages);
    public List<string> RetrieveCachedManifests(int historyStopPointId, string agentExecutablePath);
    public void StoreCachedManifests(int historyStopPointId, string agentExecutablePath, List<string> manifests);
}