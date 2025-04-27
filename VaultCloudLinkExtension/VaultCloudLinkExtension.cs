﻿/*=====================================================================
  
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
using ACW = Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Connections;
using static System.Net.Mime.MediaTypeNames;
using VDF = Autodesk.DataManagement.Client.Framework;
using VDFV = Autodesk.DataManagement.Client.Framework.Vault;
using System.Data.Common;
using System.Runtime;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using VaultCloudLinkExtension;



// These 5 assembly attributes must be specified or your extension will not load. 
//[assembly: AssemblyCompany("Autodesk")]
//[assembly: AssemblyProduct("VaultCloudLinkExtension")]
[assembly: AssemblyDescription("Cloud Link Panel - Sample App")]

// The extension ID needs to be unique for each extension.  
// Make sure to generate your own ID when writing your own extension. 
[assembly: Autodesk.Connectivity.Extensibility.Framework.ExtensionId("08D2A77C-D7D8-4837-AA7A-E22869474664")]

// This number gets incremented for each Vault release.
// *ComponentUpgradeEveryRelease-Client*
[assembly: Autodesk.Connectivity.Extensibility.Framework.ApiVersion("19.0")]


namespace VaultCloudLinkExtension
{

    /// <summary>
    /// This class implements the IExtension interface, which means it tells Vault Explorer what 
    /// commands and custom tabs are provided by this extension.
    /// </summary>
    public class VaultCloudLinkExtension : IExplorerExtension
    {
        // Capture the current theme on startup
        internal static string mCurrentTheme = "light";

        internal static Connection? conn { get; set; }

        internal static IApplication? mApplication { get; set; }

        internal static string? mCloudLinkUdpName { get; set; }

        internal static Settings? mSettings { get; set; }

        internal static PropDef[]? mFldrPropDefs = null;

        private static string? mUrl;
        private static string? mCurrentUrl;


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
                                                "Cloud Link", typeof(CloudViewControl));
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
            conn = application.Connection;
            mFldrPropDefs = conn.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FLDR");
        }

        /// <summary>
        /// This function is called after the user is logged out of the Vault Server.
        /// Part of the IExtension interface.
        /// </summary>
        /// <param name="application">Provides information about the running application.</param>
        public void OnLogOff(IApplication application)
        {
            conn = null;
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
            mApplication = application;

            mCurrentTheme = VDF.Forms.Library.CurrentTheme.ToString();
            VDF.Forms.Library.ThemeChanged += ThemeChanged;

            mSettings = new Settings();
            mSettings = Settings.Load();
            mCloudLinkUdpName = mSettings.CloudLinkProperty;
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
        void CloudViewPanel_SelectionChanged(object? sender, DockPanelSelectionChangedEventArgs e)
        {
            if (e.Context.SelectedObject != null && e.Context.SelectedObject.TypeId.EntityClassId == "FLDR")
            {
                try
                {
                    long FolderId = e.Context.SelectedObject.Id;
                    //check if URL navigation is limited to a specific categor only and the current folder matches it; iterate parents if not
                    ACW.Folder mFolder = conn.WebServiceManager.DocumentService.GetFolderById(FolderId);
                    if (mSettings?.VaultFolderCat != "*" && !(mFolder.Cat.CatName == mSettings?.VaultFolderCat))
                    {
                        if (mFolder.FullName != "$")
                        {
                            do
                            {
                                mFolder = conn.WebServiceManager.DocumentService.GetFolderById(mFolder.ParId);
                                if (mFolder.Cat.CatName == mSettings?.VaultFolderCat)
                                {
                                    FolderId = mFolder.Id;
                                    break;
                                }
                            } while (mFolder.FullName != "$");
                        }
                    }

                    //get the selected folder's property values
                    PropInst[]? mSourcePropInsts = conn?.WebServiceManager.PropertyService.GetPropertiesByEntityIds("FLDR", new long[] { FolderId });
                    string mPropDispName = mSettings.CloudLinkProperty;
                    long? mPropId = mFldrPropDefs?.Where(static n => n.DispName == mSettings.CloudLinkProperty).FirstOrDefault().Id;

                    //it might happen that the prop is not assigned to a folder
                    try
                    {
                        mUrl = (string)mSourcePropInsts.Where(n => n.PropDefId == mPropId).FirstOrDefault().Val;
                        
                        // the link might include markdown syntax, so we need to decode it
                        mUrl = System.Net.WebUtility.HtmlDecode(mUrl);

                        // Check if the URL contains markdown syntax and extract the actual URL
                        if (mUrl.StartsWith("[") && mUrl.Contains("](") && mUrl.EndsWith(")"))
                        {
                            int startIndex = mUrl.IndexOf("](") + 2;
                            int endIndex = mUrl.LastIndexOf(")");
                            mUrl = mUrl.Substring(startIndex, endIndex - startIndex);
                        }
                    }
                    catch (Exception)
                    {
                        mUrl = "about:blank";
                    }

                    if (mUrl == null || mUrl == "")
                    {
                        mUrl = "about:blank";
                    }

                }
                catch (Exception)
                {
                }

                if (mUrl != mCurrentUrl && mUrl != "about:blank")
                {
                    // If the URL is different, we need to navigate to the new URL.
                    try
                    {
                        // The event args has our custom panel object.  We need to cast it to our type.
                        CloudViewControl? CefControl = e.Context.UserControl as CloudViewControl;

                        // navigate URL, it might be blank as evaluated before

                        // Send selection to the panel so that it can display the object.
                        CefControl?.NavigateToUrlAsync(mUrl);
                        mCurrentUrl = mUrl;
                    }
                    catch (Exception ex)
                    {
                        // If something goes wrong, we don't want the exception to bubble up to Vault Explorer.
                    }
                }
            }
        }


        /// <summary>
        /// Update the extension's theme if the host's changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemeChanged(object? sender, VDF.Forms.Library.UITheme e)
        {
            mCurrentTheme = VDF.Forms.Library.CurrentTheme.ToString().ToLower();
        }

    }
}
