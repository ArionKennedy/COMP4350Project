Git workflow

1. Create feature branch off of master for your changes  
2. Commit all you changes to your feature branch  
3. Merge feature branch to develop  
4. Test changes in develop  
5. Create PR to master  
6. Merge PR to master once it has approvals

Coding Standards

* Use CamelCase for method and variable names  
* Const variables must be all caps with an underscore between words. Additionally all const variables must be stored in a dedicated constant file  
* Functions should generally accomplish one task  
* Comment complex functions as explicitly as one can within reason  
* Write test cases for one’s own functions  
* For all non-explicitly mentioned standards reference:  
  * [https\://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

Review Practices

* When creating a PR, provide reasons for changes or additions, and a broad overview of what you have done. Also mention if there is AI generated code in the PR description.  
* When making comments on a PR distinguish mandatory fixes from nitpicks  
* Notify the team member who opened the PR that you have reviewed it