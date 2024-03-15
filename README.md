# ASML project readme

## Pull request policy

The `main` and `dev` branches are protected. For each new feature, change to the `dev` branch and create a new branch from there. Name it something like `JoelVermeulen-LoginScreen` You can commit and push to your branch as much as you want. The last commit that you make to that branch is the only one that can be merged into `dev`. This commit should be fully working and the tests should be passing.

Then, merge the `dev` branch into your feature branch. This way you can solve any merge conflicts on your own machine. Then, test run the app and run all of the tests.

If everything is working, create a pull request from your feature branch to the `dev` branch. This pull request will be automatically tested. If it works, someone can review it and merge it into the `dev` branch.

When the `dev` branch is stable and fully working, a pull request is created to merge it into the `main` branch. This pull request will also be automatically tested. If it works, someone can review it and merge it into the `main` branch.
