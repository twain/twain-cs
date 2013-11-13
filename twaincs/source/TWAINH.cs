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
using System.Runtime.InteropServices;

namespace TWAINWorkingGroup
{
    /// <summary>
    /// This file contains content gleaned from version 2.3 of the C/C++ TWAIN.H
    /// header file released by the TWAIN Working Group.  It's organized like that
    /// file to make it easier to maintain.
    /// 
    /// Please do not add any code to this module, save for the minimum needed to
    /// maintain a particular definition (such as TW_STR32)...
    /// </summary>
    public partial class TWAIN
    {
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
        TW_INT32................int
        
        TW_UINT8................byte
        TW_UINT16...............ushort
        TW_UINT32...............uint
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
        /// Used for strings that go up to 32-bytes...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_STR32
        {
            /// <summary>
            /// We're stuck with this, because marshalling with packed alignment
            /// can't handle arrays...
            /// </summary>
            private ushort sz32Item_00;
            private ushort sz32Item_01;
            private ushort sz32Item_02;
            private ushort sz32Item_03;
            private ushort sz32Item_04;
            private ushort sz32Item_05;
            private ushort sz32Item_06;
            private ushort sz32Item_07;
            private ushort sz32Item_08;
            private ushort sz32Item_09;
            private ushort sz32Item_10;
            private ushort sz32Item_11;
            private ushort sz32Item_12;
            private ushort sz32Item_13;
            private ushort sz32Item_14;
            private ushort sz32Item_15;
            private ushort sz32Item_16;

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
                // Unpack what we have into a string...
                string sz =
                    Convert.ToChar(sz32Item_00 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_00 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_01 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_01 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_02 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_02 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_03 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_03 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_04 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_04 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_05 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_05 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_06 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_06 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_07 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_07 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_08 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_08 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_09 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_09 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_10 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_10 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_11 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_11 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_12 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_12 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_13 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_13 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_14 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_14 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_15 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_15 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_16 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_16 >> 8) & 0xFF).ToString();

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
                SetValue(a_sz,true);
            }

