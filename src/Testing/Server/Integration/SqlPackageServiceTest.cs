﻿using NUnit.Framework;
using Remotis.Contract;
using Remotis.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;

namespace Remotis.Testing.Service
{
    [Category ("Integration")]
    public class SqlPackageServiceTest
    {
        

        #region Setup & Cleanup
        [SetUp]
        public void InitializeTestEnvironement()
        {
            CleanOutputFile();
            DeployPackage();
        }

        private void CleanOutputFile()
        {
            if (File.Exists("Toto2.txt"))
                File.Delete("Toto2.txt");
        }
        private void DeployPackage()
        {
            //Build the fullpath for the file to read
            Directory.CreateDirectory("Etl");
            var packageFullPath = FileOnDisk.CreatePhysicalFile(@"Etl\Sample.dtsx", string.Format("{0}.Resources.Sample.dtsx", this.GetType().Namespace));

            CleanPackage();
            var integrationServer = new Application();
            integrationServer.PackagePassword = "p@ssw0rd";
            var package = integrationServer.LoadPackage(packageFullPath, null);

            if (!integrationServer.FolderExistsOnDtsServer(@"File System\RemotisTesting", ConfigurationReader.GetServerName()))
                integrationServer.CreateFolderOnDtsServer(@"File System", "RemotisTesting", ConfigurationReader.GetServerName());
            
            // Save the package under myFolder which is found under the 
            // File System folder on the Integration Services service.
            integrationServer.SaveToDtsServer(package, null, @"File System\RemotisTesting\Sample", ConfigurationReader.GetServerName());
        }

        [TearDown]
        public void CleanupTestEnvironement()
        {
            CleanPackage();
            CleanOutputFile();
        }

        private void CleanPackage()
        {
            var integrationServer = new Application();
            if (integrationServer.ExistsOnDtsServer(@"File System\RemotisTesting\Sample", ConfigurationReader.GetServerName()))
                integrationServer.RemoveFromDtsServer(@"File System\RemotisTesting\Sample", ConfigurationReader.GetServerName());
        } 
        #endregion

        [Test]
        public void Run_FilePackage_Sucessful()
        {
            var packageInfo = new SqlPackage()
            {
                Password="p@ssw0rd",
                Path = @"File System\RemotisTesting\",
                Name="Sample"
            };
            
            var packageService = new PackageService();
            var result = packageService.Run(packageInfo);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Errors, Has.Count.EqualTo(0));
            Assert.That(File.Exists("Toto2.txt"));
        }


    }
}
