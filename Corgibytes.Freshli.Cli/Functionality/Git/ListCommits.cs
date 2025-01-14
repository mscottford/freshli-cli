using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CliWrap;
using Corgibytes.Freshli.Cli.Exceptions;
using Corgibytes.Freshli.Cli.Functionality.Analysis;
using Corgibytes.Freshli.Cli.Resources;

namespace Corgibytes.Freshli.Cli.Functionality.Git;

public class ListCommits : IListCommits
{
    private readonly IConfiguration _configuration;

    public ListCommits(IConfiguration configuration) => _configuration = configuration;

    public IEnumerable<GitCommit> ForRepository(IHistoryStopData historyStopData) => GitLog(historyStopData);

    public GitCommit MostRecentCommit(IHistoryStopData historyStopData)
    {
        // Fetch only the latest as this returns a list (for re-usability) we have to return the first item of that list
        var commit = GitLog(historyStopData, true);
        return commit.First();
    }

    private IEnumerable<GitCommit> GitLog(IHistoryStopData historyStopData, bool latestOnly = false)
    {
        var stdErrBuffer = new StringBuilder();
        var stdOutBuffer = new StringBuilder();
        var command = CliWrap.Cli.Wrap(_configuration.GitPath).WithArguments(
                args =>
                {
                    args
                        .Add("log")
                        // Commit hash, author date, strict ISO 8601 format
                        // Lists commits as '583d813db3e28b9b44a29db352e2f0e1b4c6e420 2021-05-19T15:24:24-04:00'
                        // Source: https://git-scm.com/docs/pretty-formats
                        .Add("--pretty=format:%H %aI");
                    if (latestOnly)
                    {
                        args.Add("--max-count=1");
                    }
                })
            .WithValidation(CommandResultValidation.None)
            .WithWorkingDirectory(historyStopData.Path)
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
            .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer));

        using var task = command.ExecuteAsync().Task;
        task.Wait();

        if (task.Result.ExitCode != 0)
        {
            throw new GitException($"{CliOutput.Exception_Git_EncounteredError}\n{stdErrBuffer}");
        }

        var commitsWithDates = stdOutBuffer.ToString().Split("\n");
        var commits = new List<GitCommit>();

        foreach (var commitAndDate in commitsWithDates)
        {
            var separated = commitAndDate.Split(" ");
            commits.Add(new GitCommit(
                separated[0],
                DateTimeOffset.ParseExact(separated[1], "yyyy'-'MM'-'dd'T'HH':'mm':'ssK", null)
            ));
        }

        return commits;
    }
}
