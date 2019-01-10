// -----------------------------------------------------------------------
// <copyright file="ProjectInformation.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildDependsOnMe
{
    using System;
    using System.Collections.Generic;

    public class ProjectInformation : IEquatable<ProjectInformation>
    {
        public string AssemblyName
        {
            get;
            set;
        }

        public string ProjectGuid
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;

        }

        public IDictionary<string, string> DependentOnProjects
        {
            get;
            set;
        }

        public override int GetHashCode()
        {
            return ProjectGuid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectInformation);
        }

        public bool Equals(ProjectInformation other)
        {
            bool areEqual = false;

            if (other != null)
            {
                if (GetHashCode() == other.GetHashCode())
                {
                    if (ProjectGuid.Equals(other.ProjectGuid))
                    {
                        areEqual = true;
                    }
                }
            }

            return areEqual;
        }
    }
}
