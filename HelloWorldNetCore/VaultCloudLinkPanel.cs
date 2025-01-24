/*=====================================================================
  
  This file is part of the Autodesk Vault API Code Samples.

  Copyright (C) Autodesk Inc.  All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.
=====================================================================*/

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
//[assembly: AssemblyProduct("VaultCloudLinkPanelExplorerExtension")]
[assembly: AssemblyDescription("Sample App")]

// The extension ID needs to be unique for each extension.  
// Make sure to generate your own ID when writing your own extension. 
[assembly: Autodesk.Connectivity.Extensibility.Framework.ExtensionId("[Guid(08D2A77C-D7D8-4837-AA7A-E22869474664)]")]

// This number gets incremented for each Vault release.
// *ComponentUpgradeEveryRelease-Client*
[assembly: Autodesk.Connectivity.Extensibility.Framework.ApiVersion("19.0")]


namespace VaultCloudLinkPanel
{

    /// <summary>
    /// This class implements the IExtension interface, which means it tells Vault Explorer what 
    /// commands and custom tabs are provided by this extension.
    /// </summary>
    public class VaultCloudLinkPanelExplorerExtension : IExplorerExtension
    {

        #region IExtension Members

        /// <summary>
        /// This function tells Vault Explorer what custom commands this extension provides.
        /// Part of the IExtension interface.
        /// </summary>
        /// <returns>A collection of CommandSites, which are collections of custom commands.</returns>
        public IEnumerable<CommandSite>? CommandSites()
        {
            return null;
        }


        /// <summary>
        /// This function tells Vault Explorer what custom tabs this extension provides.
        /// Part of the IExtension interface.
        /// </summary>
        /// <returns>A collection of DetailTabs, each object represents a custom tab.</returns>
        public IEnumerable<DetailPaneTab>? DetailTabs()
        {
            return null;
        }


        /// <summary>
        /// This function tells Vault Explorer what custom dock panels this extension provides.
        /// Part of the IExtension interface.
        /// </summary>
        /// <returns>A collection of DockPanel(s), each object represents a custom dock panel.</returns>
        public IEnumerable<DockPanel> DockPanels()
        {
            // Create a DockPanel list to return from method
            List<DockPanel> dockPanels = new List<DockPanel>();

            // Create a DockPanel for displaying in Vault Explorer
            DockPanel? CloudViewPanel = new DockPanel(Guid.Parse("A3F2C322-B55F-41B5-819E-2620534F1B21"),
                                                "HelloWorldDockPanel", typeof(MyCustomTabControl));
            CloudViewPanel.SelectionChanged += CloudViewPanel_SelectionChanged;
            dockPanels.Add(CloudViewPanel);

            // Returns panels
            return dockPanels;
        }




        /// <summary>
        /// This function is called after the user logs in to the Vault Server.
        /// Part of the IExtension interface.
        /// </summary>
        /// <param name="application">Provides information about the running application.</param>
        public void OnLogOn(IApplication application)
        {
        }

        /// <summary>
        /// This function is called after the user is logged out of the Vault Server.
        /// Part of the IExtension interface.
        /// </summary>
        /// <param name="application">Provides information about the running application.</param>
        public void OnLogOff(IApplication application)
        {
        }

        /// <summary>
        /// This function is called before the application is closed.
        /// Part of the IExtension interface.
        /// </summary>
        /// <param name="application">Provides information about the running application.</param>
        public void OnShutdown(IApplication application)
        {
            // Although this function is empty for this project, it's still needs to be defined 
            // because it's part of the IExtension interface.
        }

        /// <summary>
        /// This function is called after the application starts up.
        /// Part of the IExtension interface.
        /// </summary>
        /// <param name="application">Provides information about the running application.</param>
        public void OnStartup(IApplication application)
        {
            // Although this function is empty for this project, it's still needs to be defined 
            // because it's part of the IExtension interface.
        }

        /// <summary>
        /// This function tells Vault Exlorer which default commands should be hidden.
        /// Part of the IExtension interface.
        /// </summary>
        /// <returns>A collection of command names.</returns>
        public IEnumerable<string>? HiddenCommands()
        {
            // This extension does not hide any commands.
            return null;
        }

        /// <summary>
        /// This function allows the extension to define special behavior for Custom Entity types.
        /// Part of the IExtension interface.
        /// </summary>
        /// <returns>A collection of CustomEntityHandler objects.  Each object defines special behavior
        /// for a specific Custom Entity type.</returns>
        public IEnumerable<CustomEntityHandler>? CustomEntityHandlers()
        {
            // This extension does not provide special Custom Entity behavior.
            return null;
        }

        #endregion

        
        /// <summary>
        /// This function is called whenever our custom panel is active and the selection has changed in the main grid.
        /// </summary>
        /// <param name="sender">The sender object.  Usually not used.</param>
        /// <param name="e">The event args.  Provides additional information about the environment.</param>
        void CloudViewPanel_SelectionChanged(object sender, DockPanelSelectionChangedEventArgs e)
        {
            try
            {
                // The event args has our custom panel object.  We need to cast it to our type.
                CloudViewControl? CefControl = e.Context.UserControl as CloudViewControl;

                // Get the selected object's CloudLink property value
                string mUrl = string.Empty;
                if (e.Context.SelectedObjects.Length > 0)
                {
                    // Get the selected object
                    var selectedObject = e.Context.SelectedObjects[0];
                    // Get the CloudLink property value
                    PropertyInfo cloudLinkProperty = selectedObject.GetType().GetProperty("CloudLink");
                    if (cloudLinkProperty != null)
                    {
                        mUrl = cloudLinkProperty.GetValue(selectedObject) as string;
                    }
                }

                // Send selection to the panel so that it can display the object.
                CefControl?.Navigate(mUrl);
            }
            catch (Exception ex)
            {
                // If something goes wrong, we don't want the exception to bubble up to Vault Explorer.
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