            /// <summary>
            /// Use this on Mac OS X if you have a call that uses a string
            /// that doesn't include the prefix byte...
            /// </summary>
            /// <returns></returns>
            public void SetNoPrefix(string a_sz)
            {
                SetValue(a_sz,false);
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

                // Pack the data...
                sz32Item_00 = (ushort)((sz[1] << 8) + sz[0]);
                sz32Item_01 = (ushort)((sz[3] << 8) + sz[2]);
                sz32Item_02 = (ushort)((sz[5] << 8) + sz[4]);
                sz32Item_03 = (ushort)((sz[7] << 8) + sz[6]);
                sz32Item_04 = (ushort)((sz[9] << 8) + sz[8]);
                sz32Item_05 = (ushort)((sz[11] << 8) + sz[10]);
                sz32Item_06 = (ushort)((sz[13] << 8) + sz[12]);
                sz32Item_07 = (ushort)((sz[15] << 8) + sz[14]);
                sz32Item_08 = (ushort)((sz[17] << 8) + sz[16]);
                sz32Item_09 = (ushort)((sz[19] << 8) + sz[18]);
                sz32Item_10 = (ushort)((sz[21] << 8) + sz[20]);
                sz32Item_11 = (ushort)((sz[23] << 8) + sz[22]);
                sz32Item_12 = (ushort)((sz[25] << 8) + sz[24]);
                sz32Item_13 = (ushort)((sz[27] << 8) + sz[26]);
                sz32Item_14 = (ushort)((sz[29] << 8) + sz[28]);
                sz32Item_15 = (ushort)((sz[31] << 8) + sz[30]);
                sz32Item_16 = (ushort)((sz[33] << 8) + sz[32]);
            }
        }

        /// <summary>
        /// Used for strings that go up to 64-bytes...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_STR64
        {
            /// <summary>
            /// We're stuck with this, because marshalling with packed alignment
            /// can't handle arrays...
            /// </summary>
            public ushort sz32Item_000;
            public ushort sz32Item_001;
            public ushort sz32Item_002;
            public ushort sz32Item_003;
            public ushort sz32Item_004;
            public ushort sz32Item_005;
            public ushort sz32Item_006;
            public ushort sz32Item_007;
            public ushort sz32Item_008;
            public ushort sz32Item_009;
            public ushort sz32Item_010;
            public ushort sz32Item_011;
            public ushort sz32Item_012;
            public ushort sz32Item_013;
            public ushort sz32Item_014;
            public ushort sz32Item_015;
            public ushort sz32Item_016;
            public ushort sz32Item_017;
            public ushort sz32Item_018;
            public ushort sz32Item_019;
            public ushort sz32Item_020;
            public ushort sz32Item_021;
            public ushort sz32Item_022;
            public ushort sz32Item_023;
            public ushort sz32Item_024;
            public ushort sz32Item_025;
            public ushort sz32Item_026;
            public ushort sz32Item_027;
            public ushort sz32Item_028;
            public ushort sz32Item_029;
            public ushort sz32Item_030;
            public ushort sz32Item_031;
            public ushort sz32Item_032;

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
                string sz =
                    Convert.ToChar(sz32Item_000 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_000 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_001 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_001 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_002 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_002 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_003 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_003 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_004 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_004 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_005 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_005 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_006 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_006 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_007 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_007 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_008 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_008 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_009 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_009 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_010 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_010 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_011 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_011 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_012 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_012 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_013 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_013 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_014 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_014 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_015 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_015 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_016 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_016 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_017 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_017 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_018 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_018 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_019 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_019 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_020 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_020 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_021 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_021 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_022 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_022 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_023 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_023 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_024 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_024 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_025 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_025 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_026 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_026 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_027 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_027 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_028 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_028 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_029 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_029 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_030 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_030 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_031 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_031 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_032 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_032 >> 8) & 0xFF).ToString();

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
                    "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0";
                if (sz.Length > 64)
                {
                    sz = sz.Remove(64);
                }

                // Pack the data...
                sz32Item_000 = (ushort)((sz[1] << 8) + sz[0]);
                sz32Item_001 = (ushort)((sz[3] << 8) + sz[2]);
                sz32Item_002 = (ushort)((sz[5] << 8) + sz[4]);
                sz32Item_003 = (ushort)((sz[7] << 8) + sz[6]);
                sz32Item_004 = (ushort)((sz[9] << 8) + sz[8]);
                sz32Item_005 = (ushort)((sz[11] << 8) + sz[10]);
                sz32Item_006 = (ushort)((sz[13] << 8) + sz[12]);
                sz32Item_007 = (ushort)((sz[15] << 8) + sz[14]);
                sz32Item_008 = (ushort)((sz[17] << 8) + sz[16]);
                sz32Item_009 = (ushort)((sz[19] << 8) + sz[18]);
                sz32Item_010 = (ushort)((sz[21] << 8) + sz[20]);
                sz32Item_011 = (ushort)((sz[23] << 8) + sz[22]);
                sz32Item_012 = (ushort)((sz[25] << 8) + sz[24]);
                sz32Item_013 = (ushort)((sz[27] << 8) + sz[26]);
                sz32Item_014 = (ushort)((sz[29] << 8) + sz[28]);
                sz32Item_015 = (ushort)((sz[31] << 8) + sz[30]);
                sz32Item_016 = (ushort)((sz[33] << 8) + sz[32]);
                sz32Item_017 = (ushort)((sz[35] << 8) + sz[34]);
                sz32Item_018 = (ushort)((sz[37] << 8) + sz[36]);
                sz32Item_019 = (ushort)((sz[39] << 8) + sz[38]);
                sz32Item_020 = (ushort)((sz[41] << 8) + sz[40]);
                sz32Item_021 = (ushort)((sz[43] << 8) + sz[42]);
                sz32Item_022 = (ushort)((sz[45] << 8) + sz[44]);
                sz32Item_023 = (ushort)((sz[47] << 8) + sz[46]);
                sz32Item_024 = (ushort)((sz[49] << 8) + sz[48]);
                sz32Item_025 = (ushort)((sz[51] << 8) + sz[50]);
                sz32Item_026 = (ushort)((sz[53] << 8) + sz[52]);
                sz32Item_027 = (ushort)((sz[55] << 8) + sz[54]);
                sz32Item_028 = (ushort)((sz[57] << 8) + sz[56]);
                sz32Item_029 = (ushort)((sz[59] << 8) + sz[58]);
                sz32Item_030 = (ushort)((sz[61] << 8) + sz[60]);
                sz32Item_031 = (ushort)((sz[63] << 8) + sz[62]);
                sz32Item_032 = (ushort)((sz[64] << 8) + sz[63]);
            }
        }

        /// <summary>
        /// Used for strings that go up to 128-bytes...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_STR128
        {
            /// <summary>
            /// We're stuck with this, because marshalling with packed alignment
            /// can't handle arrays...
            /// </summary>
            public ushort sz32Item_000;
            public ushort sz32Item_001;
            public ushort sz32Item_002;
            public ushort sz32Item_003;
            public ushort sz32Item_004;
            public ushort sz32Item_005;
            public ushort sz32Item_006;
            public ushort sz32Item_007;
            public ushort sz32Item_008;
            public ushort sz32Item_009;
            public ushort sz32Item_010;
            public ushort sz32Item_011;
            public ushort sz32Item_012;
            public ushort sz32Item_013;
            public ushort sz32Item_014;
            public ushort sz32Item_015;
            public ushort sz32Item_016;
            public ushort sz32Item_017;
            public ushort sz32Item_018;
            public ushort sz32Item_019;
            public ushort sz32Item_020;
            public ushort sz32Item_021;
            public ushort sz32Item_022;
            public ushort sz32Item_023;
            public ushort sz32Item_024;
            public ushort sz32Item_025;
            public ushort sz32Item_026;
            public ushort sz32Item_027;
            public ushort sz32Item_028;
            public ushort sz32Item_029;
            public ushort sz32Item_030;
            public ushort sz32Item_031;
            public ushort sz32Item_032;
            public ushort sz32Item_033;
            public ushort sz32Item_034;
            public ushort sz32Item_035;
            public ushort sz32Item_036;
            public ushort sz32Item_037;
            public ushort sz32Item_038;
            public ushort sz32Item_039;
            public ushort sz32Item_040;
            public ushort sz32Item_041;
            public ushort sz32Item_042;
            public ushort sz32Item_043;
            public ushort sz32Item_044;
            public ushort sz32Item_045;
            public ushort sz32Item_046;
            public ushort sz32Item_047;
            public ushort sz32Item_048;
            public ushort sz32Item_049;
            public ushort sz32Item_050;
            public ushort sz32Item_051;
            public ushort sz32Item_052;
            public ushort sz32Item_053;
            public ushort sz32Item_054;
            public ushort sz32Item_055;
            public ushort sz32Item_056;
            public ushort sz32Item_057;
            public ushort sz32Item_058;
            public ushort sz32Item_059;
            public ushort sz32Item_060;
            public ushort sz32Item_061;
            public ushort sz32Item_062;
            public ushort sz32Item_063;
            public ushort sz32Item_064;

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
                string sz =
                    Convert.ToChar(sz32Item_000 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_000 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_001 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_001 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_002 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_002 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_003 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_003 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_004 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_004 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_005 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_005 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_006 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_006 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_007 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_007 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_008 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_008 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_009 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_009 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_010 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_010 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_011 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_011 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_012 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_012 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_013 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_013 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_014 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_014 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_015 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_015 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_016 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_016 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_017 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_017 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_018 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_018 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_019 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_019 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_020 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_020 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_021 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_021 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_022 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_022 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_023 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_023 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_024 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_024 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_025 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_025 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_026 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_026 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_027 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_027 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_028 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_028 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_029 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_029 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_030 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_030 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_031 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_031 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_032 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_032 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_033 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_033 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_034 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_034 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_035 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_035 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_036 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_036 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_037 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_037 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_038 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_038 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_039 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_039 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_040 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_040 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_041 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_041 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_042 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_042 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_043 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_043 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_044 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_044 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_045 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_045 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_046 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_046 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_047 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_047 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_048 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_048 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_049 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_049 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_050 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_050 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_051 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_051 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_052 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_052 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_053 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_053 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_054 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_054 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_055 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_055 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_056 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_056 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_057 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_057 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_058 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_058 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_059 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_059 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_060 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_060 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_061 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_061 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_062 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_062 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_063 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_063 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_064 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_064 >> 8) & 0xFF).ToString();

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

                // Pack the data...
                sz32Item_000 = (ushort)((sz[1] << 8) + sz[0]);
                sz32Item_001 = (ushort)((sz[3] << 8) + sz[2]);
                sz32Item_002 = (ushort)((sz[5] << 8) + sz[4]);
                sz32Item_003 = (ushort)((sz[7] << 8) + sz[6]);
                sz32Item_004 = (ushort)((sz[9] << 8) + sz[8]);
                sz32Item_005 = (ushort)((sz[11] << 8) + sz[10]);
                sz32Item_006 = (ushort)((sz[13] << 8) + sz[12]);
                sz32Item_007 = (ushort)((sz[15] << 8) + sz[14]);
                sz32Item_008 = (ushort)((sz[17] << 8) + sz[16]);
                sz32Item_009 = (ushort)((sz[19] << 8) + sz[18]);
                sz32Item_010 = (ushort)((sz[21] << 8) + sz[20]);
                sz32Item_011 = (ushort)((sz[23] << 8) + sz[22]);
                sz32Item_012 = (ushort)((sz[25] << 8) + sz[24]);
                sz32Item_013 = (ushort)((sz[27] << 8) + sz[26]);
                sz32Item_014 = (ushort)((sz[29] << 8) + sz[28]);
                sz32Item_015 = (ushort)((sz[31] << 8) + sz[30]);
                sz32Item_016 = (ushort)((sz[33] << 8) + sz[32]);
                sz32Item_017 = (ushort)((sz[35] << 8) + sz[34]);
                sz32Item_018 = (ushort)((sz[37] << 8) + sz[36]);
                sz32Item_019 = (ushort)((sz[39] << 8) + sz[38]);
                sz32Item_020 = (ushort)((sz[41] << 8) + sz[40]);
                sz32Item_021 = (ushort)((sz[43] << 8) + sz[42]);
                sz32Item_022 = (ushort)((sz[45] << 8) + sz[44]);
                sz32Item_023 = (ushort)((sz[47] << 8) + sz[46]);
                sz32Item_024 = (ushort)((sz[49] << 8) + sz[48]);
                sz32Item_025 = (ushort)((sz[51] << 8) + sz[50]);
                sz32Item_026 = (ushort)((sz[53] << 8) + sz[52]);
                sz32Item_027 = (ushort)((sz[55] << 8) + sz[54]);
                sz32Item_028 = (ushort)((sz[57] << 8) + sz[56]);
                sz32Item_029 = (ushort)((sz[59] << 8) + sz[58]);
                sz32Item_030 = (ushort)((sz[61] << 8) + sz[60]);
                sz32Item_031 = (ushort)((sz[63] << 8) + sz[62]);
                sz32Item_032 = (ushort)((sz[65] << 8) + sz[64]);
                sz32Item_033 = (ushort)((sz[67] << 8) + sz[66]);
                sz32Item_034 = (ushort)((sz[69] << 8) + sz[68]);
                sz32Item_035 = (ushort)((sz[71] << 8) + sz[70]);
                sz32Item_036 = (ushort)((sz[73] << 8) + sz[72]);
                sz32Item_037 = (ushort)((sz[75] << 8) + sz[74]);
                sz32Item_038 = (ushort)((sz[77] << 8) + sz[76]);
                sz32Item_039 = (ushort)((sz[79] << 8) + sz[78]);
                sz32Item_040 = (ushort)((sz[81] << 8) + sz[80]);
                sz32Item_041 = (ushort)((sz[83] << 8) + sz[82]);
                sz32Item_042 = (ushort)((sz[85] << 8) + sz[84]);
                sz32Item_043 = (ushort)((sz[87] << 8) + sz[86]);
                sz32Item_044 = (ushort)((sz[89] << 8) + sz[88]);
                sz32Item_045 = (ushort)((sz[91] << 8) + sz[90]);
                sz32Item_046 = (ushort)((sz[93] << 8) + sz[92]);
                sz32Item_047 = (ushort)((sz[95] << 8) + sz[94]);
                sz32Item_048 = (ushort)((sz[97] << 8) + sz[96]);
                sz32Item_049 = (ushort)((sz[99] << 8) + sz[98]);
                sz32Item_050 = (ushort)((sz[101] << 8) + sz[100]);
                sz32Item_051 = (ushort)((sz[103] << 8) + sz[102]);
                sz32Item_052 = (ushort)((sz[105] << 8) + sz[104]);
                sz32Item_053 = (ushort)((sz[107] << 8) + sz[106]);
                sz32Item_054 = (ushort)((sz[109] << 8) + sz[108]);
                sz32Item_055 = (ushort)((sz[111] << 8) + sz[110]);
                sz32Item_056 = (ushort)((sz[113] << 8) + sz[112]);
                sz32Item_057 = (ushort)((sz[115] << 8) + sz[114]);
                sz32Item_058 = (ushort)((sz[117] << 8) + sz[116]);
                sz32Item_059 = (ushort)((sz[119] << 8) + sz[118]);
                sz32Item_060 = (ushort)((sz[121] << 8) + sz[120]);
                sz32Item_061 = (ushort)((sz[123] << 8) + sz[122]);
                sz32Item_062 = (ushort)((sz[125] << 8) + sz[124]);
                sz32Item_063 = (ushort)((sz[127] << 8) + sz[126]);
                sz32Item_064 = (ushort)((sz[129] << 8) + sz[128]);
            }
        }

        /// <summary>
        /// Used for strings that go up to 256-bytes...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_STR255
        {
            /// <summary>
            /// We're stuck with this, because marshalling with packed alignment
            /// can't handle arrays...
            /// </summary>
            public ushort sz32Item_000;
            public ushort sz32Item_001;
            public ushort sz32Item_002;
            public ushort sz32Item_003;
            public ushort sz32Item_004;
            public ushort sz32Item_005;
            public ushort sz32Item_006;
            public ushort sz32Item_007;
            public ushort sz32Item_008;
            public ushort sz32Item_009;
            public ushort sz32Item_010;
            public ushort sz32Item_011;
            public ushort sz32Item_012;
            public ushort sz32Item_013;
            public ushort sz32Item_014;
            public ushort sz32Item_015;
            public ushort sz32Item_016;
            public ushort sz32Item_017;
            public ushort sz32Item_018;
            public ushort sz32Item_019;
            public ushort sz32Item_020;
            public ushort sz32Item_021;
            public ushort sz32Item_022;
            public ushort sz32Item_023;
            public ushort sz32Item_024;
            public ushort sz32Item_025;
            public ushort sz32Item_026;
            public ushort sz32Item_027;
            public ushort sz32Item_028;
            public ushort sz32Item_029;
            public ushort sz32Item_030;
            public ushort sz32Item_031;
            public ushort sz32Item_032;
            public ushort sz32Item_033;
            public ushort sz32Item_034;
            public ushort sz32Item_035;
            public ushort sz32Item_036;
            public ushort sz32Item_037;
            public ushort sz32Item_038;
            public ushort sz32Item_039;
            public ushort sz32Item_040;
            public ushort sz32Item_041;
            public ushort sz32Item_042;
            public ushort sz32Item_043;
            public ushort sz32Item_044;
            public ushort sz32Item_045;
            public ushort sz32Item_046;
            public ushort sz32Item_047;
            public ushort sz32Item_048;
            public ushort sz32Item_049;
            public ushort sz32Item_050;
            public ushort sz32Item_051;
            public ushort sz32Item_052;
            public ushort sz32Item_053;
            public ushort sz32Item_054;
            public ushort sz32Item_055;
            public ushort sz32Item_056;
            public ushort sz32Item_057;
            public ushort sz32Item_058;
            public ushort sz32Item_059;
            public ushort sz32Item_060;
            public ushort sz32Item_061;
            public ushort sz32Item_062;
            public ushort sz32Item_063;
            public ushort sz32Item_064;
            public ushort sz32Item_065;
            public ushort sz32Item_066;
            public ushort sz32Item_067;
            public ushort sz32Item_068;
            public ushort sz32Item_069;
            public ushort sz32Item_070;
            public ushort sz32Item_071;
            public ushort sz32Item_072;
            public ushort sz32Item_073;
            public ushort sz32Item_074;
            public ushort sz32Item_075;
            public ushort sz32Item_076;
            public ushort sz32Item_077;
            public ushort sz32Item_078;
            public ushort sz32Item_079;
            public ushort sz32Item_080;
            public ushort sz32Item_081;
            public ushort sz32Item_082;
            public ushort sz32Item_083;
            public ushort sz32Item_084;
            public ushort sz32Item_085;
            public ushort sz32Item_086;
            public ushort sz32Item_087;
            public ushort sz32Item_088;
            public ushort sz32Item_089;
            public ushort sz32Item_090;
            public ushort sz32Item_091;
            public ushort sz32Item_092;
            public ushort sz32Item_093;
            public ushort sz32Item_094;
            public ushort sz32Item_095;
            public ushort sz32Item_096;
            public ushort sz32Item_097;
            public ushort sz32Item_098;
            public ushort sz32Item_099;
            public ushort sz32Item_100;
            public ushort sz32Item_101;
            public ushort sz32Item_102;
            public ushort sz32Item_103;
            public ushort sz32Item_104;
            public ushort sz32Item_105;
            public ushort sz32Item_106;
            public ushort sz32Item_107;
            public ushort sz32Item_108;
            public ushort sz32Item_109;
            public ushort sz32Item_110;
            public ushort sz32Item_111;
            public ushort sz32Item_112;
            public ushort sz32Item_113;
            public ushort sz32Item_114;
            public ushort sz32Item_115;
            public ushort sz32Item_116;
            public ushort sz32Item_117;
            public ushort sz32Item_118;
            public ushort sz32Item_119;
            public ushort sz32Item_120;
            public ushort sz32Item_121;
            public ushort sz32Item_122;
            public ushort sz32Item_123;
            public ushort sz32Item_124;
            public ushort sz32Item_125;
            public ushort sz32Item_126;
            public ushort sz32Item_127;

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
                string sz =
                    Convert.ToChar(sz32Item_000 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_000 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_001 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_001 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_002 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_002 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_003 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_003 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_004 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_004 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_005 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_005 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_006 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_006 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_007 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_007 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_008 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_008 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_009 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_009 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_010 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_010 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_011 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_011 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_012 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_012 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_013 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_013 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_014 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_014 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_015 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_015 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_016 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_016 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_017 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_017 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_018 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_018 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_019 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_019 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_020 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_020 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_021 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_021 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_022 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_022 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_023 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_023 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_024 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_024 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_025 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_025 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_026 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_026 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_027 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_027 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_028 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_028 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_029 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_029 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_030 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_030 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_031 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_031 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_032 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_032 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_033 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_033 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_034 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_034 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_035 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_035 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_036 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_036 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_037 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_037 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_038 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_038 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_039 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_039 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_040 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_040 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_041 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_041 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_042 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_042 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_043 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_043 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_044 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_044 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_045 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_045 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_046 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_046 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_047 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_047 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_048 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_048 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_049 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_049 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_050 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_050 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_051 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_051 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_052 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_052 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_053 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_053 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_054 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_054 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_055 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_055 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_056 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_056 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_057 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_057 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_058 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_058 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_059 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_059 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_060 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_060 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_061 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_061 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_062 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_062 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_063 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_063 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_064 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_064 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_065 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_065 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_066 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_066 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_067 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_067 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_068 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_068 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_069 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_069 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_070 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_070 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_071 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_071 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_072 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_072 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_073 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_073 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_074 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_074 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_075 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_075 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_076 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_076 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_077 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_077 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_078 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_078 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_079 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_079 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_080 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_080 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_081 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_081 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_082 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_082 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_083 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_083 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_084 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_084 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_085 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_085 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_086 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_086 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_087 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_087 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_088 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_088 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_089 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_089 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_090 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_090 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_091 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_091 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_092 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_092 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_093 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_093 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_094 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_094 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_095 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_095 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_096 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_096 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_097 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_097 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_098 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_098 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_099 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_099 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_100 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_100 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_101 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_101 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_102 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_102 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_103 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_103 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_104 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_104 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_105 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_105 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_106 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_106 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_107 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_107 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_108 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_108 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_109 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_109 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_110 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_110 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_111 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_111 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_112 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_112 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_113 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_113 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_114 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_114 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_115 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_115 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_116 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_116 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_117 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_117 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_118 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_118 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_119 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_119 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_120 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_120 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_121 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_121 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_122 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_122 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_123 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_123 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_124 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_124 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_125 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_125 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_126 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_126 >> 8) & 0xFF).ToString() +
                    Convert.ToChar(sz32Item_127 & 0xFF).ToString() +
                    Convert.ToChar((sz32Item_127 >> 8) & 0xFF).ToString();

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

                // Pack the data...
                sz32Item_000 = (ushort)((sz[1] << 8) + sz[0]);
                sz32Item_001 = (ushort)((sz[3] << 8) + sz[2]);
                sz32Item_002 = (ushort)((sz[5] << 8) + sz[4]);
                sz32Item_003 = (ushort)((sz[7] << 8) + sz[6]);
                sz32Item_004 = (ushort)((sz[9] << 8) + sz[8]);
                sz32Item_005 = (ushort)((sz[11] << 8) + sz[10]);
                sz32Item_006 = (ushort)((sz[13] << 8) + sz[12]);
                sz32Item_007 = (ushort)((sz[15] << 8) + sz[14]);
                sz32Item_008 = (ushort)((sz[17] << 8) + sz[16]);
                sz32Item_009 = (ushort)((sz[19] << 8) + sz[18]);
                sz32Item_010 = (ushort)((sz[21] << 8) + sz[20]);
                sz32Item_011 = (ushort)((sz[23] << 8) + sz[22]);
                sz32Item_012 = (ushort)((sz[25] << 8) + sz[24]);
                sz32Item_013 = (ushort)((sz[27] << 8) + sz[26]);
                sz32Item_014 = (ushort)((sz[29] << 8) + sz[28]);
                sz32Item_015 = (ushort)((sz[31] << 8) + sz[30]);
                sz32Item_016 = (ushort)((sz[33] << 8) + sz[32]);
                sz32Item_017 = (ushort)((sz[35] << 8) + sz[34]);
                sz32Item_018 = (ushort)((sz[37] << 8) + sz[36]);
                sz32Item_019 = (ushort)((sz[39] << 8) + sz[38]);
                sz32Item_020 = (ushort)((sz[41] << 8) + sz[40]);
                sz32Item_021 = (ushort)((sz[43] << 8) + sz[42]);
                sz32Item_022 = (ushort)((sz[45] << 8) + sz[44]);
                sz32Item_023 = (ushort)((sz[47] << 8) + sz[46]);
                sz32Item_024 = (ushort)((sz[49] << 8) + sz[48]);
                sz32Item_025 = (ushort)((sz[51] << 8) + sz[50]);
                sz32Item_026 = (ushort)((sz[53] << 8) + sz[52]);
                sz32Item_027 = (ushort)((sz[55] << 8) + sz[54]);
                sz32Item_028 = (ushort)((sz[57] << 8) + sz[56]);
                sz32Item_029 = (ushort)((sz[59] << 8) + sz[58]);
                sz32Item_030 = (ushort)((sz[61] << 8) + sz[60]);
                sz32Item_031 = (ushort)((sz[63] << 8) + sz[62]);
                sz32Item_032 = (ushort)((sz[65] << 8) + sz[64]);
                sz32Item_033 = (ushort)((sz[67] << 8) + sz[66]);
                sz32Item_034 = (ushort)((sz[69] << 8) + sz[68]);
                sz32Item_035 = (ushort)((sz[71] << 8) + sz[70]);
                sz32Item_036 = (ushort)((sz[73] << 8) + sz[72]);
                sz32Item_037 = (ushort)((sz[75] << 8) + sz[74]);
                sz32Item_038 = (ushort)((sz[77] << 8) + sz[76]);
                sz32Item_039 = (ushort)((sz[79] << 8) + sz[78]);
                sz32Item_040 = (ushort)((sz[81] << 8) + sz[80]);
                sz32Item_041 = (ushort)((sz[83] << 8) + sz[82]);
                sz32Item_042 = (ushort)((sz[85] << 8) + sz[84]);
                sz32Item_043 = (ushort)((sz[87] << 8) + sz[86]);
                sz32Item_044 = (ushort)((sz[89] << 8) + sz[88]);
                sz32Item_045 = (ushort)((sz[91] << 8) + sz[90]);
                sz32Item_046 = (ushort)((sz[93] << 8) + sz[92]);
                sz32Item_047 = (ushort)((sz[95] << 8) + sz[94]);
                sz32Item_048 = (ushort)((sz[97] << 8) + sz[96]);
                sz32Item_049 = (ushort)((sz[99] << 8) + sz[98]);
                sz32Item_050 = (ushort)((sz[101] << 8) + sz[100]);
                sz32Item_051 = (ushort)((sz[103] << 8) + sz[102]);
                sz32Item_052 = (ushort)((sz[105] << 8) + sz[104]);
                sz32Item_053 = (ushort)((sz[107] << 8) + sz[106]);
                sz32Item_054 = (ushort)((sz[109] << 8) + sz[108]);
                sz32Item_055 = (ushort)((sz[111] << 8) + sz[110]);
                sz32Item_056 = (ushort)((sz[113] << 8) + sz[112]);
                sz32Item_057 = (ushort)((sz[115] << 8) + sz[114]);
                sz32Item_058 = (ushort)((sz[117] << 8) + sz[116]);
                sz32Item_059 = (ushort)((sz[119] << 8) + sz[118]);
                sz32Item_060 = (ushort)((sz[121] << 8) + sz[120]);
                sz32Item_061 = (ushort)((sz[123] << 8) + sz[122]);
                sz32Item_062 = (ushort)((sz[125] << 8) + sz[124]);
                sz32Item_063 = (ushort)((sz[127] << 8) + sz[126]);
                sz32Item_064 = (ushort)((sz[129] << 8) + sz[128]);
                sz32Item_065 = (ushort)((sz[131] << 8) + sz[130]);
                sz32Item_066 = (ushort)((sz[133] << 8) + sz[132]);
                sz32Item_067 = (ushort)((sz[135] << 8) + sz[134]);
                sz32Item_068 = (ushort)((sz[137] << 8) + sz[136]);
                sz32Item_069 = (ushort)((sz[139] << 8) + sz[138]);
                sz32Item_070 = (ushort)((sz[141] << 8) + sz[140]);
                sz32Item_071 = (ushort)((sz[143] << 8) + sz[142]);
                sz32Item_072 = (ushort)((sz[145] << 8) + sz[144]);
                sz32Item_073 = (ushort)((sz[147] << 8) + sz[146]);
                sz32Item_074 = (ushort)((sz[149] << 8) + sz[148]);
                sz32Item_075 = (ushort)((sz[151] << 8) + sz[150]);
                sz32Item_076 = (ushort)((sz[153] << 8) + sz[152]);
                sz32Item_077 = (ushort)((sz[155] << 8) + sz[154]);
                sz32Item_078 = (ushort)((sz[157] << 8) + sz[156]);
                sz32Item_079 = (ushort)((sz[159] << 8) + sz[158]);
                sz32Item_080 = (ushort)((sz[161] << 8) + sz[160]);
                sz32Item_081 = (ushort)((sz[163] << 8) + sz[162]);
                sz32Item_082 = (ushort)((sz[165] << 8) + sz[164]);
                sz32Item_083 = (ushort)((sz[167] << 8) + sz[166]);
                sz32Item_084 = (ushort)((sz[169] << 8) + sz[168]);
                sz32Item_085 = (ushort)((sz[171] << 8) + sz[170]);
                sz32Item_086 = (ushort)((sz[173] << 8) + sz[172]);
                sz32Item_087 = (ushort)((sz[175] << 8) + sz[174]);
                sz32Item_088 = (ushort)((sz[177] << 8) + sz[176]);
                sz32Item_089 = (ushort)((sz[179] << 8) + sz[178]);
                sz32Item_090 = (ushort)((sz[181] << 8) + sz[180]);
                sz32Item_091 = (ushort)((sz[183] << 8) + sz[182]);
                sz32Item_092 = (ushort)((sz[185] << 8) + sz[184]);
                sz32Item_093 = (ushort)((sz[187] << 8) + sz[186]);
                sz32Item_094 = (ushort)((sz[189] << 8) + sz[188]);
                sz32Item_095 = (ushort)((sz[191] << 8) + sz[190]);
                sz32Item_096 = (ushort)((sz[193] << 8) + sz[192]);
                sz32Item_097 = (ushort)((sz[195] << 8) + sz[194]);
                sz32Item_098 = (ushort)((sz[197] << 8) + sz[196]);
                sz32Item_099 = (ushort)((sz[199] << 8) + sz[198]);
                sz32Item_100 = (ushort)((sz[201] << 8) + sz[200]);
                sz32Item_101 = (ushort)((sz[203] << 8) + sz[202]);
                sz32Item_102 = (ushort)((sz[205] << 8) + sz[204]);
                sz32Item_103 = (ushort)((sz[207] << 8) + sz[206]);
                sz32Item_104 = (ushort)((sz[209] << 8) + sz[208]);
                sz32Item_105 = (ushort)((sz[211] << 8) + sz[210]);
                sz32Item_106 = (ushort)((sz[213] << 8) + sz[212]);
                sz32Item_107 = (ushort)((sz[215] << 8) + sz[214]);
                sz32Item_108 = (ushort)((sz[217] << 8) + sz[216]);
                sz32Item_109 = (ushort)((sz[219] << 8) + sz[218]);
                sz32Item_110 = (ushort)((sz[221] << 8) + sz[220]);
                sz32Item_111 = (ushort)((sz[223] << 8) + sz[222]);
                sz32Item_112 = (ushort)((sz[225] << 8) + sz[224]);
                sz32Item_113 = (ushort)((sz[227] << 8) + sz[226]);
                sz32Item_114 = (ushort)((sz[229] << 8) + sz[228]);
                sz32Item_115 = (ushort)((sz[231] << 8) + sz[230]);
                sz32Item_116 = (ushort)((sz[233] << 8) + sz[232]);
                sz32Item_117 = (ushort)((sz[235] << 8) + sz[234]);
                sz32Item_118 = (ushort)((sz[237] << 8) + sz[236]);
                sz32Item_119 = (ushort)((sz[239] << 8) + sz[238]);
                sz32Item_120 = (ushort)((sz[241] << 8) + sz[240]);
                sz32Item_121 = (ushort)((sz[243] << 8) + sz[242]);
                sz32Item_122 = (ushort)((sz[245] << 8) + sz[244]);
                sz32Item_123 = (ushort)((sz[247] << 8) + sz[246]);
                sz32Item_124 = (ushort)((sz[249] << 8) + sz[248]);
                sz32Item_125 = (ushort)((sz[251] << 8) + sz[250]);
                sz32Item_126 = (ushort)((sz[253] << 8) + sz[252]);
                sz32Item_127 = (ushort)((sz[255] << 8) + sz[254]);
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
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_CALLBACK
        {
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
            public IntPtr CallBackProc;
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
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_EVENT
        {
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
                    case   0: a_twinfo = Info_000; return;
                    case   1: a_twinfo = Info_001; return;
                    case   2: a_twinfo = Info_002; return;
                    case   3: a_twinfo = Info_003; return;
                    case   4: a_twinfo = Info_004; return;
                    case   5: a_twinfo = Info_005; return;
                    case   6: a_twinfo = Info_006; return;
                    case   7: a_twinfo = Info_007; return;
                    case   8: a_twinfo = Info_008; return;
                    case   9: a_twinfo = Info_009; return;
                    case  10: a_twinfo = Info_010; return;
                    case  11: a_twinfo = Info_011; return;
                    case  12: a_twinfo = Info_012; return;
                    case  13: a_twinfo = Info_013; return;
                    case  14: a_twinfo = Info_014; return;
                    case  15: a_twinfo = Info_015; return;
                    case  16: a_twinfo = Info_016; return;
                    case  17: a_twinfo = Info_017; return;
                    case  18: a_twinfo = Info_018; return;
                    case  19: a_twinfo = Info_019; return;
                    case  20: a_twinfo = Info_020; return;
                    case  21: a_twinfo = Info_021; return;
                    case  22: a_twinfo = Info_022; return;
                    case  23: a_twinfo = Info_023; return;
                    case  24: a_twinfo = Info_024; return;
                    case  25: a_twinfo = Info_025; return;
                    case  26: a_twinfo = Info_026; return;
                    case  27: a_twinfo = Info_027; return;
                    case  28: a_twinfo = Info_028; return;
                    case  29: a_twinfo = Info_029; return;
                    case  30: a_twinfo = Info_030; return;
                    case  31: a_twinfo = Info_031; return;
                    case  32: a_twinfo = Info_032; return;
                    case  33: a_twinfo = Info_033; return;
                    case  34: a_twinfo = Info_034; return;
                    case  35: a_twinfo = Info_035; return;
                    case  36: a_twinfo = Info_036; return;
                    case  37: a_twinfo = Info_037; return;
                    case  38: a_twinfo = Info_038; return;
                    case  39: a_twinfo = Info_039; return;
                    case  40: a_twinfo = Info_040; return;
                    case  41: a_twinfo = Info_041; return;
                    case  42: a_twinfo = Info_042; return;
                    case  43: a_twinfo = Info_043; return;
                    case  44: a_twinfo = Info_044; return;
                    case  45: a_twinfo = Info_045; return;
                    case  46: a_twinfo = Info_046; return;
                    case  47: a_twinfo = Info_047; return;
                    case  48: a_twinfo = Info_048; return;
                    case  49: a_twinfo = Info_049; return;
                    case  50: a_twinfo = Info_050; return;
                    case  51: a_twinfo = Info_051; return;
                    case  52: a_twinfo = Info_052; return;
                    case  53: a_twinfo = Info_053; return;
                    case  54: a_twinfo = Info_054; return;
                    case  55: a_twinfo = Info_055; return;
                    case  56: a_twinfo = Info_056; return;
                    case  57: a_twinfo = Info_057; return;
                    case  58: a_twinfo = Info_058; return;
                    case  59: a_twinfo = Info_059; return;
                    case  60: a_twinfo = Info_060; return;
                    case  61: a_twinfo = Info_061; return;
                    case  62: a_twinfo = Info_062; return;
                    case  63: a_twinfo = Info_063; return;
                    case  64: a_twinfo = Info_064; return;
                    case  65: a_twinfo = Info_065; return;
                    case  66: a_twinfo = Info_066; return;
                    case  67: a_twinfo = Info_067; return;
                    case  68: a_twinfo = Info_068; return;
                    case  69: a_twinfo = Info_069; return;
                    case  70: a_twinfo = Info_070; return;
                    case  71: a_twinfo = Info_071; return;
                    case  72: a_twinfo = Info_072; return;
                    case  73: a_twinfo = Info_073; return;
                    case  74: a_twinfo = Info_074; return;
                    case  75: a_twinfo = Info_075; return;
                    case  76: a_twinfo = Info_076; return;
                    case  77: a_twinfo = Info_077; return;
                    case  78: a_twinfo = Info_078; return;
                    case  79: a_twinfo = Info_079; return;
                    case  80: a_twinfo = Info_080; return;
                    case  81: a_twinfo = Info_081; return;
                    case  82: a_twinfo = Info_082; return;
                    case  83: a_twinfo = Info_083; return;
                    case  84: a_twinfo = Info_084; return;
                    case  85: a_twinfo = Info_085; return;
                    case  86: a_twinfo = Info_086; return;
                    case  87: a_twinfo = Info_087; return;
                    case  88: a_twinfo = Info_088; return;
                    case  89: a_twinfo = Info_089; return;
                    case  90: a_twinfo = Info_090; return;
                    case  91: a_twinfo = Info_091; return;
                    case  92: a_twinfo = Info_092; return;
                    case  93: a_twinfo = Info_093; return;
                    case  94: a_twinfo = Info_094; return;
                    case  95: a_twinfo = Info_095; return;
                    case  96: a_twinfo = Info_096; return;
                    case  97: a_twinfo = Info_097; return;
                    case  98: a_twinfo = Info_098; return;
                    case  99: a_twinfo = Info_099; return;
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
                    case   0: Info_000 = a_twinfo; return;
                    case   1: Info_001 = a_twinfo; return;
                    case   2: Info_002 = a_twinfo; return;
                    case   3: Info_003 = a_twinfo; return;
                    case   4: Info_004 = a_twinfo; return;
                    case   5: Info_005 = a_twinfo; return;
                    case   6: Info_006 = a_twinfo; return;
                    case   7: Info_007 = a_twinfo; return;
                    case   8: Info_008 = a_twinfo; return;
                    case   9: Info_009 = a_twinfo; return;
                    case  10: Info_010 = a_twinfo; return;
                    case  11: Info_011 = a_twinfo; return;
                    case  12: Info_012 = a_twinfo; return;
                    case  13: Info_013 = a_twinfo; return;
                    case  14: Info_014 = a_twinfo; return;
                    case  15: Info_015 = a_twinfo; return;
                    case  16: Info_016 = a_twinfo; return;
                    case  17: Info_017 = a_twinfo; return;
                    case  18: Info_018 = a_twinfo; return;
                    case  19: Info_019 = a_twinfo; return;
                    case  20: Info_020 = a_twinfo; return;
                    case  21: Info_021 = a_twinfo; return;
                    case  22: Info_022 = a_twinfo; return;
                    case  23: Info_023 = a_twinfo; return;
                    case  24: Info_024 = a_twinfo; return;
                    case  25: Info_025 = a_twinfo; return;
                    case  26: Info_026 = a_twinfo; return;
                    case  27: Info_027 = a_twinfo; return;
                    case  28: Info_028 = a_twinfo; return;
                    case  29: Info_029 = a_twinfo; return;
                    case  30: Info_030 = a_twinfo; return;
                    case  31: Info_031 = a_twinfo; return;
                    case  32: Info_032 = a_twinfo; return;
                    case  33: Info_033 = a_twinfo; return;
                    case  34: Info_034 = a_twinfo; return;
                    case  35: Info_035 = a_twinfo; return;
                    case  36: Info_036 = a_twinfo; return;
                    case  37: Info_037 = a_twinfo; return;
                    case  38: Info_038 = a_twinfo; return;
                    case  39: Info_039 = a_twinfo; return;
                    case  40: Info_040 = a_twinfo; return;
                    case  41: Info_041 = a_twinfo; return;
                    case  42: Info_042 = a_twinfo; return;
                    case  43: Info_043 = a_twinfo; return;
                    case  44: Info_044 = a_twinfo; return;
                    case  45: Info_045 = a_twinfo; return;
                    case  46: Info_046 = a_twinfo; return;
                    case  47: Info_047 = a_twinfo; return;
                    case  48: Info_048 = a_twinfo; return;
                    case  49: Info_049 = a_twinfo; return;
                    case  50: Info_050 = a_twinfo; return;
                    case  51: Info_051 = a_twinfo; return;
                    case  52: Info_052 = a_twinfo; return;
                    case  53: Info_053 = a_twinfo; return;
                    case  54: Info_054 = a_twinfo; return;
                    case  55: Info_055 = a_twinfo; return;
                    case  56: Info_056 = a_twinfo; return;
                    case  57: Info_057 = a_twinfo; return;
                    case  58: Info_058 = a_twinfo; return;
                    case  59: Info_059 = a_twinfo; return;
                    case  60: Info_060 = a_twinfo; return;
                    case  61: Info_061 = a_twinfo; return;
                    case  62: Info_062 = a_twinfo; return;
                    case  63: Info_063 = a_twinfo; return;
                    case  64: Info_064 = a_twinfo; return;
                    case  65: Info_065 = a_twinfo; return;
                    case  66: Info_066 = a_twinfo; return;
                    case  67: Info_067 = a_twinfo; return;
                    case  68: Info_068 = a_twinfo; return;
                    case  69: Info_069 = a_twinfo; return;
                    case  70: Info_070 = a_twinfo; return;
                    case  71: Info_071 = a_twinfo; return;
                    case  72: Info_072 = a_twinfo; return;
                    case  73: Info_073 = a_twinfo; return;
                    case  74: Info_074 = a_twinfo; return;
                    case  75: Info_075 = a_twinfo; return;
                    case  76: Info_076 = a_twinfo; return;
                    case  77: Info_077 = a_twinfo; return;
                    case  78: Info_078 = a_twinfo; return;
                    case  79: Info_079 = a_twinfo; return;
                    case  80: Info_080 = a_twinfo; return;
                    case  81: Info_081 = a_twinfo; return;
                    case  82: Info_082 = a_twinfo; return;
                    case  83: Info_083 = a_twinfo; return;
                    case  84: Info_084 = a_twinfo; return;
                    case  85: Info_085 = a_twinfo; return;
                    case  86: Info_086 = a_twinfo; return;
                    case  87: Info_087 = a_twinfo; return;
                    case  88: Info_088 = a_twinfo; return;
                    case  89: Info_089 = a_twinfo; return;
                    case  90: Info_090 = a_twinfo; return;
                    case  91: Info_091 = a_twinfo; return;
                    case  92: Info_092 = a_twinfo; return;
                    case  93: Info_093 = a_twinfo; return;
                    case  94: Info_094 = a_twinfo; return;
                    case  95: Info_095 = a_twinfo; return;
                    case  96: Info_096 = a_twinfo; return;
                    case  97: Info_097 = a_twinfo; return;
                    case  98: Info_098 = a_twinfo; return;
                    case  99: Info_099 = a_twinfo; return;
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
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Pack = 2)]
        public struct TW_FILESYSTEM
        {
            [FieldOffset(0)]
            public TW_STR255 InputName;

            [FieldOffset(256)]
            public TW_STR255 OutputName;

            [FieldOffset(512)]
            public IntPtr Context;

            [FieldOffset(520)]
            public int Recursive;
            [FieldOffset(520)]
            public ushort Subdirectories;

            [FieldOffset(524)]
            public int FileType;
            [FieldOffset(524)]
            public uint FileSystemType;

            [FieldOffset(528)]
            public uint Size;

            [FieldOffset(532)]
            public TW_STR32 CreateTimeDate;

            [FieldOffset(566)]
            public TW_STR32 ModifiedTimeDate;

            [FieldOffset(600)]
            public IntPtr FreeSpace;

            [FieldOffset(604)]
            public IntPtr NewImageSize;

            [FieldOffset(608)]
            public IntPtr NumberOfFiles;

            [FieldOffset(612)]
            public IntPtr NumberOfSnippets;

            [FieldOffset(616)]
            public IntPtr DeviceGroupMask;

            [FieldOffset(620)]
            public byte Reserved;

            [FieldOffset(1127)] // 620 + 508 - 1
            private byte ReservedEnd;
        }
        [StructLayout(LayoutKind.Explicit, Pack = 2)]
        public struct TW_FILESYSTEM_LEGACY
        {
            [FieldOffset(0)]
            public TW_STR255 InputName;

            [FieldOffset(256)]
            public TW_STR255 OutputName;

            [FieldOffset(512)]
            public uint Context;

            [FieldOffset(516)]
            public int Recursive;
            [FieldOffset(516)]
            public ushort Subdirectories;

            [FieldOffset(520)]
            public int FileType;
            [FieldOffset(520)]
            public uint FileSystemType;

            [FieldOffset(524)]
            public uint Size;

            [FieldOffset(528)]
            public TW_STR32 CreateTimeDate;

            [FieldOffset(562)]
            public TW_STR32 ModifiedTimeDate;

            [FieldOffset(596)]
            public IntPtr FreeSpace;

            [FieldOffset(600)]
            public IntPtr NewImageSize;

            [FieldOffset(604)]
            public IntPtr NumberOfFiles;

            [FieldOffset(608)]
            public IntPtr NumberOfSnippets;

            [FieldOffset(612)]
            public IntPtr DeviceGroupMask;

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
            public TW_ELEMENT8 Reponse_00;
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
            public IntPtr pCommand;
            public uint CommandBytes;
            public int Direction;
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
            public IntPtr UTF8string;
        }

        /// <summary>
        /// This structure is used to handle the user interface coordination between an application and a Source.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public struct TW_USERINTERFACE
        {
            public ushort ShowUI;
            public ushort ModalUI;
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
            PDFA2 = 16
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

            // used with DAT_CALLBACK.
            REGISTER_CALLBACK = 0x0902,

            // used with DAT_CAPABILITY.
            RESETALL = 0x0A01
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
            PRINTERTEXT = 0x124A
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
        private const int STSCC = 0x10000; // get us past the custom space
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
            S1, // Nothing loaded or open
            S2, // DSM loaded
            S3, // DSM open
            S4, // Data Source open, programmatic mode (no GUI)
            S5, // GUI up or waiting to transfer first image
            S6, // ready to start transferring image
            S7  // transferring image or transfer done
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // DSM_Entry...
        ///////////////////////////////////////////////////////////////////////////////
        #region DSM_Entry...

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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryNullDest
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryNullDest
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryNullDest
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryNullDest
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntry
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntry
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntry
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntry
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryAudioAudioinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryAudioAudioinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryAudioAudioinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryAudioAudioinfo
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryCallback
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryCallback
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryCallback
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryCallback
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        private delegate UInt16 WindowsDsmEntryCallbackDelegate
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );
        private delegate UInt16 LinuxDsmEntryCallbackDelegate
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );
        private delegate UInt16 MacosxDsmEntryCallbackDelegate
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryCallback2
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback2
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryCallback2
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback2
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryCallback2
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback2
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryCallback2
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback
        );
        private delegate UInt16 WindowsDsmEntryCallback2Delegate
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );
        private delegate UInt16 LinuxDsmEntryCallback2Delegate
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );
        private delegate UInt16 MacosxDsmEntryCallback2Delegate
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryCapability
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryCapability
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryCapability
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryCapability
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryCustomdsdata
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomedsdata
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryCustomdsdata
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomdsdata
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryCustomdsdata
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomdsdata
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryCustomdsdata
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomedsdata
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryDeviceevent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryDeviceevent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryDeviceevent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryDeviceevent
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryEvent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryEvent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryEvent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryEvent
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryEntrypoint
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT twentrypoint
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryEntrypoint
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT twentrypoint
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryEntrypoint
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT twentrypoint
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryEntrypoint
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT twentrypoint
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryFilesystem
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryFilesystem
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryFilesystem
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryFilesystem
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryIdentity
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_LEGACY twidentity
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryIdentity
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_LEGACY twidentity
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryIdentity
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_LEGACY twidentity
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryIdentity
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_MACOSX twidentity
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryParent
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr hwnd
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryParent
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr hwnd
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryParent
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr hwnd
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryParent
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryPassthru
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryPassthru
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryPassthru
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryPassthru
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryPendingxfers
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryPendingxfers
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryPendingxfers
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryPendingxfers
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntrySetupfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntrySetupfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntrySetupfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntrySetupfilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntrySetupmemxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntrySetupmemxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntrySetupmemxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntrySetupmemxfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryStatus
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryStatus
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryStatus
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryStatus
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryStatusutf8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryStatusutf8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryStatusutf8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryStatusutf8
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryUserinterface
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryUserinterface
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryUserinterface
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryUserinterface
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryXfergroup
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryXfergroup
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryXfergroup
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryXfergroup
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryCiecolor
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryCiecolor
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryCiecolor
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryCiecolor
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryExtimageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryExtimageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryExtimageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryExtimageinfo
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryFilter
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryFilter
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryFilter
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryFilter
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryGrayresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryGrayresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryGrayresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryGrayresponse
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryIccprofile
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryIccprofile
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryIccprofile
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryIccprofile
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryImagefilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryImagefilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryImagefilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryImagefilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryImageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO twimageinfo
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryImageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO twimageinfo
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryImageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO twimageinfo
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryImageinfo
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO twimageinfo
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryImagelayout
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryImagelayout
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryImagelayout
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryImagelayout
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGEMEMFILEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twimagememfilexfer"></param>
        /// <returns></returns>
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryImagememfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememfilexfer
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryImagememfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememfilexfer
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryImagememfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememfilexfer
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryImagememfilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememfilexfer
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryImagememxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryImagememxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryImagememxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryImagememxfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER_MACOSX twimagememxfer
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryImagenativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrBitmap
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryImagenativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrBitmap
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryImagenativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrBitmap
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryImagenativexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryJpegcompression
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryJpegcompression
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryJpegcompression
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryJpegcompression
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryPalette8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryPalette8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryPalette8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryPalette8
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
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
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwain32DsmEntryRgbresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 WindowsTwaindsmDsmEntryRgbresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 LinuxDsmEntryRgbresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );
        [DllImport("/System/Library/Frameworks/TWAIN.framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        private static extern UInt16 MacosxDsmEntryRgbresponse
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );

        #endregion
    }
}
