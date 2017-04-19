using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace NCmdLiner.SolutionCreator
{
    [RunInstaller(true)]
    public partial class SolutionCreatorInstaller : System.Configuration.Install.Installer
    {
        public SolutionCreatorInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            this.Context.LogMessage("Adding NCmdLiner.SolutionCreator to File Explorer context menu...");
            const string solutionCreatorKeyPath = @"Software\Classes\Folder\shell\NCmdLiner.SolutionCreator";
            using (var solutionCreatorKey = Registry.LocalMachine.CreateSubKey(solutionCreatorKeyPath))
            {
                if (solutionCreatorKey == null) throw new NullReferenceException("Failed to create or open registry key: " + solutionCreatorKeyPath);
                solutionCreatorKey.SetValue(null,"Create New NCmdLiner Solution...");               
            }
            
            const string solutionCreatorCommandKeyPath = @"Software\Classes\Folder\shell\NCmdLiner.SolutionCreator\command";
            using (var solutionCreatorCommandKey = Registry.LocalMachine.CreateSubKey(solutionCreatorCommandKeyPath))
            {
                if (solutionCreatorCommandKey == null) throw new NullReferenceException("Failed to create or open registry key: " + solutionCreatorCommandKeyPath);
                solutionCreatorCommandKey.SetValue(null, string.Format("\"{0}\" CreateSolution /targetRootFolder=\"%1\"", Assembly.GetExecutingAssembly().Location), RegistryValueKind.String);
            }

            const string solutionCreatorKeyPath2 = @"Software\Classes\*\shell\NCmdLiner.SolutionCreator";
            using (var solutionCreatorKey = Registry.LocalMachine.CreateSubKey(solutionCreatorKeyPath2))
            {
                if (solutionCreatorKey == null) throw new NullReferenceException("Failed to create or open registry key: " + solutionCreatorKeyPath2);
                solutionCreatorKey.SetValue(null, "Create New NCmdLiner Solution...");
            }

            const string solutionCreatorCommandKeyPath2 = @"Software\Classes\*\shell\NCmdLiner.SolutionCreator\command";
            using (var solutionCreatorCommandKey = Registry.LocalMachine.CreateSubKey(solutionCreatorCommandKeyPath2))
            {
                if (solutionCreatorCommandKey == null) throw new NullReferenceException("Failed to create or open registry key: " + solutionCreatorCommandKeyPath2);
                solutionCreatorCommandKey.SetValue(null, string.Format("\"{0}\" CreateSolution /targetRootFolder=\"%1\"", Assembly.GetExecutingAssembly().Location), RegistryValueKind.String);
            }
            this.Context.LogMessage("Finished adding NCmdLiner.SolutionCreator to File Explorer context menu.");
            RemoveEmptyFolders();
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            this.Context.LogMessage("Removing NCmdLiner.SolutionCreator from File Explorer context menu...");
            const string solutionCreatorKeyPath = @"Software\Classes\Folder\shell\NCmdLiner.SolutionCreator";
            const string solutionCreatorKeyPath2 = @"Software\Classes\*\shell\NCmdLiner.SolutionCreator";
            Registry.LocalMachine.DeleteSubKeyTree(solutionCreatorKeyPath);
            Registry.LocalMachine.DeleteSubKeyTree(solutionCreatorKeyPath2);            
            this.Context.LogMessage("Finished removing NCmdLiner.SolutionCreator from File Explorer context menu.");
            RemoveEmptyFolders();
            RemoveTemplatesFolder();
            base.Uninstall(savedState);
        }

        private void RemoveTemplatesFolder()
        {
            var productVersionDirectory = GetProductVersionDirectory();
            if (productVersionDirectory != null)
            {
                var templatesFolder = Path.Combine(productVersionDirectory.FullName, "Templates");
                if (!Directory.Exists(templatesFolder)) return;
                Context.LogMessage("Removing templates folder: " + templatesFolder);
                RemoveFolder(templatesFolder);
            }
        }

        private void RemoveFolder(string folderPath)
        {
            try
            {
                Directory.Delete(folderPath,true);
            }
            catch (Exception ex)
            {
                this.Context.LogMessage($"Failed to remove folder '{folderPath}'. {ex.Message}");
            }            
        }

        private DirectoryInfo GetProductVersionDirectory()
        {
            var assembly = typeof(SolutionCreatorInstaller).Assembly;
            if (assembly.Location == null) return null;
            var assemblyFile = new FileInfo(assembly.Location);
            return assemblyFile.Directory;
        }

        private DirectoryInfo GetProductDirectory()
        {
            var productVersionDirectory = GetProductVersionDirectory();
            return productVersionDirectory?.Parent;
        }

        private void RemoveEmptyFolders()
        {                 
            var productDirectory = GetProductDirectory();
            this.Context.LogMessage("Removing empty folders under: " + productDirectory?.FullName);
            RemoveEmptyFolders(productDirectory?.FullName);
        }

        private void RemoveEmptyFolders(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath)) return;
            var files = Directory.GetFiles(folderPath);
            if (files.Length > 0) return;
            var subFolderPaths = Directory.GetDirectories(folderPath);
            if (subFolderPaths.Length > 0)
            {
                foreach (var subFolderPath in subFolderPaths)
                {
                    RemoveEmptyFolders(subFolderPath);
                }
                subFolderPaths = Directory.GetDirectories(folderPath);
            }
            if (subFolderPaths.Length > 0) return;
            Directory.Delete(folderPath);            
        }
    }
}
