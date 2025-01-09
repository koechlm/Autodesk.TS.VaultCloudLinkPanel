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
[assembly: AssemblyCompany("Autodesk")]
[assembly: AssemblyProduct("HelloWorldCommandExtension")]
[assembly: AssemblyDescription("Sample App")]

// The extension ID needs to be unique for each extension.  
// Make sure to generate your own ID when writing your own extension. 
[assembly: Autodesk.Connectivity.Extensibility.Framework.ExtensionId("7ADC0766-F085-46d7-A2EB-C68F79CBF4E7")]

// This number gets incremented for each Vault release.
// *ComponentUpgradeEveryRelease-Client*
[assembly: Autodesk.Connectivity.Extensibility.Framework.ApiVersion("19.0")]


namespace HelloWorld
{

    /// <summary>
    /// This class implements the IExtension interface, which means it tells Vault Explorer what 
    /// commands and custom tabs are provided by this extension.
    /// </summary>
    public class HelloWorldCommandExtension : IExplorerExtension
    {

        #region IExtension Members

        /// <summary>
        /// This function tells Vault Explorer what custom commands this extension provides.
        /// Part of the IExtension interface.
        /// </summary>
        /// <returns>A collection of CommandSites, which are collections of custom commands.</returns>
        public IEnumerable<CommandSite> CommandSites()
        {
            // Create the Hello World command object.
            CommandItem helloWorldCmdItem = new CommandItem("HelloWorldCommand", "Hello World...") 
            { 
                // this command is active when a File is selected
                NavigationTypes = new SelectionTypeId[] { SelectionTypeId.File, SelectionTypeId.FileVersion }, 

                // this command is not active if there are multiple entities selected
                MultiSelectEnabled = false 
            };

            // The HelloWorldCommandHandler function is called when the custom command is executed.
            helloWorldCmdItem.Execute += HelloWorldCommandHandler;

            // Create the Go To First Parent command object.
            CommandItem GoToFirstParentCmdItem = new CommandItem("GoToFirstParentCommand", "Go To First Parent...")
            {
                // this command is active when a File is selected
                NavigationTypes = new SelectionTypeId[] { SelectionTypeId.File, SelectionTypeId.FileVersion },

                // this command is not active if there are multiple entities selected
                MultiSelectEnabled = false
            };

            // The GoToFirstParentCommandHandler function is called when the custom command is executed.
            GoToFirstParentCmdItem.Execute += GoToFirstParentCommandHandler;

            // Create the Go To First Item command object.
            CommandItem GoToFirstItemCmdItem = new CommandItem("GoToFirstItemCommand", "Go To First Item...")
            {
                // this command is active when a File is selected
                NavigationTypes = new SelectionTypeId[] { SelectionTypeId.File, SelectionTypeId.FileVersion },

                // this command is not active if there are multiple entities selected
                MultiSelectEnabled = false
            };

            // The GoToFirstItemCommandHandler function is called when the custom command is executed.
            GoToFirstItemCmdItem.Execute += GoToFirstItemCommandHandler;

            // Create the Go To First ChangeOrder command object.
            CommandItem GoToFirstChangeOrderCmdItem = new CommandItem("GoToFirstChangeOrderCommand", "Go To First ChangeOrder...")
            {
                // this command is active when a File is selected
                NavigationTypes = new SelectionTypeId[] { SelectionTypeId.File, SelectionTypeId.FileVersion },

                // this command is not active if there are multiple entities selected
                MultiSelectEnabled = false
            };

            // The GoToFirstChangeOrderCommandHandler function is called when the custom command is executed.
            GoToFirstChangeOrderCmdItem.Execute += GoToFirstChangeOrderCommandHandler;

            // Create a command site to hook the command to the Advanced toolbar
            CommandSite toolbarCmdSite = new CommandSite("HelloWorldCommand.Toolbar", "Hello World Menu") 
            { 
                Location = CommandSiteLocation.AdvancedToolbar, 
                DeployAsPulldownMenu = false 
            };
            toolbarCmdSite.AddCommand(helloWorldCmdItem);
            toolbarCmdSite.AddCommand(GoToFirstParentCmdItem);
            toolbarCmdSite.AddCommand(GoToFirstItemCmdItem);
            toolbarCmdSite.AddCommand(GoToFirstChangeOrderCmdItem);

            // Create another command site to hook the command to the right-click menu for Files.
            CommandSite fileContextCmdSite = new CommandSite("HelloWorldCommand.FileContextMenu", "Hello World Menu") 
            { 
                Location = CommandSiteLocation.FileContextMenu, 
                DeployAsPulldownMenu = false 
            };
            fileContextCmdSite.AddCommand(helloWorldCmdItem);
            fileContextCmdSite.AddCommand(GoToFirstParentCmdItem);
            fileContextCmdSite.AddCommand(GoToFirstItemCmdItem);
            fileContextCmdSite.AddCommand(GoToFirstChangeOrderCmdItem);

            // Now the custom command is available in 2 places.

            // Gather the sites in a List.
            List<CommandSite> sites = new List<CommandSite>();
            sites.Add(toolbarCmdSite);
            sites.Add(fileContextCmdSite);

            // Return the list of CommandSites.
            return sites;
        }


