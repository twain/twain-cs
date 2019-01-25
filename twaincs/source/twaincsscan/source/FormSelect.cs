///////////////////////////////////////////////////////////////////////////////////////
//
//  TWAINCSScan.FormSelect
//
//  This class helps us select a TWAIN driver that we wish to open.
//
///////////////////////////////////////////////////////////////////////////////////////
//  Author          Date            Version     Comment
//  M.McLaughlin    21-May-2014     2.0.0.0     64-bit Linux
//  M.McLaughlin    27-Feb-2014     1.1.0.0     ShowImage additions
//  M.McLaughlin    21-Oct-2013     1.0.0.0     Initial Release
///////////////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2013-2019 Kodak Alaris Inc.
//
//  Permission is hereby granted, free of charge, to any person obtaining a
//  copy of this software and associated documentation files (the "Software"),
//  to deal in the Software without restriction, including without limitation
//  the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//  DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TWAINWorkingGroupToolkit;

namespace TWAINCSScan
{
    public partial class FormSelect : Form
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Public Methods...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Methods...

        /// <summary>
        /// Our constructor...
        /// </summary>
        /// <param name="a_aszIdentity">list of scanners to show</param>
        /// <param name="a_szDefault">the default selection</param>
        public FormSelect(string[] a_aszIdentity, string a_szDefault)
        {
            string[] aszIdentity;
            string[] aszDefault;

            // Init stuff...
            InitializeComponent();

            // Explode the default...
            aszDefault = TWAINCSToolkit.CsvParse(a_szDefault);

            // Suspend updating...
            m_listboxSelect.BeginUpdate();

            // Populate our driver list...
            foreach (string sz in a_aszIdentity)
            {
                aszIdentity = TWAINCSToolkit.CsvParse(sz);
                m_listboxSelect.Items.Add(aszIdentity[11].ToString());
            }

            // Select the default...
            m_listboxSelect.SelectedIndex = m_listboxSelect.FindStringExact(aszDefault[11]);

            // Resume updating...
            m_listboxSelect.EndUpdate();
        }

        public string GetSelectedDriver()
        {
            return (m_szSelected);
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Methods...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Methods...

        /// <summary>
        /// Select this as our driver and close the dialog...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_buttonOpen_Click(object sender, EventArgs e)
        {
            m_szSelected = (string)m_listboxSelect.SelectedItem;
        }

        private void m_listboxSelect_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            m_szSelected = (string)m_listboxSelect.SelectedItem;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Attributes...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Attributes...

        /// <summary>
        /// The currently selected item...
        /// </summary>
        private string m_szSelected;

        #endregion
    }
}
