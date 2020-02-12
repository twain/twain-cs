using System;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

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

            // We need this for Windows.  This is where we filter for
            // Windows events.  This is needed for two reasons:  first,
            // so that events targeting the driver's userinterface can
            // be properly processed.  And second, so that we can detect
            // DAT_NULL events and act on them...
            SetMessageFilter(true);
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
                return (m_terminal.PreFilterMessage(a_message.HWnd,a_message.Msg,a_message.WParam,a_message.LParam));
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
        /// Private attributes...
        /// </summary>
        private Terminal m_terminal = null;
    }
}
