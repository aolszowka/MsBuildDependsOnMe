// -----------------------------------------------------------------------
// <copyright file="ProjectInformationFactory.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildDependsOnMe
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    static class ProjectInformationFactory
    {
        public static ProjectInformation Create(string projectFile)
        {
            ProjectInformation result = new ProjectInformation();

            XDocument projXml = XDocument.Load(projectFile);

            result.Path = projectFile;
            result.AssemblyName = MSBuildUtilities.GetMSBuildAssemblyName(projXml, projectFile);
            result.ProjectGuid = MSBuildUtilities.GetMSBuildProjectGuid(projXml, projectFile);
            result.DependentOnProjects = _GetProjectDependencies(projXml, projectFile);

            return result;
        }

        private static IDictionary<string, string> _GetProjectDependencies(XDocument projXml, string projectPath)
        {
            Dictionary<string, string> projectDependencies = new Dictionary<string, string>();

            IEnumerable<XElement> projectReferenceNodes = MSBuildUtilities.GetProjectReferenceNodes(projXml);

            foreach (XElement projectReferenceNode in projectReferenceNodes)
            {
                string dependentProjectGuid = MSBuildUtilities.GetProjectReferenceGUID(projectReferenceNode, projectPath);
                string relativePathToProject = MSBuildUtilities.GetProjectReferenceIncludeValue(projectReferenceNode, projectPath);
                string pathToDependentProject = PathUtilities.ResolveRelativePath(projectPath, relativePathToProject);

                if (projectDependencies.ContainsKey(dependentProjectGuid))
                {
                    string message = $"Guid `{dependentProjectGuid}` is duplicated in project `{projectPath}`";
                    throw new InvalidOperationException(message);
                }
                else
                {
                    projectDependencies.Add(dependentProjectGuid, pathToDependentProject);
                }
            }

            return projectDependencies;
        }
    }
}
