// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildDependsOnMe
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            string targetProject = @"S:\TimsSVN\6x\Trunk\Dotnet\Tests\Utilities\ComputersUnlimited.Tests.Utilities.csproj";
            string[] targetDirectories = new string[] { @"S:\TimsSVN\6x\Trunk\Dotnet", @"S:\TimsSVN\6x\Trunk\Fusion" };

            IEnumerable<string> projectsThatHaveDirectDependencyOnMe = DependsOnMe.ForDirectReferences(targetProject, targetDirectories);

            foreach (string project in projectsThatHaveDirectDependencyOnMe)
            {
                Console.WriteLine(project);
            }
        }
    }
}
