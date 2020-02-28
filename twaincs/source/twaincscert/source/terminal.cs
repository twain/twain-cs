///////////////////////////////////////////////////////////////////////////////////////
//
//  twaincscert.Program
//
//  Our entry point.
//
///////////////////////////////////////////////////////////////////////////////////////
//  Author          Date            Comment
//  M.McLaughlin    01-Jan-2020     Initial Release
///////////////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2020-2020 Kodak Alaris Inc.
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

// Helpers...
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using TWAINWorkingGroup;

namespace twaincscert
{
    /// <summary>
    /// The certification object that we'll use to test and exercise functions
    /// for TWAIN Direct.
    /// </summary>
    public sealed class Terminal : IDisposable
    {
        // Public Methods
        #region Public Methods

        /// <summary>
        /// Initialize stuff...
        /// </summary>
        public Terminal(FormMain a_formmain)
        {
            // Pick some colors...
            if (TWAIN.GetProcessor() == TWAIN.Processor.MIPS64EL)
            {
                m_consolecolorDefault = ConsoleColor.Black;
                m_consolecolorBlue = ConsoleColor.DarkBlue;
                m_consolecolorGreen = ConsoleColor.DarkGreen;
                m_consolecolorRed = ConsoleColor.Red;
                m_consolecolorYellow = ConsoleColor.DarkYellow;
            }
            else
            {
                m_consolecolorDefault = ConsoleColor.Gray;
                m_consolecolorBlue = ConsoleColor.Cyan;
                m_consolecolorGreen = ConsoleColor.Green;
                m_consolecolorRed = ConsoleColor.Red;
                m_consolecolorYellow = ConsoleColor.Yellow;
            }

            // Make sure we have a console...
            m_streamreaderConsole = Interpreter.CreateConsole();

            // Init stuff...
            m_blSilent = false;
            m_blSilentEvents = false;
            m_lkeyvalue = new List<KeyValue>();
            m_objectKeyValue = new object();
            m_lcallstack = new List<CallStack>();
            m_formmain = null;
            m_intptrHwnd = IntPtr.Zero;
            if (a_formmain != null)
            {
                m_formmain = a_formmain;
                m_intptrHwnd = a_formmain.Handle;
            }

            // Set up the base stack with the program arguments, we know
            // this is the base stack for two reasons: first, it has no
            // script, and second, it's first... :)
            CallStack callstack = default(CallStack);
            callstack.functionarguments.aszCmd = Config.GetCommandLine();
            m_lcallstack.Add(callstack);

            // Build our command table...
            m_ldispatchtable = new List<Interpreter.DispatchTable>();

            // Discovery and Selection...
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdHelp,                         new string[] { "help", "?" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdCertify,                      new string[] { "certify" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdList,                         new string[] { "list" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdQuit,                         new string[] { "ex", "exit", "q", "quit" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdSelect,                       new string[] { "select" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdStatus,                       new string[] { "status" }));

            // Dsmentry (all DG/DAT/MSG stuff comes here)...
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdDsmEntry,                     new string[] { "dsmentry" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdDsmLoad,                      new string[] { "dsmload" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdDsmUnload,                    new string[] { "dsmunload" }));

            // Scripting...
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdAllocate,                     new string[] { "allocate" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdCall,                         new string[] { "call" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdCd,                           new string[] { "cd", "pwd" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdClean,                        new string[] { "clean" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdDir,                          new string[] { "dir", "ls" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdEcho,                         new string[] { "echo" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdEchoBlue,                     new string[] { "echo.blue" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdEchoGreen,                    new string[] { "echo.green" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdEchoPassfail,                 new string[] { "echo.passfail" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdEchoRed,                      new string[] { "echo.red" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdEchoTitlesuite,               new string[] { "echo.titlesuite" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdEchoTitletest,                new string[] { "echo.titletest" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdEchoYellow,                   new string[] { "echo.yellow" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdFree,                         new string[] { "free" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdGc,                           new string[] { "gc" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdGoto,                         new string[] { "goto" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdIf,                           new string[] { "if" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdImage,                        new string[] { "image" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdIncrement,                    new string[] { "increment" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdInput,                        new string[] { "input" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdLog,                          new string[] { "log" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdReport,                       new string[] { "report" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdReturn,                       new string[] { "return" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdRun,                          new string[] { "run" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdRunv,                         new string[] { "runv" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdSaveImage,                    new string[] { "saveimage" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdSetGlobal,                    new string[] { "setglobal" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdSetLocal,                     new string[] { "setlocal" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdSleep,                        new string[] { "sleep" }));
            m_ldispatchtable.Add(new Interpreter.DispatchTable(CmdWait,                         new string[] { "wait" }));

            // Say hi...
            Assembly assembly = typeof(Terminal).Assembly;
            AssemblyName assemblyname = assembly.GetName();
            Version version = assemblyname.Version;
            DateTime datetime = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.MinorRevision * 2);
            string szSystem = "(";
            switch (TWAIN.GetPlatform())
            {
                default: szSystem += "UNKNOWN-OS"; break;
                case TWAIN.Platform.WINDOWS: szSystem += "Windows"; break;
                case TWAIN.Platform.LINUX: szSystem += "Linux"; break;
                case TWAIN.Platform.MACOSX: szSystem += "macOS"; break;
            }
            switch (TWAIN.GetProcessor())
            {
                default: szSystem += " UNKNOWN-PROCESSOR"; break;
                case TWAIN.Processor.X86:
                case TWAIN.Processor.X86_64: szSystem += " Intel"; break;
                case TWAIN.Processor.MIPS64EL: szSystem += " MIPSEL"; break;
            }
            switch (TWAIN.GetMachineWordBitSize())
            {
                default: szSystem += " UNKNOWN-BITSIZE"; break;
                case 32: szSystem += " 32-bit"; break;
                case 64: szSystem += " 64-bit"; break;
            }
            szSystem += ")";
            m_szBanner = "TWAIN Certification v" + version.Major + "." + version.Minor + " " + datetime.Day + "-" + datetime.ToString("MMM") + "-" + datetime.Year + " " + szSystem;
            Display(m_szBanner);
            Display("Enter \"certify\" to certify a scanner.");
            Display("Enter \"help\" for more info.");
        }

        /// <summary>
        /// Destructor...
        /// </summary>
        ~Terminal()
        {
            Dispose(false);
        }

        /// <summary>
        /// Cleanup...
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Run the certification tool...
        /// </summary>
        public void Run()
        {
            string szPrompt = "tc";
            Interpreter interpreter = new Interpreter(szPrompt + ">>> ", m_consolecolorDefault, m_consolecolorGreen);

            // If not windows we have to turn on echo...
            if (TWAINWorkingGroup.TWAIN.GetPlatform() != TWAIN.Platform.WINDOWS)
            {
                var typeSystemConsoleDriver = System.Type.GetType("System.ConsoleDriver");
                if (typeSystemConsoleDriver != null)
                {
                    var setecho = typeSystemConsoleDriver.GetMethod("SetEcho", BindingFlags.Static | BindingFlags.NonPublic);
                    if (setecho != null)
                    {
                        setecho.Invoke(System.Console.In, new object[] { false });
                        setecho.Invoke(System.Console.In, new object[] { true });
                    }
                }
            }

            // Run until told to stop...
            while (true)
            {
                bool blDone;
                string szCmd;
                string[] aszCmd;

                // Prompt...
                szCmd = interpreter.Prompt(m_streamreaderConsole, ((m_twain == null) ? 1 : (int)m_twain.GetState()));

                // Tokenize...
                aszCmd = interpreter.Tokenize(szCmd);

                // Expansion of symbols...
                Expansion(ref aszCmd);

                // Dispatch...
                Interpreter.FunctionArguments functionarguments = default(Interpreter.FunctionArguments);
                functionarguments.aszCmd = aszCmd;
                blDone = interpreter.Dispatch(ref functionarguments, m_ldispatchtable);
                if (blDone)
                {
                    return;
                }
            }
       }

        /// <summary>
        /// Monitor for DG_CONTROL / DAT_NULL / MSG_* stuff.  Actually, we're
        /// just passing this down into TWAIN.CS, so it can keep track of
        /// things for us...
        /// </summary>
        /// <param name="a_intptrHwnd">Handle of window we're monitoring</param>
        /// <param name="a_iMsg">Message received</param>
        /// <param name="a_intptrWparam">Argument to message</param>
        /// <param name="a_intptrLparam">Another argument to message</param>
        /// <returns></returns>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool PreFilterMessage
        (
            IntPtr a_intptrHwnd,
            int a_iMsg,
            IntPtr a_intptrWparam,
            IntPtr a_intptrLparam
        )
        {
            if (m_twain != null)
            {
                return (m_twain.PreFilterMessage(a_intptrHwnd, a_iMsg, a_intptrWparam, a_intptrLparam));
            }
            return (true);
        }

        #endregion


        // Public Definitions
        #region Public Definitions

        /// <summary>
        /// The scope of a variable...
        /// </summary>
        public enum VariableScope
        {
            Auto = 0,
            Local = 1,
            Global = 2
        }

        #endregion


        // Private Methods (DsmEntry)
        #region Private Methods (DsmEntry)

        /// <summary>
        /// Send a command to the currently loaded DSM...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdDsmEntry(ref Interpreter.FunctionArguments a_functionarguments)
        {
            TWAIN.DG dg = TWAIN.DG.MASK;
            TWAIN.DAT dat = TWAIN.DAT.NULL;
            TWAIN.MSG msg = TWAIN.MSG.NULL;
            CallStack callstack = m_lcallstack[m_lcallstack.Count - 1];

            // Init stuff...
            a_functionarguments.iDg = 0;
            a_functionarguments.iDat = 0;
            a_functionarguments.iMsg = 0;
            a_functionarguments.sts = TWAIN.STS.BADPROTOCOL;
            a_functionarguments.szReturnValue = "";

            // Validate at the top level...
            if (m_twain == null)
            {
                DisplayRed("***ERROR*** - dsmload wasn't run, so we is having no braims");
                a_functionarguments.sts = TWAIN.STS.SEQERROR;
                return (false);
            }
            if (a_functionarguments.aszCmd.Length != 7)
            {
                DisplayRed("***ERROR*** - command needs 7 arguments: dsmentry src dst dg dat msg memref");
                return (false);
            }

            // Look for Source...
            if (a_functionarguments.aszCmd[1] == "src")
            {
            }
            else
            {
                DisplayRed("Unrecognized src - <" + a_functionarguments.aszCmd[1] + ">");
                return (false);
            }

            // Look for Destination...
            if (a_functionarguments.aszCmd[2] == "ds")
            {
            }
            else if (a_functionarguments.aszCmd[2] == "null")
            {
            }
            else
            {
                DisplayRed("Unrecognized dst - <" + a_functionarguments.aszCmd[2] + ">");
                return (false);
            }

            // Look for DG...
            if (!a_functionarguments.aszCmd[3].ToLowerInvariant().StartsWith("dg_"))
            {
                DisplayRed("Unrecognized dg - <" + a_functionarguments.aszCmd[3] + ">");
                return (false);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_functionarguments.aszCmd[3].ToLowerInvariant().StartsWith("dg_0x"))
                {
                    if (!int.TryParse(a_functionarguments.aszCmd[3].ToLowerInvariant().Substring(3), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out a_functionarguments.iDg))
                    {
                        DisplayRed("Badly constructed dg - <" + a_functionarguments.aszCmd[3] + ">");
                        return (false);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_functionarguments.aszCmd[3].ToUpperInvariant().Substring(3), out dg))
                    {
                        DisplayRed("Unrecognized dg - <" + a_functionarguments.aszCmd[3] + ">");
                        return (false);
                    }
                    a_functionarguments.iDg = (int)dg;
                }
            }

            // Look for DAT...
            if (!a_functionarguments.aszCmd[4].ToLowerInvariant().StartsWith("dat_"))
            {
                DisplayRed("Unrecognized dat - <" + a_functionarguments.aszCmd[4] + ">");
                return (false);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_functionarguments.aszCmd[4].ToLowerInvariant().StartsWith("dat_0x"))
                {
                    if (!int.TryParse(a_functionarguments.aszCmd[4].ToLowerInvariant().Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out a_functionarguments.iDat))
                    {
                        DisplayRed("Badly constructed dat - <" + a_functionarguments.aszCmd[4] + ">");
                        return (false);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_functionarguments.aszCmd[4].ToUpperInvariant().Substring(4), out dat))
                    {
                        DisplayRed("Unrecognized dat - <" + a_functionarguments.aszCmd[4] + ">");
                        return (false);
                    }
                    a_functionarguments.iDat = (int)dat;
                }
            }

            // Look for MSG...
            if (!a_functionarguments.aszCmd[5].ToLowerInvariant().StartsWith("msg_"))
            {
                DisplayRed("Unrecognized msg - <" + a_functionarguments.aszCmd[5] + ">");
                return (false);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_functionarguments.aszCmd[5].ToLowerInvariant().StartsWith("msg_0x"))
                {
                    if (!int.TryParse(a_functionarguments.aszCmd[5].ToLowerInvariant().Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out a_functionarguments.iMsg))
                    {
                        DisplayRed("Badly constructed dat - <" + a_functionarguments.aszCmd[5] + ">");
                        return (false);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_functionarguments.aszCmd[5].ToUpperInvariant().Substring(4), out msg))
                    {
                        DisplayRed("Unrecognized msg - <" + a_functionarguments.aszCmd[5] + ">");
                        return (false);
                    }
                    a_functionarguments.iMsg = (int)msg;
                }
            }

            // If this is issued directly by the user or runv is being used, let's
            // give more info...
            if (!m_blRunningScript || m_blVerbose)
            {
                CSV csv = new CSV();
                foreach (string szCmd in a_functionarguments.aszCmd)
                {
                    csv.Add(szCmd);
                }
                DisplayBlue("snd: " + csv.Get());
            }

            // Send the command...
            switch (a_functionarguments.iDat)
            {
                // Ruh-roh, since we can't marshal it, we have to return an error,
                // it would be nice to have a solution for this, but that will need
                // a dynamic marshalling system...
                default:
                    a_functionarguments.sts = TWAIN.STS.BADPROTOCOL;
                    break;

                // DAT_AUDIOFILEXFER...
                case (int)TWAIN.DAT.AUDIOFILEXFER:
                    {
                        a_functionarguments.sts = m_twain.DatAudiofilexfer((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg);
                        a_functionarguments.szReturnValue = "";
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_AUDIOINFO..
                case (int)TWAIN.DAT.AUDIOINFO:
                    {
                        TWAIN.TW_AUDIOINFO twaudioinfo = default(TWAIN.TW_AUDIOINFO);
                        a_functionarguments.sts = m_twain.DatAudioinfo((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twaudioinfo);
                        a_functionarguments.szReturnValue = m_twain.AudioinfoToCsv(twaudioinfo);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_AUDIONATIVEXFER..
                case (int)TWAIN.DAT.AUDIONATIVEXFER:
                    {
                        IntPtr intptr = IntPtr.Zero;
                        a_functionarguments.sts = m_twain.DatAudionativexfer((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref intptr);
                        a_functionarguments.szReturnValue = intptr.ToString();
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_CALLBACK...
                case (int)TWAIN.DAT.CALLBACK:
                    {
                        TWAIN.TW_CALLBACK twcallback = default(TWAIN.TW_CALLBACK);
                        m_twain.CsvToCallback(ref twcallback, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatCallback((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twcallback);
                        a_functionarguments.szReturnValue = m_twain.CallbackToCsv(twcallback);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_CALLBACK2...
                case (int)TWAIN.DAT.CALLBACK2:
                    {
                        TWAIN.TW_CALLBACK2 twcallback2 = default(TWAIN.TW_CALLBACK2);
                        m_twain.CsvToCallback2(ref twcallback2, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatCallback2((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twcallback2);
                        a_functionarguments.szReturnValue = m_twain.Callback2ToCsv(twcallback2);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
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
                        m_twain.CsvToCapability(ref twcapability, ref szStatus, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatCapability((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twcapability);
                        if ((a_functionarguments.sts == TWAIN.STS.SUCCESS) || (a_functionarguments.sts == TWAIN.STS.CHECKSTATUS))
                        {
                            a_functionarguments.szReturnValue = m_twain.CapabilityToCsv(twcapability, ((TWAIN.MSG)a_functionarguments.iMsg != TWAIN.MSG.QUERYSUPPORT));
                        }
                        else
                        {
                            a_functionarguments.szReturnValue = a_functionarguments.aszCmd[6];
                        }
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_CIECOLOR..
                case (int)TWAIN.DAT.CIECOLOR:
                    {
                        //TWAIN.TW_CIECOLOR twciecolor = default(TWAIN.TW_CIECOLOR);
                        //a_functionarguments.sts = m_twain.DatCiecolor((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twciecolor);
                        //a_functionarguments.szReturnValue = m_twain.CiecolorToCsv(twciecolor);
                        //callstack.functionarguments.sts = a_functionarguments.sts;
                        //callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_CUSTOMDSDATA...
                case (int)TWAIN.DAT.CUSTOMDSDATA:
                    {
                        TWAIN.TW_CUSTOMDSDATA twcustomdsdata = default(TWAIN.TW_CUSTOMDSDATA);
                        m_twain.CsvToCustomdsdata(ref twcustomdsdata, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatCustomdsdata((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twcustomdsdata);
                        a_functionarguments.szReturnValue = m_twain.CustomdsdataToCsv(twcustomdsdata);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_DEVICEEVENT...
                case (int)TWAIN.DAT.DEVICEEVENT:
                    {
                        TWAIN.TW_DEVICEEVENT twdeviceevent = default(TWAIN.TW_DEVICEEVENT);
                        a_functionarguments.sts = m_twain.DatDeviceevent((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twdeviceevent);
                        a_functionarguments.szReturnValue = m_twain.DeviceeventToCsv(twdeviceevent);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_ENTRYPOINT...
                case (int)TWAIN.DAT.ENTRYPOINT:
                    {
                        TWAIN.TW_ENTRYPOINT twentrypoint = default(TWAIN.TW_ENTRYPOINT);
                        a_functionarguments.sts = m_twain.DatEntrypoint((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twentrypoint);
                        a_functionarguments.szReturnValue = m_twain.EntrypointToCsv(twentrypoint);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_EVENT...
                case (int)TWAIN.DAT.EVENT:
                    {
                        TWAIN.TW_EVENT twevent = default(TWAIN.TW_EVENT);
                        a_functionarguments.sts = m_twain.DatEvent((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twevent);
                        a_functionarguments.szReturnValue = m_twain.EventToCsv(twevent);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_EXTIMAGEINFO...
                case (int)TWAIN.DAT.EXTIMAGEINFO:
                    {
                        TWAIN.TW_EXTIMAGEINFO twextimageinfo = default(TWAIN.TW_EXTIMAGEINFO);
                        m_twain.CsvToExtimageinfo(ref twextimageinfo, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatExtimageinfo((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twextimageinfo);
                        a_functionarguments.szReturnValue = m_twain.ExtimageinfoToCsv(twextimageinfo);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_FILESYSTEM...
                case (int)TWAIN.DAT.FILESYSTEM:
                    {
                        TWAIN.TW_FILESYSTEM twfilesystem = default(TWAIN.TW_FILESYSTEM);
                        m_twain.CsvToFilesystem(ref twfilesystem, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatFilesystem((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twfilesystem);
                        a_functionarguments.szReturnValue = m_twain.FilesystemToCsv(twfilesystem);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_FILTER...
                case (int)TWAIN.DAT.FILTER:
                    {
                        //TWAIN.TW_FILTER twfilter = default(TWAIN.TW_FILTER);
                        //m_twain.CsvToFilter(ref twfilter, a_functionarguments.aszCmd[6]);
                        //a_functionarguments.sts = m_twain.DatFilter((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twfilter);
                        //a_functionarguments.szReturnValue = m_twain.FilterToCsv(twfilter);
                        //callstack.functionarguments.sts = a_functionarguments.sts;
                        //callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_GRAYRESPONSE...
                case (int)TWAIN.DAT.GRAYRESPONSE:
                    {
                        //TWAIN.TW_GRAYRESPONSE twgrayresponse = default(TWAIN.TW_GRAYRESPONSE);
                        //m_twain.CsvToGrayresponse(ref twgrayresponse, a_functionarguments.aszCmd[6]);
                        //a_functionarguments.sts = m_twain.DatGrayresponse((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twgrayresponse);
                        //a_functionarguments.szReturnValue = m_twain.GrayresponseToCsv(twgrayresponse);
                        //callstack.functionarguments.sts = a_functionarguments.sts;
                        //callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_ICCPROFILE...
                case (int)TWAIN.DAT.ICCPROFILE:
                    {
                        TWAIN.TW_MEMORY twmemory = default(TWAIN.TW_MEMORY);
                        a_functionarguments.sts = m_twain.DatIccprofile((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twmemory);
                        a_functionarguments.szReturnValue = m_twain.IccprofileToCsv(twmemory);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_IDENTITY...
                case (int)TWAIN.DAT.IDENTITY:
                    {


                        TWAIN.TW_IDENTITY twidentity = default(TWAIN.TW_IDENTITY);
                        m_twain.CsvToIdentity(ref twidentity, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatIdentity((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twidentity);
                        a_functionarguments.szReturnValue = m_twain.IdentityToCsv(twidentity);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_IMAGEFILEXFER...
                case (int)TWAIN.DAT.IMAGEFILEXFER:
                    {
                        a_functionarguments.sts = m_twain.DatImagefilexfer((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg);
                        a_functionarguments.szReturnValue = "";
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_IMAGEINFO...
                case (int)TWAIN.DAT.IMAGEINFO:
                    {
                        TWAIN.TW_IMAGEINFO twimageinfo = default(TWAIN.TW_IMAGEINFO);
                        m_twain.CsvToImageinfo(ref twimageinfo, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatImageinfo((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twimageinfo);
                        a_functionarguments.szReturnValue = m_twain.ImageinfoToCsv(twimageinfo);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_IMAGELAYOUT...
                case (int)TWAIN.DAT.IMAGELAYOUT:
                    {
                        TWAIN.TW_IMAGELAYOUT twimagelayout = default(TWAIN.TW_IMAGELAYOUT);
                        m_twain.CsvToImagelayout(ref twimagelayout, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatImagelayout((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twimagelayout);
                        a_functionarguments.szReturnValue = m_twain.ImagelayoutToCsv(twimagelayout);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_IMAGEMEMFILEXFER...
                case (int)TWAIN.DAT.IMAGEMEMFILEXFER:
                    {
                        TWAIN.TW_IMAGEMEMXFER twimagememxfer = default(TWAIN.TW_IMAGEMEMXFER);
                        m_twain.CsvToImagememxfer(ref twimagememxfer, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatImagememfilexfer((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twimagememxfer);
                        a_functionarguments.szReturnValue = m_twain.ImagememxferToCsv(twimagememxfer);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_IMAGEMEMXFER...
                case (int)TWAIN.DAT.IMAGEMEMXFER:
                    {
                        TWAIN.TW_IMAGEMEMXFER twimagememxfer = default(TWAIN.TW_IMAGEMEMXFER);
                        m_twain.CsvToImagememxfer(ref twimagememxfer, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatImagememxfer((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twimagememxfer);
                        a_functionarguments.szReturnValue = m_twain.ImagememxferToCsv(twimagememxfer);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_IMAGENATIVEXFER...
                case (int)TWAIN.DAT.IMAGENATIVEXFER:
                    {
                        IntPtr intptrBitmapHandle = IntPtr.Zero;
                        a_functionarguments.sts = m_twain.DatImagenativexferHandle((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref intptrBitmapHandle);
                        a_functionarguments.szReturnValue = intptrBitmapHandle.ToString();
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_JPEGCOMPRESSION...
                case (int)TWAIN.DAT.JPEGCOMPRESSION:
                    {
                        //TWAIN.TW_JPEGCOMPRESSION twjpegcompression = default(TWAIN.TW_JPEGCOMPRESSION);
                        //m_twain.CsvToJpegcompression(ref twjpegcompression, a_functionarguments.aszCmd[6]);
                        //a_functionarguments.sts = m_twain.DatJpegcompression((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twjpegcompression);
                        //a_functionarguments.szReturnValue = m_twain.JpegcompressionToCsv(twjpegcompression);
                        //callstack.functionarguments.sts = a_functionarguments.sts;
                        //callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_METRICS...
                case (int)TWAIN.DAT.METRICS:
                    {
                        TWAIN.TW_METRICS twmetrics = default(TWAIN.TW_METRICS);
                        a_functionarguments.sts = m_twain.DatMetrics((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twmetrics);
                        a_functionarguments.szReturnValue = m_twain.MetricsToCsv(twmetrics);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_PALETTE8...
                case (int)TWAIN.DAT.PALETTE8:
                    {
                        //TWAIN.TW_PALETTE8 twpalette8 = default(TWAIN.TW_PALETTE8);
                        //m_twain.CsvToPalette8(ref twpalette8, a_functionarguments.aszCmd[6]);
                        //a_functionarguments.sts = m_twain.DatPalette8((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twpalette8);
                        //a_functionarguments.szReturnValue = m_twain.Palette8ToCsv(twpalette8);
                        //callstack.functionarguments.sts = a_functionarguments.sts;
                        //callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_PARENT...
                case (int)TWAIN.DAT.PARENT:
                    {
                        a_functionarguments.sts = m_twain.DatParent((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref m_intptrHwnd);
                    }
                    break;

                // DAT_PASSTHRU...
                case (int)TWAIN.DAT.PASSTHRU:
                    {
                        TWAIN.TW_PASSTHRU twpassthru = default(TWAIN.TW_PASSTHRU);
                        m_twain.CsvToPassthru(ref twpassthru, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatPassthru((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twpassthru);
                        a_functionarguments.szReturnValue = m_twain.PassthruToCsv(twpassthru);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_PENDINGXFERS...
                case (int)TWAIN.DAT.PENDINGXFERS:
                    {
                        TWAIN.TW_PENDINGXFERS twpendingxfers = default(TWAIN.TW_PENDINGXFERS);
                        a_functionarguments.sts = m_twain.DatPendingxfers((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twpendingxfers);
                        a_functionarguments.szReturnValue = m_twain.PendingxfersToCsv(twpendingxfers);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_RGBRESPONSE...
                case (int)TWAIN.DAT.RGBRESPONSE:
                    {
                        //TWAIN.TW_RGBRESPONSE twrgbresponse = default(TWAIN.TW_RGBRESPONSE);
                        //m_twain.CsvToRgbresponse(ref twrgbresponse, a_functionarguments.aszCmd[6]);
                        //a_functionarguments.sts = m_twain.DatRgbresponse((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twrgbresponse);
                        //a_functionarguments.szReturnValue = m_twain.RgbresponseToCsv(twrgbresponse);
                        //callstack.functionarguments.sts = a_functionarguments.sts;
                        //callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_SETUPFILEXFER...
                case (int)TWAIN.DAT.SETUPFILEXFER:
                    {
                        TWAIN.TW_SETUPFILEXFER twsetupfilexfer = default(TWAIN.TW_SETUPFILEXFER);
                        m_twain.CsvToSetupfilexfer(ref twsetupfilexfer, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatSetupfilexfer((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twsetupfilexfer);
                        a_functionarguments.szReturnValue = m_twain.SetupfilexferToCsv(twsetupfilexfer);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_SETUPMEMXFER...
                case (int)TWAIN.DAT.SETUPMEMXFER:
                    {
                        TWAIN.TW_SETUPMEMXFER twsetupmemxfer = default(TWAIN.TW_SETUPMEMXFER);
                        a_functionarguments.sts = m_twain.DatSetupmemxfer((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twsetupmemxfer);
                        a_functionarguments.szReturnValue = m_twain.SetupmemxferToCsv(twsetupmemxfer);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_STATUS...
                case (int)TWAIN.DAT.STATUS:
                    {
                        TWAIN.TW_STATUS twstatus = default(TWAIN.TW_STATUS);
                        a_functionarguments.sts = m_twain.DatStatus((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twstatus);
                        a_functionarguments.szReturnValue = m_twain.StatusToCsv(twstatus);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_STATUSUTF8...
                case (int)TWAIN.DAT.STATUSUTF8:
                    {
                        TWAIN.TW_STATUSUTF8 twstatusutf8 = default(TWAIN.TW_STATUSUTF8);
                        a_functionarguments.sts = m_twain.DatStatusutf8((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twstatusutf8);
                        a_functionarguments.szReturnValue = m_twain.Statusutf8ToCsv(twstatusutf8);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_TWAINDIRECT...
                case (int)TWAIN.DAT.TWAINDIRECT:
                    {
                        TWAIN.TW_TWAINDIRECT twtwaindirect = default(TWAIN.TW_TWAINDIRECT);
                        m_twain.CsvToTwaindirect(ref twtwaindirect, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatTwaindirect((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twtwaindirect);
                        a_functionarguments.szReturnValue = m_twain.TwaindirectToCsv(twtwaindirect);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_USERINTERFACE...
                case (int)TWAIN.DAT.USERINTERFACE:
                    {
                        TWAIN.TW_USERINTERFACE twuserinterface = default(TWAIN.TW_USERINTERFACE);
                        m_twain.CsvToUserinterface(ref twuserinterface, a_functionarguments.aszCmd[6]);
                        a_functionarguments.sts = m_twain.DatUserinterface((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref twuserinterface);
                        a_functionarguments.szReturnValue = m_twain.UserinterfaceToCsv(twuserinterface);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;

                // DAT_XFERGROUP...
                case (int)TWAIN.DAT.XFERGROUP:
                    {
                        uint uXferGroup = 0;
                        a_functionarguments.sts = m_twain.DatXferGroup((TWAIN.DG)a_functionarguments.iDg, (TWAIN.MSG)a_functionarguments.iMsg, ref uXferGroup);
                        a_functionarguments.szReturnValue = string.Format("0x{0:X}", uXferGroup);
                        callstack.functionarguments.sts = a_functionarguments.sts;
                        callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    }
                    break;
            }

            // If this is issued directly by the user or runv is being used, let's
            // give more info...
            if (!m_blRunningScript || m_blVerbose)
            {
                DisplayBlue("rcv: " + a_functionarguments.szReturnValue);
                DisplayBlue("sts: " + a_functionarguments.sts);
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Load the DSM...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdDsmLoad(ref Interpreter.FunctionArguments a_functionarguments)
        {
            Assembly assembly = typeof(Terminal).Assembly;
            AssemblyName assemblyname = assembly.GetName();
            Version version = assemblyname.Version;

            string szManufacturer = "TWAIN Working Group";
            string szProductFamily = "TWAIN Open Source";
            string szProductName = "TWAIN Certification";
            UInt16 u16ProtocolMajor = (UInt16)TWAIN.TWON_PROTOCOL.MAJOR;
            UInt16 u16ProtocolMinor = (UInt16)TWAIN.TWON_PROTOCOL.MINOR;
            UInt32 u32SupportedGroups = ((int)TWAIN.DG.APP2 | (int)TWAIN.DG.CONTROL | (int)TWAIN.DG.IMAGE);
            TWAIN.TWCY twcy = TWAIN.TWCY.USA;
            string szInfo = "TWAIN Certification";
            TWAIN.TWLG twlg = TWAIN.TWLG.ENGLISH;
            UInt16 u16MajorNum = (UInt16)version.Major;
            UInt16 u16MinorNum = (UInt16)version.Minor;
            bool blUseLegacyDSM = false;
            bool blUseCallbacks = false;
            TWAIN.DeviceEventCallback deviceeventcallback = DeviceEventCallback;
            TWAIN.ScanCallback scancallback = ScanCallback;
            TWAIN.RunInUiThreadDelegate runinuithreaddelegate = null;
            IntPtr intptrHwnd = m_intptrHwnd;

            // Nope...
            if (m_twain != null)
            {
                return (false);
            }

            // Look for matches in the argument list...
            for (int aa = 0; aa < a_functionarguments.aszCmd.Length; aa++)
            {
                // Split it on the first '='...
                string[] aszKeyValue = a_functionarguments.aszCmd[aa].Split(new char[] { '=' }, 1);
                if (aszKeyValue.Length != 2)
                {
                    continue;
                }

                // Dispatch...
                switch (aszKeyValue[0].ToLowerInvariant())
                {
                    default:
                        DisplayRed("Unrecognized argument...<" + a_functionarguments.aszCmd[aa] + ">");
                        break;
                    case "manufacturer":
                        szManufacturer = aszKeyValue[1];
                        break;
                    case "productfamily":
                        szProductFamily = aszKeyValue[1];
                        break;
                    case "productname":
                        szProductName = aszKeyValue[1];
                        break;
                    case "protocolmajor":
                        UInt16.TryParse(aszKeyValue[1], out u16ProtocolMajor);
                        break;
                    case "protocolminor":
                        UInt16.TryParse(aszKeyValue[1], out u16ProtocolMinor);
                        break;
                    case "supportedgroups":
                        UInt32.TryParse(aszKeyValue[1], out u32SupportedGroups);
                        break;
                    case "twcy":
                        Enum.TryParse(aszKeyValue[1], out twcy);
                        break;
                    case "info":
                        szInfo = aszKeyValue[1];
                        break;
                    case "twlg":
                        Enum.TryParse(aszKeyValue[1], out twlg);
                        break;
                    case "majornum":
                        UInt16.TryParse(aszKeyValue[1], out u16MajorNum);
                        break;
                    case "minornum":
                        UInt16.TryParse(aszKeyValue[1], out u16MinorNum);
                        break;
                    case "uselegacydsm":
                        bool.TryParse(aszKeyValue[1], out blUseLegacyDSM);
                        break;
                    case "usecallbacks":
                        bool.TryParse(aszKeyValue[1], out blUseCallbacks);
                        break;
                }
            }

            // Instantiate TWAIN, and register ourselves...
            m_twain = new TWAIN
            (
                szManufacturer,
                szProductFamily,
                szProductName,
                u16ProtocolMajor,
                u16ProtocolMinor,
                u32SupportedGroups,
                twcy,
                szInfo,
                twlg,
                u16MajorNum,
                u16MinorNum,
                blUseLegacyDSM,
                blUseCallbacks,
                deviceeventcallback,
                scancallback,
                runinuithreaddelegate,
                intptrHwnd
            );

            // All done...
            return (false);
        }

        /// <summary>
        /// Unload the DSM...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdDsmUnload(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // Nope...
            if (m_twain == null)
            {
                return (false);
            }

            // Shut it down, make sure it's really gone...
            m_twain.Dispose();
            GC.Collect();
            m_twain = null;

            // All done...
            return (false);
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
            TWAIN.STS sts;
            TWAIN.TW_DEVICEEVENT twdeviceevent;

            // Drain the event queue...
            while (true)
            {
                // Try to get an event...
                twdeviceevent = default(TWAIN.TW_DEVICEEVENT);
                sts = m_twain.DatDeviceevent(TWAIN.DG.CONTROL, TWAIN.MSG.GET, ref twdeviceevent);
                if (sts != TWAIN.STS.SUCCESS)
                {
                    break;
                }

                // Process what we got...
                //WriteOutput("*** DeviceEvent: " + m_twain.DeviceeventToCsv(twdeviceevent) + Environment.NewLine);
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
        private TWAIN.STS ScanCallback(bool a_blClosing)
        {
            // Scoot...
            if (m_twain == null)
            {
                return (TWAIN.STS.FAILURE);
            }

            // We're leaving...
            if (a_blClosing)
            {
                return (TWAIN.STS.SUCCESS);
            }

            // Collect DAT_NULL stuff...
            if (m_twain.IsMsgXferReady())
            {
                lock (m_lmsgDatNull)
                {
                    m_lmsgDatNull.Add(TWAIN.MSG.XFERREADY);
                    m_autoreseteventMsgDatNull.Set();
                }
            }
            if (m_twain.IsMsgCloseDsReq())
            {
                lock (m_lmsgDatNull)
                {
                    m_lmsgDatNull.Add(TWAIN.MSG.CLOSEDSREQ);
                    m_autoreseteventMsgDatNull.Set();
                }
            }
            if (m_twain.IsMsgCloseDsOk())
            {
                lock (m_lmsgDatNull)
                {
                    m_lmsgDatNull.Add(TWAIN.MSG.CLOSEDSOK);
                    m_autoreseteventMsgDatNull.Set();
                }
            }

            // We're waiting for that first image to show up, if we don't
            // see it, then return...
            if (!m_twain.IsMsgXferReady())
            {
                // If we're on Windows we need to send event requests to the driver...
                if (TWAIN.GetPlatform() == TWAIN.Platform.WINDOWS)
                {
                    TWAIN.TW_EVENT twevent = default(TWAIN.TW_EVENT);
                    twevent.pEvent = Marshal.AllocHGlobal(256); // over allocate for MSG structure
                    if (twevent.pEvent != IntPtr.Zero)
                    {
                        m_twain.DatEvent(TWAIN.DG.CONTROL, TWAIN.MSG.PROCESSEVENT, ref twevent);
                        Marshal.FreeHGlobal(twevent.pEvent);
                    }
                }

                // Scoot...
                return (TWAIN.STS.SUCCESS);
            }

            // All done...
            return (TWAIN.STS.SUCCESS);
        }

        #endregion


        // Private Methods (commands)
        #region Private Methods (commands)

        /// <summary>
        /// Allocate a handle or a pointer, we'll get an IntPtr to it...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdAllocate(ref Interpreter.FunctionArguments a_functionarguments)
        {
            UInt32 u32Bytes = 0;
            IntPtr intptr = IntPtr.Zero;
            CallStack callstack = m_lcallstack[m_lcallstack.Count - 1];

            // Validate...
            if (a_functionarguments.aszCmd.Length != 4)
            {
                DisplayError("command needs 3 arguments", a_functionarguments);
                a_functionarguments.sts = TWAIN.STS.BADVALUE;
                a_functionarguments.szReturnValue = "0";
                callstack.functionarguments.sts = a_functionarguments.sts;
                callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                return (false);
            }

            // Get the size...
            if (!UInt32.TryParse(a_functionarguments.aszCmd[3], out u32Bytes))
            {
                DisplayError("size is not a number <" + a_functionarguments.aszCmd[3] + ">", a_functionarguments);
                a_functionarguments.sts = TWAIN.STS.BADVALUE;
                a_functionarguments.szReturnValue = "0";
                callstack.functionarguments.sts = a_functionarguments.sts;
                callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                SetVariable(a_functionarguments.aszCmd[2], "0", 0, VariableScope.Local);
                return (false);
            }

            // Allocate a handle...
            if (a_functionarguments.aszCmd[1].ToLowerInvariant() == "handle")
            {
                // Allocate the memory...
                intptr = m_twain.DsmMemAlloc(u32Bytes);
                if (intptr == IntPtr.Zero)
                {
                    DisplayError("allocation failed <" + a_functionarguments.aszCmd[2] + ">", a_functionarguments);
                    a_functionarguments.sts = TWAIN.STS.LOWMEMORY;
                    a_functionarguments.szReturnValue = "0";
                    callstack.functionarguments.sts = a_functionarguments.sts;
                    callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    SetVariable(a_functionarguments.aszCmd[2], "0", 0, VariableScope.Local);
                    return (false);
                }
            }

            // Allocate a pointer...
            else if (a_functionarguments.aszCmd[1].ToLowerInvariant() == "pointer")
            {
                intptr = Marshal.AllocHGlobal((IntPtr)u32Bytes);
                if (intptr == IntPtr.Zero)
                {
                    DisplayError("allocation failed <" + a_functionarguments.aszCmd[2] + ">", a_functionarguments);
                    a_functionarguments.sts = TWAIN.STS.LOWMEMORY;
                    a_functionarguments.szReturnValue = "0";
                    callstack.functionarguments.sts = a_functionarguments.sts;
                    callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    SetVariable(a_functionarguments.aszCmd[2], "0", 0, VariableScope.Local);
                    return (false);
                }
            }

            // Oops...
            else
            {
                DisplayError("unrecognized flag <" + a_functionarguments.aszCmd[1] + ">", a_functionarguments);
                a_functionarguments.sts = TWAIN.STS.BADVALUE;
                a_functionarguments.szReturnValue = "0";
                callstack.functionarguments.sts = a_functionarguments.sts;
                callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                return (false);
            }

            // All done...
            a_functionarguments.sts = TWAIN.STS.SUCCESS;
            a_functionarguments.szReturnValue = intptr.ToString();
            callstack.functionarguments.sts = a_functionarguments.sts;
            callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
            SetVariable(a_functionarguments.aszCmd[2], intptr.ToString(), (int)u32Bytes, VariableScope.Local);
            return (false);
        }

        /// <summary>
        /// Call a function...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>Free
        private bool CmdCall(ref Interpreter.FunctionArguments a_functionarguments)
        {
            int iLine;
            string szLabel;

            // Validate...
            if (    (a_functionarguments.aszScript == null)
                ||  (a_functionarguments.aszScript.Length < 2)
                ||  (a_functionarguments.aszScript[0] == null)
                ||  (a_functionarguments.aszCmd == null)
                ||  (a_functionarguments.aszCmd.Length < 2)
                ||  (a_functionarguments.aszCmd[1] == null))
            {
                return (false);
            }

            // Search for a match...
            szLabel = ":" + a_functionarguments.aszCmd[1];
            for (iLine = 0; iLine < a_functionarguments.aszScript.Length; iLine++)
            {
                if (a_functionarguments.aszScript[iLine].Trim() == szLabel)
                {
                    // We need this to go to the function...
                    a_functionarguments.blGotoLabel = true;
                    a_functionarguments.iLabelLine = iLine;

                    // We need this to get back...
                    CallStack callstack = default(CallStack);
                    callstack.functionarguments = a_functionarguments;
                    m_lcallstack.Add(callstack);
                    return (false);
                }
            }

            // Ugh...
            DisplayError("function label not found: <" + szLabel + ">", a_functionarguments);
            return (false);
        }

        /// <summary>
        /// Show or set the current directory...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdCd(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // No data...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[1] == null) || (a_functionarguments.aszCmd[0].ToLowerInvariant() == "pwd"))
            {
                Display(Directory.GetCurrentDirectory(), true);
                return (false);
            }

            // Ruh-roh...
            if (!Directory.Exists(a_functionarguments.aszCmd[1]))
            {
                DisplayError("cd failed - path not found", a_functionarguments);
                return (false);
            }

            // Set the current directory...
            try
            {
                Directory.SetCurrentDirectory(a_functionarguments.aszCmd[1]);
            }
            catch (Exception exception)
            {
                DisplayError("cd failed - " + exception.Message, a_functionarguments);
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Clean the images folder...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdClean(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // The images folder...
            string szImagesFolder = Path.Combine(Config.Get("writeFolder", null), "images");

            // Delete the images folder...
            if (Directory.Exists(szImagesFolder))
            {
                try
                {
                    DirectoryInfo directoryinfo = new DirectoryInfo(szImagesFolder);
                    foreach (System.IO.FileInfo file in directoryinfo.GetFiles()) file.Delete();
                    foreach (System.IO.DirectoryInfo subDirectory in directoryinfo.GetDirectories()) subDirectory.Delete(true);
                }
                catch (Exception exception)
                {
                    DisplayError("couldn't delete <" + szImagesFolder + "> - " + exception.Message, a_functionarguments);
                    return (false);
                }
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Lists the files and folders in the current directory...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdDir(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // Get the folders...
            string[] aszFolders = Directory.GetDirectories(".");
            if ((aszFolders != null) && (aszFolders.Length > 0))
            {
                Array.Sort(aszFolders);
                foreach (string sz in aszFolders)
                {
                    Display(sz.Replace(".\\","").Replace("./","") + Path.DirectorySeparatorChar);
                }
            }

            // Get the files...
            string[] aszFiles = Directory.GetFiles(".");
            if ((aszFiles != null) && (aszFiles.Length > 0))
            {
                Array.Sort(aszFiles);
                foreach (string sz in aszFiles)
                {
                    Display(sz.Replace(".\\", "").Replace("./", ""));
                }
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Echo text...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdEcho(ref Interpreter.FunctionArguments a_functionarguments)
        {
            return (CmdEchoColor(ref a_functionarguments, ConsoleColor.White));
        }

        /// <summary>
        /// Echo text as blue...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdEchoBlue(ref Interpreter.FunctionArguments a_functionarguments)
        {
            return (CmdEchoColor(ref a_functionarguments, ConsoleColor.Blue));
        }

        /// <summary>
        /// Echo text as green...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdEchoGreen(ref Interpreter.FunctionArguments a_functionarguments)
        {
            return (CmdEchoColor(ref a_functionarguments, ConsoleColor.Green));
        }

        /// <summary>
        /// Echo text as red...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdEchoRed(ref Interpreter.FunctionArguments a_functionarguments)
        {
            return (CmdEchoColor(ref a_functionarguments, ConsoleColor.Red));
        }

        /// <summary>
        /// Echo text as yellow...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdEchoYellow(ref Interpreter.FunctionArguments a_functionarguments)
        {
            return (CmdEchoColor(ref a_functionarguments, ConsoleColor.Yellow));
        }

        /// <summary>
        /// Echo text as white...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdEchoColor(ref Interpreter.FunctionArguments a_functionarguments, ConsoleColor a_consolecolor)
        {
            int ii;
            string szLine = "";
            string[] aszCmd;

            // No data...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[0] == null))
            {
                Display("", true);
                return (false);
            }

            // Copy the array...
            aszCmd = new string[a_functionarguments.aszCmd.Length];
            Array.Copy(a_functionarguments.aszCmd, aszCmd, a_functionarguments.aszCmd.Length);

            // Expand the symbols...
            Expansion(ref aszCmd);

            // Turn it into a line...
            for (ii = 1; ii < aszCmd.Length; ii++)
            {
                szLine += ((szLine == "") ? "" : " ") + aszCmd[ii];
            }

            // Spit it out...
            switch (a_consolecolor)
            {
                default:
                    Display(szLine, true);
                    break;

                case ConsoleColor.Blue:
                    DisplayBlue(szLine, true);
                    break;

                case ConsoleColor.Green:
                    DisplayGreen(szLine, true);
                    break;

                case ConsoleColor.Red:
                    DisplayRed(szLine, true);
                    break;

                case ConsoleColor.Yellow:
                    DisplayYellow(szLine, true);
                    break;
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Display a pass/fail message...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdEchoPassfail(ref Interpreter.FunctionArguments a_functionarguments)
        {
            string szLine;
            string szDots = "..........................................................................................................";

            // No data...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 3) || (a_functionarguments.aszCmd[0] == null))
            {
                DisplayError("echo.passfail needs two arguments", a_functionarguments);
                return (false);
            }

            // Build the string...
            szLine = a_functionarguments.aszCmd[1];
            if ((szDots.Length - szLine.Length) > 0)
            {
                szLine += szDots.Substring(0, szDots.Length - szLine.Length);
            }
            else
            {
                szLine += "...";
            }
            szLine += a_functionarguments.aszCmd[2];

            // Spit it out...
            if (a_functionarguments.aszCmd[2].Contains("fail") || a_functionarguments.aszCmd[2].Contains("error"))
            {
                DisplayRed(szLine, true);
            }
            else if (a_functionarguments.aszCmd[2].Contains("warn"))
            {
                DisplayYellow(szLine, true);
            }
            else
            {
                Display(szLine, true);
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Display a title suite message...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdEchoTitlesuite(ref Interpreter.FunctionArguments a_functionarguments)
        {
            string szStars = "**************************************************************************************************************";

            // No data...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[0] == null))
            {
                DisplayError("echo.titlesuite needs one argument", a_functionarguments);
                return (false);
            }

            // Display it...
            DisplayYellow("", true);
            DisplayYellow("", true);
            DisplayYellow("", true);
            DisplayYellow(szStars, true);
            DisplayYellow(a_functionarguments.aszCmd[1], true);

            // All done...
            return (false);
        }

        /// <summary>
        /// Display a title test message...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdEchoTitletest(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // No data...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[0] == null))
            {
                DisplayError("echo.titletest needs one argument", a_functionarguments);
                return (false);
            }

            // Display it...
            DisplayYellow("", true);
            DisplayYellow("", true);
            DisplayYellow("", true);
            DisplayYellow(a_functionarguments.aszCmd[1], true);

            // All done...
            return (false);
        }

        /// <summary>
        /// Free a handle or a pointer...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdFree(ref Interpreter.FunctionArguments a_functionarguments)
        {
            UInt64 u64IntPtr = 0;
            IntPtr intptr = IntPtr.Zero;
            CallStack callstack = m_lcallstack[m_lcallstack.Count - 1];

            // Validate...
            if (a_functionarguments.aszCmd.Length != 3)
            {
                DisplayError("command needs 3 arguments", a_functionarguments);
                a_functionarguments.sts = TWAIN.STS.BADVALUE;
                a_functionarguments.szReturnValue = "";
                callstack.functionarguments.sts = a_functionarguments.sts;
                callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                return (false);
            }

            // Get the handle or pointer from the variable...
            bool blGlobal = false;
            KeyValue keyvalue = default(KeyValue);
            keyvalue.szKey = a_functionarguments.aszCmd[2];
            if (!GetVariable(keyvalue.szKey, -1, out keyvalue.szValue, out keyvalue.iBytes, out blGlobal, VariableScope.Local))
            {
                DisplayError("variable not found (make sure to set it to 0 at the top of the function)", a_functionarguments);
                a_functionarguments.sts = TWAIN.STS.BADVALUE;
                a_functionarguments.szReturnValue = "";
                callstack.functionarguments.sts = a_functionarguments.sts;
                callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                return (false);
            }

            // Get the value...
            if (!UInt64.TryParse(keyvalue.szValue, out u64IntPtr))
            {
                a_functionarguments.sts = TWAIN.STS.BADVALUE;
                a_functionarguments.szReturnValue = "0";
                callstack.functionarguments.sts = a_functionarguments.sts;
                callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                return (false);
            }

            // If the value is non-zero, free it and clear the variable...
            if (u64IntPtr > 0)
            {
                if (a_functionarguments.aszCmd[1] == "handle")
                {
                    intptr = (IntPtr)u64IntPtr;
                    m_twain.DsmMemFree(ref intptr);
                }
                else if (a_functionarguments.aszCmd[1] == "pointer")
                {
                    intptr = (IntPtr)u64IntPtr;
                    Marshal.FreeHGlobal(intptr);
                }
                else
                {
                    DisplayError("unrecognized flag <" + a_functionarguments.aszCmd[1] + ">", a_functionarguments);
                    a_functionarguments.sts = TWAIN.STS.BADVALUE;
                    a_functionarguments.szReturnValue = "0";
                    callstack.functionarguments.sts = a_functionarguments.sts;
                    callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    return (false);
                }
            }

            // All done (set the variable to 0)...
            a_functionarguments.sts = TWAIN.STS.SUCCESS;
            a_functionarguments.szReturnValue = "";
            callstack.functionarguments.sts = a_functionarguments.sts;
            callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
            SetVariable(keyvalue.szKey, "0", 0, blGlobal ? VariableScope.Global : VariableScope.Local);
            return (false);
        }

        /// <summary>
        /// Garbage collection, used to freak out the system and catch
        /// bugs that linger in places, like the bonjour interface...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdGc(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // Let's see if we can break things...
            GC.Collect();

            // All done...
            return (false);
        }

        /// <summary>
        /// Goto the user...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdGoto(ref Interpreter.FunctionArguments a_functionarguments)
        {
            int iLine;
            string szLabel;

            // Validate...
            if (    (a_functionarguments.aszScript == null)
                ||  (a_functionarguments.aszScript.Length < 2)
                ||  (a_functionarguments.aszScript[0] == null)
                ||  (a_functionarguments.aszCmd == null)
                ||  (a_functionarguments.aszCmd.Length < 2)
                ||  (a_functionarguments.aszCmd[1] == null))
            {
                return (false);
            }

            // Search for a match...
            szLabel = ":" + a_functionarguments.aszCmd[1];
            for (iLine = 0; iLine < a_functionarguments.aszScript.Length; iLine++)
            {
                if (a_functionarguments.aszScript[iLine].Trim() == szLabel)
                {
                    a_functionarguments.blGotoLabel = true;
                    a_functionarguments.iLabelLine = iLine;
                    return (false);
                }
            }

            // Ugh...
            DisplayError("goto label not found: <" + szLabel + ">", a_functionarguments);
            return (false);
        }

        /// <summary>
        /// Help the user...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdHelp(ref Interpreter.FunctionArguments a_functionarguments)
        {
            string szCommand;

            // Summary...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[1] == null))
            {
                Display(m_szBanner);
                Display("");
                DisplayRed("Overview");
                Display("help intro...................................introduction to this program");
                Display("help certification...........................certifying a scanner");
                Display("help scripting...............................general discussion of scripting");
                Display("");
                DisplayRed("Data Source Manager (DSM) commands");
                Display("certify [productname] [verbose]..............certify a TWAIN driver");
                Display("dsmload [args]...............................load the DSM");
                Display("dsmunload....................................unload the DSM");
                Display("dsmentry.....................................send a command to the DSM");
                Display("help [command]...............................this text or info about a command");
                Display("wait [timeout]...............................wait for a DAT_NULL message");
                Display("");
                DisplayRed("Scripting");
                Display("allocate {flag} {variable} {size}............allocate memory");
                Display("call {label}.................................call function");
                Display("cd [path]....................................shows or sets the current directory");
                Display("clean........................................clean the images folder");
                Display("dir..........................................lists files and folders in the current directory");
                Display("echo[.color] [text]..........................echo text");
                Display("echo.passfail {title} {result}...............echo test result in a tabular form");
                Display("echo.titlesuite {title}......................echo test suite");
                Display("echo.titletest {title}.......................echo test title");
                Display("free {flag} {variable}.......................free memory");
                Display("goto {label}.................................jump to the :label in the script");
                Display("if {item1} {operator} {item2} goto {label}...if statement");
                Display("increment {dst} {src} [step].................increment src by step and store in dst");
                Display("json2xml {file|json}.........................convert json formatted data to xml");
                Display("log {info|warn|error,etc} text...............add a line to the log file");
                Display("report {initialize {driver}|save {folder}}...self certification report");
                Display("return [status]..............................return from call function");
                Display("run [script].................................run a script");
                Display("runv [script]................................run a script verbosely");
                Display("setglobal [key [value]]......................show, set, or delete global keys");
                Display("setlocal [key [value]].......................show, set, or delete local keys");
                Display("sleep {milliseconds}.........................pause the current thread");
                return (false);
            }

            // Get the command...
            szCommand = a_functionarguments.aszCmd[1].ToLower();

            // Overview
            #region Overview

            // Scripting...
            if ((szCommand == "intro"))
            {
                /////////0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
                DisplayRed("INTRODUCTION TO THIS PROGRAM");
                Display("The TWAIN Certification program is an interpreter that interacts with TWAIN scanners.");
                Display("It's main purpose is to run certification scripts testing compliance with the TWAIN");
                Display("Specification.  It can also be used to test individual TWAIN commands with a high");
                Display("degree of granularity.  User's should be prepared to snuggle up with a copy of the");
                Display("TWAIN Specification if they want to get the most out of this program.  The most current");
                Display("version can be found at:  https://twain.org");
                Display("");
                Display("For help with scripting enter 'help scripting'.  It will also be instructive to look at the");
                Display("certification scripts, which are extensive, and exercise most of this program's features.");
                Display("");
                Display("For information about certifying scanners enter 'help certification'.");
                Display("");
                Display("Commands that may be of special interest are (use help for more info):");
                Display("  dsmload - load a DSM");
                Display("  dsmunload - unload a DSM");
                Display("  dsmentry - send a command to a DSM");
                Display("  quit - exit from this program");
                Display("  cd - change the current folder");
                Display("  run [script] - with no arguments lists scripts, or run a script");
                Display("");
                Display("The command prompt includes what the certification tool regards as the current TWAIN state,");
                Display("(ex: tw4>>>).  It's important to remember that while this should match the TWAIN state in");
                Display("the driver, there is no guarantee it is doing so.");
                return (false);
            }

            // Scripting...
            if ((szCommand == "certification") || (szCommand == "certify"))
            {
                /////////0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
                DisplayRed("CERTIFYING A SCANNER");
                Display("Certification is accomplished using scripts contained in the data/Certification folder.  These");
                Display("must be run, for both 32-bit and 64-bit systems, on all platforms supported by the driver.");
                Display("");
                Display("The following command runs certification:");
                Display("  certify [\"driver\"] [verbose]");
                Display("  Where,");
                Display("      driver - the TW_IDENTITY.ProductName of a TWAIN driver");
                Display("      verbose - if you want to see under the hood");
                Display("");
                Display("If no are given, the command shows a list of the installed TWAIN driver and prompts for the one");
                Display("to use.  Follow the instructions to complete the process.");
                Display("");
                Display("If debugging a problem the various scripts can be run separately.  Manually run the Opends");
                Display("script, and then the script you want to focus on.  Or write a script that includes the items");
                Display("that need testing.  The output from verbose is included in the output file, so that can also");
                Display("be a good place to start.");
                return (false);
            }

            // Scripting...
            if ((szCommand == "scripting"))
            {
                /////////0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
                DisplayRed("GENERAL DISCUSSION OF SCRIPTING");
                Display("The TWAIN Certification program is designed to test scanners.  It looks at DAT objects.  It's");
                Display("script based to make it easier to manage the tests.  Users can create and run their own tests,");
                Display("such as extracting key items from an existing test to make it easier to debug.");
                Display("");
                Display("The 'language' is not sophisticated.  It supports a goto, a conditional goto, and a call and");
                Display("run function.  The set and increment commands manage variables.  All of the TWAIN calls are");
                Display("accessible, including some extras used to stress the system.  Custom capabilities can be");
                Display("accessed using numbers.  Custom operations are not supported at this time (a flexible");
                Display("marshalling system would be needed).  The semicolon ';' is the comment indicator, it can appear");
                Display("by itself or at the end of a line.");
                Display("");
                Display("The most interesting part of the scripting support is variable expansion.  Variables take the");
                Display("form ${source:target} with the following available sources:");
                Display("");
                Display("  '${arg:[index.]target}'");
                Display("  Expands an argument argument to run, runv, or call.  A target of 0 is the name of the script");
                Display("  or label; 1 - n accesses the rest of the arguments.  An index can be specified to access any");
                Display("  command in the stack, but only 0 is recommended to look at the last user command.");
                Display("");
                Display("  '${bits:}'");
                Display("  32 or 64.");
                Display("");
                Display("  '${ds:}'");
                Display("  Complete TW_IDENTITY of the current scanner driver.");
                Display("");
                Display("  '${folder:target}'");
                Display("  Resolves to the full path for targeted special folder: certification, data, desktop, local,");
                Display("  pictures, roaming");
                Display("");
                Display("  '${format:specifier|value}'");
                Display("  Formats the value according to the specifier.");
                Display("");
                Display("  '${get:target}'");
                Display("  The value last assigned to the target using the set command.");
                Display("");
                Display("  '${getindex:target.index}'");
                Display("  Runs the target through CSV and returns the item at the requested index.");
                Display("");
                Display("  '${localtime:[format]}'");
                Display("  Returns the current local time using the DateTime format.");
                Display("");
                Display("  '${platform:}'");
                Display("  WINDOWS, LINUX, or MACOSX.");
                Display("");
                Display("  '${program:}'");
                Display("  Gets the name of the program running this script, its version, date, and machine word size.");
                Display("");
                Display("  '${report:}'");
                Display("  Full path to the generated self certification report (after 'report save command').");
                Display("");
                Display("  '${ret:[index]}'");
                Display("  The value supplied to the return command that ended the last run, runv, or call.  If an");
                Display("  index is supplied the string is run through CSV and the indexed element is returned.");
                Display("");
                Display("  '${state:}'");
                Display("  The current TWAIN state (1 through 7).");
                Display("");
                Display("  '${sts:}'");
                Display("  The TWAIN TWRC return code for the command.  If the status was TWRC_FAILURE, then this will");
                Display("  contain the TWCC condition code.");
                Display("");
                Display("Note that some tricks are allowed, one can do ${ret:${get:index}}, using the set and increment");
                Display("increment commands to enumerate through all of the fields returned in a TW_* structure.");
                return (false);
            }

            #endregion

            // Data Source Manager (DSM) commands
            #region Data Source Manager (DSM) commands

            // Help...
            if ((szCommand == "help"))
            {
                DisplayRed("HELP [COMMAND]");
                Display("Provides assistence with command and their arguments.  It does not");
                Display("go into detail on TWAIN.  Please read the Specification for more");
                Display("information.");
                Display("");
                Display("Curly brackets {} indicate mandatory arguments to a command.  Square");
                Display("brackets [] indicate optional arguments.");
                return (false);
            }

            // Dsmload...
            if ((szCommand == "dsmload"))
            {
                Assembly assembly = typeof(Terminal).Assembly;
                AssemblyName assemblyname = assembly.GetName();
                Version version = assemblyname.Version;

                DisplayRed("DSMLOAD");
                Display("Load the Data Source Manager (DSM).  This must be done before using");
                Display("dsmentry.  There are several arguments, which must be prefaced with");
                Display("their name:");
                Display("  manufacturer - tw_identity.Manufacturer (default='TWAIN Working Group')");
                Display("  productfamily - tw_identity.ProductFamily (default='TWAIN Open Source')");
                Display("  productname - tw_identity.ProductName (default='TWAIN Certification')");
                Display("  protocolmajor - tw_identity.ProtocolMajor (default=TWON_PROTOCOLMAJOR)");
                Display("  protocolminor - tw_identity.ProtocolMinor (default=TWON_PROTOCOLMINOR)");
                Display("  supportedgroups - tw_identity.SupportedGroups (default=DG_APP2|DG_CONTROL|DG_IMAGE)");
                Display("  twcy - tw_identity.Version.Country (default=TWCY_USA)");
                Display("  info - tw_identity.Version.Info (default='TWAIN Certification')");
                Display("  twlg - tw_identity.Version.Language (default=TWLG_ENGLISH)");
                Display("  majornum - tw_identity.Version.MajorNum (default=" + version.Major + ")");
                Display("  minornum - tw_identity.Version.MinorNum (default=" + version.Minor + ")");
                Display("  uselegacydsm - true|false (default=false)");
                Display("  usecallbacks - true|false (default=true, ignore if uselegacydsm is false)");
                return (false);
            }

            // Dsmload...
            if ((szCommand == "dsmunload"))
            {
                DisplayRed("DSMUNLOAD");
                Display("Unload the Data Source Manager (DSM).");
                return (false);
            }

            // Dsmentry...
            if ((szCommand == "dsmentry"))
            {
                DisplayRed("DSMENTRY src dst dg dat msg memref");
                Display("Send a command to the DSM with the following arguments:");
                Display("  src - source of the message (this application)");
                Display("  dst - destination, null for DSM commands, otherwise the Data Source");
                Display("  dg - Data group (DG_*)");
                Display("  dat - Data access type (DAT_*)");
                Display("  msg - Message");
                Display("  memref - the corresponding TW_* structure in CSV format");
                Display(" ");
                Display("Most commands support MSG_GET which can be used to get a look at the");
                Display("CSV format, which can be compared to the TW_* structure described in");
                Display("the TWAIN Specification.");
                return (false);
            }

            // Wait...
            if ((szCommand == "wait [reset|timeout]"))
            {
                DisplayRed("WAIT");
                Display("Wait for a DSM_NULL message, such as MSG_XFERREADY.  The message can");
                Display("arrive through the message pump on Windows, or the callback system on");
                Display("any of the platforms (assuming TWAINDSM is in use).");
                Display(" ");
                Display("The reset argument clears any pending messages.  It's recommended to");
                Display("do this before calls transitioning to state 5.");
                Display(" ");
                Display("The timeout is optional.  Specify it in milliseconds.  If the timeout");
                Display("is triggered the value of ${ret:} is 'timeout'.");
                Display(" ");
                Display("If one more messages are received they'll appear in ${ret:} as a comma");
                Display("separated list (ex: 'MSG_XFERREADY').");
                return (false);
            }

            // Quit...
            if ((szCommand == "quit"))
            {
                DisplayRed("QUIT");
                Display("Exit from this program.");
                return (false);
            }

            // Status...
            if ((szCommand == "status"))
            {
                DisplayRed("STATUS");
                Display("General information about the current operation of the program.");
                return (false);
            }

            #endregion

            // Scripting
            #region Scripting

            // Allocate...
            if ((szCommand == "allocate"))
            {
                DisplayRed("ALLOCATE {HANDLE|POINTER} {VARIABLE} {SIZE}");
                Display("Allocate a pointer or a handle of SIZE bytes and store the value in");
                Display("the specified variable.  On failure the value will be 0.");
                return (false);
            }

            // Call...
            else if ((szCommand == "call"))
            {
                DisplayRed("CALL {FUNCTION [argument1 [argument2 [...]]}");
                Display("Call a function with optional arguments.  Check '${ret:} to see what the");
                Display("function sent back with its RETURN command.  The function must be prefixed");
                Display("with a colon.  For example...");
                Display("  call XYZ");
                Display("  ; the script will return here");
                Display("  ...");
                Display("  :XYZ");
                Display("  return");
                Display("");
                Display("Gotos are easy to implement, and easy to script, but they can get out of");
                Display("control fast.  Keep functions small.  And when doing a goto inside of a");
                Display("function, use the function name as a prefix to help avoid reusing the same");
                Display("label in more than one place.  For example...");
                Display("  call XYZ abc");
                Display("  ; the script will return here");
                Display("  ...");
                Display("  :XYZ");
                Display("  if '${arg:1}' == 'abc' goto XYZ.ABC");
                Display("  return 'is not abc'");
                Display("  :XYZ.ABC");
                Display("  return 'is abc'");
                return (false);
            }

            // Cd...
            if ((szCommand == "cd"))
            {
                DisplayRed("CD [PATH]");
                Display("Show the current directory.  If a path is specified, change to that path.");
                return (false);
            }

            // Clean...
            if ((szCommand == "clean"))
            {
                DisplayRed("CLEAN");
                Display("Delete all files and folders in the images folder.");
                return (false);
            }

            // Dir...
            if ((szCommand == "dir"))
            {
                DisplayRed("DIR");
                Display("Directory command, lists files and folders in the current directory.");
                return (false);
            }

            // Echo...
            if ((szCommand == "echo"))
            {
                Display("ECHO[.COLOR] [TEXT]");
                Display("Echoes the text.  If there is no text an empty line is echoed.  The");
                return (false);
            }

            // Echop.assfail...
            if ((szCommand == "echo.passfail"))
            {
                DisplayRed("ECHOPASSFAIL [TITLE] [RESULT]");
                Display("Echoes the title and result in a tabular format.");
                return (false);
            }

            // Free...
            if ((szCommand == "free"))
            {
                DisplayRed("FREE {HANDLE|POINTER} {VARIABLE}");
                Display("Free a pointer or a handle in the specified variable.  If it");
                Display("is already 0, no action is taken.  On success the variable is");
                Display("set to 0.");
                return (false);
            }

            // Goto...
            if ((szCommand == "goto"))
            {
                DisplayRed("GOTO {LABEL}");
                Display("Jump to the specified label in the script.  The label must be");
                Display("prefixed with a colon.  For example...");
                Display("");
                Display("Examples");
                Display("  goto XYZ");
                Display("  :XYZ");
                return (false);
            }

            // If...
            if ((szCommand == "if"))
            {
                DisplayRed("IF {ITEM1} {OPERATOR} {ITEM2} [OPERATOR2 ITEM3] GOTO {LABEL}");
                Display("If the operator for ITEM1 and ITEM2 is true, then goto the");
                Display("label.  For the best experience get in the habit of putting");
                Display("either single or double quotes around the items.");
                Display("");
                Display("The & and | operators require the addition of a second operator");
                Display("(== or !=)and a third item for comparison with the result of the");
                Display("boolean operation.");
                Display("");
                Display("Operators");
                Display("==...........values are equal (case sensitive)");
                Display("<............item1 is numerically less than item2");
                Display("<=...........item1 is numerically less than or equal to item2");
                Display(">............item1 is numerically greater than item2");
                Display(">=...........item1 is numerically greater than or equal to item2");
                Display("&............item1 AND item2 compared to item3");
                Display("|............item1 OR item2 compared to item3");
                Display("~~...........values are equal (case insensitive)");
                Display("contains.....item2 is contained in item1 (case sensitive)");
                Display("~contains....item2 is contained in item1 (case insensitive)");
                Display("!=...........values are not equal (case sensitive)");
                Display("!~...........values are not equal (case insensitive)");
                Display("!contains....item2 is not contained in item1 (case sensitive)");
                Display("!~contains...item2 is not contained in item1 (case sensitive)");
                Display("");
                Display("Items");
                Display("See 'help scripting' for a list of the items that can be compared");
                Display("in addition to literals.");
                Display("");
                Display("Examples");
                Display("  if '${sts:}' != 'SUCCESS' goto FAIL");
                Display("  if '${get:value}' & '1' == '1' goto BITSET");
                return (false);
            }

            // Image...
            if ((szCommand == "image"))
            {
                DisplayRed("IMAGE {free} {variable}");
                DisplayRed("IMAGE {append} {variable} {TW_IMAGEMEMXFER}");
                DisplayRed("IMAGE {addheader} {variable} {TW_IMAGEINFO}");
                DisplayRed("IMAGE {delete} {folder}");
                DisplayRed("IMAGE {save} {variable} {native|memfile|memory} {filename}");
                Display("Use image append to chain together data from one or more DAT_IMAGEMEMXFER");
                Display("or DAT_IMAGEMEMFILEXFER calls.  Use image free to free that memory when");
                Display("done with it.");
                Display("");
                Display("Use image save to save an image to disk.  You can use the return value from");
                Display("DAT_IMAGENATIVEXFER to save a .bmp bitmap file.  If an extension isn't given");
                Display("one will be provided: .bmp, .jpg, or .tif.  Use image cleanfolder to specify");
                Display("a folder where .bmp, .jpg, and .tif files will be removed.");
                Display("");
                Display("Examples");
                Display("  image append mymemoryimage '${get:twimagememxfer}'");
                Display("  image addheader mymemoryimage  '${get:twimageinfo}'");
                Display("  image save mymemoryimage memfile ./myfile");
                Display("  image free mymemoryimage");
                Display("");
                Display("  image save twmemref memfile ./myfile");
                Display("");
                Display("  image append mymemfileimage '${get:twimagememxfer}'");
                Display("  image save mymemfileimage memfile ./myfile");
                Display("  image free mymemfileimage");
                Display("");
                return (false);
            }

            // Increment...
            if ((szCommand == "increment"))
            {
                DisplayRed("INCREMENT {DST} {SRC} [STEP]");
                Display("Increments SRC by STEP and stores in DST.  STEP defaults to 1.");
                return (false);
            }

            // pwd...
            if ((szCommand == "pwd"))
            {
                DisplayRed("PWD");
                Display("Show the path to the current working directory.");
                return (false);
            }

            // Report...
            if ((szCommand == "report"))
            {
                DisplayRed("REPORT {INITIALIZE | SAVE {FOLDER}}");
                Display("Initialize or save a self certification report.");
                return (false);
            }

            // Return...
            if ((szCommand == "return"))
            {
                DisplayRed("RETURN [DATA]");
                Display("Return data from a call function or a script invoked with RUN or");
                Display("RUNV.  The caller examines this value with the '${ret:}' symbol.");
                return (false);
            }

            // Run...
            if ((szCommand == "run"))
            {
                DisplayRed("RUN [SCRIPT]");
                Display("Runs the specified script.  SCRIPT is the full path to the script");
                Display("to be run.  If a SCRIPT is not specified, the scripts in the");
                Display("current folder are listed.");
                return (false);
            }

            // Run verbose...
            if ((szCommand == "runv"))
            {
                Display("RUNV [SCRIPT]");
                Display("Runs the specified script.  SCRIPT is the full path to the script");
                Display("to be run.  If a SCRIPT is not specified, the scripts in the");
                Display("current folder are listed.  The script commands are displayed.");
                return (false);
            }

            // Set...
            if ((szCommand == "setglobal"))
            {
                DisplayRed("SETGLOBAL {KEY} {VALUE}");
                Display("Set a key to the specified value.  If a KEY is not specified");
                Display("all of the current keys are listed with their values.  These");
                Display("are global to all scripts.  Use SETLOCAL when possible to");
                Display("prevent names from colliding.");
                Display("");
                Display("Values");
                Display("See 'help scripting' for information about the kinds of values");
                Display("that can be accessed in addition to simple literals.");
                return (false);
            }

            // Setlocal...
            if ((szCommand == "setlocal"))
            {
                DisplayRed("SETLOCAL {KEY} {VALUE}");
                Display("Set a key to the specified value.  If a KEY is not specified");
                Display("all of the current keys are listed with their values.  This");
                Display("key only exists in the context of the current function.  So");
                Display("it's safe to use the same value across invokations of 'call'");
                Display("or run, they won't interfere with each other.  Use SETGLOBAL");
                Display("command for values global to all scripts.");
                Display("");
                Display("Values");
                Display("See 'help scripting' for information about the kinds of values");
                Display("that can be accessed in addition to simple literals.");
                return (false);
            }

            // Sleep...
            if ((szCommand == "sleep"))
            {
                DisplayRed("SLEEP {MILLISECONDS}");
                Display("Pause the thread for the specified number of milliseconds.");
                return (false);
            }

            #endregion

            // Well, this ain't good...
            DisplayError("unrecognized command: " + a_functionarguments.aszCmd[1], a_functionarguments);

            // All done...
            return (false);
        }

        /// <summary>
        /// Certify a TWAIN driver...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdCertify(ref Interpreter.FunctionArguments a_functionarguments)
        {
            bool blRunningScriptRestore = m_blRunningScript;
            bool blVerboseRestore = m_blVerbose;
            bool blTwainSuccess = false;
            string szSelection = "";
            Interpreter interpreter = new Interpreter("", m_consolecolorDefault, m_consolecolorGreen);

            // Gotta pretend we're in a script...
            m_blRunningScript = true;

            // If we have arguments, drop them in...
            if ((a_functionarguments.aszCmd != null) && (a_functionarguments.aszCmd.Length > 1) && (a_functionarguments.aszCmd[1] != null))
            {
                for (int aa = 1; aa < a_functionarguments.aszCmd.Length; aa++)
                {
                    if (a_functionarguments.aszCmd[aa].ToLowerInvariant() == "verbose")
                    {
                        m_blVerbose = true;
                    }
                    else
                    {
                        szSelection = a_functionarguments.aszCmd[aa];
                    }
                }
            }

            // Tell the user the plan...
            DisplayYellow("TWAIN Certification");
            Display("  This tool certifies TWAIN drivers.  To complete certification it must be run on.");
            Display("  all platforms supported by the driver:  Windows, Linux, macOS for 32-bit and 64-bit.");
            Display("");
            Display("  The tool will ask for some information, and begin the test, which only takes a");
            Display("  few minutes to complete.  On success the tool will provide instructions on how");
            Display("  submit information about a TWAIN driver that has passed certification.");

            // Show the available scanners, so the user can pick one...
            if (string.IsNullOrEmpty(szSelection))
            {
                List<string> aszDrivers = new List<string>();
                Interpreter.FunctionArguments functionarguments;

                // Load the DSM...
                functionarguments = new Interpreter.FunctionArguments();
                functionarguments.aszCmd = new string[] { "dsmload" };
                CmdDsmLoad(ref functionarguments);
                if (functionarguments.sts != TWAIN.STS.SUCCESS)
                {
                    Display("");
                    DisplayRed("dsmload error...");
                    m_blRunningScript = blRunningScriptRestore;
                    m_blVerbose = blVerboseRestore;
                    return (false);
                }

                // Open the DSM...
                functionarguments = new Interpreter.FunctionArguments();
                functionarguments.aszCmd = new string[] { "dsmentry", "src", "null", "dg_control", "dat_parent", "msg_opendsm", "hwnd" };
                CmdDsmEntry(ref functionarguments);
                if (functionarguments.sts != TWAIN.STS.SUCCESS)
                {
                    Display("");
                    DisplayRed("dsmopen error...");
                    m_blRunningScript = blRunningScriptRestore;
                    m_blVerbose = blVerboseRestore;
                    return (false);
                }

                // Walk the driver list...
                functionarguments = new Interpreter.FunctionArguments();
                functionarguments.aszCmd = new string[] { "dsmentry", "src", "null", "dg_control", "dat_identity", "msg_getfirst", "0,0,0,ENGLISH_USA,USA,,0,0,0x0,,," };
                while (true)
                {
                    // Ask for this item...
                    CmdDsmEntry(ref functionarguments);
                    if (functionarguments.sts != TWAIN.STS.SUCCESS)
                    {
                        break;
                    }

                    // Parse out the data...
                    string[] aszTwidentity = CSV.Parse(functionarguments.szReturnValue);
                    if (aszTwidentity.Length == 12)
                    {
                        aszDrivers.Add(aszTwidentity[11]);
                    }

                    // Next entry...
                    functionarguments = new Interpreter.FunctionArguments();
                    functionarguments.aszCmd = new string[] { "dsmentry", "src", "null", "dg_control", "dat_identity", "msg_getnext", "0,0,0,ENGLISH_USA,USA,,0,0,0x0,,," };
                }

                // Close the DSM...
                functionarguments = new Interpreter.FunctionArguments();
                functionarguments.aszCmd = new string[] { "dsmentry", "src", "null", "dg_control", "dat_parent", "msg_closedsm", "hwnd" };

                // Unload the DSM...
                functionarguments = new Interpreter.FunctionArguments();
                functionarguments.aszCmd = new string[] { "dsmunload" };
                CmdDsmUnload(ref functionarguments);

                // Ruh-roh...
                if (aszDrivers.Count == 0)
                {
                    Display("");
                    DisplayRed("No drivers found...");
                    m_blRunningScript = blRunningScriptRestore;
                    m_blVerbose = blVerboseRestore;
                    return (false);
                }

                // Sort it...
                aszDrivers.Sort();

                // What we found...
                Display("");
                DisplayYellow("Available TWAIN Drivers:");
                foreach (string szDriver in aszDrivers)
                {
                    Display("  " + szDriver);
                }

                // Ask for a scanner...
                bool blFound = false;
                Display("");
                Display("Please enter the name of a scanner from the list above (or quit to get out):");
                Display("(partial names are okay, as long as they are unique)");
                while (true)
                {
                    interpreter.SetPrompt("certify driver>>> ");
                    szSelection = interpreter.Prompt(m_streamreaderConsole, 0);
                    if (   (szSelection.ToLowerInvariant() == "quit")
                        || (szSelection.ToLowerInvariant() == "q"))
                    {
                        szSelection = "";
                        m_blRunningScript = blRunningScriptRestore;
                        m_blVerbose = blVerboseRestore;
                        return (false);
                    }
                    if (szSelection.Length > 0)
                    {
                        foreach (string szDriver in aszDrivers)
                        {
                            if (szDriver.ToLowerInvariant().Contains(szSelection.ToLowerInvariant()))
                            {
                                szSelection = szDriver;
                                blFound = true;
                                break;
                            }
                        }
                        if (blFound)
                        {
                            break;
                        }
                    }
                }
            }

            // We're bailing...
            if (string.IsNullOrEmpty(szSelection))
            {
                m_blRunningScript = blRunningScriptRestore;
                m_blVerbose = blVerboseRestore;
                return (false);
            }

            // Confirmation to proceed...
            Display("");
            while (true)
            {
                interpreter.SetPrompt("Certify '" + szSelection + "'" + " (yes/no)? ");
                string szAnswer = interpreter.Prompt(m_streamreaderConsole, 0);
                if (szAnswer.ToLowerInvariant().StartsWith("y"))
                {
                    break;
                }
                else if (szAnswer.ToLowerInvariant().StartsWith("n"))
                {
                    m_blRunningScript = blRunningScriptRestore;
                    m_blVerbose = blVerboseRestore;
                    return (false);
                }
            }

            // Let's do it!
            {
                Display("");
                Display("");
                Display("");

                // Try our binary path first, if that fails use the current folder...
                string szCertificationFolder = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Path.Combine("data", "Certification"));
                if (!Directory.Exists(szCertificationFolder))
                {
                    szCertificationFolder = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("data", "Certification"));
                    if (!Directory.Exists(szCertificationFolder))
                    {
                        Display("");
                        DisplayRed("Can't find our data/Certification folder, it should be in the");
                        DisplayRed("same folder as this application, or the current folder.");
                        m_blRunningScript = blRunningScriptRestore;
                        m_blVerbose = blVerboseRestore;
                        return (false);
                    }
                }

                // Go there and fire up the intepreter...
                string szCurrentDirectory = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(szCertificationFolder);
                Interpreter.FunctionArguments functionarguments = new Interpreter.FunctionArguments();
                functionarguments = new Interpreter.FunctionArguments();
                if (m_blVerbose)
                {
                    functionarguments.aszCmd = new string[3] { "runv", "certification", szSelection };
                }
                else
                {
                    functionarguments.aszCmd = new string[3] { "run", "certification", szSelection };
                }
                CmdRun(ref functionarguments);
                blTwainSuccess = (functionarguments.szReturnValue == "pass");
                Directory.SetCurrentDirectory(szCurrentDirectory);
            }

            // Report successful results, and point the user to twaindirect.org...
            if (blTwainSuccess)
            {
                Display("");
                Display("");
                Display("");
                DisplayGreen("TWAIN Certification passed...");
                Display("If you are the manufacturer of this driver, please go to the TWAIN");
                Display("website at https://twain.org to register your scanner.  There is a");
                Display("'TWAIN Self Certification' folder on your desktop that contains the");
                Display("report for this run.");
            }

            // Report unsuccessful results, and point the user to twaindirect.org...
            else
            {
                Display("");
                Display("");
                Display("");
                DisplayRed("TWAIN Certification failed...");
                Display("Please refer to the log files in the 'TWAIN Self Certification' folder");
                Display("on your desktop for additional information.");
            }

            // All done...
            m_blRunningScript = blRunningScriptRestore;
            m_blVerbose = blVerboseRestore;
            return (false);
        }

        /// <summary>
        /// Process an if-statement...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdIf(ref Interpreter.FunctionArguments a_functionarguments)
        {
            int iAction = 0;
            bool blDoAction = false;
            string szItem1 = "";
            string szItem2 = "";
            string szItem3 = "";
            string szOperator = "";
            string szOperator2 = "";

            // Get all of the stuff (logical operators need more)...
            if (   (a_functionarguments.aszCmd[2] == "&")
                || (a_functionarguments.aszCmd[2] == "|"))
            {
                if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 7) || (a_functionarguments.aszCmd[1] == null))
                {
                    DisplayError("badly formed if-statement", a_functionarguments);
                    return (false);
                }
                szItem1 = a_functionarguments.aszCmd[1];
                szOperator = a_functionarguments.aszCmd[2];
                szItem2 = a_functionarguments.aszCmd[3];
                szOperator2 = a_functionarguments.aszCmd[4];
                szItem3 = a_functionarguments.aszCmd[5];
                iAction = 6;
            }
            // Everybody else needs less stuff...
            else
            {
                if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 5) || (a_functionarguments.aszCmd[1] == null))
                {
                    DisplayError("badly formed if-statement", a_functionarguments);
                    return (false);
                }
                szItem1 = a_functionarguments.aszCmd[1];
                szOperator = a_functionarguments.aszCmd[2];
                szItem2 = a_functionarguments.aszCmd[3];
                iAction = 4;
            }

            // Items must match (case sensitive)...
            if (szOperator == "==")
            {
                if (szItem1 == szItem2)
                {
                    blDoAction = true;
                }
            }

            // Items must match (case insensitive)...
            else if (szOperator == "~~")
            {
                if (szItem1.ToLowerInvariant() == szItem2.ToLowerInvariant())
                {
                    blDoAction = true;
                }
            }

            // Items must not match (case sensitive)...
            else if (szOperator == "!=")
            {
                if (szItem1 != szItem2)
                {
                    blDoAction = true;
                }
            }

            // Items must not match (case insensitive)...
            else if (szOperator == "!~")
            {
                if (szItem1.ToLowerInvariant() != szItem2.ToLowerInvariant())
                {
                    blDoAction = true;
                }
            }

            // Item1 > Item2...
            else if (szOperator == ">")
            {
                int iItem1;
                int iItem2;
                if (!int.TryParse(szItem1, out iItem1))
                {
                    DisplayError("<" + szItem1 + "> > <" + szItem2 + "> is invalid", a_functionarguments);
                }
                else
                {
                    if (!int.TryParse(szItem2, out iItem2))
                    {
                        DisplayError("<" + szItem1 + "> > <" + szItem2 + "> is invalid", a_functionarguments);
                    }
                    else
                    {
                        if (iItem1 > iItem2)
                        {
                            blDoAction = true;
                        }
                    }
                }
            }

            // Item1 >= Item2...
            else if (szOperator == ">=")
            {
                int iItem1;
                int iItem2;
                if (!int.TryParse(szItem1, out iItem1))
                {
                    DisplayError("<" + szItem1 + "> >= <" + szItem2 + "> is invalid", a_functionarguments);
                }
                else
                {
                    if (!int.TryParse(szItem2, out iItem2))
                    {
                        DisplayError("<" + szItem1 + "> >= <" + szItem2 + "> is invalid", a_functionarguments);
                    }
                    else
                    {
                        if (iItem1 >= iItem2)
                        {
                            blDoAction = true;
                        }
                    }
                }
            }

            // Item1 < Item2...
            else if (szOperator == "<")
            {
                int iItem1;
                int iItem2;
                if (!int.TryParse(szItem1, out iItem1))
                {
                    DisplayError("<" + szItem1 + "> < <" + szItem2 + "> is invalid", a_functionarguments);
                }
                else
                {
                    if (!int.TryParse(szItem2, out iItem2))
                    {
                        DisplayError("<" + szItem1 + "> < <" + szItem2 + "> is invalid", a_functionarguments);
                    }
                    else
                    {
                        if (iItem1 < iItem2)
                        {
                            blDoAction = true;
                        }
                    }
                }
            }

            // Item1 <= Item2...
            else if (szOperator == "<=")
            {
                int iItem1;
                int iItem2;
                if (!int.TryParse(szItem1, out iItem1))
                {
                    DisplayError("<" + szItem1 + "> <= <" + szItem2 + "> is invalid", a_functionarguments);
                }
                else
                {
                    if (!int.TryParse(szItem2, out iItem2))
                    {
                        DisplayError("<" + szItem1 + "> <= <" + szItem2 + "> is invalid", a_functionarguments);
                    }
                    else
                    {
                        if (iItem1 <= iItem2)
                        {
                            blDoAction = true;
                        }
                    }
                }
            }

            // Bit-wise AND, always compared to a target...
            else if (szOperator == "&")
            {
                int iAnd = 0;
                int iItem1 = 0;
                int iItem2 = 0;
                int iItem3 = 0;
                bool blItem1 = false;
                bool blItem2 = false;
                bool blItem3 = false;
                if (szItem1.ToLowerInvariant().StartsWith("0x"))
                {
                    blItem1 = int.TryParse(szItem1.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iItem1);
                }
                else
                {
                    blItem1 = int.TryParse(szItem1, out iItem1);
                }
                if (szItem2.ToLowerInvariant().StartsWith("0x"))
                {
                    blItem2 = int.TryParse(szItem2.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iItem2);
                }
                else
                {
                    blItem2 = int.TryParse(szItem2, out iItem2);
                }
                if (szItem3.ToLowerInvariant().StartsWith("0x"))
                {
                    blItem3 = int.TryParse(szItem3.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iItem3);
                }
                else
                {
                    blItem3 = int.TryParse(szItem3, out iItem3);
                }
                if (!blItem1 || !blItem2 || !blItem3)
                {
                    DisplayError("badly formed if-statement...", a_functionarguments);
                    return (false);
                }
                iAnd = iItem1 & iItem2;
                if ((szOperator2 == "==") && (iAnd == iItem3))
                {
                    blDoAction = true;
                }
                else if ((szOperator2 == "!=") && (iAnd != iItem3))
                {
                    blDoAction = true;
                }
                else
                {
                    blDoAction = false;
                }
            }

            // Bit-wise OR, always compared to a target...
            else if (szOperator == "|")
            {
                int iOr = 0;
                int iItem1 = 0;
                int iItem2 = 0;
                int iItem3 = 0;
                bool blItem1 = false;
                bool blItem2 = false;
                bool blItem3 = false;
                if (szItem1.ToLowerInvariant().StartsWith("0x"))
                {
                    blItem1 = int.TryParse(szItem1.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iItem1);
                }
                else
                {
                    blItem1 = int.TryParse(szItem1, out iItem1);
                }
                if (szItem2.ToLowerInvariant().StartsWith("0x"))
                {
                    blItem2 = int.TryParse(szItem2.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iItem2);
                }
                else
                {
                    blItem2 = int.TryParse(szItem2, out iItem2);
                }
                if (szItem3.ToLowerInvariant().StartsWith("0x"))
                {
                    blItem3 = int.TryParse(szItem3.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iItem3);
                }
                else
                {
                    blItem3 = int.TryParse(szItem3, out iItem3);
                }
                if (!blItem1 || !blItem2 || !blItem3)
                {
                    DisplayError("badly formed if-statement", a_functionarguments);
                    return (false);
                }
                iOr = iItem1 | iItem2;
                if ((szOperator2 == "==") && (iOr == iItem3))
                {
                    blDoAction = true;
                }
                else if ((szOperator2 == "!=") && (iOr != iItem3))
                {
                    blDoAction = true;
                }
                else
                {
                    blDoAction = false;
                }
            }

            // Item1 must contain items2 (case sensitive)...
            else if (szOperator == "contains")
            {
                if (szItem1.Contains(szItem2))
                {
                    blDoAction = true;
                }
            }

            // Item1 must contain items2 (case insensitive)...
            else if (szOperator == "~contains")
            {
                if (szItem1.ToLowerInvariant().Contains(szItem2.ToLowerInvariant()))
                {
                    blDoAction = true;
                }
            }

            // Item1 must not contain items2 (case sensitive)...
            else if (szOperator == "!contains")
            {
                if (!szItem1.Contains(szItem2))
                {
                    blDoAction = true;
                }
            }

            // Item1 must not contain items2 (case insensitive)...
            else if (szOperator == "!~contains")
            {
                if (!szItem1.ToLowerInvariant().Contains(szItem2.ToLowerInvariant()))
                {
                    blDoAction = true;
                }
            }

            // Unrecognized operator...
            else
            {
                DisplayError("unrecognized operator: <" + szOperator + ">", a_functionarguments);
                return (false);
            }

            // We've been told to do the action...
            if (blDoAction)
            {
                // We're doing a goto...
                if (a_functionarguments.aszCmd[iAction].ToLowerInvariant() == "goto")
                {
                    int iLine;
                    string szLabel;

                    // Validate...
                    if ((a_functionarguments.aszCmd.Length < (iAction + 1)) || string.IsNullOrEmpty(a_functionarguments.aszCmd[iAction + 1]))
                    {
                        DisplayError("goto label is missing...", a_functionarguments);
                        return (false);
                    }

                    // Find the label...
                    szLabel = ":" + a_functionarguments.aszCmd[iAction + 1];
                    for (iLine = 0; iLine < a_functionarguments.aszScript.Length; iLine++)
                    {
                        if (a_functionarguments.aszScript[iLine].Trim() == szLabel)
                        {
                            a_functionarguments.blGotoLabel = true;
                            a_functionarguments.iLabelLine = iLine;
                            return (false);
                        }
                    }

                    // Ugh...
                    DisplayError("goto label not found: <" + szLabel + ">", a_functionarguments);
                    return (false);
                }

                // We have no idea what we're doing...
                else
                {
                    DisplayError("unrecognized action: <" + a_functionarguments.aszCmd[iAction] + ">", a_functionarguments);
                    return (false);
                }
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Manage an image byte array for DAT_IMAGEMEMXFER or DAT_IMAGEMEMFILEXFER...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdImage(ref Interpreter.FunctionArguments a_functionarguments)
        {
            int iResult = 0;
            UInt64 u64Ptr = 0;
            IntPtr intptrPtr = IntPtr.Zero;
            bool blGlobal = false;
            byte[] abImage;
            string szFilename = "";
            KeyValue keyvalue = default(KeyValue);
            TWAIN.TW_IMAGEINFO twimageinfo = default(TWAIN.TW_IMAGEINFO);
            TWAIN.TW_IMAGEMEMXFER twimagememxfer = default(TWAIN.TW_IMAGEMEMXFER);

            // Validate...
            if (a_functionarguments.aszCmd.Length < 2)
            {
                DisplayError("must specify an action for the image command", a_functionarguments);
                return (false);
            }

            // Pick an action...
            switch (a_functionarguments.aszCmd[1].ToLowerInvariant())
            {
                // Oops...
                default:
                    DisplayError("unrecognized action", a_functionarguments);
                    return (false);

                // Add a header to data (this is always going to be TIFF)...
                case "addheader":
                    // Validate
                    if (a_functionarguments.aszCmd.Length < 4)
                    {
                        DisplayError("must specify a variable and tw_imageinfo data", a_functionarguments);
                        return (false);
                    }

                    // Validate...
                    keyvalue.szKey = a_functionarguments.aszCmd[2];
                    if (!m_twain.CsvToImageinfo(ref twimageinfo, a_functionarguments.aszCmd[3]))
                    {
                        DisplayError("invalid tw_imageinfo data <" + a_functionarguments.aszCmd[3] + ">", a_functionarguments);
                        return (false);
                    }

                    // Find the local value for this key...
                    GetVariable(keyvalue.szKey, -1, out keyvalue.szValue, out keyvalue.iBytes, out blGlobal, VariableScope.Local);

                    // If we got a pointer
                    if (!UInt64.TryParse(keyvalue.szValue, out u64Ptr))
                    {
                        DisplayError("bad pointer <" + keyvalue.szValue + ">", a_functionarguments);
                        return (false);
                    }

                    // Ruh-roh...
                    if (u64Ptr == 0)
                    {
                        DisplayError("null pointer <" + a_functionarguments.aszCmd[2] + ">", a_functionarguments);
                        return (false);
                    }

                    // The pointer to our raw image data...
                    intptrPtr = (IntPtr)u64Ptr;

                    // Bitonal uncompressed...
                    if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.BW) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.NONE))
                    {
                        TWAIN.TiffBitonalUncompressed tiffbitonaluncompressed;
                        tiffbitonaluncompressed = new TWAIN.TiffBitonalUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)keyvalue.iBytes);
                        intptrPtr = Marshal.ReAllocHGlobal(intptrPtr, (IntPtr)(Marshal.SizeOf(tiffbitonaluncompressed) + keyvalue.iBytes));
                        NativeMethods.MemMove((IntPtr)((UInt64)intptrPtr + (UInt64)Marshal.SizeOf(tiffbitonaluncompressed)), intptrPtr, keyvalue.iBytes);
                        Marshal.StructureToPtr(tiffbitonaluncompressed, intptrPtr, true);
                        SetVariable(keyvalue.szKey, keyvalue.szValue, (int)(Marshal.SizeOf(tiffbitonaluncompressed) + keyvalue.iBytes), VariableScope.Local);
                    }

                    // Bitonal GROUP4...
                    else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.BW) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.GROUP4))
                    {
                        TWAIN.TiffBitonalG4 tiffbitonalg4;
                        tiffbitonalg4 = new TWAIN.TiffBitonalG4((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)keyvalue.iBytes);
                        intptrPtr = Marshal.ReAllocHGlobal(intptrPtr, (IntPtr)(Marshal.SizeOf(tiffbitonalg4) + keyvalue.iBytes));
                        NativeMethods.MemMove((IntPtr)((UInt64)intptrPtr + (UInt64)Marshal.SizeOf(tiffbitonalg4)), intptrPtr, keyvalue.iBytes);
                        Marshal.StructureToPtr(tiffbitonalg4, intptrPtr, true);
                        SetVariable(keyvalue.szKey, keyvalue.szValue, (int)(Marshal.SizeOf(tiffbitonalg4) + keyvalue.iBytes), VariableScope.Local);
                    }

                    // Gray uncompressed...
                    else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.GRAY) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.NONE))
                    {
                        TWAIN.TiffGrayscaleUncompressed tiffgrayscaleuncompressed;
                        tiffgrayscaleuncompressed = new TWAIN.TiffGrayscaleUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)keyvalue.iBytes);
                        intptrPtr = Marshal.ReAllocHGlobal(intptrPtr, (IntPtr)(Marshal.SizeOf(tiffgrayscaleuncompressed) + keyvalue.iBytes));
                        NativeMethods.MemMove((IntPtr)((UInt64)intptrPtr + (UInt64)Marshal.SizeOf(tiffgrayscaleuncompressed)), intptrPtr, keyvalue.iBytes);
                        Marshal.StructureToPtr(tiffgrayscaleuncompressed, intptrPtr, true);
                        SetVariable(keyvalue.szKey, keyvalue.szValue, (int)(Marshal.SizeOf(tiffgrayscaleuncompressed) + keyvalue.iBytes), VariableScope.Local);
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
                        tiffcoloruncompressed = new TWAIN.TiffColorUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)keyvalue.iBytes);
                        intptrPtr = Marshal.ReAllocHGlobal(intptrPtr, (IntPtr)(Marshal.SizeOf(tiffcoloruncompressed) + keyvalue.iBytes));
                        NativeMethods.MemMove((IntPtr)((UInt64)intptrPtr + (UInt64)Marshal.SizeOf(tiffcoloruncompressed)), intptrPtr, keyvalue.iBytes);
                        Marshal.StructureToPtr(tiffcoloruncompressed, intptrPtr, true);
                        SetVariable(keyvalue.szKey, keyvalue.szValue, (int)(Marshal.SizeOf(tiffcoloruncompressed) + keyvalue.iBytes), VariableScope.Local);
                    }

                    // RGB JPEG...
                    else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.RGB) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.JPEG))
                    {
                        // No work to be done, we'll output JPEG...
                    }

                    // Oh well...
                    else
                    {
                        DisplayError("unsupported format <" + twimageinfo.PixelType + "," + twimageinfo.Compression + ">", a_functionarguments);
                        return (false);
                    }
                    return (false);

                // Append (grow the memory)...
                case "append":
                    // Validate
                    if (a_functionarguments.aszCmd.Length < 4)
                    {
                        DisplayError("must specify a variable and tw_imagememxfer data", a_functionarguments);
                        return (false);
                    }

                    // Validate...
                    keyvalue.szKey = a_functionarguments.aszCmd[2];
                    if (!m_twain.CsvToImagememxfer(ref twimagememxfer, a_functionarguments.aszCmd[3]))
                    {
                        DisplayError("invalid tw_imagememxfer data <" + a_functionarguments.aszCmd[3] + ">", a_functionarguments);
                        return (false);
                    }

                    // Validate the pointer...
                    if (twimagememxfer.Memory.TheMem == IntPtr.Zero)
                    {
                        DisplayError("null tw_imagememxfer.memory.themem data <" + a_functionarguments.aszCmd[3] + ">", a_functionarguments);
                        return (false);
                    }

                    // Only do this bit of there's something to do...
                    if (twimagememxfer.BytesWritten > 0)
                    {
                        keyvalue.szKey = a_functionarguments.aszCmd[2];
                        GetVariable(keyvalue.szKey, -1, out keyvalue.szValue, out keyvalue.iBytes, out blGlobal, VariableScope.Local);
                        if (string.IsNullOrEmpty(keyvalue.szValue) || UInt64.TryParse(keyvalue.szValue, out u64Ptr))
                        {
                            // We're starting fresh...
                            if (u64Ptr == 0)
                            {
                                intptrPtr = Marshal.AllocHGlobal((int)twimagememxfer.BytesWritten);
                                NativeMethods.MemCpy(intptrPtr, twimagememxfer.Memory.TheMem, (int)twimagememxfer.BytesWritten);
                                SetVariable(keyvalue.szKey, intptrPtr.ToString(), (int)twimagememxfer.BytesWritten, VariableScope.Local);
                            }
                            // We're appending...
                            else
                            {
                                intptrPtr = (IntPtr)u64Ptr;
                                intptrPtr = Marshal.ReAllocHGlobal(intptrPtr, (IntPtr)(keyvalue.iBytes + (int)twimagememxfer.BytesWritten));
                                NativeMethods.MemCpy((IntPtr)((UInt64)intptrPtr + (UInt64)keyvalue.iBytes), twimagememxfer.Memory.TheMem, (int)twimagememxfer.BytesWritten);
                                SetVariable(keyvalue.szKey, intptrPtr.ToString(), keyvalue.iBytes + (int)twimagememxfer.BytesWritten, VariableScope.Local);
                            }
                        }
                    }
                    return (false);

                // Cleanfolder...
                case "cleanfolder":
                    // Validate
                    if (a_functionarguments.aszCmd.Length != 3)
                    {
                        DisplayError("must specify 3 arguments", a_functionarguments);
                        return (false);
                    }
                    if (!Directory.Exists(a_functionarguments.aszCmd[2]))
                    {
                        try { Directory.CreateDirectory(a_functionarguments.aszCmd[2]); } catch { /* don't care */ }
                    }
                    else
                    {
                        string[] aszFiles = Directory.GetFiles(a_functionarguments.aszCmd[2], "*.bmp");
                        foreach (string szFile in aszFiles) try { File.Delete(szFile); } catch { /* don't care */ }
                        aszFiles = Directory.GetFiles(a_functionarguments.aszCmd[2], "*.tif");
                        foreach (string szFile in aszFiles) try { File.Delete(szFile); } catch { /* don't care */ }
                        aszFiles = Directory.GetFiles(a_functionarguments.aszCmd[2], "*.jpg");
                        foreach (string szFile in aszFiles) try { File.Delete(szFile); } catch { /* don't care */ }
                    }
                    return (false);

                // Free...
                case "free":
                    // Validate
                    if (a_functionarguments.aszCmd.Length < 3)
                    {
                        DisplayError("must specify a variable for the image free command", a_functionarguments);
                        return (false);
                    }

                    // Get the value...
                    keyvalue.szKey = a_functionarguments.aszCmd[2];
                    GetVariable(keyvalue.szKey, -1, out keyvalue.szValue, out keyvalue.iBytes, out blGlobal, VariableScope.Local);

                    // If we have a pointer, free it, and set the varible to 0...
                    if (UInt64.TryParse(keyvalue.szValue, out u64Ptr))
                    {
                        intptrPtr = (IntPtr)u64Ptr;
                        if (intptrPtr != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(intptrPtr);
                            SetVariable(keyvalue.szKey, "0", 0, VariableScope.Local);
                        }
                    }
                    return (false);

                // Save...
                case "save":
                    // Validate
                    if (a_functionarguments.aszCmd.Length < 5)
                    {
                        DisplayError("must specify a variable, xfermech, and a filename for the image save command", a_functionarguments);
                        return (false);
                    }

                    // More validating...
                    if ((a_functionarguments.aszCmd[3].ToLowerInvariant() != "native") && (a_functionarguments.aszCmd[3].ToLowerInvariant() != "memfile") && (a_functionarguments.aszCmd[3].ToLowerInvariant() != "memory"))
                    {
                        DisplayError("unrecognized xfermech <" + a_functionarguments.aszCmd[3] + ">", a_functionarguments);
                        return (false);
                    }

                    // Get the value...
                    keyvalue.szKey = a_functionarguments.aszCmd[2];
                    GetVariable(keyvalue.szKey, -1, out keyvalue.szValue, out keyvalue.iBytes, out blGlobal, VariableScope.Local);

                    // Validate the pointer...
                    if (!UInt64.TryParse(keyvalue.szValue, out u64Ptr))
                    {
                        DisplayError("bad pointer <" + keyvalue.szKey + ">", a_functionarguments);
                        return (false);
                    }
                    if (u64Ptr == 0)
                    {
                        DisplayError("null pointer <" + keyvalue.szKey + ">", a_functionarguments);
                        return (false);
                    }

                    // Make sure the folder exists...
                    if (!Directory.Exists(Path.GetDirectoryName(a_functionarguments.aszCmd[4])))
                    {
                        try { Directory.CreateDirectory(Path.GetDirectoryName(a_functionarguments.aszCmd[4])); } catch { /* don't care */ }
                    }

                    // Okay, we're ready to save...
                    intptrPtr = (IntPtr)u64Ptr;
                    switch (a_functionarguments.aszCmd[3].ToLowerInvariant())
                    {
                        default:
                        case "native":
                            abImage = m_twain.NativeToByteArray(intptrPtr, true);
                            try
                            {
                                szFilename = a_functionarguments.aszCmd[4];
                                if (!Path.GetFileName(szFilename).Contains("."))
                                {
                                    szFilename += ".bmp";
                                }
                                File.WriteAllBytes(szFilename, abImage);
                            }
                            catch (Exception exception)
                            {
                                DisplayError("image save failed <" + szFilename + "> - " + exception.Message, a_functionarguments);
                                return (false);
                            }
                            break;

                        case "memfile":
                        case "memory":
                            iResult = NativeMethods.WriteImageFile(a_functionarguments.aszCmd[4], intptrPtr, keyvalue.iBytes);
                            if (iResult == -1)
                            {
                                DisplayError("image save failed <" + a_functionarguments.aszCmd[4] + ">", a_functionarguments);
                                return (false);
                            }
                            break;
                    }
                    return (false);
            }
        }

        /// <summary>
        /// Return from the current function...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdIncrement(ref Interpreter.FunctionArguments a_functionarguments)
        {
            int iSrc;
            int iDst;
            int iStep = 1;

            // Validate...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 3) || (a_functionarguments.aszCmd[1] == null))
            {
                DisplayError("badly formed increment", a_functionarguments);
                return (false);
            }

            // Turn the source into a number...
            if (!int.TryParse(a_functionarguments.aszCmd[2], out iSrc))
            {
                DisplayError("source is not a number", a_functionarguments);
                return (false);
            }

            // Get the step...
            if ((a_functionarguments.aszCmd.Length >= 4) && (a_functionarguments.aszCmd[3] != null))
            {
                if (!int.TryParse(a_functionarguments.aszCmd[3], out iStep))
                {
                    DisplayError("step is not a number", a_functionarguments);
                    return (false);
                }
            }

            // Increment the value...
            iDst = iSrc + iStep;

            // Gotta know if the value is local or global, local wins so we'll search there...
            bool blGlobal = true;
            if ((m_lcallstack.Count > 0) && (m_lcallstack[m_lcallstack.Count - 1].lkeyvalue != null))
            {
                foreach (KeyValue keyvalue in m_lcallstack[m_lcallstack.Count - 1].lkeyvalue)
                {
                    if (keyvalue.szKey == a_functionarguments.aszCmd[1])
                    {
                        blGlobal = false;
                        break;
                    }
                }
            }

            // Store the value...
            Interpreter.FunctionArguments functionarguments = default(Interpreter.FunctionArguments);
            functionarguments.aszCmd = new string[3];
            functionarguments.aszCmd[0] = blGlobal ? "setglobal" : "setlocal";
            functionarguments.aszCmd[1] = a_functionarguments.aszCmd[1];
            functionarguments.aszCmd[2] = iDst.ToString();
            if (blGlobal)
            {
                CmdSetGlobal(ref functionarguments);
            }
            else
            {
                CmdSetLocal(ref functionarguments);
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Accept command input from the user.  The data is returned in the
        /// ${ret:} variable.  The first argument is a prompt, the rest of
        /// the arguments are optional, and indicate values that must be
        /// entered if the input command is going to return...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdInput(ref Interpreter.FunctionArguments a_functionarguments)
        {
            int ii;
            string szCmd = "";
            string szPrompt = "enter input: ";
            List<string> lszCommands = new List<string>();

            // Get the prompt...
            if ((a_functionarguments.aszCmd.Length >= 2) && (a_functionarguments.aszCmd[1] != null))
            {
                szPrompt = a_functionarguments.aszCmd[1];
            }

            // Get the commands...
            for (ii = 3; true; ii++)
            {
                if ((ii >= a_functionarguments.aszCmd.Length) || string.IsNullOrEmpty(a_functionarguments.aszCmd[ii - 1]))
                {
                    break;
                }
                lszCommands.Add(a_functionarguments.aszCmd[ii - 1]);
            }

            // Loopy...
            Interpreter interpreter = new Interpreter(szPrompt, m_consolecolorDefault, m_consolecolorGreen);
            while (true)
            {
                // Get the command...
                szCmd = interpreter.Prompt(m_streamreaderConsole, ((m_twain == null) ? 1 : (int)m_twain.GetState()));

                // If we have no commands to compare it against, we're done...
                if (lszCommands.Count == 0)
                {
                    break;
                }

                // Otherwise, we have to look for a match...
                bool blFound = false;
                foreach (string szCommand in lszCommands)
                {
                    if (szCmd.ToLowerInvariant() == szCommand.ToLowerInvariant())
                    {
                        blFound = true;
                        break;
                    }
                }

                // We got a match...
                if (blFound)
                {
                    break;
                }
            }

            // Update the return value...
            CallStack callstack = m_lcallstack[m_lcallstack.Count - 1];
            callstack.functionarguments.szReturnValue = szCmd;
            m_lcallstack[m_lcallstack.Count - 1] = callstack;

            // All done...
            return (false);
        }

        /// <summary>
        /// List scanners, both ones on the LAN and ones that are
        /// available in the cloud (when we get that far)...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdList(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // All done...
            return (false);
        }

        /// <summary>
        /// Quit...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdQuit(ref Interpreter.FunctionArguments a_functionarguments)
        {
            return (true);
        }

        /// <summary>
        /// Log text...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdLog(ref Interpreter.FunctionArguments a_functionarguments)
        {
            int ii;
            int iStart;
            string szSeverity;
            string szMessage;

            // If we have no arguments, then log a blank informational...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[1] == null))
            {
                Log.Info("");
                return (false);
            }

            // Pick a severity...
            switch (a_functionarguments.aszCmd[1])
            {
                default:
                    szSeverity = "info";
                    iStart = 1;
                    break;
                case "info":
                    szSeverity = "info";
                    iStart = 2;
                    break;
                case "warn":
                    szSeverity = "warn";
                    iStart = 2;
                    break;
                case "error":
                    szSeverity = "error";
                    iStart = 2;
                    break;
                case "verbose":
                    szSeverity = "verbose";
                    iStart = 2;
                    break;
                case "assert":
                    szSeverity = "assert";
                    iStart = 2;
                    break;
            }

            // Build the message...
            szMessage = "";
            for (ii = iStart; ii < a_functionarguments.aszCmd.Length; ii++)
            {
                szMessage += (szMessage == "") ? a_functionarguments.aszCmd[ii] : " " + a_functionarguments.aszCmd[ii];
            }

            // Log it...
            switch (szSeverity)
            {
                default:
                case "info":
                    Log.Info(szMessage);
                    break;
                case "warn":
                    Log.Warn(szMessage);
                    break;
                case "error":
                    Log.Error(szMessage);
                    break;
                case "verbose":
                    Log.Verbose(szMessage);
                    break;
                case "assert":
                    Log.Assert(szMessage);
                    break;
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Manage the self cert report...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdReport(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // If we have no arguments, then log a complaint...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[1] == null))
            {
                DisplayError("please specify initialize or save", a_functionarguments);
                return (false);
            }

            // Clear...
            if (a_functionarguments.aszCmd[1].ToLowerInvariant() == "initialize")
            {
                m_stringbuilderSelfCertReport = new StringBuilder();
                m_szSelfCertReportPath = null;
                m_szSelfCertReportProductname = "";
                if ((a_functionarguments.aszCmd.Length < 3) || (a_functionarguments.aszCmd[2] == null))
                {
                    DisplayError("please specify a driver productname", a_functionarguments);
                    return (false);
                }
                m_szSelfCertReportProductname = a_functionarguments.aszCmd[2];
            }

            // Save file...
            else if (a_functionarguments.aszCmd[1].ToLowerInvariant() == "save")
            {
                string szFolder = "";
                if (string.IsNullOrEmpty(m_szSelfCertReportProductname))
                {
                    DisplayError("'report initialize' must come before 'report save'...", a_functionarguments);
                    return (false);
                }
                if ((a_functionarguments.aszCmd.Length >= 3) && (a_functionarguments.aszCmd[2] != null))
                {
                    szFolder = a_functionarguments.aszCmd[2];
                }
                try
                {
                    if (string.IsNullOrEmpty(szFolder))
                    {
                        szFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "TWAIN Self Certification");
                    }
                    if (!Directory.Exists(szFolder))
                    {
                        Directory.CreateDirectory(szFolder);
                    }
                    m_szSelfCertReportPath = Path.Combine(szFolder, Regex.Replace(m_szSelfCertReportProductname, "[^.a-zA-Z0-9]", "_")) + ".txt";
                    File.WriteAllText(m_szSelfCertReportPath, m_stringbuilderSelfCertReport.ToString());
                }
                catch (Exception exception)
                {
                    DisplayError("save threw exception: " + exception.Message, a_functionarguments);
                    m_szSelfCertReportPath = null;
                    return (false);
                }
            }

            // No idea...
            else
            {
                DisplayError("unrecognized commend: " + a_functionarguments.aszCmd[1], a_functionarguments);
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Return from the current function...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdReturn(ref Interpreter.FunctionArguments a_functionarguments)
        {
            CallStack callstack;

            // If we don't have anything on the stack, then scoot...
            if ((m_lcallstack == null) || (m_lcallstack.Count == 0))
            {
                return (false);
            }

            // If this is the base of the stack, then return is a noop...
            if (m_lcallstack.Count == 1)
            {
                return (false);
            }

            // Make a copy of the last item (which we're about to delete)...
            callstack = m_lcallstack[m_lcallstack.Count - 1];

            // Remove the last item...
            m_lcallstack.RemoveAt(m_lcallstack.Count - 1);

            // Set the line we want to jump back to...
            a_functionarguments.blGotoLabel = true;
            a_functionarguments.iLabelLine = callstack.functionarguments.iCurrentLine + 1;

            // Make a note of the return value for "ret:"...
            if ((a_functionarguments.aszCmd != null) && (a_functionarguments.aszCmd.Length > 1))
            {
                callstack = m_lcallstack[m_lcallstack.Count - 1];
                callstack.functionarguments.szReturnValue = a_functionarguments.aszCmd[1];
                m_lcallstack[m_lcallstack.Count - 1] = callstack;
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// With no arguments, list the scripts.  With an argument,
        /// run the specified script.  This one runs silent.
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdRun(ref Interpreter.FunctionArguments a_functionarguments)
        {
            bool blSuccess;
            bool blSilent = m_blSilent;
            bool blSilentEvents = m_blSilentEvents;
            if (m_blVerbose)
            {
                m_blSilent = false;
                m_blSilentEvents = false;
            }
            else
            {
                m_blSilent = true;
                m_blSilentEvents = true;
            }
            blSuccess = CmdRunv(ref a_functionarguments);
            m_blSilent = blSilent;
            m_blSilentEvents = blSilentEvents;
            return (blSuccess);
        }

        /// <summary>
        /// With no arguments, list the scripts.  With an argument,
        /// run the specified script.  The one runs verbose.
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdRunv(ref Interpreter.FunctionArguments a_functionarguments)
        {
            string szPrompt = "tc>>> ";
            string[] aszScript;
            string szScriptFile;
            int iCallStackCount;
            CallStack callstack;
            Interpreter interpreter;

            // List...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[1] == null))
            {
                // Get the script files...
                string[] aszScriptFiles = Directory.GetFiles(".", "*.tc");
                if ((aszScriptFiles == null) || (aszScriptFiles.Length == 0))
                {
                    DisplayError("no script files found", a_functionarguments);
                }

                // List what we found...
                Display("SCRIPT FILES");
                foreach (string sz in aszScriptFiles)
                {
                    Display(Path.GetFileNameWithoutExtension(sz), true);
                }

                // All done...
                return (false);
            }

            // Make sure the file exists...
            szScriptFile = a_functionarguments.aszCmd[1];
            if (!File.Exists(szScriptFile))
            {
                szScriptFile = a_functionarguments.aszCmd[1] + ".tc";
                if (!File.Exists(szScriptFile))
                {
                    DisplayError("script not found...<" + szScriptFile + ">", a_functionarguments);
                    return (false);
                }
            }

            // Read the file...
            try
            {
                aszScript = File.ReadAllLines(szScriptFile);
            }
            catch (Exception exception)
            {
                DisplayError("failed to read script...<" + szScriptFile + ">" + exception.Message, a_functionarguments);
                return (false);
            }

            // Give ourselves an interpreter...
            interpreter = new Interpreter("", m_consolecolorDefault, m_consolecolorGreen);

            // Bump ourself up on the call stack, because we're really
            // working like a call.  At this point we'll be running with
            // at least 2 items on the stack.  If we drop down to 1 item
            // that's a hint that the return command was used to get out
            // of the script...
            callstack = default(CallStack);
            callstack.functionarguments = a_functionarguments;
            callstack.functionarguments.szScriptFile = szScriptFile;
            callstack.functionarguments.aszScript = aszScript;
            m_lcallstack.Add(callstack);
            iCallStackCount = m_lcallstack.Count;

            // Run each line in the script...
            int iLine = 0;
            bool blReturn = false;
            TWAIN.STS sts = TWAIN.STS.SUCCESS;
            string szReturnValue = "";
            m_blRunningScript = true;
            while (iLine < aszScript.Length)
            {
                bool blDone;
                string szLine;
                string[] aszCmd;

                // Grab our line...
                szLine = aszScript[iLine];

                // Show the command...
                if (!m_blSilent)
                {
                    Display(szPrompt + szLine.Trim());
                }

                // Tokenize...
                aszCmd = interpreter.Tokenize(szLine.Trim());

                // Restore the last dsmentry status and value, if this isn't a dsmentry command,
                // we need this so we can examine the values in other commands...
                if (   (aszCmd[0] != "dsmentry")
                    && (aszCmd[0] != "echo")
                    && (aszCmd[0] != "goto")
                    && (aszCmd[0] != "if")
                    && (aszCmd[0] != "increment")
                    && (aszCmd[0] != "log")
                    && (aszCmd[0] != "setglobal")
                    && (aszCmd[0] != "setlocal")
                    && (aszCmd[0] != "sleep"))
                {
                    callstack = m_lcallstack[m_lcallstack.Count - 1];
                    callstack.functionarguments.sts = sts;
                    callstack.functionarguments.szReturnValue = szReturnValue;
                    m_lcallstack[m_lcallstack.Count - 1] = callstack;
                }

                // Expansion of symbols...
                Expansion(ref aszCmd);

                // Dispatch...
                Interpreter.FunctionArguments functionarguments = default(Interpreter.FunctionArguments);
                functionarguments.aszCmd = aszCmd;
                functionarguments.szScriptFile = szScriptFile;
                functionarguments.aszScript = aszScript;
                functionarguments.iCurrentLine = iLine;
                blDone = interpreter.Dispatch(ref functionarguments, m_ldispatchtable);
                if (blDone)
                {
                    break;
                }

                // Squirrel this stuff away, not every command should override these
                // items, otherwise we can't test them...  :)
                if (   (aszCmd[0] == "allocatehandle")
                    || (aszCmd[0] == "allocatepointer")
                    || (aszCmd[0] == "dsmentry")
                    || (aszCmd[0] == "wait"))
                {
                    sts = functionarguments.sts;
                    szReturnValue = functionarguments.szReturnValue;
                    if (m_lcallstack.Count > 0)
                    {
                        callstack = m_lcallstack[m_lcallstack.Count - 1];
                        callstack.functionarguments.sts = sts;
                        callstack.functionarguments.szReturnValue = szReturnValue;
                        m_lcallstack[m_lcallstack.Count - 1] = callstack;
                    }
                }

                // Handle gotos...
                if (functionarguments.blGotoLabel)
                {
                    iLine = functionarguments.iLabelLine;
                }
                // Otherwise, just increment...
                else
                {
                    iLine += 1;
                }

                // If the count dropped, that's a sign we need to bail...
                if (m_lcallstack.Count < iCallStackCount)
                {
                    blReturn = true;
                    break;
                }
            }

            // Pop this item, and pass along the return value, but don't do it
            // if we detect that a return call was made in the script, because
            // it will have already popped the stack for us...
            if (!blReturn && (m_lcallstack.Count > 1))
            {
                szReturnValue = m_lcallstack[m_lcallstack.Count - 1].functionarguments.szReturnValue;
                if (szReturnValue == null)
                {
                    szReturnValue = "";
                }
                m_lcallstack.RemoveAt(m_lcallstack.Count - 1);
                callstack = m_lcallstack[m_lcallstack.Count - 1];
                callstack.functionarguments.szReturnValue = szReturnValue;
                m_lcallstack[m_lcallstack.Count - 1] = callstack;
            }

            // Handle the last item on the stack, so we can report how things turned out...
            else if (m_lcallstack.Count == 1)
            {
                a_functionarguments.szReturnValue = m_lcallstack[m_lcallstack.Count - 1].functionarguments.szReturnValue;
            }

            // All done...
            m_blRunningScript = false;
            return (false);
        }

        /// <summary>
        /// Save an image to disk, used for DAT_IMAGEMEMFILEXFER, DAT_IMAGEMEMXFER,
        /// and DAT_IMAGENATIVEXFER.  We don't need it for DAT_IMAGEFILEXFER...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdSaveImage(ref Interpreter.FunctionArguments a_functionarguments)
        {
            byte[] abImage;
            UInt64 u64Handle;

            // Basic sanity check...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[1] == null))
            {
                DisplayError("insufficient arguments, please see help for saveimage", a_functionarguments);
                return (false);
            }

            // Dispatch off the second argument...
            switch (a_functionarguments.aszCmd[1].ToLowerInvariant())
            {
                // Oh dear...
                default:
                    DisplayError("unrecognized type <" + a_functionarguments.aszCmd[1] + ">", a_functionarguments);
                    return (false);

                // Memory transfer, we need the image file, pointer, byte size, and DAT_IMAGEINFO metadata...
                case "mem":
                    if (a_functionarguments.aszCmd.Length < 6)
                    {
                        DisplayError("insufficient arguments, please see help for saveimage", a_functionarguments);
                        return (false);
                    }
                    break;

                // Memory file transfer, we need the image file, pointer, and the byte size...
                case "memfile":
                    if (a_functionarguments.aszCmd.Length < 5)
                    {
                        DisplayError("insufficient arguments, please see help for saveimage", a_functionarguments);
                        return (false);
                    }
                    break;

                // Native transfer, we need the image file and the handle...
                case "native":
                    if (a_functionarguments.aszCmd.Length < 4)
                    {
                        DisplayError("insufficient arguments, please see help for saveimage", a_functionarguments);
                        return (false);
                    }
                    string szImageFile = a_functionarguments.aszCmd[2];
                    if (!UInt64.TryParse(a_functionarguments.aszCmd[3], out u64Handle))
                    {
                        DisplayError("failed to convert the byte size", a_functionarguments);
                        return (false);
                    }
                    // This beastie autodetects the image type...
                    abImage = m_twain.NativeToByteArray((IntPtr)u64Handle, true);
                    try
                    {
                        File.WriteAllBytes(szImageFile, abImage);
                    }
                    catch (Exception exception)
                    {
                        DisplayError("saveimage failed <" + szImageFile + "> - " + exception.Message, a_functionarguments);
                        return (false);
                    }
                    break;
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Select a scanner, do a snapshot, if needed, if no selection
        /// is offered, then pick the first scanner found...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdSelect(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // All done...
            return (false);
        }

        /// <summary>
        /// With no arguments, list the keys with their values.  With an argument,
        /// set the specified value.
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdSetGlobal(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // If we don't have any arguments, list what we have...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[1] == null))
            {
                lock (m_objectKeyValue)
                {
                    // Hmmm...
                    if (m_lkeyvalue.Count == 0)
                    {
                        DisplayError("no keys to list", a_functionarguments);
                        return (false);
                    }

                    // Loopy...
                    Display("KEY/VALUE PAIRS");
                    foreach (KeyValue keyvalue in m_lkeyvalue)
                    {
                        Display(keyvalue.szKey + "=<" + keyvalue.szValue + ">");
                    }
                }

                // All done...
                return (false);
            }

            // Set the variable in the global list...
            SetVariable(a_functionarguments.aszCmd[1], (a_functionarguments.aszCmd.Length < 3) || (a_functionarguments.aszCmd[2] == null) ? "" : a_functionarguments.aszCmd[2], 0, VariableScope.Global);

            // All done...
            return (false);
        }

        /// <summary>
        /// With no arguments, list the keys with their values.  With an argument,
        /// set the specified value.  All done in the context of the topmost item
        /// on the stack...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdSetLocal(ref Interpreter.FunctionArguments a_functionarguments)
        {
            CallStack callstack;

            // If we don't have a stack, add this to the global list...
            if (m_lcallstack.Count == 0)
            {
                CmdSetGlobal(ref a_functionarguments);
                return (false);
            }

            // This is what we'll be referencing...
            callstack = m_lcallstack[m_lcallstack.Count - 1];
            if (callstack.lkeyvalue == null)
            {
                callstack.lkeyvalue = new List<KeyValue>();
            }

            // If we don't have any arguments, list what we have for the whole stack...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || (a_functionarguments.aszCmd[1] == null))
            {
                int ii;

                // We got nothing...
                if (callstack.lkeyvalue.Count == 0)
                {
                    DisplayError("no local keys to list", a_functionarguments);
                    return (false);
                }

                // Loopy...
                Display("LOCAL KEY/VALUE PAIRS");
                for (ii = m_lcallstack.Count - 1; ii >= 0; ii--)
                {
                    Display("  STACK #" + ii);
                    if ((m_lcallstack[ii].lkeyvalue == null) || (m_lcallstack[ii].lkeyvalue.Count == 0))
                    {
                        Display("    (no local variables)" + ii);
                    }
                    else
                    {
                        foreach (KeyValue keyvalue in m_lcallstack[ii].lkeyvalue)
                        {
                            Display("    " + keyvalue.szKey + "=<" + keyvalue.szValue + ">");
                        }
                    }
                }

                // All done...
                return (false);
            }

            // Set the variable in the local list...
            SetVariable(a_functionarguments.aszCmd[1], (a_functionarguments.aszCmd.Length < 3) || (a_functionarguments.aszCmd[2] == null) ? "" : a_functionarguments.aszCmd[2], 0, VariableScope.Local);

            // All done...
            return (false);
        }

        /// <summary>
        /// Sleep some number of milliseconds...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdSleep(ref Interpreter.FunctionArguments a_functionarguments)
        {
            int iMilliseconds;

            // Get the milliseconds...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2) || !int.TryParse(a_functionarguments.aszCmd[1], out iMilliseconds))
            {
                iMilliseconds = 0;
            }
            if (iMilliseconds < 0)
            {
                iMilliseconds = 0;
            }

            // Wait...
            Thread.Sleep(iMilliseconds);

            // All done...
            return (false);
        }

        /// <summary>
        /// Status of the program...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdStatus(ref Interpreter.FunctionArguments a_functionarguments)
        {
            // Platform...
            Display("Platform........." + TWAINWorkingGroup.TWAIN.GetPlatform() + " (" + TWAINWorkingGroup.TWAIN.GetMachineWordBitSize() + "-bit)");

            // DSM is not loaded...
            if (m_twain == null)
            {
                Display("DSM Path.........(not loaded)");
                return (false);
            }

            // DSM in use...
            Display("DSM Path........." + m_twain.GetDsmPath());

            // State...
            Display("TWAIN State......" + ((int)m_twain.GetState()));

            // If state 4 or higher, what is our driver?
            if (m_twain.GetState() >= TWAIN.STATE.S4)
            {
                Display("TWAIN Driver....." + m_twain.GetDsIdentity());
            }

            // All done...
            return (false);
        }

        /// <summary>
        /// Wait for one or more events with a timeout, or clear pending events...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        private bool CmdWait(ref Interpreter.FunctionArguments a_functionarguments)
        {
            int ii;
            int iMilliseconds = 0;
            CallStack callstack = m_lcallstack[m_lcallstack.Count - 1];

            // Validate...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length < 2))
            {
                return (false);
            }

            // Examine all of the arguments...
            for (ii = 1; ii < a_functionarguments.aszCmd.Length; ii++)
            {
                // Clear pending messages...
                if (a_functionarguments.aszCmd[1].ToLowerInvariant() == "reset")
                {
                    lock (m_lmsgDatNull)
                    {
                        m_lmsgDatNull.Clear();
                        m_autoreseteventMsgDatNull.Reset();
                    }
                    a_functionarguments.szReturnValue = "";
                    callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                    return (false);
                }

                // A number is the timeout value...
                else if (int.TryParse(a_functionarguments.aszCmd[ii], out iMilliseconds))
                {
                    if (iMilliseconds < 0)
                    {
                        iMilliseconds = 0;
                    }
                }
            }

            // Wait for a signal or a timeout...
            bool blSignaled;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            blSignaled = m_autoreseteventMsgDatNull.WaitOne(iMilliseconds);
            if (!blSignaled)
            {
                a_functionarguments.szReturnValue = "timeout";
                callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;
                return (false);
            }

            // Build a list of what we got, take a snapshot so we
            // hold onto it for as little time as possible...
            List<TWAIN.MSG> lmsgDatNull = null;
            a_functionarguments.szReturnValue = "";
            lock (m_lmsgDatNull)
            {
                lmsgDatNull = new List<TWAIN.MSG>(m_lmsgDatNull);
            }
            foreach (TWAIN.MSG msg in lmsgDatNull)
            {
                a_functionarguments.szReturnValue += (string.IsNullOrEmpty(a_functionarguments.szReturnValue) ? "" : ",") + "MSG_" + msg;
            }

            // Update the stack...
            callstack.functionarguments.szReturnValue = a_functionarguments.szReturnValue;

            // All done...
            return (false);
        }

        #endregion


        // Private Methods (certification)
        #region Private Methods (certification)

        /// <summary>
        /// Run the TWAIN Certification tests.  
        /// </summary>
        private void TwainDirectCertification()
        {
            int iPass = 0;
            int iFail = 0;
            int iSkip = 0;
            int iTotal = 0;
            string szCertificationFolder;
            string[] aszCategories;

            // Find our cert stuff...
            szCertificationFolder = Path.Combine(Config.Get("writeFolder", ""), "tasks");
            szCertificationFolder = Path.Combine(szCertificationFolder, "certification");

            // Whoops...nothing to work with...
            if (!Directory.Exists(szCertificationFolder))
            {
                DisplayError("cannot find certification folder:\n" + szCertificationFolder, new Interpreter.FunctionArguments());
                return;
            }

            // Get the categories...
            aszCategories = Directory.GetDirectories(szCertificationFolder);
            if (aszCategories == null)
            {
                DisplayError("cannot find any certification categories:\n" + szCertificationFolder, new Interpreter.FunctionArguments());
                return;
            }

            // Pass count...
            Log.Info("certification>>> PASS: " + iPass);

            // Fail count...
            Log.Info("certification>>> FAIL: " + iFail);

            // Skip count...
            Log.Info("certification>>> SKIP: " + iSkip);

            // Total count...
            Log.Info("certification>>> TOTAL: " + iTotal);
        }

        #endregion


        // Private Methods (misc)
        #region Private Methods (misc)

        /// <summary>
        /// Display text (if allowed)...
        /// </summary>
        /// <param name="a_szText">the text to display</param>
        private void Display(string a_szText, bool a_blForce = false)
        {
            if (!m_blSilent || a_blForce)
            {
                Console.Out.WriteLine(a_szText);
            }
            if (m_stringbuilderSelfCertReport != null)
            {
                m_stringbuilderSelfCertReport.AppendLine(a_szText);
            }
        }

        /// <summary>
        /// Display text (if allowed)...
        /// </summary>
        /// <param name="a_szText">the text to display</param>
        private void DisplayBlue(string a_szText, bool a_blForce = false)
        {
            if (!m_blSilent || a_blForce)
            {
                if (Console.BackgroundColor == ConsoleColor.Black)
                {
                    Console.ForegroundColor = m_consolecolorBlue;
                    Console.Out.WriteLine(a_szText);
                    Console.ForegroundColor = m_consolecolorDefault;
                }
                else
                {
                    Console.Out.WriteLine(a_szText);
                }
                if (m_stringbuilderSelfCertReport != null)
                {
                    m_stringbuilderSelfCertReport.AppendLine(a_szText);
                }
            }
        }

        /// <summary>
        /// Display text (if allowed)...
        /// </summary>
        /// <param name="a_szText">the text to display</param>
        private void DisplayGreen(string a_szText, bool a_blForce = false)
        {
            if (!m_blSilent || a_blForce)
            {
                if (Console.BackgroundColor == ConsoleColor.Black)
                {
                    Console.ForegroundColor = m_consolecolorGreen;
                    Console.Out.WriteLine(a_szText);
                    Console.ForegroundColor = m_consolecolorDefault;
                }
                else
                {
                    Console.Out.WriteLine(a_szText);
                }
                if (m_stringbuilderSelfCertReport != null)
                {
                    m_stringbuilderSelfCertReport.AppendLine(a_szText);
                }
            }
        }

        /// <summary>
        /// Display text (if allowed)...
        /// </summary>
        /// <param name="a_szText">the text to display</param>
        private void DisplayRed(string a_szText, bool a_blForce = false)
        {
            if (!m_blSilent || a_blForce)
            {
                if (Console.BackgroundColor == ConsoleColor.Black)
                {
                    Console.ForegroundColor = m_consolecolorRed;
                    Console.Out.WriteLine(a_szText);
                    Console.ForegroundColor = m_consolecolorDefault;
                }
                else
                {
                    Console.Out.WriteLine(a_szText);
                }
                if (m_stringbuilderSelfCertReport != null)
                {
                    m_stringbuilderSelfCertReport.AppendLine(a_szText);
                }
            }
        }

        /// <summary>
        /// Display text (if allowed)...
        /// </summary>
        /// <param name="a_szText">the text to display</param>
        private void DisplayYellow(string a_szText, bool a_blForce = false)
        {
            if (!m_blSilent || a_blForce)
            {
                if (Console.BackgroundColor == ConsoleColor.Black)
                {
                    Console.ForegroundColor = m_consolecolorYellow;
                    Console.Out.WriteLine(a_szText);
                    Console.ForegroundColor = m_consolecolorDefault;
                }
                else
                {
                    Console.Out.WriteLine(a_szText);
                }
                if (m_stringbuilderSelfCertReport != null)
                {
                    m_stringbuilderSelfCertReport.AppendLine(a_szText);
                }
            }
        }

        /// <summary>
        /// Display an error message...
        /// </summary>
        /// <param name="a_szText">the text to display</param>
        private void DisplayError(string a_szText, Interpreter.FunctionArguments a_functionarguments)
        {
            string szMessage;
            if (string.IsNullOrEmpty(a_functionarguments.szScriptFile))
            {
                szMessage = "ERROR: " + a_szText;
            }
            else
            {
                szMessage =
                    "ERROR: " + a_szText +
                    " ('" + a_functionarguments.szScriptFile +
                    "' at line " + (a_functionarguments.iCurrentLine + 1) + ")";
            }
            Console.Out.WriteLine(szMessage);
            if (m_stringbuilderSelfCertReport != null)
            {
                m_stringbuilderSelfCertReport.AppendLine(szMessage);
            }
        }

        /// <summary>
        /// Cleanup...
        /// </summary>
        /// <param name="a_blDisposing">true if we need to clean up managed resources</param>
        internal void Dispose(bool a_blDisposing)
        {
            // Free managed resources...
            if (a_blDisposing)
            {
                if (m_autoreseteventMsgDatNull != null)
                {
                    m_autoreseteventMsgDatNull.Dispose();
                    m_autoreseteventMsgDatNull = null;
                }
                if (m_autoreseteventScannerslist != null)
                {
                    m_autoreseteventScannerslist.Dispose();
                    m_autoreseteventScannerslist = null;
                }
                if (m_twain != null)
                {
                    m_twain.Dispose();
                    m_twain = null;
                }
            }
        }

        /// <summary>
        /// Expand symbols that we find in the tokenized strings.  Symbols take the form
        /// ${source:key} where source can be one of the following:
        ///     - the JSON text from the response to the last API command
        ///     - the list maintains by the set command
        ///     - the return value from the last run/runv or call in a script
        ///     - the arguments to the program, run/runv or call in a script
        /// 
        /// Symbols can be nests, for instance, if the first argument to a call
        /// is a JSON key, it can be expanded as:
        ///     - ${rj:${arg:1}}
        /// </summary>
        /// <param name="a_aszCmd">tokenized string array to expand</param>
        private void Expansion(ref string[] a_aszCmd)
        {
            int ii;
            int iReferenceCount;
            int iCmd;
            int iIndexLeft;
            int iIndexRight;
            CallStack callstack;

            // Expansion...
            for (iCmd = 0; iCmd < a_aszCmd.Length; iCmd++)
            {
                // If we don't find an occurrance of ${ in the string, then we're done...
                if (!a_aszCmd[iCmd].Contains("${"))
                {
                    continue;
                }

                // Find each outermost ${ in the string, meaning that if we have the
                // following ${rj:${arg:1}}${get:y} we only want to find the rj and
                // the get, the arg will be handled inside of the rj, so that means
                // we have to properly count our way to the closing } for rj...
                for (iIndexLeft = a_aszCmd[iCmd].IndexOf("${");
                     iIndexLeft >= 0;
                     iIndexLeft = a_aszCmd[iCmd].IndexOf("${"))
                {
                    string szSymbol;
                    string szValue;
                    string szKey = a_aszCmd[iCmd];

                    // Find the corresponding }...
                    iIndexRight = -1;
                    iReferenceCount = 0;
                    for (ii = iIndexLeft + 2; ii < szKey.Length; ii++)
                    {
                        // Either exit or decrement our reference count...
                        if (szKey[ii] == '}')
                        {
                            if (iReferenceCount == 0)
                            {
                                iIndexRight = ii;
                                break;
                            }
                            iReferenceCount -= 1;
                        }

                        // Bump up the reference count...
                        if ((szKey[ii] == '$') && ((ii + 1) < szKey.Length) && (szKey[ii + 1] == '{'))
                        {
                            iReferenceCount += 1;
                        }
                    }

                    // If we didn't find a closing }, we're done...
                    if (iIndexRight == -1)
                    {
                        break;
                    }

                    // This is our symbol...
                    // 0123456789
                    // aa${rj:x}a
                    // left index is 2, right index is 8, size is 7, so (r - l) + 1
                    szSymbol = szKey.Substring(iIndexLeft, (iIndexRight - iIndexLeft) + 1);

                    // Expand the stuff to the right of the source, so if we have
                    // ${rj:x} we'll get x back, but if we have ${rj:${arg:1}}, we'll
                    // get the value of ${arg:1} back...
                    if (   szSymbol.StartsWith("${arg:")
                        || szSymbol.StartsWith("${bits:")
                        || szSymbol.StartsWith("${ds:")
                        || szSymbol.StartsWith("${folder:")
                        || szSymbol.StartsWith("${format:")
                        || szSymbol.StartsWith("${get:")
                        || szSymbol.StartsWith("${getindex:")
                        || szSymbol.StartsWith("${localtime:")
                        || szSymbol.StartsWith("${platform:")
                        || szSymbol.StartsWith("${program:")
                        || szSymbol.StartsWith("${report:")
                        || szSymbol.StartsWith("${ret:")
                        || szSymbol.StartsWith("${state:")
                        || szSymbol.StartsWith("${sts:"))
                    {
                        int iSymbolIndexLeft = szSymbol.IndexOf(":") + 1;
                        int iSymbolIndexLength;
                        string[] asz = new string[1];
                        asz[0] = szSymbol.Substring(0, szSymbol.Length - 1).Substring(iSymbolIndexLeft);
                        iSymbolIndexLength = asz[0].Length;
                        Expansion(ref asz);
                        szSymbol = szSymbol.Remove(iSymbolIndexLeft, iSymbolIndexLength);
                        szSymbol = szSymbol.Insert(iSymbolIndexLeft, asz[0]);
                    }

                    // Assume the worse...
                    szValue = "";

                    // Get data from the top of the call stack...
                    if (szSymbol.StartsWith("${arg:"))
                    {
                        if ((m_lcallstack != null) && (m_lcallstack.Count > 0))
                        {
                            string szTarget = szSymbol.Substring(0, szSymbol.Length - 1).Substring(6);
                            // If we have an index, use it to find our callstack.  As a general rule
                            // the only 'safe' index to use is 0, since that'll point to the last
                            // command entered by the user.  But fancier stuff is possible...
                            if (szTarget.Contains("."))
                            {
                                // Use the index info to get the right data...
                                int iIndex;
                                string szIndex = szTarget.Remove(szTarget.IndexOf("."));
                                if (int.TryParse(szIndex, out iIndex))
                                {
                                    iIndex += 1; // so we can have an index origin at 0...
                                    if (iIndex < 1)
                                    {
                                        iIndex = 1;
                                    }
                                    else if (iIndex >= m_lcallstack.Count)
                                    {
                                        iIndex = m_lcallstack.Count - 1;
                                    }
                                }
                                callstack = m_lcallstack[iIndex];
                                // Ditch the index info...
                                szTarget = szTarget.Substring(szTarget.IndexOf(".") + 1);
                            }
                            // Use the top of the stack...
                            else
                            {
                                callstack = m_lcallstack[m_lcallstack.Count - 1];
                            }
                            // Return the number of arguments, wherever we ended up...
                            if (szTarget == "#")
                            {
                                // Needs to be -2 to remove "call xxx" or "run xxx" from count...
                                szValue = (callstack.functionarguments.aszCmd.Length - 2).ToString();
                            }
                            // Return the requested argument, wherever we ended up, -1 gets the command...
                            else
                            {
                                int iIndex;
                                if (int.TryParse(szTarget, out iIndex))
                                {
                                    if ((callstack.functionarguments.aszCmd != null) && (iIndex >= -1) && ((iIndex + 1) < callstack.functionarguments.aszCmd.Length))
                                    {
                                        szValue = callstack.functionarguments.aszCmd[iIndex + 1];
                                    }
                                }
                            }
                        }
                    }

                    // Get number of bits in the machine word...
                    else if (szSymbol.StartsWith("${bits:"))
                    {
                        szValue = TWAINWorkingGroup.TWAIN.GetMachineWordBitSize().ToString();
                    }

                    // Get data from the data source (indexible)...
                    else if (szSymbol.StartsWith("${ds:"))
                    {
                        if (m_twain == null)
                        {
                            szValue = "(driver not loaded)";
                        }
                        else if (szSymbol == "${ds:}")
                        {
                            szValue = m_twain.GetDsIdentity();
                        }
                        else
                        {
                            int iIndex = 0;
                            if (int.TryParse(szSymbol.Replace("${ds:", "").Replace("}", ""), out iIndex))
                            {
                                string[] asz = CSV.Parse(m_twain.GetDsIdentity());
                                if ((iIndex < 0) || (iIndex >= asz.Length))
                                {
                                    szValue = "";
                                }
                                else
                                {
                                    szValue = asz[iIndex];
                                }
                            }
                        }
                    }

                    // Format the data using a specifier...
                    else if (szSymbol.StartsWith("${format:"))
                    {
                        // Strip off ${...}, split specifier and value...
                        string szFormat = szSymbol.Substring(0, szSymbol.Length - 1).Substring(9);
                        string[] aszFormat = szFormat.Split(new char[] { '|' }, 2);
                        if (aszFormat.Length >= 2)
                        {
                            // Assume it's a number...
                            if (aszFormat[0].ToLowerInvariant().Contains("x") || aszFormat[0].ToLowerInvariant().Contains("d"))
                            {
                                int iValue = 0;
                                int.TryParse(aszFormat[1], out iValue);
                                szValue = string.Format("{0:" + aszFormat[0] + "}", iValue);
                            }
                            // Assume it's a string...
                            else
                            {
                                szValue = string.Format("{0:" + aszFormat[0] + "}", aszFormat[1]);
                            }
                        }
                    }

                    // Use value as a GET key to get a value, we don't allow a null in this
                    // case, it has to be an empty string.  We search the local list first,
                    // and if that fails go to the global list...
                    else if (szSymbol.StartsWith("${get:"))
                    {
                        int iBytes = 0;
                        bool blGlobal = false;

                        // Strip off ${...}
                        string szGet = szSymbol.Substring(0, szSymbol.Length - 1).Substring(6);

                        if (szGet == "twain.state")
                        {
                            iBytes = 0;
                        }

                        // Get the value, if any...
                        GetVariable(szGet, -1, out szValue, out iBytes, out blGlobal);
                    }

                    // Use value as a GET key to get a value, we don't allow a null in this
                    // case, it has to be an empty string.  CSV the value we get, and return
                    // just the item indicated by the index.    We search the local list
                    // first, and if that fails go to the global list...
                    if (szSymbol.StartsWith("${getindex:"))
                    {
                        int iIndex = 0;
                        int iBytes = 0;
                        bool blGlobal = false;

                        // Strip off ${...}, split name and index...
                        string szGet = szSymbol.Substring(0, szSymbol.Length - 1).Substring(11);
                        string[] aszGet = szGet.Split('.');

                        // Get the value, if any...
                        if ((aszGet.Length > 1) && int.TryParse(aszGet[1], out iIndex))
                        {
                            GetVariable(aszGet[0], iIndex, out szValue, out iBytes, out blGlobal);
                        }
                    }

                    // Special folders
                    else if (szSymbol.StartsWith("${folder:"))
                    {
                        if (szSymbol == "${folder:certification}")
                        {
                            szValue = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Path.Combine("data", "Certification"));
                        }
                        else if (szSymbol == "${folder:data}")
                        {
                            szValue = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "data");
                        }
                        else if (szSymbol == "${folder:desktop}")
                        {
                            szValue = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                        }
                        else if (szSymbol == "${folder:local}")
                        {
                            szValue = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        }
                        else if (szSymbol == "${folder:pictures}")
                        {
                            szValue = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                        }
                        else if (szSymbol == "${folder:roaming}")
                        {
                            szValue = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        }
                        else
                        {
                            // Not the most brilliant idea, but what the hell...
                            szValue = szSymbol.Replace("${", "").Replace(":", "").Replace("}", "");
                        }
                    }

                    // Access to the local time...
                    else if (szSymbol.StartsWith("${localtime:"))
                    {
                        DateTime datetime = DateTime.Now;
                        string szFormat = szSymbol.Substring(0, szSymbol.Length - 1).Substring(12);
                        try
                        {
                            szValue = datetime.ToString(szFormat);
                        }
                        catch
                        {
                            szValue = datetime.ToString();
                        }
                    }

                    // Get the platform...
                    else if (szSymbol.StartsWith("${platform:"))
                    {
                        szValue = TWAINWorkingGroup.TWAIN.GetPlatform().ToString();
                    }

                    // Get the program, version, and machine word size (meant for display only)...
                    else if (szSymbol.StartsWith("${program:"))
                    {
                        Assembly assembly = typeof(Terminal).Assembly;
                        AssemblyName assemblyname = assembly.GetName();
                        Version version = assemblyname.Version;
                        DateTime datetime = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.MinorRevision * 2);
                        szValue = assemblyname.Name + " v" + version.Major + "." + version.Minor + " " + datetime.Day + "-" + datetime.ToString("MMM") + "-" + datetime.Year + " " + ((IntPtr.Size == 4) ? "(32-bit)" : "(64-bit)");
                    }

                    // Full path to the self cert report (meant for display only)...
                    else if (szSymbol.StartsWith("${report:"))
                    {
                        if (string.IsNullOrEmpty(m_szSelfCertReportPath))
                        {
                            szValue = "(self cert file not specified)";
                        }
                        else if (!File.Exists(m_szSelfCertReportPath))
                        {
                            szValue = m_szSelfCertReportPath + " (not found)";
                        }
                        else
                        {
                            szValue = m_szSelfCertReportPath;
                        }
                    }

                    // Get data from the return value...
                    else if (szSymbol.StartsWith("${ret:"))
                    {
                        callstack = m_lcallstack[m_lcallstack.Count - 1];
                        if (callstack.functionarguments.szReturnValue != null)
                        {
                            if (szSymbol == "${ret:}")
                            {
                                szValue = callstack.functionarguments.szReturnValue;
                            }
                            else
                            {
                                int iIndex = 0;
                                if (int.TryParse(szSymbol.Replace("${ret:", "").Replace("}", ""), out iIndex))
                                {
                                    string[] asz = CSV.Parse(callstack.functionarguments.szReturnValue);
                                    if ((iIndex < 0) || (iIndex >= asz.Length))
                                    {
                                        szValue = "";
                                    }
                                    else
                                    {
                                        szValue = asz[iIndex];
                                    }
                                }
                            }
                        }
                    }

                    // Get the TWAIN state...
                    else if (szSymbol.StartsWith("${state:"))
                    {
                        if (m_twain == null)
                        {
                            szValue = "1";
                        }
                        else
                        {
                            szValue = ((int)m_twain.GetState()).ToString();
                        }
                    }

                    // Get data from the TWAIN status...
                    else if (szSymbol.StartsWith("${sts:"))
                    {
                        callstack = m_lcallstack[m_lcallstack.Count - 1];
                        szValue = callstack.functionarguments.sts.ToString();
                    }

                    // Failsafe (we should catch all of these up above)...
                    if (szValue == null)
                    {
                        szValue = "";
                    }

                    // Replace the current contents with the expanded value...
                    a_aszCmd[iCmd] = a_aszCmd[iCmd].Remove(iIndexLeft, (iIndexRight - iIndexLeft) + 1).Insert(iIndexLeft, szValue);
                }
            }
        }

        /// <summary>
        /// Get a variable from the local or global lists...
        /// </summary>
        /// <param name="a_szKey">key to find</param>
        /// <param name="a_iIndex">-1 for all, 0 - n for a CSV index</param>
        /// <param name="a_szValue">value found</param>
        /// <param name="a_iBytes">bytes found</param>
        /// <param name="a_blGlobal">true if global</param>
        /// <param name="a_variablescope">scope</param>
        /// <returns>true if found</returns>
        private bool GetVariable(string a_szKey, int a_iIndex, out string a_szValue, out int a_iBytes, out bool a_blGlobal, VariableScope a_variablescope = VariableScope.Auto)
        {
            // Local check...
            if ((a_variablescope != VariableScope.Global) && (m_lcallstack.Count > 0) && (m_lcallstack[m_lcallstack.Count - 1].lkeyvalue != null))
            {
                foreach (KeyValue keyvalue in m_lcallstack[m_lcallstack.Count - 1].lkeyvalue)
                {
                    if (keyvalue.szKey == a_szKey)
                    {
                        if (a_iIndex == -1)
                        {
                            a_szValue = (keyvalue.szValue == null) ? "" : keyvalue.szValue;
                            a_iBytes = keyvalue.iBytes;
                            a_blGlobal = false;
                            return (true);
                        }
                        else
                        {
                            string[] aszValue = CSV.Parse((keyvalue.szValue == null) ? "" : keyvalue.szValue);
                            a_szValue = ((a_iIndex >= 0) && (a_iIndex < aszValue.Length)) ? aszValue[a_iIndex] : "";
                            a_iBytes = keyvalue.iBytes;
                            a_blGlobal = false;
                            return (true);
                        }
                    }
                }
            }

            // Global check...
            if (a_variablescope != VariableScope.Local)
            {
                lock (m_objectKeyValue)
                {
                    if (m_lkeyvalue.Count >= 0)
                    {
                        foreach (KeyValue keyvalue in m_lkeyvalue)
                        {
                            if (keyvalue.szKey == a_szKey)
                            {
                                if (a_iIndex == -1)
                                {
                                    a_szValue = (keyvalue.szValue == null) ? "" : keyvalue.szValue;
                                    a_iBytes = keyvalue.iBytes;
                                    a_blGlobal = true;
                                    return (true);
                                }
                                else
                                {
                                    string[] aszValue = CSV.Parse((keyvalue.szValue == null) ? "" : keyvalue.szValue);
                                    a_szValue = ((a_iIndex >= 0) && (a_iIndex < aszValue.Length)) ? aszValue[a_iIndex] : "";
                                    a_iBytes = keyvalue.iBytes;
                                    a_blGlobal = true;
                                    return (true);
                                }
                            }
                        }
                    }
                }
            }

            // No joy...
            a_szValue = "";
            a_iBytes = 0;
            a_blGlobal = false;
            return (false);
        }

        /// <summary>
        /// Set a global or a local variable...
        /// </summary>
        /// <param name="a_szKey">key to set</param>
        /// <param name="a_szValue">value to set</param>
        /// <param name="a_iBytes">bytes to set</param>
        /// <param name="a_variablescope">scope</param>
        public void SetVariable(string a_szKey, string a_szValue, int a_iBytes, VariableScope a_variablescope = VariableScope.Auto)
        {
            int iKey;

            // If automatic, check if we have a value in either list...
            if (a_variablescope == VariableScope.Auto)
            {
                string szValue;
                int iBytes;
                bool blGlobal;
                bool blFound = GetVariable(a_szKey, -1, out szValue, out iBytes, out blGlobal);
                // If not found, force local...
                if (!blFound)
                {
                    a_variablescope = VariableScope.Local;
                }
                // Otherwise, force to what we found...
                else
                {
                    a_variablescope = blGlobal ? VariableScope.Global : VariableScope.Local;
                }
            }

            // We're local if not-global, or if we don't have a stack...
            if ((a_variablescope == VariableScope.Local) || (m_lcallstack.Count == 0))
            {
                CallStack callstack;

                // This is what we'll be referencing...
                callstack = m_lcallstack[m_lcallstack.Count - 1];
                if (callstack.lkeyvalue == null)
                {
                    callstack.lkeyvalue = new List<KeyValue>();
                }

                // Find the value for this key...
                for (iKey = 0; iKey < callstack.lkeyvalue.Count; iKey++)
                {
                    if (callstack.lkeyvalue[iKey].szKey == a_szKey)
                    {
                        break;
                    }
                }

                // If we have no value to set, then delete this item...
                if (string.IsNullOrEmpty(a_szValue))
                {
                    if (iKey < callstack.lkeyvalue.Count)
                    {
                        callstack.lkeyvalue.Remove(callstack.lkeyvalue[iKey]);
                        m_lcallstack[m_lcallstack.Count - 1] = callstack;
                    }
                    return;
                }

                // Create a new keyvalue...
                KeyValue keyvalueNew = default(KeyValue);
                keyvalueNew.szKey = a_szKey;
                keyvalueNew.szValue = a_szValue;
                keyvalueNew.iBytes = a_iBytes;

                // If the key already exists, update its value...
                if (iKey < callstack.lkeyvalue.Count)
                {
                    callstack.lkeyvalue[iKey] = keyvalueNew;
                    m_lcallstack[m_lcallstack.Count - 1] = callstack;
                    return;
                }

                // Otherwise, add it, and sort...
                callstack.lkeyvalue.Add(keyvalueNew);
                callstack.lkeyvalue.Sort(SortByKeyAscending);
                m_lcallstack[m_lcallstack.Count - 1] = callstack;

                // All done...
                return;
            }

            // Global: we need protection...
            lock (m_objectKeyValue)
            {
                // Find the value for this key...
                for (iKey = 0; iKey < m_lkeyvalue.Count; iKey++)
                {
                    if (m_lkeyvalue[iKey].szKey == a_szKey)
                    {
                        break;
                    }
                }

                // If we have no value to set, then delete this item...
                if (string.IsNullOrEmpty(a_szValue))
                {
                    if (iKey < m_lkeyvalue.Count)
                    {
                        m_lkeyvalue.Remove(m_lkeyvalue[iKey]);
                    }
                    return;
                }

                // Create a new keyvalue...
                KeyValue keyvalueNew = default(KeyValue);
                keyvalueNew.szKey = a_szKey;
                keyvalueNew.szValue = a_szValue;
                keyvalueNew.iBytes = a_iBytes;

                // If the key already exists, update its value...
                if (iKey < m_lkeyvalue.Count)
                {
                    m_lkeyvalue[iKey] = keyvalueNew;
                    return;
                }

                // Otherwise, add it, and sort...
                m_lkeyvalue.Add(keyvalueNew);
                m_lkeyvalue.Sort(SortByKeyAscending);
            }
        }

        /// <summary>
        /// A comparison operator for sorting keys in CmdSet...
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <returns></returns>
        private int SortByKeyAscending(KeyValue a_keyvalue1, KeyValue a_keyvalue2)
        {

            return (a_keyvalue1.szKey.CompareTo(a_keyvalue2.szKey));
        }

        /// <summary>
        /// Set the return value on the top callstack item...
        /// </summary>
        /// <param name="a_szReturn"></param>
        /// <returns></returns>
        private void SetReturnValue(string a_szReturnValue)
        {
            if (m_lcallstack.Count < 1) return;
            CallStack callstack = m_lcallstack[m_lcallstack.Count - 1];
            callstack.functionarguments.szReturnValue = a_szReturnValue;
            m_lcallstack[m_lcallstack.Count - 1] = callstack;
        }

        #endregion


        // Private Definitions
        #region Private Definitions

        /// <summary>
        /// Select the cloud import code we're working with...
        /// </summary>
        private enum CloudImport
        {
            Undefined,
            HazyBits
        }

        /// <summary>
        /// A key/value pair...
        /// </summary>
        private struct KeyValue
        {
            /// <summary>
            /// Our key...
            /// </summary>
            public string szKey;

            /// <summary>
            /// The key's value...
            /// </summary>
            public string szValue;

            /// <summary>
            /// The bytesize of value, only used when
            /// value is an IntPtr to memory...
            /// </summary>
            public int iBytes;
        }

        /// <summary>
        /// Call stack info...
        /// </summary>
        private struct CallStack
        {
            /// <summary>
            /// The arguments to this call...
            /// </summary>
            public Interpreter.FunctionArguments functionarguments;
            public List<KeyValue> lkeyvalue;
        }

        #endregion


        // Private Attributes
        #region Private Attributes

        /// <summary>
        /// Our TWAIN object...
        /// </summary>
        private TWAIN m_twain;

        // The handle to our window...
        private FormMain m_formmain;
        private IntPtr m_intptrHwnd;

        /// <summary>
        /// Map commands to functions...
        /// </summary>
        private List<Interpreter.DispatchTable> m_ldispatchtable;

        /// <summary>
        /// Our console input...embiggened...
        /// </summary>
        private StreamReader m_streamreaderConsole;
        private ConsoleColor m_consolecolorDefault;
        private ConsoleColor m_consolecolorBlue;
        private ConsoleColor m_consolecolorGreen;
        private ConsoleColor m_consolecolorRed;
        private ConsoleColor m_consolecolorYellow;

        /// <summary>
        /// No output when this is true...
        /// </summary>
        private bool m_blSilent;
        private bool m_blSilentEvents;
        private bool m_blRunningScript;
        private bool m_blVerbose;

        /// <summary>
        /// The list of key/value pairs created by the SET command...
        /// </summary>
        private List<KeyValue> m_lkeyvalue;
        private object m_objectKeyValue;

        /// <summary>
        /// A last in first off stack of function calls...
        /// </summary>
        private List<CallStack> m_lcallstack;

        /// <summary>
        /// Our certification report, in glorious text...
        /// </summary>
        private StringBuilder m_stringbuilderSelfCertReport;
        private string m_szSelfCertReportPath;
        private string m_szSelfCertReportProductname;

        /// <summary>
        /// The opening banner (program, version, etc)...
        /// </summary>
        private string m_szBanner;

        /// <summary>
        /// List of scanners paired with us in a cloud...
        /// </summary>
        private AutoResetEvent m_autoreseteventScannerslist;

        /// <summary>
        /// We use this to run code in the context of the caller's UI thread...
        /// </summary>
        /// <param name="a_object">object (really a control)</param>
        /// <param name="a_action">code to run</param>
        public delegate void RunInUiThreadDelegate(Object a_object, Action a_action);

        /// <summary>
        /// The list of pending DAT_NULL events...
        /// </summary>
        private List<TWAIN.MSG> m_lmsgDatNull = new List<TWAIN.MSG>();
        private AutoResetEvent m_autoreseteventMsgDatNull = new AutoResetEvent(false);

        #endregion
    }

    /// <summary>
    /// Our configuration object.  We must be able to access this
    /// from anywhere in the code...
    /// </summary>
    public static class Config
    {
        // Public Methods...
        #region Public Methods...

        /// <summary>
        /// Get a value, if the value can't be found, return a default.  We look
        /// for the item first in the command line arguments, then in the data
        /// read from the config file, and finally based on a small collection of
        /// keywords accessing static information.  This way we can override the
        /// configuration at any point with a minimum of fuss.
        /// </summary>
        /// <param name="a_szKey">the item we're seeking</param>
        /// <param name="a_szDefault">the default if we don't find it</param>
        /// <returns>the result</returns>
        public static string Get(string a_szKey, string a_szDefault)
        {
            // Try the command line first...
            if (ms_aszCommandLine != null)
            {
                string szKey = a_szKey + "=";
                foreach (string sz in ms_aszCommandLine)
                {
                    if ((sz == a_szKey) || (sz == szKey))
                    {
                        return ("");
                    }
                    if (sz.StartsWith(szKey))
                    {
                        return (sz.Remove(0, szKey.Length));
                    }
                }
            }

            // Try the JSON...
            if (ms_jsonlookup != null)
            {
                string szValue;
                JsonLookup.EPROPERTYTYPE epropertytype;
                if (ms_jsonlookup.GetCheck(a_szKey, out szValue, out epropertytype, false))
                {
                    return (szValue);
                }
            }

            // Try the folders...
            if (a_szKey == "executablePath")
            {
                return (ms_szExecutablePath);
            }
            if (a_szKey == "executableName")
            {
                return (ms_szExecutableName);
            }
            if (a_szKey == "readFolder")
            {
                return (ms_szReadFolder);
            }
            if (a_szKey == "writeFolder")
            {
                return (ms_szWriteFolder);
            }

            // This is our default for the pfxFile, but only
            // if the user didn't override it with their own
            // default...
            if ((a_szKey == "pfxFile") && string.IsNullOrEmpty(a_szDefault))
            {
                return (Path.Combine(Path.Combine(ms_szReadFolder, "data"), "certificate.p12"));
            }

            // All done...
            return (a_szDefault);
        }

        /// <summary>
        /// Get a long value, if the value can't be found, return a default...
        /// </summary>
        /// <param name="a_szKey"></param>
        /// <param name="a_szDefault"></param>
        /// <returns></returns>
        public static long Get(string a_szKey, long a_lDefault)
        {
            // Get the value...
            string szValue = Get(a_szKey, "@@@NOTFOUND@@@@");

            // We didn't find it, use the default...
            if (szValue == "@@@NOTFOUND@@@@")
            {
                return (a_lDefault);
            }

            // Try to get the value...
            long lValue;
            if (long.TryParse(szValue, out lValue))
            {
                return (lValue);
            }

            // No joy, use the default...
            return (a_lDefault);
        }

        /// <summary>
        /// Get a double value, if the value can't be found, return a default...
        /// </summary>
        /// <param name="a_szKey"></param>
        /// <param name="a_szDefault"></param>
        /// <returns></returns>
        public static double Get(string a_szKey, double a_dfDefault)
        {
            // Get the value...
            string szValue = Get(a_szKey, "@@@NOTFOUND@@@@");

            // We didn't find it, use the default...
            if (szValue == "@@@NOTFOUND@@@@")
            {
                return (a_dfDefault);
            }

            // Try to get the value...
            double dfValue;
            if (double.TryParse(szValue, out dfValue))
            {
                return (dfValue);
            }

            // No joy, use the default...
            return (a_dfDefault);
        }

        /// <summary>
        /// Return the command line passed into us...
        /// </summary>
        /// <returns>the command line</returns>
        public static string[] GetCommandLine()
        {
            return (ms_aszCommandLine);
        }

        /// <summary>
        /// Get a string from the supplied resource, return the key if we can't
        /// find the resource...
        /// </summary>
        /// <param name="a_resourcemanager">resource to use</param>
        /// <param name="a_szKey">key to lookup</param>
        /// <returns>string we found, or the keyname</returns>
        public static string GetResource(ResourceManager a_resourcemanager, string a_szKey)
        {
            string szResult = "";

            // Ruh-roh...
            if (a_resourcemanager == null)
            {
                return (a_szKey);
            }

            // Also, ruh-roh...
            try
            {
                szResult = a_resourcemanager.GetString(a_szKey);
            }
            catch
            {
                return (a_szKey);
            }
            if (string.IsNullOrEmpty(szResult))
            {
                return (a_szKey);
            }

            // Got it...
            return (szResult);
        }

        /// <summary>
        /// Load the configuration object.  We want to read in the
        /// configuaration data (in JSON format) and a list of the
        /// command line arguments.
        /// </summary>
        /// <param name="a_szExecutablePath">the fill path to the program using us</param>
        /// <param name="a_szCommandLine">key[=value] groupings</param>
        /// <param name="a_szConfigFile">a JSON file</param>
        public static bool Load(string a_szExecutablePath, string[] a_aszCommandLine, string a_szConfigFile)
        {
            try
            {
                // Work out where our executable lives...
                ms_szExecutablePath = a_szExecutablePath;
                ms_szExecutableName = Path.GetFileNameWithoutExtension(ms_szExecutablePath);

                // The read folder is the path to the executable.  This is where we're
                // going to find our appdata.txt file, which contains configuration
                // information that can be overridden by the user (assuming they have
                // rights to it).  We'll put other readonly stuff here too, like the
                // certification tests...
                ms_szReadFolder = Path.GetDirectoryName(ms_szExecutablePath);

                // The write folder is the path to all of the the files we can update,
                // which includes image files, metadata, log files, registration/selection
                // files.  This stuff is specific to a user, so by default we're going to
                // keep it in their %appdata%/twaindirect/executablename folder...
                ms_szWriteFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                ms_szWriteFolder = Path.Combine(ms_szWriteFolder, "twaindirect");
                ms_szWriteFolder = Path.Combine(ms_szWriteFolder, ms_szExecutableName);

                // Store the command line...
                ms_aszCommandLine = a_aszCommandLine;

                // Load the config, we'll first look for a name decorated version
                // of the file (ex: TwainDirect.Scanner.appdata.txt) in the data
                // folder, and if that fails we'll try the same folder as the .exe,
                // if that fails we'll do just the name given to us...
                string szConfigFile = Path.Combine(Path.Combine(ms_szReadFolder, "data"), ms_szExecutableName + "." + a_szConfigFile);
                if (!File.Exists(szConfigFile))
                {
                    szConfigFile = Path.Combine(ms_szReadFolder, ms_szExecutableName + "." + a_szConfigFile);
                    if (!File.Exists(szConfigFile))
                    {
                        szConfigFile = Path.Combine(ms_szReadFolder, a_szConfigFile);
                    }
                }
                if (File.Exists(szConfigFile))
                {
                    long a_lJsonErrorindex;
                    string szConfig = File.ReadAllText(szConfigFile);
                    ms_jsonlookup = new JsonLookup();
                    ms_jsonlookup.Load(szConfig, out a_lJsonErrorindex);
                }

                // Check if the user wants to override the read and write folders...
                ms_szReadFolder = Get("readFolder", ms_szReadFolder);
                ms_szWriteFolder = Get("writeFolder", ms_szWriteFolder);

                // Make sure we have a write folder...
                if (!Directory.Exists(ms_szWriteFolder))
                {
                    Directory.CreateDirectory(ms_szWriteFolder);
                }
            }
            catch
            {
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Return 32 or 64...
        /// </summary>
        /// <returns>32 or 64</returns>
        public static long GetMachineWordSize()
        {
            if (IntPtr.Size == 4)
            {
                return (32);
            }
            return (64);
        }

        /// <summary>
        /// Put a UAC shield on a button...
        /// </summary>
        public static void ElevateButton(IntPtr a_intptrHandle)
        {
            NativeMethods.SendMessage(a_intptrHandle, NativeMethods.BCM_SETSHIELD, IntPtr.Zero, IntPtr.Zero - 1);
        }

        #endregion


        // Private Attributes...
        #region Private Attributes

        /// <summary>
        /// The command line arguments...
        /// </summary>
        private static string[] ms_aszCommandLine = null;

        /// <summary>
        /// The JSON lookup object that contains any configuration data
        /// tht we want to access using a Get() command...
        /// </summary>
        private static JsonLookup ms_jsonlookup = null;

        /// <summary>
        /// The full path to our program...
        /// </summary>
        private static string ms_szExecutablePath;

        /// <summary>
        /// The base name, without extension of our program...
        /// </summary>
        private static string ms_szExecutableName;

        /// <summary>
        /// The readonly folder, which includes binaries and template
        /// files that are never updated by running code...
        /// </summary>
        private static string ms_szReadFolder;

        /// <summary>
        /// The writeable folder, which is where logs, temporary files,
        /// and configurable content is located...
        /// </summary>
        private static string ms_szWriteFolder;

        #endregion
    }

    /// <summary>
    /// Interpret and dispatch user commands...
    /// </summary>
    public sealed class Interpreter
    {
        // Public Methods
        #region Public Methods

        /// <summary>
        /// Our constructor...
        /// </summary>
        /// <param name="a_szPrompt">initialize the prompt</param>
        public Interpreter(string a_szPrompt, ConsoleColor a_consolecolorDefault, ConsoleColor a_consolecolorPrompt)
        {
            // Our prompt...
            m_szPrompt = (string.IsNullOrEmpty(a_szPrompt) ? ">>>" : a_szPrompt);
            m_consolecolorDefault = a_consolecolorDefault;
            m_consolecolorPrompt = a_consolecolorPrompt;
        }

        /// <summary>
        /// Create a console on Windows...
        /// </summary>
        public static StreamReader CreateConsole()
        {
            // Make sure we have a console...
            if (TWAIN.GetPlatform() == TWAIN.Platform.WINDOWS)
            {
                // Get our console...
                NativeMethods.AllocConsole();

                // Only do this junk if running under Visual Studio, note that we'll
                // lose color.  So it goes...
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    var handle = NativeMethods.CreateFile
                    (
                        "CONOUT$",
                        NativeMethods.DesiredAccess.GenericWrite | NativeMethods.DesiredAccess.GenericWrite,
                        NativeMethods.FileShare.Read | NativeMethods.FileShare.Write,
                        IntPtr.Zero,
                        NativeMethods.CreationDisposition.OpenExisting,
                        NativeMethods.FileAttributes.Normal,
                        IntPtr.Zero
                    );
                    SafeFileHandle safefilehandle = new SafeFileHandle(handle, true);
                    FileStream fileStream = new FileStream(safefilehandle, FileAccess.Write);
                    StreamWriter streamwriterStdout = new StreamWriter(fileStream, Encoding.ASCII);
                    streamwriterStdout.AutoFlush = true;
                    Console.SetOut(streamwriterStdout);
                }
            }

            // And because life is hard, we need to up the size of standard input...
            StreamReader streamreaderConsole = new StreamReader(Console.OpenStandardInput(65536));
            return (streamreaderConsole);
        }

        /// <summary>
        /// Get the desktop windows for Windows systems...
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetDesktopWindow()
        {
            // Get an hwnd...
            if (TWAIN.GetPlatform() == TWAIN.Platform.WINDOWS)
            {
                return (NativeMethods.GetDesktopWindow());
            }
            else
            {
                return (IntPtr.Zero);
            }
        }

        /// <summary>
        /// Prompt for input, returning a string, if there's any data...
        /// </summary>
        /// <param name="a_streamreaderConsole">the console to use</param>
        /// <param name="a_iTwainState">twain state</param>
        /// <returns>data captured</returns>
        public string Prompt(StreamReader a_streamreaderConsole, int a_iTwainState)
        {
            string szCmd;

            // Read in a line...
            while (true)
            {
                string szPrompt = m_szPrompt;
                if (a_iTwainState > 0)
                {
                    szPrompt = szPrompt.Replace(">>>", a_iTwainState + ">>>");
                }

                // Write out the prompt...
                if (Console.BackgroundColor == ConsoleColor.Black)
                {
                    Console.ForegroundColor = m_consolecolorPrompt;
                    Console.Out.Write(szPrompt);
                    Console.ForegroundColor = m_consolecolorDefault;
                }
                else
                {
                    Console.Out.Write(szPrompt);
                }

                // Read in a line...
                if (Environment.OSVersion.ToString().Contains("Microsoft Windows"))
                {
                    szCmd = (a_streamreaderConsole == null) ? Console.In.ReadLine() : a_streamreaderConsole.ReadLine();
                    if (string.IsNullOrEmpty(szCmd))
                    {
                        continue;
                    }
                }
                else
                {
                    List<char> lchLine = new List<char>();
                    TextReader textreader = (a_streamreaderConsole == null) ? Console.In : a_streamreaderConsole;
                    bool blTyping = true;
                    while (blTyping)
                    {
                        int iCh = textreader.Read();
                        switch (iCh)
                        {
                            // A character...
                            default:
                                lchLine.Add((char)iCh);
                                Console.Out.Write((char)iCh);
                                break;

                            // Backspace and delete...
                            case 0x08:
                            case 0x7f:
                                if (lchLine.Count > 0)
                                {
                                    lchLine.RemoveAt(lchLine.Count - 1);
                                    Console.Out.Write('\b');
                                    Console.Out.Write(' ');
                                    Console.Out.Write('\b');
                                }
                                break;

                            // Newline and carriage return...
                            case 0x0a:
                            case 0x0d:
                                Console.Out.Write((char)iCh);
                                blTyping = false;
                                break;
                        }
                    }
                    szCmd = "";
                    foreach (char ch in lchLine)
                    {
                        szCmd += ch;
                    }
                }

                // Trim whitespace...
                szCmd = szCmd.Trim();
                if (string.IsNullOrEmpty(szCmd))
                {
                    continue;
                }

                // We must have data...
                break;
            }

            // All done...
            return (szCmd);
        }

        /// <summary>
        /// Change the prompt...
        /// </summary>
        /// <param name="a_szPrompt">new prompt</param>
        public void SetPrompt(string a_szPrompt)
        {
            m_szPrompt = a_szPrompt;
        }

        /// <summary>
        /// Tokenize a string, with support for single quotes and double quotes.
        /// Inside the body of a quote the only thing can can be (or needs to be)
        /// escaped is the current quote token.  The result is an array of strings...
        /// </summary>
        /// <param name="a_szCmd">command to tokenize</param>
        /// <returns>array of strings</returns>
        public string[] Tokenize(string a_szCmd)
        {
            int cc;
            int tt;
            char szQuote = (char)0;
            string[] aszTokens;

            // We're coming out of this with at least one token...
            aszTokens = new string[1];
            tt = 0;

            // Validate...
            if (string.IsNullOrEmpty(a_szCmd))
            {
                aszTokens[tt] = "";
                return (aszTokens);
            }

            // Handle comments...
            if (a_szCmd[0] == ';')
            {
                aszTokens[tt] = "";
                return (aszTokens);
            }

            // Skip over goto labels...
            if (a_szCmd[0] == ':')
            {
                aszTokens[tt] = "";
                return (aszTokens);
            }

            // If we have no special characters, then we're done...
            if (a_szCmd.IndexOfAny(new char[] { ' ', '\t', '\'', '"' }) == -1)
            {
                aszTokens[tt] = a_szCmd;
                return (aszTokens);
            }

            // Devour leading whitespace...
            cc = 0;
            while ((cc < a_szCmd.Length) && ((a_szCmd[cc] == ' ') || (a_szCmd[cc] == '\t')))
            {
                cc += 1;
            }

            // Loopy...
            while (cc < a_szCmd.Length)
            {
                // Handle single and double quotes...
                if ((a_szCmd[cc] == '\'') || (a_szCmd[cc] == '"'))
                {
                    // Skip the quote...
                    szQuote = a_szCmd[cc];
                    cc += 1;

                    // Copy all of the string to the next unescaped single quote...
                    while (cc < a_szCmd.Length)
                    {
                        // We found our terminator (don't copy it)...
                        if (a_szCmd[cc] == szQuote)
                        {
                            szQuote = (char)0;
                            cc += 1;
                            break;
                        }

                        // We're escaping the quote...
                        if ((cc + 1 < a_szCmd.Length) && (a_szCmd[cc] == '\\') && (a_szCmd[cc + 1] == szQuote))
                        {
                            aszTokens[tt] += szQuote;
                            cc += 1;
                        }

                        // Otherwise, just copy the character...
                        else
                        {
                            aszTokens[tt] += a_szCmd[cc];
                        }

                        // Next character...
                        cc += 1;
                    }
                }

                // Handle whitespace...
                else if ((a_szCmd[cc] == ' ') || (a_szCmd[cc] == '\t'))
                {
                    // Devour all of the whitespace...
                    while ((cc < a_szCmd.Length) && ((a_szCmd[cc] == ' ') || (a_szCmd[cc] == '\t')))
                    {
                        cc += 1;
                    }

                    // If we have more data, prep for it...
                    if (cc < a_szCmd.Length)
                    {
                        string[] asz = new string[aszTokens.Length + 1];
                        Array.Copy(aszTokens, asz, aszTokens.Length);
                        asz[aszTokens.Length] = "";
                        aszTokens = asz;
                        tt += 1;
                    }
                }

                // Bail if we find an inline comment...
                else if ((szQuote == (char)0) && (a_szCmd[cc] == ';'))
                {
                    Array.Resize<string>(ref aszTokens, aszTokens.Length - 1);
                    break;
                }

                // Anything else is data in the current token...
                else
                {
                    aszTokens[tt] += a_szCmd[cc];
                    cc += 1;
                }

                // Next character.,
            }

            // All done...
            return (aszTokens);
        }

        /// <summary>
        /// Dispatch a command...
        /// </summary>
        /// <param name="a_functionarguments">the arguments to the command</param>
        /// <param name="a_dispatchtable">dispatch table</param>
        /// <returns>true if the program should exit</returns>
        public bool Dispatch(ref FunctionArguments a_functionarguments, List<DispatchTable> a_ldispatchtable)
        {
            string szCmd;

            // Apparently we got nothing, it's a noop...
            if ((a_functionarguments.aszCmd == null) || (a_functionarguments.aszCmd.Length == 0) || string.IsNullOrEmpty(a_functionarguments.aszCmd[0]))
            {
                return (false);
            }

            // Find the command...
            szCmd = a_functionarguments.aszCmd[0].ToLowerInvariant();
            foreach (DispatchTable dispatchtable in a_ldispatchtable)
            {
                foreach (string sz in dispatchtable.m_aszCmd)
                {
                    if (sz == szCmd)
                    {
                        return (dispatchtable.m_function(ref a_functionarguments));
                    }
                }
            }

            // No joy, make sure to lose the last transaction if the
            // user enters a bad command, so that we reduce the risk
            // of it be badly interpreted later on...
            if (string.IsNullOrEmpty(a_functionarguments.szScriptFile))
            {
                Console.Out.WriteLine("command not found: " + a_functionarguments.aszCmd[0]);
            }
            else
            {
                Console.Out.WriteLine
                (
                    "command not found: " + a_functionarguments.aszCmd[0] +
                    " ('" + a_functionarguments.szScriptFile +
                    "' at line " + (a_functionarguments.iCurrentLine + 1) + ")"
                );
            }
            return (false);
        }

        #endregion


        // Public Definitions
        #region Public Definitions

        public struct FunctionArguments
        {
            /// <summary>
            /// The tokenized command...
            /// </summary>
            public string[] aszCmd;

            /// <summary>
            /// The script we're running or null, used for
            /// commands like "goto"...
            /// </summary>
            public string szScriptFile;
            public string[] aszScript;

            /// <summary>
            /// True if we've been asked to jump to a label,
            /// which includes the index to go to...
            /// </summary>
            public bool blGotoLabel;
            public int iLabelLine;

            /// <summary>
            /// The command triplet...
            /// </summary>
            public int iDg;
            public int iDat;
            public int iMsg;

            /// <summary>
            /// The status return from the driver...
            /// </summary>
            public TWAIN.STS sts;

            /// <summary>
            /// The function value when returning from a call...
            /// </summary>
            public string szReturnValue;

            /// <summary>
            /// The current line in the script...
            /// </summary>
            public int iCurrentLine;
        }

        /// <summary>
        /// Function to call from the Dispatcher...
        /// </summary>
        /// <param name="a_aszCmd">arguments</param>
        /// <param name="a_aszScript">script or null</param>
        /// <returns>true if the program should exit</returns>
        public delegate bool Function(ref FunctionArguments a_functionarguments);

        /// <summary>
        /// Map commands to functions...
        /// </summary>
        public class DispatchTable
        {
            /// <summary>
            /// Stock our entries...
            /// </summary>
            /// <param name="a_function">the function</param>
            /// <param name="a_aszCmd">command variants for this function</param>
            public DispatchTable(Function a_function, string[] a_aszCmd)
            {
                m_aszCmd = a_aszCmd;
                m_function = a_function;
            }

            /// <summary>
            /// Variations for this command...
            /// </summary>
            public string[] m_aszCmd;

            /// <summary>
            /// Function to call...
            /// </summary>
            public Function m_function;
        }

        #endregion


        // Private Attributes
        #region Private Attributes

        /// <summary>
        /// Our prompt...
        /// </summary>
        private string m_szPrompt;
        private ConsoleColor m_consolecolorDefault;
        private ConsoleColor m_consolecolorPrompt;

        #endregion
    }

    /// <summary>
    /// This is a simple JSON parser.  It was written by staring at the JSON
    /// railroad diagrams and coding like a loon, and then stress testing it
    /// with a variety of real world strings.
    /// 
    /// It makes no claims to being overly fast.  It is relatively efficient
    /// insofar as it makes a single pass, doesn't munge the original string
    /// and creates a tree representing the data.
    /// 
    /// It's main benefit is that it doesn't require a binding to a strongly
    /// typed class, so the data can be literally anything.  And it supports
    /// a simple dotted notation for looking up content.  Adding functions
    /// to allow for enumerating the data wouldn't be hard, but haven't been
    /// added at this time.
    /// 
    /// Two functions do all the heavy lifting:
    /// 
    ///     bool Load(string a_szJson, out lResponseCharacterOffset)
    ///     Causes the class to parse the JSON data and create an internal
    ///     data structure for it.  If a problem occurs it returns false and
    ///     an index into the string showering where the badness occurred.
    ///     
    ///     string Get(string a_szKey)
    ///     Returns a string for the item that was found.  If you need to
    ///     know the type, then use the GetCheck function.
    ///     
    ///     Here's some sample JSON:
    ///         {
    ///             "array": [
    ///                 {
    ///                     "first": 1
    ///                 },
    ///                 {
    ///                     "second": {
    ///                         "third": 3
    ///                     }
    ///                 }
    ///             ]
    ///         }
    ///         
    ///     To get the data for "third" we use the key:
    ///     
    ///         array[1].second.third
    ///         
    ///     As you might expect, if the property name has a dot or square
    ///     brackets in it, that would cause a problem.  This isn't an issue
    ///     for how we plan to use it, but it would be possible to get
    ///     around it if needed, by using a prefix delimiter in the lookup
    ///     string.
    /// 
    /// Clarity is a good thing, and hopefully this code is fairly simple to
    /// chug through.
    /// </summary>
    public sealed class JsonLookup
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Public Methods...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Methods...

        /// <summary>
        /// Init stuff...
        /// </summary>
        public JsonLookup()
        {
            m_blStrictParsingRules = false;
            m_property = null;
            m_szJson = null;
        }

        /// <summary>
        /// Dump the contents of the property tree...
        /// </summary>
        /// <returns>the JSON string</returns>
		public string Dump()
        {
            if (m_lkeyvalueOverride == null)
            {
                m_lkeyvalueOverride = new List<KeyValue>();
            }
            return (DumpPrivate(m_property, 0, "", false));
        }

        /// <summary>
        /// Does a find, and if the item is found, does a get...
        /// </summary>
        /// <param name="a_szKey">key to find</param>
        /// <param name="a_iStartingIndex">array index to start at</param>
        /// <param name="a_iCount">count of array elements to search</param>
        /// <param name="a_szValue">value to return</param>
        /// <param name="a_epropertytype">type of property that we've found</param>
        /// <param name="a_blLog">log if key isn't found</param>
        /// <returns>0 - n on success, -1 on failure</returns>
        public int FindGet(string a_szKey, int a_iStartingIndex, int a_iCount, out string a_szValue, out EPROPERTYTYPE a_epropertytype, bool a_blLog)
        {
            bool blSuccess;
            int iIndex;
            int iIndexBrackets;
            string szKey;

            // Init stuff...
            a_szValue = "";
            a_epropertytype = EPROPERTYTYPE.UNDEFINED;

            // Find the item...
            iIndex = FindKey(a_szKey, a_iStartingIndex, a_iCount);
            if (iIndex < 0)
            {
                return (iIndex);
            }

            // If we found it, then create the full key at this index...
            iIndexBrackets = a_szKey.IndexOf("[].");
            if (iIndexBrackets < 0)
            {
                return (-1);
            }
            szKey = a_szKey.Substring(0, iIndexBrackets) + "[" + iIndex + "]." + a_szKey.Substring(iIndexBrackets + 3);

            // And get the data...
            blSuccess = GetCheck(szKey, out a_szValue, out a_epropertytype, a_blLog);
            if (!blSuccess)
            {
                return (-1);
            }

            // All done...
            return (iIndex);
        }

        /// <summary>
        ///	Find a key in an array of objects.  The JSON needs to take
        ///	a form similar to:
        ///		{ "array":[
        ///			{"key1":data},
        ///			{"key2":data},
        ///			{"key3":data},
        ///			...
        ///		}
        ///	In this case a search string could be:
        ///		array[].key2
        ///	Where [] indicates the array to be enumerated, and key2 is
        ///	the property name we're searching for.  In the case of a
        ///	rooted array like:
        ///		[
        ///			{"key1":data},
        ///			{"key2":data},
        ///			{"key3":data},
        ///			...
        ///		]
        ///	Then the search string would be [].key2
        /// </summary>
        /// <param name="a_szKey">key to find</param>
        /// <param name="a_iStartingIndex">array index to start at</param>
        /// <param name="a_iCount">count of array elements to search</param>
        /// <returns>0 - n on success, -1 on failure</returns>
        public int FindKey(string a_szKey, int a_iStartingIndex = 0, int a_iCount = 0)
        {
            bool blSuccess;
            int iCount;
            int iIndex;
            string szKey;
            string szKeyLeft;
            string szKeyRight;
            string szValue;
            EPROPERTYTYPE epropertytype;

            // Validate...
            if (string.IsNullOrEmpty(a_szKey) || (a_iStartingIndex < 0) || (a_iCount < 0))
            {
                return (-1);
            }

            // The function is useless unless it can enumerate through an array,
            // so just bail if the user didn't ask for this...
            iIndex = a_szKey.IndexOf("[].");
            if (iIndex == -1)
            {
                return (-1);
            }

            // Get the left and right sides of the key...
            szKeyLeft = a_szKey.Substring(0, iIndex);
            szKeyRight = a_szKey.Substring(iIndex + 3);

            // The left side can be empty, but not the right side...
            if (string.IsNullOrEmpty(szKeyRight))
            {
                return (-1);
            }

            // The left side must be found, and it must be an array...
            blSuccess = GetCheck(szKeyLeft, out szValue, out epropertytype, false);
            if (!blSuccess || (epropertytype != EPROPERTYTYPE.ARRAY))
            {
                return (-1);
            }

            // Figure out our endpoint in the array...
            if (a_iCount == 0)
            {
                iCount = int.MaxValue;
            }
            else
            {
                iCount = a_iStartingIndex + a_iCount;
            }

            // Okay, it's time to loop.  We need to make sure the array item exists
            // before we look for the key, so this is a two step process...
            for (iIndex = a_iStartingIndex; iIndex < iCount; iIndex++)
            {
                // Build the array key...
                szKey = szKeyLeft + "[" + iIndex + "]";

                // Does the array element exist?  And is it an object?
                blSuccess = GetCheck(szKey, out szValue, out epropertytype, false);
                if (!blSuccess || (epropertytype != EPROPERTYTYPE.OBJECT))
                {
                    return (-1);
                }

                // Okay, try to get the full key...
                szKey += "." + szKeyRight;
                blSuccess = GetCheck(szKey, out szValue, out epropertytype, false);
                if (blSuccess)
                {
                    return (iIndex);
                }
            }

            // No joy...
            return (-1);
        }

        /// <summary>
        /// Get the string associated with the key.  This is a convenience
        /// function, because in most cases getting back a null string is
        /// enough, so we don't need a special boolean check...
        /// </summary>
        /// <param name="a_szKey">dotted key notation</param>
        /// <param name="a_szValue">value to return</param>
        /// <returns>string on success, else null</returns>
        public string Get(string a_szKey, bool a_blLog = true)
        {
            bool blSuccess;
            string szValue;
            EPROPERTYTYPE epropertytype;

            // Do the lookup...
            blSuccess = GetCheck(a_szKey, out szValue, out epropertytype, a_blLog);

            // We're good...
            if (blSuccess)
            {
                return (szValue);
            }

            // Ruh-roh...
            return (null);
        }

        /// <summary>
        /// Get the string associated with the key, and let us know how
        /// the lookup turned out...
        /// </summary>
        /// <param name="a_szKey">dotted key notation</param>
        /// <param name="a_szValue">value to return</param>
        /// <param name="a_epropertytype">type of property that we've found</param>
        /// <param name="a_blLog">log if key isn't found</param>
        /// <returns>true on success</returns>
		public bool GetCheck(string a_szKey, out string a_szValue, out EPROPERTYTYPE a_epropertytype, bool a_blLog)
        {
            string[] aszKey;
            string szIndex;
            string szBaseName;
            string szProperty;
            UInt32 kk;
            UInt32 uu;
            UInt32 u32Index;
            Property property;

            // Init...
            a_szValue = "";
            a_epropertytype = EPROPERTYTYPE.UNDEFINED;

            // Validate...
            if ((a_szKey == null) || (m_property == null))
            {
                Log.Error("GetCheck: null argument...");
                return (false);
            }

            // If the key is empty, return the whole object...
            if (a_szKey.Length == 0)
            {
                a_szValue = m_szJson;
                a_epropertytype = m_property.epropertytype;
                return (true);
            }

            // Fully tokenize the key so we can look ahead when needed...
            aszKey = a_szKey.Split('.');

            // Search, always skip the root if it's an object,
            // if it's an array we need to process it...
            property = m_property;
            if (property.epropertytype == EPROPERTYTYPE.OBJECT)
            {
                property = m_property.propertyChild;
            }
            for (kk = 0; kk < aszKey.Length; kk++)
            {
                // Extract the basename, in case we have an index...
                szBaseName = aszKey[kk];
                if (szBaseName.Contains("["))
                {
                    szBaseName = szBaseName.Substring(0, szBaseName.IndexOf('['));
                }

                // Look for a match among the siblings at this level...
                while (property != null)
                {
                    GetProperty(property, out szProperty);
                    if (szProperty == szBaseName)
                    {
                        break;
                    }
                    property = property.propertySibling;
                }

                // No joy...
                if (property == null)
                {
                    if (a_blLog)
                    {
                        Log.Info("GetCheck: key not found..." + a_szKey);
                    }
                    return (false);
                }

                // If we found a value, then we're done...
                if ((property.epropertytype == EPROPERTYTYPE.STRING)
                    || (property.epropertytype == EPROPERTYTYPE.NUMBER)
                    || (property.epropertytype == EPROPERTYTYPE.BOOLEAN)
                    || (property.epropertytype == EPROPERTYTYPE.NULL))
                {
                    // If there's more to the key, then we weren't successful...
                    if ((kk + 1) < aszKey.Length)
                    {
                        if (a_blLog)
                        {
                            Log.Info("GetCheck: key not found..." + a_szKey);
                        }
                        return (false);
                    }

                    // Return what we found...
                    return (GetValue(property, out a_szValue, out a_epropertytype));
                }

                // We found an object...
                if (property.epropertytype == EPROPERTYTYPE.OBJECT)
                {
                    // We've no more keys, so return the object...
                    if ((kk + 1) >= aszKey.Length)
                    {
                        return (GetValue(property, out a_szValue, out a_epropertytype));
                    }

                    // Otherwise, step into the object...
                    property = property.propertyChild;
                    continue;
                }

                // If we're an array, we need to walk the siblings of the child...
                if (property.epropertytype == EPROPERTYTYPE.ARRAY)
                {
                    // If we don't have a '[' and this is the last key, return the whole thing...
                    if (!aszKey[kk].Contains("[") && ((kk + 1) >= aszKey.Length))
                    {
                        return (GetValue(property, out a_szValue, out a_epropertytype));
                    }
                    // If we don't have a '[' and there is more to the key, we're in trouble...
                    else if (!aszKey[kk].Contains("[") && ((kk + 1) < aszKey.Length))
                    {
                        if (a_blLog)
                        {
                            Log.Info("GetCheck: key not found..." + a_szKey);
                        }
                        return (false);
                    }

                    // We must have a valid index in the key...
                    szIndex = aszKey[kk].Substring(aszKey[kk].IndexOf('['));
                    if ((szIndex.Length < 3) || !szIndex.StartsWith("[") || !szIndex.EndsWith("]"))
                    {
                        if (a_blLog)
                        {
                            Log.Info("GetCheck: key not found..." + a_szKey);
                        }
                        return (false);
                    }

                    // Get the basename and look for a match...
                    if (!UInt32.TryParse(szIndex.Substring(1, szIndex.Length - 2), out u32Index))
                    {
                        if (a_blLog)
                        {
                            Log.Info("GetCheck: key not found..." + a_szKey);
                        }
                        return (false);
                    }

                    // Step into the child...
                    property = property.propertyChild;
                    if (property == null)
                    {
                        if (a_blLog)
                        {
                            Log.Info("GetCheck: key not found..." + a_szKey);
                        }
                        return (false);
                    }

                    // Walk the siblings in this child...
                    for (uu = 0; uu < u32Index; uu++)
                    {
                        property = property.propertySibling;
                        if (property == null)
                        {
                            if (a_blLog)
                            {
                                Log.Info("GetCheck: key not found..." + a_szKey);
                            }
                            return (false);
                        }
                    }

                    // We've no more keys, so return the object...
                    if ((kk + 1) >= aszKey.Length)
                    {
                        return (GetValue(property, out a_szValue, out a_epropertytype));
                    }

                    // If the thing we hit is an object, then we need to step into it...
                    if (property.epropertytype == EPROPERTYTYPE.OBJECT)
                    {
                        property = property.propertyChild;
                    }

                    // Otherwise, keep on looking...
                    continue;
                }

                // Well, this was unexpected...
                Log.Info("GetCheck: unexpected error..." + a_szKey);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Get the string associated with the key.  This is a convenience
        /// function, because in most cases getting back a null string is
        /// enough, so we don't need a special boolean check.
        /// 
        /// The caller of this function is expecting to get back JSON data
        /// in string form.  We examine that data, looking for evidence
        /// that the strings are escaped, and if so, we unescape them.
        /// </summary>
        /// <param name="a_szKey">dotted key notation</param>
        /// <param name="a_szValue">value to return</param>
        /// <returns>string on success, else null</returns>
        public string GetJson(string a_szKey)
        {
            bool blSuccess;
            int iIndex;
            string szValue;
            EPROPERTYTYPE epropertytype;

            // Do the lookup...
            blSuccess = GetCheck(a_szKey, out szValue, out epropertytype, true);

            // No joy...
            if (!blSuccess)
            {
                return (null);
            }

            // Look for the first double-quote in the string, if the character
            // before it is a backslash, then we'll replace all of the \" with
            // ".  By definition the " has to be in index 1 or higher for this
            // to make sense, so we can ignore index 0...
            iIndex = szValue.IndexOf('"');
            if (iIndex >= 1)
            {
                if (szValue[iIndex - 1] == '\\')
                {
                    szValue = szValue.Replace("\\\"", "\"");
                }
            }

            // Ruh-roh...
            return (szValue);
        }

        /// <summary>
        /// Get the property of the key, if not found we'll come back
        /// with undefined...
        /// </summary>
        /// <param name="a_szKey">dotted key notation</param>
        /// <returns>type of the property</returns>
        public JsonLookup.EPROPERTYTYPE GetType(string a_szKey)
        {
            bool blSuccess;
            string szValue;
            JsonLookup.EPROPERTYTYPE epropertytype;

            // Do the lookup...
            blSuccess = GetCheck(a_szKey, out szValue, out epropertytype, true);

            // We're good...
            if (blSuccess)
            {
                return (epropertytype);
            }

            // Ruh-roh...
            return (JsonLookup.EPROPERTYTYPE.UNDEFINED);
        }

        /// <summary>
        /// Get the JSON data as XML.  This is a simple name/value conversion...
        /// </summary>
        /// <param name="a_szRootName">instead of o, use this as the name for the outermost tag</o></param>
        /// <returns>the XML string</returns>
        public string GetXml(string a_szRootName = "")
        {
            return (GetXmlPrivate(m_property, a_szRootName, 0, ""));
        }

        /// <summary>
        /// Loads a JSON string...
        /// </summary>
        /// <param name="a_szJson">JSON string to parse</param>
        /// <param name="a_lJsonErrorindex">index where error occurred, if return is false</param>
        /// <returns>true on success</returns>
		public bool Load(string a_szJson, out long a_lJsonErrorindex)
        {
            bool blSuccess;
            UInt32 u32Json;

            // Init stuff...
            a_lJsonErrorindex = 0;

            // Free old content...
            Unload();

            // We have no new data...
            if (a_szJson == null)
            {
                return (true);
            }

            // Make a copy of the string, in C# we'll work with the
            // index instead of pointers like we do in C/C++...
            u32Json = 0;
            m_szJson = a_szJson;

            // Parse the JSON and return...
            blSuccess = Deserialize(ref u32Json);
            if (!blSuccess)
            {
                a_lJsonErrorindex = u32Json;
                Unload();
            }

            // All done...
            return (blSuccess);
        }

        /// <summary>
        /// Add an override for the Dump.  A value of null can be used
        /// to "delete" an override.  Overrides are only supported for
        /// boolean, null, number, and string.  It would be possible to
        /// add support for array and object, but that seems a lot more
        /// risky, so I'm holding off for now.
        /// </summary>
        /// <param name="a_szKey"></param>
        /// <param name="a_szValue"></param>
        public void Override(string a_szKey, string a_szValue)
        {
            // Make sure we have our list...
            if (m_lkeyvalueOverride == null)
            {
                m_lkeyvalueOverride = new List<KeyValue>();
            }

            // If we already have this key, remove it...
            int iIndex = m_lkeyvalueOverride.FindIndex(item => item.szKey == a_szKey);
            if (iIndex >= 0)
            {
                m_lkeyvalueOverride.RemoveAt(iIndex);
            }

            // Add the new data to our list...
            if (a_szValue != null)
            {
                KeyValue keyvalue = new KeyValue();
                keyvalue.szKey = a_szKey;
                keyvalue.szValue = a_szValue;
                m_lkeyvalueOverride.Add(keyvalue);
            }
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Public Definitions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Definitions...

        /// <summary>
        /// The kinds of data types we can get from the class...
        /// </summary>
        public enum EPROPERTYTYPE
        {
            UNDEFINED = 0,
            ARRAY = 1,
            OBJECT = 2,
            STRING = 3,
            BOOLEAN = 4,
            NUMBER = 5,
            NULL = 6,
            LAST
        };

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Methods...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Methods...

        /// <summary>
        /// The control function for parsing the JSON string...
        /// </summary>
        /// <param name="a_u32Json">JSON to parse</param>
        /// <returns>true on success</returns>
        private bool Deserialize(ref UInt32 a_u32Json)
        {
            UInt32 u32Json = a_u32Json;

            // Validate...
            if (string.IsNullOrEmpty(m_szJson))
            {
                Log.Error("Deserialize: null arguments...");
                return (false);
            }

            // Initialize the first property as root...
            m_property = new Property();

            // Clear any whitespace...
            if (!SkipWhitespace(ref u32Json))
            {
                Log.Error("Deserialize: we ran out of data...");
                a_u32Json = u32Json;
                return (false);
            }

            // We have an object...
            if (m_szJson[(int)u32Json] == '{')
            {
                // What we are...
                m_property.epropertytype = EPROPERTYTYPE.OBJECT;

                // We don't need a colon, we just go straight to looking for the object,
                // this function returns the closing curly bracket (if it finds it)...
                if (!ParseObject(m_property, ref u32Json))
                {
                    Log.Error("Deserialize: ParseObject failed...");
                    a_u32Json = u32Json;
                    return (false);
                }
            }

            // Else we have an array...
            else if (m_szJson[(int)u32Json] == '[')
            {
                // What we are...
                m_property.epropertytype = EPROPERTYTYPE.ARRAY;

                // We don't need a colon, we just go straight to looking for the object,
                // this function returns the closing curly bracket (if it finds it)...
                if (!ParseArray(m_property, ref u32Json))
                {
                    Log.Error("Deserialize: ParseArray failed...");
                    a_u32Json = u32Json;
                    return (false);
                }
            }

            // Else we have a problem...
            else
            {
                Log.Error("Deserialize: bad token...");
                a_u32Json = u32Json;
                return (false);
            }


            // All of the remaining content can only be whitespace...
            if (SkipWhitespace(ref u32Json))
            {
                Log.Error("Deserialize: found cruft...");
                a_u32Json = u32Json;
                return (false);
            }

            // All done...
            a_u32Json = u32Json;
            return (true);
        }

        /// <summary>
        /// Diagnostic dump of the results of a Load, this function
        /// runs recursively...
        /// </summary>
        /// <param name="a_property">property to dump</param>
        /// <param name="a_iDepth">depth we're at</param>
        /// <param name="a_szKey">key for this item</param>
        /// <param name="a_blArray">true if we're elements in an array</param>
        /// <returns>the JSON string</returns>
        private string DumpPrivate
        (
            Property a_property,
            int a_iDepth,
            string a_szKey,
            bool a_blArray
        )
        {
            int iArray;
            string szKey;
            string szName;
            string szValue;
            string szResult;
            Property property;
            KeyValue keyvalue;
            EPROPERTYTYPE epropertytype;

            // Init...
            iArray = -1;
            szResult = "";
            property = a_property;
            if (property == null)
            {
                property = m_property;
            }

            // Dump...
            while (property != null)
            {
                // Our key...
                szKey = a_szKey;

                // We're in an array, so subscript us...
                if (a_blArray)
                {
                    iArray += 1;
                    szKey += "[" + iArray + "]";
                }

                switch (property.epropertytype)
                {
                    // This can't be right...
                    default:
                        return ("");

                    // Dump an array...
                    case EPROPERTYTYPE.ARRAY:
                        // name:[ or just [
                        GetProperty(property, out szName);
                        if (!string.IsNullOrEmpty(szName))
                        {
                            szResult += "\"" + szName + "\":[";
                            szKey += string.IsNullOrEmpty(szKey) ? szName : "." + szName;
                        }
                        else
                        {
                            szResult += "[";
                        }

                        // If we have a kiddie, dive down into it...
                        if (property.propertyChild != null)
                        {
                            szResult += DumpPrivate(property.propertyChild, a_iDepth + 1, szKey, true);
                        }

                        // If the last character is a comma, remove it...
                        if (szResult.EndsWith(","))
                        {
                            szResult = szResult.Remove(szResult.Length - 1);
                        }

                        // just ],
                        szResult += "],";
                        break;

                    // Dump a boolean, null, or number...
                    case EPROPERTYTYPE.BOOLEAN:
                    case EPROPERTYTYPE.NULL:
                    case EPROPERTYTYPE.NUMBER:
                        GetProperty(property, out szName);
                        szKey += string.IsNullOrEmpty(szKey) ? szName : "." + szName;
                        keyvalue = m_lkeyvalueOverride.Find(item => item.szKey == szKey);
                        if ((keyvalue == null) || (keyvalue.szValue == null))
                        {
                            GetValue(property, out szValue, out epropertytype);
                            szResult += "\"" + szName + "\":" + szValue + ",";
                        }
                        else
                        {
                            szResult += "\"" + szName + "\":" + keyvalue.szValue + ",";
                        }
                        break;

                    // Dump an object...
                    case EPROPERTYTYPE.OBJECT:
                        // name:{ or just {
                        GetProperty(property, out szName);
                        if (!string.IsNullOrEmpty(szName))
                        {
                            szResult += "\"" + szName + "\":{";
                            szKey += string.IsNullOrEmpty(szKey) ? szName : "." + szName;
                        }
                        else
                        {
                            szResult += "{";
                        }

                        // If we have a kiddie, dive down into it...
                        if (property.propertyChild != null)
                        {
                            szResult += DumpPrivate(property.propertyChild, a_iDepth + 1, szKey, false);
                        }

                        // If the last character is a comma, remove it...
                        if (szResult.EndsWith(","))
                        {
                            szResult = szResult.Remove(szResult.Length - 1);
                        }

                        // just },
                        szResult += "},";
                        break;

                    // Dump a string...
                    case EPROPERTYTYPE.STRING:
                        GetProperty(property, out szName);
                        szKey += string.IsNullOrEmpty(szKey) ? szName : "." + szName;
                        keyvalue = m_lkeyvalueOverride.Find(item => item.szKey == szKey);
                        if ((keyvalue == null) || (keyvalue.szValue == null))
                        {
                            GetValue(property, out szValue, out epropertytype);
                            szResult += "\"" + szName + "\":\"" + szValue + "\",";
                        }
                        else
                        {
                            szResult += "\"" + szName + "\":\"" + keyvalue.szValue + "\",";
                        }
                        break;
                }

                // Next sibling...
                property = property.propertySibling;
            }

            // If the last character is a comma, remove it...
            if (szResult.EndsWith(","))
            {
                szResult = szResult.Remove(szResult.Length - 1);
            }

            // All done...
            return (szResult);
        }

        /// <summary>
        /// Emit the JSON data as compact XML.  We're doing this in a
        /// literal fashion.  Therefore:
        /// 
        ///    {
        ///        "metadata": {
        ///             "address": {
        ///	                "imageNumber": 1,
        ///	                "imagePart": 1,
        ///	                "moreParts": lastPartInFile,
        ///	                "sheetNumber": 1,
        ///	                "source": "feederFront",
        ///	                "streamName": "stream0",
        ///	                "sourceName": "source0",
        ///	                "pixelFormatName": "pixelFormat0"
        ///             },
        ///             "image": {
        ///	                "compression": "none",
        ///	                "pixelFormat": "bw1",
        ///	                "pixelHeight": 2200,
        ///	                "pixelOffsetX": 0,
        ///	                "pixelOffsetY": 0,
        ///	                "pixelWidth": 1728,
        ///	                "resolution": 200,
        ///	                "size": 476279
        ///             },
		///             "status": {
		///	                 "success": true
		///             }
	    ///        }
        ///    }
        ///    
        /// Appears in XML as:
        ///    <o>
        ///        <o:metadata>
        ///             <o:address>
        ///	                <n:imageNumber>1</n:imageNumber>
        ///	                <n:imagePart>1</n:imagePart>
        ///	                <s:moreParts>lastPartInFile</s:moreParts>
        ///	                <n:sheetNumber>1</n:sheetNumber>
        ///	                <s:source>feederFront</s:source>
        ///	                <s:streamName>stream0</s:streamName>
        ///	                <s:sourceName>source0</s:sourceName>
        ///	                <s:pixelFormatName>pixelFormat0</s:pixelFormatName>
        ///             </o:address>
        ///             <o:image>
        ///	                <s:compression>none</s:compression>
        ///	                <pixelFormat>bw1</n:pixelFormat>
        ///	                <n:pixelHeight>2200</n:pixelHeight>
        ///	                <n:pixelOffsetX>0</n:pixelOffsetX>
        ///	                <n:pixelOffsetY>0</n:pixelOffsetY>
        ///	                <n:pixelWidth>1728</n:pixelWidth>
        ///	                <n:resolution>200</n:resolution>
        ///	                <n:size>476279</n:size>
        ///             </o:image>
		///             <o:status>
		///	                 <s:success>true</s:success>
		///             </o:status>
	    ///        </o:metadata>
        ///    </o>
        ///    
        /// Arrays are handle like so:
        /// 
        ///     {
        ///         "array": [1, 2, 3]
        ///     }
        ///     
        ///     <o>
        ///         <a:array>
        ///             <n:item>1</n:item>
        ///             <n:item>2</n:item>
        ///             <n:item>3</n:item>
        ///         </a:array>
        ///     </o>
        ///     
        /// We do allow overriding the outermost tag, so that instead
        /// of "o" it can be something a little more descriptive, like
        /// "tdm" for TWAIN metadata...
        /// 
        /// </summary>
        /// <param name="a_property">property to emit</param>
        /// <param name="a_szRootName">rootname to use for outermost tag at depth 0</param>
        /// <param name="a_iDepth">depth we're at</param>
        /// <param name="a_szXml">current string provided by caller</param>
        /// /// <returns>an XML string, or null on error</returns>
        private string GetXmlPrivate(Property a_property, string a_szRootName, int a_iDepth, string a_szXml)
        {
            string szXml = a_szXml;
            string szData;
            string szName;
            Property property;
            EPROPERTYTYPE epropertytype;

            // Init...
            property = a_property;
            if (property == null)
            {
                return (null);
            }

            // Loopy...
            while (property != null)
            {
                // Get the name...
                if (!GetProperty(property, out szName))
                {
                    return (null);
                }

                // If we didn't get a name, make one up...
                if (string.IsNullOrEmpty(szName))
                {
                    switch (property.epropertytype)
                    {
                        default: szName = "z"; break;
                        case EPROPERTYTYPE.ARRAY: szName = "a"; break;
                        case EPROPERTYTYPE.OBJECT:
                            // We can override the outermost tag...
                            if (!string.IsNullOrEmpty(a_szRootName) && (a_iDepth == 0))
                            {
                                szName = a_szRootName;
                            }
                            else
                            {
                                szName = "o";
                            }
                            break;
                    }
                }

                // If we got a name, prefix it with obj or arr, if needed...
                else
                {
                    switch (property.epropertytype)
                    {
                        default: szName = "z:" + szName; break;
                        case EPROPERTYTYPE.ARRAY: szName = "a:" + szName; break;
                        case EPROPERTYTYPE.BOOLEAN: szName = "b:" + szName; break;
                        case EPROPERTYTYPE.NULL: szName = "u:" + szName; break;
                        case EPROPERTYTYPE.NUMBER: szName = "n:" + szName; break;
                        case EPROPERTYTYPE.OBJECT: szName = "o:" + szName; break;
                        case EPROPERTYTYPE.STRING: szName = "s:" + szName; break;
                    }
                }

                // ADD: our opening tag...
                szXml += "<" + szName + ">";

                // Get the value...
                if (!GetValue(property, out szData, out epropertytype))
                {
                    return (null);
                }

                // Dive into our kiddie, if we have one...
                if (property.propertyChild != null)
                {
                    // Dive in...
                    szXml = GetXmlPrivate(property.propertyChild, "", a_iDepth + 1, szXml);
                    if (szXml == null)
                    {
                        return (null);
                    }
                }
                else
                {
                    szXml += szData;
                }

                // ADD: our closing tag...
                szXml += "</" + szName + ">";

                // Next sibling...
                property = property.propertySibling;
            }

            // This is what we have so far...
            return (szXml);
        }

        /// <summary>
        /// Free a property tree...
        /// </summary>
        /// <param name="a_property"></param>
        private void FreeProperty(Property a_property)
        {
            Property property;
            Property propertySibling;

            // Validate...
            if (a_property == null)
            {
                return;
            }

            // Remove siblings, go after children as needed...
            for (property = a_property; property != null; property = propertySibling)
            {
                // Next sibling...
                propertySibling = property.propertySibling;

                // Remove kiddies...
                if (property.propertyChild != null)
                {
                    FreeProperty(property.propertyChild);
                    property.propertyChild = null;
                }

                // Remove ourselves...
                property = null;
            }
        }

        /// <summary>
        /// Get a property name.  When the JSON rules are relaxed we allow
        /// for the following combinations:
        ///
        ///     "property": ...
        ///     'property': ...
        ///     \"property\": ...
        ///     \'property\': ...
        ///     property: ...
        ///     
        /// </summary>
        /// <param name="a_property">our place in the tree</param>
        /// <param name="a_szProperty">whatever we find (it can be empty)</param>
        /// <returns></returns>
        private bool GetProperty(Property a_property, out string a_szProperty)
        {
            // Init stuff...
            a_szProperty = "";

            // Validate the arguments...
            if (a_property == null)
            {
                Log.Error("GetProperty: null argument...");
                return (false);
            }

            // No name (we get this with the root and with arrays)...
            if (a_property.u32PropertyLength == 0)
            {
                return (true);
            }

            // Copy the property, losing the quotes...
            if ((m_szJson[(int)a_property.u32PropertyOffset] == '"') || (m_szJson[(int)a_property.u32PropertyOffset] == '\''))
            {
                a_szProperty = m_szJson.Substring((int)(a_property.u32PropertyOffset + 1), (int)(a_property.u32PropertyLength - 2));
            }

            // Under relaxed mode, handle escaped quotes...
            else if (((a_property.u32PropertyOffset + 1) < m_szJson.Length)
                     && ((m_szJson.Substring((int)a_property.u32PropertyOffset, 2) == "\\\"")
                     || (m_szJson.Substring((int)a_property.u32PropertyOffset, 2) == "\\'")))
            {
                a_szProperty = m_szJson.Substring((int)(a_property.u32PropertyOffset + 2), (int)(a_property.u32PropertyLength - 4));
            }

            // Under relaxed mode, we may not have quotes to lose...
            else
            {
                a_szProperty = m_szJson.Substring((int)a_property.u32PropertyOffset, (int)a_property.u32PropertyLength);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Get a value.  When the JSON rules are relaxed we allow for the
        /// following combinations:
        ///
        ///     "property": ...
        ///     'property': ...
        ///     \"property\": ...
        ///     \'property\': ...
        ///
        /// </summary>
        /// <param name="a_property">our place in the tree</param>
        /// <param name="a_szValue">whatever we find (it can be empty)</param>
        /// <param name="a_epropertytype">the type of the item we found</param>
        /// <returns></returns>
        private bool GetValue(Property a_property, out string a_szValue, out EPROPERTYTYPE a_epropertytype)
        {
            // Clear the target...
            a_szValue = "";
            a_epropertytype = EPROPERTYTYPE.UNDEFINED;

            // Validate the arguments...
            if (a_property == null)
            {
                Log.Error("GetValue: null argument...");
                return (false);
            }

            // Handle strings...
            if (a_property.epropertytype == EPROPERTYTYPE.STRING)
            {
                // Empty string...
                if (a_property.u32ValueLength == 0)
                {
                    a_epropertytype = a_property.epropertytype;
                    return (true);
                }

                // Handle escaped quotes...
                else if (((a_property.u32ValueOffset + 1) < m_szJson.Length)
                         && ((m_szJson.Substring((int)a_property.u32ValueOffset, 2) == "\\\"")
                         || (m_szJson.Substring((int)a_property.u32ValueOffset, 2) == "\\'")))
                {
                    a_szValue = m_szJson.Substring((int)(a_property.u32ValueOffset + 2), (int)(a_property.u32ValueLength - 4));
                }

                // Handle regular quotes...
                else
                {
                    // All we have is "" (an empty string)...
                    if (a_property.u32ValueLength == 2)
                    {
                        a_szValue = "";
                    }
                    // We have data in our quotes...
                    else
                    {
                        a_szValue = m_szJson.Substring((int)(a_property.u32ValueOffset + 1), (int)(a_property.u32ValueLength - 2));
                    }
                }
            }

            // Handle everything else...
            else
            {
                // Copy the entire block of data (whole objects and arrays included)...
                a_szValue = m_szJson.Substring((int)a_property.u32ValueOffset, (int)a_property.u32ValueLength);
            }

            // All done...
            a_epropertytype = a_property.epropertytype;
            return (true);
        }

        /// <summary>
        /// Work our way through an object...
        /// </summary>
        /// <param name="a_property">current place in the tree</param>
        /// <param name="a_u32Json">current offset into the JSON string</param>
        /// <returns></returns>
        private bool ParseObject(Property a_property, ref UInt32 a_u32Json)
        {
            UInt32 u32Json;
            UInt32 u32ValueOffset;
            Property property;
            Property propertyPrev;

            // Init stuff...
            u32Json = a_u32Json;

            // We have to start with an open square bracket...
            if (m_szJson[(int)u32Json] != '{')
            {
                Log.Error("ParseObject: expected open curly...");
                return (false);
            }

            // Make a note of where we are...
            u32ValueOffset = u32Json;

            // Skip the curly...
            u32Json += 1;

            // Clear any whitespace...
            if (!SkipWhitespace(ref u32Json))
            {
                Log.Error("ParseObject: we ran out of data...");
                a_u32Json = u32Json;
                return (false);
            }

            // We're an empty object...
            if (m_szJson[(int)u32Json] == '}')
            {
                a_property.epropertytype = EPROPERTYTYPE.OBJECT;
                a_property.u32ValueOffset = u32ValueOffset;
                a_property.u32ValueLength = (u32Json + 1) - a_property.u32ValueOffset;
                a_u32Json = u32Json + 1;
                return (true);
            }

            // Loopy...
            propertyPrev = a_property;
            for (; u32Json < m_szJson.Length; u32Json++)
            {
                // Create a new record...
                property = new Property();

                // First kiddie in the list, so it's our child...
                if (a_property.propertyChild == null)
                {
                    a_property.propertyChild = property;
                    propertyPrev = a_property.propertyChild;
                }
                // Append to the end of our child's sibling list...
                else
                {
                    propertyPrev.propertySibling = property;
                    propertyPrev = propertyPrev.propertySibling;
                }

                // Clear any whitespace...
                if (!SkipWhitespace(ref u32Json))
                {
                    Log.Error("ParseObject: we ran out of data...");
                    a_u32Json = u32Json;
                    return (false);
                }

                // This needs to be a property name...
                if (!ParseString(property, ref u32Json, false))
                {
                    Log.Error("ParseObject: ParseString failed...");
                    a_u32Json = u32Json;
                    return (false);
                }

                // Clear any whitespace...
                if (!SkipWhitespace(ref u32Json))
                {
                    a_u32Json = u32Json;
                    Log.Error("ParseObject: we ran out of data...");
                    return (false);
                }

                // We need a colon...
                if (m_szJson[(int)u32Json] != ':')
                {
                    Log.Error("ParseObject: expected a colon...");
                    a_u32Json = u32Json;
                    return (false);
                }

                // Clear the colon...
                u32Json += 1;

                // Clear any whitespace...
                if (!SkipWhitespace(ref u32Json))
                {
                    a_u32Json = u32Json;
                    Log.Error("ParseObject: we ran out of data...");
                    return (false);
                }

                // This needs to be a value...
                if (!ParseValue(property, ref u32Json))
                {
                    Log.Error("ParseObject: ParseValue failed...");
                    a_u32Json = u32Json;
                    return (false);
                }

                // Clear any whitespace...
                if (!SkipWhitespace(ref u32Json))
                {
                    Log.Error("ParseObject: we ran out of data...");
                    a_u32Json = u32Json;
                    return (false);
                }

                // If we see a comma, we have more coming...
                if (m_szJson[(int)u32Json] == ',')
                {
                    continue;
                }

                // If we see a closing square bracket, then we're done...
                if (m_szJson[(int)u32Json] == '}')
                {
                    a_property.epropertytype = EPROPERTYTYPE.OBJECT;
                    a_property.u32ValueOffset = u32ValueOffset;
                    a_property.u32ValueLength = (u32Json + 1) - a_property.u32ValueOffset;
                    a_u32Json = u32Json + 1;
                    return (true);
                }

                // Uh-oh...
                break;
            }

            // Uh-oh...
            Log.Error("ParseObject: expected a closing curly...");
            a_u32Json = u32Json;
            return (false);
        }

        /// <summary>
        /// Work our way through an array...
        /// </summary>
        /// <param name="a_property">current place in the tree</param>
        /// <param name="a_u32Json">current offset into the JSON string</param>
        /// <returns></returns>
        private bool ParseArray(Property a_property, ref UInt32 a_u32Json)
        {
            UInt32 u32Json;
            UInt32 u32ValueOffset;
            Property property;
            Property propertyPrev;

            // Init stuff...
            u32Json = a_u32Json;

            // We have to start with an open square bracket...
            if (m_szJson[(int)u32Json] != '[')
            {
                Log.Error("ParseArray: expected a open square bracket...");
                return (false);
            }

            // Make a note of where we are...
            u32ValueOffset = u32Json;

            // Skip the bracket...
            u32Json += 1;

            // Clear any whitespace...
            if (!SkipWhitespace(ref u32Json))
            {
                Log.Error("ParseObject: we ran out of data...");
                a_u32Json = u32Json;
                return (false);
            }

            // We're an empty array...
            if (m_szJson[(int)u32Json] == ']')
            {
                a_property.epropertytype = EPROPERTYTYPE.ARRAY;
                a_property.u32ValueOffset = u32ValueOffset;
                a_property.u32ValueLength = (u32Json + 1) - a_property.u32ValueOffset;
                a_u32Json = u32Json + 1;
                return (true);
            }

            // Loopy...
            propertyPrev = a_property;
            for (; u32Json < m_szJson.Length; u32Json++)
            {
                // Create a new record...
                property = new Property();

                // First kiddie in the list, so it's our child...
                if (a_property.propertyChild == null)
                {
                    a_property.propertyChild = property;
                    propertyPrev = a_property.propertyChild;
                }
                // Append to the end of our child's sibling list...
                else
                {
                    propertyPrev.propertySibling = property;
                    propertyPrev = propertyPrev.propertySibling;
                }

                // Clear any whitespace...
                if (!SkipWhitespace(ref u32Json))
                {
                    a_u32Json = u32Json;
                    Log.Error("ParseObject: we ran out of data...");
                    return (false);
                }

                // This needs to be a value...
                if (!ParseValue(property, ref u32Json))
                {
                    Log.Error("ParseArray: ParseValue failed...");
                    a_u32Json = u32Json;
                    return (false);
                }

                // Clear any whitespace...
                if (!SkipWhitespace(ref u32Json))
                {
                    Log.Error("ParseArray: we ran out of data...");
                    a_u32Json = u32Json;
                    return (false);
                }

                // If we see a comma, we have more coming...
                if (m_szJson[(int)u32Json] == ',')
                {
                    continue;
                }

                // If we see a closing square bracket, then we're done...
                if (m_szJson[(int)u32Json] == ']')
                {
                    a_property.epropertytype = EPROPERTYTYPE.ARRAY;
                    a_property.u32ValueOffset = u32ValueOffset;
                    a_property.u32ValueLength = (u32Json + 1) - a_property.u32ValueOffset;
                    a_u32Json = u32Json + 1;
                    return (true);
                }

                // Uh-oh...
                break;
            }

            // Uh-oh...
            Log.Error("ParseArray: expected a closing square bracket...");
            a_u32Json = u32Json;
            return (false);
        }

        /// <summary>
        /// Work our way through a value...
        /// </summary>
        /// <param name="a_property">current place in the tree</param>
        /// <param name="a_u32Json">current offset into the JSON string</param>
        /// <returns></returns>
        private bool ParseValue(Property a_property, ref UInt32 a_u32Json)
        {
            UInt32 u32Json = a_u32Json;

            switch (m_szJson[(int)u32Json])
            {
                // Well, that wasn't value...
                default:
                    Log.Error("ParseValue: unexpected token at (" + u32Json + ")...<" + m_szJson[(int)u32Json] + ">");
                    return (false);

                // A string or an escaped string...
                case '"':
                case '\'':
                case '\\':
                    return (ParseString(a_property, ref a_u32Json, true));

                // A number...
                case '-':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return (ParseNumber(a_property, ref a_u32Json));

                // An object...
                case '{':
                    return (ParseObject(a_property, ref a_u32Json));

                // An array...
                case '[':
                    return (ParseArray(a_property, ref a_u32Json));

                // A boolean true...
                case 't':
                    if (m_szJson[(int)(u32Json + 1)] != 'r')
                    {
                        Log.Error("ParseValue: it ain't tRue...");
                        a_u32Json = u32Json + 1;
                        return (false);
                    }
                    if (m_szJson[(int)(u32Json + 2)] != 'u')
                    {
                        Log.Error("ParseValue: it ain't trUe...");
                        a_u32Json = u32Json + 2;
                        return (false);
                    }
                    if (m_szJson[(int)(u32Json + 3)] != 'e')
                    {
                        Log.Error("ParseValue: it ain't truE...");
                        a_u32Json = u32Json + 3;
                        return (false);
                    }
                    a_u32Json = u32Json + 4;
                    a_property.epropertytype = EPROPERTYTYPE.BOOLEAN;
                    a_property.u32ValueOffset = u32Json;
                    a_property.u32ValueLength = 4;
                    return (true);

                // A boolean false...
                case 'f':
                    if (m_szJson[(int)(u32Json + 1)] != 'a')
                    {
                        Log.Error("ParseValue: it ain't fAlse...");
                        a_u32Json = u32Json + 1;
                        return (false);
                    }
                    if (m_szJson[(int)(u32Json + 2)] != 'l')
                    {
                        Log.Error("ParseValue: it ain't faLse...");
                        a_u32Json = u32Json + 2;
                        return (false);
                    }
                    if (m_szJson[(int)(u32Json + 3)] != 's')
                    {
                        Log.Error("ParseValue: it ain't falSe...");
                        a_u32Json = u32Json + 3;
                        return (false);
                    }
                    if (m_szJson[(int)(u32Json + 4)] != 'e')
                    {
                        Log.Error("ParseValue: it ain't falsE...");
                        a_u32Json = u32Json + 4;
                        return (false);
                    }
                    a_u32Json = u32Json + 5;
                    a_property.epropertytype = EPROPERTYTYPE.BOOLEAN;
                    a_property.u32ValueOffset = u32Json;
                    a_property.u32ValueLength = 5;
                    return (true);

                // A boolean null...
                case 'n':
                    if (m_szJson[(int)(u32Json + 1)] != 'u')
                    {
                        Log.Error("ParseValue: it ain't nUll...");
                        a_u32Json = u32Json + 1;
                        return (false);
                    }
                    if (m_szJson[(int)(u32Json + 2)] != 'l')
                    {
                        Log.Error("ParseValue: it ain't nuLl...");
                        a_u32Json = u32Json + 2;
                        return (false);
                    }
                    if (m_szJson[(int)(u32Json + 3)] != 'l')
                    {
                        Log.Error("ParseValue: it ain't nulL...");
                        a_u32Json = u32Json + 3;
                        return (false);
                    }
                    a_u32Json = u32Json + 4;
                    a_property.epropertytype = EPROPERTYTYPE.NULL;
                    a_property.u32ValueOffset = u32Json;
                    a_property.u32ValueLength = 4;
                    return (true);
            }
        }

        /// <summary>
        /// Work our way through a string (property or value)...
        /// </summary>
        /// <param name="a_property">current place in the tree</param>
        /// <param name="a_u32Json">current offset into the JSON string</param>
        /// <param name="a_blValue">true if a value</param>
        /// <returns></returns>
        bool ParseString(Property a_property, ref UInt32 a_u32Json, bool a_blValue)
        {
            string szQuote;
            UInt32 u32Json = a_u32Json;

            // Under strict rules the first character must be a doublequote...
            if (m_blStrictParsingRules)
            {
                if (m_szJson[(int)u32Json] != '"')
                {
                    Log.Error("ParseString: expected a opening doublequote...");
                    return (false);
                }
                szQuote = m_szJson[(int)u32Json].ToString();
            }

            // Come here for relaxed rules...
            else
            {
                // Handle escaped quotes...
                if (((u32Json + 1) < m_szJson.Length) && ((m_szJson.Substring((int)u32Json, 2) == "\\\"") || (m_szJson.Substring((int)u32Json, 2) == "\\'")))
                {
                    szQuote = m_szJson.Substring((int)u32Json, 2);
                }

                // A value must have an opening quote (double or single), or
                // if we detect that we have a quote, then pop in here...
                else if (a_blValue || (m_szJson[(int)u32Json] == '"') || (m_szJson[(int)u32Json] == '\''))
                {
                    if ((m_szJson[(int)u32Json] != '"') && (m_szJson[(int)u32Json] != '\''))
                    {
                        Log.Error("ParseString: expected an opening quote...");
                        return (false);
                    }
                    szQuote = m_szJson[(int)u32Json].ToString();
                }

                // A property name can have quotes or be an alphanumeric, and underscore or a dollarsign...
                else
                {
                    if (!Char.IsLetterOrDigit(m_szJson[(int)u32Json]) && (m_szJson[(int)u32Json] != '_') && (m_szJson[(int)u32Json] != '$'))
                    {
                        Log.Error("ParseString: expected a valid property name...");
                        return (false);
                    }
                    szQuote = m_szJson[(int)u32Json].ToString();
                }
            }

            // Init stuff...
            if (a_blValue)
            {
                a_property.u32ValueOffset = u32Json;
            }
            else
            {
                a_property.u32PropertyOffset = u32Json;
            }

            // Clear the quote if we found one...
            if ((szQuote == "\\\"") || (szQuote == "\\'"))
            {
                u32Json += 2;
            }
            else if ((szQuote == "\"") || (szQuote == "'"))
            {
                u32Json += 1;
            }

            // Loopy...
            for (; u32Json < m_szJson.Length; u32Json++)
            {
                // Fail on a control character...
                if (Char.IsControl(m_szJson[(int)u32Json]))
                {
                    Log.Error("ParseString: detected a control character...");
                    a_u32Json = u32Json;
                    return (false);
                }

                // Under strict rules a doublequote ends us...
                if (m_blStrictParsingRules)
                {
                    if (m_szJson[(int)u32Json] == '"')
                    {
                        if (a_blValue)
                        {
                            a_property.u32ValueLength = (u32Json + 1) - a_property.u32ValueOffset;
                            a_property.epropertytype = EPROPERTYTYPE.STRING;
                        }
                        else
                        {
                            a_property.u32PropertyLength = (u32Json + 1) - a_property.u32PropertyOffset;
                            // Don't change the type, we could be anything...
                        }
                        a_u32Json = u32Json + 1;
                        return (true);
                    }
                }

                // Under relaxed rules we'll end on a matching escaped quite...
                else if (((u32Json + 1) < m_szJson.Length) && (szQuote == (m_szJson.Substring((int)u32Json, 2))))
                {
                    if (a_blValue)
                    {
                        a_property.u32ValueLength = (u32Json + 2) - a_property.u32ValueOffset;
                        a_property.epropertytype = EPROPERTYTYPE.STRING;
                    }
                    else
                    {
                        a_property.u32PropertyLength = (u32Json + 2) - a_property.u32PropertyOffset;
                        // Don't change the type, we could be anything...
                    }
                    a_u32Json = u32Json + 2;
                    return (true);
                }

                // Under relaxed rules we'll end on a matching quote, if we have one, this
                // path is guaranteed to catch the closing quote on a value...
                else if ((m_szJson[(int)u32Json].ToString() == szQuote) && ((szQuote == "\"") || (szQuote == "'")))
                {
                    if (a_blValue)
                    {
                        a_property.u32ValueLength = (u32Json + 1) - a_property.u32ValueOffset;
                        a_property.epropertytype = EPROPERTYTYPE.STRING;
                    }
                    else
                    {
                        a_property.u32PropertyLength = (u32Json + 1) - a_property.u32PropertyOffset;
                        // Don't change the type, we could be anything...
                    }
                    a_u32Json = u32Json + 1;
                    return (true);
                }

                // Otherwise, (still under relaxed rules) if we're a property name, and we
                // didn't open on a quote, we'll end on anything that isn't alphanumeric,
                // an underscore or a dollarsign...
                else if (!a_blValue
                         && (szQuote != "\"")
                         && (szQuote != "'")
                         && !Char.IsLetterOrDigit(m_szJson[(int)u32Json])
                         && (m_szJson[(int)u32Json] != '_')
                         && (m_szJson[(int)u32Json] != '$'))
                {
                    a_property.u32PropertyLength = u32Json - a_property.u32PropertyOffset;
                    // Don't change the type, we could be anything...
                    // Don't skip over this item...
                    a_u32Json = u32Json;
                    return (true);
                }

                // If we're not a backslash, we're okay...
                if (m_szJson[(int)u32Json] != '\\')
                {
                    continue;
                }

                // Handle escape characters...
                u32Json += 1;
                switch (m_szJson[(int)u32Json])
                {
                    default:
                        Log.Error("ParseString: bad escape character at (" + u32Json + ")...<" + m_szJson[(int)u32Json] + ">");
                        a_u32Json = u32Json;
                        return (false);

                    case '"':
                    case '\\':
                    case '/':
                    case 'n':
                    case 'r':
                    case 't':
                    case 'b':
                    case 'f':
                        continue;

                    // Make sure we have at least four of these in a row...
                    case 'u':
                        if (!IsXDigit(m_szJson[(int)(u32Json + 1)]))
                        {
                            Log.Error("ParseString: it ain't a \\uXxxx");
                            a_u32Json = u32Json + 1;
                            return (false);
                        }
                        if (!IsXDigit(m_szJson[(int)(u32Json + 2)]))
                        {
                            Log.Error("ParseString: it ain't a \\uxXxx");
                            a_u32Json = u32Json + 2;
                            return (false);
                        }
                        if (!IsXDigit(m_szJson[(int)(u32Json + 3)]))
                        {
                            Log.Error("ParseString: it ain't a \\uxxXx");
                            a_u32Json = u32Json + 3;
                            return (false);
                        }
                        if (!IsXDigit(m_szJson[(int)(u32Json + 4)]))
                        {
                            Log.Error("ParseString: it ain't a \\uxxxX");
                            a_u32Json = u32Json + 4;
                            return (false);
                        }
                        a_u32Json = u32Json + 5;
                        continue;
                }
            }

            // Uh-oh...
            Log.Error("ParseString: expected a closing quote or something...");
            a_u32Json = u32Json;
            return (false);
        }

        /// <summary>
        /// Work our way through a number...
        /// </summary>
        /// <param name="a_property">current place in the tree</param>
        /// <param name="a_u32Json">current offset into the JSON string</param>
        /// <returns></returns>
        bool ParseNumber(Property a_property, ref UInt32 a_u32Json)
        {
            bool blDecimalDetected;
            bool blExponentDetected;
            bool blExponentSignDetected;
            bool blExponentDigitDetected;
            bool blLeadingZero;
            bool blNonZeroDigitDetected;
            UInt32 u32Json;

            // Init stuff...
            blDecimalDetected = false;
            blExponentDetected = false;
            blExponentSignDetected = false;
            blExponentDigitDetected = false;
            blLeadingZero = false;
            blNonZeroDigitDetected = false;
            a_property.u32ValueOffset = a_u32Json;

            // Loopy...
            for (u32Json = a_u32Json; u32Json < m_szJson.Length; u32Json++)
            {
                // Detect termination of the number and watch for illegal values...
                switch (m_szJson[(int)u32Json])
                {
                    // We've a problem...
                    default:
                        Log.Error("ParseNumber: not a valid token in a number...");
                        a_u32Json = u32Json;
                        return (false);

                    // We're done (and okay) on the following...
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                    case ',':
                    case '}':
                    case ']':
                        // Bad exponent...
                        if (blExponentDetected && !blExponentDigitDetected)
                        {
                            Log.Error("ParseNumber: bad exponent...");
                            a_u32Json = u32Json;
                            return (false);
                        }

                        // Don't skip past this value, the function above us needs to be able to check it...
                        a_u32Json = u32Json;
                        a_property.epropertytype = EPROPERTYTYPE.NUMBER;
                        a_property.u32ValueLength = u32Json - a_property.u32ValueOffset;
                        return (true);

                    // We're good...
                    case '-':
                    case '.':
                    case '+':
                    case 'e':
                    case 'E':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                }

                // Fail on embedded or trailing minus (not part of exponent, that's further down)...
                // good: 1-23
                // bad: 1-, 1-123
                if (!blExponentDetected && (m_szJson[(int)u32Json] == '-'))
                {
                    if ((m_szJson[(int)u32Json] == '-') && ((u32Json != a_u32Json) || (u32Json >= m_szJson.Length)))
                    {
                        Log.Error("ParseNumber: problem with how minus is being used...");
                        a_u32Json = u32Json;
                        return (false);
                    }
                    continue;
                }

                // Detect a leading zero...
                if (!blNonZeroDigitDetected && (m_szJson[(int)u32Json] == '0'))
                {
                    // We can be the first or second item in the string...
                    if ((u32Json == a_u32Json) || (u32Json == (a_u32Json + 1)))
                    {
                        blLeadingZero = true;
                        continue;
                    }
                    Log.Error("ParseNumber: found a leading zero...");
                    a_u32Json = u32Json;
                    return (false);
                }

                // Fail on a leading zero...
                // ex: 000, 0123
                if (blLeadingZero && !blNonZeroDigitDetected && Char.IsDigit(m_szJson[(int)u32Json]))
                {
                    Log.Error("ParseNumber: found a leading zero...");
                    a_u32Json = u32Json;
                    return (false);
                }

                // Fail on multiple decimals or a decimal with no leading digit...
                if (m_szJson[(int)u32Json] == '.')
                {
                    if (blDecimalDetected
                        || (!blLeadingZero && !blNonZeroDigitDetected))
                    {
                        Log.Error("ParseNumber: bad decimal point...");
                        a_u32Json = u32Json;
                        return (false);
                    }
                    // Clear the leading zero check, we don't need this anymore...
                    blLeadingZero = false;
                    blDecimalDetected = true;
                    continue;
                }

                // Fail on multiple exponent or an exponent with no leading digit...
                if ((m_szJson[(int)u32Json] == 'e') || (m_szJson[(int)u32Json] == 'E'))
                {
                    if (blExponentDetected
                        || (!blLeadingZero && !blNonZeroDigitDetected))
                    {
                        Log.Error("ParseNumber: bad exponent...");
                        a_u32Json = u32Json;
                        return (false);
                    }
                    blExponentDetected = true;
                    continue;
                }

                // Fail on multiple exponent sign, or sign with no leading exponent,
                // or sign after exponent digit...
                if ((m_szJson[(int)u32Json] == '+') || (m_szJson[(int)u32Json] == '-'))
                {
                    if (blExponentSignDetected
                        || !blExponentDetected
                        || blExponentDigitDetected)
                    {
                        Log.Error("ParseNumber: bad exponent...");
                        a_u32Json = u32Json;
                        return (false);
                    }
                    blExponentSignDetected = true;
                    continue;
                }

                // Detected an integer digit...
                if (!blDecimalDetected && !blExponentDetected)
                {
                    switch (m_szJson[(int)u32Json])
                    {
                        default:
                            break;
                        case '-':
                        case '.':
                        case '+':
                        case 'e':
                        case 'E':
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            blNonZeroDigitDetected = true;
                            continue;
                    }
                }

                // Make sure we catch decimal numbers...
                if (Char.IsDigit(m_szJson[(int)u32Json]))
                {
                    if (!blExponentDetected)
                    {
                        blNonZeroDigitDetected = true;
                    }
                    else
                    {
                        blExponentDigitDetected = true;
                    }
                }
            }

            // Uh-oh...
            Log.Error("ParseNumber: problem with a number...");
            a_u32Json = u32Json;
            return (false);
        }

        /// <summary>
        /// Skip whitespace in the JSON string...
        /// </summary>
        /// <param name="a_u32Json">index to move</param>
        /// <returns>false if we run out of string</returns>
        private bool SkipWhitespace(ref UInt32 a_u32Json)
        {
            // Loopy...
            while (a_u32Json < m_szJson.Length)
            {
                if (!Char.IsWhiteSpace(m_szJson[(int)a_u32Json]))
                {
                    return (true);
                }
                a_u32Json += 1;
            }

            // We ran out of data...
            return (false);
        }

        /// <summary>
        /// Free resources...
        /// </summary>
        private void Unload()
        {
            m_szJson = null;

            if (m_property != null)
            {
                FreeProperty(m_property);
                m_property = null;
            }
        }

        /// <summary>
        /// C# leaves out the most amazing stuff...
        /// </summary>
        /// <param name="c">character to check</param>
        /// <returns>true if its a hexit</returns>
        private static bool IsXDigit(char c)
        {
            if (Char.IsDigit(c)) return true;
            if ((c >= 'a') && (c <= 'f')) return true;
            if ((c >= 'A') && (c <= 'F')) return true;
            return false;
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Definitions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Definitions...

        /// <summary>
        /// Our main data structure for holding onto the information about
        /// a JSON string after we've loaded it...
        /// </summary>
        private class Property
        {
            // Init stuff...
            public Property()
            {
                propertySibling = null;
                propertyChild = null;
                epropertytype = EPROPERTYTYPE.UNDEFINED;
                u32PropertyOffset = 0;
                u32PropertyLength = 0;
                u32ValueOffset = 0;
                u32ValueLength = 0;
            }

            // Our attributes...
            public Property propertySibling;
            public Property propertyChild;
            public EPROPERTYTYPE epropertytype;
            public UInt32 u32PropertyOffset;
            public UInt32 u32PropertyLength;
            public UInt32 u32ValueOffset;
            public UInt32 u32ValueLength;
        };

        /// <summary>
        /// Key/Value pair structure, used to override the
        /// values of keys during a Dump()...
        /// </summary>
        private class KeyValue
        {
            public string szKey;
            public string szValue;
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Attributes...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Attributes...

        /// <summary>
        /// A place to store the JSON string while we work with it...
        /// </summary>
        private string m_szJson;

        /// <summary>
        /// A place to store the load data on the JSON string...
        /// </summary>
        private Property m_property;

        /// <summary>
        /// If false, then property names don't have to have quotes
        /// as long as they don't have any embedded whitespace, and
        /// single-quotes are allowed.  This makes it easier on folks
        /// generating JSON from scripted languages, or if they use
        /// command line tools like cURL...
        /// </summary>
        private bool m_blStrictParsingRules;

        /// <summary>
        /// List of changes to the JSON in key/value pairs...
        /// </summary>
        private List<KeyValue> m_lkeyvalueOverride;

        #endregion
    }

    /// <summary>
    /// Our logger.  If we bump up to 4.5 (and if mono supports it at compile
    /// time), then we'll be able to add the following to our traces, which
    /// seems like it should be more than enough to locate log messages.  For
    /// now we'll leave the log messages undecorated:
    ///     [CallerFilePath] string file = "",
    ///     [CallerMemberName] string member = "",
    ///     [CallerLineNumber] int line = 0
    /// </summary>
    public static class Log
    {
        // Public Methods...
        #region Public Methods...

        /// <summary>
        /// Write an assert message, but only throw with a debug build...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void Assert(string a_szMessage)
        {
            WriteEntry("A", a_szMessage, true);
            #if DEBUG
            throw new Exception(a_szMessage);
            #endif
        }

        /// <summary>
        /// Close tracing...
        /// </summary>
        public static void Close()
        {
            if (!ms_blFirstPass)
            {
                Trace.Close();
                ms_filestream.Close();
                ms_filestream = null;
            }
            ms_blFirstPass = true;
            ms_blOpened = false;
            ms_blFlush = false;
            ms_iMessageNumber = 0;
        }

        /// <summary>
        /// Write an error message...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void Error(string a_szMessage)
        {
            WriteEntry("E", a_szMessage, true);
        }

        /// <summary>
        /// Get the debugging level...
        /// </summary>
        /// <returns>the level</returns>
        public static int GetLevel()
        {
            return (ms_iLevel);
        }

        /// <summary>
        /// Write an informational message...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void Info(string a_szMessage)
        {
            WriteEntry(".", a_szMessage, ms_blFlush);
        }

        /// <summary>
        /// Turn on the listener for our log file...
        /// </summary>
        /// <param name="a_szName">the name of our log</param>
        /// <param name="a_szPath">the path where we want our log to go</param>
        /// <param name="a_iLevel">debug level</param>
        public static void Open(string a_szName, string a_szPath, int a_iLevel)
        {
            string szLogFile;

            // Init stuff...
            ms_blFirstPass = true;
            ms_blOpened = true;
            ms_blFlush = false;
            ms_iMessageNumber = 0;
            ms_iLevel = a_iLevel;

            // We're Windows...
            if (Environment.OSVersion.ToString().Contains("Microsoft Windows"))
            {
                ms_blIsWindows = true;
            }

            // Ask for a TWAINDSM log...
            if (a_iLevel > 0)
            {
                Environment.SetEnvironmentVariable("TWAINDSM_LOG", Path.Combine(a_szPath, "twaindsm.log"));
                Environment.SetEnvironmentVariable("TWAINDSM_MODE", "w");
            }

            // Backup old stuff...
            szLogFile = Path.Combine(a_szPath, a_szName);
            try
            {
                if (File.Exists(szLogFile + "_backup_2.log"))
                {
                    File.Delete(szLogFile + "_backup_2.log");
                }
                if (File.Exists(szLogFile + "_backup_1.log"))
                {
                    File.Move(szLogFile + "_backup_1.log", szLogFile + "_backup_2.log");
                }
                if (File.Exists(szLogFile + ".log"))
                {
                    File.Move(szLogFile + ".log", szLogFile + "_backup_1.log");
                }
            }
            catch
            {
                // Don't care, keep going...
            }

            // Turn on the listener...
            ms_filestream = File.Open(szLogFile + ".log", FileMode.Append, FileAccess.Write, FileShare.Read);
            Trace.Listeners.Add(new TextWriterTraceListener(ms_filestream, a_szName + "Listener"));
        }

        /// <summary>
        /// Set the debugging level
        /// </summary>
        /// <param name="a_iLevel">a bitmask</param>
        public static void SetLevel(int a_iLevel)
        {
            // Squirrel this value away...
            ms_iLevel = a_iLevel;

            // One has to opt out of flushing, since the consequence
            // of turning it off often involves losing log data...
            if ((a_iLevel & c_iDebugNoFlush) == c_iDebugNoFlush)
            {
                SetFlush(false);
            }
            else
            {
                SetFlush(true);
            }
        }

        /// <summary>
        /// Flush data to the file...
        /// </summary>
        public static void SetFlush(bool a_blFlush)
        {
            ms_blFlush = a_blFlush;
            if (a_blFlush)
            {
                Trace.Flush();
            }
        }

        /// <summary>
        /// Set our state delegate...
        /// </summary>
        public static void SetStateDelegate(GetStateDelegate a_getstatedelegate)
        {
            GetState = a_getstatedelegate;
        }

        /// <summary>
        /// Write a verbose message, this is extra info that isn't normally
        /// needed to diagnose problems, but may provide insight into what
        /// the code is doing...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void Verbose(string a_szMessage)
        {
            WriteEntry("V", a_szMessage, ms_blFlush);
        }

        /// <summary>
        /// Write a verbose data message, this is extra info, specifically
        /// data transfers, that isn't normally needed to diagnose problems.
        /// Turning this one can really bloat the logs...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void VerboseData(string a_szMessage)
        {
            WriteEntry("D", a_szMessage, ms_blFlush);
        }

        /// <summary>
        /// Write an warning message...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void Warn(string a_szMessage)
        {
            WriteEntry("W", a_szMessage, ms_blFlush);
        }

        /// <summary>
        /// Do this for all of them...
        /// </summary>
        /// <param name="a_szMessage">The message</param>
        /// <param name="a_szSeverity">Message severity</param>
        /// <param name="a_blFlush">Flush it to disk</param>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static void WriteEntry(string a_szSeverity, string a_szMessage, bool a_blFlush)
        {
            long lThreadId;

            // Filter...
            switch (a_szSeverity)
            {
                // Always log these...
                case "A": break;
                case "E": break;
                case "W": break;

                // Log informationals when bit-0 is set...
                case ".":
                    if ((ms_iLevel & c_iDebugInfo) != 0)
                    {
                        break;
                    }
                    return;

                // Log verbose when bit-1 is set...
                case "V":
                    if ((ms_iLevel & c_iDebugVerbose) != 0)
                    {
                        a_szSeverity = ".";
                        break;
                    }
                    return;

                // Log verbose data when bit-2 is set...
                case "D":
                    if ((ms_iLevel & c_iDebugVerboseData) != 0)
                    {
                        a_szSeverity = ".";
                        break;
                    }
                    return;
            }

            // Get our thread id...
            if (ms_blIsWindows)
            {
                lThreadId = NativeMethods.GetCurrentThreadId();
            }
            else
            {
                lThreadId = Thread.CurrentThread.ManagedThreadId; // AppDomain.GetCurrentThreadId();
            }

            // First pass...
            if (ms_blFirstPass)
            {
                string szPlatform;

                // We're Windows...
                if (Environment.OSVersion.ToString().Contains("Microsoft Windows"))
                {
                    szPlatform = "windows";
                }

                // We're Mac OS X (this has to come before LINUX!!!)...
                else if (Directory.Exists("/Library/Application Support"))
                {
                    szPlatform = "macosx";
                }

                // We're Linux...
                else if (Environment.OSVersion.ToString().Contains("Unix"))
                {
                    szPlatform = "linux";
                }

                // We have a problem, Log will throw for us...
                else
                {
                    szPlatform = "unknown";
                }

                if (!ms_blOpened)
                {
                    // We'll assume they want logging, since they didn't tell us...
                    Open("Twain", ".", 1);
                }
                Trace.UseGlobalLock = true;
                ms_blFirstPass = false;
                Trace.WriteLine
                (
                    string.Format
                    (
                        "{0:D6} {1} T{2:D8} {3} V{4} ts:{5} os:{6}",
                        ms_iMessageNumber++,
                        DateTime.Now.ToString("HHmmssffffff"),
                        lThreadId,
                        (GetState == null) ? "S1" : GetState(),
                        a_szSeverity.ToString(),
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffffff"),
                        szPlatform
                    )
                );
            }

            // And log it...
            Trace.WriteLine
            (
                string.Format
                (
                    "{0:D6} {1} T{2:D8} {3} V{4} {5}",
                    ms_iMessageNumber++,
                    DateTime.Now.ToString("HHmmssffffff"),
                    lThreadId,
                    (GetState == null) ? "S1" : GetState(),
                    a_szSeverity.ToString(),
                    a_szMessage
                )
            );

            // Flush it...
            if (a_blFlush)
            {
                Trace.Flush();
            }
        }

        #endregion


        // Public Definitions...
        #region Public Definitions...

        /// <summary>
        /// Our severity levels...
        /// </summary>
        public enum Severity
        {
            Info,
            Warning,
            Error,
            Throw
        }

        /// <summary>
        /// We use this to get state info from other entities...
        /// </summary>
        /// <returns></returns>
        public delegate string GetStateDelegate();

        #endregion


        // Private Methods...
        #region Private Methods

        /// <summary>
        /// A place holder if we don't have a way to get state info...
        /// </summary>
        /// <returns>S0</returns>
        private static string GetStateLocal()
        {
            return ("S0");
        }

        #endregion


        // Private Definitions...
        #region Private Definitions...

        /// <summary>
        /// LogLevel bitmask...
        /// </summary>
        private const int c_iDebugInfo = 0x0001;
        private const int c_iDebugVerbose = 0x0002;
        private const int c_iDebugVerboseData = 0x0004;
        private const int c_iDebugNoFlush = 0x0008;

        #endregion


        // Private Attributes...
        #region Private Attributes

        private static bool ms_blFirstPass = true;
        private static bool ms_blOpened = false;
        private static bool ms_blFlush = false;
        private static int ms_iMessageNumber = 0;
        private static int ms_iLevel = 0;
        private static bool ms_blIsWindows = false;
        private static FileStream ms_filestream = null;

        /// <summary>
        /// We can override this with a function that will give us
        /// a state number whenever we record a line in the log...
        /// </summary>
        public static GetStateDelegate GetState = GetStateLocal;

        #endregion
    }

    /// <summary>
    /// P/Invokes, note that these are designed to be only accessible to classes in
    /// the TwainDirect.Support namespace.  So if you need this functions in other
    /// namespaces, they have to be wrapped in a function somewhere in one of the
    /// TwainDirect.Support classes that have public access (not this one)...
    /// </summary>
    internal sealed class NativeMethods
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Windows
        ///////////////////////////////////////////////////////////////////////////////
        #region Windows

        /// <summary>
        /// So we can get a console window on Windows...
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int AllocConsole();

        /// <summary>
        /// Get the desktop window so we have a parent...
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "SetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateFile
            (string lpFileName
            , [MarshalAs(UnmanagedType.U4)] DesiredAccess dwDesiredAccess
            , [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode
            , IntPtr lpSecurityAttributes
            , [MarshalAs(UnmanagedType.U4)] CreationDisposition dwCreationDisposition
            , [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes
            , IntPtr hTemplateFile
            );
        [Flags]
        public enum DesiredAccess : uint
        {
            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000
        }
        [Flags]
        public enum FileShare : uint
        {
            /// <summary>
            /// 
            /// </summary>
            None = 0x00000000,
            /// <summary>
            /// Enables subsequent open operations on an object to request read access. 
            /// Otherwise, other processes cannot open the object if they request read access. 
            /// If this flag is not specified, but the object has been opened for read access, the function fails.
            /// </summary>
            Read = 0x00000001,
            /// <summary>
            /// Enables subsequent open operations on an object to request write access. 
            /// Otherwise, other processes cannot open the object if they request write access. 
            /// If this flag is not specified, but the object has been opened for write access, the function fails.
            /// </summary>
            Write = 0x00000002,
            /// <summary>
            /// Enables subsequent open operations on an object to request delete access. 
            /// Otherwise, other processes cannot open the object if they request delete access.
            /// If this flag is not specified, but the object has been opened for delete access, the function fails.
            /// </summary>
            Delete = 0x00000004
        }
        public enum CreationDisposition : uint
        {
            /// <summary>
            /// Creates a new file. The function fails if a specified file exists.
            /// </summary>
            New = 1,
            /// <summary>
            /// Creates a new file, always. 
            /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes, 
            /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
            /// </summary>
            CreateAlways = 2,
            /// <summary>
            /// Opens a file. The function fails if the file does not exist. 
            /// </summary>
            OpenExisting = 3,
            /// <summary>
            /// Opens a file, always. 
            /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
            /// </summary>
            OpenAlways = 4,
            /// <summary>
            /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
            /// The calling process must open the file with the GENERIC_WRITE access right. 
            /// </summary>
            TruncateExisting = 5
        }
        [Flags]
        public enum FileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }

        public const int STD_INPUT_HANDLE = -10;
        public const int STD_OUTPUT_HANDLE = -11;
        public const int STD_ERROR_HANDLE = -12;
        public const int MY_CODE_PAGE = 437;

        /// <summary>
        /// Having this helps a little bit with logging on Windows, it's
        /// not a huge win, though, so it may well go away at some point...
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]

        internal static extern int GetCurrentThreadId();
        // Message sent to the Window when a Bonjour event occurs.
        public const int BONJOUR_EVENT = (0x8000 + 0x100); // WM_USER

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void free(IntPtr ptr);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr calloc(IntPtr num, IntPtr size);

        [DllImport("user32.dll")]
        public static extern int GetMessage
        (
            out MSG lpMsg,
            IntPtr hWnd,
            int wMsgFilterMin,
            int wMsgFilterMax
        );

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
        internal const int BCM_FIRST = 0x1600; //Normal button
        internal const int BCM_SETSHIELD = (BCM_FIRST + 0x000C); //Elevated button

        [DllImport("user32.dll")]
        public static extern int TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref MSG lpMsg);

        [DllImport("wsock32.dll")]
        public static extern int WSAAsyncSelect
        (
            IntPtr socket,
            IntPtr hWnd,
            int wMsg,
            int lEvent
        );

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryExW(string lpFileName, IntPtr hReservedNull, int dwFlags);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr GetProcAddress
        (
            IntPtr hModule,
            [MarshalAs(UnmanagedType.LPStr)] string procName
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int FreeLibrary(IntPtr handle);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return", Justification = "This declaration is not used on 64-bit Windows.")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2", Justification = "This declaration is not used on 64-bit Windows.")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Justification = "Entry point does exist on 64-bit Windows.")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLong")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return", Justification = "This declaration is not used on 64-bit Windows.")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2", Justification = "This declaration is not used on 64-bit Windows.")]
        public static extern int SetWindowLong(IntPtr hWnd, Int32 nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Justification = "Entry point does exist on 64-bit Windows.")]
        public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowExW
        (
           UInt32 dwExStyle,
           [MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
           [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName,
           Int32 dwStyle,
           Int32 x,
           Int32 y,
           Int32 nWidth,
           Int32 nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam
        );

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U2)]
        public static extern short RegisterClassExW([In] ref WNDCLASSEXW lpwc);

        /// <summary>
        /// The Windows Point structure.
        /// Needed for the PreFilterMessage function when we're
        /// handling DAT_EVENT...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// The Windows MSG structure.
        /// Needed for the PreFilterMessage function when we're
        /// handling DAT_EVENT...
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public Int32 message;
            public IntPtr wParam;
            public IntPtr lParam;
            public Int32 time;
            public POINT pt;
        }

        public delegate IntPtr WndProcDelegate(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable", Justification = "Not allocating any resources.")]
        public struct WNDCLASSEXW
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public int style;
            public IntPtr lpfnWndProc; // not WndProc
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public string lpszMenuName;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetModuleHandleW(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, int iMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        [DllImport("libc.so", EntryPoint = "memcpy", SetLastError = false)]
        private static extern void memcpy(IntPtr dest, IntPtr src, int count);

        /// <summary>
        /// Copy intptr to intptr...
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="count"></param>
        public static void MemCpy(IntPtr dest, IntPtr src, int count)
        {
            if (TWAIN.GetPlatform() == TWAIN.Platform.WINDOWS)
            {
                CopyMemory(dest, src, (uint)count);
            }
            else
            {
                memcpy(dest, src, count);
            }
        }

        [DllImport("kernel32.dll", EntryPoint = "MoveMemory", SetLastError = false)]
        private static extern void MoveMemory(IntPtr dest, IntPtr src, uint count);

        [DllImport("libc.so", EntryPoint = "memmove", SetLastError = false)]
        private static extern void memmove(IntPtr dest, IntPtr src, int count);

        /// <summary>
        /// Safely move intptr to intptr...
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="count"></param>
        public static void MemMove(IntPtr dest, IntPtr src, int count)
        {
            if (TWAIN.GetPlatform() == TWAIN.Platform.WINDOWS)
            {
                MoveMemory(dest, src, (uint)count);
            }
            else
            {
                memmove(dest, src, count);
            }
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Int32 _wfopen_s(out IntPtr pFile, string filename, string mode);

        [DllImport("libc.so", CharSet = CharSet.Ansi, SetLastError = true, BestFitMapping=false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr fopen([MarshalAs(UnmanagedType.LPStr)] string filename, [MarshalAs(UnmanagedType.LPStr)] string mode);

        [DllImport("msvcrt.dll", EntryPoint = "fwrite", SetLastError = true)]
        private static extern IntPtr fwriteWin(IntPtr buffer, IntPtr size, IntPtr number, IntPtr file);

        [DllImport("libc.so", EntryPoint = "fwrite", SetLastError = true)]
        private static extern IntPtr fwrite(IntPtr buffer, IntPtr size, IntPtr number, IntPtr file);

        [DllImport("msvcrt.dll", EntryPoint = "fclose", SetLastError = true)]
        private static extern IntPtr fcloseWin(IntPtr file);

        [DllImport("libc.so", EntryPoint = "fclose", SetLastError = true)]
        private static extern IntPtr fclose(IntPtr file);

        /// <summary>
        /// Write stuff to a file without having to rebuffer it...
        /// </summary>
        /// <param name="a_szFilename"></param>
        /// <param name="a_intptrPtr"></param>
        /// <param name="a_iBytes"></param>
        /// <returns></returns>
        public static int WriteImageFile(string a_szFilename, IntPtr a_intptrPtr, int a_iBytes)
        {
            try
            {
                // If we don't have an extension, try to add one...
                if (!Path.GetFileName(a_szFilename).Contains(".") && (a_iBytes >= 2))
                {
                    byte[] abData = new byte[2];
                    Marshal.Copy(a_intptrPtr, abData, 0, 2);
                    // BMP
                    if ((abData[0] == 0x42) && (abData[1] == 0x4D))
                    {
                        a_szFilename += ".bmp";
                    }
                    else if ((abData[0] == 0x49) && (abData[1] == 0x49))
                    {
                        a_szFilename += ".tif";
                    }
                    else if ((abData[0] == 0xFF) && (abData[1] == 0xD8))
                    {
                        a_szFilename += ".jpg";
                    }
                }

                // Handle Windows...
                if (TWAIN.GetPlatform() == TWAIN.Platform.WINDOWS)
                {
                    IntPtr intptrFile;
                    IntPtr intptrBytes;
                    if (_wfopen_s(out intptrFile, a_szFilename, "wb") != 0)
                    {
                        return (-1);
                    }
                    intptrBytes = fwriteWin(a_intptrPtr, (IntPtr)a_iBytes, (IntPtr)1, intptrFile);
                    fcloseWin(intptrFile);
                    return ((int)intptrBytes);
                }

                // Handle everybody else...
                else
                {
                    IntPtr intptrFile;
                    IntPtr intptrBytes;
                    intptrFile = fopen(a_szFilename, "w");
                    if (intptrFile == IntPtr.Zero)
                    {
                        return (-1);
                    }
                    intptrBytes = fwrite(a_intptrPtr, (IntPtr)a_iBytes, (IntPtr)1, intptrFile);
                    fclose(intptrFile);
                    return ((int)intptrBytes);
                }
            }
            catch (Exception exception)
            {
                Log.Error("Write file failed <" + a_szFilename + "> - " + exception.Message);
                return (-1);
            }
        }

        #endregion
    }
}
