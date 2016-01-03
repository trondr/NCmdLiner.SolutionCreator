﻿using System;
using Common.Logging;
using NCmdLiner.Exceptions;
using NCmdLiner.SolutionCreator.BootStrap;
using NCmdLiner.SolutionCreator.Library.Common;

namespace NCmdLiner.SolutionCreator
{
    internal class Program
    {
        [STAThread]
        private static int Main(string[] args)
        {
            var returnValue = 0;
            try
            {
                var logger = LogManager.GetLogger<Program>();
                var applicationInfo = BootStrapper.Container.Resolve<IApplicationInfo>();
                try
                {
                    applicationInfo.Authors = @"github.com.trondr";
                    // ReSharper disable once CoVariantArrayConversion
                    object[] commandTargets = BootStrapper.Container.ResolveAll<CommandDefinition>();
                    logger.InfoFormat("Start: {0} ({1}). Command line: {2}", applicationInfo.Name, applicationInfo.Version, Environment.CommandLine);
                    return CmdLinery.Run(commandTargets, args, applicationInfo, new NotepadMessenger());
                }
                catch (MissingCommandException ex)
                {
                    logger.ErrorFormat("Missing command. {0}", ex.Message);
                    returnValue = 1;
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error when exeuting command. {0}", ex.ToString());
                    returnValue = 2;
                }
                finally
                {
                    logger.InfoFormat("Stop: {0} ({1}). Return value: {2}", applicationInfo.Name, applicationInfo.Version, returnValue);
#if DEBUG
                    Console.WriteLine("Press ENTER...");
                    Console.ReadLine();
#endif
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error when wiring up the application.{0}{1}", Environment.NewLine, ex);
                returnValue = 3;
            }
            finally
            {                
#if DEBUG
                Console.WriteLine("Press ENTER again...");
                Console.ReadLine();
#endif
            }
            return returnValue;
        }
    }
}
