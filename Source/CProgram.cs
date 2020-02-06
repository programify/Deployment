//****************************************************************************
//
//   (c) Programify Ltd
//   Application Deployment Utility                                CProgram.cs
//
//****************************************************************************
/*
 *   Command line :
 *
 *        DEPLOYMENT [<switches>]
 *
 *   Where :
 *
 *        <switches> are one or more optional switches (keywords preceeded by
 *        the forward oblique character) that may appear in any order and are
 *        case insensitive. However, be aware that any alphabetic data passed 
 *        to the application via a switch may be case sensitive.
 *
 *        Available switches :
 *
 *             /APP={name}
 *
 *                  Specifies the name of the application's executable
 *                  .EXE file. It is assumed that its configuration
 *                  .INI file will have an identical file name.
 *
 *                  The .INI file will be given two values in one section:
 *
 *                       [AppInstall]
 *
 *                       Account=       Specifies the user account name if
 *                                      the application is to be restricted
 *                                      to one user account.
 *
 *                       UserData=      Specifies where the user data will
 *                                      be stored.
 *
 *             /CULTURE={culture-code}
 *
 *                  Default "en-GB".
 *
 *             /-
 *
 *                  Ignore any command line switches found to the right
 *                  of this switch. This switch marks the end of the 
 *                  actively interpreted command line.
 */

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   15-12-19  Created project.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


//****************************************************************************
//                                                                   Namespace
//****************************************************************************
namespace Deployment
{


//****************************************************************************
//                                                                       Class
//****************************************************************************
static class CProgram
{
//--------------------------------------------------------------------- Public

//-------------------------------------------------------------------- Private

// Objects

static public CForm      goForm ;

// Locks

// Structs

static private volatile CultureInfo     moCulture ;

// Volatile properties

static private volatile string          mstrCulture ;

//----------------------------------------------------------------- Properties

static public  string         CultureName  { get => mstrCulture ; }

static public  CultureInfo    CultureInfo  { get => moCulture ; }

//------------------------------------------------------------ InteropServices


//****************************************************************************
//                                                                     Methods
//****************************************************************************


//============================================================================
//                                                                        Main
//============================================================================
[STAThread]
static void Main (string [] astrArgs)
{
// Init Windows .NET Form application
     Application.EnableVisualStyles () ;
     Application.SetCompatibleTextRenderingDefault (false) ;
// Process command line arguments
     CommandLine (astrArgs) ;
// Adopt cultural settings
     moCulture = new CultureInfo (mstrCulture, false) ;
// Pass control to Windows form
     goForm = new CForm () ;
     Application.Run (goForm) ;
     goForm.Dispose () ;
}


//----------------------------------------------------------------------------
//                                                                 CommandLine
//----------------------------------------------------------------------------
static private bool CommandLine (string [] astrArgs)
{
     Boolean   bfBreak ;
     Boolean   bfSuccess ;
     Int32     iStart ;
     string    strValue ;
     EnumSwitchId  eSwitchId ;

// Init
     bfBreak   = false ;
     bfSuccess = false ;
// Set default values
     mstrCulture = "en-GB" ;
// Enumerate command line arguments
     foreach (string strArgument in astrArgs)
     {
     // Identify start of switch arguments
          iStart = strArgument.IndexOf ('/') ;
          if (iStart != 0)
               continue ;
     // Determine which switch is being presented
          eSwitchId = SwitchGetId (strArgument, out strValue) ;
     // Distribute on identified switch
          switch (eSwitchId)
          {
               case EnumSwitchId.Culture :
                    mstrCulture = strValue ;
                    break ;

               case EnumSwitchId.Verbose :
                    break ;

          // Ignore remainder of command line if "/-" switch is encountered
               case EnumSwitchId.IgnoreArgs :
                    bfBreak = true ;
                    break ;

               default :
                    goto exit_function ;
          }
     // Break out of loop
          if (bfBreak)
               break ;
     }
// Success
     bfSuccess = true ;

exit_function:

     return bfSuccess ;
}


//----------------------------------------------------------------------------
//                                                                 SwitchGetId
//----------------------------------------------------------------------------
static private EnumSwitchId SwitchGetId (string strArgument, out string strValue)
{
     Int32     iEnd ;
     Int32     iValue ;
     string    strName ;

// Init
     strValue = null ;
// Extract switch name from full expression
     iEnd = strArgument.IndexOf ('=') ;
     if (iEnd <= 0)
     {
          iValue = 0 ;
          iEnd   = strArgument.Length ;
     }
     else
          iValue = iEnd + 1 ;
     strName = strArgument.Substring (0, iEnd) ;
// Extract value being assigned to named switch
     if (iValue > 0)
          strValue = strArgument.Substring (iValue) ;
// Detect optional switches
     if (strName.Equals ("/CULTURE", StringComparison.InvariantCultureIgnoreCase))
          return EnumSwitchId.Culture ;
     if (strName.Equals ("/VERBOSE", StringComparison.InvariantCultureIgnoreCase))
          return EnumSwitchId.Verbose ;
// Ignore remaining arguments on command line
     if (string.Equals (strName, "/-", StringComparison.InvariantCultureIgnoreCase))
          return EnumSwitchId.IgnoreArgs ;

     return EnumSwitchId.Unknown ;
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
