Feature: Cache
    Scenario: Prepare
        Given a directory named "~/.freshli" does not exist
        When I run `freshli cache prepare`
        Then the directory named "~/.freshli" should exist
        And a file named "~/.freshli/freshli.db" should exist
        And we can open a SQLite connection to "~/.freshli/freshli.db"

    Scenario: Prepare with overridden cache-dir
        Given a directory named "somewhere_else" does not exist
        When I run `freshli cache prepare --cache-dir somewhere_else`
        Then the directory named "somewhere_else" should exist
        And a file named "somewhere_else/freshli.db" should exist
        And we can open a SQLite connection to "somewhere_else/freshli.db"

    Scenario: Destroy
        Given a directory named "~/.freshli" exists
        And a blank file named "~/.freshli/freshli.db" exists
        When I run `freshli cache destroy` interactively
        And I type "y"
        Then the directory named "~/.freshli" should not exist

    Scenario: Destroy Cancel
        Given a directory named "~/.freshli" exists
        And a blank file named "~/.freshli/freshli.db" exists
        When I run `freshli cache destroy` interactively
        And I type "n"
        Then the directory named "~/.freshli" should exist

    Scenario: Destroy cancel by default
        Given a directory named "~/.freshli" exists
        And a blank file named "~/.freshli/freshli.db" exists
        When I run `freshli cache destroy` interactively
        And I type ""
        Then the directory named "~/.freshli" should exist

    Scenario: Destroy force
        Given a directory named "~/.freshli" exists
        And a blank file named "~/.freshli/freshli.db" exists
        When I run `freshli cache destroy --force`
        Then the directory named "~/.freshli" should not exist

    Scenario: Destroy with overridden cache-dir
        Given a directory named "somewhere_else" exists
        And a blank file named "somewhere_else/freshli.db" exists
        When I run `freshli cache destroy --cache-dir somewhere_else` interactively
        And I type "y"
        Then the directory named "somewhere_else" should not exist

    Scenario: Destroy cancel with overridden cache-dir
        Given a directory named "somewhere_else" exists
        And a blank file named "somewhere_else/freshli.db" exists
        When I run `freshli cache destroy --cache-dir somewhere_else` interactively
        And I type "n"
        Then the directory named "somewhere_else" should exist

    Scenario: Destroy cancel by default with overridden cache-dir
        Given a directory named "somewhere_else" exists
        And a blank file named "somewhere_else/freshli.db" exists
        When I run `freshli cache destroy --cache-dir somewhere_else` interactively
        And I type ""
        Then the directory named "somewhere_else" should exist

    Scenario: Destroy force with overridden cache-dir
        Given a directory named "somewhere_else" exists
        And a blank file named "somewhere_else/freshli.db" exists
        When I run `freshli cache destroy --cache-dir somewhere_else --force`
        Then the directory named "somewhere_else" should not exist

    Scenario: Destroy on non-cache folder
        Given a directory named "~/.freshli" exists
        And a blank file named "~/.freshli/nonsense.txt" exists
        When I run `freshli cache destroy --force`
        Then the directory named "~/.freshli" should exist
        And the file named "~/.freshli/nonsense.txt" should exist