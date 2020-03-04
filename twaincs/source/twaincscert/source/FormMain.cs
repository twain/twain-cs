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
        /// TWAIN needs help, if we want it to run stuff in our main
        /// UI thread...
        /// </summary>
        /// <param name="control">the control to run in</param>
        /// <param name="code">the code to run</param>
        static public void RunInUiThread(Object a_object, Action a_action)
        {
            Control control = (Control)a_object;
            if (control.InvokeRequired)
            {
                control.Invoke(new Terminal.RunInUiThreadDelegate(RunInUiThread), new object[] { a_object, a_action });
                return;
            }
            a_action();
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
