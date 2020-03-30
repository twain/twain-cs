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
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using TWAINWorkingGroup;

namespace TWAINCSTst
{
    /// <summary>
    /// Here's a form for us to tinker with.  We're using this to exercise the
    /// TWAIN object.
    /// </summary>
    public partial class FormMain : Form, IMessageFilter, IDisposable
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

            // Open the log in our working folder, and say hi...
            TWAINWorkingGroup.Log.Open("TWAINCSTst", ".", 1);
            TWAINWorkingGroup.Log.Info("TWAINCSTst v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());

            // Make sure we cleanup if unexpectedly closed...
            this.FormClosing += new FormClosingEventHandler(FormMain_FormClosing);

            // This next bit establishes the rules for the various DSM's on the
            // various operating systems.

            // Windows controls...
            if (TWAIN.GetPlatform() == TWAIN.Platform.WINDOWS)
            {
                // Choose between TWAIN_32 and TWAINDSM, note that we always default
                // to the open source TWAIN DSM...
                m_checkboxUseTwain32.Enabled = (TWAIN.GetMachineWordBitSize() == 32);
                m_checkboxUseCallbacks.Enabled = true;
                m_checkboxUseTwain32.Checked = false;
                m_checkboxUseCallbacks.Checked = true;
            }

            // Linux controls...
            else if (TWAIN.GetPlatform() == TWAIN.Platform.LINUX)
            {
                // We don't give the user control between the DSM versions, because
                // the 64-bit problem is handled as seamlessly as possible...
                m_checkboxUseTwain32.Enabled = false;
                m_checkboxUseCallbacks.Enabled = false;
                m_checkboxUseTwain32.Checked = false;
                m_checkboxUseCallbacks.Checked = true;
            }

            // Mac OS X controls...
            else if (TWAIN.GetPlatform() == TWAIN.Platform.MACOSX)
            {
                // Choose between /System/Library/Frameworks/TWAIN.framework and
                // /Library/Frameworks/TWAINDSM.framework, note that we always default
                // to the open source TWAIN DSM...
                m_checkboxUseTwain32.Enabled = true;
                m_checkboxUseCallbacks.Enabled = false;
                m_checkboxUseTwain32.Checked = false;
                m_checkboxUseCallbacks.Checked = true;
            }

            // Autoscroll the text in our output box...
            m_richtextboxOutput.HideSelection = false;
            m_richtextboxOutput.SelectionProtected = false;

            // Init other stuff...
            m_twain = null;
            m_intptrHwnd = Handle;

            // Init our image controls...
            InitImage();

            // Load our triplet dropdown...
            List<string> aszDat = new List<string>();
            List<string> aszMsg = new List<string>();
            foreach (string szDat in (string[])Enum.GetNames(typeof(TWAIN.DAT))) aszDat.Add("DAT_" + szDat);
            foreach (string szMsg in (string[])Enum.GetNames(typeof(TWAIN.MSG))) aszMsg.Add("MSG_" + szMsg);
            this.m_comboboxDG.Items.AddRange(new string[] { "DG_AUDIO", "DG_CONTROL", "DG_IMAGE" });
            this.m_comboboxDAT.Items.AddRange(aszDat.ToArray());
            this.m_comboboxMSG.Items.AddRange(aszMsg.ToArray());

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

            // Set the capbility box to our app info...
            m_richtextboxCapability.Text =
                "TWAIN Working Group," +
                "TWAIN Sharp," +
                "TWAIN Sharp Test App," +
                "2," +
                "4," +
                "0x20000003," +
                "USA," +
                "testing...," +
                "ENGLISH_USA";
        }

        /// <summary>
        /// Call the form's filter function to catch stuff like MSG.XFERREADY...
        /// </summary>
        /// <param name="a_message">Message to process</param>
        /// <returns>Result of the processing</returns>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool PreFilterMessage(ref Message a_message)
        {
            if (m_twain != null)
            {
                return (m_twain.PreFilterMessage(a_message.HWnd, a_message.Msg, a_message.WParam, a_message.LParam));
            }
            return (true);
        }

        /// <summary>
        /// Our scan callback event, used to drive the engine when scanning...
        /// </summary>
        public delegate void ScanCallbackEvent();

        /// <summary>
        /// Our event handler for the scan callback event.  This will be
        /// called once by ScanCallbackTrigger on receipt of an event
        /// like MSG_XFERREADY, and then will be reissued on every call
        /// into ScanCallback until we're done and get back to state 4.
        ///  
        /// This helps to make sure we're always running in the context
        /// of FormMain on Windows, which is critical if we want drivers
        /// to work properly.  It also gives a way to break up the calls
        /// so the message pump is still reponsive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanCallbackEventHandler(object sender, EventArgs e)
        {
            ScanCallback((m_twain == null) ? true : (m_twain.GetState() <= TWAIN.STATE.S3));
        }

        /// <summary>
        /// Rollback the TWAIN state to whatever is requested...
        /// </summary>
        /// <param name="a_state"></param>
        public void Rollback(TWAIN.STATE a_state)
        {
            string szTwmemref = "";
            string szStatus = "";
            TWAIN.STS sts;

            // Make sure we have something to work with...
            if (m_twain == null)
            {
                return;
            }

            // Walk the states, we don't care about the status returns.  Basically,
            // these need to work, or we're guaranteed to hang...

            // 7 --> 6
            if ((m_twain.GetState() == TWAIN.STATE.S7) && (a_state < TWAIN.STATE.S7))
            {
                szTwmemref = "0,0";
                szStatus = "";
                sts = Send("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", ref szTwmemref, ref szStatus);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
            }

            // 6 --> 5
            if ((m_twain.GetState() == TWAIN.STATE.S6) && (a_state < TWAIN.STATE.S6))
            {
                szTwmemref = "0,0";
                szStatus = "";
                sts = Send("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_RESET", ref szTwmemref, ref szStatus);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_RESET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
            }

            // 5 --> 4
            if ((m_twain.GetState() == TWAIN.STATE.S5) && (a_state < TWAIN.STATE.S5))
            {
                szTwmemref = "0,0," + m_intptrHwnd;
                szStatus = "";
                sts = Send("DG_CONTROL", "DAT_USERINTERFACE", "MSG_DISABLEDS", ref szTwmemref, ref szStatus);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_CONTROL", "DAT_USERINTERFACE", "MSG_DISABLEDS", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
                ClearEvents();
            }

            // 4 --> 3
            if ((m_twain.GetState() == TWAIN.STATE.S4) && (a_state < TWAIN.STATE.S4))
            {
                if (!m_checkboxUseCallbacks.Checked)
                {
                    Application.RemoveMessageFilter(this);
                }
                szTwmemref = m_twain.GetDsIdentity();
                szStatus = "";
                sts = Send("DG_CONTROL", "DAT_IDENTITY", "MSG_CLOSEDS", ref szTwmemref, ref szStatus);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_CONTROL", "DAT_IDENTITY", "MSG_CLOSEDS", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
            }

            // 3 --> 2
            if ((m_twain.GetState() == TWAIN.STATE.S3) && (a_state < TWAIN.STATE.S3))
            {
                szTwmemref = m_intptrHwnd.ToString();
                szStatus = "";
                sts = Send("DG_CONTROL", "DAT_PARENT", "MSG_CLOSEDSM", ref szTwmemref, ref szStatus);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_CONTROL", "DAT_PARENT", "MSG_CLOSEDSM", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
            }

            // 2 --> 1
            if ((m_twain.GetState() == TWAIN.STATE.S2) && (a_state < TWAIN.STATE.S2))
            {
                m_twain.Dispose();
                m_twain = null;
            }
        }

