using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Autodesk.Connectivity.Explorer.Extensibility;
using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Connections;
using VDF = Autodesk.DataManagement.Client.Framework;
using VDFV = Autodesk.DataManagement.Client.Framework.Vault;


// These 5 assembly attributes must be specified or your extension will not load. 
//[assembly: AssemblyCompany("Autodesk")]
//[assembly: AssemblyProduct("HelloWorldCommandExtension")]
[assembly: AssemblyDescription("Sample App")]

// The extension ID needs to be unique for each extension.  
// Make sure to generate your own ID when writing your own extension. 
[assembly: Autodesk.Connectivity.Extensibility.Framework.ExtensionId("7ADC0766-F085-46d7-A2EB-C68F79CBF4E7")]

// This number gets incremented for each Vault release.
// *ComponentUpgradeEveryRelease-Client*
[assembly: Autodesk.Connectivity.Extensibility.Framework.ApiVersion("19.0")]

namespace HelloWorldNetCore
{
    public class HelloWorldNetExtension : IExplorerExtension
    {

        IEnumerable<CommandSite> IExplorerExtension.CommandSites()
        {
            throw new NotImplementedException();
        }

        IEnumerable<CustomEntityHandler> IExplorerExtension.CustomEntityHandlers()
        {
            throw new NotImplementedException();
        }

        IEnumerable<DetailPaneTab> IExplorerExtension.DetailTabs()
        {
            throw new NotImplementedException();
        }

        IEnumerable<DockPanel> IExplorerExtension.DockPanels()
        {
            throw new NotImplementedException();
        }

        IEnumerable<string> IExplorerExtension.HiddenCommands()
        {
            throw new NotImplementedException();
        }

        void IExplorerExtension.OnLogOff(IApplication application)
        {
            throw new NotImplementedException();
        }

        void IExplorerExtension.OnLogOn(IApplication application)
        {
            throw new NotImplementedException();
        }

        void IExplorerExtension.OnShutdown(IApplication application)
        {
            throw new NotImplementedException();
        }

        void IExplorerExtension.OnStartup(IApplication application)
        {
            throw new NotImplementedException();
        }
    }
}
