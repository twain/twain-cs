///////////////////////////////////////////////////////////////////////////////////////
//
//  TwainWorkingGroup.TWAIN
//
//  These are the definitions for TWAIN.  They're essentially the C/C++
//  TWAIN.H file contents translated to C#, with modifications that
//  recognize the differences between Windows, Linux and Mac OS X.
//
///////////////////////////////////////////////////////////////////////////////////////
//  Author          Date            TWAIN       Comment
//  M.McLaughlin    13-Mar-2019     2.4.0.3     Add language code page support for strings
//  M.McLaughlin    13-Nov-2015     2.4.0.0     Updated to latest spec
//  M.McLaughlin    13-Sep-2015     2.3.1.2     DsmMem bug fixes
//  M.McLaughlin    26-Aug-2015     2.3.1.1     Log fix and sync with TWAIN Direct
//  M.McLaughlin    13-Mar-2015     2.3.1.0     Numerous fixes
//  M.McLaughlin    13-Oct-2014     2.3.0.4     Added logging
//  M.McLaughlin    24-Jun-2014     2.3.0.3     Stability fixes
//  M.McLaughlin    21-May-2014     2.3.0.2     64-Bit Linux
//  M.McLaughlin    27-Feb-2014     2.3.0.1     AnyCPU support
//  M.McLaughlin    21-Oct-2013     2.3.0.0     Initial Release
///////////////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2013-2020 Kodak Alaris Inc.
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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TWAINWorkingGroup
{
    /// <summary>
    /// This file contains content gleaned from version 2.4 of the C/C++ TWAIN.H
    /// header file released by the TWAIN Working Group.  It's organized like that
    /// file to make it easier to maintain.
    /// 
    /// Please do not add any code to this module, save for the minimum needed to
    /// maintain a particular definition (such as TW_STR32)...
    /// </summary>
    public partial class TWAIN
    {
        ///////////////////////////////////////////////////////////////////////////////
        // TWAIN Version...
        ///////////////////////////////////////////////////////////////////////////////
        #region Protocol Version...
        public enum TWON_PROTOCOL
        {
            MAJOR = 2,
            MINOR = 4 // Changed for Version 2.4
        };
        #endregion

        ///////////////////////////////////////////////////////////////////////////////
        // Type Definitions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Type Definitions...

        // Follow these rules
        /******************************************************************************

        TW_HANDLE...............IntPtr
        TW_MEMREF...............IntPtr
        TW_UINTPTR..............UIntPtr
        
        TW_INT8.................char
        TW_INT16................short
        TW_INT32................int (was long on Linux 64-bit)
        
        TW_UINT8................byte
        TW_UINT16...............ushort
        TW_UINT32...............uint (was ulong on Linux 64-bit)
        TW_BOOL.................ushort
        
        ******************************************************************************/

        /// <summary>
        /// Our supported platforms...
        /// </summary>
        public enum Platform
        {
            UNKNOWN,
            WINDOWS,
            LINUX,
            MACOSX
        };

        /// <summary>
        /// Our supported processors...
        /// </summary>
        public enum Processor
        {
            UNKNOWN,
            X86,
            X86_64,
            MIPS64EL
        };

        /// <summary>
        /// Used for strings that go up to 32-bytes...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct TW_STR32
        {
            /// <summary>
            /// We're stuck with this, because marshalling with packed alignment
            /// can't handle arrays...
            /// </summary>
            private byte byItem000; private byte byItem001; private byte byItem002; private byte byItem003;
            private byte byItem004; private byte byItem005; private byte byItem006; private byte byItem007;
            private byte byItem008; private byte byItem009; private byte byItem010; private byte byItem011;
            private byte byItem012; private byte byItem013; private byte byItem014; private byte byItem015;
            private byte byItem016; private byte byItem017; private byte byItem018; private byte byItem019;
            private byte byItem020; private byte byItem021; private byte byItem022; private byte byItem023;
            private byte byItem024; private byte byItem025; private byte byItem026; private byte byItem027;
            private byte byItem028; private byte byItem029; private byte byItem030; private byte byItem031;
            private byte byItem032; private byte byItem033;

            /// <summary>
            /// The normal get...
            /// </summary>
            /// <returns></returns>
            public string Get()
            {
                return (GetValue(true));
            }

            /// <summary>
            /// Use this on Mac OS X if you have a call that uses a string
            /// that doesn't include the prefix byte...
            /// </summary>
            /// <returns></returns>
            public string GetNoPrefix()
            {
                return (GetValue(false));
            }

            /// <summary>
            /// Get our value...
            /// </summary>
            /// <returns></returns>
            private string GetValue(bool a_blMayHavePrefix)
            {
                // convert what we have into a byte array
                byte[] abyItem = new byte[34];
                abyItem[0]  = byItem000; abyItem[1]  = byItem001; abyItem[2]  = byItem002; abyItem[3]  = byItem003;
                abyItem[4]  = byItem004; abyItem[5]  = byItem005; abyItem[6]  = byItem006; abyItem[7]  = byItem007;
                abyItem[8]  = byItem008; abyItem[9]  = byItem009; abyItem[10] = byItem010; abyItem[11] = byItem011;
                abyItem[12] = byItem012; abyItem[13] = byItem013; abyItem[14] = byItem014; abyItem[15] = byItem015;
                abyItem[16] = byItem016; abyItem[17] = byItem017; abyItem[18] = byItem018; abyItem[19] = byItem019;
                abyItem[20] = byItem020; abyItem[21] = byItem021; abyItem[22] = byItem022; abyItem[23] = byItem023;
                abyItem[24] = byItem024; abyItem[25] = byItem025; abyItem[26] = byItem026; abyItem[27] = byItem027;
                abyItem[28] = byItem028; abyItem[29] = byItem029; abyItem[30] = byItem030; abyItem[31] = byItem031;
                abyItem[32] = byItem032; abyItem[33] = byItem033;

                // Zero anything after the NUL...
                bool blNul = false;
                for (int ii = 0; ii < abyItem.Length; ii++)
                {
                    if (!blNul && (abyItem[ii] == 0))
                    {
                        blNul = true;
                    }
                    else if (blNul)
                    {
                        abyItem[ii] = 0;
                    }
                }

                // change encoding of byte array, then convert the bytes array to a string
                string sz = Encoding.Unicode.GetString(Encoding.Convert(Language.GetEncoding(), Encoding.Unicode, abyItem));

                // If the first character is a NUL, then return the empty string...
                while ((sz.Length > 0) && (sz[0] == '\0'))
                {
                    sz = sz.Remove(0, 1);
                }

                // We have an emptry string...
                if (sz.Length == 0)
                {
                    return ("");
                }

                // If we're running on a Mac, take off the prefix 'byte'...
                if (a_blMayHavePrefix && (TWAIN.GetPlatform() == Platform.MACOSX))
                {
                    sz = sz.Remove(0, 1);
                }

                // If we detect a NUL, then split around it...
                if (sz.IndexOf('\0') >= 0)
                {
                    sz = sz.Split(new char[] { '\0' })[0];
                }

                // All done...
                return (sz);
            }

            /// <summary>
            /// The normal set...
            /// </summary>
            /// <returns></returns>
            public void Set(string a_sz)
            {
                SetValue(a_sz, true);
            }

            /// <summary>
            /// Use this on Mac OS X if you have a call that uses a string
            /// that doesn't include the prefix byte...
            /// </summary>
            /// <returns></returns>
            public void SetNoPrefix(string a_sz)
            {
                SetValue(a_sz, false);
            }

            /// <summary>
            /// Set our value...
            /// </summary>
            /// <param name="a_sz"></param>
            private void SetValue(string a_sz, bool a_blMayHavePrefix)
            {
                // If we're running on a Mac, tack on the prefix 'byte'...
                if (a_sz == null)
                {
                    a_sz = "";
                }
                else if (a_blMayHavePrefix && (TWAIN.GetPlatform() == Platform.MACOSX))
                {
                    a_sz = (char)a_sz.Length + a_sz;
                }

                // Make sure that we're NUL padded...
                string sz = a_sz +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0";
                if (sz.Length > 34)
                {
                    sz = sz.Remove(34);
                }

                // convert string to byte array, then change the encoding of the byte array
                byte[] abyItem = Encoding.Convert(Encoding.Unicode, Language.GetEncoding(), Encoding.Unicode.GetBytes(sz));

                // convert byte array to bytes
                if (abyItem.Length > 0)
                {
                    byItem000 = abyItem[0];  byItem001 = abyItem[1];  byItem002 = abyItem[2];  byItem003 = abyItem[3];
                    byItem004 = abyItem[4];  byItem005 = abyItem[5];  byItem006 = abyItem[6];  byItem007 = abyItem[7];
                    byItem008 = abyItem[8];  byItem009 = abyItem[9];  byItem010 = abyItem[10]; byItem011 = abyItem[11];
                    byItem012 = abyItem[12]; byItem013 = abyItem[13]; byItem014 = abyItem[14]; byItem015 = abyItem[15];
                    byItem016 = abyItem[16]; byItem017 = abyItem[17]; byItem018 = abyItem[18]; byItem019 = abyItem[19];
                    byItem020 = abyItem[20]; byItem021 = abyItem[21]; byItem022 = abyItem[22]; byItem023 = abyItem[23];
                    byItem024 = abyItem[24]; byItem025 = abyItem[25]; byItem026 = abyItem[26]; byItem027 = abyItem[27];
                    byItem028 = abyItem[28]; byItem029 = abyItem[29]; byItem030 = abyItem[30]; byItem031 = abyItem[31];
                    byItem032 = abyItem[32]; byItem033 = abyItem[33];
                }
            }
        }

        /// <summary>
        /// Used for strings that go up to 64-bytes...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct TW_STR64
        {
            /// <summary>
            /// We're stuck with this, because marshalling with packed alignment
            /// can't handle arrays...
            /// </summary>
            private byte byItem000; private byte byItem001; private byte byItem002; private byte byItem003;
            private byte byItem004; private byte byItem005; private byte byItem006; private byte byItem007;
            private byte byItem008; private byte byItem009; private byte byItem010; private byte byItem011;
            private byte byItem012; private byte byItem013; private byte byItem014; private byte byItem015;
            private byte byItem016; private byte byItem017; private byte byItem018; private byte byItem019;
            private byte byItem020; private byte byItem021; private byte byItem022; private byte byItem023;
            private byte byItem024; private byte byItem025; private byte byItem026; private byte byItem027;
            private byte byItem028; private byte byItem029; private byte byItem030; private byte byItem031;
            private byte byItem032; private byte byItem033; private byte byItem034; private byte byItem035;
            private byte byItem036; private byte byItem037; private byte byItem038; private byte byItem039;
            private byte byItem040; private byte byItem041; private byte byItem042; private byte byItem043;
            private byte byItem044; private byte byItem045; private byte byItem046; private byte byItem047;
            private byte byItem048; private byte byItem049; private byte byItem050; private byte byItem051;
            private byte byItem052; private byte byItem053; private byte byItem054; private byte byItem055;
            private byte byItem056; private byte byItem057; private byte byItem058; private byte byItem059;
            private byte byItem060; private byte byItem061; private byte byItem062; private byte byItem063;
            private byte byItem064; private byte byItem065;

            /// <summary>
            /// The normal get...
            /// </summary>
            /// <returns></returns>
            public string Get()
            {
                return (GetValue(true));
            }

            /// <summary>
            /// Use this on Mac OS X if you have a call that uses a string
            /// that doesn't include the prefix byte...
            /// </summary>
            /// <returns></returns>
            public string GetNoPrefix()
            {
                return (GetValue(false));
            }

            /// <summary>
            /// Get our value...
            /// </summary>
            /// <returns></returns>
            private string GetValue(bool a_blMayHavePrefix)
            {
                // convert what we have into a byte array
                byte[] abyItem = new byte[66];
                abyItem[0]  = byItem000; abyItem[1]  = byItem001; abyItem[2]  = byItem002; abyItem[3]  = byItem003;
                abyItem[4]  = byItem004; abyItem[5]  = byItem005; abyItem[6]  = byItem006; abyItem[7]  = byItem007;
                abyItem[8]  = byItem008; abyItem[9]  = byItem009; abyItem[10] = byItem010; abyItem[11] = byItem011;
                abyItem[12] = byItem012; abyItem[13] = byItem013; abyItem[14] = byItem014; abyItem[15] = byItem015;
                abyItem[16] = byItem016; abyItem[17] = byItem017; abyItem[18] = byItem018; abyItem[19] = byItem019;
                abyItem[20] = byItem020; abyItem[21] = byItem021; abyItem[22] = byItem022; abyItem[23] = byItem023;
                abyItem[24] = byItem024; abyItem[25] = byItem025; abyItem[26] = byItem026; abyItem[27] = byItem027;
                abyItem[28] = byItem028; abyItem[29] = byItem029; abyItem[30] = byItem030; abyItem[31] = byItem031;
                abyItem[32] = byItem032; abyItem[33] = byItem033; abyItem[34] = byItem034; abyItem[35] = byItem035;
                abyItem[36] = byItem036; abyItem[37] = byItem037; abyItem[38] = byItem038; abyItem[39] = byItem039;
                abyItem[40] = byItem040; abyItem[41] = byItem041; abyItem[42] = byItem042; abyItem[43] = byItem043;
                abyItem[44] = byItem044; abyItem[45] = byItem045; abyItem[46] = byItem046; abyItem[47] = byItem047;
                abyItem[48] = byItem048; abyItem[49] = byItem049; abyItem[50] = byItem050; abyItem[51] = byItem051;
                abyItem[52] = byItem052; abyItem[53] = byItem053; abyItem[54] = byItem054; abyItem[55] = byItem055;
                abyItem[56] = byItem056; abyItem[57] = byItem057; abyItem[58] = byItem058; abyItem[59] = byItem059;
                abyItem[60] = byItem060; abyItem[61] = byItem061; abyItem[62] = byItem062; abyItem[63] = byItem063;
                abyItem[64] = byItem064; abyItem[65] = byItem065;

                // Zero anything after the NUL...
                bool blNul = false;
                for (int ii = 0; ii < abyItem.Length; ii++)
                {
                    if (!blNul && (abyItem[ii] == 0))
                    {
                        blNul = true;
                    }
                    else if (blNul)
                    {
                        abyItem[ii] = 0;
                    }
                }

                // change encoding of byte array, then convert the bytes array to a string
                string sz = Encoding.Unicode.GetString(Encoding.Convert(Language.GetEncoding(), Encoding.Unicode, abyItem));

                // If the first character is a NUL, then return the empty string...
                if (sz[0] == '\0')
                {
                    return ("");
                }

                // If we're running on a Mac, take off the prefix 'byte'...
                if (a_blMayHavePrefix && (TWAIN.GetPlatform() == Platform.MACOSX))
                {
                    sz = sz.Remove(0, 1);
                }

                // If we detect a NUL, then split around it...
                if (sz.IndexOf('\0') >= 0)
                {
                    sz = sz.Split(new char[] { '\0' })[0];
                }

                // All done...
                return (sz);
            }

            /// <summary>
            /// The normal set...
            /// </summary>
            /// <returns></returns>
            public void Set(string a_sz)
            {
                SetValue(a_sz, true);
            }

            /// <summary>
            /// Use this on Mac OS X if you have a call that uses a string
            /// that doesn't include the prefix byte...
            /// </summary>
            /// <returns></returns>
            public void SetNoPrefix(string a_sz)
            {
                SetValue(a_sz, false);
            }

            /// <summary>
            /// Set our value...
            /// </summary>
            /// <param name="a_sz"></param>
            private void SetValue(string a_sz, bool a_blMayHavePrefix)
            {
                // If we're running on a Mac, tack on the prefix 'byte'...
                if (a_sz == null)
                {
                    a_sz = "";
                }
                else if (a_blMayHavePrefix && (TWAIN.GetPlatform() == Platform.MACOSX))
                {
                    a_sz = (char)a_sz.Length + a_sz;
                }

                // Make sure that we're NUL padded...
                string sz =
                    a_sz +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0";
                if (sz.Length > 66)
                {
                    sz = sz.Remove(66);
                }

                // convert string to byte array, then change the encoding of the byte array
                byte[] abyItem = Encoding.Convert(Encoding.Unicode, Language.GetEncoding(), Encoding.Unicode.GetBytes(sz));

                // concert byte array to bytes
                byItem000 = abyItem[0];  byItem001 = abyItem[1];  byItem002 = abyItem[2];  byItem003 = abyItem[3];
                byItem004 = abyItem[4];  byItem005 = abyItem[5];  byItem006 = abyItem[6];  byItem007 = abyItem[7];
                byItem008 = abyItem[8];  byItem009 = abyItem[9];  byItem010 = abyItem[10]; byItem011 = abyItem[11];
                byItem012 = abyItem[12]; byItem013 = abyItem[13]; byItem014 = abyItem[14]; byItem015 = abyItem[15];
                byItem016 = abyItem[16]; byItem017 = abyItem[17]; byItem018 = abyItem[18]; byItem019 = abyItem[19];
                byItem020 = abyItem[20]; byItem021 = abyItem[21]; byItem022 = abyItem[22]; byItem023 = abyItem[23];
                byItem024 = abyItem[24]; byItem025 = abyItem[25]; byItem026 = abyItem[26]; byItem027 = abyItem[27];
                byItem028 = abyItem[28]; byItem029 = abyItem[29]; byItem030 = abyItem[30]; byItem031 = abyItem[31];
                byItem032 = abyItem[32]; byItem033 = abyItem[33]; byItem034 = abyItem[34]; byItem035 = abyItem[35];
                byItem036 = abyItem[36]; byItem037 = abyItem[37]; byItem038 = abyItem[38]; byItem039 = abyItem[39];
                byItem040 = abyItem[40]; byItem041 = abyItem[41]; byItem042 = abyItem[42]; byItem043 = abyItem[43];
                byItem044 = abyItem[44]; byItem045 = abyItem[45]; byItem046 = abyItem[46]; byItem047 = abyItem[47];
                byItem048 = abyItem[48]; byItem049 = abyItem[49]; byItem050 = abyItem[50]; byItem051 = abyItem[51];
                byItem052 = abyItem[52]; byItem053 = abyItem[53]; byItem054 = abyItem[54]; byItem055 = abyItem[55];
                byItem056 = abyItem[56]; byItem057 = abyItem[57]; byItem058 = abyItem[58]; byItem059 = abyItem[59];
                byItem060 = abyItem[60]; byItem061 = abyItem[61]; byItem062 = abyItem[62]; byItem063 = abyItem[63];
                byItem064 = abyItem[64]; byItem065 = abyItem[65];
            }
        }

        /// <summary>
        /// Used for strings that go up to 128-bytes...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct TW_STR128
        {
            /// <summary>
            /// We're stuck with this, because marshalling with packed alignment
            /// can't handle arrays...
            /// </summary>
            private byte byItem000; private byte byItem001; private byte byItem002; private byte byItem003;
            private byte byItem004; private byte byItem005; private byte byItem006; private byte byItem007;
            private byte byItem008; private byte byItem009; private byte byItem010; private byte byItem011;
            private byte byItem012; private byte byItem013; private byte byItem014; private byte byItem015;
            private byte byItem016; private byte byItem017; private byte byItem018; private byte byItem019;
            private byte byItem020; private byte byItem021; private byte byItem022; private byte byItem023;
            private byte byItem024; private byte byItem025; private byte byItem026; private byte byItem027;
            private byte byItem028; private byte byItem029; private byte byItem030; private byte byItem031;
            private byte byItem032; private byte byItem033; private byte byItem034; private byte byItem035;
            private byte byItem036; private byte byItem037; private byte byItem038; private byte byItem039;
            private byte byItem040; private byte byItem041; private byte byItem042; private byte byItem043;
            private byte byItem044; private byte byItem045; private byte byItem046; private byte byItem047;
            private byte byItem048; private byte byItem049; private byte byItem050; private byte byItem051;
            private byte byItem052; private byte byItem053; private byte byItem054; private byte byItem055;
            private byte byItem056; private byte byItem057; private byte byItem058; private byte byItem059;
            private byte byItem060; private byte byItem061; private byte byItem062; private byte byItem063;
            private byte byItem064; private byte byItem065; private byte byItem066; private byte byItem067;
            private byte byItem068; private byte byItem069; private byte byItem070; private byte byItem071;
            private byte byItem072; private byte byItem073; private byte byItem074; private byte byItem075;
            private byte byItem076; private byte byItem077; private byte byItem078; private byte byItem079;
            private byte byItem080; private byte byItem081; private byte byItem082; private byte byItem083;
            private byte byItem084; private byte byItem085; private byte byItem086; private byte byItem087;
            private byte byItem088; private byte byItem089; private byte byItem090; private byte byItem091;
            private byte byItem092; private byte byItem093; private byte byItem094; private byte byItem095;
            private byte byItem096; private byte byItem097; private byte byItem098; private byte byItem099;
            private byte byItem100; private byte byItem101; private byte byItem102; private byte byItem103;
            private byte byItem104; private byte byItem105; private byte byItem106; private byte byItem107;
            private byte byItem108; private byte byItem109; private byte byItem110; private byte byItem111;
            private byte byItem112; private byte byItem113; private byte byItem114; private byte byItem115;
            private byte byItem116; private byte byItem117; private byte byItem118; private byte byItem119;
            private byte byItem120; private byte byItem121; private byte byItem122; private byte byItem123;
            private byte byItem124; private byte byItem125; private byte byItem126; private byte byItem127;
            private byte byItem128; private byte byItem129;

            /// <summary>
            /// The normal get...
            /// </summary>
            /// <returns></returns>
            public string Get()
            {
                return (GetValue(true));
            }

            /// <summary>
            /// Use this on Mac OS X if you have a call that uses a string
            /// that doesn't include the prefix byte...
            /// </summary>
            /// <returns></returns>
            public string GetNoPrefix()
            {
                return (GetValue(false));
            }

            /// <summary>
            /// Get our value...
            /// </summary>
            /// <returns></returns>
            private string GetValue(bool a_blMayHavePrefix)
            {
                // convert what we have into a byte array
                byte[] abyItem = new byte[130];
                abyItem[0]  = byItem000; abyItem[1]  = byItem001; abyItem[2]  = byItem002; abyItem[3]  = byItem003;
                abyItem[4]  = byItem004; abyItem[5]  = byItem005; abyItem[6]  = byItem006; abyItem[7]  = byItem007;
                abyItem[8]  = byItem008; abyItem[9]  = byItem009; abyItem[10] = byItem010; abyItem[11] = byItem011;
                abyItem[12] = byItem012; abyItem[13] = byItem013; abyItem[14] = byItem014; abyItem[15] = byItem015;
                abyItem[16] = byItem016; abyItem[17] = byItem017; abyItem[18] = byItem018; abyItem[19] = byItem019;
                abyItem[20] = byItem020; abyItem[21] = byItem021; abyItem[22] = byItem022; abyItem[23] = byItem023;
                abyItem[24] = byItem024; abyItem[25] = byItem025; abyItem[26] = byItem026; abyItem[27] = byItem027;
                abyItem[28] = byItem028; abyItem[29] = byItem029; abyItem[30] = byItem030; abyItem[31] = byItem031;
                abyItem[32] = byItem032; abyItem[33] = byItem033; abyItem[34] = byItem034; abyItem[35] = byItem035;
                abyItem[36] = byItem036; abyItem[37] = byItem037; abyItem[38] = byItem038; abyItem[39] = byItem039;
                abyItem[40] = byItem040; abyItem[41] = byItem041; abyItem[42] = byItem042; abyItem[43] = byItem043;
                abyItem[44] = byItem044; abyItem[45] = byItem045; abyItem[46] = byItem046; abyItem[47] = byItem047;
                abyItem[48] = byItem048; abyItem[49] = byItem049; abyItem[50] = byItem050; abyItem[51] = byItem051;
                abyItem[52] = byItem052; abyItem[53] = byItem053; abyItem[54] = byItem054; abyItem[55] = byItem055;
                abyItem[56] = byItem056; abyItem[57] = byItem057; abyItem[58] = byItem058; abyItem[59] = byItem059;
                abyItem[60] = byItem060; abyItem[61] = byItem061; abyItem[62] = byItem062; abyItem[63] = byItem063;
                abyItem[64] = byItem064; abyItem[65] = byItem065; abyItem[66] = byItem066; abyItem[67] = byItem067;
                abyItem[68] = byItem068; abyItem[69] = byItem069; abyItem[70] = byItem070; abyItem[71] = byItem071;
                abyItem[72] = byItem072; abyItem[73] = byItem073; abyItem[74] = byItem074; abyItem[75] = byItem075;
                abyItem[76] = byItem076; abyItem[77] = byItem077; abyItem[78] = byItem078; abyItem[79] = byItem079;
                abyItem[80] = byItem080; abyItem[81] = byItem081; abyItem[82] = byItem082; abyItem[83] = byItem083;
                abyItem[84] = byItem084; abyItem[85] = byItem085; abyItem[86] = byItem086; abyItem[87] = byItem087;
                abyItem[88] = byItem088; abyItem[89] = byItem089; abyItem[90] = byItem090; abyItem[91] = byItem091;
                abyItem[92] = byItem092; abyItem[93] = byItem093; abyItem[94] = byItem094; abyItem[95] = byItem095;
                abyItem[96] = byItem096; abyItem[97] = byItem097; abyItem[98] = byItem098; abyItem[99] = byItem099;
                abyItem[100] = byItem100; abyItem[101] = byItem101; abyItem[102] = byItem102; abyItem[103] = byItem103;
                abyItem[104] = byItem104; abyItem[105] = byItem105; abyItem[106] = byItem106; abyItem[107] = byItem107;
                abyItem[108] = byItem108; abyItem[109] = byItem109; abyItem[110] = byItem110; abyItem[111] = byItem111;
                abyItem[112] = byItem112; abyItem[113] = byItem113; abyItem[114] = byItem114; abyItem[115] = byItem115;
                abyItem[116] = byItem116; abyItem[117] = byItem117; abyItem[118] = byItem118; abyItem[119] = byItem119;
                abyItem[120] = byItem120; abyItem[121] = byItem121; abyItem[122] = byItem122; abyItem[123] = byItem123;
                abyItem[124] = byItem124; abyItem[125] = byItem125; abyItem[126] = byItem126; abyItem[127] = byItem127;
                abyItem[128] = byItem128; abyItem[129] = byItem129;

                // Zero anything after the NUL...
                bool blNul = false;
                for (int ii = 0; ii < abyItem.Length; ii++)
                {
                    if (!blNul && (abyItem[ii] == 0))
                    {
                        blNul = true;
                    }
                    else if (blNul)
                    {
                        abyItem[ii] = 0;
                    }
                }

                // change encoding of byte array, then convert the bytes array to a string
                string sz = Encoding.Unicode.GetString(Encoding.Convert(Language.GetEncoding(), Encoding.Unicode, abyItem));

                // If the first character is a NUL, then return the empty string...
                if (sz[0] == '\0')
                {
                    return ("");
                }

                // If we're running on a Mac, take off the prefix 'byte'...
                if (a_blMayHavePrefix && (TWAIN.GetPlatform() == Platform.MACOSX))
                {
                    sz = sz.Remove(0, 1);
                }

                // If we detect a NUL, then split around it...
                if (sz.IndexOf('\0') >= 0)
                {
                    sz = sz.Split(new char[] { '\0' })[0];
                }

                // All done...
                return (sz);
            }

            /// <summary>
            /// The normal set...
            /// </summary>
            /// <returns></returns>
            public void Set(string a_sz)
            {
                SetValue(a_sz, true);
            }

            /// <summary>
            /// Use this on Mac OS X if you have a call that uses a string
            /// that doesn't include the prefix byte...
            /// </summary>
            /// <returns></returns>
            public void SetNoPrefix(string a_sz)
            {
                SetValue(a_sz, false);
            }

            /// <summary>
            /// Set our value...
            /// </summary>
            /// <param name="a_sz"></param>
            private void SetValue(string a_sz, bool a_blMayHavePrefix)
            {
                // If we're running on a Mac, tack on the prefix 'byte'...
                if (a_sz == null)
                {
                    a_sz = "";
                }
                else if (a_blMayHavePrefix && (TWAIN.GetPlatform() == Platform.MACOSX))
                {
                    a_sz = (char)a_sz.Length + a_sz;
                }

                // Make sure that we're NUL padded...
                string sz =
                    a_sz +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0";
                if (sz.Length > 130)
                {
                    sz = sz.Remove(130);
                }

                // convert string to byte array, then change the encoding of the byte array
                byte[] abyItem = Encoding.Convert(Encoding.Unicode, Language.GetEncoding(), Encoding.Unicode.GetBytes(sz));

                // concert byte array to bytes
                byItem000 = abyItem[0];  byItem001 = abyItem[1];  byItem002 = abyItem[2];  byItem003 = abyItem[3];
                byItem004 = abyItem[4];  byItem005 = abyItem[5];  byItem006 = abyItem[6];  byItem007 = abyItem[7];
                byItem008 = abyItem[8];  byItem009 = abyItem[9];  byItem010 = abyItem[10]; byItem011 = abyItem[11];
                byItem012 = abyItem[12]; byItem013 = abyItem[13]; byItem014 = abyItem[14]; byItem015 = abyItem[15];
                byItem016 = abyItem[16]; byItem017 = abyItem[17]; byItem018 = abyItem[18]; byItem019 = abyItem[19];
                byItem020 = abyItem[20]; byItem021 = abyItem[21]; byItem022 = abyItem[22]; byItem023 = abyItem[23];
                byItem024 = abyItem[24]; byItem025 = abyItem[25]; byItem026 = abyItem[26]; byItem027 = abyItem[27];
                byItem028 = abyItem[28]; byItem029 = abyItem[29]; byItem030 = abyItem[30]; byItem031 = abyItem[31];
                byItem032 = abyItem[32]; byItem033 = abyItem[33]; byItem034 = abyItem[34]; byItem035 = abyItem[35];
                byItem036 = abyItem[36]; byItem037 = abyItem[37]; byItem038 = abyItem[38]; byItem039 = abyItem[39];
                byItem040 = abyItem[40]; byItem041 = abyItem[41]; byItem042 = abyItem[42]; byItem043 = abyItem[43];
                byItem044 = abyItem[44]; byItem045 = abyItem[45]; byItem046 = abyItem[46]; byItem047 = abyItem[47];
                byItem048 = abyItem[48]; byItem049 = abyItem[49]; byItem050 = abyItem[50]; byItem051 = abyItem[51];
                byItem052 = abyItem[52]; byItem053 = abyItem[53]; byItem054 = abyItem[54]; byItem055 = abyItem[55];
                byItem056 = abyItem[56]; byItem057 = abyItem[57]; byItem058 = abyItem[58]; byItem059 = abyItem[59];
                byItem060 = abyItem[60]; byItem061 = abyItem[61]; byItem062 = abyItem[62]; byItem063 = abyItem[63];
                byItem064 = abyItem[64]; byItem065 = abyItem[65]; byItem066 = abyItem[66]; byItem067 = abyItem[67];
                byItem068 = abyItem[68]; byItem069 = abyItem[69]; byItem070 = abyItem[70]; byItem071 = abyItem[71];
                byItem072 = abyItem[72]; byItem073 = abyItem[73]; byItem074 = abyItem[74]; byItem075 = abyItem[75];
                byItem076 = abyItem[76]; byItem077 = abyItem[77]; byItem078 = abyItem[78]; byItem079 = abyItem[79];
                byItem080 = abyItem[80]; byItem081 = abyItem[81]; byItem082 = abyItem[82]; byItem083 = abyItem[83];
                byItem084 = abyItem[84]; byItem085 = abyItem[85]; byItem086 = abyItem[86]; byItem087 = abyItem[87];
                byItem088 = abyItem[88]; byItem089 = abyItem[89]; byItem090 = abyItem[90]; byItem091 = abyItem[91];
                byItem092 = abyItem[92]; byItem093 = abyItem[93]; byItem094 = abyItem[94]; byItem095 = abyItem[95];
                byItem096 = abyItem[96]; byItem097 = abyItem[97]; byItem098 = abyItem[98]; byItem099 = abyItem[99];
                byItem100 = abyItem[100]; byItem101 = abyItem[101]; byItem102 = abyItem[102]; byItem103 = abyItem[103];
                byItem104 = abyItem[104]; byItem105 = abyItem[105]; byItem106 = abyItem[106]; byItem107 = abyItem[107];
                byItem108 = abyItem[108]; byItem109 = abyItem[109]; byItem110 = abyItem[110]; byItem111 = abyItem[111];
                byItem112 = abyItem[112]; byItem113 = abyItem[113]; byItem114 = abyItem[114]; byItem115 = abyItem[115];
                byItem116 = abyItem[116]; byItem117 = abyItem[117]; byItem118 = abyItem[118]; byItem119 = abyItem[119];
                byItem120 = abyItem[120]; byItem121 = abyItem[121]; byItem122 = abyItem[122]; byItem123 = abyItem[123];
                byItem124 = abyItem[124]; byItem125 = abyItem[125]; byItem126 = abyItem[126]; byItem127 = abyItem[127];
                byItem128 = abyItem[128]; byItem129 = abyItem[129];
            }
        }

        /// <summary>
        /// Used for strings that go up to 256-bytes...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct TW_STR255
        {
            /// <summary>
            /// We're stuck with this, because marshalling with packed alignment
            /// can't handle arrays...
            /// </summary>
            private byte byItem000; private byte byItem001; private byte byItem002; private byte byItem003;
            private byte byItem004; private byte byItem005; private byte byItem006; private byte byItem007;
            private byte byItem008; private byte byItem009; private byte byItem010; private byte byItem011;
            private byte byItem012; private byte byItem013; private byte byItem014; private byte byItem015;
            private byte byItem016; private byte byItem017; private byte byItem018; private byte byItem019;
            private byte byItem020; private byte byItem021; private byte byItem022; private byte byItem023;
            private byte byItem024; private byte byItem025; private byte byItem026; private byte byItem027;
            private byte byItem028; private byte byItem029; private byte byItem030; private byte byItem031;
            private byte byItem032; private byte byItem033; private byte byItem034; private byte byItem035;
            private byte byItem036; private byte byItem037; private byte byItem038; private byte byItem039;
            private byte byItem040; private byte byItem041; private byte byItem042; private byte byItem043;
            private byte byItem044; private byte byItem045; private byte byItem046; private byte byItem047;
            private byte byItem048; private byte byItem049; private byte byItem050; private byte byItem051;
            private byte byItem052; private byte byItem053; private byte byItem054; private byte byItem055;
            private byte byItem056; private byte byItem057; private byte byItem058; private byte byItem059;
            private byte byItem060; private byte byItem061; private byte byItem062; private byte byItem063;
            private byte byItem064; private byte byItem065; private byte byItem066; private byte byItem067;
            private byte byItem068; private byte byItem069; private byte byItem070; private byte byItem071;
            private byte byItem072; private byte byItem073; private byte byItem074; private byte byItem075;
            private byte byItem076; private byte byItem077; private byte byItem078; private byte byItem079;
            private byte byItem080; private byte byItem081; private byte byItem082; private byte byItem083;
            private byte byItem084; private byte byItem085; private byte byItem086; private byte byItem087;
            private byte byItem088; private byte byItem089; private byte byItem090; private byte byItem091;
            private byte byItem092; private byte byItem093; private byte byItem094; private byte byItem095;
            private byte byItem096; private byte byItem097; private byte byItem098; private byte byItem099;
            private byte byItem100; private byte byItem101; private byte byItem102; private byte byItem103;
            private byte byItem104; private byte byItem105; private byte byItem106; private byte byItem107;
            private byte byItem108; private byte byItem109; private byte byItem110; private byte byItem111;
            private byte byItem112; private byte byItem113; private byte byItem114; private byte byItem115;
            private byte byItem116; private byte byItem117; private byte byItem118; private byte byItem119;
            private byte byItem120; private byte byItem121; private byte byItem122; private byte byItem123;
            private byte byItem124; private byte byItem125; private byte byItem126; private byte byItem127;
            private byte byItem128; private byte byItem129; private byte byItem130; private byte byItem131;
            private byte byItem132; private byte byItem133; private byte byItem134; private byte byItem135;
            private byte byItem136; private byte byItem137; private byte byItem138; private byte byItem139;
            private byte byItem140; private byte byItem141; private byte byItem142; private byte byItem143;
            private byte byItem144; private byte byItem145; private byte byItem146; private byte byItem147;
            private byte byItem148; private byte byItem149; private byte byItem150; private byte byItem151;
            private byte byItem152; private byte byItem153; private byte byItem154; private byte byItem155;
            private byte byItem156; private byte byItem157; private byte byItem158; private byte byItem159;
            private byte byItem160; private byte byItem161; private byte byItem162; private byte byItem163;
            private byte byItem164; private byte byItem165; private byte byItem166; private byte byItem167;
            private byte byItem168; private byte byItem169; private byte byItem170; private byte byItem171;
            private byte byItem172; private byte byItem173; private byte byItem174; private byte byItem175;
            private byte byItem176; private byte byItem177; private byte byItem178; private byte byItem179;
            private byte byItem180; private byte byItem181; private byte byItem182; private byte byItem183;
            private byte byItem184; private byte byItem185; private byte byItem186; private byte byItem187;
            private byte byItem188; private byte byItem189; private byte byItem190; private byte byItem191;
            private byte byItem192; private byte byItem193; private byte byItem194; private byte byItem195;
            private byte byItem196; private byte byItem197; private byte byItem198; private byte byItem199;
            private byte byItem200; private byte byItem201; private byte byItem202; private byte byItem203;
            private byte byItem204; private byte byItem205; private byte byItem206; private byte byItem207;
            private byte byItem208; private byte byItem209; private byte byItem210; private byte byItem211;
            private byte byItem212; private byte byItem213; private byte byItem214; private byte byItem215;
            private byte byItem216; private byte byItem217; private byte byItem218; private byte byItem219;
            private byte byItem220; private byte byItem221; private byte byItem222; private byte byItem223;
            private byte byItem224; private byte byItem225; private byte byItem226; private byte byItem227;
            private byte byItem228; private byte byItem229; private byte byItem230; private byte byItem231;
            private byte byItem232; private byte byItem233; private byte byItem234; private byte byItem235;
            private byte byItem236; private byte byItem237; private byte byItem238; private byte byItem239;
            private byte byItem240; private byte byItem241; private byte byItem242; private byte byItem243;
            private byte byItem244; private byte byItem245; private byte byItem246; private byte byItem247;
            private byte byItem248; private byte byItem249; private byte byItem250; private byte byItem251;
            private byte byItem252; private byte byItem253; private byte byItem254; private byte byItem255;

            /// <summary>
            /// The normal get...
            /// </summary>
            /// <returns></returns>
            public string Get()
            {
                return (GetValue(true));
            }

            /// <summary>
            /// Use this on Mac OS X if you have a call that uses a string
            /// that doesn't include the prefix byte...
            /// </summary>
            /// <returns></returns>
            public string GetNoPrefix()
            {
                return (GetValue(false));
            }

            /// <summary>
            /// Get our value...
            /// </summary>
            /// <returns></returns>
            private string GetValue(bool a_blMayHavePrefix)
            {
                // convert what we have into a byte array
                byte[] abyItem = new byte[256];
                abyItem[0]  = byItem000; abyItem[1]  = byItem001; abyItem[2]  = byItem002; abyItem[3]  = byItem003;
                abyItem[4]  = byItem004; abyItem[5]  = byItem005; abyItem[6]  = byItem006; abyItem[7]  = byItem007;
                abyItem[8]  = byItem008; abyItem[9]  = byItem009; abyItem[10] = byItem010; abyItem[11] = byItem011;
                abyItem[12] = byItem012; abyItem[13] = byItem013; abyItem[14] = byItem014; abyItem[15] = byItem015;
                abyItem[16] = byItem016; abyItem[17] = byItem017; abyItem[18] = byItem018; abyItem[19] = byItem019;
                abyItem[20] = byItem020; abyItem[21] = byItem021; abyItem[22] = byItem022; abyItem[23] = byItem023;
                abyItem[24] = byItem024; abyItem[25] = byItem025; abyItem[26] = byItem026; abyItem[27] = byItem027;
                abyItem[28] = byItem028; abyItem[29] = byItem029; abyItem[30] = byItem030; abyItem[31] = byItem031;
                abyItem[32] = byItem032; abyItem[33] = byItem033; abyItem[34] = byItem034; abyItem[35] = byItem035;
                abyItem[36] = byItem036; abyItem[37] = byItem037; abyItem[38] = byItem038; abyItem[39] = byItem039;
                abyItem[40] = byItem040; abyItem[41] = byItem041; abyItem[42] = byItem042; abyItem[43] = byItem043;
                abyItem[44] = byItem044; abyItem[45] = byItem045; abyItem[46] = byItem046; abyItem[47] = byItem047;
                abyItem[48] = byItem048; abyItem[49] = byItem049; abyItem[50] = byItem050; abyItem[51] = byItem051;
                abyItem[52] = byItem052; abyItem[53] = byItem053; abyItem[54] = byItem054; abyItem[55] = byItem055;
                abyItem[56] = byItem056; abyItem[57] = byItem057; abyItem[58] = byItem058; abyItem[59] = byItem059;
                abyItem[60] = byItem060; abyItem[61] = byItem061; abyItem[62] = byItem062; abyItem[63] = byItem063;
                abyItem[64] = byItem064; abyItem[65] = byItem065; abyItem[66] = byItem066; abyItem[67] = byItem067;
                abyItem[68] = byItem068; abyItem[69] = byItem069; abyItem[70] = byItem070; abyItem[71] = byItem071;
                abyItem[72] = byItem072; abyItem[73] = byItem073; abyItem[74] = byItem074; abyItem[75] = byItem075;
                abyItem[76] = byItem076; abyItem[77] = byItem077; abyItem[78] = byItem078; abyItem[79] = byItem079;
                abyItem[80] = byItem080; abyItem[81] = byItem081; abyItem[82] = byItem082; abyItem[83] = byItem083;
                abyItem[84] = byItem084; abyItem[85] = byItem085; abyItem[86] = byItem086; abyItem[87] = byItem087;
                abyItem[88] = byItem088; abyItem[89] = byItem089; abyItem[90] = byItem090; abyItem[91] = byItem091;
                abyItem[92] = byItem092; abyItem[93] = byItem093; abyItem[94] = byItem094; abyItem[95] = byItem095;
                abyItem[96] = byItem096; abyItem[97] = byItem097; abyItem[98] = byItem098; abyItem[99] = byItem099;
                abyItem[100] = byItem100; abyItem[101] = byItem101; abyItem[102] = byItem102; abyItem[103] = byItem103;
                abyItem[104] = byItem104; abyItem[105] = byItem105; abyItem[106] = byItem106; abyItem[107] = byItem107;
                abyItem[108] = byItem108; abyItem[109] = byItem109; abyItem[110] = byItem110; abyItem[111] = byItem111;
                abyItem[112] = byItem112; abyItem[113] = byItem113; abyItem[114] = byItem114; abyItem[115] = byItem115;
                abyItem[116] = byItem116; abyItem[117] = byItem117; abyItem[118] = byItem118; abyItem[119] = byItem119;
                abyItem[120] = byItem120; abyItem[121] = byItem121; abyItem[122] = byItem122; abyItem[123] = byItem123;
                abyItem[124] = byItem124; abyItem[125] = byItem125; abyItem[126] = byItem126; abyItem[127] = byItem127;
                abyItem[128] = byItem128; abyItem[129] = byItem129; abyItem[130] = byItem130; abyItem[131] = byItem131;
                abyItem[132] = byItem132; abyItem[133] = byItem133; abyItem[134] = byItem134; abyItem[135] = byItem135;
                abyItem[136] = byItem136; abyItem[137] = byItem137; abyItem[138] = byItem138; abyItem[139] = byItem139;
                abyItem[140] = byItem140; abyItem[141] = byItem141; abyItem[142] = byItem142; abyItem[143] = byItem143;
                abyItem[144] = byItem144; abyItem[145] = byItem145; abyItem[146] = byItem146; abyItem[147] = byItem147;
                abyItem[148] = byItem148; abyItem[149] = byItem149; abyItem[150] = byItem150; abyItem[151] = byItem151;
                abyItem[152] = byItem152; abyItem[153] = byItem153; abyItem[154] = byItem154; abyItem[155] = byItem155;
                abyItem[156] = byItem156; abyItem[157] = byItem157; abyItem[158] = byItem158; abyItem[159] = byItem159;
                abyItem[160] = byItem160; abyItem[161] = byItem161; abyItem[162] = byItem162; abyItem[163] = byItem163;
                abyItem[164] = byItem164; abyItem[165] = byItem165; abyItem[166] = byItem166; abyItem[167] = byItem167;
                abyItem[168] = byItem168; abyItem[169] = byItem169; abyItem[170] = byItem170; abyItem[171] = byItem171;
                abyItem[172] = byItem172; abyItem[173] = byItem173; abyItem[174] = byItem174; abyItem[175] = byItem175;
                abyItem[176] = byItem176; abyItem[177] = byItem177; abyItem[178] = byItem178; abyItem[179] = byItem179;
                abyItem[180] = byItem180; abyItem[181] = byItem181; abyItem[182] = byItem182; abyItem[183] = byItem183;
                abyItem[184] = byItem184; abyItem[185] = byItem185; abyItem[186] = byItem186; abyItem[187] = byItem187;
                abyItem[188] = byItem188; abyItem[189] = byItem189; abyItem[190] = byItem190; abyItem[191] = byItem191;
                abyItem[192] = byItem192; abyItem[193] = byItem193; abyItem[194] = byItem194; abyItem[195] = byItem195;
                abyItem[196] = byItem196; abyItem[197] = byItem197; abyItem[198] = byItem198; abyItem[199] = byItem199;
                abyItem[200] = byItem200; abyItem[201] = byItem201; abyItem[202] = byItem202; abyItem[203] = byItem203;
                abyItem[204] = byItem204; abyItem[205] = byItem205; abyItem[206] = byItem206; abyItem[207] = byItem207;
                abyItem[208] = byItem208; abyItem[209] = byItem209; abyItem[210] = byItem210; abyItem[211] = byItem211;
                abyItem[212] = byItem212; abyItem[213] = byItem213; abyItem[214] = byItem214; abyItem[215] = byItem215;
                abyItem[216] = byItem216; abyItem[217] = byItem217; abyItem[218] = byItem218; abyItem[219] = byItem219;
                abyItem[220] = byItem220; abyItem[221] = byItem221; abyItem[222] = byItem222; abyItem[223] = byItem223;
                abyItem[224] = byItem224; abyItem[225] = byItem225; abyItem[226] = byItem226; abyItem[227] = byItem227;
                abyItem[228] = byItem228; abyItem[229] = byItem229; abyItem[230] = byItem230; abyItem[231] = byItem231;
                abyItem[232] = byItem232; abyItem[233] = byItem233; abyItem[234] = byItem234; abyItem[235] = byItem235;
                abyItem[236] = byItem236; abyItem[237] = byItem237; abyItem[238] = byItem238; abyItem[239] = byItem239;
                abyItem[240] = byItem240; abyItem[241] = byItem241; abyItem[242] = byItem242; abyItem[243] = byItem243;
                abyItem[244] = byItem244; abyItem[245] = byItem245; abyItem[246] = byItem246; abyItem[247] = byItem247;
                abyItem[248] = byItem248; abyItem[249] = byItem249; abyItem[250] = byItem250; abyItem[251] = byItem251;
                abyItem[252] = byItem252; abyItem[253] = byItem253; abyItem[254] = byItem254; abyItem[255] = byItem255;

                // Zero anything after the NUL...
                bool blNul = false;
                for (int ii = 0; ii < abyItem.Length; ii++)
                {
                    if (!blNul && (abyItem[ii] == 0))
                    {
                        blNul = true;
                    }
                    else if (blNul)
                    {
                        abyItem[ii] = 0;
                    }
                }

                // change encoding of byte array, then convert the bytes array to a string
                string sz = Encoding.Unicode.GetString(Encoding.Convert(Language.GetEncoding(), Encoding.Unicode, abyItem));

                // If the first character is a NUL, then return the empty string...
                if (sz[0] == '\0')
                {
                    return ("");
                }

                // If we're running on a Mac, take off the prefix 'byte'...
                if (a_blMayHavePrefix && (TWAIN.GetPlatform() == Platform.MACOSX))
                {
                    sz = sz.Remove(0, 1);
                }

                // If we detect a NUL, then split around it...
                if (sz.IndexOf('\0') >= 0)
                {
                    sz = sz.Split(new char[] { '\0' })[0];
                }

                // All done...
                return (sz);
            }

            /// <summary>
            /// The normal set...
            /// </summary>
            /// <returns></returns>
            public void Set(string a_sz)
            {
                SetValue(a_sz, true);
            }

            /// <summary>
            /// Use this on Mac OS X if you have a call that uses a string
            /// that doesn't include the prefix byte...
            /// </summary>
            /// <returns></returns>
            public void SetNoPrefix(string a_sz)
            {
                SetValue(a_sz, false);
            }

            /// <summary>
            /// Set our value...
            /// </summary>
            /// <param name="a_sz"></param>
            private void SetValue(string a_sz, bool a_blMayHavePrefix)
            {
                // If we're running on a Mac, tack on the prefix 'byte'...
                if (a_sz == null)
                {
                    a_sz = "";
                }
                else if (a_blMayHavePrefix && (TWAIN.GetPlatform() == Platform.MACOSX))
                {
                    a_sz = (char)a_sz.Length + a_sz;
                }

                // Make sure that we're NUL padded...
                string sz =
                    a_sz +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0";
                if (sz.Length > 256)
                {
                    sz = sz.Remove(256);
                }

                // convert string to byte array, then change the encoding of the byte array
                byte[] abyItem = Encoding.Convert(Encoding.Unicode, Language.GetEncoding(), Encoding.Unicode.GetBytes(sz));

                // concert byte array to bytes
                byItem000 = abyItem[0];  byItem001 = abyItem[1];  byItem002 = abyItem[2];  byItem003 = abyItem[3];
                byItem004 = abyItem[4];  byItem005 = abyItem[5];  byItem006 = abyItem[6];  byItem007 = abyItem[7];
                byItem008 = abyItem[8];  byItem009 = abyItem[9];  byItem010 = abyItem[10]; byItem011 = abyItem[11];
                byItem012 = abyItem[12]; byItem013 = abyItem[13]; byItem014 = abyItem[14]; byItem015 = abyItem[15];
                byItem016 = abyItem[16]; byItem017 = abyItem[17]; byItem018 = abyItem[18]; byItem019 = abyItem[19];
                byItem020 = abyItem[20]; byItem021 = abyItem[21]; byItem022 = abyItem[22]; byItem023 = abyItem[23];
                byItem024 = abyItem[24]; byItem025 = abyItem[25]; byItem026 = abyItem[26]; byItem027 = abyItem[27];
                byItem028 = abyItem[28]; byItem029 = abyItem[29]; byItem030 = abyItem[30]; byItem031 = abyItem[31];
                byItem032 = abyItem[32]; byItem033 = abyItem[33]; byItem034 = abyItem[34]; byItem035 = abyItem[35];
                byItem036 = abyItem[36]; byItem037 = abyItem[37]; byItem038 = abyItem[38]; byItem039 = abyItem[39];
                byItem040 = abyItem[40]; byItem041 = abyItem[41]; byItem042 = abyItem[42]; byItem043 = abyItem[43];
                byItem044 = abyItem[44]; byItem045 = abyItem[45]; byItem046 = abyItem[46]; byItem047 = abyItem[47];
                byItem048 = abyItem[48]; byItem049 = abyItem[49]; byItem050 = abyItem[50]; byItem051 = abyItem[51];
                byItem052 = abyItem[52]; byItem053 = abyItem[53]; byItem054 = abyItem[54]; byItem055 = abyItem[55];
                byItem056 = abyItem[56]; byItem057 = abyItem[57]; byItem058 = abyItem[58]; byItem059 = abyItem[59];
                byItem060 = abyItem[60]; byItem061 = abyItem[61]; byItem062 = abyItem[62]; byItem063 = abyItem[63];
                byItem064 = abyItem[64]; byItem065 = abyItem[65]; byItem066 = abyItem[66]; byItem067 = abyItem[67];
                byItem068 = abyItem[68]; byItem069 = abyItem[69]; byItem070 = abyItem[70]; byItem071 = abyItem[71];
                byItem072 = abyItem[72]; byItem073 = abyItem[73]; byItem074 = abyItem[74]; byItem075 = abyItem[75];
                byItem076 = abyItem[76]; byItem077 = abyItem[77]; byItem078 = abyItem[78]; byItem079 = abyItem[79];
                byItem080 = abyItem[80]; byItem081 = abyItem[81]; byItem082 = abyItem[82]; byItem083 = abyItem[83];
                byItem084 = abyItem[84]; byItem085 = abyItem[85]; byItem086 = abyItem[86]; byItem087 = abyItem[87];
                byItem088 = abyItem[88]; byItem089 = abyItem[89]; byItem090 = abyItem[90]; byItem091 = abyItem[91];
                byItem092 = abyItem[92]; byItem093 = abyItem[93]; byItem094 = abyItem[94]; byItem095 = abyItem[95];
                byItem096 = abyItem[96]; byItem097 = abyItem[97]; byItem098 = abyItem[98]; byItem099 = abyItem[99];
                byItem100 = abyItem[100]; byItem101 = abyItem[101]; byItem102 = abyItem[102]; byItem103 = abyItem[103];
                byItem104 = abyItem[104]; byItem105 = abyItem[105]; byItem106 = abyItem[106]; byItem107 = abyItem[107];
                byItem108 = abyItem[108]; byItem109 = abyItem[109]; byItem110 = abyItem[110]; byItem111 = abyItem[111];
                byItem112 = abyItem[112]; byItem113 = abyItem[113]; byItem114 = abyItem[114]; byItem115 = abyItem[115];
                byItem116 = abyItem[116]; byItem117 = abyItem[117]; byItem118 = abyItem[118]; byItem119 = abyItem[119];
                byItem120 = abyItem[120]; byItem121 = abyItem[121]; byItem122 = abyItem[122]; byItem123 = abyItem[123];
                byItem124 = abyItem[124]; byItem125 = abyItem[125]; byItem126 = abyItem[126]; byItem127 = abyItem[127];
                byItem128 = abyItem[128]; byItem129 = abyItem[129]; byItem130 = abyItem[130]; byItem131 = abyItem[131];
                byItem132 = abyItem[132]; byItem133 = abyItem[133]; byItem134 = abyItem[134]; byItem135 = abyItem[135];
                byItem136 = abyItem[136]; byItem137 = abyItem[137]; byItem138 = abyItem[138]; byItem139 = abyItem[139];
                byItem140 = abyItem[140]; byItem141 = abyItem[141]; byItem142 = abyItem[142]; byItem143 = abyItem[143];
                byItem144 = abyItem[144]; byItem145 = abyItem[145]; byItem146 = abyItem[146]; byItem147 = abyItem[147];
                byItem148 = abyItem[148]; byItem149 = abyItem[149]; byItem150 = abyItem[150]; byItem151 = abyItem[151];
                byItem152 = abyItem[152]; byItem153 = abyItem[153]; byItem154 = abyItem[154]; byItem155 = abyItem[155];
                byItem156 = abyItem[156]; byItem157 = abyItem[157]; byItem158 = abyItem[158]; byItem159 = abyItem[159];
                byItem160 = abyItem[160]; byItem161 = abyItem[161]; byItem162 = abyItem[162]; byItem163 = abyItem[163];
                byItem164 = abyItem[164]; byItem165 = abyItem[165]; byItem166 = abyItem[166]; byItem167 = abyItem[167];
                byItem168 = abyItem[168]; byItem169 = abyItem[169]; byItem170 = abyItem[170]; byItem171 = abyItem[171];
                byItem172 = abyItem[172]; byItem173 = abyItem[173]; byItem174 = abyItem[174]; byItem175 = abyItem[175];
                byItem176 = abyItem[176]; byItem177 = abyItem[177]; byItem178 = abyItem[178]; byItem179 = abyItem[179];
                byItem180 = abyItem[180]; byItem181 = abyItem[181]; byItem182 = abyItem[182]; byItem183 = abyItem[183];
                byItem184 = abyItem[184]; byItem185 = abyItem[185]; byItem186 = abyItem[186]; byItem187 = abyItem[187];
                byItem188 = abyItem[188]; byItem189 = abyItem[189]; byItem190 = abyItem[190]; byItem191 = abyItem[191];
                byItem192 = abyItem[192]; byItem193 = abyItem[193]; byItem194 = abyItem[194]; byItem195 = abyItem[195];
                byItem196 = abyItem[196]; byItem197 = abyItem[197]; byItem198 = abyItem[198]; byItem199 = abyItem[199];
                byItem200 = abyItem[200]; byItem201 = abyItem[201]; byItem202 = abyItem[202]; byItem203 = abyItem[203];
                byItem204 = abyItem[204]; byItem205 = abyItem[205]; byItem206 = abyItem[206]; byItem207 = abyItem[207];
                byItem208 = abyItem[208]; byItem209 = abyItem[209]; byItem210 = abyItem[210]; byItem211 = abyItem[211];
                byItem212 = abyItem[212]; byItem213 = abyItem[213]; byItem214 = abyItem[214]; byItem215 = abyItem[215];
                byItem216 = abyItem[216]; byItem217 = abyItem[217]; byItem218 = abyItem[218]; byItem219 = abyItem[219];
                byItem220 = abyItem[220]; byItem221 = abyItem[221]; byItem222 = abyItem[222]; byItem223 = abyItem[223];
                byItem224 = abyItem[224]; byItem225 = abyItem[225]; byItem226 = abyItem[226]; byItem227 = abyItem[227];
                byItem228 = abyItem[228]; byItem229 = abyItem[229]; byItem230 = abyItem[230]; byItem231 = abyItem[231];
                byItem232 = abyItem[232]; byItem233 = abyItem[233]; byItem234 = abyItem[234]; byItem235 = abyItem[235];
                byItem236 = abyItem[236]; byItem237 = abyItem[237]; byItem238 = abyItem[238]; byItem239 = abyItem[239];
                byItem240 = abyItem[240]; byItem241 = abyItem[241]; byItem242 = abyItem[242]; byItem243 = abyItem[243];
                byItem244 = abyItem[244]; byItem245 = abyItem[245]; byItem246 = abyItem[246]; byItem247 = abyItem[247];
                byItem248 = abyItem[248]; byItem249 = abyItem[249]; byItem250 = abyItem[250]; byItem251 = abyItem[251];
                byItem252 = abyItem[252]; byItem253 = abyItem[253]; byItem254 = abyItem[254]; byItem255 = abyItem[255];
            }
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Structure Definitions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Structure Definitions..

        /// <summary>
        /// Fixed point structure type.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_FIX32
        {
            public short Whole;
            public ushort Frac;
        }

        /// <summary>
        /// Defines a frame rectangle in ICAP_UNITS coordinates.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_FRAME
        {
            public TW_FIX32 Left;
            public TW_FIX32 Top;
            public TW_FIX32 Right;
            public TW_FIX32 Bottom;
        }

        /// <summary>
        /// Defines the parameters used for channel-specific transformation.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_DECODEFUNCTION
        {
            public TW_FIX32 StartIn;
            public TW_FIX32 BreakIn;
            public TW_FIX32 EndIn;
            public TW_FIX32 StartOut;
            public TW_FIX32 BreakOut;
            public TW_FIX32 EndOut;
            public TW_FIX32 Gamma;
            public TW_FIX32 SampleCount;
        }

        /// <summary>
        /// Stores a Fixed point number in two parts, a whole and a fractional part.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_TRANSFORMSTAGE
        {
            public TW_DECODEFUNCTION Decode_0;
            public TW_DECODEFUNCTION Decode_1;
            public TW_DECODEFUNCTION Decode_2;
            public TW_FIX32 Mix_0_0;
            public TW_FIX32 Mix_0_1;
            public TW_FIX32 Mix_0_2;
            public TW_FIX32 Mix_1_0;
            public TW_FIX32 Mix_1_1;
            public TW_FIX32 Mix_1_2;
            public TW_FIX32 Mix_2_0;
            public TW_FIX32 Mix_2_1;
            public TW_FIX32 Mix_2_2;
        }

        /// <summary>
        /// Stores a list of values for a capability, the ItemList is commented
        /// out so that the caller can collect information about it with a
        /// marshalling call...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_ARRAY
        {
            public TWTY ItemType;
            public uint NumItems;
            //public byte[] ItemList;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_ARRAY_MACOSX
        {
            public uint ItemType;
            public uint NumItems;
            //public byte[] ItemList;
        }

        /// <summary>
        /// Information about audio data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_AUDIOINFO
        {
            public TW_STR255 Name;
            public uint Reserved;
        }

        /// <summary>
        /// Used to register callbacks.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_CALLBACK
        {
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr CallBackProc;
            public uint RefCon;
            public ushort Message;
        }

        /// <summary>
        /// Used to register callbacks.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_CALLBACK2
        {
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr CallBackProc;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public UIntPtr RefCon;
            public ushort Message;
        }

        /// <summary>
        /// Used by application to get/set capability from/in a data source.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_CAPABILITY
        {
            public CAP Cap;
            public TWON ConType;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr hContainer;
        }

        /// <summary>
        /// Defines a CIE XYZ space tri-stimulus value.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_CIEPOINT
        {
            public TW_FIX32 X;
            public TW_FIX32 Y;
            public TW_FIX32 Z;
        }

        /// <summary>
        /// Defines the mapping from an RGB color space device into CIE 1931 (XYZ) color space.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_CIECOLOR
        {
            public ushort ColorSpace;
            public short LowEndian;
            public short DeviceDependent;
            public int VersionNumber;
            public TW_TRANSFORMSTAGE StageABC;
            public TW_TRANSFORMSTAGE StageLNM;
            public TW_CIEPOINT WhitePoint;
            public TW_CIEPOINT BlackPoint;
            public TW_CIEPOINT WhitePaper;
            public TW_CIEPOINT BlackInk;
            public TW_FIX32 Samples;
        }

        /// <summary>
        /// Allows for a data source and application to pass custom data to each other.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_CUSTOMDSDATA
        {
            public uint InfoLength;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr hData;
        }

        /// <summary>
        /// Provides information about the Event that was raised by the Source.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_DEVICEEVENT
        {
            public uint Event;
            public TW_STR255 DeviceName;
            public uint BatteryMinutes;
            public short BatteryPercentage;
            public int PowerSupply;
            public TW_FIX32 XResolution;
            public TW_FIX32 YResolution;
            public uint FlashUsed2;
            public uint AutomaticCapture;
            public uint TimeBeforeFirstCapture;
            public uint TimeBetweenCaptures;
        }

        /// <summary>
        /// This structure holds the tri-stimulus color palette information for TW_PALETTE8 structures.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_ELEMENT8
        {
            public byte Index;
            public byte Channel1;
            public byte Channel2;
            public byte Channel3;
        }

        /// <summary>
        /// DAT_ENTRYPOINT. returns essential entry points.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_ENTRYPOINT
        {
            public UInt32 Size;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_Entry;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_MemAllocate;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_MemFree;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_MemLock;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_MemUnlock;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_ENTRYPOINT_LINUX64
        {
            public long Size;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_Entry;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_MemAllocate;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_MemFree;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_MemLock;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_MemUnlock;
        }
        public struct TW_ENTRYPOINT_DELEGATES
        {
            public UInt32 Size;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr DSM_Entry;
            public DSM_MEMALLOC DSM_MemAllocate;
            public DSM_MEMFREE DSM_MemFree;
            public DSM_MEMLOCK DSM_MemLock;
            public DSM_MEMUNLOCK DSM_MemUnlock;
        }
        public delegate IntPtr DSM_MEMALLOC(uint size);
        public delegate void DSM_MEMFREE(IntPtr handle);
        public delegate IntPtr DSM_MEMLOCK(IntPtr handle);
        public delegate void DSM_MEMUNLOCK(IntPtr handle);

        /// <summary>
        /// Stores a group of enumerated values for a capability, the ItemList is
        /// commented out so that the caller can collect information about it with
        /// a marshalling call...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_ENUMERATION
        {
            public TWTY ItemType;
            public uint NumItems;
            public uint CurrentIndex;
            public uint DefaultIndex;
            //public byte[] ItemList;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_ENUMERATION_LINUX64
        {
            public TWTY ItemType;
            public ulong NumItems;
            public ulong CurrentIndex;
            public ulong DefaultIndex;
            //public byte[] ItemList;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TW_ENUMERATION_MACOSX
        {
            public uint ItemType;
            public uint NumItems;
            public uint CurrentIndex;
            public uint DefaultIndex;
            //public byte[] ItemList;
        }

        /// <summary>
        /// Used to pass application events/messages from the application to the Source.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_EVENT
        {
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr pEvent;
            public ushort TWMessage;
        }

        /// <summary>
        /// DAT_FILTER...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_FILTER_DESCRIPTOR
        {
            public UInt32 Size;
            public UInt32 HueStart;
            public UInt32 HueEnd;
            public UInt32 SaturationStart;
            public UInt32 SaturationEnd;
            public UInt32 ValueStart;
            public UInt32 ValueEnd;
            public UInt32 Replacement;
        }

        /// <summary>
        /// DAT_FILTER...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_FILTER
        {
            public UInt32 Size;
            public UInt32 DescriptorCount;
            public UInt32 MaxDescriptorCount;
            public UInt32 Condition;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr hDescriptors;
        }

        /// <summary>
        /// This structure is used to pass specific information between the data source and the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_INFO
        {
            public ushort InfoId;
            public ushort ItemType;
            public ushort NumItems;
            public ushort ReturnCode;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public UIntPtr Item;
        }

        /// <summary>
        /// This structure is used to pass specific information between the data source and the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_EXTIMAGEINFO
        {
            public uint NumInfos;
            public TW_INFO Info_000;
            public TW_INFO Info_001;
            public TW_INFO Info_002;
            public TW_INFO Info_003;
            public TW_INFO Info_004;
            public TW_INFO Info_005;
            public TW_INFO Info_006;
            public TW_INFO Info_007;
            public TW_INFO Info_008;
            public TW_INFO Info_009;
            public TW_INFO Info_010;
            public TW_INFO Info_011;
            public TW_INFO Info_012;
            public TW_INFO Info_013;
            public TW_INFO Info_014;
            public TW_INFO Info_015;
            public TW_INFO Info_016;
            public TW_INFO Info_017;
            public TW_INFO Info_018;
            public TW_INFO Info_019;
            public TW_INFO Info_020;
            public TW_INFO Info_021;
            public TW_INFO Info_022;
            public TW_INFO Info_023;
            public TW_INFO Info_024;
            public TW_INFO Info_025;
            public TW_INFO Info_026;
            public TW_INFO Info_027;
            public TW_INFO Info_028;
            public TW_INFO Info_029;
            public TW_INFO Info_030;
            public TW_INFO Info_031;
            public TW_INFO Info_032;
            public TW_INFO Info_033;
            public TW_INFO Info_034;
            public TW_INFO Info_035;
            public TW_INFO Info_036;
            public TW_INFO Info_037;
            public TW_INFO Info_038;
            public TW_INFO Info_039;
            public TW_INFO Info_040;
            public TW_INFO Info_041;
            public TW_INFO Info_042;
            public TW_INFO Info_043;
            public TW_INFO Info_044;
            public TW_INFO Info_045;
            public TW_INFO Info_046;
            public TW_INFO Info_047;
            public TW_INFO Info_048;
            public TW_INFO Info_049;
            public TW_INFO Info_050;
            public TW_INFO Info_051;
            public TW_INFO Info_052;
            public TW_INFO Info_053;
            public TW_INFO Info_054;
            public TW_INFO Info_055;
            public TW_INFO Info_056;
            public TW_INFO Info_057;
            public TW_INFO Info_058;
            public TW_INFO Info_059;
            public TW_INFO Info_060;
            public TW_INFO Info_061;
            public TW_INFO Info_062;
            public TW_INFO Info_063;
            public TW_INFO Info_064;
            public TW_INFO Info_065;
            public TW_INFO Info_066;
            public TW_INFO Info_067;
            public TW_INFO Info_068;
            public TW_INFO Info_069;
            public TW_INFO Info_070;
            public TW_INFO Info_071;
            public TW_INFO Info_072;
            public TW_INFO Info_073;
            public TW_INFO Info_074;
            public TW_INFO Info_075;
            public TW_INFO Info_076;
            public TW_INFO Info_077;
            public TW_INFO Info_078;
            public TW_INFO Info_079;
            public TW_INFO Info_080;
            public TW_INFO Info_081;
            public TW_INFO Info_082;
            public TW_INFO Info_083;
            public TW_INFO Info_084;
            public TW_INFO Info_085;
            public TW_INFO Info_086;
            public TW_INFO Info_087;
            public TW_INFO Info_088;
            public TW_INFO Info_089;
            public TW_INFO Info_090;
            public TW_INFO Info_091;
            public TW_INFO Info_092;
            public TW_INFO Info_093;
            public TW_INFO Info_094;
            public TW_INFO Info_095;
            public TW_INFO Info_096;
            public TW_INFO Info_097;
            public TW_INFO Info_098;
            public TW_INFO Info_099;
            public TW_INFO Info_100;
            public TW_INFO Info_101;
            public TW_INFO Info_102;
            public TW_INFO Info_103;
            public TW_INFO Info_104;
            public TW_INFO Info_105;
            public TW_INFO Info_106;
            public TW_INFO Info_107;
            public TW_INFO Info_108;
            public TW_INFO Info_109;
            public TW_INFO Info_110;
            public TW_INFO Info_111;
            public TW_INFO Info_112;
            public TW_INFO Info_113;
            public TW_INFO Info_114;
            public TW_INFO Info_115;
            public TW_INFO Info_116;
            public TW_INFO Info_117;
            public TW_INFO Info_118;
            public TW_INFO Info_119;
            public TW_INFO Info_120;
            public TW_INFO Info_121;
            public TW_INFO Info_122;
            public TW_INFO Info_123;
            public TW_INFO Info_124;
            public TW_INFO Info_125;
            public TW_INFO Info_126;
            public TW_INFO Info_127;
            public TW_INFO Info_128;
            public TW_INFO Info_129;
            public TW_INFO Info_130;
            public TW_INFO Info_131;
            public TW_INFO Info_132;
            public TW_INFO Info_133;
            public TW_INFO Info_134;
            public TW_INFO Info_135;
            public TW_INFO Info_136;
            public TW_INFO Info_137;
            public TW_INFO Info_138;
            public TW_INFO Info_139;
            public TW_INFO Info_140;
            public TW_INFO Info_141;
            public TW_INFO Info_142;
            public TW_INFO Info_143;
            public TW_INFO Info_144;
            public TW_INFO Info_145;
            public TW_INFO Info_146;
            public TW_INFO Info_147;
            public TW_INFO Info_148;
            public TW_INFO Info_149;
            public TW_INFO Info_150;
            public TW_INFO Info_151;
            public TW_INFO Info_152;
            public TW_INFO Info_153;
            public TW_INFO Info_154;
            public TW_INFO Info_155;
            public TW_INFO Info_156;
            public TW_INFO Info_157;
            public TW_INFO Info_158;
            public TW_INFO Info_159;
            public TW_INFO Info_160;
            public TW_INFO Info_161;
            public TW_INFO Info_162;
            public TW_INFO Info_163;
            public TW_INFO Info_164;
            public TW_INFO Info_165;
            public TW_INFO Info_166;
            public TW_INFO Info_167;
            public TW_INFO Info_168;
            public TW_INFO Info_169;
            public TW_INFO Info_170;
            public TW_INFO Info_171;
            public TW_INFO Info_172;
            public TW_INFO Info_173;
            public TW_INFO Info_174;
            public TW_INFO Info_175;
            public TW_INFO Info_176;
            public TW_INFO Info_177;
            public TW_INFO Info_178;
            public TW_INFO Info_179;
            public TW_INFO Info_180;
            public TW_INFO Info_181;
            public TW_INFO Info_182;
            public TW_INFO Info_183;
            public TW_INFO Info_184;
            public TW_INFO Info_185;
            public TW_INFO Info_186;
            public TW_INFO Info_187;
            public TW_INFO Info_188;
            public TW_INFO Info_189;
            public TW_INFO Info_190;
            public TW_INFO Info_191;
            public TW_INFO Info_192;
            public TW_INFO Info_193;
            public TW_INFO Info_194;
            public TW_INFO Info_195;
            public TW_INFO Info_196;
            public TW_INFO Info_197;
            public TW_INFO Info_198;
            public TW_INFO Info_199;

            public void Get(uint a_uIndex, ref TW_INFO a_twinfo)
            {
                switch (a_uIndex)
                {
                    default: return;
                    case 0: a_twinfo = Info_000; return;
                    case 1: a_twinfo = Info_001; return;
                    case 2: a_twinfo = Info_002; return;
                    case 3: a_twinfo = Info_003; return;
                    case 4: a_twinfo = Info_004; return;
                    case 5: a_twinfo = Info_005; return;
                    case 6: a_twinfo = Info_006; return;
                    case 7: a_twinfo = Info_007; return;
                    case 8: a_twinfo = Info_008; return;
                    case 9: a_twinfo = Info_009; return;
                    case 10: a_twinfo = Info_010; return;
                    case 11: a_twinfo = Info_011; return;
                    case 12: a_twinfo = Info_012; return;
                    case 13: a_twinfo = Info_013; return;
                    case 14: a_twinfo = Info_014; return;
                    case 15: a_twinfo = Info_015; return;
                    case 16: a_twinfo = Info_016; return;
                    case 17: a_twinfo = Info_017; return;
                    case 18: a_twinfo = Info_018; return;
                    case 19: a_twinfo = Info_019; return;
                    case 20: a_twinfo = Info_020; return;
                    case 21: a_twinfo = Info_021; return;
                    case 22: a_twinfo = Info_022; return;
                    case 23: a_twinfo = Info_023; return;
                    case 24: a_twinfo = Info_024; return;
                    case 25: a_twinfo = Info_025; return;
                    case 26: a_twinfo = Info_026; return;
                    case 27: a_twinfo = Info_027; return;
                    case 28: a_twinfo = Info_028; return;
                    case 29: a_twinfo = Info_029; return;
                    case 30: a_twinfo = Info_030; return;
                    case 31: a_twinfo = Info_031; return;
                    case 32: a_twinfo = Info_032; return;
                    case 33: a_twinfo = Info_033; return;
                    case 34: a_twinfo = Info_034; return;
                    case 35: a_twinfo = Info_035; return;
                    case 36: a_twinfo = Info_036; return;
                    case 37: a_twinfo = Info_037; return;
                    case 38: a_twinfo = Info_038; return;
                    case 39: a_twinfo = Info_039; return;
                    case 40: a_twinfo = Info_040; return;
                    case 41: a_twinfo = Info_041; return;
                    case 42: a_twinfo = Info_042; return;
                    case 43: a_twinfo = Info_043; return;
                    case 44: a_twinfo = Info_044; return;
                    case 45: a_twinfo = Info_045; return;
                    case 46: a_twinfo = Info_046; return;
                    case 47: a_twinfo = Info_047; return;
                    case 48: a_twinfo = Info_048; return;
                    case 49: a_twinfo = Info_049; return;
                    case 50: a_twinfo = Info_050; return;
                    case 51: a_twinfo = Info_051; return;
                    case 52: a_twinfo = Info_052; return;
                    case 53: a_twinfo = Info_053; return;
                    case 54: a_twinfo = Info_054; return;
                    case 55: a_twinfo = Info_055; return;
                    case 56: a_twinfo = Info_056; return;
                    case 57: a_twinfo = Info_057; return;
                    case 58: a_twinfo = Info_058; return;
                    case 59: a_twinfo = Info_059; return;
                    case 60: a_twinfo = Info_060; return;
                    case 61: a_twinfo = Info_061; return;
                    case 62: a_twinfo = Info_062; return;
                    case 63: a_twinfo = Info_063; return;
                    case 64: a_twinfo = Info_064; return;
                    case 65: a_twinfo = Info_065; return;
                    case 66: a_twinfo = Info_066; return;
                    case 67: a_twinfo = Info_067; return;
                    case 68: a_twinfo = Info_068; return;
                    case 69: a_twinfo = Info_069; return;
                    case 70: a_twinfo = Info_070; return;
                    case 71: a_twinfo = Info_071; return;
                    case 72: a_twinfo = Info_072; return;
                    case 73: a_twinfo = Info_073; return;
                    case 74: a_twinfo = Info_074; return;
                    case 75: a_twinfo = Info_075; return;
                    case 76: a_twinfo = Info_076; return;
                    case 77: a_twinfo = Info_077; return;
                    case 78: a_twinfo = Info_078; return;
                    case 79: a_twinfo = Info_079; return;
                    case 80: a_twinfo = Info_080; return;
                    case 81: a_twinfo = Info_081; return;
                    case 82: a_twinfo = Info_082; return;
                    case 83: a_twinfo = Info_083; return;
                    case 84: a_twinfo = Info_084; return;
                    case 85: a_twinfo = Info_085; return;
                    case 86: a_twinfo = Info_086; return;
                    case 87: a_twinfo = Info_087; return;
                    case 88: a_twinfo = Info_088; return;
                    case 89: a_twinfo = Info_089; return;
                    case 90: a_twinfo = Info_090; return;
                    case 91: a_twinfo = Info_091; return;
                    case 92: a_twinfo = Info_092; return;
                    case 93: a_twinfo = Info_093; return;
                    case 94: a_twinfo = Info_094; return;
                    case 95: a_twinfo = Info_095; return;
                    case 96: a_twinfo = Info_096; return;
                    case 97: a_twinfo = Info_097; return;
                    case 98: a_twinfo = Info_098; return;
                    case 99: a_twinfo = Info_099; return;
                    case 100: a_twinfo = Info_100; return;
                    case 101: a_twinfo = Info_101; return;
                    case 102: a_twinfo = Info_102; return;
                    case 103: a_twinfo = Info_103; return;
                    case 104: a_twinfo = Info_104; return;
                    case 105: a_twinfo = Info_105; return;
                    case 106: a_twinfo = Info_106; return;
                    case 107: a_twinfo = Info_107; return;
                    case 108: a_twinfo = Info_108; return;
                    case 109: a_twinfo = Info_109; return;
                    case 110: a_twinfo = Info_110; return;
                    case 111: a_twinfo = Info_111; return;
                    case 112: a_twinfo = Info_112; return;
                    case 113: a_twinfo = Info_113; return;
                    case 114: a_twinfo = Info_114; return;
                    case 115: a_twinfo = Info_115; return;
                    case 116: a_twinfo = Info_116; return;
                    case 117: a_twinfo = Info_117; return;
                    case 118: a_twinfo = Info_118; return;
                    case 119: a_twinfo = Info_119; return;
                    case 120: a_twinfo = Info_120; return;
                    case 121: a_twinfo = Info_121; return;
                    case 122: a_twinfo = Info_122; return;
                    case 123: a_twinfo = Info_123; return;
                    case 124: a_twinfo = Info_124; return;
                    case 125: a_twinfo = Info_125; return;
                    case 126: a_twinfo = Info_126; return;
                    case 127: a_twinfo = Info_127; return;
                    case 128: a_twinfo = Info_128; return;
                    case 129: a_twinfo = Info_129; return;
                    case 130: a_twinfo = Info_130; return;
                    case 131: a_twinfo = Info_131; return;
                    case 132: a_twinfo = Info_132; return;
                    case 133: a_twinfo = Info_133; return;
                    case 134: a_twinfo = Info_134; return;
                    case 135: a_twinfo = Info_135; return;
                    case 136: a_twinfo = Info_136; return;
                    case 137: a_twinfo = Info_137; return;
                    case 138: a_twinfo = Info_138; return;
                    case 139: a_twinfo = Info_139; return;
                    case 140: a_twinfo = Info_140; return;
                    case 141: a_twinfo = Info_141; return;
                    case 142: a_twinfo = Info_142; return;
                    case 143: a_twinfo = Info_143; return;
                    case 144: a_twinfo = Info_144; return;
                    case 145: a_twinfo = Info_145; return;
                    case 146: a_twinfo = Info_146; return;
                    case 147: a_twinfo = Info_147; return;
                    case 148: a_twinfo = Info_148; return;
                    case 149: a_twinfo = Info_149; return;
                    case 150: a_twinfo = Info_150; return;
                    case 151: a_twinfo = Info_151; return;
                    case 152: a_twinfo = Info_152; return;
                    case 153: a_twinfo = Info_153; return;
                    case 154: a_twinfo = Info_154; return;
                    case 155: a_twinfo = Info_155; return;
                    case 156: a_twinfo = Info_156; return;
                    case 157: a_twinfo = Info_157; return;
                    case 158: a_twinfo = Info_158; return;
                    case 159: a_twinfo = Info_159; return;
                    case 160: a_twinfo = Info_160; return;
                    case 161: a_twinfo = Info_161; return;
                    case 162: a_twinfo = Info_162; return;
                    case 163: a_twinfo = Info_163; return;
                    case 164: a_twinfo = Info_164; return;
                    case 165: a_twinfo = Info_165; return;
                    case 166: a_twinfo = Info_166; return;
                    case 167: a_twinfo = Info_167; return;
                    case 168: a_twinfo = Info_168; return;
                    case 169: a_twinfo = Info_169; return;
                    case 170: a_twinfo = Info_170; return;
                    case 171: a_twinfo = Info_171; return;
                    case 172: a_twinfo = Info_172; return;
                    case 173: a_twinfo = Info_173; return;
                    case 174: a_twinfo = Info_174; return;
                    case 175: a_twinfo = Info_175; return;
                    case 176: a_twinfo = Info_176; return;
                    case 177: a_twinfo = Info_177; return;
                    case 178: a_twinfo = Info_178; return;
                    case 179: a_twinfo = Info_179; return;
                    case 180: a_twinfo = Info_180; return;
                    case 181: a_twinfo = Info_181; return;
                    case 182: a_twinfo = Info_182; return;
                    case 183: a_twinfo = Info_183; return;
                    case 184: a_twinfo = Info_184; return;
                    case 185: a_twinfo = Info_185; return;
                    case 186: a_twinfo = Info_186; return;
                    case 187: a_twinfo = Info_187; return;
                    case 188: a_twinfo = Info_188; return;
                    case 189: a_twinfo = Info_189; return;
                    case 190: a_twinfo = Info_190; return;
                    case 191: a_twinfo = Info_191; return;
                    case 192: a_twinfo = Info_192; return;
                    case 193: a_twinfo = Info_193; return;
                    case 194: a_twinfo = Info_194; return;
                    case 195: a_twinfo = Info_195; return;
                    case 196: a_twinfo = Info_196; return;
                    case 197: a_twinfo = Info_197; return;
                    case 198: a_twinfo = Info_198; return;
                    case 199: a_twinfo = Info_199; return;
                }
            }

            public void Set(uint a_uIndex, ref TW_INFO a_twinfo)
            {
                switch (a_uIndex)
                {
                    default: return;
                    case 0: Info_000 = a_twinfo; return;
                    case 1: Info_001 = a_twinfo; return;
                    case 2: Info_002 = a_twinfo; return;
                    case 3: Info_003 = a_twinfo; return;
                    case 4: Info_004 = a_twinfo; return;
                    case 5: Info_005 = a_twinfo; return;
                    case 6: Info_006 = a_twinfo; return;
                    case 7: Info_007 = a_twinfo; return;
                    case 8: Info_008 = a_twinfo; return;
                    case 9: Info_009 = a_twinfo; return;
                    case 10: Info_010 = a_twinfo; return;
                    case 11: Info_011 = a_twinfo; return;
                    case 12: Info_012 = a_twinfo; return;
                    case 13: Info_013 = a_twinfo; return;
                    case 14: Info_014 = a_twinfo; return;
                    case 15: Info_015 = a_twinfo; return;
                    case 16: Info_016 = a_twinfo; return;
                    case 17: Info_017 = a_twinfo; return;
                    case 18: Info_018 = a_twinfo; return;
                    case 19: Info_019 = a_twinfo; return;
                    case 20: Info_020 = a_twinfo; return;
                    case 21: Info_021 = a_twinfo; return;
                    case 22: Info_022 = a_twinfo; return;
                    case 23: Info_023 = a_twinfo; return;
                    case 24: Info_024 = a_twinfo; return;
                    case 25: Info_025 = a_twinfo; return;
                    case 26: Info_026 = a_twinfo; return;
                    case 27: Info_027 = a_twinfo; return;
                    case 28: Info_028 = a_twinfo; return;
                    case 29: Info_029 = a_twinfo; return;
                    case 30: Info_030 = a_twinfo; return;
                    case 31: Info_031 = a_twinfo; return;
                    case 32: Info_032 = a_twinfo; return;
                    case 33: Info_033 = a_twinfo; return;
                    case 34: Info_034 = a_twinfo; return;
                    case 35: Info_035 = a_twinfo; return;
                    case 36: Info_036 = a_twinfo; return;
                    case 37: Info_037 = a_twinfo; return;
                    case 38: Info_038 = a_twinfo; return;
                    case 39: Info_039 = a_twinfo; return;
                    case 40: Info_040 = a_twinfo; return;
                    case 41: Info_041 = a_twinfo; return;
                    case 42: Info_042 = a_twinfo; return;
                    case 43: Info_043 = a_twinfo; return;
                    case 44: Info_044 = a_twinfo; return;
                    case 45: Info_045 = a_twinfo; return;
                    case 46: Info_046 = a_twinfo; return;
                    case 47: Info_047 = a_twinfo; return;
                    case 48: Info_048 = a_twinfo; return;
                    case 49: Info_049 = a_twinfo; return;
                    case 50: Info_050 = a_twinfo; return;
                    case 51: Info_051 = a_twinfo; return;
                    case 52: Info_052 = a_twinfo; return;
                    case 53: Info_053 = a_twinfo; return;
                    case 54: Info_054 = a_twinfo; return;
                    case 55: Info_055 = a_twinfo; return;
                    case 56: Info_056 = a_twinfo; return;
                    case 57: Info_057 = a_twinfo; return;
                    case 58: Info_058 = a_twinfo; return;
                    case 59: Info_059 = a_twinfo; return;
                    case 60: Info_060 = a_twinfo; return;
                    case 61: Info_061 = a_twinfo; return;
                    case 62: Info_062 = a_twinfo; return;
                    case 63: Info_063 = a_twinfo; return;
                    case 64: Info_064 = a_twinfo; return;
                    case 65: Info_065 = a_twinfo; return;
                    case 66: Info_066 = a_twinfo; return;
                    case 67: Info_067 = a_twinfo; return;
                    case 68: Info_068 = a_twinfo; return;
                    case 69: Info_069 = a_twinfo; return;
                    case 70: Info_070 = a_twinfo; return;
                    case 71: Info_071 = a_twinfo; return;
                    case 72: Info_072 = a_twinfo; return;
                    case 73: Info_073 = a_twinfo; return;
                    case 74: Info_074 = a_twinfo; return;
                    case 75: Info_075 = a_twinfo; return;
                    case 76: Info_076 = a_twinfo; return;
                    case 77: Info_077 = a_twinfo; return;
                    case 78: Info_078 = a_twinfo; return;
                    case 79: Info_079 = a_twinfo; return;
                    case 80: Info_080 = a_twinfo; return;
                    case 81: Info_081 = a_twinfo; return;
                    case 82: Info_082 = a_twinfo; return;
                    case 83: Info_083 = a_twinfo; return;
                    case 84: Info_084 = a_twinfo; return;
                    case 85: Info_085 = a_twinfo; return;
                    case 86: Info_086 = a_twinfo; return;
                    case 87: Info_087 = a_twinfo; return;
                    case 88: Info_088 = a_twinfo; return;
                    case 89: Info_089 = a_twinfo; return;
                    case 90: Info_090 = a_twinfo; return;
                    case 91: Info_091 = a_twinfo; return;
                    case 92: Info_092 = a_twinfo; return;
                    case 93: Info_093 = a_twinfo; return;
                    case 94: Info_094 = a_twinfo; return;
                    case 95: Info_095 = a_twinfo; return;
                    case 96: Info_096 = a_twinfo; return;
                    case 97: Info_097 = a_twinfo; return;
                    case 98: Info_098 = a_twinfo; return;
                    case 99: Info_099 = a_twinfo; return;
                    case 100: Info_100 = a_twinfo; return;
                    case 101: Info_101 = a_twinfo; return;
                    case 102: Info_102 = a_twinfo; return;
                    case 103: Info_103 = a_twinfo; return;
                    case 104: Info_104 = a_twinfo; return;
                    case 105: Info_105 = a_twinfo; return;
                    case 106: Info_106 = a_twinfo; return;
                    case 107: Info_107 = a_twinfo; return;
                    case 108: Info_108 = a_twinfo; return;
                    case 109: Info_109 = a_twinfo; return;
                    case 110: Info_110 = a_twinfo; return;
                    case 111: Info_111 = a_twinfo; return;
                    case 112: Info_112 = a_twinfo; return;
                    case 113: Info_113 = a_twinfo; return;
                    case 114: Info_114 = a_twinfo; return;
                    case 115: Info_115 = a_twinfo; return;
                    case 116: Info_116 = a_twinfo; return;
                    case 117: Info_117 = a_twinfo; return;
                    case 118: Info_118 = a_twinfo; return;
                    case 119: Info_119 = a_twinfo; return;
                    case 120: Info_120 = a_twinfo; return;
                    case 121: Info_121 = a_twinfo; return;
                    case 122: Info_122 = a_twinfo; return;
                    case 123: Info_123 = a_twinfo; return;
                    case 124: Info_124 = a_twinfo; return;
                    case 125: Info_125 = a_twinfo; return;
                    case 126: Info_126 = a_twinfo; return;
                    case 127: Info_127 = a_twinfo; return;
                    case 128: Info_128 = a_twinfo; return;
                    case 129: Info_129 = a_twinfo; return;
                    case 130: Info_130 = a_twinfo; return;
                    case 131: Info_131 = a_twinfo; return;
                    case 132: Info_132 = a_twinfo; return;
                    case 133: Info_133 = a_twinfo; return;
                    case 134: Info_134 = a_twinfo; return;
                    case 135: Info_135 = a_twinfo; return;
                    case 136: Info_136 = a_twinfo; return;
                    case 137: Info_137 = a_twinfo; return;
                    case 138: Info_138 = a_twinfo; return;
                    case 139: Info_139 = a_twinfo; return;
                    case 140: Info_140 = a_twinfo; return;
                    case 141: Info_141 = a_twinfo; return;
                    case 142: Info_142 = a_twinfo; return;
                    case 143: Info_143 = a_twinfo; return;
                    case 144: Info_144 = a_twinfo; return;
                    case 145: Info_145 = a_twinfo; return;
                    case 146: Info_146 = a_twinfo; return;
                    case 147: Info_147 = a_twinfo; return;
                    case 148: Info_148 = a_twinfo; return;
                    case 149: Info_149 = a_twinfo; return;
                    case 150: Info_150 = a_twinfo; return;
                    case 151: Info_151 = a_twinfo; return;
                    case 152: Info_152 = a_twinfo; return;
                    case 153: Info_153 = a_twinfo; return;
                    case 154: Info_154 = a_twinfo; return;
                    case 155: Info_155 = a_twinfo; return;
                    case 156: Info_156 = a_twinfo; return;
                    case 157: Info_157 = a_twinfo; return;
                    case 158: Info_158 = a_twinfo; return;
                    case 159: Info_159 = a_twinfo; return;
                    case 160: Info_160 = a_twinfo; return;
                    case 161: Info_161 = a_twinfo; return;
                    case 162: Info_162 = a_twinfo; return;
                    case 163: Info_163 = a_twinfo; return;
                    case 164: Info_164 = a_twinfo; return;
                    case 165: Info_165 = a_twinfo; return;
                    case 166: Info_166 = a_twinfo; return;
                    case 167: Info_167 = a_twinfo; return;
                    case 168: Info_168 = a_twinfo; return;
                    case 169: Info_169 = a_twinfo; return;
                    case 170: Info_170 = a_twinfo; return;
                    case 171: Info_171 = a_twinfo; return;
                    case 172: Info_172 = a_twinfo; return;
                    case 173: Info_173 = a_twinfo; return;
                    case 174: Info_174 = a_twinfo; return;
                    case 175: Info_175 = a_twinfo; return;
                    case 176: Info_176 = a_twinfo; return;
                    case 177: Info_177 = a_twinfo; return;
                    case 178: Info_178 = a_twinfo; return;
                    case 179: Info_179 = a_twinfo; return;
                    case 180: Info_180 = a_twinfo; return;
                    case 181: Info_181 = a_twinfo; return;
                    case 182: Info_182 = a_twinfo; return;
                    case 183: Info_183 = a_twinfo; return;
                    case 184: Info_184 = a_twinfo; return;
                    case 185: Info_185 = a_twinfo; return;
                    case 186: Info_186 = a_twinfo; return;
                    case 187: Info_187 = a_twinfo; return;
                    case 188: Info_188 = a_twinfo; return;
                    case 189: Info_189 = a_twinfo; return;
                    case 190: Info_190 = a_twinfo; return;
                    case 191: Info_191 = a_twinfo; return;
                    case 192: Info_192 = a_twinfo; return;
                    case 193: Info_193 = a_twinfo; return;
                    case 194: Info_194 = a_twinfo; return;
                    case 195: Info_195 = a_twinfo; return;
                    case 196: Info_196 = a_twinfo; return;
                    case 197: Info_197 = a_twinfo; return;
                    case 198: Info_198 = a_twinfo; return;
                    case 199: Info_199 = a_twinfo; return;
                }
            }
        }

        /// <summary>
        /// Provides information about the currently selected device.
        /// TBD -- need a 32/64 bit solution for this mess
        /// </summary>
        [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "ModifiedTimeDate")]
        [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "CreateTimeDate")]
        [StructLayout(LayoutKind.Explicit, Pack = 2)]
        public struct TW_FILESYSTEM
        {
            [FieldOffset(0)]
            public TW_STR255 InputName;

            [FieldOffset(256)]
            public TW_STR255 OutputName;

            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            [FieldOffset(512)]
            public IntPtr Context;

            [FieldOffset(520)]
            public Int32 Recursive;
            [FieldOffset(520)]
            public UInt16 Subdirectories;

            [FieldOffset(524)]
            public Int32 FileType;
            [FieldOffset(524)]
            public UInt32 FileSystemType;

            [FieldOffset(528)]
            public UInt32 Size;

            [FieldOffset(532)]
            public TW_STR32 CreateTimeDate;

            [FieldOffset(566)]
            public TW_STR32 ModifiedTimeDate;

            [FieldOffset(600)]
            public UInt32 FreeSpace;

            [FieldOffset(604)]
            public UInt32 NewImageSize;

            [FieldOffset(608)]
            public UInt32 NumberOfFiles;

            [FieldOffset(612)]
            public UInt32 NumberOfSnippets;

            [FieldOffset(616)]
            public UInt32 DeviceGroupMask;

            [FieldOffset(620)]
            public byte Reserved;

            [FieldOffset(1127)] // 620 + 508 - 1
            private byte ReservedEnd;
        }
        [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "ModifiedTimeDate")]
        [StructLayout(LayoutKind.Explicit, Pack = 2)]
        public struct TW_FILESYSTEM_LEGACY
        {
            [FieldOffset(0)]
            public TW_STR255 InputName;

            [FieldOffset(256)]
            public TW_STR255 OutputName;

            [FieldOffset(512)]
            public UInt32 Context;

            [FieldOffset(516)]
            public Int32 Recursive;
            [FieldOffset(516)]
            public UInt16 Subdirectories;

            [FieldOffset(520)]
            public Int32 FileType;
            [FieldOffset(520)]
            public UInt32 FileSystemType;

            [FieldOffset(524)]
            public UInt32 Size;

            [FieldOffset(528)]
            public TW_STR32 CreateTimeDate;

            [FieldOffset(562)]
            public TW_STR32 ModifiedTimeDate;

            [FieldOffset(596)]
            public UInt32 FreeSpace;

            [FieldOffset(600)]
            public UInt32 NewImageSize;

            [FieldOffset(604)]
            public UInt32 NumberOfFiles;

            [FieldOffset(608)]
            public UInt32 NumberOfSnippets;

            [FieldOffset(612)]
            public UInt32 DeviceGroupMask;

            [FieldOffset(616)]
            public byte Reserved;

            [FieldOffset(1123)] // 616 + 508 - 1
            private byte ReservedEnd;
        }

        /// <summary>
        /// This structure is used by the application to specify a set of mapping values to be applied to grayscale data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_GRAYRESPONSE
        {
            public TW_ELEMENT8 Response_00;
        }

        /// <summary>
        /// A general way to describe the version of software that is running.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_VERSION
        {
            public ushort MajorNum;
            public ushort MinorNum;
            public TWLG Language;
            public TWCY Country;
            public TW_STR32 Info;
        }

        /// <summary>
        /// Provides identification information about a TWAIN entity.
        /// The use of Padding is there to allow us to use the structure
        /// with Linux 64-bit systems where the TW_INT32 and TW_UINT32
        /// types were long, and therefore 64-bits in size.  This should
        /// have no impact with well-behaved systems that have these types
        /// as 32-bit, but should prevent memory corruption in all other
        /// situations...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_IDENTITY
        {
            public ulong Id;
            public TW_VERSION Version;
            public ushort ProtocolMajor;
            public ushort ProtocolMinor;
            public uint SupportedGroups;
            public TW_STR32 Manufacturer;
            public TW_STR32 ProductFamily;
            public TW_STR32 ProductName;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_IDENTITY_LEGACY
        {
            public uint Id;
            public TW_VERSION Version;
            public ushort ProtocolMajor;
            public ushort ProtocolMinor;
            public uint SupportedGroups;
            public TW_STR32 Manufacturer;
            public TW_STR32 ProductFamily;
            public TW_STR32 ProductName;
            private UInt64 Padding; // accounts for Id and SupportedGroups
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_IDENTITY_LINUX64
        {
            public ulong Id;
            public TW_VERSION Version;
            public ushort ProtocolMajor;
            public ushort ProtocolMinor;
            public ulong SupportedGroups;
            public TW_STR32 Manufacturer;
            public TW_STR32 ProductFamily;
            public TW_STR32 ProductName;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_IDENTITY_MACOSX
        {
            public uint Id;
            public TW_VERSION Version;
            public ushort ProtocolMajor;
            public ushort ProtocolMinor;
            private ushort padding;
            public uint SupportedGroups;
            public TW_STR32 Manufacturer;
            public TW_STR32 ProductFamily;
            public TW_STR32 ProductName;
        }

        /// <summary>
        /// Describes the “real” image data, that is, the complete image being transferred between the Source and application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_IMAGEINFO
        {
            public TW_FIX32 XResolution;
            public TW_FIX32 YResolution;
            public int ImageWidth;
            public int ImageLength;
            public short SamplesPerPixel;
            public short BitsPerSample_0;
            public short BitsPerSample_1;
            public short BitsPerSample_2;
            public short BitsPerSample_3;
            public short BitsPerSample_4;
            public short BitsPerSample_5;
            public short BitsPerSample_6;
            public short BitsPerSample_7;
            public short BitsPerPixel;
            public ushort Planar;
            public short PixelType;
            public ushort Compression;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_IMAGEINFO_LINUX64
        {
            public TW_FIX32 XResolution;
            public TW_FIX32 YResolution;
            public int ImageWidth;
            public int ImageLength;
            public short SamplesPerPixel;
            public short BitsPerSample_0;
            public short BitsPerSample_1;
            public short BitsPerSample_2;
            public short BitsPerSample_3;
            public short BitsPerSample_4;
            public short BitsPerSample_5;
            public short BitsPerSample_6;
            public short BitsPerSample_7;
            public short BitsPerPixel;
            public ushort Planar;
            public short PixelType;
            public ushort Compression;
        }

        /// <summary>
        /// Involves information about the original size of the acquired image.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_IMAGELAYOUT
        {
            public TW_FRAME Frame;
            public uint DocumentNumber;
            public uint PageNumber;
            public uint FrameNumber;
        }

        /// <summary>
        /// Provides information for managing memory buffers.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_MEMORY
        {
            public uint Flags;
            public uint Length;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr TheMem;
        }

        /// <summary>
        /// Describes the form of the acquired data being passed from the Source to the application.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_IMAGEMEMXFER
        {
            public ushort Compression;
            public uint BytesPerRow;
            public uint Columns;
            public uint Rows;
            public uint XOffset;
            public uint YOffset;
            public uint BytesWritten;
            public TW_MEMORY Memory;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_IMAGEMEMXFER_LINUX64
        {
            public ushort Compression;
            public UInt64 BytesPerRow;
            public UInt64 Columns;
            public UInt64 Rows;
            public UInt64 XOffset;
            public UInt64 YOffset;
            public UInt64 BytesWritten;
            public UInt64 MemoryFlags;
            public UInt64 MemoryLength;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr MemoryTheMem;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_IMAGEMEMXFER_MACOSX
        {
            public uint Compression;
            public uint BytesPerRow;
            public uint Columns;
            public uint Rows;
            public uint XOffset;
            public uint YOffset;
            public uint BytesWritten;
            public TW_MEMORY Memory;
        }

        /// <summary>
        /// Describes the information necessary to transfer a JPEG-compressed image.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_JPEGCOMPRESSION
        {
            public ushort ColorSpace;
            public uint SubSampling;
            public ushort NumComponents;
            public ushort QuantMap_0;
            public ushort QuantMap_1;
            public ushort QuantMap_2;
            public ushort QuantMap_3;
            public TW_MEMORY QuantTable_0;
            public TW_MEMORY QuantTable_1;
            public TW_MEMORY QuantTable_2;
            public TW_MEMORY QuantTable_3;
            public ushort HuffmanMap_0;
            public ushort HuffmanMap_1;
            public ushort HuffmanMap_2;
            public ushort HuffmanMap_3;
            public TW_MEMORY HuffmanDC_0;
            public TW_MEMORY HuffmanDC_1;
            public TW_MEMORY HuffmanAC_0;
            public TW_MEMORY HuffmanAC_2;
        }

        /// <summary>
        /// Collects scanning metrics after returning to state 4
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_METRICS
        {
            public uint SizeOf;
            public uint ImageCount;
            public uint SheetCount;
        }

        /// <summary>
        /// Stores a single value (item) which describes a capability.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_ONEVALUE
        {
            public TWTY ItemType;
            // public uint Item;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TW_ONEVALUE_MACOSX
        {
            public uint ItemType;
            // public uint Item;
        }

        /// <summary>
        /// This structure holds the color palette information.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_PALETTE8
        {
            public ushort Flags;
            public ushort Length;
            public TW_ELEMENT8 Colors_000;
        }

        /// <summary>
        /// Used to bypass the TWAIN protocol when communicating with a device.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_PASSTHRU
        {
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr pCommand;
            public uint CommandBytes;
            public int Direction;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr pData;
            public uint DataBytes;
            public uint DataBytesXfered;
        }

        /// <summary>
        /// This structure tells the application how many more complete transfers the Source currently has available.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_PENDINGXFERS
        {
            public ushort Count;
            public uint EOJ;
        }

        /// <summary>
        /// Stores a range of individual values describing a capability.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_RANGE
        {
            public TWTY ItemType;
            public uint MinValue;
            public uint MaxValue;
            public uint StepSize;
            public uint DefaultValue;
            public uint CurrentValue;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_RANGE_LINUX64
        {
            public TWTY ItemType;
            public ulong MinValue;
            public ulong MaxValue;
            public ulong StepSize;
            public ulong DefaultValue;
            public ulong CurrentValue;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TW_RANGE_MACOSX
        {
            public uint ItemType;
            public uint MinValue;
            public uint MaxValue;
            public uint StepSize;
            public uint DefaultValue;
            public uint CurrentValue;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private struct TW_RANGE_FIX32
        {
            public TWTY ItemType;
            public TW_FIX32 MinValue;
            public TW_FIX32 MaxValue;
            public TW_FIX32 StepSize;
            public TW_FIX32 DefaultValue;
            public TW_FIX32 CurrentValue;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct TW_RANGE_FIX32_MACOSX
        {
            public uint ItemType;
            public TW_FIX32 MinValue;
            public TW_FIX32 MaxValue;
            public TW_FIX32 StepSize;
            public TW_FIX32 DefaultValue;
            public TW_FIX32 CurrentValue;
        }

        /// <summary>
        /// This structure is used by the application to specify a set of mapping values to be applied to RGB color data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_RGBRESPONSE
        {
            public TW_ELEMENT8 Response_00;
        }

        /// <summary>
        /// Describes the file format and file specification information for a transfer through a disk file.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_SETUPFILEXFER
        {
            public TW_STR255 FileName;
            public TWFF Format;
            public short VRefNum;
        }

        /// <summary>
        /// Provides the application information about the Source’s requirements and preferences regarding allocation of transfer buffer(s).
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_SETUPMEMXFER
        {
            public uint MinBufSize;
            public uint MaxBufSize;
            public uint Preferred;
        }

        /// <summary>
        /// Describes the status of a source.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_STATUS
        {
            public ushort ConditionCode;
            public ushort Data;
        }

        /// <summary>
        /// Translates the contents of Status into a localized UTF8string.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_STATUSUTF8
        {
            public TW_STATUS Status;
            public uint Size;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr UTF8string;
        }

        /// <summary>
        /// Passthru for TWAIN Direct tasks.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_TWAINDIRECT
        {
            public uint SizeOf;
            public ushort CommunicationManager;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr Send;
            public uint SendSize;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr Receive;
            public uint ReceiveSize;
        }

        /// <summary>
        /// This structure is used to handle the user interface coordination between an application and a Source.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_USERINTERFACE
        {
            public ushort ShowUI;
            public ushort ModalUI;
            [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
            public IntPtr hParent;
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Generic Constants...
        ///////////////////////////////////////////////////////////////////////////////
        #region Generic Constants...

        /// <summary>
        /// Container Types...
        /// </summary>
        public enum TWON : ushort
        {
            ARRAY = 3,
            ENUMERATION = 4,
            ONEVALUE = 5,
            RANGE = 6,

            ICONID = 962,
            DSMID = 461,
            DSMCODEID = 63
        }

        /// <summary>
        /// Don't care values...
        /// </summary>
        public const byte TWON_DONTCARE8 = 0xff;
        public const ushort TWON_DONTCARE16 = 0xff;
        public const uint TWON_DONTCARE32 = 0xffffffff;

        /// <summary>
        /// Flags used in TW_MEMORY structure.
        /// </summary>
        public enum TWMF : ushort
        {
            APPOWNS = 0x0001,
            DSMOWNS = 0x0002,
            DSOWNS = 0x0004,
            POINTER = 0x0008,
            HANDLE = 0x0010
        }

        /// <summary>
        /// Type values...
        /// </summary>
        public enum TWTY : ushort
        {
            INT8 = 0x0000,
            INT16 = 0x0001,
            INT32 = 0x0002,

            UINT8 = 0x0003,
            UINT16 = 0x0004,
            UINT32 = 0x0005,

            BOOL = 0x0006,

            FIX32 = 0x0007,

            FRAME = 0x0008,

            STR32 = 0x0009,
            STR64 = 0x000a,
            STR128 = 0x000b,
            STR255 = 0x000c,
            HANDLE = 0x000f
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Capability Constants...
        ///////////////////////////////////////////////////////////////////////////////
        #region Capability Constants...

        /// <summary>
        /// CAP_ALARMS values
        /// </summary>
        public enum TWAL : ushort
        {
            ALARM = 0,
            FEEDERERROR = 1,
            FEEDERWARNING = 2,
            BARCODE = 3,
            DOUBLEFEED = 4,
            JAM = 5,
            PATCHCODE = 6,
            POWER = 7,
            SKEW = 8
        }

        /// <summary>
        /// ICAP_AUTOSIZE values
        /// </summary>
        public enum TWAS : ushort
        {
            NONE = 0,
            AUTO = 1,
            CURRENT = 2
        }

        /// <summary>
        /// TWEI_BARCODEROTATION values
        /// </summary>
        public enum TWBCOR : ushort
        {
            ROT0 = 0,
            ROT90 = 1,
            ROT180 = 2,
            ROT270 = 3,
            ROTX = 4
        }

        /// <summary>
        /// ICAP_BARCODESEARCHMODE values
        /// </summary>
        public enum TWBD : ushort
        {
            HORZ = 0,
            VERT = 1,
            HORZVERT = 2,
            VERTHORZ = 3
        }

        /// <summary>
        /// ICAP_BITORDER values
        /// </summary>
        public enum TWBO : ushort
        {
            LSBFIRST = 0,
            MSBFIRST = 1
        }

        /// <summary>
        /// ICAP_AUTODISCARDBLANKPAGES values
        /// </summary>
        public enum TWBP : short
        {
            DISABLE = -2,
            AUTO = -1
        }

        /// <summary>
        /// ICAP_BITDEPTHREDUCTION values
        /// </summary>
        public enum TWBR : ushort
        {
            THRESHOLD = 0,
            HALFTONE = 1,
            CUSTHALFTONE = 2,
            DIFFUSION = 3,
            DYNAMICTHRESHOLD = 4
        }

        /// <summary>
        /// ICAP_SUPPORTEDBARCODETYPES and TWEI_BARCODETYPE values
        /// </summary>
        public enum TWBT : ushort
        {
            X3OF9 = 0, // 3OF9 in TWAIN.H
            X2OF5INTERLEAVED = 1, // 2OF5INTERLEAVED in TWAIN.H
            X2OF5NONINTERLEAVED = 2, // 2OF5NONINTERLEAVED in TWAIN.H
            CODE93 = 3,
            CODE128 = 4,
            UCC128 = 5,
            CODABAR = 6,
            UPCA = 7,
            UPCE = 8,
            EAN8 = 9,
            EAN13 = 10,
            POSTNET = 11,
            PDF417 = 12,
            X2OF5INDUSTRIAL = 13, // 2OF5INDUSTRIAL in TWAIN.H
            X2OF5MATRIX = 14, // 2OF5MATRIX in TWAIN.H
            X2OF5DATALOGIC = 15, // 2OF5DATALOGIC in TWAIN.H
            X2OF5IATA = 16, // 2OF5IATA in TWAIN.H
            X3OF9FULLASCII = 17, // 3OF9FULLASCII in TWAIN.H
            CODABARWITHSTARTSTOP = 18,
            MAXICODE = 19,
            QRCODE = 20
        }

        /// <summary>
        /// ICAP_COMPRESSION values
        /// </summary>
        public enum TWCP : ushort
        {
            NONE = 0,
            PACKBITS = 1,
            GROUP31D = 2,
            GROUP31DEOL = 3,
            GROUP32D = 4,
            GROUP4 = 5,
            JPEG = 6,
            LZW = 7,
            JBIG = 8,
            PNG = 9,
            RLE4 = 10,
            RLE8 = 11,
            BITFIELDS = 12,
            ZIP = 13,
            JPEG2000 = 14
        }

        /// <summary>
        /// CAP_CAMERASIDE and TWEI_PAGESIDE values
        /// </summary>
        public enum TWCS : ushort
        {
            BOTH = 0,
            TOP = 1,
            BOTTOM = 2
        }

        /// <summary>
        /// CAP_CLEARBUFFERS values
        /// </summary>
        public enum TWCB : ushort
        {
            AUTO = 0,
            CLEAR = 1,
            NOCLEAR = 2
        }

        /// <summary>
        /// CAP_DEVICEEVENT values
        /// </summary>
        public enum TWDE : ushort
        {
            CUSTOMEVENTS = 0x8000,
            CHECKAUTOMATICCAPTURE = 0,
            CHECKBATTERY = 1,
            CHECKDEVICEONLINE = 2,
            CHECKFLASH = 3,
            CHECKPOWERSUPPLY = 4,
            CHECKRESOLUTION = 5,
            DEVICEADDED = 6,
            DEVICEOFFLINE = 7,
            DEVICEREADY = 8,
            DEVICEREMOVED = 9,
            IMAGECAPTURED = 10,
            IMAGEDELETED = 11,
            PAPERDOUBLEFEED = 12,
            PAPERJAM = 13,
            LAMPFAILURE = 14,
            POWERSAVE = 15,
            POWERSAVENOTIFY = 16
        }

        /// <summary>
        /// TW_PASSTHRU.Direction values
        /// </summary>
        public enum TWDR : ushort
        {
            GET = 1,
            SET = 2
        }

        /// <summary>
        /// TWEI_DESKEWSTATUS values
        /// </summary>
        public enum TWDSK : ushort
        {
            SUCCESS = 0,
            REPORTONLY = 1,
            FAIL = 2,
            DISABLED = 3
        }

        /// <summary>
        /// CAP_DUPLEX values
        /// </summary>
        public enum TWDX : ushort
        {
            NONE = 0,
            X1PASSDUPLEX = 1, // 1PASSDUPLEX in TWAIN.H
            X2PASSDUPLEX = 2  // 2PASSDUPLEX in TWAIN.H
        }

        /// <summary>
        /// CAP_FEEDERALIGNMENT values
        /// </summary>
        public enum TWFA : ushort
        {
            NONE = 0,
            LEFT = 1,
            CENTER = 2,
            RIGHT = 3
        }

        /// <summary>
        /// ICAP_FEEDERTYPE values
        /// </summary>
        public enum TWFE : ushort
        {
            GENERAL = 0,
            PHOTO = 1
        }

        /// <summary>
        /// ICAP_IMAGEFILEFORMAT values
        /// </summary>
        public enum TWFF : ushort
        {
            TIFF = 0,
            PICT = 1,
            BMP = 2,
            XBM = 3,
            JFIF = 4,
            FPX = 5,
            TIFFMULTI = 6,
            PNG = 7,
            SPIFF = 8,
            EXIF = 9,
            PDF = 10,
            JP2 = 11,
            JPX = 13,
            DEJAVU = 14,
            PDFA = 15,
            PDFA2 = 16,
            PDFRASTER = 17
        }

        /// <summary>
        /// ICAP_FLASHUSED2 values
        /// </summary>
        public enum TWFL : ushort
        {
            NONE = 0,
            OFF = 1,
            ON = 2,
            AUTO = 3,
            REDEYE = 4
        }

        /// <summary>
        /// CAP_FEEDERORDER values
        /// </summary>
        public enum TWFO : ushort
        {
            FIRSTPAGEFIRST = 0,
            LASTPAGEFIRST = 1
        }

        /// <summary>
        /// CAP_FEEDERPOCKET values
        /// </summary>
        public enum TWFP : ushort
        {
            POCKETERROR = 0,
            POCKET1 = 1,
            POCKET2 = 2,
            POCKET3 = 3,
            POCKET4 = 4,
            POCKET5 = 5,
            POCKET6 = 6,
            POCKET7 = 7,
            POCKET8 = 8,
            POCKET9 = 9,
            POCKET10 = 10,
            POCKET11 = 11,
            POCKET12 = 12,
            POCKET13 = 13,
            POCKET14 = 14,
            POCKET15 = 15,
            POCKET16 = 16
        }

        /// <summary>
        /// ICAP_FLIPROTATION values
        /// </summary>
        public enum TWFR : ushort
        {
            BOOK = 0,
            FANFOLD = 1
        }

        /// <summary>
        /// ICAP_FILTER values
        /// </summary>
        public enum TWFT : ushort
        {
            RED = 0,
            GREEN = 1,
            BLUE = 2,
            NONE = 3,
            WHITE = 4,
            CYAN = 5,
            MAGENTA = 6,
            YELLOW = 7,
            BLACK = 8
        }

        /// <summary>
        /// TW_FILESYSTEM.FileType values
        /// </summary>
        public enum TWFY : ushort
        {
            CAMERA = 0,
            CAMERATOP = 1,
            CAMERABOTTOM = 2,
            CAMERAPREVIEW = 3,
            DOMAIN = 4,
            HOST = 5,
            DIRECTORY = 6,
            IMAGE = 7,
            UNKNOWN = 8
        }

        /// <summary>
        /// ICAP_ICCPROFILE values
        /// </summary>
        public enum TWIC : ushort
        {
            NONE = 0,
            LINK = 1,
            EMBED = 2
        }

        /// <summary>
        /// ICAP_IMAGEFILTER values
        /// </summary>
        public enum TWIF : ushort
        {
            NONE = 0,
            AUTO = 1,
            LOWPASS = 2,
            BANDPASS = 3,
            HIGHPASS = 4,
            TEXT = BANDPASS,
            FINELINE = HIGHPASS
        }

        /// <summary>
        /// ICAP_IMAGEMERGE values
        /// </summary>
        public enum TWIM : ushort
        {
            NONE = 0,
            FRONTONTOP = 1,
            FRONTONBOTTOM = 2,
            FRONTONLEFT = 3,
            FRONTONRIGHT = 4
        }

        /// <summary>
        /// CAP_JOBCONTROL values
        /// </summary>
        public enum TWJC : ushort
        {
            NONE = 0,
            JSIC = 1,
            JSIS = 2,
            JSXC = 3,
            JSXS = 4
        }

        /// <summary>
        /// ICAP_JPEGQUALITY values
        /// </summary>
        public enum TWJQ : short
        {
            UNKNOWN = -4,
            LOW = -3,
            MEDIUM = -2,
            HIGH = -1
        }

        /// <summary>
        /// ICAP_LIGHTPATH values
        /// </summary>
        public enum TWLP : ushort
        {
            REFLECTIVE = 0,
            TRANSMISSIVE = 1
        }

        /// <summary>
        /// ICAP_LIGHTSOURCE values
        /// </summary>
        public enum TWLS : ushort
        {
            RED = 0,
            GREEN = 1,
            BLUE = 2,
            NONE = 3,
            WHITE = 4,
            UV = 5,
            IR = 6
        }

        /// <summary>
        /// TWEI_MAGTYPE values
        /// </summary>
        public enum TWMD : ushort
        {
            MICR = 0,
            RAW = 1,
            INVALID = 2
        }

        /// <summary>
        /// ICAP_NOISEFILTER values
        /// </summary>
        public enum TWNF : ushort
        {
            NONE = 0,
            AUTO = 1,
            LONEPIXEL = 2,
            MAJORITYRULE = 3
        }

        /// <summary>
        /// ICAP_ORIENTATION values
        /// </summary>
        public enum TWOR : ushort
        {
            ROT0 = 0,
            ROT90 = 1,
            ROT180 = 2,
            ROT270 = 3,
            PORTRAIT = ROT0,
            LANDSCAPE = ROT270,
            AUTO = 4,
            AUTOTEXT = 5,
            AUTOPICTURE = 6
        }

        /// <summary>
        /// ICAP_OVERSCAN values
        /// </summary>
        public enum TWOV : ushort
        {
            NONE = 0,
            AUTO = 1,
            TOPBOTTOM = 2,
            LEFTRIGHT = 3,
            ALL = 4
        }

        /// <summary>
        /// Palette types for TW_PALETTE8
        /// </summary>
        public enum TWPA : ushort
        {
            RGB = 0,
            GRAY = 1,
            CMY = 2
        }

        /// <summary>
        /// ICAP_PLANARCHUNKY values
        /// </summary>
        public enum TWPC : ushort
        {
            CHUNKY = 0,
            PLANAR = 1
        }

        /// <summary>
        /// TWEI_PATCHCODE values
        /// </summary>
        public enum TWPCH : ushort
        {
            PATCH1 = 0,
            PATCH2 = 1,
            PATCH3 = 2,
            PATCH4 = 3,
            PATCH6 = 4,
            PATCHT = 5
        }

        /// <summary>
        /// ICAP_PIXELFLAVOR values
        /// </summary>
        public enum TWPF : ushort
        {
            CHOCOLATE = 0,
            VANILLA = 1
        }

        /// <summary>
        /// CAP_PRINTERMODE values
        /// </summary>
        public enum TWPM : ushort
        {
            SINGLESTRING = 0,
            MULTISTRING = 1,
            COMPOUNDSTRING = 2
        }

        /// <summary>
        /// CAP_PRINTER values
        /// </summary>
        public enum TWPR : ushort
        {
            IMPRINTERTOPBEFORE = 0,
            IMPRINTERTOPAFTER = 1,
            IMPRINTERBOTTOMBEFORE = 2,
            IMPRINTERBOTTOMAFTER = 3,
            ENDORSERTOPBEFORE = 4,
            ENDORSERTOPAFTER = 5,
            ENDORSERBOTTOMBEFORE = 6,
            ENDORSERBOTTOMAFTER = 7
        }

        /// <summary>
        /// CAP_PRINTERFONTSTYLE Added 2.3 (TWPF in TWAIN.H)
        /// </summary>
        public enum TWPFS : ushort
        {
            NORMAL = 0,
            BOLD = 1,
            ITALIC = 2,
            LARGESIZE = 3,
            SMALLSIZE = 4
        }

        /// <summary>
        /// CAP_PRINTERINDEXTRIGGER Added 2.3
        /// </summary>
        public enum TWCT : ushort
        {
            PAGE = 0,
            PATCH1 = 1,
            PATCH2 = 2,
            PATCH3 = 3,
            PATCH4 = 4,
            PATCHT = 5,
            PATCH6 = 6
        }

        /// <summary>
        /// CAP_POWERSUPPLY values
        /// </summary>
        public enum TWPS : ushort
        {
            EXTERNAL = 0,
            BATTERY = 1
        }

        /// <summary>
        /// ICAP_PIXELTYPE values (PT_ means Pixel Type)
        /// </summary>
        public enum TWPT : ushort
        {
            BW = 0,
            GRAY = 1,
            RGB = 2,
            PALETTE = 3,
            CMY = 4,
            CMYK = 5,
            YUV = 6,
            YUVK = 7,
            CIEXYZ = 8,
            LAB = 9,
            SRGB = 10,
            SCRGB = 11,
            INFRARED = 16
        }

        /// <summary>
        /// CAP_SEGMENTED values
        /// </summary>
        public enum TWSG : ushort
        {
            NONE = 0,
            AUTO = 1,
            MANUAL = 2
        }

        /// <summary>
        /// ICAP_FILMTYPE values
        /// </summary>
        public enum TWFM : ushort
        {
            POSITIVE = 0,
            NEGATIVE = 1
        }

        /// <summary>
        /// CAP_DOUBLEFEEDDETECTION values
        /// </summary>
        public enum TWDF : ushort
        {
            ULTRASONIC = 0,
            BYLENGTH = 1,
            INFRARED = 2
        }

        /// <summary>
        /// CAP_DOUBLEFEEDDETECTIONSENSITIVITY values
        /// </summary>
        public enum TWUS : ushort
        {
            LOW = 0,
            MEDIUM = 1,
            HIGH = 2
        }

        /// <summary>
        /// CAP_DOUBLEFEEDDETECTIONRESPONSE values
        /// </summary>
        public enum TWDP : ushort
        {
            STOP = 0,
            STOPANDWAIT = 1,
            SOUND = 2,
            DONOTIMPRINT = 3
        }

        /// <summary>
        /// ICAP_MIRROR values
        /// </summary>
        public enum TWMR : ushort
        {
            NONE = 0,
            VERTICAL = 1,
            HORIZONTAL = 2
        }

        /// <summary>
        /// ICAP_JPEGSUBSAMPLING values
        /// </summary>
        public enum TWJS : ushort
        {
            X444YCBCR = 0, // 444YCBCR in TWAIN.H
            X444RGB = 1, // 444RGB in TWAIN.H
            X422 = 2, // 422 in TWAIN.H
            X421 = 3, // 421 in TWAIN.H
            X411 = 4, // 411 in TWAIN.H
            X420 = 5, // 420 in TWAIN.H
            X410 = 6, // 410 in TWAIN.H
            X311 = 7  // 311 in TWAIN.H
        }

        /// <summary>
        /// CAP_PAPERHANDLING values
        /// </summary>
        public enum TWPH : ushort
        {
            NORMAL = 0,
            FRAGILE = 1,
            THICK = 2,
            TRIFOLD = 3,
            PHOTOGRAPH = 4
        }

        /// <summary>
        /// CAP_INDICATORSMODE values
        /// </summary>
        public enum TWCI : ushort
        {
            INFO = 0,
            WARNING = 1,
            ERROR = 2,
            WARMUP = 3
        }

        /// <summary>
        /// ICAP_SUPPORTEDSIZES values (SS_ means Supported Sizes)
        /// </summary>
        public enum TWSS : ushort
        {
            NONE = 0,
            A4 = 1,
            JISB5 = 2,
            USLETTER = 3,
            USLEGAL = 4,
            A5 = 5,
            ISOB4 = 6,
            ISOB6 = 7,
            USLEDGER = 9,
            USEXECUTIVE = 10,
            A3 = 11,
            ISOB3 = 12,
            A6 = 13,
            C4 = 14,
            C5 = 15,
            C6 = 16,
            X4A0 = 17, // 4A0 in TWAIN.H
            X2A0 = 18, // 2A0 in TWAIN.H
            A0 = 19,
            A1 = 20,
            A2 = 21,
            A7 = 22,
            A8 = 23,
            A9 = 24,
            A10 = 25,
            ISOB0 = 26,
            ISOB1 = 27,
            ISOB2 = 28,
            ISOB5 = 29,
            ISOB7 = 30,
            ISOB8 = 31,
            ISOB9 = 32,
            ISOB10 = 33,
            JISB0 = 34,
            JISB1 = 35,
            JISB2 = 36,
            JISB3 = 37,
            JISB4 = 38,
            JISB6 = 39,
            JISB7 = 40,
            JISB8 = 41,
            JISB9 = 42,
            JISB10 = 43,
            C0 = 44,
            C1 = 45,
            C2 = 46,
            C3 = 47,
            C7 = 48,
            C8 = 49,
            C9 = 50,
            C10 = 51,
            USSTATEMENT = 52,
            BUSINESSCARD = 53,
            MAXSIZE = 54
        }

        /// <summary>
        /// ICAP_XFERMECH values (SX_ means Setup XFer)
        /// </summary>
        public enum TWSX : ushort
        {
            NATIVE = 0,
            FILE = 1,
            MEMORY = 2,
            MEMFILE = 4
        }

        /// <summary>
        /// ICAP_UNITS values (UN_ means UNits)
        /// </summary>
        public enum TWUN : ushort
        {
            INCHES = 0,
            CENTIMETERS = 1,
            PICAS = 2,
            POINTS = 3,
            TWIPS = 4,
            PIXELS = 5,
            MILLIMETERS = 6
        }

        /// <summary>
        /// Country Constants
        /// </summary>
        public enum TWCY : ushort
        {
            AFGHANISTAN = 1001,
            ALGERIA = 213,
            AMERICANSAMOA = 684,
            ANDORRA = 33,
            ANGOLA = 1002,
            ANGUILLA = 8090,
            ANTIGUA = 8091,
            ARGENTINA = 54,
            ARUBA = 297,
            ASCENSIONI = 247,
            AUSTRALIA = 61,
            AUSTRIA = 43,
            BAHAMAS = 8092,
            BAHRAIN = 973,
            BANGLADESH = 880,
            BARBADOS = 8093,
            BELGIUM = 32,
            BELIZE = 501,
            BENIN = 229,
            BERMUDA = 8094,
            BHUTAN = 1003,
            BOLIVIA = 591,
            BOTSWANA = 267,
            BRITAIN = 6,
            BRITVIRGINIS = 8095,
            BRAZIL = 55,
            BRUNEI = 673,
            BULGARIA = 359,
            BURKINAFASO = 1004,
            BURMA = 1005,
            BURUNDI = 1006,
            CAMAROON = 237,
            CANADA = 2,
            CAPEVERDEIS = 238,
            CAYMANIS = 8096,
            CENTRALAFREP = 1007,
            CHAD = 1008,
            CHILE = 56,
            CHINA = 86,
            CHRISTMASIS = 1009,
            COCOSIS = 1009,
            COLOMBIA = 57,
            COMOROS = 1010,
            CONGO = 1011,
            COOKIS = 1012,
            COSTARICA = 506,
            CUBA = 5,
            CYPRUS = 357,
            CZECHOSLOVAKIA = 42,
            DENMARK = 45,
            DJIBOUTI = 1013,
            DOMINICA = 8097,
            DOMINCANREP = 8098,
            EASTERIS = 1014,
            ECUADOR = 593,
            EGYPT = 20,
            ELSALVADOR = 503,
            EQGUINEA = 1015,
            ETHIOPIA = 251,
            FALKLANDIS = 1016,
            FAEROEIS = 298,
            FIJIISLANDS = 679,
            FINLAND = 358,
            FRANCE = 33,
            FRANTILLES = 596,
            FRGUIANA = 594,
            FRPOLYNEISA = 689,
            FUTANAIS = 1043,
            GABON = 241,
            GAMBIA = 220,
            GERMANY = 49,
            GHANA = 233,
            GIBRALTER = 350,
            GREECE = 30,
            GREENLAND = 299,
            GRENADA = 8099,
            GRENEDINES = 8015,
            GUADELOUPE = 590,
            GUAM = 671,
            GUANTANAMOBAY = 5399,
            GUATEMALA = 502,
            GUINEA = 224,
            GUINEABISSAU = 1017,
            GUYANA = 592,
            HAITI = 509,
            HONDURAS = 504,
            HONGKONG = 852,
            HUNGARY = 36,
            ICELAND = 354,
            INDIA = 91,
            INDONESIA = 62,
            IRAN = 98,
            IRAQ = 964,
            IRELAND = 353,
            ISRAEL = 972,
            ITALY = 39,
            IVORYCOAST = 225,
            JAMAICA = 8010,
            JAPAN = 81,
            JORDAN = 962,
            KENYA = 254,
            KIRIBATI = 1018,
            KOREA = 82,
            KUWAIT = 965,
            LAOS = 1019,
            LEBANON = 1020,
            LIBERIA = 231,
            LIBYA = 218,
            LIECHTENSTEIN = 41,
            LUXENBOURG = 352,
            MACAO = 853,
            MADAGASCAR = 1021,
            MALAWI = 265,
            MALAYSIA = 60,
            MALDIVES = 960,
            MALI = 1022,
            MALTA = 356,
            MARSHALLIS = 692,
            MAURITANIA = 1023,
            MAURITIUS = 230,
            MEXICO = 3,
            MICRONESIA = 691,
            MIQUELON = 508,
            MONACO = 33,
            MONGOLIA = 1024,
            MONTSERRAT = 8011,
            MOROCCO = 212,
            MOZAMBIQUE = 1025,
            NAMIBIA = 264,
            NAURU = 1026,
            NEPAL = 977,
            NETHERLANDS = 31,
            NETHANTILLES = 599,
            NEVIS = 8012,
            NEWCALEDONIA = 687,
            NEWZEALAND = 64,
            NICARAGUA = 505,
            NIGER = 227,
            NIGERIA = 234,
            NIUE = 1027,
            NORFOLKI = 1028,
            NORWAY = 47,
            OMAN = 968,
            PAKISTAN = 92,
            PALAU = 1029,
            PANAMA = 507,
            PARAGUAY = 595,
            PERU = 51,
            PHILLIPPINES = 63,
            PITCAIRNIS = 1030,
            PNEWGUINEA = 675,
            POLAND = 48,
            PORTUGAL = 351,
            QATAR = 974,
            REUNIONI = 1031,
            ROMANIA = 40,
            RWANDA = 250,
            SAIPAN = 670,
            SANMARINO = 39,
            SAOTOME = 1033,
            SAUDIARABIA = 966,
            SENEGAL = 221,
            SEYCHELLESIS = 1034,
            SIERRALEONE = 1035,
            SINGAPORE = 65,
            SOLOMONIS = 1036,
            SOMALI = 1037,
            SOUTHAFRICA = 27,
            SPAIN = 34,
            SRILANKA = 94,
            STHELENA = 1032,
            STKITTS = 8013,
            STLUCIA = 8014,
            STPIERRE = 508,
            STVINCENT = 8015,
            SUDAN = 1038,
            SURINAME = 597,
            SWAZILAND = 268,
            SWEDEN = 46,
            SWITZERLAND = 41,
            SYRIA = 1039,
            TAIWAN = 886,
            TANZANIA = 255,
            THAILAND = 66,
            TOBAGO = 8016,
            TOGO = 228,
            TONGAIS = 676,
            TRINIDAD = 8016,
            TUNISIA = 216,
            TURKEY = 90,
            TURKSCAICOS = 8017,
            TUVALU = 1040,
            UGANDA = 256,
            USSR = 7,
            UAEMIRATES = 971,
            UNITEDKINGDOM = 44,
            USA = 1,
            URUGUAY = 598,
            VANUATU = 1041,
            VATICANCITY = 39,
            VENEZUELA = 58,
            WAKE = 1042,
            WALLISIS = 1043,
            WESTERNSAHARA = 1044,
            WESTERNSAMOA = 1045,
            YEMEN = 1046,
            YUGOSLAVIA = 38,
            ZAIRE = 243,
            ZAMBIA = 260,
            ZIMBABWE = 263,
            ALBANIA = 355,
            ARMENIA = 374,
            AZERBAIJAN = 994,
            BELARUS = 375,
            BOSNIAHERZGO = 387,
            CAMBODIA = 855,
            CROATIA = 385,
            CZECHREPUBLIC = 420,
            DIEGOGARCIA = 246,
            ERITREA = 291,
            ESTONIA = 372,
            GEORGIA = 995,
            LATVIA = 371,
            LESOTHO = 266,
            LITHUANIA = 370,
            MACEDONIA = 389,
            MAYOTTEIS = 269,
            MOLDOVA = 373,
            MYANMAR = 95,
            NORTHKOREA = 850,
            PUERTORICO = 787,
            RUSSIA = 7,
            SERBIA = 381,
            SLOVAKIA = 421,
            SLOVENIA = 386,
            SOUTHKOREA = 82,
            UKRAINE = 380,
            USVIRGINIS = 340,
            VIETNAM = 84
        }

        /// <summary>
        /// Language Constants
        /// </summary>
        public enum TWLG : short
        {
            USERLOCALE = -1,
            DAN = 0,
            DUT = 1,
            ENG = 2,
            FCF = 3,
            FIN = 4,
            FRN = 5,
            GER = 6,
            ICE = 7,
            ITN = 8,
            NOR = 9,
            POR = 10,
            SPA = 11,
            SWE = 12,
            USA = 13,
            AFRIKAANS = 14,
            ALBANIA = 15,
            ARABIC = 16,
            ARABIC_ALGERIA = 17,
            ARABIC_BAHRAIN = 18,
            ARABIC_EGYPT = 19,
            ARABIC_IRAQ = 20,
            ARABIC_JORDAN = 21,
            ARABIC_KUWAIT = 22,
            ARABIC_LEBANON = 23,
            ARABIC_LIBYA = 24,
            ARABIC_MOROCCO = 25,
            ARABIC_OMAN = 26,
            ARABIC_QATAR = 27,
            ARABIC_SAUDIARABIA = 28,
            ARABIC_SYRIA = 29,
            ARABIC_TUNISIA = 30,
            ARABIC_UAE = 31,
            ARABIC_YEMEN = 32,
            BASQUE = 33,
            BYELORUSSIAN = 34,
            BULGARIAN = 35,
            CATALAN = 36,
            CHINESE = 37,
            CHINESE_HONGKONG = 38,
            CHINESE_PRC = 39,
            CHINESE_SINGAPORE = 40,
            CHINESE_SIMPLIFIED = 41,
            CHINESE_TAIWAN = 42,
            CHINESE_TRADITIONAL = 43,
            CROATIA = 44,
            CZECH = 45,
            DANISH = DAN,
            DUTCH = DUT,
            DUTCH_BELGIAN = 46,
            ENGLISH = ENG,
            ENGLISH_AUSTRALIAN = 47,
            ENGLISH_CANADIAN = 48,
            ENGLISH_IRELAND = 49,
            ENGLISH_NEWZEALAND = 50,
            ENGLISH_SOUTHAFRICA = 51,
            ENGLISH_UK = 52,
            ENGLISH_USA = USA,
            ESTONIAN = 53,
            FAEROESE = 54,
            FARSI = 55,
            FINNISH = FIN,
            FRENCH = FRN,
            FRENCH_BELGIAN = 56,
            FRENCH_CANADIAN = FCF,
            FRENCH_LUXEMBOURG = 57,
            FRENCH_SWISS = 58,
            GERMAN = GER,
            GERMAN_AUSTRIAN = 59,
            GERMAN_LUXEMBOURG = 60,
            GERMAN_LIECHTENSTEIN = 61,
            GERMAN_SWISS = 62,
            GREEK = 63,
            HEBREW = 64,
            HUNGARIAN = 65,
            ICELANDIC = ICE,
            INDONESIAN = 66,
            ITALIAN = ITN,
            ITALIAN_SWISS = 67,
            JAPANESE = 68,
            KOREAN = 69,
            KOREAN_JOHAB = 70,
            LATVIAN = 71,
            LITHUANIAN = 72,
            NORWEGIAN = NOR,
            NORWEGIAN_BOKMAL = 73,
            NORWEGIAN_NYNORSK = 74,
            POLISH = 75,
            PORTUGUESE = POR,
            PORTUGUESE_BRAZIL = 76,
            ROMANIAN = 77,
            RUSSIAN = 78,
            SERBIAN_LATIN = 79,
            SLOVAK = 80,
            SLOVENIAN = 81,
            SPANISH = TWLG.SPA,
            SPANISH_MEXICAN = 82,
            SPANISH_MODERN = 83,
            SWEDISH = TWLG.SWE,
            THAI = 84,
            TURKISH = 85,
            UKRANIAN = 86,
            ASSAMESE = 87,
            BENGALI = 88,
            BIHARI = 89,
            BODO = 90,
            DOGRI = 91,
            GUJARATI = 92,
            HARYANVI = 93,
            HINDI = 94,
            KANNADA = 95,
            KASHMIRI = 96,
            MALAYALAM = 97,
            MARATHI = 98,
            MARWARI = 99,
            MEGHALAYAN = 100,
            MIZO = 101,
            NAGA = 102,
            ORISSI = 103,
            PUNJABI = 104,
            PUSHTU = 105,
            SERBIAN_CYRILLIC = 106,
            SIKKIMI = 107,
            SWEDISH_FINLAND = 108,
            TAMIL = 109,
            TELUGU = 110,
            TRIPURI = 111,
            URDU = 112,
            VIETNAMESE = 113
            //NOTE: when adding to this list, also update Language->Set()
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Data Groups...
        ///////////////////////////////////////////////////////////////////////////////
        #region Data Groups...

        /// <summary>
        /// Data Groups...
        /// </summary>
        public enum DG : uint
        {
            CONTROL = 0x1,
            IMAGE = 0x2,
            AUDIO = 0x4,

            // More Data Functionality may be added in the future.
            // These are for items that need to be determined before DS is opened.
            // NOTE: Supported Functionality constants must be powers of 2 as they are
            //       used as bitflags when Application asks DSM to present a list of DSs.
            //       to support backward capability the App and DS will not use the fields
            DSM2 = 0x10000000,
            APP2 = 0x20000000,
            DS2 = 0x40000000,
            MASK = 0xFFFF
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Data Argument Types...
        ///////////////////////////////////////////////////////////////////////////////
        #region Data Argument Types...

        /// <summary>
        /// Data Argument Types...
        /// </summary>
        public enum DAT : ushort
        {
            // NULL and Custom Base...
            NULL = 0x0,
            CUSTOM = 0x8000,

            // Data Argument Types for the DG_CONTROL Data Group.
            CAPABILITY = 0x1,
            EVENT = 0x2,
            IDENTITY = 0x3,
            PARENT = 0x4,
            PENDINGXFERS = 0x5,
            SETUPMEMXFER = 0x6,
            SETUPFILEXFER = 0x7,
            STATUS = 0x8,
            USERINTERFACE = 0x9,
            XFERGROUP = 0xa,
            CUSTOMDSDATA = 0xc,
            DEVICEEVENT = 0xd,
            FILESYSTEM = 0xe,
            PASSTHRU = 0xf,
            CALLBACK = 0x10,
            STATUSUTF8 = 0x11,
            CALLBACK2 = 0x12,
            METRICS = 0x13,
            TWAINDIRECT = 0x14,

            // Data Argument Types for the DG_IMAGE Data Group.
            IMAGEINFO = 0x0101,
            IMAGELAYOUT = 0x0102,
            IMAGEMEMXFER = 0x0103,
            IMAGENATIVEXFER = 0x0104,
            IMAGEFILEXFER = 0x105,
            CIECOLOR = 0x106,
            GRAYRESPONSE = 0x107,
            RGBRESPONSE = 0x108,
            JPEGCOMPRESSION = 0x109,
            PALETTE8 = 0x10a,
            EXTIMAGEINFO = 0x10b,
            FILTER = 0x10c,

            /* Data Argument Types for the DG_AUDIO Data Group. */
            AUDIOFILEXFER = 0x201,
            AUDIOINFO = 0x202,
            AUDIONATIVEXFER = 0x203,

            /* misplaced */
            ICCPROFILE = 0x401,
            IMAGEMEMFILEXFER = 0x402,
            ENTRYPOINT = 0x403
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Messages...
        ///////////////////////////////////////////////////////////////////////////////
        #region Messages...

        /// <summary>
        /// All message constants are unique.
        /// Messages are grouped according to which DATs they are used with.
        /// </summary>
        public enum MSG : ushort
        {
            // Only used to clear fields...
            NULL = 0x0,

            // Generic messages may be used with any of several DATs.
            GET = 0x1,
            GETCURRENT = 0x2,
            GETDEFAULT = 0x3,
            GETFIRST = 0x4,
            GETNEXT = 0x5,
            SET = 0x6,
            RESET = 0x7,
            QUERYSUPPORT = 0x8,
            GETHELP = 0x9,
            GETLABEL = 0xa,
            GETLABELENUM = 0xb,
            SETCONSTRAINT = 0xc,

            // Messages used with DAT_NULL.
            XFERREADY = 0x101,
            CLOSEDSREQ = 0x102,
            CLOSEDSOK = 0x103,
            DEVICEEVENT = 0x104,

            // Messages used with a pointer to DAT_PARENT data.
            OPENDSM = 0x301,
            CLOSEDSM = 0x302,

            // Messages used with a pointer to a DAT_IDENTITY structure.
            OPENDS = 0x401,
            CLOSEDS = 0x402,
            USERSELECT = 0x403,

            // Messages used with a pointer to a DAT_USERINTERFACE structure.
            DISABLEDS = 0x501,
            ENABLEDS = 0x502,
            ENABLEDSUIONLY = 0x503,

            // Messages used with a pointer to a DAT_EVENT structure.
            PROCESSEVENT = 0x601,

            // Messages used with a pointer to a DAT_PENDINGXFERS structure
            ENDXFER = 0x701,
            STOPFEEDER = 0x702,

            // Messages used with a pointer to a DAT_FILESYSTEM structure
            CHANGEDIRECTORY = 0x0801,
            CREATEDIRECTORY = 0x0802,
            DELETE = 0x0803,
            FORMATMEDIA = 0x0804,
            GETCLOSE = 0x0805,
            GETFIRSTFILE = 0x0806,
            GETINFO = 0x0807,
            GETNEXTFILE = 0x0808,
            RENAME = 0x0809,
            COPY = 0x080A,
            AUTOMATICCAPTUREDIRECTORY = 0x080B,

            // Messages used with a pointer to a DAT_PASSTHRU structure
            PASSTHRU = 0x0901,

            // used with DAT_CALLBACK
            REGISTER_CALLBACK = 0x0902,

            // used with DAT_CAPABILITY
            RESETALL = 0x0A01,

            // used with DAT_TWAINDIRECT
            SETTASK = 0x0B01
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Capabilities...
        ///////////////////////////////////////////////////////////////////////////////
        #region Capabilities...

        /// <summary>
        /// The naming convention is a little awkward, but it allows us to
        /// achieve a unified capability type...
        /// </summary>
        public enum CAP : ushort
        {
            // Base of custom capabilities.
            CAP_CUSTOMBASE = 0x8000,

            /* all data sources are REQUIRED to support these caps */
            CAP_XFERCOUNT = 0x0001,

            /* image data sources are REQUIRED to support these caps */
            ICAP_COMPRESSION = 0x0100,
            ICAP_PIXELTYPE = 0x0101,
            ICAP_UNITS = 0x0102,
            ICAP_XFERMECH = 0x0103,

            // all data sources MAY support these caps.
            CAP_AUTHOR = 0x1000,
            CAP_CAPTION = 0x1001,
            CAP_FEEDERENABLED = 0x1002,
            CAP_FEEDERLOADED = 0x1003,
            CAP_TIMEDATE = 0x1004,
            CAP_SUPPORTEDCAPS = 0x1005,
            CAP_EXTENDEDCAPS = 0x1006,
            CAP_AUTOFEED = 0x1007,
            CAP_CLEARPAGE = 0x1008,
            CAP_FEEDPAGE = 0x1009,
            CAP_REWINDPAGE = 0x100a,
            CAP_INDICATORS = 0x100b,
            CAP_PAPERDETECTABLE = 0x100d,
            CAP_UICONTROLLABLE = 0x100e,
            CAP_DEVICEONLINE = 0x100f,
            CAP_AUTOSCAN = 0x1010,
            CAP_THUMBNAILSENABLED = 0x1011,
            CAP_DUPLEX = 0x1012,
            CAP_DUPLEXENABLED = 0x1013,
            CAP_ENABLEDSUIONLY = 0x1014,
            CAP_CUSTOMDSDATA = 0x1015,
            CAP_ENDORSER = 0x1016,
            CAP_JOBCONTROL = 0x1017,
            CAP_ALARMS = 0x1018,
            CAP_ALARMVOLUME = 0x1019,
            CAP_AUTOMATICCAPTURE = 0x101a,
            CAP_TIMEBEFOREFIRSTCAPTURE = 0x101b,
            CAP_TIMEBETWEENCAPTURES = 0x101c,
            CAP_CLEARBUFFERS = 0x101d,
            CAP_MAXBATCHBUFFERS = 0x101e,
            CAP_DEVICETIMEDATE = 0x101f,
            CAP_POWERSUPPLY = 0x1020,
            CAP_CAMERAPREVIEWUI = 0x1021,
            CAP_DEVICEEVENT = 0x1022,
            CAP_SERIALNUMBER = 0x1024,
            CAP_PRINTER = 0x1026,
            CAP_PRINTERENABLED = 0x1027,
            CAP_PRINTERINDEX = 0x1028,
            CAP_PRINTERMODE = 0x1029,
            CAP_PRINTERSTRING = 0x102a,
            CAP_PRINTERSUFFIX = 0x102b,
            CAP_LANGUAGE = 0x102c,
            CAP_FEEDERALIGNMENT = 0x102d,
            CAP_FEEDERORDER = 0x102e,
            CAP_REACQUIREALLOWED = 0x1030,
            CAP_BATTERYMINUTES = 0x1032,
            CAP_BATTERYPERCENTAGE = 0x1033,
            CAP_CAMERASIDE = 0x1034,
            CAP_SEGMENTED = 0x1035,
            CAP_CAMERAENABLED = 0x1036,
            CAP_CAMERAORDER = 0x1037,
            CAP_MICRENABLED = 0x1038,
            CAP_FEEDERPREP = 0x1039,
            CAP_FEEDERPOCKET = 0x103a,
            CAP_AUTOMATICSENSEMEDIUM = 0x103b,
            CAP_CUSTOMINTERFACEGUID = 0x103c,
            CAP_SUPPORTEDCAPSSEGMENTUNIQUE = 0x103d,
            CAP_SUPPORTEDDATS = 0x103e,
            CAP_DOUBLEFEEDDETECTION = 0x103f,
            CAP_DOUBLEFEEDDETECTIONLENGTH = 0x1040,
            CAP_DOUBLEFEEDDETECTIONSENSITIVITY = 0x1041,
            CAP_DOUBLEFEEDDETECTIONRESPONSE = 0x1042,
            CAP_PAPERHANDLING = 0x1043,
            CAP_INDICATORSMODE = 0x1044,
            CAP_PRINTERVERTICALOFFSET = 0x1045,
            CAP_POWERSAVETIME = 0x1046,
            CAP_PRINTERCHARROTATION = 0x1047,
            CAP_PRINTERFONTSTYLE = 0x1048,
            CAP_PRINTERINDEXLEADCHAR = 0x1049,
            CAP_PRINTERINDEXMAXVALUE = 0x104A,
            CAP_PRINTERINDEXNUMDIGITS = 0x104B,
            CAP_PRINTERINDEXSTEP = 0x104C,
            CAP_PRINTERINDEXTRIGGER = 0x104D,
            CAP_PRINTERSTRINGPREVIEW = 0x104E,
            CAP_SHEETCOUNT = 0x104F,

            // image data sources MAY support these caps.
            ICAP_AUTOBRIGHT = 0x1100,
            ICAP_BRIGHTNESS = 0x1101,
            ICAP_CONTRAST = 0x1103,
            ICAP_CUSTHALFTONE = 0x1104,
            ICAP_EXPOSURETIME = 0x1105,
            ICAP_FILTER = 0x1106,
            ICAP_FLASHUSED = 0x1107,
            ICAP_GAMMA = 0x1108,
            ICAP_HALFTONES = 0x1109,
            ICAP_HIGHLIGHT = 0x110a,
            ICAP_IMAGEFILEFORMAT = 0x110c,
            ICAP_LAMPSTATE = 0x110d,
            ICAP_LIGHTSOURCE = 0x110e,
            ICAP_ORIENTATION = 0x1110,
            ICAP_PHYSICALWIDTH = 0x1111,
            ICAP_PHYSICALHEIGHT = 0x1112,
            ICAP_SHADOW = 0x1113,
            ICAP_FRAMES = 0x1114,
            ICAP_XNATIVERESOLUTION = 0x1116,
            ICAP_YNATIVERESOLUTION = 0x1117,
            ICAP_XRESOLUTION = 0x1118,
            ICAP_YRESOLUTION = 0x1119,
            ICAP_MAXFRAMES = 0x111a,
            ICAP_TILES = 0x111b,
            ICAP_BITORDER = 0x111c,
            ICAP_CCITTKFACTOR = 0x111d,
            ICAP_LIGHTPATH = 0x111e,
            ICAP_PIXELFLAVOR = 0x111f,
            ICAP_PLANARCHUNKY = 0x1120,
            ICAP_ROTATION = 0x1121,
            ICAP_SUPPORTEDSIZES = 0x1122,
            ICAP_THRESHOLD = 0x1123,
            ICAP_XSCALING = 0x1124,
            ICAP_YSCALING = 0x1125,
            ICAP_BITORDERCODES = 0x1126,
            ICAP_PIXELFLAVORCODES = 0x1127,
            ICAP_JPEGPIXELTYPE = 0x1128,
            ICAP_TIMEFILL = 0x112a,
            ICAP_BITDEPTH = 0x112b,
            ICAP_BITDEPTHREDUCTION = 0x112c,
            ICAP_UNDEFINEDIMAGESIZE = 0x112d,
            ICAP_IMAGEDATASET = 0x112e,
            ICAP_EXTIMAGEINFO = 0x112f,
            ICAP_MINIMUMHEIGHT = 0x1130,
            ICAP_MINIMUMWIDTH = 0x1131,
            ICAP_AUTODISCARDBLANKPAGES = 0x1134,
            ICAP_FLIPROTATION = 0x1136,
            ICAP_BARCODEDETECTIONENABLED = 0x1137,
            ICAP_SUPPORTEDBARCODETYPES = 0x1138,
            ICAP_BARCODEMAXSEARCHPRIORITIES = 0x1139,
            ICAP_BARCODESEARCHPRIORITIES = 0x113a,
            ICAP_BARCODESEARCHMODE = 0x113b,
            ICAP_BARCODEMAXRETRIES = 0x113c,
            ICAP_BARCODETIMEOUT = 0x113d,
            ICAP_ZOOMFACTOR = 0x113e,
            ICAP_PATCHCODEDETECTIONENABLED = 0x113f,
            ICAP_SUPPORTEDPATCHCODETYPES = 0x1140,
            ICAP_PATCHCODEMAXSEARCHPRIORITIES = 0x1141,
            ICAP_PATCHCODESEARCHPRIORITIES = 0x1142,
            ICAP_PATCHCODESEARCHMODE = 0x1143,
            ICAP_PATCHCODEMAXRETRIES = 0x1144,
            ICAP_PATCHCODETIMEOUT = 0x1145,
            ICAP_FLASHUSED2 = 0x1146,
            ICAP_IMAGEFILTER = 0x1147,
            ICAP_NOISEFILTER = 0x1148,
            ICAP_OVERSCAN = 0x1149,
            ICAP_AUTOMATICBORDERDETECTION = 0x1150,
            ICAP_AUTOMATICDESKEW = 0x1151,
            ICAP_AUTOMATICROTATE = 0x1152,
            ICAP_JPEGQUALITY = 0x1153,
            ICAP_FEEDERTYPE = 0x1154,
            ICAP_ICCPROFILE = 0x1155,
            ICAP_AUTOSIZE = 0x1156,
            ICAP_AUTOMATICCROPUSESFRAME = 0x1157,
            ICAP_AUTOMATICLENGTHDETECTION = 0x1158,
            ICAP_AUTOMATICCOLORENABLED = 0x1159,
            ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE = 0x115a,
            ICAP_COLORMANAGEMENTENABLED = 0x115b,
            ICAP_IMAGEMERGE = 0x115c,
            ICAP_IMAGEMERGEHEIGHTTHRESHOLD = 0x115d,
            ICAP_SUPPORTEDEXTIMAGEINFO = 0x115e,
            ICAP_FILMTYPE = 0x115f,
            ICAP_MIRROR = 0x1160,
            ICAP_JPEGSUBSAMPLING = 0x1161,

            // image data sources MAY support these audio caps.
            ACAP_XFERMECH = 0x1202
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Extended Image Info Attributes section  Added 1.7...
        ///////////////////////////////////////////////////////////////////////////////
        #region Extended Image Info Attributes section  Added 1.7...

        /// <summary>
        /// Extended Image Info Attributes...
        /// </summary>
        public enum TWEI : ushort
        {
            BARCODEX = 0x1200,
            BARCODEY = 0x1201,
            BARCODETEXT = 0x1202,
            BARCODETYPE = 0x1203,
            DESHADETOP = 0x1204,
            DESHADELEFT = 0x1205,
            DESHADEHEIGHT = 0x1206,
            DESHADEWIDTH = 0x1207,
            DESHADESIZE = 0x1208,
            SPECKLESREMOVED = 0x1209,
            HORZLINEXCOORD = 0x120A,
            HORZLINEYCOORD = 0x120B,
            HORZLINELENGTH = 0x120C,
            HORZLINETHICKNESS = 0x120D,
            VERTLINEXCOORD = 0x120E,
            VERTLINEYCOORD = 0x120F,
            VERTLINELENGTH = 0x1210,
            VERTLINETHICKNESS = 0x1211,
            PATCHCODE = 0x1212,
            ENDORSEDTEXT = 0x1213,
            FORMCONFIDENCE = 0x1214,
            FORMTEMPLATEMATCH = 0x1215,
            FORMTEMPLATEPAGEMATCH = 0x1216,
            FORMHORZDOCOFFSET = 0x1217,
            FORMVERTDOCOFFSET = 0x1218,
            BARCODECOUNT = 0x1219,
            BARCODECONFIDENCE = 0x121A,
            BARCODEROTATION = 0x121B,
            BARCODETEXTLENGTH = 0x121C,
            DESHADECOUNT = 0x121D,
            DESHADEBLACKCOUNTOLD = 0x121E,
            DESHADEBLACKCOUNTNEW = 0x121F,
            DESHADEBLACKRLMIN = 0x1220,
            DESHADEBLACKRLMAX = 0x1221,
            DESHADEWHITECOUNTOLD = 0x1222,
            DESHADEWHITECOUNTNEW = 0x1223,
            DESHADEWHITERLMIN = 0x1224,
            DESHADEWHITERLAVE = 0x1225,
            DESHADEWHITERLMAX = 0x1226,
            BLACKSPECKLESREMOVED = 0x1227,
            WHITESPECKLESREMOVED = 0x1228,
            HORZLINECOUNT = 0x1229,
            VERTLINECOUNT = 0x122A,
            DESKEWSTATUS = 0x122B,
            SKEWORIGINALANGLE = 0x122C,
            SKEWFINALANGLE = 0x122D,
            SKEWCONFIDENCE = 0x122E,
            SKEWWINDOWX1 = 0x122F,
            SKEWWINDOWY1 = 0x1230,
            SKEWWINDOWX2 = 0x1231,
            SKEWWINDOWY2 = 0x1232,
            SKEWWINDOWX3 = 0x1233,
            SKEWWINDOWY3 = 0x1234,
            SKEWWINDOWX4 = 0x1235,
            SKEWWINDOWY4 = 0x1236,
            BOOKNAME = 0x1238,
            CHAPTERNUMBER = 0x1239,
            DOCUMENTNUMBER = 0x123A,
            PAGENUMBER = 0x123B,
            CAMERA = 0x123C,
            FRAMENUMBER = 0x123D,
            FRAME = 0x123E,
            PIXELFLAVOR = 0x123F,
            ICCPROFILE = 0x1240,
            LASTSEGMENT = 0x1241,
            SEGMENTNUMBER = 0x1242,
            MAGDATA = 0x1243,
            MAGTYPE = 0x1244,
            PAGESIDE = 0x1245,
            FILESYSTEMSOURCE = 0x1246,
            IMAGEMERGED = 0x1247,
            MAGDATALENGTH = 0x1248,
            PAPERCOUNT = 0x1249,
            PRINTERTEXT = 0x124A,
            TWAINDIRECTMETADATA = 0x124B
        }

        public enum TWEJ : ushort
        {
            NONE = 0x0000,
            MIDSEPARATOR = 0x0001,
            PATCH1 = 0x0002,
            PATCH2 = 0x0003,
            PATCH3 = 0x0004,
            PATCH4 = 0x0005,
            PATCH6 = 0x0006,
            PATCHT = 0x0007
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Return Codes and Condition Codes section...
        ///////////////////////////////////////////////////////////////////////////////
        #region Return Codes and Condition Codes section...

        /// <summary>
        /// We're departing from a strict translation of TWAIN.H so that
        /// we can achieve a unified status return type.  
        /// </summary>
        public const int STSCC = 0x10000; // get us past the custom space
        public enum STS
        {
            // Custom base (same for TWRC and TWCC)...
            CUSTOMBASE = 0x8000,

            // Return codes...
            SUCCESS = 0,
            FAILURE = 1,
            CHECKSTATUS = 2,
            CANCEL = 3,
            DSEVENT = 4,
            NOTDSEVENT = 5,
            XFERDONE = 6,
            ENDOFLIST = 7,
            INFONOTSUPPORTED = 8,
            DATANOTAVAILABLE = 9,
            BUSY = 10,
            SCANNERLOCKED = 11,

            // Condition codes (always associated with TWRC_FAILURE)...
            BUMMER = STSCC + 1,
            LOWMEMORY = STSCC + 2,
            NODS = STSCC + 3,
            MAXCONNECTIONS = STSCC + 4,
            OPERATIONERROR = STSCC + 5,
            BADCAP = STSCC + 6,
            BADPROTOCOL = STSCC + 9,
            BADVALUE = STSCC + 10,
            SEQERROR = STSCC + 11,
            BADDEST = STSCC + 12,
            CAPUNSUPPORTED = STSCC + 13,
            CAPBADOPERATION = STSCC + 14,
            CAPSEQERROR = STSCC + 15,
            DENIED = STSCC + 16,
            FILEEXISTS = STSCC + 17,
            FILENOTFOUND = STSCC + 18,
            NOTEMPTY = STSCC + 19,
            PAPERJAM = STSCC + 20,
            PAPERDOUBLEFEED = STSCC + 21,
            FILEWRITEERROR = STSCC + 22,
            CHECKDEVICEONLINE = STSCC + 23,
            INTERLOCK = STSCC + 24,
            DAMAGEDCORNER = STSCC + 25,
            FOCUSERROR = STSCC + 26,
            DOCTOOLIGHT = STSCC + 27,
            DOCTOODARK = STSCC + 28,
            NOMEDIA = STSCC + 29
        }

        /// <summary>
        /// bit patterns: for query the operation that are supported by the data source on a capability
        /// Application gets these through DG_CONTROL/DAT_CAPABILITY/MSG_QUERYSUPPORT
        /// </summary>
        public enum TWQC : ushort
        {
            GET = 0x0001,
            SET = 0x0002,
            GETDEFAULT = 0x0004,
            GETCURRENT = 0x0008,
            RESET = 0x0010,
            SETCONSTRAINT = 0x0020,
            CONSTRAINABLE = 0x0040
        }

        /// <summary>
        /// The TWAIN States...
        /// </summary>
        public enum STATE
        {
            S1 = 1, // Nothing loaded or open
            S2 = 2, // DSM loaded
            S3 = 3, // DSM open
            S4 = 4, // Data Source open, programmatic mode (no GUI)
            S5 = 5, // GUI up or waiting to transfer first image
            S6 = 6, // ready to start transferring image
            S7 = 7  // transferring image or transfer done
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Language section...
        ///////////////////////////////////////////////////////////////////////////////
        #region Language section...

        /// <summary>
        /// Handle encoding between C# and what the DS is currently set to.
        /// NOTE: this is static for users of this object do not have to track
        ///       the encoding (i.e. let TWAIN.cs deal with all of this). This
        ///       means there is one language for all open DSes, so the last one wins.
        /// </summary>
        private static class Language
        {
            /// <summary>
            /// The encoding to use for strings to/from the DS
            /// </summary>
            /// <returns></returns>
            public static Encoding GetEncoding()
            {
                return (m_encoding);
            }

            /// <summary>
            /// The current language of the DS
            /// </summary>
            /// <returns></returns>
            public static void Set(TWLG a_twlg)
            {
                switch (a_twlg)
                {
                    default:
                        // NOTE: can only get here if a TWLG was added, but it wasn't added here
                        m_encoding = Encoding.GetEncoding(1252);
                        break;

                    case TWLG.USERLOCALE:
                        // NOTE: this should never come back from the DS. only here for completeness
                        m_encoding = Encoding.GetEncoding(1252);
                        break;

                    case TWLG.THAI:
                        m_encoding = Encoding.GetEncoding(874);
                        break;

                    case TWLG.JAPANESE:
                        m_encoding = Encoding.GetEncoding(932);
                        break;

                    case TWLG.CHINESE:
                    case TWLG.CHINESE_PRC:
                    case TWLG.CHINESE_SINGAPORE:
                    case TWLG.CHINESE_SIMPLIFIED:
                        m_encoding = Encoding.GetEncoding(936);
                        break;

                    case TWLG.KOREAN:
                    case TWLG.KOREAN_JOHAB:
                        m_encoding = Encoding.GetEncoding(949);
                        break;

                    case TWLG.CHINESE_HONGKONG:
                    case TWLG.CHINESE_TAIWAN:
                    case TWLG.CHINESE_TRADITIONAL:
                        m_encoding = Encoding.GetEncoding(950);
                        break;

                    case TWLG.ALBANIA:
                    case TWLG.CROATIA:
                    case TWLG.CZECH:
                    case TWLG.HUNGARIAN:
                    case TWLG.POLISH:
                    case TWLG.ROMANIAN:
                    case TWLG.SERBIAN_LATIN:
                    case TWLG.SLOVAK:
                    case TWLG.SLOVENIAN:
                        m_encoding = Encoding.GetEncoding(1250);
                        break;

                    case TWLG.BYELORUSSIAN:
                    case TWLG.BULGARIAN:
                    case TWLG.RUSSIAN:
                    case TWLG.SERBIAN_CYRILLIC:
                    case TWLG.UKRANIAN:
                        m_encoding = Encoding.GetEncoding(1251);
                        break;

                    case TWLG.AFRIKAANS:
                    case TWLG.BASQUE:
                    case TWLG.CATALAN:
                    case TWLG.DAN: // DANISH
                    case TWLG.DUT: // DUTCH
                    case TWLG.DUTCH_BELGIAN:
                    case TWLG.ENG: // ENGLISH
                    case TWLG.ENGLISH_AUSTRALIAN:
                    case TWLG.ENGLISH_CANADIAN:
                    case TWLG.ENGLISH_IRELAND:
                    case TWLG.ENGLISH_NEWZEALAND:
                    case TWLG.ENGLISH_SOUTHAFRICA:
                    case TWLG.ENGLISH_UK:
                    case TWLG.USA:
                    case TWLG.FAEROESE:
                    case TWLG.FIN: // FINNISH
                    case TWLG.FRN: // FRENCH
                    case TWLG.FRENCH_BELGIAN:
                    case TWLG.FCF: // FRENCH_CANADIAN
                    case TWLG.FRENCH_LUXEMBOURG:
                    case TWLG.FRENCH_SWISS:
                    case TWLG.GER: // GERMAN
                    case TWLG.GERMAN_AUSTRIAN:
                    case TWLG.GERMAN_LIECHTENSTEIN:
                    case TWLG.GERMAN_LUXEMBOURG:
                    case TWLG.GERMAN_SWISS:
                    case TWLG.ICE: // ICELANDIC
                    case TWLG.INDONESIAN:
                    case TWLG.ITN: // ITALIAN
                    case TWLG.ITALIAN_SWISS:
                    case TWLG.NOR: // NORWEGIAN
                    case TWLG.NORWEGIAN_BOKMAL:
                    case TWLG.NORWEGIAN_NYNORSK:
                    case TWLG.POR: // PORTUGUESE
                    case TWLG.PORTUGUESE_BRAZIL:
                    case TWLG.SPA: // SPANISH
                    case TWLG.SPANISH_MEXICAN:
                    case TWLG.SPANISH_MODERN:
                    case TWLG.SWE: // SWEDISH
                    case TWLG.SWEDISH_FINLAND:
                        m_encoding = Encoding.GetEncoding(1252);
                        break;

                    case TWLG.GREEK:
                        m_encoding = Encoding.GetEncoding(1253);
                        break;

                    case TWLG.TURKISH:
                        m_encoding = Encoding.GetEncoding(1254);
                        break;

                    case TWLG.HEBREW:
                        m_encoding = Encoding.GetEncoding(1255);
                        break;

                    case TWLG.ARABIC:
                    case TWLG.ARABIC_ALGERIA:
                    case TWLG.ARABIC_BAHRAIN:
                    case TWLG.ARABIC_EGYPT:
                    case TWLG.ARABIC_IRAQ:
                    case TWLG.ARABIC_JORDAN:
                    case TWLG.ARABIC_KUWAIT:
                    case TWLG.ARABIC_LEBANON:
                    case TWLG.ARABIC_LIBYA:
                    case TWLG.ARABIC_MOROCCO:
                    case TWLG.ARABIC_OMAN:
                    case TWLG.ARABIC_QATAR:
                    case TWLG.ARABIC_SAUDIARABIA:
                    case TWLG.ARABIC_SYRIA:
                    case TWLG.ARABIC_TUNISIA:
                    case TWLG.ARABIC_UAE:
                    case TWLG.ARABIC_YEMEN:
                    case TWLG.FARSI:
                    case TWLG.URDU:
                        m_encoding = Encoding.GetEncoding(1256);
                        break;

                    case TWLG.ESTONIAN:
                    case TWLG.LATVIAN:
                    case TWLG.LITHUANIAN:
                        m_encoding = Encoding.GetEncoding(1257);
                        break;

                    case TWLG.VIETNAMESE:
                        m_encoding = Encoding.GetEncoding(1258);
                        break;

                    case TWLG.ASSAMESE:
                    case TWLG.BENGALI:
                    case TWLG.BIHARI:
                    case TWLG.BODO:
                    case TWLG.DOGRI:
                    case TWLG.GUJARATI:
                    case TWLG.HARYANVI:
                    case TWLG.HINDI:
                    case TWLG.KANNADA:
                    case TWLG.KASHMIRI:
                    case TWLG.MALAYALAM:
                    case TWLG.MARATHI:
                    case TWLG.MARWARI:
                    case TWLG.MEGHALAYAN:
                    case TWLG.MIZO:
                    case TWLG.NAGA:
                    case TWLG.ORISSI:
                    case TWLG.PUNJABI:
                    case TWLG.PUSHTU:
                    case TWLG.SIKKIMI:
                    case TWLG.TAMIL:
                    case TWLG.TELUGU:
                    case TWLG.TRIPURI:
                        // NOTE: there are no known code pages for these, so we will use English
                        m_encoding = Encoding.GetEncoding(1252);
                        break;
                }
            }

            private static Encoding m_encoding = Encoding.GetEncoding(1252);
        }

        #endregion

    }


    /// <summary>
    /// All of our DllImports live here...
    /// </summary>
    internal sealed class NativeMethods
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Windows
        ///////////////////////////////////////////////////////////////////////////////
        #region Windows

        /// <summary>
        /// Get the ID for the current thread...
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern uint GetCurrentThreadId();

        /// <summary>
        /// Allocate a handle to memory...
        /// </summary>
        /// <param name="uFlags"></param>
        /// <param name="dwBytes"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        /// <summary>
        /// Free a memory handle...
        /// </summary>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GlobalFree(IntPtr hMem);

        /// <summary>
        /// Lock a memory handle...
        /// </summary>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GlobalLock(IntPtr hMem);

        /// <summary>
        /// Unlock a memory handle...
        /// </summary>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        internal static extern UIntPtr GlobalSize(IntPtr hMem);

        [DllImport("msvcrt.dll")]
        internal static extern UIntPtr _msize(IntPtr ptr);

        [DllImport("libc.so")]
        internal static extern UIntPtr malloc_usable_size(IntPtr ptr);

        [DllImport("libSystem.dylib")]
        internal static extern UIntPtr malloc_size(IntPtr ptr);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        internal static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        [DllImport("libc", EntryPoint = "memcpy", SetLastError = false)]
        internal static extern void memcpy(IntPtr dest, IntPtr src, IntPtr count);

        [DllImport("kernel32.dll", EntryPoint = "MoveMemory", SetLastError = false)]
        internal static extern void MoveMemory(IntPtr dest, IntPtr src, uint count);

        [DllImport("libc", EntryPoint = "memmove", SetLastError = false)]
        internal static extern void memmove(IntPtr dest, IntPtr src, IntPtr count);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern Int32 _wfopen_s(out IntPtr pFile, string filename, string mode);

        [DllImport("libc", CharSet = CharSet.Ansi, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr fopen([MarshalAs(UnmanagedType.LPStr)] string filename, [MarshalAs(UnmanagedType.LPStr)] string mode);

        [DllImport("msvcrt.dll", EntryPoint = "fwrite", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern IntPtr fwriteWin(IntPtr buffer, IntPtr size, IntPtr number, IntPtr file);

        [DllImport("libc", EntryPoint = "fwrite", SetLastError = true)]
        internal static extern IntPtr fwrite(IntPtr buffer, IntPtr size, IntPtr number, IntPtr file);

        [DllImport("msvcrt.dll", EntryPoint = "fclose", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern IntPtr fcloseWin(IntPtr file);

        [DllImport("libc", EntryPoint = "fclose", SetLastError = true)]
        internal static extern IntPtr fclose(IntPtr file);

        #endregion


        // We're supporting every DSM that we can...

        /// <summary>
        /// Use this entry for generic access to the DSM where the
        /// destination must be IntPtr.Zero (null)...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="memref"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryNullDest
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryNullDest
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryNullDest
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryNullDest
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryNullDest
        (
            ref TWAIN.TW_IDENTITY origin,
            IntPtr zero,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryNullDest
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            IntPtr zero,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryNullDest
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            IntPtr zero,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );


        /// <summary>
        /// Use for generic access to the DSM where the destination must
        /// reference a data source...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="memref"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntry
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntry
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntry
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntry
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntry
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntry
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntry
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );


        /// <summary>
        /// Use this for DG_AUDIO / DAT.AUDIOFILEXFER / MSG.GET calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="memref"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryAudioAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryAudioAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryAudioAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryAudioAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryAudioAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryAudioAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryAudioAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr memref
        );

        /// <summary>
        /// Use this for DG_AUDIO / DAT.AUDIOINFO / MSG.GET calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twaudioinfo"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryAudioAudioinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_AUDIOINFO twaudioinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryAudioAudioinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_AUDIOINFO twaudioinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryAudioAudioinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_AUDIOINFO twaudioinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryAudioAudioinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_AUDIOINFO twaudioinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryAudioAudioinfo
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_AUDIOINFO twaudioinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryAudioAudioinfo
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_AUDIOINFO twaudioinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryAudioAudioinfo
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_AUDIOINFO twaudioinfo
        );

        /// <summary>
        /// Use this for DG_AUDIO / DAT.AUDIONATIVEXFER / MSG.GET...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="hWav"></param>
        /// <returns></returns>
        /// *** We'll add this later...maybe***

        /// <summary>
        /// Use this for DG_CONTROL / DAT.CALLBACK / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twcallback"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryCallback
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK twcallback
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryCallback
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK twcallback
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryCallback
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK twcallback
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryCallback
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK twcallback
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryCallback
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK twcallback
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryCallback
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK twcallback
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryCallback
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK twcallback
        );
        public delegate UInt16 WindowsDsmEntryCallbackDelegate
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twnull
        );
        public delegate UInt16 LinuxDsmEntryCallbackDelegate
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twnull
        );
        public delegate UInt16 Linux020302Dsm64bitEntryCallbackDelegate
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twnull
        );
        public delegate UInt16 MacosxDsmEntryCallbackDelegate
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twnull
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.CALLBACK2 / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twcallback"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryCallback2
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK2 twcallback2
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryCallback2
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK2 twcallback2
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryCallback2
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK2 twcallback2
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryCallback2
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK2 twcallback2
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryCallback2
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK2 twcallback2
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryCallback2
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK2 twcallback
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryCallback2
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY des,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CALLBACK2 twcallback
        );
        private delegate UInt16 WindowsDsmEntryCallback2Delegate
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twnull
        );
        private delegate UInt16 LinuxDsmEntryCallback2Delegate
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twnull
        );
        private delegate UInt16 Linux020302Dsm64bitEntryCallback2Delegate
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twnull
        );
        private delegate UInt16 MacosxDsmEntryCallback2Delegate
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twnull
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.CAPABILITY / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twcapability"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryCapability
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CAPABILITY twcapability
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryCapability
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CAPABILITY twcapability
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryCapability
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CAPABILITY twcapability
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryCapability
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CAPABILITY twcapability
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryCapability
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CAPABILITY twcapability
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryCapability
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CAPABILITY twcapability
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryCapability
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CAPABILITY twcapability
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.CUSTOMDSDATA / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twcustomdsdata"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryCustomdsdata
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CUSTOMDSDATA twcustomedsdata
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryCustomdsdata
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CUSTOMDSDATA twcustomdsdata
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryCustomdsdata
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CUSTOMDSDATA twcustomdsdata
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryCustomdsdata
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CUSTOMDSDATA twcustomdsdata
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryCustomdsdata
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CUSTOMDSDATA twcustomdsdata
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryCustomdsdata
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CUSTOMDSDATA twcustomedsdata
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryCustomdsdata
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CUSTOMDSDATA twcustomedsdata
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.DEVICEEVENT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twdeviceevent"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryDeviceevent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_DEVICEEVENT twdeviceevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryDeviceevent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_DEVICEEVENT twdeviceevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryDeviceevent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_DEVICEEVENT twdeviceevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryDeviceevent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_DEVICEEVENT twdeviceevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryDeviceevent
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_DEVICEEVENT twdeviceevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryDeviceevent
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_DEVICEEVENT twdeviceevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryDeviceevent
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_DEVICEEVENT twdeviceevent
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.EVENT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twevent"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryEvent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EVENT twevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryEvent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EVENT twevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryEvent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EVENT twevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryEvent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EVENT twevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryEvent
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EVENT twevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryEvent
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EVENT twevent
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryEvent
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EVENT twevent
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.ENTRYPOINT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twentrypoint"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryEntrypoint
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_ENTRYPOINT twentrypoint
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryEntrypoint
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_ENTRYPOINT twentrypoint
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryEntrypoint
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_ENTRYPOINT twentrypoint
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryEntrypoint
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_ENTRYPOINT twentrypoint
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryEntrypoint
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_ENTRYPOINT_LINUX64 twentrypoint
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryEntrypoint
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_ENTRYPOINT twentrypoint
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryEntrypoint
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_ENTRYPOINT twentrypoint
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.FILESYSTEM / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twentrypoint"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryFilesystem
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILESYSTEM twfilesystem
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryFilesystem
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILESYSTEM twfilesystem
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryFilesystem
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILESYSTEM twfilesystem
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryFilesystem
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILESYSTEM twfilesystem
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryFilesystem
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILESYSTEM twfilesystem
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryFilesystem
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILESYSTEM twfilesystem
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryFilesystem
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILESYSTEM twfilesystem
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.IDENTITY / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twidentity"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryIdentity
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IDENTITY_LEGACY twidentity
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryIdentityState4
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IDENTITY_LEGACY twidentity
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryIdentity
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IDENTITY_LEGACY twidentity
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryIdentity
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IDENTITY_LEGACY twidentity
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryIdentity
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IDENTITY_LEGACY twidentity
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryIdentity
        (
            ref TWAIN.TW_IDENTITY_LINUX64 origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IDENTITY_LINUX64 twidentity
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryIdentity
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IDENTITY_MACOSX twidentity
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryIdentity
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IDENTITY_MACOSX twidentity
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.NULL / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="memref"></param>
        /// <returns></returns>
        /// ***Only needed for drivers, so we don't have it***

        /// <summary>
        /// Use this for DG_CONTROL / DAT.PARENT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="hbitmap"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryParent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr hwnd
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryParent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr hwnd
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryParent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr hwnd
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryParent
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr hwnd
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryParent
        (
            ref TWAIN.TW_IDENTITY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr hwnd
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryParent
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr hwnd
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryParent
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr hwnd
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.PASSTHRU / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twpassthru"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryPassthru
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PASSTHRU twpassthru
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryPassthru
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PASSTHRU twpassthru
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryPassthru
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PASSTHRU twpassthru
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryPassthru
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PASSTHRU twpassthru
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryPassthru
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PASSTHRU twpassthru
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryPassthru
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PASSTHRU twpassthru
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryPassthru
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PASSTHRU twpassthru
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.PENDINGXFERS / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twpendingxfers"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryPendingxfers
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PENDINGXFERS twpendingxfers
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryPendingxfers
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PENDINGXFERS twpendingxfers
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryPendingxfers
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PENDINGXFERS twpendingxfers
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryPendingxfers
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PENDINGXFERS twpendingxfers
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryPendingxfers
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PENDINGXFERS twpendingxfers
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryPendingxfers
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PENDINGXFERS twpendingxfers
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryPendingxfers
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PENDINGXFERS twpendingxfers
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.SETUPFILEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twsetupfilexfer"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntrySetupfilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPFILEXFER twsetupfilexfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntrySetupfilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPFILEXFER twsetupfilexfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntrySetupfilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPFILEXFER twsetupfilexfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntrySetupfilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPFILEXFER twsetupfilexfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntrySetupfilexfer
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPFILEXFER twsetupfilexfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntrySetupfilexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPFILEXFER twsetupfilexfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntrySetupfilexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPFILEXFER twsetupfilexfer
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.SETUPMEMXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twsetupmemxfer"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntrySetupmemxfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPMEMXFER twsetupmemxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntrySetupmemxfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPMEMXFER twsetupmemxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntrySetupmemxfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPMEMXFER twsetupmemxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntrySetupmemxfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPMEMXFER twsetupmemxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntrySetupmemxfer
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPMEMXFER twsetupmemxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntrySetupmemxfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPMEMXFER twsetupmemxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntrySetupmemxfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_SETUPMEMXFER twsetupmemxfer
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.STATUS / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twstatus"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryStatus
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryStatusState3
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryStatus
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryStatusState3
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryStatus
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryStatusState3
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryStatus
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryStatusState3
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryStatus
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryStatusState3
        (
            ref TWAIN.TW_IDENTITY origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryStatus
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryStatusState3
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryStatus
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryStatusState3
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUS twstatus
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.STATUSUTF8 / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twstatusutf8"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryStatusutf8
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUSUTF8 twstatusutf8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryStatusutf8
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUSUTF8 twstatusutf8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryStatusutf8
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUSUTF8 twstatusutf8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryStatusutf8
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUSUTF8 twstatusutf8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryStatusutf8
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUSUTF8 twstatusutf8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryStatusutf8
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUSUTF8 twstatusutf8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryStatusutf8
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_STATUSUTF8 twstatusutf8
        );

        /// <summary>
        /// Use this for DG.CONTROL / DAT.TWAINDIRECT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twtwaindirect"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryTwaindirect
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_TWAINDIRECT twtwaindirect
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryTwaindirect
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_TWAINDIRECT twtwaindirect
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryTwaindirect
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_TWAINDIRECT twtwaindirect
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryTwaindirect
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_TWAINDIRECT twtwaindirect
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryTwaindirect
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_TWAINDIRECT twtwaindirect
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryTwaindirect
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_TWAINDIRECT twtwaindirect
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryTwaindirect
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_TWAINDIRECT twtwaindirect
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.USERINTERFACE / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twuserinterface"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryUserinterface
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_USERINTERFACE twuserinterface
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryUserinterface
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_USERINTERFACE twuserinterface
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryUserinterface
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_USERINTERFACE twuserinterface
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryUserinterface
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_USERINTERFACE twuserinterface
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryUserinterface
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_USERINTERFACE twuserinterface
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryUserinterface
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_USERINTERFACE twuserinterface
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryUserinterface
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_USERINTERFACE twuserinterface
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.XFERGROUP / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twuint32"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryXfergroup
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref UInt32 twuint32
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryXfergroup
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref UInt32 twuint32
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryXfergroup
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref UInt32 twuint32
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryXfergroup
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref UInt32 twuint32
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryXfergroup
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref UInt32 twuint32
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryXfergroup
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref UInt32 twuint32
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryXfergroup
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref UInt32 twuint32
        );

        /// <summary>
        /// Use this for DG_AUDIO / DAT.AUDIOFILEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twmemref"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryAudiofilexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );

        /// <summary>
        /// Use this for DG_AUDIO / DAT.AUDIONATIVEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="intptr"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryAudionativexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrWav
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryAudionativexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrWav
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryAudionativexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrWav
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryAudionativexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrWav
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryAudionativexfer
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrWav
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryAudionativexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrAiff
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryAudionativexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrAiff
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.CIECOLOR / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twciecolor"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryCiecolor
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CIECOLOR twciecolor
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryCiecolor
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CIECOLOR twciecolor
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryCiecolor
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CIECOLOR twciecolor
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryCiecolor
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CIECOLOR twciecolor
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryCiecolor
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CIECOLOR twciecolor
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryCiecolor
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CIECOLOR twciecolor
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryCiecolor
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_CIECOLOR twciecolor
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.EXTIMAGEINFO / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twextimageinfo"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryExtimageinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EXTIMAGEINFO twextimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryExtimageinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EXTIMAGEINFO twextimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryExtimageinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EXTIMAGEINFO twextimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryExtimageinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EXTIMAGEINFO twextimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryExtimageinfo
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EXTIMAGEINFO twextimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryExtimageinfo
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EXTIMAGEINFO twextimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryExtimageinfo
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_EXTIMAGEINFO twextimageinfo
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.FILTER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twfilter"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryFilter
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILTER twfilter
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryFilter
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILTER twfilter
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryFilter
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILTER twfilter
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryFilter
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILTER twfilter
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryFilter
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILTER twfilter
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryFilter
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILTER twfilter
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryFilter
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_FILTER twfilter
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.GRAYRESPONSE / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twgrayresponse"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryGrayresponse
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_GRAYRESPONSE twgrayresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryGrayresponse
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_GRAYRESPONSE twgrayresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryGrayresponse
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_GRAYRESPONSE twgrayresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryGrayresponse
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_GRAYRESPONSE twgrayresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryGrayresponse
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_GRAYRESPONSE twgrayresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryGrayresponse
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_GRAYRESPONSE twgrayresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryGrayresponse
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_GRAYRESPONSE twgrayresponse
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.ICCPROFILE / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twmemory"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryIccprofile
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_MEMORY twmemory
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryIccprofile
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_MEMORY twmemory
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryIccprofile
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_MEMORY twmemory
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryIccprofile
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_MEMORY twmemory
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryIccprofile
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_MEMORY twmemory
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryIccprofile
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_MEMORY twmemory
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryIccprofile
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_MEMORY twmemory
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGEFILEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twmemref"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImagefilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImagefilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImagefilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImagefilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImagefilexfer
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImagefilexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImagefilexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            IntPtr twmemref
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGEINFO / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twimageinfo"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImageinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEINFO twimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImageinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEINFO twimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImageinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEINFO twimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImageinfo
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEINFO twimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImageinfo
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEINFO_LINUX64 twimageinfolinux64
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImageinfo
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEINFO twimageinfo
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImageinfo
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEINFO twimageinfo
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGELAYOUT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twimagelayout"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImagelayout
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGELAYOUT twimagelayout
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImagelayout
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGELAYOUT twimagelayout
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImagelayout
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGELAYOUT twimagelayout
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImagelayout
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGELAYOUT twimagelayout
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImagelayout
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGELAYOUT twimagelayout
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImagelayout
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGELAYOUT twimagelayout
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImagelayout
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGELAYOUT twimagelayout
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGEMEMFILEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twimagememxfer"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImagememfilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER twimagememxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImagememfilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER twimagememxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImagememfilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER twimagememxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImagememfilexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER twimagememxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImagememfilexfer
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER_LINUX64 twimagememxferlinux64
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImagememfilexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER_MACOSX twimagememxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImagememfilexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER_MACOSX twimagememxfer
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGEMEMXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twimagememxfer"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImagememxfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER twimagememxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImagememxfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER twimagememxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImagememxfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER twimagememxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImagememxfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER twimagememxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImagememxfer
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER_LINUX64 twimagememxferlinux64
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImagememxfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER_MACOSX twimagememxfer
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImagememxfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_IMAGEMEMXFER_MACOSX twimagememxfer
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGENATIVEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImagenativexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrBitmap
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImagenativexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrBitmap
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImagenativexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrBitmap
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImagenativexfer
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrBitmap
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImagenativexfer
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrBitmap
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImagenativexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrBitmap
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImagenativexfer
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref IntPtr intptrBitmap
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.JPEGCOMPRESSION / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twjpegcompression"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryJpegcompression
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_JPEGCOMPRESSION twjpegcompression
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryJpegcompression
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_JPEGCOMPRESSION twjpegcompression
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryJpegcompression
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_JPEGCOMPRESSION twjpegcompression
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryJpegcompression
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_JPEGCOMPRESSION twjpegcompression
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryJpegcompression
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_JPEGCOMPRESSION twjpegcompression
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryJpegcompression
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_JPEGCOMPRESSION twjpegcompression
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryJpegcompression
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_JPEGCOMPRESSION twjpegcompression
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.METRICS / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twmetrics"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryMetrics
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_METRICS twmetrics
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryMetrics
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_METRICS twmetrics
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryMetrics
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_METRICS twmetrics
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryMetrics
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_METRICS twmetrics
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryMetrics
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_METRICS twmetrics
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryMetrics
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_METRICS twmetrics
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryMetrics
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_METRICS twmetrics
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.PALETTE8 / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twpalette8"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryPalette8
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PALETTE8 twpalette8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryPalette8
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PALETTE8 twpalette8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryPalette8
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PALETTE8 twpalette8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryPalette8
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PALETTE8 twpalette8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryPalette8
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PALETTE8 twpalette8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryPalette8
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PALETTE8 twpalette8
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryPalette8
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_PALETTE8 twpalette8
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.RGBRESPONSE / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twrgbresponse"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryRgbresponse
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_RGBRESPONSE twrgbresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryRgbresponse
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_RGBRESPONSE twrgbresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryRgbresponse
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_RGBRESPONSE twrgbresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryRgbresponse
        (
            ref TWAIN.TW_IDENTITY_LEGACY origin,
            ref TWAIN.TW_IDENTITY_LEGACY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_RGBRESPONSE twrgbresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryRgbresponse
        (
            ref TWAIN.TW_IDENTITY origin,
            ref TWAIN.TW_IDENTITY dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_RGBRESPONSE twrgbresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryRgbresponse
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_RGBRESPONSE twrgbresponse
        );
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryRgbresponse
        (
            ref TWAIN.TW_IDENTITY_MACOSX origin,
            ref TWAIN.TW_IDENTITY_MACOSX dest,
            TWAIN.DG dg,
            TWAIN.DAT dat,
            TWAIN.MSG msg,
            ref TWAIN.TW_RGBRESPONSE twrgbresponse
        );

    }
}