        /// <summary>
        /// This function tells Vault Explorer what custom tabs this extension provides.
        /// Part of the IExtension interface.
        /// </summary>
        /// <returns>A collection of DetailTabs, each object represents a custom tab.</returns>
        public IEnumerable<DetailPaneTab> DetailTabs()
        {
            // Create a DetailPaneTab list to return from method
            List<DetailPaneTab> mTabs = new List<DetailPaneTab>();

            // Create Selection Info tab for Files
            DetailPaneTab filePropertyTab = new DetailPaneTab("File.Tab.PropertyGrid",
                                                        "Selection Info",
                                                        SelectionTypeId.File,
                                                        typeof(MyCustomTabControl));

            // The propertyTab_SelectionChanged is called whenever our tab is active and the selection changes in the 
            // main grid.
            filePropertyTab.SelectionChanged += propertyTab_SelectionChanged;
            mTabs.Add(filePropertyTab);

            // Create Selection Info tab for Folders
            DetailPaneTab folderPropertyTab = new DetailPaneTab("Folder.Tab.PropertyGrid",
                                                        "Selection Info",
                                                        SelectionTypeId.Folder,
                                                        typeof(MyCustomTabControl));
            folderPropertyTab.SelectionChanged += propertyTab_SelectionChanged;
            mTabs.Add(folderPropertyTab);

            // Create Selection Info tab for Items
            DetailPaneTab itemPropertyTab = new DetailPaneTab("Item.Tab.PropertyGrid",
                                                        "Selection Info",
                                                        SelectionTypeId.Item,
                                                        typeof(MyCustomTabControl));
            itemPropertyTab.SelectionChanged += propertyTab_SelectionChanged;
            mTabs.Add(itemPropertyTab);

            // Create Selection Info tab for Change Orders
            DetailPaneTab coPropertyTab = new DetailPaneTab("Co.Tab.PropertyGrid",
                                                        "Selection Info",
                                                        SelectionTypeId.ChangeOrder,
                                                        typeof(MyCustomTabControl));    
            coPropertyTab.SelectionChanged += propertyTab_SelectionChanged;
            mTabs.Add(coPropertyTab);


            // Create a WebView Browser tab for Files
            DetailPaneTab fileBrowserTab = new DetailPaneTab("File.Tab.Browser",
                                                        "Browser",
                                                        SelectionTypeId.File,
                                                        typeof(BrowserControl));
            fileBrowserTab.SelectionChanged +=browserTab_SelectionChanged;
            mTabs.Add(fileBrowserTab);


            // Return tabs
            return mTabs;
        }

        private void browserTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // The event args has our custom tab object.  We need to cast it to our type.
                BrowserControl tabControl = e.Context.UserControl as BrowserControl;
                // Send selection to the tab so that it can display the object.
                tabControl.mNavigate("https://www.google.com");
            }
            catch (Exception ex)
            {
                // If something goes wrong, we don't want the exception to bubble up to Vault Explorer.
                MessageBox.Show("Error: " + ex.Message);
            }
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
            DockPanel dockPanel = new DockPanel(Guid.Parse("5fa18611-43f7-4b60-933b-b3f7861e1c6e"),
                                                "HelloWorldDockPanel", typeof(MyCustomTabControl));
            dockPanels.Add(dockPanel);

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
        public IEnumerable<string> HiddenCommands()
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
        public IEnumerable<CustomEntityHandler> CustomEntityHandlers()
        {
            // This extension does not provide special Custom Entity behavior.
            return null;
        }

#endregion


