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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using VDF = Autodesk.DataManagement.Client.Framework;

namespace HelloWorld
{
    public partial class MyCustomTabControl : UserControl
    {
        private Color m_defaultColumnHeaderBackColor;
        public MyCustomTabControl()
        {
            InitializeComponent();
            m_defaultColumnHeaderBackColor = mPropertyGrid.LineColor;

            SetTheme(VDF.Forms.Library.CurrentTheme);
            VDF.Forms.Library.ThemeChanged += ThemeChanged;
        }

        public void SetSelectedObject( object o ) 
        {
            mPropertyGrid.SelectedObject = o;
        }

        private void ThemeChanged(object sender, VDF.Forms.Library.UITheme theme)
        {
            SetTheme(theme);
        }

        private void SetTheme(VDF.Forms.Library.UITheme theme)
        {
            // The control is already responding to theme change,
            // but only property grid's column header is not.
            // Adjust its appearence explicitly so it looks better in Dark theme.
            switch (theme)
            {
                case VDF.Forms.Library.UITheme.Dark:
                    mPropertyGrid.CategoryForeColor = Color.White;
                    mPropertyGrid.LineColor = Color.CadetBlue;
                    break;
                case VDF.Forms.Library.UITheme.Light:
                case VDF.Forms.Library.UITheme.Classic:
                    mPropertyGrid.CategoryForeColor = Color.Black;
                    mPropertyGrid.LineColor = m_defaultColumnHeaderBackColor;
                    break;
            }
        }
    }
}
