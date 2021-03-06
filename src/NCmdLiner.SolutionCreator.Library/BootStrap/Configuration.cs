﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Windows.Documents;
using Common.Logging;
using NCmdLiner.SolutionCreator.Library.Services;

namespace NCmdLiner.SolutionCreator.Library.BootStrap
{
    public class Configuration : IConfiguration
    {
        private readonly ITemplatePath _templatePath;
        private readonly ILog _logger;

        public Configuration(ITemplatePath templatePath, ILog logger)
        {
            _templatePath = templatePath;
            _logger = logger;
        }

        private string _logDirectoryPath;
        private string _logFileName;        
        private IEnumerable<string> _templatesFolders;
        
        private NameValueCollection Section
        {
            get
            {
                if (_section == null)
                {
                    var ns = this.GetType().Namespace;
                    if (ns == null) throw new ArgumentException("Unable to determine name space of type: " + this.GetType());
                    var sectionName = ns.Replace(".Library.BootStrap", ""); //Remove the "Plumbing part of the namespace"
                    _section = (NameValueCollection)ConfigurationManager.GetSection(sectionName);
                    if (_section == null)
                    {
                        throw new ConfigurationErrorsException("Missing section in application configuration file: " + sectionName);
                    }
                }
                return _section;
            }
        }
        private NameValueCollection _section;

        public string LogDirectoryPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logDirectoryPath))
                {                    
                    _logDirectoryPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables(Section["LogDirectoryPath"]));
                }
                return _logDirectoryPath;
            }
            set { _logDirectoryPath = value; }
        }

        public string LogFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_logFileName))
                {                    
                    _logFileName = Environment.ExpandEnvironmentVariables(Section["LogFileName"]);
                }
                return _logFileName;
            }
            set { _logFileName = value; }
        }

        public IEnumerable<string> TemplatesFolders
        {
            get
            {
                if (_templatesFolders == null)
                {                    
                    var templatesFoldersString = Section["TemplatesFolders"];
                    var templatesFolders = templatesFoldersString.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var templatesFolder in templatesFolders)
                    {                        
                        var resolvedTemplatesFolder = _templatePath.GetFullPath(templatesFolder);
                        _logger.Debug("Template folder: " + resolvedTemplatesFolder);
                        yield return resolvedTemplatesFolder;
                    }
                }
                else
                {
                    foreach (var templatesFolder in _templatesFolders)
                    {
                        var resolvedTemplatesFolder = _templatePath.GetFullPath(templatesFolder);
                        _logger.Debug("Template folder: " + resolvedTemplatesFolder);
                        yield return resolvedTemplatesFolder;
                    }    
                }                
            }
            set { _templatesFolders = value; }
        }
    }
}