// -----------------------------------------------------------------------
// <copyright file="DependsOnMe.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018-2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildDependsOnMe
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    static class DependsOnMe
    {
        /// <summary>
        /// Gets all Project Files that are understood by this
        /// tool from the given directory and all subdirectories.
        /// </summary>
        /// <param name="targetDirectory">The directory to scan for projects.</param>
        /// <returns>All projects that this tool supports.</returns>
        internal static IEnumerable<string> GetProjectsInDirectory(string targetDirectory)
        {
            HashSet<string> supportedFileExtensions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                ".csproj",
                ".fsproj",
                ".synproj",
                ".vbproj",
            };

            return
                Directory
                .EnumerateFiles(targetDirectory, "*proj", SearchOption.AllDirectories)
                .Where(currentFile => supportedFileExtensions.Contains(Path.GetExtension(currentFile)));
        }

        internal static IEnumerable<ProjectInformation> LoadProjectInformation(IEnumerable<string> targetDirectories)
        {
            return
                targetDirectories
                .AsParallel()
                .SelectMany(targetDirectory => GetProjectsInDirectory(targetDirectory))
                .Distinct()
                .AsParallel()
                .Select(currentProject => ProjectInformationFactory.Create(currentProject));
        }

        internal static IEnumerable<string> ForDirectReferences(string targetProject, IEnumerable<string> targetDirectories)
        {
            IEnumerable<ProjectInformation> projectsToScan = LoadProjectInformation(targetDirectories);

            return ForDirectReferences(targetProject, projectsToScan.ToArray());

        }

        internal static IEnumerable<string> ForDirectReferences(string targetProject, ProjectInformation[] projectsToScan)
        {
            ProjectInformation targetProjectInformation = ProjectInformationFactory.Create(targetProject);

            return
                projectsToScan
                .AsParallel()
                .Where(projectToScan => projectToScan.DependentOnProjects.ContainsKey(targetProjectInformation.ProjectGuid))
                .Select(projectInformation => projectInformation.Path)
                .Distinct();
        }

        internal static IEnumerable<string> GraphDirectReferences(string targetProject, IEnumerable<string> targetDirectories)
        {
            yield return "digraph g {";

            yield return $"\"{ProjectInformationFactory.Create(targetProject).AssemblyName}\"";

            var directReferences = ForDirectReferences(targetProject, targetDirectories);

            foreach(var directReference in directReferences)
            {
                yield return $"\"{ProjectInformationFactory.Create(directReference).AssemblyName}\"->\"{ProjectInformationFactory.Create(targetProject).AssemblyName}\"";
            }

            yield return "}";
        }

        internal static IEnumerable<string> GraphDownlineTree(string targetProject, IEnumerable<string> targetDirectories)
        {
            ProjectInformation[] projectsToScan = LoadProjectInformation(targetDirectories).ToArray();

            yield return "digraph g {";

            yield return $"\"{ProjectInformationFactory.Create(targetProject).AssemblyName}\"";

            Stack<string> projectsToEvaluate = new Stack<string>();
            HashSet<string> resolvedProjects = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            // Seed this with our project
            projectsToEvaluate.Push(targetProject);

            while (projectsToEvaluate.Count != 0)
            {
                string currentProject = projectsToEvaluate.Pop();

                if (resolvedProjects.Contains(currentProject))
                {
                    // Save the stack and don't do anything
                }
                else
                {
                    // Mark this project as resolved
                    resolvedProjects.Add(currentProject);

                    IEnumerable<string> directReferences = ForDirectReferences(currentProject, projectsToScan);

                    foreach (string directReference in directReferences)
                    {
                        yield return $"\"{ProjectInformationFactory.Create(directReference).AssemblyName}\"->\"{ProjectInformationFactory.Create(currentProject).AssemblyName}\"";

                        if (!resolvedProjects.Contains(directReference))
                        {
                            projectsToEvaluate.Push(directReference);
                        }
                    }
                }
            }

            yield return "}";
        }

    }
}
