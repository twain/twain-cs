///////////////////////////////////////////////////////////////////////////////////////
//
// TWAINCSTst.FormMain
//
// This is the main form for a developerment application.  Don't use this code as
// the template for a new app.  Use TWAINCSSCAN instead, it's smaller and easier
// to understand.
//
// This application offers access to all of the standard and custom TWAIN content
// that one can find in a scanner vendor's TWAIN driver.  It's a diagnostic tool,
// that lets one exercise drivers.  It also showcases all of the ways of getting
// and setting driver data.  So while TWAINCSSCAN is the template for a new
// application, this is the reference guide for specific items that a developer
// may want to add to their code.
//
///////////////////////////////////////////////////////////////////////////////////////
//  Author          Date            TWAIN       Comment
//  M.McLaughlin    21-Oct-2013     2.3         Initial Release
///////////////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2013 Kodak Alaris Inc.
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TWAINWorkingGroupToolkit;

namespace TWAINCSTst
{
    /// <summary>
    /// Here's a form for us to tinker with.  We're using this to exercise the
    /// TWAIN object.  A TWAINCSToolkit class has been created to hide the
    /// details of TWAIN from the main application.  This abstraction creates
    /// additional code, but protects the main app from the details of the
    /// TWAIN interface.
    /// </summary>
    public partial class FormMain : Form, IMessageFilter
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Public Functions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Functions...

        /// <summary>
        /// Init our form, and our TWAIN class.  You might want to consider
        /// moving TWAIN into its own thread, or even running it from a
        /// separate process, so that the main application is always responsive
        /// no matter what's going on in the driver...
        /// </summary>
        public FormMain()
        {
            ContextMenu contextmenu;
            MenuItem menuitem;

            // Init our form...
            InitializeComponent();

            // Make sure we cleanup if unexpectedly closed...
            this.FormClosing += new FormClosingEventHandler(FormMain_FormClosing);

            // This next bit establishes the rules for the various DSM's on the
            // various operating systems.

            // Windows controls...
            if (TWAINCSToolkit.GetPlatform() == "WINDOWS")
            {
                m_checkboxUseTwain32.Enabled = (TWAINCSToolkit.GetMachineWordBitSize() == 32);
                m_checkboxUseCallbacks.Enabled = true;
                m_checkboxUseTwain32.Checked = false;
                m_checkboxUseCallbacks.Checked = true;
            }

            // Linux controls...
            else if (TWAINCSToolkit.GetPlatform() == "LINUX")
            {
                m_checkboxUseTwain32.Checked = false;
                m_checkboxUseCallbacks.Checked = true;
                m_checkboxUseTwain32.Enabled = false;
                m_checkboxUseCallbacks.Enabled = false;
            }

            // Mac OS X controls...
            else if (TWAINCSToolkit.GetPlatform() == "MACOSX")
            {
                m_checkboxUseTwain32.Checked = false;
                m_checkboxUseCallbacks.Checked = true;
                m_checkboxUseTwain32.Enabled = false;
                m_checkboxUseCallbacks.Enabled = false;
            }

            // Autoscroll the text in our output box...
            m_richtextboxOutput.HideSelection = false;
            m_richtextboxOutput.SelectionProtected = false;

            // Init other stuff...
            m_twaincstoolkit = null;

            // Init our image controls...
            InitImage();

            // Load our triplet dropdown...
            this.m_comboboxDG.Items.AddRange(TWAINCSToolkit.GetTwainDg());
            this.m_comboboxDAT.Items.AddRange(TWAINCSToolkit.GetTwainDat());
            this.m_comboboxMSG.Items.AddRange(TWAINCSToolkit.GetTwainMsg());

            // Init our triplet dropdown...
            AutoDropdown("", "", "");

            // Context menu for our value box...
            contextmenu = new ContextMenu();
            menuitem = new MenuItem("Copy");
            menuitem.Click += new EventHandler(richtextboxcapability_Copy);
            contextmenu.MenuItems.Add(menuitem);
            menuitem = new MenuItem("Paste");
            menuitem.Click += new EventHandler(richtextboxcapability_Paste);
            contextmenu.MenuItems.Add(menuitem);
            m_richtextboxCapability.ContextMenu = contextmenu;
            contextmenu = null;

            // Context menu for our output box...
            contextmenu = new ContextMenu();
            menuitem = new MenuItem("Copy");
            menuitem.Click += new EventHandler(richtextboxoutput_Copy);
            contextmenu.MenuItems.Add(menuitem);
            menuitem = new MenuItem("Paste");
            menuitem.Click += new EventHandler(richtextboxoutput_Paste);
            contextmenu.MenuItems.Add(menuitem);
            m_richtextboxOutput.ContextMenu = contextmenu;
            contextmenu = null;
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Functions, miscellaneous functions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Functions...

        /// <summary>
        /// Make sure we clean up on a surprise close...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_blClosing = true;
            if (m_twaincstoolkit != null)
            {
                m_twaincstoolkit.Cleanup();
                m_twaincstoolkit = null;
            }
            CleanImage();
        }

