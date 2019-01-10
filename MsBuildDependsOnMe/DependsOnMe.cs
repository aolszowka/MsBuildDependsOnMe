// -----------------------------------------------------------------------
// <copyright file="DependsOnMe.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018. All rights reserved.
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
            HashSet<string> supportedFileExtensions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { ".csproj", ".vbproj", ".synproj", ".fsproj" };

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
            ProjectInformation targetProjectInformation = ProjectInformationFactory.Create(targetProject);

            IEnumerable<ProjectInformation> projectsToScan = LoadProjectInformation(targetDirectories);

            return
                projectsToScan
                .AsParallel()
                .Where(projectToScan => projectToScan.DependentOnProjects.ContainsKey(targetProjectInformation.ProjectGuid))
                .Select(projectInformation => projectInformation.Path)
                .Distinct();
        }

    }
}
