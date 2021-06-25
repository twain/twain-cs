using System;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using TWAINWorkingGroup;

namespace twaincscert
{
    public partial class FormMain : Form, IMessageFilter
    {
        /// <summary>
        /// Our constructor...
        /// </summary>
        public FormMain()
        {
            // Not much going on here, because we never show this form...
            InitializeComponent();
            SetMessageFilter(true);
        }

        /// <summary>
        /// Prevent us from getting the focus, so we don't steal it from
        /// our console...
        /// </summary>
        protected override bool ShowWithoutActivation
        {
            get
            {
                return (true);
            }
        }

        /// <summary>
        /// Monitor for DG_CONTROL / DAT_NULL / MSG_* stuff (ex MSG_XFERREADY), this
        /// function is only triggered when SetMessageFilter() is called with 'true'...
        /// </summary>
        /// <param name="a_message">Message to process</param>
        /// <returns>Result of the processing</returns>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool PreFilterMessage(ref Message a_message)
        {
            if (m_terminal != null)
            {
                return (m_terminal.PreFilterMessage(a_message.HWnd, a_message.Msg, a_message.WParam, a_message.LParam));
            }
            return (true);
        }

        /// <summary>
        /// Turn message filtering on or off, we use this to capture stuff
        /// like MSG_XFERREADY.  If it's off, then it's assumed we're getting
        /// this info through DAT_CALLBACK2...
        /// </summary>
        /// <param name="a_blAdd">True to turn it on</param>
        public void SetMessageFilter(bool a_blAdd)
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
        /// Link us to our terminal object...
        /// </summary>
        /// <param name="a_terminal">our terminal object</param>
        public void SetTerminal(Terminal a_terminal)
        {
            m_terminal = a_terminal;
        }

        /// <summary>
        /// Our event handler for the scan callback event.  This will be
        /// called once by ScanCallbackTrigger on receipt of an event
        /// like MSG_XFERREADY, and then will be reissued on every call
        /// into ScanCallback until we're done and get back to state 4.
        ///  
        /// This helps to make sure we're always running in the context
        /// of FormMain on Windows, which is critical if we want drivers
        /// to work properly.  It also gives a way to break up the calls
        /// so the message pump is still responsive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ScanCallbackEventHandler(object sender, EventArgs e)
        {
            m_terminal.ScanCallback((m_terminal.GetTwain() == null) ? true : (m_terminal.GetTwain().GetState() <= TWAIN.STATE.S3));
        }

        /// <summary>
        /// Private attributes...
        /// </summary>
        private Terminal m_terminal = null;
    }
}
