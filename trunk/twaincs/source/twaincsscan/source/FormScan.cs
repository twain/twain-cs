///////////////////////////////////////////////////////////////////////////////////////
//
// TWAINCSScan.FormScan
//
// This is the main class for the application.  We're showing how to select and
// load a TWAIN driver.  How to configure it for a scan session, and how to capture
// and display images.
//
// This (moreso than then TWAINCSTst) is designed to be a possible template for
// developers looking to add TWAIN to their C# applications.  It's small, and it's
// focused on the tasks needed to capture image data.
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
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TWAINWorkingGroupToolkit;

namespace TWAINCSScan
{
    /// <summary>
    /// Our mainform for this application...
    /// </summary>
    public partial class FormScan : Form
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Public Methods...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Methods...

        /// <summary>
        /// Our constructor...
        /// </summary>
        public FormScan()
        {
            // Build our form...
            InitializeComponent();

            // Init other stuff...
            m_blIndicators = false;
            m_blExit = false;
            m_iUseBitmap = 0;
            this.FormClosing += new FormClosingEventHandler(FormScan_FormClosing);

            // Create our image capture object...
            try
            {
                m_twaincstoolkit = new TWAINCSToolkit
                (
                    this.Handle,
                    WriteOutput,
                    ShowImage,
                    null,
                    "TWAIN Working Group",
                    "TWAIN Sharp",
                    "TWAIN Sharp Scan App",
                    2,
                    3,
                    new string[] { "DF_APP2", "DG_CONTROL", "DG_IMAGE" },
                    "USA",
                    "testing...",
                    "ENGLISH_USA",
                    1,
                    0,
                    false,
                    true
                );
            }
            catch
            {
                m_twaincstoolkit = null;
                m_blExit = true;
                return;
            }

            // Init our picture box...
            InitImage();

            // Init our buttons...
            SetButtons(EBUTTONSTATE.CLOSED);
        }

        /// <summary>
        /// Something horrible has happened and we need to abort...
        /// </summary>
        /// <returns></returns>
        public bool ExitRequested()
        {
            return (m_blExit);
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Methods...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Methods...


        /// <summary>
        /// We're being closed, clean up nicely...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormScan_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Get rid of the toolkit...
            if (m_twaincstoolkit != null)
            {
                m_twaincstoolkit.Cleanup();
                m_twaincstoolkit = null;
            }

            // This will prevent ShowImage from doing anything as we close...
            m_graphics1 = null;
        }

        /// <summary>
        /// The user wants to setup a customdsdata or a scan session...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_buttonSetup_Click(object sender, EventArgs e)
        {
            m_formsetup.StartPosition = FormStartPosition.CenterParent;
            m_formsetup.ShowDialog(this);
        }

        /// <summary>
        /// Start a scan session...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_buttonScan_Click(object sender, EventArgs e)
        {
            m_iUseBitmap = 0;
            string szTwmemref;
            string szStatus = "";
            TWAINCSToolkit.STS sts;

            // Silently start scanning if we detect that customdsdata is supported,
            // otherwise bring up the driver GUI so the user can change settings...
            if (m_formsetup.IsCustomDsDataSupported())
            {
                szTwmemref = "0,0";
            }
            else
            {
                szTwmemref = "1,0";
            }
            sts = m_twaincstoolkit.Send("DG_CONTROL", "DAT_USERINTERFACE", "MSG_ENABLEDS", ref szTwmemref, ref szStatus);
            SetButtons(EBUTTONSTATE.SCANNING);
        }

        /// <summary>
        /// Debugging output that we can monitor, this is just a place
        /// holder for this particular application...
        /// </summary>
        /// <param name="a_szOutput"></param>
        private void WriteOutput(string a_szOutput)
        {
            return;
        }

        /// <summary>
        /// Show an image...
        /// </summary>
        /// <param name="a_sts">Current status</param>
        /// <param name="a_bitmap">C# bitmap of the image</param>
        /// <param name="a_szFile">File name, if doing a file transfer</param>
        public void ShowImage(TWAINCSToolkit.STS a_sts, Bitmap a_bitmap, string a_szFile)
        {
            // We're leaving...
            if (m_graphics1 == null)
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
                BeginInvoke(new MethodInvoker(delegate() { ShowImage(a_sts, (a_bitmap == null) ? null : new Bitmap(a_bitmap), a_szFile); }));
                return;
            }

            // We're processing end of scan...
            if (a_bitmap == null)
            {
                // Report errors, but only if the driver's indicators have
                // been turned off, otherwise we'll hit the user with multiple
                // dialogs for the same error...
                if (!m_blIndicators && (a_sts != TWAINCSToolkit.STS.SUCCESS))
                {
                    MessageBox.Show("End of session status: " + a_sts);
                }

                // Get ready for the next scan...
                SetButtons(EBUTTONSTATE.OPEN);
                return;
            }

