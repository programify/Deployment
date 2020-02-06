//****************************************************************************
//
//   (c) Programify Ltd
//   Main Form Window Logic                                           CForm.cs
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   15-12-19  Added this module to the project.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Management;
using System.Resources;                 // References: Microsoft.Deployment.Resources
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//---------------------------------------------------------------- Custom DLLs

using Programify;                       // References: Programify.dll

//----------------------------------------------------------------- Data Types

using BYTE = System.Byte ;
using WORD = System.UInt16 ;


//****************************************************************************
//                                                                   Namespace
//****************************************************************************
namespace Deployment
{


//****************************************************************************
//                                                                       Class
//****************************************************************************
public partial class CForm : Form
{
//--------------------------------------------------------------------- Public

//-------------------------------------------------------------------- Private

private   string    mstrAccount ;  // Current user account name.
private   string    mstrCustom ;   // Custom folder.
private   string    mstrCommon ;   // Common data folder shared by all users.
private   string    mstrData ;     // User selected data folder.
private   string    mstrDeploy ;   // Executable to call to perform /DEPLOY (can be missing or blank).
private   string    mstrDetail ;   // Information to provide support when reporting errors.
private   string    mstrIni ;      // File name of target application's INI file.
private   string    mstrManufac ;  // Manufacturer's name (Programify Client).
private   string    mstrPrivate ;  // User's private account data folder.
private   string    mstrProduct ;  // Product's name.
private   string    mstrScDesc ;   // Shortcut description.
private   string    mstrScIcon ;   // Shortcut icon file name.
private   string    mstrScTarget ; // Shortcut target executable.
private   string    mstrScTitle ;  // Shortcut title as it appears on the desktop.
private   string    mstrSoftware ; // Protected "Program Files" folder.
private   string    mstrSystem ;   // Windows system folder.
private   string    mstrVersion ;  // Version number of application being deployed.
private   string    mstrWorking ;  // Temporary location of unzipped files.

private   WORD      mwStep ;       // Step number within deployment process.

//--------------------------------------------------------------- Enumerations

private   Series.Cause        meErrCause ;
private   Series.Exception    meErrException ;

private   EnumFormAction      meAction ;

//-------------------------------------------------------------------- Objects

private   CLibIniFile         moDeployIni ;

private   ResourceManager     moResources ;

//---------------------------------------------------------------------- Locks

//-------------------------------------------------------------------- Structs

//----------------------------------------------------------------- Properties

//------------------------------------------------------------ InteropServices

internal static class NativeMethods
{        
     [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
     public static extern bool IsWow64Process (IntPtr hProcess, out bool Wow64Process) ;

     [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
     public static extern bool Wow64DisableWow64FsRedirection (out IntPtr OldValue) ;

     [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
     public static extern bool Wow64RevertWow64FsRedirection (IntPtr OldValue) ;

     [DllImport("gdi32.dll")]
     public static extern int GetDeviceCaps (IntPtr hdc, int nIndex) ;
}


//****************************************************************************
//                                                                     Methods
//****************************************************************************


//============================================================================
//                                                                       CForm
//----------------------------------------------------------------------------
public CForm ()
{
// Prepare form window
     WindowState   = FormWindowState.Normal ;
     AutoScaleMode = AutoScaleMode.Dpi ;
// Global inits
     moResources    = Strings.ResourceManager ;
     meAction       = EnumFormAction.DoStep ;
     meErrCause     = Series.Cause.None ;
     meErrException = Series.Exception.None ;
     mwStep         = (WORD) EnumStep.SelectTargetFolders ;
// Start up window
     InitializeComponent () ;
// Fetch base folder names from environment
     mstrSystem   = Environment.GetFolderPath (Environment.SpecialFolder.System) ;
     mstrCommon   = Environment.GetFolderPath (Environment.SpecialFolder.CommonApplicationData) ;
     mstrPrivate  = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData) ;
     mstrSoftware = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFiles) ;
     mstrWorking  = Environment.CurrentDirectory ;
     mstrAccount  = Environment.UserName ;
// Set up automated form
     timing.Start () ;
}


//============================================================================
//                                                                       Yield
//----------------------------------------------------------------------------
static public void Yield ()
{
// Service windows events
     Application.DoEvents () ;
// Share processor with other applications
     Thread.Yield () ;
}


//############################################################################
//                                                                 Event_Timer
//----------------------------------------------------------------------------
/*
 *   Event_Timer() is called every 100ms as specified in SetUpAutomation().
 *   With each call, the form command code is examined and acted on if it is
 *   set to a value other than "FormAction.DoNothing".
 *
 *   This effect was designed to operate while the Bossac programmer is
 *   operating to show that the Update utility is itself busy.
 */
private void Event_Timer (Object oTimer, EventArgs eventArgs)
{
     EnumFormAction      action ;

     Series.Cause        eErrCause ;
     Series.Exception    eErrException ;

// Check if an error was detected
     if (meErrCause != Series.Cause.None)
     {
     // Controlled access to the error reporting methods
          eErrCause      = meErrCause ;
          eErrException  = meErrException ;
     // Reset global members
          meErrCause     = Series.Cause.None ;
          meErrException = Series.Exception.None ;
          ReportError (eErrCause, eErrException) ;
     }
// Init
     action = meAction ;
// Ignore if no action to be tacken yet
     if (meAction == EnumFormAction.DoNothing)
          return ;
// Prevent from repeating incoming action
     meAction = EnumFormAction.DoNothing ;

// Distribute control on action
     switch (action)
     {
          case EnumFormAction.DoStep :
               switch ((EnumStep) mwStep)
               {
                    case EnumStep.SelectTargetFolders : DoSelectFolders () ; break ;
                    case EnumStep.DeployProgramFiles :  DoDeployment () ;    break ;
                    case EnumStep.CloseApp :            DoCloseApp () ;      break ;
               }
               break ;

          case EnumFormAction.DoCloseApp :
               DoCloseApp () ;
               break ;
     }
}


//----------------------------------------------------------------------------
//                                                                TakeNextStep
//----------------------------------------------------------------------------
/*
 *   TakeNextStep() controls progress through the several steps of deployment.
 *   This function can be called by manually clicking on a Next button, or 
 *   directly from within a function to perform an automatic step forward.
 *
 *   stepDirection allows for more complex future forms which allow back 
 *   stepping. However, at the moment it is not actively used.
 *
 *   Returns true if a change in the step counter was performed.
 */
private Boolean TakeNextStep (EnumStep stepDirection)
{
     WORD      wStep ;

// Move to next step
     wStep = mwStep ;
// Distribute on direction
     switch (stepDirection)
     {
          case EnumStep.Next :
          // Attempt to move to next step
               wStep ++ ;
          // Ignore if no forward steps remain
               if (wStep >= (WORD) EnumStep.NoMoreSteps)
                    return false ;
               break ;

          case EnumStep.Back :
          // Attempt to move to next step
               wStep -- ;
          // Ignore if no backward steps remain
               if (wStep == 0)
                    return false ;
               break ;
     }
// Prepare form for the next step
     mwStep   = wStep ;
     meAction = EnumFormAction.DoStep ;
     return true ;
}


//----------------------------------------------------------------------------
//                                                             DoSelectFolders
//----------------------------------------------------------------------------
private void DoSelectFolders ()
{
     string    strAppend ;

// Set up form
     btnInstall.Enabled = true ;
// Load deployment configuration
     moDeployIni  = new CLibIniFile (mstrWorking, "Deployment.ini") ;
     mstrManufac  = moDeployIni.GetString (EnumSections.Target, EnumKeys.Manufacturer) ;
     mstrProduct  = moDeployIni.GetString (EnumSections.Target, EnumKeys.Product) ;
     mstrVersion  = moDeployIni.GetString (EnumSections.Target, EnumKeys.Version) ;
     mstrIni      = moDeployIni.GetString (EnumSections.Target, EnumKeys.Ini) ;
     mstrDeploy   = moDeployIni.GetString (EnumSections.Target, EnumKeys.DeployUsing) ;
// Append manufacturer and product
     strAppend     = $"\\{mstrManufac}\\{mstrProduct}" ;
     mstrCommon   += strAppend ;
     mstrPrivate  += strAppend ;
     mstrSoftware += strAppend ;
// Assign software product details to visible controls
     tbTitle.Text   = "Install " + moDeployIni.GetString (EnumSections.Target, EnumKeys.Title) ;
     tbVersion.Text = "Version " + mstrVersion ;
// Display OS environment
     DisplayFolders () ;
}


//----------------------------------------------------------------------------
//                                                                DoDeployment
//----------------------------------------------------------------------------
/*
 *   DoDeployment() copies all extracted files in the working folder to the 
 *   "Program Files" folder.
 */
private void DoDeployment ()
{
// Set up form
     btnInstall.Enabled = false ;
     tbData.ReadOnly = true ;
// Prepare client's product folders
     Directory.CreateDirectory (mstrData) ;
     Directory.CreateDirectory (mstrSoftware) ;
// Transfer most extracted files to the protected software folder
     TransferFiles (EnumSections.Protected, mstrSoftware) ;
// Transfer some extracted files to the unprotected user data folder
     TransferFiles (EnumSections.UserData,  mstrData) ;
// Link static installation INI to dynamic user config INI
     LinkToUserData () ;
// Create desktop shortcut if required
     if (cbShortcut.Checked)
          CreateShortcut () ;
// Allow target application to perform specialized deployment methods
     CallTargetToDeploy () ;
// Install any device drivers
     InstallDrivers () ;
// Show deployment successful
     InfoMessageBox (EnumResx.Finished) ;
// Automatically take next step
     TakeNextStep (EnumStep.Next) ;
}


//----------------------------------------------------------------------------
//                                                                  DoCloseApp
//----------------------------------------------------------------------------
private void DoCloseApp ()
{
     Application.Exit () ;
}


//----------------------------------------------------------------------------
//                                                          CallTargetToDeploy
//----------------------------------------------------------------------------
/*
 *   DoCallApp() calls the application with its built-in /DEPLOY switch which
 *   creates the necessary folder structures and user INI confoguration file.
 */
private void CallTargetToDeploy ()
{
     string    strInstalledApp ;

     Process   process ;

// Ignore if no special deployment methods required
     if (string.IsNullOrEmpty (mstrDeploy))
          return ;
// Construct fully pathed EXE from Deployment.ini's [Target] DeployUsing=''
     strInstalledApp = $"{mstrSoftware}\\{mstrDeploy}" ;
// Invoke application to allow it to perform its /DEPLOY functionality
     process = new Process () ;

     process.StartInfo.UseShellExecute        = false ;
     process.StartInfo.CreateNoWindow         = true ;
     process.StartInfo.RedirectStandardOutput = true ;
     process.StartInfo.RedirectStandardError  = true ;
     process.StartInfo.FileName               = strInstalledApp ;
     process.StartInfo.Arguments              = "/DEPLOY" ;

     process.Start () ;
     process.WaitForExit () ;
     process.Dispose () ;
}

//----------------------------------------------------------------------------
//                                                              InstallDrivers
//----------------------------------------------------------------------------
private Boolean InstallDrivers ()
{
     Boolean   bfProcInfo ;
     Boolean   bfRedirect ;
     Boolean   bfSuccess ;
     Boolean   bfWow64 ;
     string    strArgs ;
     string    strPnPUtil ;
     string    strDriverPath ;
     string    strFilename ;
     WORD      wDriverNum ;

     IntPtr    hProcess ;
     IntPtr    hRestore ;

     Process             process ;
     ProcessStartInfo    startinfo ;

// Init
     bfSuccess  = false ;
     wDriverNum = 0 ;
     meErrCause = Series.Cause.None ;
// Get a handle to this current process
     hProcess   = Process.GetCurrentProcess ().Handle ;
     hRestore   = IntPtr.Zero ;
// Check if it is running on a 64-bit processor
     bfProcInfo = NativeMethods.IsWow64Process (hProcess, out bfWow64) ;
     if (! bfProcInfo)
     {
          meErrCause = Series.Cause.IsWow64Process ;
          goto exit_method ;
     }
// Disable file system redirection for this thread
     if (bfWow64)
     {
          bfRedirect = NativeMethods.Wow64DisableWow64FsRedirection (out hRestore) ;
          if (! bfRedirect)
          {
               meErrCause = Series.Cause.Wow64Disable ;
               goto exit_method ;
          }
     }
// Enumerate all file names in the specified section of the Deployment.ini
     while (true)
     {
     // Fetch next file listed in specified section
          wDriverNum ++ ;
     // Get driver's INF file name from deployment.ini [DeviceDrivers] 1='' (Can be more than one in the future)
          strFilename = moDeployIni.GetString (EnumSections.DeviceDrivers, wDriverNum.ToString (CProgram.CultureInfo)) ;
     // Stop if next filename was not found
          if (string.IsNullOrEmpty (strFilename))
               break ;
     // Construct fully pathed INF file spec
          strDriverPath = $"{mstrWorking}\\{strFilename}" ;
     // Get location of "Windows\\System32\\InfDefaultInstall.exe"
          strPnPUtil    = $"{mstrSystem}\\pnputil.exe" ;
     // Construct parameters for driver installer
          strArgs       = $"/add-driver {strDriverPath}" ;
     // Invoke device driver installation
          process   = new Process () ;
          startinfo = new ProcessStartInfo () ;
          startinfo.UseShellExecute = false ;
          startinfo.CreateNoWindow  = true ;
          startinfo.FileName        = strPnPUtil ;
          startinfo.Arguments       = strArgs ;

          process.StartInfo = startinfo ;

          try
          {
               process.Start () ;
          }
          catch (InvalidOperationException)      { meErrException = Series.Exception.InvalidOp ;    }
          catch (Win32Exception)                 { meErrException = Series.Exception.Win32 ;        }  // System.ComponentModel
          catch (PlatformNotSupportedException)  { meErrException = Series.Exception.NotSupported ; }
     // Intercept caught exceptions
          if (meErrException == Series.Exception.None)
               process.WaitForExit () ;

          process.Dispose () ;

     // Stop without success
          if (meErrCause != Series.Cause.None)
          {
               meErrCause |= Series.Cause.PnPUtil ;
               goto exit_method ;
          }
     }
// Success
     bfSuccess = true ;

exit_method:

// Restore file system redirection for this thread
     if (bfWow64)
          NativeMethods.Wow64RevertWow64FsRedirection (hRestore) ;

     return bfSuccess ;
}


//----------------------------------------------------------------------------
//                                                               TransferFiles
//----------------------------------------------------------------------------
private Boolean TransferFiles (string strSection, string strFolder)
{
     Boolean   bfSuccess ;
     string    strDest ;
     string    strFilename ;
     string    strSource ;
     WORD      wFileNum ;

// Init
     bfSuccess  = false ;
     wFileNum   = 0 ;
     meErrCause = Series.Cause.None ;
// Enumerate all file names in the specified section of the Deployment.ini
     while (true)
     {
     // Fetch next file listed in specified section
          wFileNum ++ ;
          strFilename = moDeployIni.GetString (strSection, wFileNum.ToString (CProgram.CultureInfo)) ;
     // Stop if next filename was not found
          if (string.IsNullOrEmpty (strFilename))
               break ;
     // Construct source file spec
          strSource = $"{mstrWorking}\\{strFilename}" ;
     // Construct destination file spec
          strDest   = $"{strFolder}\\{strFilename}" ;

     // Perform file overwrite copy to specified destination folder
          try
          {
               System.IO.File.Copy (strSource, strDest, true) ;
          }
          catch (UnauthorizedAccessException)  { meErrException = Series.Exception.Access ;       }
          catch (ArgumentException)            { meErrException = Series.Exception.Argument ;     }
          catch (PathTooLongException)         { meErrException = Series.Exception.PathTooLong ;  }
          catch (DirectoryNotFoundException)   { meErrException = Series.Exception.DirNotFound ;  }
          catch (FileNotFoundException)        { meErrException = Series.Exception.FileNotFound ; }
          catch (IOException)                  { meErrException = Series.Exception.IO ;           }
          catch (NotSupportedException)        { meErrException = Series.Exception.NotSupported ; }

     // Intercept caught exceptions
          if (meErrException != Series.Exception.None)
          {
               meErrCause = Series.Cause.FileCopy ;
               mstrDetail = $"Source:\r\n{strSource}\r\n\r\nDest:\r\n{strDest}" ;
               goto exit_method ;
          }
     }
// Success
     bfSuccess = true ;

exit_method:

     return bfSuccess ;
}


//----------------------------------------------------------------------------
//                                                              CreateShortcut
//----------------------------------------------------------------------------
private void CreateShortcut ()
{
     string    strDesktop ;
     string    strShortcut ;

     IWshShortcut   shortcut ;
     WshShell       shell ;

// Init
     strDesktop = Environment.GetFolderPath (Environment.SpecialFolder.Desktop) ;
// Fetch details for desktop shortcut
     mstrScDesc   = moDeployIni.GetString (EnumSections.Shortcut, EnumKeys.Description) ;
     mstrScIcon   = moDeployIni.GetString (EnumSections.Shortcut, EnumKeys.Icon) ;
     mstrScTitle  = moDeployIni.GetString (EnumSections.Shortcut, EnumKeys.Title) ;
     mstrScTarget = moDeployIni.GetString (EnumSections.Shortcut, EnumKeys.Target) ;
// Construct filespec of .LNK file
     strShortcut = Path.Combine (strDesktop, $"{mstrScTitle}.lnk") ;
// Open Windows Shell runtime library
     shell = new WshShell () ;
// Define shortcut object and save it
     shortcut                  = (IWshShortcut) shell.CreateShortcut (strShortcut) ;
     shortcut.Description      = mstrScDesc ;
     shortcut.IconLocation     = $"{mstrSoftware}\\{mstrScIcon}" ;
     shortcut.TargetPath       = $"{mstrSoftware}\\{mstrScTarget}" ;
     shortcut.WorkingDirectory = $"{mstrSoftware}\\" ;
     shortcut.Save () ;
}


//----------------------------------------------------------------------------
//                                                              LinkToUserData
//----------------------------------------------------------------------------
/*
 *   LinkToUserData() inserts a link from the static installation INI file to 
 *   the editable user configuration file. The [AppInstall] section is a
 *   predetermined name which is required by this deployment application in
 *   order to function correctly.
 */
private void LinkToUserData ()
{
     CLibIniFile  oStaticIni ;

     oStaticIni = new CLibIniFile (mstrSoftware, mstrIni) ;
     oStaticIni.SetString (EnumSections.AppInstall, EnumKeys.UserData, mstrData) ;
     oStaticIni.SetString (EnumSections.AppInstall, EnumKeys.Version,  mstrVersion) ;
// Check if only allowing the current user to run the application
     if (rbPrivate.Checked)
          oStaticIni.SetString (EnumSections.AppInstall, EnumKeys.Account, mstrAccount) ;
     else
          oStaticIni.SetString (EnumSections.AppInstall, EnumKeys.Account, "") ;
}


//----------------------------------------------------------------------------
//                                                              DisplayFolders
//----------------------------------------------------------------------------
private void DisplayFolders ()
{
// Assign folder names to form controls
     tbSoftware.Text = mstrSoftware ;
// Assign user selected data folder to form's data folder name
     tbData.ReadOnly = true ;
     if (rbCommon.Checked)
          tbData.Text = mstrCommon ;
     if (rbPrivate.Checked)
          tbData.Text = mstrPrivate ;
     if (rbCustom.Checked)
     {
          tbData.Text = mstrCustom ;
          tbData.ReadOnly = false ;
     }
     mstrData = tbData.Text ;
}


//----------------------------------------------------------------------------
//                                                                 ReportError
//----------------------------------------------------------------------------
private void ReportError (Series.Cause eErrCause, Series.Exception eErrException)
{
     UInt32    dwErrCause ;
     UInt32    dwErrExcept ;
     string    strCode ;
     string    strMsg ;
     string    strNameCause ;
     string    strNameExcep ;
     string    strTitle ;

// Init
     dwErrCause  = (UInt32) eErrCause ;
     dwErrExcept = (UInt32) eErrException ;
     strNameCause = eErrCause.ToString () ;
     strNameExcep = eErrException.ToString () ;
// Prepare error message
     strCode  = $"0x{dwErrCause:X2}{dwErrExcept:X2}" ;
     strMsg   = $"{strCode}\r\n{strNameCause}\r\n{strNameExcep}\r\n\r\n{mstrDetail}" ;
     strTitle = moResources.GetString (EnumResx.ShortTitle, CProgram.CultureInfo) ;

     MessageBox.Show (strMsg, strTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop) ;

// Always close the app after an error
     meAction = EnumFormAction.DoCloseApp ;
}


//----------------------------------------------------------------------------
//                                                              InfoMessageBox
//----------------------------------------------------------------------------
private void InfoMessageBox (string strInResMsg)
{
      ShowMessageBox (EnumResx.ShortTitle, strInResMsg, MessageBoxButtons.OK, MessageBoxIcon.Information) ;
}


//----------------------------------------------------------------------------
//                                                              ShowMessageBox
//----------------------------------------------------------------------------
private void ShowMessageBox (string strInResTitle, string strInResMsg, MessageBoxButtons mbInButtons, MessageBoxIcon mbInIcons)
{
     string    strMsg ;
     string    strTitle ;

     strTitle = moResources.GetString (strInResTitle, CProgram.CultureInfo) ;
     strMsg   = moResources.GetString (strInResMsg,   CProgram.CultureInfo) ;

     MessageBox.Show (strMsg, strTitle, mbInButtons, mbInIcons) ;
}


//############################################################################
//                                                           Event_RadioButton
//----------------------------------------------------------------------------
private void Event_RadioButton (object sender, EventArgs e)
{
     DisplayFolders () ;
}


//############################################################################
//                                                           Event_TextChanged
//----------------------------------------------------------------------------
private void Event_TextChanged (object sender, EventArgs e)
{
     if (rbCustom.Checked)
          mstrCustom = tbData.Text ;
}


//############################################################################
//                                                             Event_ClickNext
//----------------------------------------------------------------------------
private void Event_ClickNext (object sender, EventArgs e)
{
     TakeNextStep (EnumStep.Next) ;
}


//############################################################################
//                                                            Event_ClickClose
//----------------------------------------------------------------------------
private void Event_ClickClose (object sender, EventArgs e)
{
     Application.Exit () ;
}

//############################################################################
//                                                             Event_FormMoved
//----------------------------------------------------------------------------
// 4.76190472
private void Event_FormMoved (object sender, EventArgs e)
{
     CLibForm.FixFormHeight (CProgram.goForm, (float) EnumConstants.FormHeight) ;
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
