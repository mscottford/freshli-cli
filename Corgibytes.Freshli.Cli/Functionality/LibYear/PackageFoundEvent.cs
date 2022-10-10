using System;
using Corgibytes.Freshli.Cli.Functionality.Analysis;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using PackageUrl;

namespace Corgibytes.Freshli.Cli.Functionality.LibYear;

public class PackageFoundEvent : IApplicationEvent
{
    public Guid AnalysisId { get; init; }
    public IHistoryStopData HistoryStopData { get; init; } = null!;
    public string AgentExecutablePath { get; init; } = null!;
    public PackageURL Package { get; init; } = null!;

    public void Handle(IApplicationActivityEngine eventClient) =>
        eventClient.Dispatch(new ComputeLibYearForPackageActivity
        {
            AnalysisId = AnalysisId,
            HistoryStopData = HistoryStopData,
            AgentExecutablePath = AgentExecutablePath,
            Package = Package
        });
}
