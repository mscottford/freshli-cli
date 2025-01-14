using System;
using Corgibytes.Freshli.Cli.Functionality.Analysis;

namespace Corgibytes.Freshli.Cli.Functionality.History;

public class HistoryStopPointProcessingFailedEvent : UnhandledExceptionEvent, IHistoryStopPointProcessingTask
{
    public IHistoryStopPointProcessingTask Parent { get; }

    public HistoryStopPointProcessingFailedEvent(IHistoryStopPointProcessingTask parent, Exception error) : base(error)
    {
        Parent = parent;
    }
}
