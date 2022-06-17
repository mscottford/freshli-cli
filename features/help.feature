Feature: Freshli.Cli
    Scenario: Help option
        When I run `freshli --help`
        Then the output should contain:
        """
        Usage:
          freshli [command] [options]
        """
        And the output should contain:
        """
          scan <path>                 Scan command returns metrics results for given local repository path
        """
        And the output should contain:
        """
          cache                       Manages the local cache database
        """
        And the output should contain:
        """
          compute-libyear <filepath>  Computes the libyear for a given CycloneDX file
        """

