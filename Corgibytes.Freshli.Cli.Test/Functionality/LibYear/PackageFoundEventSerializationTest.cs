using System;
using Corgibytes.Freshli.Cli.Functionality.LibYear;
using PackageUrl;
using Xunit;

namespace Corgibytes.Freshli.Cli.Test.Functionality.LibYear;

[IntegrationTest]
// ReSharper disable once UnusedType.Global
public class PackageFoundEventSerializationTest : SerializationTest<PackageFoundEvent>
{
    protected override PackageFoundEvent BuildIncoming() =>
        new()
        {
            AgentExecutablePath = "/path/to/agent",
            AnalysisId = Guid.NewGuid(),
            HistoryStopPointId = 29,
            Package = new PackageURL(
                "pkg:maven/org.apache.xmlgraphics/batik-anim@1.9.1?repository_url=repo.spring.io%2Frelease")
        };

    protected override void AssertEqual(PackageFoundEvent incoming, PackageFoundEvent outgoing)
    {
        Assert.Equal(incoming.AgentExecutablePath, outgoing.AgentExecutablePath);
        Assert.Equal(incoming.AnalysisId, outgoing.AnalysisId);
        Assert.Equal(incoming.HistoryStopPointId, outgoing.HistoryStopPointId);
        Assert.Equal(incoming.Package.ToString(), outgoing.Package.ToString());
    }
}