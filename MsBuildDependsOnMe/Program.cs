// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018-2019. All rights reserved.
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
            string targetProject = @"S:\TimsSVN\7x\Trunk\Dotnet\Source\Framework\Imaging\Core\Imaging.Core.csproj";
            string[] targetDirectories = new string[] { @"S:\TimsSVN\7x\Trunk\Dotnet", @"S:\TimsSVN\7x\Trunk\Fusion" };

            //IEnumerable<string> projectsThatHaveDirectDependencyOnMe = DependsOnMe.ForDirectReferences(targetProject, targetDirectories);

            //foreach (string project in projectsThatHaveDirectDependencyOnMe)
            //{
            //    Console.WriteLine(project);
            //}

            IEnumerable<string> graphLines = DependsOnMe.GraphDirectReferences(targetProject, targetDirectories);

            foreach (string graphLine in graphLines)
            {
                Console.WriteLine(graphLine);
            }
        }
    }
}
