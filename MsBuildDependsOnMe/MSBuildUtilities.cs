// -----------------------------------------------------------------------
// <copyright file="MSBuildUtilities.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildDependsOnMe
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public static class MSBuildUtilities
    {
        internal static XNamespace msbuildNS = @"http://schemas.microsoft.com/developer/msbuild/2003";

        /// <summary>
        /// Extracts the Project GUID from the specified proj File.
        /// </summary>
        /// <param name="projXml">The proj Xml</param>
        /// <param name="pathToProjFile">The proj File to extract the Project GUID from.</param>
        /// <returns>The specified proj File's Project GUID.</returns>
        public static string GetMSBuildProjectGuid( XDocument projXml, string pathToProjFile )
        {
            string projectGuid = null;

            XElement projectGuidElement = projXml.Descendants(msbuildNS + "ProjectGuid").FirstOrDefault();

            if(projectGuidElement == null)
            {
                string exceptionMessage = $"The Project {pathToProjFile} did not have an <ProjectGuid>";
                throw new InvalidOperationException(exceptionMessage);
            }
            else
            {
                projectGuid = projectGuidElement.Value;
            }

            return projectGuid;
        }

        public static string GetMSBuildAssemblyName( XDocument projXml, string pathToProjFile )
        {
            string assemblyName = string.Empty;

            XElement assemblyNameElement = projXml.Descendants(msbuildNS + "AssemblyName").FirstOrDefault();

            if(assemblyNameElement == null)
            {
                string exceptionMessage = $"The Project `{pathToProjFile}` did not have an AssemblyName";
                throw new InvalidOperationException(exceptionMessage);
            }
            else
            {
                assemblyName = assemblyNameElement.Value;
            }

            return assemblyName;
        }

        public static IEnumerable<XElement> GetProjectReferenceNodes( XDocument projXml )
        {
            return projXml.Descendants(msbuildNS + "ProjectReference");
        }

        public static string GetProjectReferenceGUID( XElement projectReference, string projectPath )
        {
            // Get the existing Project Reference GUID
            XElement projectReferenceGuidElement = projectReference.Descendants(msbuildNS + "Project").FirstOrDefault();
            if(projectReferenceGuidElement == null)
            {
                string exception = $"A ProjectReference in {projectPath} does not contain a Project Element; this is invalid.";
                throw new InvalidOperationException(exception);
            }

            // This is the referenced project
            string projectReferenceGuid = projectReferenceGuidElement.Value;

            return projectReferenceGuid;
        }

        public static string GetProjectReferenceName( XElement projectReference, string projectPath )
        {
            // Get the existing Project Reference Name
            XElement projectReferenceNameElement = projectReference.Descendants(msbuildNS + "Name").FirstOrDefault();
            if(projectReferenceNameElement == null)
            {
                string exception = $"A ProjectReference in {projectPath} does not contain a Name Element; this is invalid.";
                throw new InvalidOperationException(exception);
            }

            // This is the referenced project
            string projectReferenceName = projectReferenceNameElement.Value;

            return projectReferenceName;
        }

        public static string GetProjectReferenceIncludeValue( XElement projectReference, string projectPath )
        {
            // Get the existing Project Reference Include Value
            XAttribute projectReferenceIncludeAttribute = projectReference.Attribute("Include");

            if(projectReferenceIncludeAttribute == null)
            {
                string exception = $"A ProjectReference in {projectPath} does not contain an Include Attribute on it; this is invalid.";
                throw new InvalidOperationException(exception);
            }

            // This is the referenced project
            string projectReferenceInclude = projectReferenceIncludeAttribute.Value;

            return projectReferenceInclude;
        }


    }
}
