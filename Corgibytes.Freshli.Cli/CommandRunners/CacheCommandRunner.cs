using System;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using Corgibytes.Freshli.Cli.CommandOptions;
using Corgibytes.Freshli.Cli.Extensions;
using Corgibytes.Freshli.Cli.Functionality;
using Corgibytes.Freshli.Lib;

namespace Corgibytes.Freshli.Cli.CommandRunners
{
    public class CacheCommandRunner : CommandRunner<CacheCommandOptions>
    {
        public CacheCommandRunner(IServiceProvider serviceProvider, Runner runner)
            : base(serviceProvider, runner)
        {

        }

        public override int Run(CacheCommandOptions options, InvocationContext context)
        {
            return 0;
        }
    }

    public class CachePrepareCommandRunner : CommandRunner<CachePrepareCommandOptions>
    {
        public CachePrepareCommandRunner(IServiceProvider serviceProvider, Runner runner)
            : base(serviceProvider, runner)
        {

        }

        public override int Run(CachePrepareCommandOptions options, InvocationContext context)
        {
            context.Console.Out.WriteLine($"Preparing cache at {options.CacheDir}");
            try
            {
                return Cache.Prepare(options.CacheDir).ToExitCode();
            }
            catch (CacheException e)
            {
                context.Console.Error.WriteLine(e.Message);
                return false.ToExitCode();
            }
        }
    }

    public class CacheDestroyCommandRunner : CommandRunner<CacheDestroyCommandOptions>
    {
        public CacheDestroyCommandRunner(IServiceProvider serviceProvider, Runner runner)
            : base(serviceProvider, runner)
        {

        }

        public override int Run(CacheDestroyCommandOptions options, InvocationContext context)
        {
            // Unless the --force flag is passed, prompt the user whether they want to destroy the cache
            if (!options.Force && !Confirm($"Do you want to completely DELETE the directory {options.CacheDir.FullName}?", context))
            {
                context.Console.Out.WriteLine("Operation aborted. Cache not destroyed.");
                return true.ToExitCode();
            }

            // Destroy the cache
            context.Console.Out.WriteLine($"Destroying cache at {options.CacheDir}");
            try
            {
                return Cache.Destroy(options.CacheDir).ToExitCode();
            }
            // Catch errors
            catch (CacheException error)
            {
                context.Console.Error.WriteLine(error.Message);
                return error.IsWarning.ToExitCode();
            }

        }
    }
}
