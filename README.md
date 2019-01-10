# MsBuildDependsOnMe
Generate a list of projects that depend (via ProjectReference) on a specific project

## Background
In a large source tree it is helpful to understand who depends on a particular project.

While you can use tools like [MsBuildProjectReferenceDependencyGraph](https://github.com/aolszowka/MsBuildProjectReferenceDependencyGraph) to generate the entire dependency tree that will not tell you everyone who depends on you. That information is important when you look to depreciate or enhance particular projects.

## When To Use This Tool
This tool is used when you want to generate a list of projects that have a ProjectReferece ([Microsoft Docs: Common MSBuild project items](https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items?view=vs-2017)) dependency on the target project.

## Hacking
This tool is rough around the edges and was written to be quick and dirty; there is plenty of room for improvements to improve its usability.

## Contributing
Pull requests and bug reports are welcomed so long as they are MIT Licensed.

## License
This tool is MIT Licensed.