        /// <summary>
        /// Our scanning callback function.  We appeal directly to the supporting
        /// TWAIN object.  This way we don't have to maintain some kind of a loop
        /// inside of the application, which is the source of most problems that
        /// developers run into.
        /// 
        /// While it looks scary at first, there's really not a lot going on in
        /// here.  We do some sanity checks, we watch for certain kinds of events,
        /// we support the four methods of transferring images, and we dump out
        /// some meta-data about the transferred image.  However, because it does
        /// look scary I dropped in some region pragmas to break things up...
        /// </summary>
        /// <param name="a_blClosing">We're shutting down</param>
        /// <returns>TWAIN status</returns>
        private TWAIN.STS ScanCallbackTrigger(bool a_blClosing)
        {
            BeginInvoke(new MethodInvoker(delegate { ScanCallbackEventHandler(this, new EventArgs()); }));
            return (TWAIN.STS.SUCCESS);
        }
        private TWAIN.STS ScanCallback(bool a_blClosing)
        {
            string szTwmemref = "";
            string szStatus = "";
            TWAIN.STS sts;

            // Scoot...
            if (m_twain == null)
            {
                return (TWAIN.STS.FAILURE);
            }

            // We're superfluous...
            if (m_twain.GetState() <= TWAIN.STATE.S4)
            {
                return (TWAIN.STS.SUCCESS);
            }

            // We're leaving...
            if (a_blClosing)
            {
                return (TWAIN.STS.SUCCESS);
            }

            // Do this in the right thread, we'll usually be in the
            // right spot, save maybe on the first call...
            if (this.InvokeRequired)
            {
                return
                (
                    (TWAIN.STS)Invoke
                    (
                        (Func<TWAIN.STS>)delegate
                        {
                            return (ScanCallback(a_blClosing));
                        }
                    )
                );
            }

            // Handle DAT_NULL/MSG_XFERREADY...
            if (m_twain.IsMsgXferReady() && !m_blXferReadySent)
            {
                m_blXferReadySent = true;

                // What transfer mechanism are we using?
                szTwmemref = "ICAP_XFERMECH,0,0,0";
                szStatus = "";
                sts = Send("DG_CONTROL", "DAT_CAPABILITY", "MSG_GETCURRENT", ref szTwmemref, ref szStatus);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_CONTROL", "DAT_SETUPMEMXFER", "MSG_GET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
                if (szTwmemref.EndsWith("TWSX_NATIVE")) m_twsxXferMech = TWAIN.TWSX.NATIVE;
                else if (szTwmemref.EndsWith("TWSX_MEMORY")) m_twsxXferMech = TWAIN.TWSX.MEMORY;
                else if (szTwmemref.EndsWith("TWSX_FILE")) m_twsxXferMech = TWAIN.TWSX.FILE;
                else if (szTwmemref.EndsWith("TWSX_MEMFILE")) m_twsxXferMech = TWAIN.TWSX.MEMFILE;

                // Memory and memfile transfers need this...
                if ((m_twsxXferMech == TWAIN.TWSX.MEMORY) || (m_twsxXferMech == TWAIN.TWSX.MEMFILE))
                {
                    // Get the amount of memory needed...
                    szTwmemref = "0,0,0";
                    szStatus = "";
                    sts = Send("DG_CONTROL", "DAT_SETUPMEMXFER", "MSG_GET", ref szTwmemref, ref szStatus);
                    TWAIN.CsvToSetupmemxfer(ref m_twsetupmemxfer, szTwmemref);
                    szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                    WriteTriplet("DG_CONTROL", "DAT_SETUPMEMXFER", "MSG_GET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
                    if ((sts != TWAIN.STS.SUCCESS) || (m_twsetupmemxfer.Preferred == 0))
                    {
                        m_blXferReadySent = false;
                        if (!m_blDisableDsSent)
                        {
                            m_blDisableDsSent = true;
                            Rollback(TWAIN.STATE.S4);
                        }
                    }

                    // Allocate the transfer memory (with a little extra to protect ourselves)...
                    m_intptrXfer = Marshal.AllocHGlobal((int)m_twsetupmemxfer.Preferred + 65536);
                    if (m_intptrXfer == IntPtr.Zero)
                    {
                        m_blDisableDsSent = true;
                        Rollback(TWAIN.STATE.S4);
                    }
                }
            }

            // Handle DAT_NULL/MSG_CLOSEDSREQ...
            if (m_twain.IsMsgCloseDsReq() && !m_blDisableDsSent)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
            }

            // Handle DAT_NULL/MSG_CLOSEDSOK...
            if (m_twain.IsMsgCloseDsOk() && !m_blDisableDsSent)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
            }

            // This is where the statemachine runs that transfers and optionally
            // saves the images to disk (it also displays them).  It'll go back
            // and forth between states 6 and 7 until an error occurs, or until
            // we run out of images...
            if (m_blXferReadySent && !m_blDisableDsSent)
            {
                switch (m_twsxXferMech)
                {
                    default:
                    case TWAIN.TWSX.NATIVE:
                        CaptureNativeImages();
                        break;

                    case TWAIN.TWSX.MEMORY:
                        CaptureMemImages();
                        break;

                    case TWAIN.TWSX.FILE:
                        CaptureFileImages();
                        break;

                    case TWAIN.TWSX.MEMFILE:
                        CaptureMemfileImages();
                        break;
                }
            }

            // Trigger the next event, this is where things all chain together.
            // We need begininvoke to prevent blockking, so that we don't get
            // backed up into a messy kind of recursion.  We need DoEvents,
            // because if things really start moving fast it's really hard for
            // application events, like button clicks to break through...
            Application.DoEvents();
            BeginInvoke(new MethodInvoker(delegate { ScanCallbackEventHandler(this, new EventArgs()); }));

