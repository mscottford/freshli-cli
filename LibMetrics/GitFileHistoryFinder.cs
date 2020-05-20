using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LibGit2Sharp;

namespace LibMetrics
{
  public class GitFileHistoryFinder: IFileHistoryFinder
  {
    public bool DoesPathContainHistorySource(string projectRootPath)
    {
      bool result = Repository.IsValid(projectRootPath);
      if (!result)
      {
        result = IsCloneable(projectRootPath);
      }

      return result;
    }

    private bool IsCloneable(string url)
    {
      var result = true;
      var options = new CloneOptions {Checkout = false};

      string tempFolder = Path.Combine(
        Path.GetTempPath(),
        Guid.NewGuid().ToString());

      try
      {
        Repository.Clone(url, tempFolder, options);
      }
      catch (NotFoundException)
      {
        result = false;
      }

      if (Directory.Exists(tempFolder))
      {
        Directory.Delete(tempFolder, recursive: true);
      }

      return result;
    }

    public IFileHistory FileHistoryOf(string projectRootPath, string targetFile)
    {
      return new GitFileHistory(projectRootPath, targetFile);
    }
  }
}
