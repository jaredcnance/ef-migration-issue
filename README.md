### Brief Description

`dotnet ef migrations remove` uses the migration `*.Designer.cs` file to rollback state of the `DbContextModelSnapshot.cs` file.

This is likely to cause problems when merging changes across multiple branches.


### A Typical Workflow

It is not uncommon for multiple developers to work on changes on different branches and then merge those changes back into
their "master" branch. An example workflow might look like:

- branch B and C get created from branch A
- branch B generates migrations at timestamp 0
- branch C generates migrations at timestamp 1, whose designer file does not contain the changes from B
- branch B gets merged into A
- branch C gets merged into A

Resolving conflicts across common files, the `AppDbContextModelSnapshot` and the `DbContext` are relatively straightforward.
Additional migration files are simple because there will be no conflicts. 
However, the `*.Designer.cs` represent a potentially incorrect state of the model because they represent an isolated view of the total state,
not just the migration changes. Although, the `AppDbContextModelSnapshot` is a view of the total state, it is centralized, not isolated and can be easily diffed
and conflicts resolved.

When developers merge their branches back in, the migration designer file for the second branch has the chance of overriding the "state" created
by the first branch. This repository is an example of such a condition.

![workflow](https://raw.githubusercontent.com/jaredcnance/ef-migration-issue/master/state.png)

### Reproduce The Issue

To reproduce the issue, clone this repository and run: `dotnet ef migrations remove`.

#### Expected Result

The preivous migration `20170616182356_modelC` should be removed.

#### What Happens

No migration is removed, but the changes from `20170616182300_modelB` are removed from `AppDbContextModelSnapshot`.

![diff](https://raw.githubusercontent.com/jaredcnance/ef-migration-issue/master/diff.png)

This represents a serious blocker to adopting the EF Core migrations workflow on a multi-person team.