            // All done...
            return (TWAIN.STS.SUCCESS);
        }

        /// <summary>
        /// Go through the sequence needed to capture images using DAT_IMAGENATIVEXFER...
        /// </summary>
        private void CaptureNativeImages()
        {
            string szTwmemref = "";
            string szStatus = "";
            IntPtr intptrHandle = IntPtr.Zero;
            IntPtr intptrPtr = IntPtr.Zero;
            TWAIN.STS sts;
            TWAIN.TW_PENDINGXFERS twpendingxfers = default(TWAIN.TW_PENDINGXFERS);

            // Dispatch on the state...
            switch (m_twain.GetState())
            {
                // Not a good state, just scoot...
                default:
                    return;

                // We're on our way out...
                case TWAIN.STATE.S5:
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    return;

                // Native transfers...
                case TWAIN.STATE.S6:
                    szTwmemref = "0";
                    szStatus = "";
                    sts = Send("DG_IMAGE", "DAT_IMAGENATIVEXFER", "MSG_GET", ref szTwmemref, ref szStatus);
                    if (!string.IsNullOrEmpty(szTwmemref))
                    {
                        UInt64 u64Handle;
                        if (UInt64.TryParse(szTwmemref, out u64Handle))
                        {
                            intptrHandle = (IntPtr)u64Handle;
                        }
                    }
                    szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                    WriteTriplet("DG_IMAGE", "DAT_IMAGENATIVEXFER", "MSG_GET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
                    break;
            }

            // Handle problems...
            if (sts != TWAIN.STS.XFERDONE)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                return;
            }

            // Ruh-roh...
            if (intptrHandle == IntPtr.Zero)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                return;
            }

            // If we saw XFERDONE we can save the image, display it,
            // end the transfer, and see if we have more images...
            if (sts == TWAIN.STS.XFERDONE)
            {
                // Bump up our image counter, this always grows for the
                // life of the entire session...
                m_iImageCount += 1;

                // Get our pointer...
                intptrPtr = m_twain.DsmMemLock(intptrHandle);
                if (intptrPtr == IntPtr.Zero)
                {
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    return;
                }

                // Turn our image pointer into a byte array...
                int iHeaderBytes;
                byte[] abImage = m_twain.NativeToByteArray(intptrPtr, true, out iHeaderBytes);

                // Unlock and free the memory we got from the scanner...
                m_twain.DsmMemUnlock(intptrHandle);
                m_twain.DsmMemFree(ref intptrHandle);

                // Save the image to disk, if we're doing that...
                /*
                if (!string.IsNullOrEmpty(m_formsetup.GetImageFolder()))
                {
                    // Create the directory, if needed...
                    if (!Directory.Exists(m_formsetup.GetImageFolder()))
                    {
                        try
                        {
                            Directory.CreateDirectory(m_formsetup.GetImageFolder());
                        }
                        catch (Exception exception)
                        {
                            TWAINWorkingGroup.Log.Error("CreateDirectory failed - " + exception.Message);
                        }
                    }

                    // Write it out...
                    string szFilename = Path.Combine(m_formsetup.GetImageFolder(), "img" + string.Format("{0:D6}", m_iImageCount));
                    TWAIN.WriteImageFile(szFilename, m_intptrImage, m_iImageBytes, out szFilename);
                }
                */

                // Turn the byte array into a stream...
                MemoryStream memorystream = new MemoryStream(abImage);
                Bitmap bitmap = (Bitmap)Image.FromStream(memorystream);

                // Display the image...

                // Display the image...
                switch (m_iCurrentPictureBox)
                {
                    default:
                    case 0:
                        LoadImage(ref m_pictureboxImage1, ref m_graphics1, ref m_bitmapGraphic1, bitmap);
                        break;
                    case 1:
                        LoadImage(ref m_pictureboxImage2, ref m_graphics2, ref m_bitmapGraphic2, bitmap);
                        break;
                    case 2:
                        LoadImage(ref m_pictureboxImage3, ref m_graphics3, ref m_bitmapGraphic3, bitmap);
                        break;
                    case 3:
                        LoadImage(ref m_pictureboxImage4, ref m_graphics4, ref m_bitmapGraphic4, bitmap);
                        break;
                    case 4:
                        LoadImage(ref m_pictureboxImage5, ref m_graphics5, ref m_bitmapGraphic5, bitmap);
                        break;
                    case 5:
                        LoadImage(ref m_pictureboxImage6, ref m_graphics6, ref m_bitmapGraphic6, bitmap);
                        break;
                    case 6:
                        LoadImage(ref m_pictureboxImage7, ref m_graphics7, ref m_bitmapGraphic7, bitmap);
                        break;
                    case 7:
                        LoadImage(ref m_pictureboxImage8, ref m_graphics8, ref m_bitmapGraphic8, bitmap);
                        break;
                }

                // Next picture box...
                if (++m_iCurrentPictureBox >= 8)
                {
                    m_iCurrentPictureBox = 0;
                }

                // Cleanup...
                bitmap.Dispose();
                memorystream = null; // disposed by the bitmap
                abImage = null;

                // End the transfer...
                szTwmemref = "0,0";
                szStatus = "";
                sts = Send("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", ref szTwmemref, ref szStatus);
                TWAIN.CsvToPendingXfers(ref twpendingxfers, szTwmemref);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));

                // Looks like we're done!
                if (twpendingxfers.Count == 0)
                {
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    return;
                }
            }
        }

        /// <summary>
        /// Go through the sequence needed to capture images using DAT_IMAGEMEMXFER...
        /// </summary>
        private void CaptureMemImages()
        {
            string szTwmemref = "";
            string szStatus = "";
            TWAIN.STS sts;
            TWAIN.TW_IMAGEINFO twimageinfo = default(TWAIN.TW_IMAGEINFO);
            TWAIN.TW_IMAGEMEMXFER twimagememxfer = default(TWAIN.TW_IMAGEMEMXFER);
            TWAIN.TW_PENDINGXFERS twpendingxfers = default(TWAIN.TW_PENDINGXFERS);

            // Dispatch on the state...
            switch (m_twain.GetState())
            {
                // Not a good state, just scoot...
                default:
                    return;

                // We're on our way out...
                case TWAIN.STATE.S5:
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    return;

                // Memory transfers...
                case TWAIN.STATE.S6:
                case TWAIN.STATE.S7:
                    szTwmemref = "0,0,0,0,0,0,0," + ((int)TWAIN.TWMF.APPOWNS | (int)TWAIN.TWMF.POINTER) + "," + m_twsetupmemxfer.Preferred + "," + m_intptrXfer;
                    szStatus = "";
                    sts = Send("DG_IMAGE", "DAT_IMAGEMEMXFER", "MSG_GET", ref szTwmemref, ref szStatus);
                    TWAIN.CsvToImagememxfer(ref twimagememxfer, szTwmemref);
                    szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                    WriteTriplet("DG_IMAGE", "DAT_IMAGEMEMXFER", "MSG_GET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
                    break;
            }

            // Handle problems...
            if ((sts != TWAIN.STS.SUCCESS) && (sts != TWAIN.STS.XFERDONE))
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                return;
            }

            // Allocate or grow the image memory...
            if (m_intptrImage == IntPtr.Zero)
            {
                m_intptrImage = Marshal.AllocHGlobal((int)twimagememxfer.BytesWritten);
            }
            else
            {
                m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(m_iImageBytes + twimagememxfer.BytesWritten));
            }

            // Ruh-roh...
            if (m_intptrImage == IntPtr.Zero)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                return;
            }

            // Copy into the buffer, and bump up our byte tally...
            TWAIN.MemCpy(m_intptrImage + m_iImageBytes, m_intptrXfer, (int)twimagememxfer.BytesWritten);
            m_iImageBytes += (int)twimagememxfer.BytesWritten;

            // If we saw XFERDONE we can save the image, display it,
            // end the transfer, and see if we have more images...
            if (sts == TWAIN.STS.XFERDONE)
            {
                // Bump up our image counter, this always grows for the
                // life of the entire session...
                m_iImageCount += 1;

                // Get the image info...
                szTwmemref = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
                szStatus = "";
                sts = Send("DG_IMAGE", "DAT_IMAGEINFO", "MSG_GET", ref szTwmemref, ref szStatus);
                TWAIN.CsvToImageinfo(ref twimageinfo, szTwmemref);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_IMAGE", "DAT_IMAGEINFO", "MSG_GET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));

                // Add the appropriate header...

                // Bitonal uncompressed...
                if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.BW) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.NONE))
                {
                    TWAIN.TiffBitonalUncompressed tiffbitonaluncompressed;
                    tiffbitonaluncompressed = new TWAIN.TiffBitonalUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffbitonaluncompressed) + m_iImageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffbitonaluncompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffbitonaluncompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffbitonaluncompressed);
                }

                // Bitonal GROUP4...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.BW) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.GROUP4))
                {
                    TWAIN.TiffBitonalG4 tiffbitonalg4;
                    tiffbitonalg4 = new TWAIN.TiffBitonalG4((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffbitonalg4) + m_iImageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffbitonalg4)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffbitonalg4, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffbitonalg4);
                }

                // Gray uncompressed...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.GRAY) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.NONE))
                {
                    TWAIN.TiffGrayscaleUncompressed tiffgrayscaleuncompressed;
                    tiffgrayscaleuncompressed = new TWAIN.TiffGrayscaleUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffgrayscaleuncompressed) + m_iImageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffgrayscaleuncompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffgrayscaleuncompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffgrayscaleuncompressed);
                }

                // Gray JPEG...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.GRAY) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.JPEG))
                {
                    // No work to be done, we'll output JPEG...
                }

                // RGB uncompressed...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.RGB) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.NONE))
                {
                    TWAIN.TiffColorUncompressed tiffcoloruncompressed;
                    tiffcoloruncompressed = new TWAIN.TiffColorUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffcoloruncompressed) + m_iImageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffcoloruncompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffcoloruncompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffcoloruncompressed);
                }

                // RGB JPEG...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.RGB) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.JPEG))
                {
                    // No work to be done, we'll output JPEG...
                }

                // Oh well...
                else
                {
                    TWAINWorkingGroup.Log.Error("unsupported format <" + twimageinfo.PixelType + "," + twimageinfo.Compression + ">");
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    return;
                }

                // Save the image to disk, if we're doing that...
                /*
                if (!string.IsNullOrEmpty(m_formsetup.GetImageFolder()))
                {
                    // Create the directory, if needed...
                    if (!Directory.Exists(m_formsetup.GetImageFolder()))
                    {
                        try
                        {
                            Directory.CreateDirectory(m_formsetup.GetImageFolder());
                        }
                        catch (Exception exception)
                        {
                            TWAINWorkingGroup.Log.Error("CreateDirectory failed - " + exception.Message);
                        }
                    }

                    // Write it out...
                    string szFilename = Path.Combine(m_formsetup.GetImageFolder(), "img" + string.Format("{0:D6}", m_iImageCount));
                    TWAIN.WriteImageFile(szFilename, m_intptrImage, m_iImageBytes, out szFilename);
                }
                */

                // Turn the image into a byte array, and free the original memory...
                byte[] abImage = new byte[m_iImageBytes];
                Marshal.Copy(m_intptrImage, abImage, 0, m_iImageBytes);
                Marshal.FreeHGlobal(m_intptrImage);
                m_intptrImage = IntPtr.Zero;
                m_iImageBytes = 0;

                // Turn the byte array into a stream...
                MemoryStream memorystream = new MemoryStream(abImage);
                Bitmap bitmap = (Bitmap)Image.FromStream(memorystream);

                // Display the image...

                // Display the image...
                switch (m_iCurrentPictureBox)
                {
                    default:
                    case 0:
                        LoadImage(ref m_pictureboxImage1, ref m_graphics1, ref m_bitmapGraphic1, bitmap);
                        break;
                    case 1:
                        LoadImage(ref m_pictureboxImage2, ref m_graphics2, ref m_bitmapGraphic2, bitmap);
                        break;
                    case 2:
                        LoadImage(ref m_pictureboxImage3, ref m_graphics3, ref m_bitmapGraphic3, bitmap);
                        break;
                    case 3:
                        LoadImage(ref m_pictureboxImage4, ref m_graphics4, ref m_bitmapGraphic4, bitmap);
                        break;
                    case 4:
                        LoadImage(ref m_pictureboxImage5, ref m_graphics5, ref m_bitmapGraphic5, bitmap);
                        break;
                    case 5:
                        LoadImage(ref m_pictureboxImage6, ref m_graphics6, ref m_bitmapGraphic6, bitmap);
                        break;
                    case 6:
                        LoadImage(ref m_pictureboxImage7, ref m_graphics7, ref m_bitmapGraphic7, bitmap);
                        break;
                    case 7:
                        LoadImage(ref m_pictureboxImage8, ref m_graphics8, ref m_bitmapGraphic8, bitmap);
                        break;
                }

                // Next picture box...
                if (++m_iCurrentPictureBox >= 8)
                {
                    m_iCurrentPictureBox = 0;
                }

                // Cleanup...
                bitmap.Dispose();
                memorystream = null; // disposed by the bitmap
                abImage = null;

                // End the transfer...
                szTwmemref = "0,0";
                szStatus = "";
                sts = Send("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", ref szTwmemref, ref szStatus);
                TWAIN.CsvToPendingXfers(ref twpendingxfers, szTwmemref);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));

                // Looks like we're done!
                if (twpendingxfers.Count == 0)
                {
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    return;
                }
            }
        }

        /// <summary>
        /// Go through the sequence needed to capture images using DAT_IMAGEFILEXFER...
        /// </summary>
        private void CaptureFileImages()
        {
            string szTwmemref = "";
            string szStatus = "";
            string szFilename = "";
            TWAIN.STS sts;
            TWAIN.TW_IMAGEINFO twimageinfo = default(TWAIN.TW_IMAGEINFO);
            TWAIN.TW_PENDINGXFERS twpendingxfers = default(TWAIN.TW_PENDINGXFERS);

            // Dispatch on the state...
            switch (m_twain.GetState())
            {
                // Not a good state, just scoot...
                default:
                    return;

                // We're on our way out...
                case TWAIN.STATE.S5:
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    return;

                // Memory transfers...
                case TWAIN.STATE.S6:
                    // Get the image info...
                    szTwmemref = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
                    szStatus = "";
                    sts = Send("DG_IMAGE", "DAT_IMAGEINFO", "MSG_GET", ref szTwmemref, ref szStatus);
                    TWAIN.CsvToImageinfo(ref twimageinfo, szTwmemref);
                    szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                    WriteTriplet("DG_IMAGE", "DAT_IMAGEINFO", "MSG_GET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));

                    // Pick an image file format...
                    if (twimageinfo.Compression == (ushort)TWAIN.TWCP.JPEG)
                    {
                        szFilename = "C:/twain/image.jpg";
                        szTwmemref = szFilename + ",TWFF_JFIF,0";
                    }
                    else
                    {
                        szFilename = "C:/twain/image.tif";
                        szTwmemref = szFilename + ",TWFF_TIFF,0";
                    }
                    szStatus = "";
                    sts = Send("DG_CONTROL", "DAT_SETUPFILEXFER", "MSG_SET", ref szTwmemref, ref szStatus);
                    szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                    WriteTriplet("DG_CONTROL", "DAT_SETUPFILEXFER", "MSG_SET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));

                    // Transfer stuff...
                    szTwmemref = "";
                    szStatus = "";
                    sts = Send("DG_IMAGE", "DAT_IMAGEFILEXFER", "MSG_GET", ref szTwmemref, ref szStatus);
                    szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                    WriteTriplet("DG_IMAGE", "DAT_IMAGEFILEXFER", "MSG_GET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
                    break;
            }

            // Handle problems...
            if (sts != TWAIN.STS.XFERDONE)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                return;
            }

            // Bump up our image counter, this always grows for the
            // life of the entire session...
            m_iImageCount += 1;

            // Read the file...
            Bitmap bitmap = new Bitmap(szFilename);

            // Display the image...

            // Display the image...
            switch (m_iCurrentPictureBox)
            {
                default:
                case 0:
                    LoadImage(ref m_pictureboxImage1, ref m_graphics1, ref m_bitmapGraphic1, bitmap);
                    break;
                case 1:
                    LoadImage(ref m_pictureboxImage2, ref m_graphics2, ref m_bitmapGraphic2, bitmap);
                    break;
                case 2:
                    LoadImage(ref m_pictureboxImage3, ref m_graphics3, ref m_bitmapGraphic3, bitmap);
                    break;
                case 3:
                    LoadImage(ref m_pictureboxImage4, ref m_graphics4, ref m_bitmapGraphic4, bitmap);
                    break;
                case 4:
                    LoadImage(ref m_pictureboxImage5, ref m_graphics5, ref m_bitmapGraphic5, bitmap);
                    break;
                case 5:
                    LoadImage(ref m_pictureboxImage6, ref m_graphics6, ref m_bitmapGraphic6, bitmap);
                    break;
                case 6:
                    LoadImage(ref m_pictureboxImage7, ref m_graphics7, ref m_bitmapGraphic7, bitmap);
                    break;
                case 7:
                    LoadImage(ref m_pictureboxImage8, ref m_graphics8, ref m_bitmapGraphic8, bitmap);
                    break;
            }

            // Next picture box...
            if (++m_iCurrentPictureBox >= 8)
            {
                m_iCurrentPictureBox = 0;
            }

            // Cleanup...
            bitmap.Dispose();

            // End the transfer...
            szTwmemref = "0,0";
            szStatus = "";
            sts = Send("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", ref szTwmemref, ref szStatus);
            TWAIN.CsvToPendingXfers(ref twpendingxfers, szTwmemref);
            szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
            WriteTriplet("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));

            // Looks like we're done!
            if (twpendingxfers.Count == 0)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                return;
            }
        }

        /// <summary>
        /// Go through the sequence needed to capture images using DAT_IMAGEMEMFILEXFER...
        /// </summary>
        private void CaptureMemfileImages()
        {
            string szTwmemref = "";
            string szStatus = "";
            TWAIN.STS sts;
            TWAIN.TW_IMAGEINFO twimageinfo = default(TWAIN.TW_IMAGEINFO);
            TWAIN.TW_IMAGEMEMXFER twimagememxfer = default(TWAIN.TW_IMAGEMEMXFER);
            TWAIN.TW_PENDINGXFERS twpendingxfers = default(TWAIN.TW_PENDINGXFERS);

            // Dispatch on the state...
            switch (m_twain.GetState())
            {
                // Not a good state, just scoot...
                default:
                    return;

                // We're on our way out...
                case TWAIN.STATE.S5:
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    return;

                // Memory transfers...
                case TWAIN.STATE.S6:
                    // Get the image info...
                    szTwmemref = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
                    szStatus = "";
                    sts = Send("DG_IMAGE", "DAT_IMAGEINFO", "MSG_GET", ref szTwmemref, ref szStatus);
                    TWAIN.CsvToImageinfo(ref twimageinfo, szTwmemref);
                    szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                    WriteTriplet("DG_IMAGE", "DAT_IMAGEINFO", "MSG_GET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));

                    // Pick an image file format...
                    if (twimageinfo.Compression == (ushort)TWAIN.TWCP.JPEG)
                    {
                        szTwmemref = "C:/twain/image.jpg,TWFF_JFIF,0";
                    }
                    else
                    {
                        szTwmemref = "C:/twain/image.tif,TWFF_TIFF,0";
                    }
                    szStatus = "";
                    sts = Send("DG_CONTROL", "DAT_SETUPFILEXFER", "MSG_SET", ref szTwmemref, ref szStatus);
                    szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                    WriteTriplet("DG_CONTROL", "DAT_SETUPFILEXFER", "MSG_SET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));

                    // Transfer stuff...
                    szTwmemref = "0,0,0,0,0,0,0," + ((int)TWAIN.TWMF.APPOWNS | (int)TWAIN.TWMF.POINTER) + "," + m_twsetupmemxfer.Preferred + "," + m_intptrXfer;
                    szStatus = "";
                    sts = Send("DG_IMAGE", "DAT_IMAGEMEMFILEXFER", "MSG_GET", ref szTwmemref, ref szStatus);
                    TWAIN.CsvToImagememxfer(ref twimagememxfer, szTwmemref);
                    szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                    WriteTriplet("DG_IMAGE", "DAT_IMAGEMEMFILEXFER", "MSG_GET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
                    break;

                case TWAIN.STATE.S7:
                    szTwmemref = "0,0,0,0,0,0,0," + ((int)TWAIN.TWMF.APPOWNS | (int)TWAIN.TWMF.POINTER) + "," + m_twsetupmemxfer.Preferred + "," + m_intptrXfer;
                    szStatus = "";
                    sts = Send("DG_IMAGE", "DAT_IMAGEMEMFILEXFER", "MSG_GET", ref szTwmemref, ref szStatus);
                    TWAIN.CsvToImagememxfer(ref twimagememxfer, szTwmemref);
                    szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                    WriteTriplet("DG_IMAGE", "DAT_IMAGEMEMFILEXFER", "MSG_GET", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
                    break;
            }

            // Handle problems...
            if ((sts != TWAIN.STS.SUCCESS) && (sts != TWAIN.STS.XFERDONE))
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                return;
            }

            // Allocate or grow the image memory...
            if (m_intptrImage == IntPtr.Zero)
            {
                m_intptrImage = Marshal.AllocHGlobal((int)twimagememxfer.BytesWritten);
            }
            else
            {
                m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(m_iImageBytes + twimagememxfer.BytesWritten));
            }

            // Ruh-roh...
            if (m_intptrImage == IntPtr.Zero)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                return;
            }

            // Copy into the buffer, and bump up our byte tally...
            TWAIN.MemCpy(m_intptrImage + m_iImageBytes, m_intptrXfer, (int)twimagememxfer.BytesWritten);
            m_iImageBytes += (int)twimagememxfer.BytesWritten;

            // If we saw XFERDONE we can save the image, display it,
            // end the transfer, and see if we have more images...
            if (sts == TWAIN.STS.XFERDONE)
            {
                // Bump up our image counter, this always grows for the
                // life of the entire session...
                m_iImageCount += 1;

                // Save the image to disk, if we're doing that...
                /*
                if (!string.IsNullOrEmpty(m_formsetup.GetImageFolder()))
                {
                    // Create the directory, if needed...
                    if (!Directory.Exists(m_formsetup.GetImageFolder()))
                    {
                        try
                        {
                            Directory.CreateDirectory(m_formsetup.GetImageFolder());
                        }
                        catch (Exception exception)
                        {
                            TWAINWorkingGroup.Log.Error("CreateDirectory failed - " + exception.Message);
                        }
                    }

                    // Write it out...
                    string szFilename = Path.Combine(m_formsetup.GetImageFolder(), "img" + string.Format("{0:D6}", m_iImageCount));
                    TWAIN.WriteImageFile(szFilename, m_intptrImage, m_iImageBytes, out szFilename);
                }
                */

                // Turn the image into a byte array, and free the original memory...
                byte[] abImage = new byte[m_iImageBytes];
                Marshal.Copy(m_intptrImage, abImage, 0, m_iImageBytes);
                Marshal.FreeHGlobal(m_intptrImage);
                m_intptrImage = IntPtr.Zero;
                m_iImageBytes = 0;

                // Turn the byte array into a stream...
                MemoryStream memorystream = new MemoryStream(abImage);
                Bitmap bitmap = (Bitmap)Image.FromStream(memorystream);

                // Display the image...

                // Display the image...
                switch (m_iCurrentPictureBox)
                {
                    default:
                    case 0:
                        LoadImage(ref m_pictureboxImage1, ref m_graphics1, ref m_bitmapGraphic1, bitmap);
                        break;
                    case 1:
                        LoadImage(ref m_pictureboxImage2, ref m_graphics2, ref m_bitmapGraphic2, bitmap);
                        break;
                    case 2:
                        LoadImage(ref m_pictureboxImage3, ref m_graphics3, ref m_bitmapGraphic3, bitmap);
                        break;
                    case 3:
                        LoadImage(ref m_pictureboxImage4, ref m_graphics4, ref m_bitmapGraphic4, bitmap);
                        break;
                    case 4:
                        LoadImage(ref m_pictureboxImage5, ref m_graphics5, ref m_bitmapGraphic5, bitmap);
                        break;
                    case 5:
                        LoadImage(ref m_pictureboxImage6, ref m_graphics6, ref m_bitmapGraphic6, bitmap);
                        break;
                    case 6:
                        LoadImage(ref m_pictureboxImage7, ref m_graphics7, ref m_bitmapGraphic7, bitmap);
                        break;
                    case 7:
                        LoadImage(ref m_pictureboxImage8, ref m_graphics8, ref m_bitmapGraphic8, bitmap);
                        break;
                }

                // Next picture box...
                if (++m_iCurrentPictureBox >= 8)
                {
                    m_iCurrentPictureBox = 0;
                }

                // Cleanup...
                bitmap.Dispose();
                memorystream = null; // disposed by the bitmap
                abImage = null;

                // End the transfer...
                szTwmemref = "0,0";
                szStatus = "";
                sts = Send("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", ref szTwmemref, ref szStatus);
                TWAIN.CsvToPendingXfers(ref twpendingxfers, szTwmemref);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));

                // Looks like we're done!
                if (twpendingxfers.Count == 0)
                {
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    return;
                }
            }
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Functions, miscellaneous functions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Functions...

        /// <summary>
        /// Cleanup, pulled from the designer...
        /// </summary>
        /// <param name="a_blDisposing">true if we need to clean up managed resources</param>
        [SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void Dispose(bool a_blDisposing)
        {
            // Free managed resources...
            if (a_blDisposing)
            {
                // Clean our components...
                if (components != null)
                {
                    components.Dispose();
                }

                // Cleanup...
                if (m_twain != null)
                {
                    m_twain.Dispose();
                    m_twain = null;
                }
                if (m_bitmapGraphic1 != null)
                {
                    m_bitmapGraphic1.Dispose();
                    m_bitmapGraphic1 = null;
                }
                if (m_bitmapGraphic2 != null)
                {
                    m_bitmapGraphic2.Dispose();
                    m_bitmapGraphic2 = null;
                }
                if (m_bitmapGraphic3 != null)
                {
                    m_bitmapGraphic3.Dispose();
                    m_bitmapGraphic3 = null;
                }
                if (m_bitmapGraphic4 != null)
                {
                    m_bitmapGraphic4.Dispose();
                    m_bitmapGraphic4 = null;
                }
                if (m_bitmapGraphic5 != null)
                {
                    m_bitmapGraphic5.Dispose();
                    m_bitmapGraphic5 = null;
                }
                if (m_bitmapGraphic6 != null)
                {
                    m_bitmapGraphic6.Dispose();
                    m_bitmapGraphic6 = null;
                }
                if (m_bitmapGraphic7 != null)
                {
                    m_bitmapGraphic7.Dispose();
                    m_bitmapGraphic7 = null;
                }
                if (m_bitmapGraphic8 != null)
                {
                    m_bitmapGraphic8.Dispose();
                    m_bitmapGraphic8 = null;
                }
                if (m_brushBackground != null)
                {
                    m_brushBackground.Dispose();
                    m_brushBackground = null;
                }
            }

            // Do the base...
            base.Dispose(a_blDisposing);
        }

        /// <summary>
        /// Make sure we clean up on a surprise close...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetMessageFilter(false);
            m_blClosing = true;
            if (m_twain != null)
            {
                Rollback(TWAIN.STATE.S1);
            }
            CleanImage();
            TWAINWorkingGroup.Log.Close();
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
        private void AutoDropdown(string a_szDg, string a_szDat, string a_szMsg)
        {
            // We're initializing...
            if ((a_szDg == "") && (a_szDat == "") && (a_szMsg == ""))
            {
                m_comboboxDG.SelectedItem = "DG_CONTROL";
                m_comboboxDAT.SelectedItem = "DAT_PARENT";
                m_comboboxMSG.SelectedItem = "MSG_OPENDSM";
                m_richtextboxCapability.Text =
                    "TWAIN Working Group," +
                    "TWAIN Sharp," +
                    "TWAIN Sharp Test App," +
                    "2," +
                    "4," +
                    "0x20000003," +
                    "USA," +
                    "testing...," +
                    "ENGLISH_USA";
            }

            // After OPENDSM we have a choice, based on the DSM that was used...
            else if ((a_szDg == "DG_CONTROL") && (a_szDat == "DAT_PARENT") && (a_szMsg == "MSG_OPENDSM"))
            {
                string[] aszAppIdentity = CSV.Parse(m_twain.GetAppIdentity());
                int iSupportedGroups;
                int.TryParse(aszAppIdentity[8], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iSupportedGroups);
                if ((iSupportedGroups & ((int)TWAIN.DG.DSM2 | (int)TWAIN.DG.APP2)) == ((int)TWAIN.DG.DSM2 | (int)TWAIN.DG.APP2))
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
                m_richtextboxCapability.Text = "";
            }

            // After an ENTRYPOINT, switch to GETFIRST...
            else if ((a_szDg == "DG_CONTROL") && (a_szDat == "DAT_ENTRYPOINT") && (a_szMsg == "MSG_GET"))
            {
                m_comboboxDG.SelectedItem = "DG_CONTROL";
                m_comboboxDAT.SelectedItem = "DAT_IDENTITY";
                m_comboboxMSG.SelectedItem = "MSG_GETFIRST";
                m_richtextboxCapability.Text = "";
            }

            // When we CLOSEDSM, go back to OPENDSM...
            else if ((a_szDg == "DG_CONTROL") && (a_szDat == "DAT_PARENT") && (a_szMsg == "MSG_CLOSEDSM"))
            {
                m_comboboxDG.SelectedItem = "DG_CONTROL";
                m_comboboxDAT.SelectedItem = "DAT_PARENT";
                m_comboboxMSG.SelectedItem = "MSG_OPENDSM";
                m_richtextboxCapability.Text =
                    "TWAIN Working Group," +
                    "TWAIN Sharp," +
                    "TWAIN Sharp Test App," +
                    "2," +
                    "4," +
                    "0x20000003," +
                    "USA," +
                    "testing...," +
                    "ENGLISH_USA";
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
                m_richtextboxCapability.Text = "";
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
            if (   (szDg == "DG_CONTROL")
                && (szDat == "DAT_PARENT")
                && ((szMsg == "MSG_OPENDSM") || (szMsg == "MSG_CLOSEDSM")))
            {
                ManageTWAIN(szDg, szDat, szMsg);
                return;
            }

            // Validate...
            if (m_twain == null)
            {
                WriteTriplet(szDg, szDat, szMsg, "(DSM is not open)");
                return;
            }

            // Look for an @-command...
            /*
            if (m_twaincstoolkit.AtCommand(m_richtextboxCapability.Text))
            {
                return;
            }
            */

            // Everything else can go to TWAIN...
            string szResult;
            string szTwmemref;
            TWAIN.STS sts;

            // Grab the data from the cap box...
            szTwmemref = m_richtextboxCapability.Text;

            // Issue the command to TWAIN...
            szResult = "";
            sts = Send(szDg, szDat, szMsg, ref szTwmemref, ref szResult);

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
            if (sts == TWAIN.STS.SUCCESS)
            {
                AutoDropdown(szDg, szDat, szMsg);
            }
        }

        /// <summary>
        /// Send a command to the currently loaded DSM...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private TWAIN.STS Send(string a_szDg, string a_szDat, string a_szMsg, ref string a_szTwmemref, ref string a_szResult)
        {
            int iDg;
            int iDat;
            int iMsg;
            TWAIN.STS sts;
            TWAIN.DG dg = TWAIN.DG.MASK;
            TWAIN.DAT dat = TWAIN.DAT.NULL;
            TWAIN.MSG msg = TWAIN.MSG.NULL;

            // Init stuff...
            iDg = 0;
            iDat = 0;
            iMsg = 0;
            sts = TWAIN.STS.BADPROTOCOL;
            a_szResult = "";

            // Validate at the top level...
            if (m_twain == null)
            {
                WriteOutput("***ERROR*** - dsmload wasn't run, so we is having no braims");
                return (TWAIN.STS.SEQERROR);
            }

            // Look for DG...
            if (!a_szDg.ToLowerInvariant().StartsWith("dg_"))
            {
                WriteOutput("Unrecognized dg - <" + a_szDg + ">");
                return (TWAIN.STS.BADPROTOCOL);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_szDg.ToLowerInvariant().StartsWith("dg_0x"))
                {
                    if (!int.TryParse(a_szDg.ToLowerInvariant().Substring(3), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iDg))
                    {
                        WriteOutput("Badly constructed dg - <" + a_szDg + ">");
                        return (TWAIN.STS.BADPROTOCOL);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_szDg.ToUpperInvariant().Substring(3), out dg))
                    {
                        WriteOutput("Unrecognized dg - <" + a_szDg + ">");
                        return (TWAIN.STS.BADPROTOCOL);
                    }
                    iDg = (int)dg;
                }
            }

            // Look for DAT...
            if (!a_szDat.ToLowerInvariant().StartsWith("dat_"))
            {
                WriteOutput("Unrecognized dat - <" + a_szDat + ">");
                return (TWAIN.STS.BADPROTOCOL);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_szDat.ToLowerInvariant().StartsWith("dat_0x"))
                {
                    if (!int.TryParse(a_szDat.ToLowerInvariant().Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iDat))
                    {
                        WriteOutput("Badly constructed dat - <" + a_szDat + ">");
                        return (TWAIN.STS.BADPROTOCOL);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_szDat.ToUpperInvariant().Substring(4), out dat))
                    {
                        WriteOutput("Unrecognized dat - <" + a_szDat + ">");
                        return (TWAIN.STS.BADPROTOCOL);
                    }
                    iDat = (int)dat;
                }
            }

            // Look for MSG...
            if (!a_szMsg.ToLowerInvariant().StartsWith("msg_"))
            {
                WriteOutput("Unrecognized msg - <" + a_szMsg + ">");
                return (TWAIN.STS.BADPROTOCOL);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_szMsg.ToLowerInvariant().StartsWith("msg_0x"))
                {
                    if (!int.TryParse(a_szMsg.ToLowerInvariant().Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iMsg))
                    {
                        WriteOutput("Badly constructed dat - <" + a_szMsg + ">");
                        return (TWAIN.STS.BADPROTOCOL);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_szMsg.ToUpperInvariant().Substring(4), out msg))
                    {
                        WriteOutput("Unrecognized msg - <" + a_szMsg + ">");
                        return (TWAIN.STS.BADPROTOCOL);
                    }
                    iMsg = (int)msg;
                }
            }

            // Send the command...
            switch (iDat)
            {
                // Ruh-roh, since we can't marshal it, we have to return an error,
                // it would be nice to have a solution for this, but that will need
                // a dynamic marshalling system...
                default:
                    sts = TWAIN.STS.BADPROTOCOL;
                    break;

                // DAT_AUDIOFILEXFER...
                case (int)TWAIN.DAT.AUDIOFILEXFER:
                    {
                        sts = m_twain.DatAudiofilexfer((TWAIN.DG)iDg, (TWAIN.MSG)iMsg);
                        a_szTwmemref = "";
                    }
                    break;

                // DAT_AUDIOINFO..
                case (int)TWAIN.DAT.AUDIOINFO:
                    {
                        TWAIN.TW_AUDIOINFO twaudioinfo = default(TWAIN.TW_AUDIOINFO);
                        sts = m_twain.DatAudioinfo((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twaudioinfo);
                        a_szTwmemref = TWAIN.AudioinfoToCsv(twaudioinfo);
                    }
                    break;

                // DAT_AUDIONATIVEXFER..
                case (int)TWAIN.DAT.AUDIONATIVEXFER:
                    {
                        IntPtr intptr = IntPtr.Zero;
                        sts = m_twain.DatAudionativexfer((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref intptr);
                        a_szTwmemref = intptr.ToString();
                    }
                    break;

                // DAT_CALLBACK...
                case (int)TWAIN.DAT.CALLBACK:
                    {
                        TWAIN.TW_CALLBACK twcallback = default(TWAIN.TW_CALLBACK);
                        TWAIN.CsvToCallback(ref twcallback, a_szTwmemref);
                        sts = m_twain.DatCallback((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twcallback);
                        a_szTwmemref = TWAIN.CallbackToCsv(twcallback);
                    }
                    break;

                // DAT_CALLBACK2...
                case (int)TWAIN.DAT.CALLBACK2:
                    {
                        TWAIN.TW_CALLBACK2 twcallback2 = default(TWAIN.TW_CALLBACK2);
                        TWAIN.CsvToCallback2(ref twcallback2, a_szTwmemref);
                        sts = m_twain.DatCallback2((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twcallback2);
                        a_szTwmemref = TWAIN.Callback2ToCsv(twcallback2);
                    }
                    break;

                // DAT_CAPABILITY...
                case (int)TWAIN.DAT.CAPABILITY:
                    {
                        // Skip symbols for msg_querysupport, otherwise 0 gets turned into false, also
                        // if the command fails the return value is whatever was sent into us, which
                        // matches the experience one should get with C/C++...
                        string szStatus = "";
                        TWAIN.TW_CAPABILITY twcapability = default(TWAIN.TW_CAPABILITY);
                        m_twain.CsvToCapability(ref twcapability, ref szStatus, a_szTwmemref);
                        sts = m_twain.DatCapability((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twcapability);
                        if ((sts == TWAIN.STS.SUCCESS) || (sts == TWAIN.STS.CHECKSTATUS))
                        {
                            // Convert the data to CSV...
                            a_szTwmemref = m_twain.CapabilityToCsv(twcapability, ((TWAIN.MSG)iMsg != TWAIN.MSG.QUERYSUPPORT));
                            // Free the handle if the driver created it...
                            switch ((TWAIN.MSG)iMsg)
                            {
                                default: break;
                                case TWAIN.MSG.GET:
                                case TWAIN.MSG.GETCURRENT:
                                case TWAIN.MSG.GETDEFAULT:
                                case TWAIN.MSG.QUERYSUPPORT:
                                case TWAIN.MSG.RESET:
                                    m_twain.DsmMemFree(ref twcapability.hContainer);
                                    break;
                            }
                        }
                    }
                    break;

                // DAT_CIECOLOR..
                case (int)TWAIN.DAT.CIECOLOR:
                    {
                        //TWAIN.TW_CIECOLOR twciecolor = default(TWAIN.TW_CIECOLOR);
                        //sts = m_twain.DatCiecolor((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twciecolor);
                        //a_szTwmemref = m_twain.CiecolorToCsv(twciecolor);
                    }
                    break;

                // DAT_CUSTOMDSDATA...
                case (int)TWAIN.DAT.CUSTOMDSDATA:
                    {
                        TWAIN.TW_CUSTOMDSDATA twcustomdsdata = default(TWAIN.TW_CUSTOMDSDATA);
                        m_twain.CsvToCustomdsdata(ref twcustomdsdata, a_szTwmemref);
                        sts = m_twain.DatCustomdsdata((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twcustomdsdata);
                        a_szTwmemref = m_twain.CustomdsdataToCsv(twcustomdsdata);
                    }
                    break;

                // DAT_DEVICEEVENT...
                case (int)TWAIN.DAT.DEVICEEVENT:
                    {
                        TWAIN.TW_DEVICEEVENT twdeviceevent = default(TWAIN.TW_DEVICEEVENT);
                        sts = m_twain.DatDeviceevent((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twdeviceevent);
                        a_szTwmemref = TWAIN.DeviceeventToCsv(twdeviceevent);
                    }
                    break;

                // DAT_ENTRYPOINT...
                case (int)TWAIN.DAT.ENTRYPOINT:
                    {
                        TWAIN.TW_ENTRYPOINT twentrypoint = default(TWAIN.TW_ENTRYPOINT);
                        twentrypoint.Size = (uint)Marshal.SizeOf(twentrypoint);
                        sts = m_twain.DatEntrypoint((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twentrypoint);
                        a_szTwmemref = TWAIN.EntrypointToCsv(twentrypoint);
                    }
                    break;

                // DAT_EVENT...
                case (int)TWAIN.DAT.EVENT:
                    {
                        TWAIN.TW_EVENT twevent = default(TWAIN.TW_EVENT);
                        sts = m_twain.DatEvent((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twevent);
                        a_szTwmemref = TWAIN.EventToCsv(twevent);
                    }
                    break;

                // DAT_EXTIMAGEINFO...
                case (int)TWAIN.DAT.EXTIMAGEINFO:
                    {
                        TWAIN.TW_EXTIMAGEINFO twextimageinfo = default(TWAIN.TW_EXTIMAGEINFO);
                        TWAIN.CsvToExtimageinfo(ref twextimageinfo, a_szTwmemref);
                        sts = m_twain.DatExtimageinfo((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twextimageinfo);
                        a_szTwmemref = TWAIN.ExtimageinfoToCsv(twextimageinfo);
                    }
                    break;

                // DAT_FILESYSTEM...
                case (int)TWAIN.DAT.FILESYSTEM:
                    {
                        TWAIN.TW_FILESYSTEM twfilesystem = default(TWAIN.TW_FILESYSTEM);
                        TWAIN.CsvToFilesystem(ref twfilesystem, a_szTwmemref);
                        sts = m_twain.DatFilesystem((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twfilesystem);
                        a_szTwmemref = TWAIN.FilesystemToCsv(twfilesystem);
                    }
                    break;

                // DAT_FILTER...
                case (int)TWAIN.DAT.FILTER:
                    {
                        //TWAIN.TW_FILTER twfilter = default(TWAIN.TW_FILTER);
                        //m_twain.CsvToFilter(ref twfilter, a_szTwmemref);
                        //sts = m_twain.DatFilter((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twfilter);
                        //a_szTwmemref = m_twain.FilterToCsv(twfilter);
                    }
                    break;

                // DAT_GRAYRESPONSE...
                case (int)TWAIN.DAT.GRAYRESPONSE:
                    {
                        //TWAIN.TW_GRAYRESPONSE twgrayresponse = default(TWAIN.TW_GRAYRESPONSE);
                        //m_twain.CsvToGrayresponse(ref twgrayresponse, a_szTwmemref);
                        //sts = m_twain.DatGrayresponse((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twgrayresponse);
                        //a_szTwmemref = m_twain.GrayresponseToCsv(twgrayresponse);
                    }
                    break;

                // DAT_ICCPROFILE...
                case (int)TWAIN.DAT.ICCPROFILE:
                    {
                        TWAIN.TW_MEMORY twmemory = default(TWAIN.TW_MEMORY);
                        sts = m_twain.DatIccprofile((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twmemory);
                        a_szTwmemref = TWAIN.IccprofileToCsv(twmemory);
                    }
                    break;

                // DAT_IDENTITY...
                case (int)TWAIN.DAT.IDENTITY:
                    {
                        TWAIN.TW_IDENTITY twidentity = default(TWAIN.TW_IDENTITY);
                        switch (iMsg)
                        {
                            default:
                                break;
                            case (int)TWAIN.MSG.SET:
                            case (int)TWAIN.MSG.OPENDS:
                                TWAIN.CsvToIdentity(ref twidentity, a_szTwmemref);
                                break;
                        }
                        sts = m_twain.DatIdentity((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twidentity);
                        a_szTwmemref = TWAIN.IdentityToCsv(twidentity);
                    }
                    break;

                // DAT_IMAGEFILEXFER...
                case (int)TWAIN.DAT.IMAGEFILEXFER:
                    {
                        sts = m_twain.DatImagefilexfer((TWAIN.DG)iDg, (TWAIN.MSG)iMsg);
                        a_szTwmemref = "";
                    }
                    break;

                // DAT_IMAGEINFO...
                case (int)TWAIN.DAT.IMAGEINFO:
                    {
                        TWAIN.TW_IMAGEINFO twimageinfo = default(TWAIN.TW_IMAGEINFO);
                        TWAIN.CsvToImageinfo(ref twimageinfo, a_szTwmemref);
                        sts = m_twain.DatImageinfo((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twimageinfo);
                        a_szTwmemref = TWAIN.ImageinfoToCsv(twimageinfo);
                    }
                    break;

                // DAT_IMAGELAYOUT...
                case (int)TWAIN.DAT.IMAGELAYOUT:
                    {
                        TWAIN.TW_IMAGELAYOUT twimagelayout = default(TWAIN.TW_IMAGELAYOUT);
                        TWAIN.CsvToImagelayout(ref twimagelayout, a_szTwmemref);
                        sts = m_twain.DatImagelayout((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twimagelayout);
                        a_szTwmemref = TWAIN.ImagelayoutToCsv(twimagelayout);
                    }
                    break;

                // DAT_IMAGEMEMFILEXFER...
                case (int)TWAIN.DAT.IMAGEMEMFILEXFER:
                    {
                        TWAIN.TW_IMAGEMEMXFER twimagememxfer = default(TWAIN.TW_IMAGEMEMXFER);
                        TWAIN.CsvToImagememxfer(ref twimagememxfer, a_szTwmemref);
                        sts = m_twain.DatImagememfilexfer((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twimagememxfer);
                        a_szTwmemref = TWAIN.ImagememxferToCsv(twimagememxfer);
                    }
                    break;

                // DAT_IMAGEMEMXFER...
                case (int)TWAIN.DAT.IMAGEMEMXFER:
                    {
                        TWAIN.TW_IMAGEMEMXFER twimagememxfer = default(TWAIN.TW_IMAGEMEMXFER);
                        TWAIN.CsvToImagememxfer(ref twimagememxfer, a_szTwmemref);
                        sts = m_twain.DatImagememxfer((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twimagememxfer);
                        a_szTwmemref = TWAIN.ImagememxferToCsv(twimagememxfer);
                    }
                    break;

                // DAT_IMAGENATIVEXFER...
                case (int)TWAIN.DAT.IMAGENATIVEXFER:
                    {
                        IntPtr intptrBitmapHandle = IntPtr.Zero;
                        sts = m_twain.DatImagenativexferHandle((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref intptrBitmapHandle);
                        a_szTwmemref = intptrBitmapHandle.ToString();
                    }
                    break;

                // DAT_JPEGCOMPRESSION...
                case (int)TWAIN.DAT.JPEGCOMPRESSION:
                    {
                        //TWAIN.TW_JPEGCOMPRESSION twjpegcompression = default(TWAIN.TW_JPEGCOMPRESSION);
                        //m_twain.CsvToJpegcompression(ref twjpegcompression, a_szTwmemref);
                        //sts = m_twain.DatJpegcompression((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twjpegcompression);
                        //a_szTwmemref = m_twain.JpegcompressionToCsv(twjpegcompression);
                    }
                    break;

                // DAT_METRICS...
                case (int)TWAIN.DAT.METRICS:
                    {
                        TWAIN.TW_METRICS twmetrics = default(TWAIN.TW_METRICS);
                        twmetrics.SizeOf = (uint)Marshal.SizeOf(twmetrics);
                        sts = m_twain.DatMetrics((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twmetrics);
                        a_szTwmemref = TWAIN.MetricsToCsv(twmetrics);
                    }
                    break;

                // DAT_PALETTE8...
                case (int)TWAIN.DAT.PALETTE8:
                    {
                        //TWAIN.TW_PALETTE8 twpalette8 = default(TWAIN.TW_PALETTE8);
                        //m_twain.CsvToPalette8(ref twpalette8, a_szTwmemref);
                        //sts = m_twain.DatPalette8((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twpalette8);
                        //a_szTwmemref = m_twain.Palette8ToCsv(twpalette8);
                    }
                    break;

                // DAT_PARENT...
                case (int)TWAIN.DAT.PARENT:
                    {
                        sts = m_twain.DatParent((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref m_intptrHwnd);
                        a_szTwmemref = "";
                    }
                    break;

                // DAT_PASSTHRU...
                case (int)TWAIN.DAT.PASSTHRU:
                    {
                        TWAIN.TW_PASSTHRU twpassthru = default(TWAIN.TW_PASSTHRU);
                        TWAIN.CsvToPassthru(ref twpassthru, a_szTwmemref);
                        sts = m_twain.DatPassthru((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twpassthru);
                        a_szTwmemref = TWAIN.PassthruToCsv(twpassthru);
                    }
                    break;

                // DAT_PENDINGXFERS...
                case (int)TWAIN.DAT.PENDINGXFERS:
                    {
                        TWAIN.TW_PENDINGXFERS twpendingxfers = default(TWAIN.TW_PENDINGXFERS);
                        sts = m_twain.DatPendingxfers((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twpendingxfers);
                        a_szTwmemref = TWAIN.PendingxfersToCsv(twpendingxfers);
                    }
                    break;

                // DAT_RGBRESPONSE...
                case (int)TWAIN.DAT.RGBRESPONSE:
                    {
                        //TWAIN.TW_RGBRESPONSE twrgbresponse = default(TWAIN.TW_RGBRESPONSE);
                        //m_twain.CsvToRgbresponse(ref twrgbresponse, a_szTwmemref);
                        //sts = m_twain.DatRgbresponse((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twrgbresponse);
                        //a_szTwmemref = m_twain.RgbresponseToCsv(twrgbresponse);
                    }
                    break;

                // DAT_SETUPFILEXFER...
                case (int)TWAIN.DAT.SETUPFILEXFER:
                    {
                        TWAIN.TW_SETUPFILEXFER twsetupfilexfer = default(TWAIN.TW_SETUPFILEXFER);
                        TWAIN.CsvToSetupfilexfer(ref twsetupfilexfer, a_szTwmemref);
                        sts = m_twain.DatSetupfilexfer((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twsetupfilexfer);
                        a_szTwmemref = TWAIN.SetupfilexferToCsv(twsetupfilexfer);
                    }
                    break;

                // DAT_SETUPMEMXFER...
                case (int)TWAIN.DAT.SETUPMEMXFER:
                    {
                        TWAIN.TW_SETUPMEMXFER twsetupmemxfer = default(TWAIN.TW_SETUPMEMXFER);
                        sts = m_twain.DatSetupmemxfer((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twsetupmemxfer);
                        a_szTwmemref = TWAIN.SetupmemxferToCsv(twsetupmemxfer);
                    }
                    break;

                // DAT_STATUS...
                case (int)TWAIN.DAT.STATUS:
                    {
                        TWAIN.TW_STATUS twstatus = default(TWAIN.TW_STATUS);
                        sts = m_twain.DatStatus((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twstatus);
                        a_szTwmemref = TWAIN.StatusToCsv(twstatus);
                    }
                    break;

                // DAT_STATUSUTF8...
                case (int)TWAIN.DAT.STATUSUTF8:
                    {
                        TWAIN.TW_STATUSUTF8 twstatusutf8 = default(TWAIN.TW_STATUSUTF8);
                        sts = m_twain.DatStatusutf8((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twstatusutf8);
                        a_szTwmemref = m_twain.Statusutf8ToCsv(twstatusutf8);
                    }
                    break;

                // DAT_TWAINDIRECT...
                case (int)TWAIN.DAT.TWAINDIRECT:
                    {
                        TWAIN.TW_TWAINDIRECT twtwaindirect = default(TWAIN.TW_TWAINDIRECT);
                        TWAIN.CsvToTwaindirect(ref twtwaindirect, a_szTwmemref);
                        sts = m_twain.DatTwaindirect((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twtwaindirect);
                        a_szTwmemref = TWAIN.TwaindirectToCsv(twtwaindirect);
                    }
                    break;

                // DAT_USERINTERFACE...
                case (int)TWAIN.DAT.USERINTERFACE:
                    {
                        TWAIN.TW_USERINTERFACE twuserinterface = default(TWAIN.TW_USERINTERFACE);
                        m_twain.CsvToUserinterface(ref twuserinterface, a_szTwmemref);
                        sts = m_twain.DatUserinterface((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref twuserinterface);
                        a_szTwmemref = TWAIN.UserinterfaceToCsv(twuserinterface);
                    }
                    break;

                // DAT_XFERGROUP...
                case (int)TWAIN.DAT.XFERGROUP:
                    {
                        uint uXferGroup = 0;
                        sts = m_twain.DatXferGroup((TWAIN.DG)iDg, (TWAIN.MSG)iMsg, ref uXferGroup);
                        a_szTwmemref = string.Format("0x{0:X}", uXferGroup);
                    }
                    break;
            }

            // All done...
            return (sts);
        }

        /// <summary>
        /// Create and destroy TWAIN, as needed...
        /// </summary>
        /// <param name="a_szDg">Data group</param>
        /// <param name="a_szDat">Data argument type</param>
        /// <param name="a_szMsg">Operation</param>
        private void ManageTWAIN(string a_szDg, string a_szDat, string a_szMsg)
        {
            TWAIN.RunInUiThreadDelegate runinuithreaddelegate = RunInUiThread;

            // Handle MSG_OPENDSM...
            if (a_szMsg == "MSG_OPENDSM")
            {
                // Init stuff...
                m_blClosing = false;

                // Validate...
                if (m_twain == null)
                {
                    try
                    {
                        string[] aszTwidentity = m_richtextboxCapability.Text.Split(',');
                        if ((aszTwidentity == null) || (aszTwidentity.Length < 9))
                        {
                            m_twain = new TWAIN
                            (
                                "TWAIN Working Group",
                                "TWAIN Open Source",
                                "TWAIN CS Test",
                                (ushort)TWAIN.TWON_PROTOCOL.MAJOR,
                                (ushort)TWAIN.TWON_PROTOCOL.MINOR,
                                (uint)(TWAIN.DG.APP2 | TWAIN.DG.CONTROL | TWAIN.DG.IMAGE),
                                TWAIN.TWCY.USA,
                                "TWAIN CS Test",
                                TWAIN.TWLG.ENGLISH_USA,
                                2,
                                4,
                                m_checkboxUseTwain32.Checked,
                                m_checkboxUseCallbacks.Checked,
                                DeviceEventCallback,
                                ScanCallbackTrigger,
                                runinuithreaddelegate,
                                this.Handle
                            );

                            // Prep for TWAIN events...
                            SetMessageFilter(true);

                        }
                        else
                        {
                            TWAIN.TWCY twcy = TWAIN.TWCY.USA;
                            TWAIN.TWLG twlg = TWAIN.TWLG.ENGLISH_USA;
                            Enum.TryParse<TWAIN.TWCY>(aszTwidentity[6], out twcy);
                            Enum.TryParse<TWAIN.TWLG>(aszTwidentity[8], out twlg);
                            m_twain = new TWAIN
                            (
                                aszTwidentity[0],
                                aszTwidentity[1],
                                aszTwidentity[2],
                                ushort.Parse(aszTwidentity[3]),
                                ushort.Parse(aszTwidentity[4]),
                                (uint)(TWAIN.DG.APP2 | TWAIN.DG.CONTROL | TWAIN.DG.IMAGE),
                                twcy,
                                aszTwidentity[7],
                                twlg,
                                2,
                                4,
                                m_checkboxUseTwain32.Checked,
                                m_checkboxUseCallbacks.Checked,
                                DeviceEventCallback,
                                ScanCallbackTrigger,
                                runinuithreaddelegate,
                                this.Handle
                            );

                            // Prep for TWAIN events...
                            SetMessageFilter(true);
                        }
                    }
                    catch (Exception exception)
                    {
                        TWAINWorkingGroup.Log.Error("exception - " + exception.Message);
                        WriteTriplet(a_szDg, a_szDat, a_szMsg, "(unable to start, clear the data window and try again)");
                        m_twain = null;
                        return;
                    }
                }

                // Open the DSM...
                string szTwmemref = m_intptrHwnd.ToString();
                string szStatus = "";
                TWAIN.STS sts = Send("DG_CONTROL", "DAT_PARENT", "MSG_OPENDSM", ref szTwmemref, ref szStatus);
                WriteTriplet("DG_CONTROL", "DAT_PARENT", "MSG_OPENDSM", sts.ToString() + Environment.NewLine + m_intptrHwnd.ToString());

                // Fix our controls...
                if (TWAIN.GetPlatform() == TWAIN.Platform.WINDOWS)
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
                m_blClosing = true;
                Rollback(TWAIN.STATE.S1);

                // Fix our controls...
                if (TWAIN.GetPlatform() == TWAIN.Platform.WINDOWS)
                {
                    m_checkboxUseTwain32.Enabled = (TWAIN.GetMachineWordBitSize() == 32);
                    m_checkboxUseCallbacks.Enabled = true;
                }

                // Help the user...
                AutoDropdown(a_szDg, a_szDat, a_szMsg);
            }

            // Handle anything else...
            else
            {
                WriteTriplet(a_szDg, a_szDat, a_szMsg, TWAIN.STS.BADPROTOCOL.ToString());
            }
        }

        /// <summary>
        /// Our callback for device events.  This is where we catch and
        /// report that a device event has been detected.  Obviously,
        /// we're not doing much with it.  A real application would
        /// probably take some kind of action...
        /// </summary>
        /// <returns>TWAIN status</returns>
        private TWAIN.STS DeviceEventCallback()
        {
            string szTwmemref = "";
            string szStatus = "";
            TWAIN.STS sts;
            TWAIN.TW_DEVICEEVENT twdeviceevent;

            // Drain the event queue...
            while (true)
            {
                // Try to get an event...
                twdeviceevent = default(TWAIN.TW_DEVICEEVENT);
                szTwmemref = TWAIN.DeviceeventToCsv(twdeviceevent);
                sts = Send("DG_CONTROL", "DAT_DEVICEEVENT", "MSG_GET", ref szTwmemref, ref szStatus);
                if (sts != TWAIN.STS.SUCCESS)
                {
                    break;
                }

                // Report on what we got...
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteOutput("*** DeviceEvent ***" + Environment.NewLine);
                WriteTriplet("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_ENDXFER", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
            }

            // Return a status, in case we ever need it for anything...
            return (TWAIN.STS.SUCCESS);
        }

        /// <summary>
        /// TWAIN needs help, if we want it to run stuff in our main
        /// UI thread...
        /// </summary>
        /// <param name="control">the control to run in</param>
        /// <param name="code">the code to run</param>
        public void RunInUiThread(Action a_action)
        {
            RunInUiThread(this, a_action);
        }
        static public void RunInUiThread(Object a_object, Action a_action)
        {
            Control control = (Control)a_object;
            if (control.InvokeRequired)
            {
                control.Invoke(new FormMain.RunInUiThreadDelegate(RunInUiThread), new object[] { a_object, a_action });
                return;
            }
            a_action();
        }

        /// <summary>
        /// Close the Data Source...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_dat">Data argument type</param>
        /// <param name="a_msg">Operation</param>
        private void m_buttonCloseDS_Click(string a_szDg, string a_szDat, string a_szMsg)
        {
            if (m_twain != null)
            {
                Rollback(TWAIN.STATE.S3);
            }
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
            TWAIN.STS sts;
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
            ClearEvents();
            szTwmemref = "0,0," + m_intptrHwnd;
            szStatus = "";
            sts = Send("DG_CONTROL", "DAT_USERINTERFACE", "MSG_ENABLEDS", ref szTwmemref, ref szStatus);
            WriteTriplet("DG_CONTROL", "DAT_USERINTERFACE", "MSG_ENABLEDS", sts.ToString());
        }

        /// <summary>
        /// Clear our event list, and reset our event...
        /// </summary>

        public void ClearEvents()
        {
            m_blXferReadySent = false;
            m_blDisableDsSent = false;
        }

        /// <summary>
        /// Stop scanning...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void m_buttonStop_Click(object sender, EventArgs e)
        {
            if (m_twain != null)
            {
                string szTwmemref = "0,0";
                string szStatus = "";
                TWAIN.STS sts = Send("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_STOPFEEDER", ref szTwmemref, ref szStatus);
                szStatus = (szStatus == "") ? sts.ToString() : (sts.ToString() + " - " + szStatus);
                WriteTriplet("DG_CONTROL", "DAT_PENDINGXFERS", "MSG_STOPFEEDER", szStatus + ((szTwmemref == "") ? "" : (Environment.NewLine + szTwmemref)));
            }
        }

        /// <summary>
        /// React to a change to the selection of TWAIN_32.DLL...
        /// </summary>
        /// <param name="sender">Originator</param>
        /// <param name="e">Arguments</param>
        private void m_checkboxUseTwain32_CheckedChanged(object sender, EventArgs e)
        {
            // TWAIN_32.DLL om Windows doesn't support callbacks, but the
            // legacy stuff on Linux and MacOsX does; in fact that's all
            // they support...
            if (m_checkboxUseTwain32.Checked)
            {
                if (TWAIN.GetPlatform() == TWAIN.Platform.WINDOWS)
                {
                    m_checkboxUseCallbacks.Checked = false;
                }
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
        /// Our interface to TWAIN...
        /// </summary>
        private TWAIN m_twain;
        private IntPtr m_intptrHwnd;
        private bool m_blDisableDsSent = false;
        private bool m_blXferReadySent = false;
        private IntPtr m_intptrXfer = IntPtr.Zero;
        private IntPtr m_intptrImage = IntPtr.Zero;
        private int m_iImageBytes = 0;
        private TWAIN.TWSX m_twsxXferMech;
        private TWAIN.TW_SETUPMEMXFER m_twsetupmemxfer;

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
        private int m_iImageCount = 0;

        /// <summary>
        /// We use this to run code in the context of the caller's UI thread...
        /// </summary>
        /// <param name="a_object">object (really a control)</param>
        /// <param name="a_action">code to run</param>
        public delegate void RunInUiThreadDelegate(Object a_object, Action a_action);

        #endregion
    }
}