            // Display the image...
            if (m_iUseBitmap == 0)
            {
                m_iUseBitmap = 1;
                LoadImage(ref m_pictureboxImage1, ref m_graphics1, ref m_bitmapGraphic1, a_bitmap);
            }
            else
            {
                m_iUseBitmap = 0;
                LoadImage(ref m_pictureboxImage2, ref m_graphics2, ref m_bitmapGraphic2, a_bitmap);
            }
        }

        /// <summary>
        /// Load an image into a picture box, maintain its aspect ratio...
        /// </summary>
        /// <param name="a_picturebox"></param>
        /// <param name="a_graphics"></param>
        /// <param name="a_bitmapGraphic"></param>
        /// <param name="a_bitmap"></param>
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
            a_graphics.DrawImage(a_bitmap, new Rectangle(((int)a_bitmapGraphic.Width - iWidth) / 2, ((int)a_bitmapGraphic.Height - iHeight) / 2, iWidth, iHeight));
            a_picturebox.Image = a_bitmapGraphic;
            a_picturebox.Update();
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

            m_bitmapGraphic1 = new Bitmap(m_pictureboxImage1.Width, m_pictureboxImage1.Height, PixelFormat.Format32bppPArgb);
            m_graphics1 = Graphics.FromImage(m_bitmapGraphic1);
            m_graphics1.CompositingMode = CompositingMode.SourceCopy;
            m_graphics1.CompositingQuality = CompositingQuality.HighSpeed;
            m_graphics1.InterpolationMode = InterpolationMode.Low;
            m_graphics1.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            m_graphics1.SmoothingMode = SmoothingMode.HighSpeed;

            m_bitmapGraphic2 = new Bitmap(m_pictureboxImage1.Width, m_pictureboxImage1.Height, PixelFormat.Format32bppPArgb);
            m_graphics2 = Graphics.FromImage(m_bitmapGraphic2);
            m_graphics2.CompositingMode = CompositingMode.SourceCopy;
            m_graphics2.CompositingQuality = CompositingQuality.HighSpeed;
            m_graphics2.InterpolationMode = InterpolationMode.Low;
            m_graphics2.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            m_graphics2.SmoothingMode = SmoothingMode.HighSpeed;

            m_brushBackground = new SolidBrush(Color.White);
            m_rectangleBackground = new Rectangle(0, 0, m_bitmapGraphic1.Width, m_bitmapGraphic1.Height);
        }

        enum EBUTTONSTATE
        {
            CLOSED,
            OPEN,
            SCANNING
        }

        /// <summary>
        /// Configure our buttons to match our current state...
        /// </summary>
        /// <param name="a_ebuttonstate"></param>
        private void SetButtons(EBUTTONSTATE a_ebuttonstate)
        {
            switch (a_ebuttonstate)
            {
                default:
                case EBUTTONSTATE.CLOSED:
                    m_buttonOpen.Enabled = true;
                    m_buttonClose.Enabled = false;
                    m_buttonSetup.Enabled = false;
                    m_buttonScan.Enabled = false;
                    m_buttonStop.Enabled = false;
                    break;

                case EBUTTONSTATE.OPEN:
                    m_buttonOpen.Enabled = false;
                    m_buttonClose.Enabled = true;
                    m_buttonSetup.Enabled = true;
                    m_buttonScan.Enabled = true;
                    m_buttonStop.Enabled = false;
                    break;

                case EBUTTONSTATE.SCANNING:
                    m_buttonOpen.Enabled = false;
                    m_buttonClose.Enabled = false;
                    m_buttonSetup.Enabled = false;
                    m_buttonScan.Enabled = false;
                    m_buttonStop.Enabled = true;
                    break;
            }
        }


        /// <summary>
        /// Select and open a TWAIN driver...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_buttonOpen_Click(object sender, EventArgs e)
        {
            string szIdentity;
            string szCapability;
            string szDefault;
            string szStatus;
            string[] aszIdentity;
            FormSelect formselect;
            DialogResult dialogresult;
            TWAINCSToolkit.STS sts;

            // Find out which driver we're using...
            szDefault = "";
            aszIdentity = m_twaincstoolkit.GetDrivers(ref szDefault);
            formselect = new FormSelect(aszIdentity, szDefault);
            formselect.StartPosition = FormStartPosition.CenterParent;
            dialogresult = formselect.ShowDialog(this);
            if (dialogresult != System.Windows.Forms.DialogResult.OK)
            {
                m_blExit = true;
                return;
            }

            // Get all the identities...
            szIdentity = formselect.GetSelectedDriver();
            if (szIdentity == null)
            {
                m_blExit = true;
                return;
            }

            // Get the selected identity...
            m_blExit = true;
            foreach (string sz in aszIdentity)
            {
                if (sz.Contains(szIdentity))
                {
                    m_blExit = false;
                    szIdentity = sz;
                    break;
                }
            }
            if (m_blExit)
            {
                return;
            }

            // Make it the default, we don't care if this succeeds...
            szStatus = "";
            m_twaincstoolkit.Send("DG_CONTROL", "DAT_IDENTITY", "MSG_SET", ref szIdentity, ref szStatus);

            // Open it...
            szStatus = "";
            sts = m_twaincstoolkit.Send("DG_CONTROL", "DAT_IDENTITY", "MSG_OPENDS", ref szIdentity, ref szStatus);
            if (sts != TWAINCSToolkit.STS.SUCCESS)
            {
                MessageBox.Show("Unable to open scanner (it is turned on and plugged in?)");
                m_blExit = true;
                return;
            }

            // Strip off unsafe chars.  Sadly, mono let's us down here...
            m_szProductDirectory = TWAINCSToolkit.CsvParse(szIdentity)[11];
            foreach (char c in new char [41]
                            { '\x00', '\x01', '\x02', '\x03', '\x04', '\x05', '\x06', '\x07',
                              '\x08', '\x09', '\x0A', '\x0B', '\x0C', '\x0D', '\x0E', '\x0F', '\x10', '\x11', '\x12', 
                              '\x13', '\x14', '\x15', '\x16', '\x17', '\x18', '\x19', '\x1A', '\x1B', '\x1C', '\x1D', 
                              '\x1E', '\x1F', '\x22', '\x3C', '\x3E', '\x7C', ':', '*', '?', '\\', '/'
                            }
                    )
            {
                m_szProductDirectory = m_szProductDirectory.Replace(c, '_');
            }

            // We're doing memory transfers (TWSX_MEMORY == 2)...
            szStatus = "";
            szCapability = "ICAP_XFERMECH,TWON_ONEVALUE,TWTY_UINT16,2";
            sts = m_twaincstoolkit.Send("DG_CONTROL", "DAT_CAPABILITY", "MSG_SET", ref szCapability, ref szStatus);
            if (sts != TWAINCSToolkit.STS.SUCCESS)
            {
                m_blExit = true;
                return;
            }

            // Decide whether or not to show the driver's window messages...
            szStatus = "";
            szCapability = "CAP_INDICATORS,TWON_ONEVALUE,TWTY_UINT16," + (m_blIndicators?"1":"0");
            sts = m_twaincstoolkit.Send("DG_CONTROL", "DAT_CAPABILITY", "MSG_SET", ref szCapability, ref szStatus);
            if (sts != TWAINCSToolkit.STS.SUCCESS)
            {
                m_blExit = true;
                return;
            }

            // New state...
            SetButtons(EBUTTONSTATE.OPEN);

            // Create the setup form...
            m_formsetup = new FormSetup(ref m_twaincstoolkit, m_szProductDirectory);
        }

        /// <summary>
        /// Close the currently open TWAIN driver...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_buttonClose_Click(object sender, EventArgs e)
        {
            m_twaincstoolkit.CloseDriver();
            SetButtons(EBUTTONSTATE.CLOSED);
            m_formsetup.Dispose();
            m_formsetup = null;
        }

        /// <summary>
        /// Request that scanning stop (gracefully)...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_buttonStop_Click(object sender, EventArgs e)
        {
            string szPendingxfers = "0,0";
            string szStatus = "";
            m_twaincstoolkit.Send("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_STOPFEEDER", ref szPendingxfers, ref szStatus);
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Attributes...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Attributes...

        /// <summary>
        /// Use if something really bad happens...
        /// </summary>
        private bool m_blExit;

        /// <summary>
        /// Our interface to TWAIN...
        /// </summary>
        private TWAINCSToolkit m_twaincstoolkit;

        // Setup information...
        private FormSetup m_formsetup;

        /// <summary>
        /// We use this name (modified and made file system safe)
        /// as the place where we'll put customdsdata...
        /// </summary>
        private string m_szProductDirectory;

        /// <summary>
        /// If true, then show the driver's window messages while
        /// we're scanning.  Set this in the constructor...
        /// </summary>
        private bool m_blIndicators;

        /// <summary>
        /// Stuff used to display the images...
        /// </summary>
        private Bitmap m_bitmapGraphic1;
        private Bitmap m_bitmapGraphic2;
        private Graphics m_graphics1;
        private Graphics m_graphics2;
        private Brush m_brushBackground;
        private Rectangle m_rectangleBackground;
        private int m_iUseBitmap;

        #endregion
    }
}