        /// <summary>
        /// This is the function that is called whenever the custom command is executed.
        /// </summary>
        /// <param name="s">The sender object.  Usually not used.</param>
        /// <param name="e">The event args.  Provides additional information about the environment.</param>
        void HelloWorldCommandHandler(object s, CommandItemEventArgs e)
        {
            try
            {
                VDF.Vault.Currency.Connections.Connection connection = e.Context.Application.Connection;

                // The Context part of the event args tells us information about what is selected.
                // Run some checks to make sure that the selection is valid.
                if (e.Context.CurrentSelectionSet.Count() == 0)
                    MessageBox.Show("Nothing is selected");
                else if (e.Context.CurrentSelectionSet.Count() > 1)
                    MessageBox.Show("This function does not support multiple selections");
                else
                {
                    // we only have one item selected, which is the expected behavior

                    ISelection selection = e.Context.CurrentSelectionSet.First();

                    // Look of the File object.  How we do this depends on what is selected.
                    File selectedFile = null;
                    if (selection.TypeId == SelectionTypeId.File)
                    {
                        // our ISelection.Id is really a File.MasterId
                        selectedFile = connection.WebServiceManager.DocumentService.GetLatestFileByMasterId(selection.Id);
                    }
                    else if (selection.TypeId == SelectionTypeId.FileVersion)
                    {
                        // our ISelection.Id is really a File.Id
                        selectedFile = connection.WebServiceManager.DocumentService.GetFileById(selection.Id);
                    }

                    if (selectedFile == null)
                    {
                        MessageBox.Show("Selection is not a file.");
                    }
                    else
                    {
                        // this is the message we hope to see
                        MessageBox.Show(String.Format("Hello World! The file size is: {0} bytes",
                                             selectedFile.FileSize));
                    }
                }
            }
            catch (Exception ex)
            {
                // If something goes wrong, we don't want the exception to bubble up to Vault Explorer.
                MessageBox.Show("Error: " + ex.Message);
            }
        }
       

