//****************************************************************************
//
//   (c) Programify Ltd
//   Project Enumerations                                            CEnums.cs
//
//****************************************************************************

//****************************************************************************
//                                                                 Development
//****************************************************************************
/*
 *   15-12-19  Added to project.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//****************************************************************************
//                                                                   Namespace
//****************************************************************************
namespace Deployment
{

public enum EnumFormAction
{
     DoCloseApp,
     DoNothing,
     DoStep,
     Unknown
}

public enum EnumStep
{
     SelectTargetFolders = 1,
     DeployProgramFiles  = 2,
     CloseApp            = 3,
     NoMoreSteps         = 4,
     Next,
     Back
}

public enum EnumSwitchId
{
     Culture,
     IgnoreArgs,
     Verbose,
     Unknown
}

public enum EnumDeviceCap
{
    VERTRES        = 10,
    DESKTOPVERTRES = 117

// See: http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
} 

public static class EnumConstants
{
/*
 *   Target form height used to correct scaling errors by Windows. The value
 *   is in inches! It is derived from dividing the intended for form height 
 *   in pixels by the DPI of the monitor being used to display the form (168
 *   on 4K screens or 96 on HD or less).
 */
     public const double FormHeight = 4.76190472 ;
}


public static class EnumResx
{
/*
 *   Keys to string in the Resource table.
 */
     public const string Finished     = "Finished" ;
     public const string ShortTitle   = "ShortTitle" ;
     public const string UnknownError = "UnknownError" ;
}

/*
 *   Global INI key names.
 */
public static class EnumKeys
{
     public const string Account      = "Account" ;
     public const string DeployUsing  = "DeployUsing" ;
     public const string Description  = "Description" ;
     public const string Icon         = "Icon" ;
     public const string Ini          = "Ini" ;
     public const string Manufacturer = "Manufacturer" ;
     public const string Product      = "Product" ;
     public const string Target       = "Target" ;
     public const string Title        = "Title" ;
     public const string UserData     = "UserData" ;
     public const string Version      = "Version" ;
}

/*
 *   Global INI section names.
 */
public static class EnumSections
{
     public const string AppInstall    = "AppInstall" ;
     public const string DeviceDrivers = "DeviceDrivers" ;
     public const string Protected     = "Protected" ;
     public const string Shortcut      = "Shortcut" ;
     public const string Target        = "Target" ;
     public const string UserData      = "UserData" ;
}

//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
