using System;
using System.DirectoryServices.AccountManagement;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace enigmaCrypt.Win32.WinUtils
{
    public class Authentication
    {
        [DllImport("ole32.dll")]
        public static extern void CoTaskMemFree(IntPtr ptr);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct CREDUI_INFO
        {
            public int cbSize;
            public IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }


        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern bool CredUnPackAuthenticationBuffer(int dwFlags,
                                                                   IntPtr pAuthBuffer,
                                                                   uint cbAuthBuffer,
                                                                   StringBuilder pszUserName,
                                                                   ref int pcchMaxUserName,
                                                                   StringBuilder pszDomainName,
                                                                   ref int pcchMaxDomainame,
                                                                   StringBuilder pszPassword,
                                                                   ref int pcchMaxPassword);

        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO notUsedHere,
                                                                     int authError,
                                                                     ref uint authPackage,
                                                                     IntPtr InAuthBuffer,
                                                                     uint InAuthBufferSize,
                                                                     out IntPtr refOutAuthBuffer,
                                                                     out uint refOutAuthBufferSize,
                                                                     ref bool fSave,
                                                                     int flags);



        public static void TryAuthenticateUser(string msgTitle, string msgText, out NetworkCredential networkCredential)
        {
            CREDUI_INFO credui = new CREDUI_INFO();
            credui.pszCaptionText = msgTitle;
            credui.pszMessageText = msgText;
            credui.cbSize = Marshal.SizeOf(credui);
            uint authPackage = 0;
            IntPtr outCredBuffer = new IntPtr();
            uint outCredSize;
            bool save = false;
            int result = CredUIPromptForWindowsCredentials(ref credui,
                                                           0,
                                                           ref authPackage,
                                                           IntPtr.Zero,
                                                           0,
                                                           out outCredBuffer,
                                                           out outCredSize,
                                                           ref save,
                                                           1);

            var usernameBuf = new StringBuilder(100);
            var passwordBuf = new StringBuilder(100);
            var domainBuf = new StringBuilder(100);

            int maxUserName = 100;
            int maxDomain = 100;
            int maxPassword = 100;

            if (result == 0)
            {
                if (CredUnPackAuthenticationBuffer(0, outCredBuffer, outCredSize, usernameBuf, ref maxUserName,
                                                   domainBuf, ref maxDomain, passwordBuf, ref maxPassword))
                {
                    Marshal.Copy(new byte[outCredSize], 0, outCredBuffer, (int)outCredSize);

                    CoTaskMemFree(outCredBuffer);
                    networkCredential = new NetworkCredential()
                    {
                        UserName = usernameBuf.ToString(),
                        Password = passwordBuf.ToString(),
                        Domain = domainBuf.ToString()
                    };
                    return;
                }
            }
            else
            {
                Environment.Exit(0);
            }

            networkCredential = null;
        }


        public static bool IsValid(NetworkCredential netC)
        {
            const string SPLIT_1 = "\\";
            string Domain = "";

            try
            {
                if (netC.UserName.IndexOf("\\") != -1)
                {
                    string[] arrT = netC.UserName.Split(SPLIT_1[0]);
                    Domain = arrT[0];
                    netC.UserName = arrT[1];
                }

                if (Domain.Length == 0)
                {
                    Domain = System.Environment.MachineName;
                }
            }
            catch { return false; }

            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Machine, Domain))
                {
                    return pc.ValidateCredentials(netC.UserName, netC.Password);
                }
            }
            catch { return false; }
        }
    }
}