        void GoToFirstParentCommandHandler(object s, CommandItemEventArgs e)
        {
            try
            {
                VDF.Vault.Currency.Connections.Connection connection = e.Context.Application.Connection;

                // The Context part of the event args tells us information about what is selected.
                // Run some checks to make sure that the selection is valid.
                if (e.Context.CurrentSelectionSet.Count() == 0)
                    MessageBox.Show("Nothing is selected");
                else if (e.Context.CurrentSelectionSet.Count() > 1)
                    MessageBox.Show("This function does not support multiple selections");
                else
                {
                    // we only have one item selected, which is the expected behavior

                    ISelection selection = e.Context.CurrentSelectionSet.First();

                    // Look of the File object.  How we do this depends on what is selected.
                    FileAssoc assoc = null;
                    File parentFile = null;
                    if (selection.TypeId == SelectionTypeId.File)
                    {
                        // our ISelection.Id is really a File.MasterId
                        var selectedFile = connection.WebServiceManager.DocumentService.GetLatestFileByMasterId(selection.Id);
                        if(selectedFile != null)
                        {
                            var associations = connection.WebServiceManager.DocumentService.GetFileAssociationsByIds(new long[] { selectedFile.Id }, FileAssociationTypeEnum.Dependency, false, FileAssociationTypeEnum.None, false, false, false);
                            assoc = associations?.FirstOrDefault()?.FileAssocs?.FirstOrDefault();
                        }
                    }
                    else if (selection.TypeId == SelectionTypeId.FileVersion)
                    {
                        // our ISelection.Id is really a File.Id
                        var associations = connection.WebServiceManager.DocumentService.GetFileAssociationsByIds(new long[] { selection.Id }, FileAssociationTypeEnum.Dependency, false, FileAssociationTypeEnum.None, false, false, false);
                        assoc = associations?.FirstOrDefault()?.FileAssocs?.FirstOrDefault();
                    }
                    if(assoc != null )
                    {
                        parentFile = connection.WebServiceManager.DocumentService.GetFileById(assoc.ParFile.Id);
                    }
                    if(assoc == null || parentFile == null)
                    {
                        MessageBox.Show("Can not find parent file.");
                    }
                    else
                    {
                        var explorerUtil = Autodesk.Connectivity.Explorer.ExtensibilityTools.ExplorerLoader.GetExplorerUtil(e.Context.Application);
                        explorerUtil.GoToEntity(new VDFV.Currency.Entities.FileIteration(connection, parentFile));
                    }
                }
            }
            catch (Exception ex)
            {
                // If something goes wrong, we don't want the exception to bubble up to Vault Explorer.
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        void GoToFirstItemCommandHandler(object s, CommandItemEventArgs e)
        {
            try
            {
                VDF.Vault.Currency.Connections.Connection connection = e.Context.Application.Connection;

                // The Context part of the event args tells us information about what is selected.
                // Run some checks to make sure that the selection is valid.
                if (e.Context.CurrentSelectionSet.Count() == 0)
                    MessageBox.Show("Nothing is selected");
                else if (e.Context.CurrentSelectionSet.Count() > 1)
                    MessageBox.Show("This function does not support multiple selections");
                else
                {
                    // we only have one item selected, which is the expected behavior

                    ISelection selection = e.Context.CurrentSelectionSet.First();

                    // Look of the File object.  How we do this depends on what is selected.
                    Item item = null;
                    if (selection.TypeId == SelectionTypeId.File)
                    {
                        // our ISelection.Id is really a File.MasterId
                        var selectedFile = connection.WebServiceManager.DocumentService.GetLatestFileByMasterId(selection.Id);
                        if (selectedFile != null)
                        {
                            var items = connection.WebServiceManager.ItemService.GetItemsByFileId(selectedFile.Id);
                            item = items?.FirstOrDefault();
                        }
                    }
                    else if (selection.TypeId == SelectionTypeId.FileVersion)
                    {
                        // our ISelection.Id is really a File.Id
                        var items = connection.WebServiceManager.ItemService.GetItemsByFileId(selection.Id);
                        item = items?.FirstOrDefault();
                    }
                    if (item == null)
                    {
                        MessageBox.Show("Can not find linked item.");
                    }
                    else
                    {
                        var explorerUtil = Autodesk.Connectivity.Explorer.ExtensibilityTools.ExplorerLoader.GetExplorerUtil(e.Context.Application);
                        explorerUtil.GoToEntity(new VDFV.Currency.Entities.ItemRevision(connection, item));
                    }
                }
            }
            catch (Exception ex)
            {
                // If something goes wrong, we don't want the exception to bubble up to Vault Explorer.
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        void GoToFirstChangeOrderCommandHandler(object s, CommandItemEventArgs e)
        {
            try
            {
                VDF.Vault.Currency.Connections.Connection connection = e.Context.Application.Connection;

                // The Context part of the event args tells us information about what is selected.
                // Run some checks to make sure that the selection is valid.
                if (e.Context.CurrentSelectionSet.Count() == 0)
                    MessageBox.Show("Nothing is selected");
                else if (e.Context.CurrentSelectionSet.Count() > 1)
                    MessageBox.Show("This function does not support multiple selections");
                else
                {
                    // we only have one item selected, which is the expected behavior

                    ISelection selection = e.Context.CurrentSelectionSet.First();

                    // Look of the File object.  How we do this depends on what is selected.
                    ChangeOrder changeOrder = null;
                    if (selection.TypeId == SelectionTypeId.File)
                    {
                        // our ISelection.Id is really a File.MasterId
                        var coFiles = connection.WebServiceManager.ChangeOrderService.GetChangeOrderFilesByFileMasterId(selection.Id);
                        changeOrder = coFiles?.FirstOrDefault()?.ChangeOrder;
                    }
                    else if (selection.TypeId == SelectionTypeId.FileVersion)
                    {
                        // our ISelection.Id is really a File.Id
                        var selectedFile = connection.WebServiceManager.DocumentService.GetFileById(selection.Id);
                        if(selectedFile != null)
                        {
                            var coFiles = connection.WebServiceManager.ChangeOrderService.GetChangeOrderFilesByFileMasterId(selectedFile.MasterId);
                            changeOrder = coFiles?.FirstOrDefault()?.ChangeOrder;
                        }
                    }
                    if (changeOrder == null)
                    {
                        MessageBox.Show("Can not find linked ChangeOrder.");
                    }
                    else
                    {
                        var explorerUtil = Autodesk.Connectivity.Explorer.ExtensibilityTools.ExplorerLoader.GetExplorerUtil(e.Context.Application);
                        explorerUtil.GoToEntity(new VDFV.Currency.Entities.ChangeOrder(connection, changeOrder));
                    }
                }
            }
            catch (Exception ex)
            {
                // If something goes wrong, we don't want the exception to bubble up to Vault Explorer.
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// This function is called whenever our custom tab is active and the selection has changed in the main grid.
        /// </summary>
        /// <param name="sender">The sender object.  Usually not used.</param>
        /// <param name="e">The event args.  Provides additional information about the environment.</param>
        void propertyTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // The event args has our custom tab object.  We need to cast it to our type.
                MyCustomTabControl tabControl = e.Context.UserControl as MyCustomTabControl;

                // Send selection to the tab so that it can display the object.
                tabControl.SetSelectedObject(e.Context.SelectedObject);
            }
            catch (Exception ex)
            {
                // If something goes wrong, we don't want the exception to bubble up to Vault Explorer.
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