        /// <summary>
        /// Call the form's filter function to catch stuff like MSG.XFERREADY...
        /// </summary>
        /// <param name="a_message">Message to process</param>
        /// <returns>Result of the processing</returns>
        public bool PreFilterMessage(ref Message a_message)
        {
            if (m_twaincstoolkit != null)
            {
                return
                (
                    m_twaincstoolkit.PreFilterMessage
                    (
                        a_message.HWnd,
                        a_message.Msg,
                        a_message.WParam,
                        a_message.LParam
                    )
                );
            }
            return (true);
        }

        /// <summary>
        /// Copy data from a richtextbox...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void richtextboxcapability_Copy(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, m_richtextboxCapability.SelectedText);
        }
        private void richtextboxoutput_Copy(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, m_richtextboxOutput.SelectedText);
        }

        /// <summary>
        /// Paste data to a richtextbox...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void richtextboxcapability_Paste(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                m_richtextboxCapability.SelectedText = Clipboard.GetData(DataFormats.Text).ToString();
            }
        }
        private void richtextboxoutput_Paste(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                m_richtextboxOutput.SelectedText = Clipboard.GetData(DataFormats.Text).ToString();
            }
        }

        /// <summary>
        /// Initialize the picture boxes and the graphics to support them, we're
        /// doing this to maximize performance during scanner...
        /// </summary>
        private void InitImage()
        {
            // Make sure our picture boxes don't do much work...
            m_pictureboxImage1.SizeMode = PictureBoxSizeMode.Normal;
            m_pictureboxImage2.SizeMode = PictureBoxSizeMode.Normal;
            m_pictureboxImage3.SizeMode = PictureBoxSizeMode.Normal;
            m_pictureboxImage4.SizeMode = PictureBoxSizeMode.Normal;
            m_pictureboxImage5.SizeMode = PictureBoxSizeMode.Normal;
            m_pictureboxImage6.SizeMode = PictureBoxSizeMode.Normal;
            m_pictureboxImage7.SizeMode = PictureBoxSizeMode.Normal;
            m_pictureboxImage8.SizeMode = PictureBoxSizeMode.Normal;

            m_bitmapGraphic1 = new Bitmap(m_pictureboxImage1.Width, m_pictureboxImage1.Height, PixelFormat.Format32bppPArgb);
            m_graphics1 = Graphics.FromImage(m_bitmapGraphic1);
            m_graphics1.CompositingMode = CompositingMode.SourceCopy;
            m_graphics1.CompositingQuality = CompositingQuality.HighSpeed;
            m_graphics1.InterpolationMode = InterpolationMode.Low;
            m_graphics1.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            m_graphics1.SmoothingMode = SmoothingMode.HighSpeed;

            m_bitmapGraphic2 = new Bitmap(m_pictureboxImage2.Width, m_pictureboxImage2.Height, PixelFormat.Format32bppPArgb);
            m_graphics2 = Graphics.FromImage(m_bitmapGraphic2);
            m_graphics2.CompositingMode = CompositingMode.SourceCopy;
            m_graphics2.CompositingQuality = CompositingQuality.HighSpeed;
            m_graphics2.InterpolationMode = InterpolationMode.Low;
            m_graphics2.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            m_graphics2.SmoothingMode = SmoothingMode.HighSpeed;

            m_bitmapGraphic3 = new Bitmap(m_pictureboxImage3.Width, m_pictureboxImage3.Height, PixelFormat.Format32bppPArgb);
            m_graphics3 = Graphics.FromImage(m_bitmapGraphic3);
            m_graphics3.CompositingMode = CompositingMode.SourceCopy;
            m_graphics3.CompositingQuality = CompositingQuality.HighSpeed;
            m_graphics3.InterpolationMode = InterpolationMode.Low;
            m_graphics3.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            m_graphics3.SmoothingMode = SmoothingMode.HighSpeed;

            m_bitmapGraphic4 = new Bitmap(m_pictureboxImage4.Width, m_pictureboxImage4.Height, PixelFormat.Format32bppPArgb);
            m_graphics4 = Graphics.FromImage(m_bitmapGraphic4);
            m_graphics4.CompositingMode = CompositingMode.SourceCopy;
            m_graphics4.CompositingQuality = CompositingQuality.HighSpeed;
            m_graphics4.InterpolationMode = InterpolationMode.Low;
            m_graphics4.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            m_graphics4.SmoothingMode = SmoothingMode.HighSpeed;

            m_bitmapGraphic5 = new Bitmap(m_pictureboxImage5.Width, m_pictureboxImage5.Height, PixelFormat.Format32bppPArgb);
            m_graphics5 = Graphics.FromImage(m_bitmapGraphic5);
            m_graphics5.CompositingMode = CompositingMode.SourceCopy;
            m_graphics5.CompositingQuality = CompositingQuality.HighSpeed;
            m_graphics5.InterpolationMode = InterpolationMode.Low;
            m_graphics5.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            m_graphics5.SmoothingMode = SmoothingMode.HighSpeed;

            m_bitmapGraphic6 = new Bitmap(m_pictureboxImage6.Width, m_pictureboxImage6.Height, PixelFormat.Format32bppPArgb);
            m_graphics6 = Graphics.FromImage(m_bitmapGraphic6);
            m_graphics6.CompositingMode = CompositingMode.SourceCopy;
            m_graphics6.CompositingQuality = CompositingQuality.HighSpeed;
            m_graphics6.InterpolationMode = InterpolationMode.Low;
            m_graphics6.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            m_graphics6.SmoothingMode = SmoothingMode.HighSpeed;

            m_bitmapGraphic7 = new Bitmap(m_pictureboxImage7.Width, m_pictureboxImage7.Height, PixelFormat.Format32bppPArgb);
            m_graphics7 = Graphics.FromImage(m_bitmapGraphic7);
            m_graphics7.CompositingMode = CompositingMode.SourceCopy;
            m_graphics7.CompositingQuality = CompositingQuality.HighSpeed;
            m_graphics7.InterpolationMode = InterpolationMode.Low;
            m_graphics7.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            m_graphics7.SmoothingMode = SmoothingMode.HighSpeed;

            m_bitmapGraphic8 = new Bitmap(m_pictureboxImage8.Width, m_pictureboxImage8.Height, PixelFormat.Format32bppPArgb);
            m_graphics8 = Graphics.FromImage(m_bitmapGraphic8);
            m_graphics8.CompositingMode = CompositingMode.SourceCopy;
            m_graphics8.CompositingQuality = CompositingQuality.HighSpeed;
            m_graphics8.InterpolationMode = InterpolationMode.Low;
            m_graphics8.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            m_graphics8.SmoothingMode = SmoothingMode.HighSpeed;

            m_brushBackground = new SolidBrush(Color.White);
            m_rectangleBackground = new Rectangle(0, 0, m_bitmapGraphic1.Width, m_bitmapGraphic1.Height);
        }

        /// <summary>
        /// Free resources...
        /// </summary>
        private void CleanImage()
        {
            // This is weird, but it works, make sure we don't have any
            // garbage laying around before we cleanup and leave.  This
            // avoids nasty "Invalid window device" messages on Mac...
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            if (!m_pictureboxImage1.IsDisposed)
            {
                m_pictureboxImage1.Dispose();
            }
            if (m_graphics1 != null)
            {
                m_graphics1.Dispose();
                m_graphics1 = null;
            }
            if (m_bitmapGraphic1 != null)
            {
                m_bitmapGraphic1.Dispose();
                m_bitmapGraphic1 = null;
            }

            if (!m_pictureboxImage2.IsDisposed)
            {
                m_pictureboxImage2.Dispose();
            }
            if (m_graphics2 != null)
            {
                m_graphics2.Dispose();
                m_graphics2 = null;
            }
            if (m_bitmapGraphic2 != null)
            {
                m_bitmapGraphic2.Dispose();
                m_bitmapGraphic2 = null;
            }

            if (!m_pictureboxImage3.IsDisposed)
            {
                m_pictureboxImage3.Dispose();
            }
            if (m_graphics3 != null)
            {
                m_graphics3.Dispose();
                m_graphics3 = null;
            }
            if (m_bitmapGraphic3 != null)
            {
                m_bitmapGraphic3.Dispose();
                m_bitmapGraphic3 = null;
            }

            if (!m_pictureboxImage4.IsDisposed)
            {
                m_pictureboxImage4.Dispose();
            }
            if (m_graphics4 != null)
            {
                m_graphics4.Dispose();
                m_graphics4 = null;
            }
            if (m_bitmapGraphic4 != null)
            {
                m_bitmapGraphic4.Dispose();
                m_bitmapGraphic4 = null;
            }

            if (!m_pictureboxImage5.IsDisposed)
            {
                m_pictureboxImage5.Dispose();
            }
            if (m_graphics5 != null)
            {
                m_graphics5.Dispose();
                m_graphics5 = null;
            }
            if (m_bitmapGraphic5 != null)
            {
                m_bitmapGraphic5.Dispose();
                m_bitmapGraphic5 = null;
            }

            if (!m_pictureboxImage6.IsDisposed)
            {
                m_pictureboxImage6.Dispose();
            }
            if (m_graphics6 != null)
            {
                m_graphics6.Dispose();
                m_graphics6 = null;
            }
            if (m_bitmapGraphic6 != null)
            {
                m_bitmapGraphic6.Dispose();
                m_bitmapGraphic6 = null;
            }

            if (!m_pictureboxImage7.IsDisposed)
            {
                m_pictureboxImage7.Dispose();
            }
            if (m_graphics7 != null)
            {
                m_graphics7.Dispose();
                m_graphics7 = null;
            }
            if (m_bitmapGraphic7 != null)
            {
                m_bitmapGraphic7.Dispose();
                m_bitmapGraphic7 = null;
            }

            if (!m_pictureboxImage8.IsDisposed)
            {
                m_pictureboxImage8.Dispose();
            }
            if (m_graphics8 != null)
            {
                m_graphics8.Dispose();
                m_graphics8 = null;
            }
            if (m_bitmapGraphic8 != null)
            {
                m_bitmapGraphic8.Dispose();
                m_bitmapGraphic8 = null;
            }
        }

        /// <summary>
        /// Load an image into a picture box, maintain its aspect ratio...
        /// </summary>
        /// <param name="a_picturebox">The picture box we're drawing to</param>
        /// <param name="a_graphics">The graphics backing the picture box</param>
        /// <param name="a_bitmapGraphic">Characteristics of the target</param>
        /// <param name="a_bitmap">The source image</param>
        private void LoadImage(ref PictureBox a_picturebox, ref Graphics a_graphics, ref Bitmap a_bitmapGraphic, Bitmap a_bitmap)
        {
            // We want to maintain the aspect ratio...
            double fRatioWidth = (double)a_bitmapGraphic.Size.Width / (double)a_bitmap.Width;
            double fRatioHeight = (double)a_bitmapGraphic.Size.Height / (double)a_bitmap.Height;
            double fRatio = (fRatioWidth < fRatioHeight) ? fRatioWidth : fRatioHeight;
            int iWidth = (int)(a_bitmap.Width * fRatio);
            int iHeight = (int)(a_bitmap.Height * fRatio);

            // Display the image...
            a_graphics.FillRectangle(m_brushBackground, m_rectangleBackground);
            a_graphics.DrawImage(a_bitmap, new Rectangle(((int)a_bitmapGraphic.Width - iWidth)/2, ((int)a_bitmapGraphic.Height - iHeight)/2, iWidth, iHeight));
            a_picturebox.Image = a_bitmapGraphic;
            a_picturebox.Update();
        }

        /// <summary>
        /// Turn message filtering on or off...
        /// </summary>
        /// <param name="a_blAdd">True to turn it on</param>
        private void SetMessageFilter(bool a_blAdd)
        {
            if (a_blAdd)
            {
                Application.AddMessageFilter(this);
            }
            else
            {
                Application.RemoveMessageFilter(this);
            }
        }

        /// <summary>
        /// Show an image...
        /// </summary>
        /// <param name="a_bitmap">The bitmap we want to show</param>
        private void ShowImage(Bitmap a_bitmap)
        {
            // We're leaving...
            if (m_blClosing || (m_graphics1 == null) || (a_bitmap == null))
            {
                return;
            }

            // Let us be called from any thread...
            if (this.InvokeRequired)
            {
                // We need a copy of the bitmap, because we're not going to wait
                // for the thread to return.  Be careful when using EndInvoke.
                // It's possible to create a deadlock situation with the Stop
                // button press.  A much better solution would be to 
                BeginInvoke(new MethodInvoker(delegate() { ShowImage(new Bitmap(a_bitmap)); }));
                return;
            }

            // Display the image...
            switch (m_iCurrentPictureBox)
            {
                default:
                case 0:
                    LoadImage(ref m_pictureboxImage1, ref m_graphics1, ref m_bitmapGraphic1, a_bitmap);
                    break;
                case 1:
                    LoadImage(ref m_pictureboxImage2, ref m_graphics2, ref m_bitmapGraphic2, a_bitmap);
                    break;
                case 2:
                    LoadImage(ref m_pictureboxImage3, ref m_graphics3, ref m_bitmapGraphic3, a_bitmap);
                    break;
                case 3:
                    LoadImage(ref m_pictureboxImage4, ref m_graphics4, ref m_bitmapGraphic4, a_bitmap);
                    break;
                case 4:
                    LoadImage(ref m_pictureboxImage5, ref m_graphics5, ref m_bitmapGraphic5, a_bitmap);
                    break;
                case 5:
                    LoadImage(ref m_pictureboxImage6, ref m_graphics6, ref m_bitmapGraphic6, a_bitmap);
                    break;
                case 6:
                    LoadImage(ref m_pictureboxImage7, ref m_graphics7, ref m_bitmapGraphic7, a_bitmap);
                    break;
                case 7:
                    LoadImage(ref m_pictureboxImage8, ref m_graphics8, ref m_bitmapGraphic8, a_bitmap);
                    break;
            }

            // Next picture box...
            if (++m_iCurrentPictureBox >= 8)
            {
                m_iCurrentPictureBox = 0;
            }
        }

        /// <summary>
        /// Debugging output that we can monitor...
        /// </summary>
        /// <param name="a_szOutput">Text to display to the user</param>
        private void WriteOutput(string a_szOutput)
        {
            // We're leaving...
            if (m_blClosing)
            {
                return;
            }

            // Let us be called from any thread.  We don't care if a_szOutput changes
            // on the fly (it's incredibly unlikely that it will), so we're not going
            // to wait for the thread to finish...
            if (this.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { WriteOutput(a_szOutput); }));
                return;
            }

            // Display the text...
            m_richtextboxOutput.AppendText(a_szOutput);
            m_richtextboxOutput.SelectionStart = m_richtextboxOutput.Text.Length;
            m_richtextboxOutput.Refresh();
        }

        /// <summary>
        /// Write a triplet message...
        /// </summary>
        /// <param name="a_szDg">Data group</param>
        /// <param name="a_szDat">Data argument type</param>
        /// <param name="a_szMsg">Operation</param>
        /// <param name="a_szOutput">Text</param>
        private void WriteTriplet(string a_szDg, string a_szDat, string a_szMsg, string a_szOutput)
        {
            string szDg;
            string szDat;
            string szMsg;

            // We're leaving...
            if (m_blClosing)
            {
                return;
            }

            // Let us be called from any thread.  We don't care if a_szOutput changes
            // on the fly (it's incredibly unlikely that it will), so we're not going
            // to wait for the thread to finish...
            if (this.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { WriteTriplet(a_szDg, a_szDat, a_szMsg, a_szOutput); }));
                return;
            }

            // Build the triplet...
            szDg = a_szDg.StartsWith("DG_") ? a_szDg : "DG_" + a_szDg;
            szDat = a_szDat.StartsWith("DAT_") ? a_szDat : "DAT_" + a_szDat;
            szMsg = a_szMsg.StartsWith("MSG_") ? a_szMsg : "MSG_" + a_szMsg;

            // Display the triplet...
            m_richtextboxOutput.SelectionStart = m_richtextboxOutput.Text.Length;
            m_richtextboxOutput.SelectionColor = Color.Blue;
            m_richtextboxOutput.AppendText(Environment.NewLine + a_szDg + "/" + a_szDat + "/" + a_szMsg + ": ");
            m_richtextboxOutput.SelectionStart = m_richtextboxOutput.Text.Length;

            // Display the text...
            m_richtextboxOutput.SelectionStart = m_richtextboxOutput.Text.Length;
            m_richtextboxOutput.SelectionColor = Color.Black;
            m_richtextboxOutput.AppendText(a_szOutput + Environment.NewLine);
            m_richtextboxOutput.SelectionStart = m_richtextboxOutput.Text.Length;
            m_richtextboxOutput.Refresh();
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Send Triplets, the Send button is used to issue TWAIN triplets
        // to the Data Source Manager, this section covers all the supported
        // combinations...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Send Triplets...

        /// <summary>
        /// Automatically change our dropdown values to reduce user frustration,
        /// we do this to save a step when it's pretty obvious what the user
        /// will typically want to do next.  For instance, if they've just opened
        /// the DSM they probably want to start enumerating TWAIN drivers...
        /// </summary>
        /// <param name="a_szDg">Data group</param>
        /// <param name="a_szDat">Data Argument type</param>
        /// <param name="a_szMsg">Operation</param>
        void AutoDropdown(string a_szDg, string a_szDat, string a_szMsg)
        {
            // We're initializing...
            if ((a_szDg == "") && (a_szDat == "") && (a_szMsg == ""))
            {
                m_comboboxDG.SelectedItem = "DG_CONTROL";
                m_comboboxDAT.SelectedItem = "DAT_PARENT";
                m_comboboxMSG.SelectedItem = "MSG_OPENDSM";
            }

            // After OPENDSM we have a choice, based on the DSM that was used...
            else if ((a_szDg == "DG_CONTROL") && (a_szDat == "DAT_PARENT") && (a_szMsg == "MSG_OPENDSM"))
            {
                if (m_twaincstoolkit.IsDsm2())
                {
                    m_comboboxDG.SelectedItem = "DG_CONTROL";
                    m_comboboxDAT.SelectedItem = "DAT_ENTRYPOINT";
                    m_comboboxMSG.SelectedItem = "MSG_GET";
                }
                else
                {
                    m_comboboxDG.SelectedItem = "DG_CONTROL";
                    m_comboboxDAT.SelectedItem = "DAT_IDENTITY";
                    m_comboboxMSG.SelectedItem = "MSG_GETFIRST";
                }
            }

            // After an ENTRYPOINT, switch to GETFIRST...
            else if ((a_szDg == "DG_CONTROL") && (a_szDat == "DAT_ENTRYPOINT") && (a_szMsg == "MSG_GET"))
            {
                m_comboboxDG.SelectedItem = "DG_CONTROL";
                m_comboboxDAT.SelectedItem = "DAT_IDENTITY";
                m_comboboxMSG.SelectedItem = "MSG_GETFIRST";
            }

            // When we CLOSEDSM, go back to OPENDSM...
            else if ((a_szDg == "DG_CONTROL") && (a_szDat == "DAT_PARENT") && (a_szMsg == "MSG_CLOSEDSM"))
            {
                m_comboboxDG.SelectedItem = "DG_CONTROL";
                m_comboboxDAT.SelectedItem = "DAT_PARENT";
                m_comboboxMSG.SelectedItem = "MSG_OPENDSM";
            }

            // After a GETFIRST, switch to GETNEXT...
            else if ((a_szDg == "DG_CONTROL") && (a_szDat == "DAT_IDENTITY") && (a_szMsg == "MSG_GETFIRST"))
            {
                m_comboboxDG.SelectedItem = "DG_CONTROL";
                m_comboboxDAT.SelectedItem = "DAT_IDENTITY";
                m_comboboxMSG.SelectedItem = "MSG_GETNEXT";
            }

            // After an OPENDS, changing to CAPABILITY seems most useful...
            else if ((a_szDg == "DG_CONTROL") && (a_szDat == "DAT_IDENTITY") && (a_szMsg == "MSG_OPENDS"))
            {
                m_comboboxDG.SelectedItem = "DG_CONTROL";
                m_comboboxDAT.SelectedItem = "DAT_CAPABILITY";
                m_comboboxMSG.SelectedItem = "MSG_GETCURRENT";
            }

            // After a CLOSEDS, switch to CLOSEDSM...
            else if ((a_szDg == "DG_CONTROL") && (a_szDat == "DAT_IDENTITY") && (a_szMsg == "MSG_CLOSEDS"))
            {
                m_comboboxDG.SelectedItem = "DG_CONTROL";
                m_comboboxDAT.SelectedItem = "DAT_PARENT";
                m_comboboxMSG.SelectedItem = "MSG_CLOSEDSM";
            }

            // After a GETFIRSTFILE, switch to GETNEXTFILE...
            else if ((a_szDg == "DG_CONTROL") && (a_szDat == "DAT_FILESYSTEM") && (a_szMsg == "MSG_GETFIRSTFILE"))
            {
                m_comboboxDG.SelectedItem = "DG_CONTROL";
                m_comboboxDAT.SelectedItem = "DAT_FILESYSTEM";
                m_comboboxMSG.SelectedItem = "MSG_GETNEXTFILE";
            }
        }

        /// <summary>
        /// Send the requested command...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void SendDat(object sender, EventArgs e)
        {
            string szDg;
            string szDat;
            string szMsg;

            // The Data Group selected by the user, this can be a drop down item or a
            // hex value (with or without the leading 0x)...
            if (m_comboboxDG.SelectedItem != null)
            {
                szDg = (string)m_comboboxDG.SelectedItem;
            }
            else
            {
                szDg = m_comboboxDG.Text;
            }

            // The Data Argument Type selected by the user, this can be a drop down
            // item or a hex value (with or without the leading 0x)...
            if (m_comboboxDAT.SelectedItem != null)
            {
                szDat = (string)m_comboboxDAT.SelectedItem;
            }
            else
            {
                szDat = m_comboboxDAT.Text;
            }

            // The Message selected by the user, this can be a drop down item or a
            // hex value (with or without the leading 0x)...
            if (m_comboboxMSG.SelectedItem != null)
            {
                szMsg = (string)m_comboboxMSG.SelectedItem;
            }
            else
            {
                szMsg = m_comboboxMSG.Text;
            }

            // Handle creation and destruction of our image capture object...
            if (    (szDg == "DG_CONTROL") && (szDat == "DAT_PARENT")
                && ((szMsg == "MSG_OPENDSM") || (szDat == "MSG_CLOSEDSM")))
            {
                ManageToolkit(szDg,szDat,szMsg);
                return;
            }

            // Validate...
            if (m_twaincstoolkit == null)
            {
                WriteTriplet(szDg, szDat, szMsg, "(DSM is not open)");
                return;
            }

            // Look for an @-command...
            if (m_twaincstoolkit.AtCommand(m_richtextboxCapability.Text))
            {
                return;
            }

            // We need to handle MSG_CLOSEDS ourselves...
            if ((szDg == "DG_CONTROL") && (szDat == "DAT_IDENTITY") && (szMsg == "MSG_CLOSEDS"))
            {
                m_buttonCloseDS_Click(szDg, szDat, szMsg);
                // Filter for TWAIN messages...
                if (!m_checkboxUseCallbacks.Checked)
                {
                    Application.RemoveMessageFilter(this);
                }

                // Issue the command...
                if (m_twaincstoolkit != null)
                {
                    m_twaincstoolkit.CloseDriver();
                }
                WriteTriplet(szDg, szDat, szMsg, TWAINCSToolkit.STS.SUCCESS.ToString());
            }

            // Everything else can go to the image capture object...
            else
            {
                string szResult;
                string szTwmemref;
                TWAINCSToolkit.STS sts;

                // Grab the data from the cap box...
                szTwmemref = m_richtextboxCapability.Text;

                // Issue the command to TWAIN...
                szResult = "";
                sts = m_twaincstoolkit.Send(szDg, szDat, szMsg, ref szTwmemref, ref szResult);

                // If the command succeeded, then update the cap box with the result...
                m_richtextboxCapability.Text = szTwmemref;

                // Tweak the result...
                if (szResult == "")
                {
                    szResult = sts.ToString();
                }
                else
                {
                    szResult = sts.ToString() + " - " + szResult;
                }

                // And write what happened to the output box...
                if (szTwmemref == "")
                {
                    WriteTriplet(szDg, szDat, szMsg, szResult);
                }
                else
                {
                    WriteTriplet(szDg, szDat, szMsg, szResult + Environment.NewLine + szTwmemref);
                }

                // Convenience settings, we do these to make it less painful
                // to work with the dropdown interface.  I've got them all
                // here, even though some are currently being handled inside
                // of other functions, like SendDatParent...
                if (sts == TWAINCSToolkit.STS.SUCCESS)
                {
                    AutoDropdown(szDg, szDat, szMsg);
                }
            }
        }

        /// <summary>
        /// Create and destroy our toolkit object, as needed...
        /// </summary>
        /// <param name="a_szDg">Data group</param>
        /// <param name="a_szDat">Data argument type</param>
        /// <param name="a_szMsg">Operation</param>
        void ManageToolkit(string a_szDg, string a_szDat, string a_szMsg)
        {
            // Handle MSG_OPENDSM...
            if (a_szMsg == "MSG_OPENDSM")
            {
                // Init stuff...
                m_blClosing = false;

                // Validate...
                if (m_twaincstoolkit != null)
                {
                    WriteTriplet(a_szDg, a_szDat, a_szMsg, "(already open)");
                    return;
                }

                // Create our image capture object...
                try
                {
                    m_twaincstoolkit = new TWAINCSToolkit
                    (
                        this.Handle,
                        WriteOutput,
                        ShowImage,
                        SetMessageFilter,
                        "TWAIN Working Group",
                        "TWAIN Sharp",
                        "TWAIN Sharp Test App",
                        2,
                        3,
                        new string[] { "DF_APP2", "DG_CONTROL", "DG_IMAGE" },
                        "USA",
                        "testing...",
                        "ENGLISH_USA",
                        1,
                        0,
                        m_checkboxUseTwain32.Checked,
                        m_checkboxUseCallbacks.Checked
                    );
                }
                catch
                {
                    WriteTriplet(a_szDg, a_szDat, a_szMsg, "(unable to start)");
                    m_twaincstoolkit = null;
                    return;
                }
                WriteTriplet(a_szDg, a_szDat, a_szMsg, TWAINCSToolkit.STS.SUCCESS.ToString());

                // Fix our controls...
                if (TWAINCSToolkit.GetPlatform() == "WINDOWS")
                {
                    m_checkboxUseTwain32.Enabled = false;
                    m_checkboxUseCallbacks.Enabled = false;
                }

                // Help the user...
                AutoDropdown(a_szDg, a_szDat, a_szMsg);
            }

            // Handle MSG_CLOSEDSM...
            else if (a_szMsg == "MSG_CLOSEDSM")
            {
                // Issue the command...
                WriteTriplet(a_szDg, a_szDat, a_szMsg, TWAINCSToolkit.STS.SUCCESS.ToString());
                m_blClosing = true;
                m_twaincstoolkit.Cleanup();
                m_twaincstoolkit = null;

                // Fix our controls...
                if (TWAINCSToolkit.GetPlatform() == "WINDOWS")
                {
                    m_checkboxUseTwain32.Enabled = (TWAINCSToolkit.GetMachineWordBitSize() == 32);
                    m_checkboxUseCallbacks.Enabled = true;
                }

                // Help the user...
                AutoDropdown(a_szDg, a_szDat, a_szMsg);
            }

            // Handle anything else...
            else
            {
                WriteTriplet(a_szDg, a_szDat, a_szMsg, TWAINCSToolkit.STS.BADPROTOCOL.ToString());
            }
        }

        /// <summary>
        /// Close the Data Source...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_dat">Data argument type</param>
        /// <param name="a_msg">Operation</param>
        private void m_buttonCloseDS_Click(string a_szDg, string a_szDat, string a_szMsg)
        {
            // Filter for TWAIN messages...
            if (!m_checkboxUseCallbacks.Checked)
            {
                Application.RemoveMessageFilter(this);
            }

            // Issue the command...
            if (m_twaincstoolkit != null)
            {
                m_twaincstoolkit.CloseDriver();
            }
            WriteTriplet(a_szDg, a_szDat, a_szMsg, TWAINCSToolkit.STS.SUCCESS.ToString());
        }

        /// <summary>
        /// Start a scanning session...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void m_buttonScan_Click(object sender, EventArgs e)
        {
            string szTwmemref;
            string szStatus;
            TWAINCSToolkit.STS sts;
            Bitmap bitmap = new Bitmap(m_pictureboxImage1.Width, m_pictureboxImage1.Height);

            // Reset the picture boxes...
            m_iCurrentPictureBox = 0;
            LoadImage(ref m_pictureboxImage1, ref m_graphics1, ref m_bitmapGraphic1, bitmap);
            LoadImage(ref m_pictureboxImage2, ref m_graphics2, ref m_bitmapGraphic2, bitmap);
            LoadImage(ref m_pictureboxImage3, ref m_graphics3, ref m_bitmapGraphic3, bitmap);
            LoadImage(ref m_pictureboxImage4, ref m_graphics4, ref m_bitmapGraphic4, bitmap);
            LoadImage(ref m_pictureboxImage5, ref m_graphics5, ref m_bitmapGraphic5, bitmap);
            LoadImage(ref m_pictureboxImage6, ref m_graphics6, ref m_bitmapGraphic6, bitmap);
            LoadImage(ref m_pictureboxImage7, ref m_graphics7, ref m_bitmapGraphic7, bitmap);
            LoadImage(ref m_pictureboxImage8, ref m_graphics8, ref m_bitmapGraphic8, bitmap);

            // Request a scan session...
            szTwmemref = "0,0";
            szStatus = "";
            sts = m_twaincstoolkit.Send("DG_CONTROL", "DAT_USERINTERFACE", "MSG_ENABLEDS", ref szTwmemref, ref szStatus);
            WriteTriplet("DG_CONTROL", "DAT_USERINTERFACE", "MSG_ENABLEDS", sts.ToString());
        }

        /// <summary>
        /// Stop scanning...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void m_buttonStop_Click(object sender, EventArgs e)
        {
            TWAINCSToolkit.STS sts;

            // Issue the command...
            if (m_twaincstoolkit != null)
            {
                m_twaincstoolkit.StopSession();
                WriteTriplet("DG_CONTROL", "DAT_USERINTERFACE", "MSG_STOPFEEDER", "SUCCESS");
            }
        }

        /// <summary>
        /// React to a change to the selection of TWAIN_32.DLL...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void m_checkboxUseTwain32_CheckedChanged(object sender, EventArgs e)
        {
            // TWAIN_32.DLL doesn't support callbacks...
            if (m_checkboxUseTwain32.Checked)
            {
                m_checkboxUseCallbacks.Checked = false;
            }
        }

        /// <summary>
        /// React to a chance to the selection of callbacks...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void m_checkboxUseCallbacks_CheckedChanged(object sender, EventArgs e)
        {
            // TWAIN_32.DLL doesn't support callbacks...
            if (m_checkboxUseCallbacks.Checked)
            {
                m_checkboxUseTwain32.Checked = false;
            }
        }

        #endregion
        

        ///////////////////////////////////////////////////////////////////////////////
        // Private Attributes...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Attributes...

        /// <summary>
        /// Our image capture object...
        /// </summary>
        private TWAINCSToolkit m_twaincstoolkit;

        /// <summary>
        /// The picture box we're currently displaying into...
        /// </summary>
        private int m_iCurrentPictureBox;

        // Let us know when we're shutting down...
        private bool m_blClosing;

        // The brush we use for the image background...
        private Brush m_brushBackground;

        // The rectangle we use to fill the background...
        private Rectangle m_rectangleBackground;

        // Bitmaps and graphics used to display images during scanning...
        private Bitmap m_bitmapGraphic1;
        private Bitmap m_bitmapGraphic2;
        private Bitmap m_bitmapGraphic3;
        private Bitmap m_bitmapGraphic4;
        private Bitmap m_bitmapGraphic5;
        private Bitmap m_bitmapGraphic6;
        private Bitmap m_bitmapGraphic7;
        private Bitmap m_bitmapGraphic8;
        private Graphics m_graphics1;
        private Graphics m_graphics2;
        private Graphics m_graphics3;
        private Graphics m_graphics4;
        private Graphics m_graphics5;
        private Graphics m_graphics6;
        private Graphics m_graphics7;
        private Graphics m_graphics8;

        #endregion
    }
}
