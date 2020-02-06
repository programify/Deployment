//****************************************************************************
//
//   Programify
//   INI Configuration Files                                       CIniFile.cs
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   06-12-19  Added this module to the project.
 *   09-12-19  Modified to allow INI file's contents to be provided by a
 *             pre-loaded string. This avoids the need to create a copy of
 *             the INI file on the users machine.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Forms;


//****************************************************************************
//                                                                   Namespace
//****************************************************************************
namespace Deployment
{


//****************************************************************************
//                                                                       Class
//****************************************************************************
/*
 *        CIniFile
 *
 *        GetInt32
 *        GetFloat
 *        GetListValue
 *        GetSection
 *        GetString
 *        Load
 *        ReadLine
 *        Save
 *        SetString
 */
public class CIniFile
{
//-------------------------------------------------------------- Static Public

//--------------------------------------------------------------------- Public

//-------------------------------------------------------------------- Private

private   Boolean   bfFound ;

private   string    strContents ;       // Entire contents of ini file.
private   string    strErrMessage ;
private   string    strErrTitle ;
private   string    strFileName ;       // File name of ini file.
private   string    strFileSpec ;       // Fully qualified path and file name of ini file.
private   string    strPathway ;        // Folder path of ini file.

//--------------------------------------------------------------- Enumerations

private   EnumError           enumError ;

//-------------------------------------------------------------------- Objects

//---------------------------------------------------------------------- Locks

//-------------------------------------------------------------------- Structs

//----------------------------------------------------------------- Properties

public    Boolean   Found       { get => bfFound ; }

public    string    Contents    { get => strContents ; }
public    string    ErrMessage  { get => strErrMessage ; }
public    string    ErrTitle    { get => strErrTitle ; }
public    string    Filename    { get => strFileName ; }
public    string    Filespec    { get => strFileSpec ; }
public    string    Pathway     { get => strPathway  ; }

public    EnumError ErrEnum     { get => enumError ; }

//------------------------------------------------------------ InteropServices


//****************************************************************************
//                                                                     Methods
//****************************************************************************


//============================================================================
//                                                                    CIniFile
//----------------------------------------------------------------------------
/*
 *   Creates an in-memory copy of a file based INI configuration file. Once 
 *   loaded into memory as a single string, key values can be retrieved.
 */
public CIniFile ()
{
}


//============================================================================
//                                                                    CIniFile
//----------------------------------------------------------------------------
public CIniFile (string strString)
{
// Init to indicate contents came from memory and cannot be saved to disk
     strFileName = null ;
     strFileSpec = null ;
     strPathway  = null ;
// Load the contents of the INI directly from a pre-existing memory-based string
     strContents = strString ;
     enumError   = EnumError.None ;
}


//============================================================================
//                                                                    CIniFile
//----------------------------------------------------------------------------
public CIniFile (string strPath, string strFilename)
{
// Use the current directory if the path string is null
     if (strPath == null)
          strPath = Directory.GetCurrentDirectory () ;
// Load the contents of the INI from a file
     Load (strPath, strFilename) ;
}


//============================================================================
//                                                                        Load
//----------------------------------------------------------------------------
public Boolean Load (string strPath, string strFile)
{
     Boolean   bfSuccess ;

// Init
     bfSuccess = false ;
     enumError = EnumError.None ;
// Verify incoming arguments
     if (string.IsNullOrEmpty (strPath))
          strPath = "" ;
     if (string.IsNullOrEmpty (strFile))
          strFile = "" ;
// Construct fully qualified file specification
     if (strPath.EndsWith ("\\", StringComparison.OrdinalIgnoreCase))
          strPathway = strPath ;
     else
          strPathway = strPath + "\\" ;
// Assign to this object
     strFileSpec = strPathway + strFile ;
     strFileName = strFile ;
     strContents = null ;
// Read entire file into memory
     try
     {
          strContents = File.ReadAllText (strFileSpec) ;
     }
     catch (ArgumentException ex)            { LogException (ex, EnumError.FileReadArg) ;      }
     catch (PathTooLongException ex)         { LogException (ex, EnumError.FileReadPathLen) ;  }
     catch (DirectoryNotFoundException ex)   { LogException (ex, EnumError.FileReadMissDir) ;  }
     catch (IOException ex)                  { LogException (ex, EnumError.FileReadIO) ;       }
     catch (UnauthorizedAccessException ex)  { LogException (ex, EnumError.FileReadAccess) ;   }
     catch (NotSupportedException ex)        { LogException (ex, EnumError.FileReadSupport) ;  }
     catch (SecurityException ex)            { LogException (ex, EnumError.FileReadSecurity) ; }
// Stop without success
     if (enumError == EnumError.None)
          bfSuccess = true ;

     bfFound = bfSuccess ;
     return bfSuccess ;
}


//----------------------------------------------------------------------------
//                                                                LogException
//----------------------------------------------------------------------------
private void LogException (Exception except, EnumError error)
{
     Type      exception ;

     enumError = error ;

     exception = except.GetType () ;
     strErrMessage = except.Message ;
     strErrTitle   = exception.FullName ;
     //MessageBox.Show (except.Message, exception.FullName, MessageBoxButtons.OK, MessageBoxIcon.Stop) ;
}


//============================================================================
//                                                                        Save
//----------------------------------------------------------------------------
static public Boolean Save ()
{
     return false ;
}


//============================================================================
//                                                                     SetBool
//----------------------------------------------------------------------------
//public void SetBool (string strSection, string strKey, Boolean bfValue)
//{
     //SetString (strSection, strKey, bfValue.ToString (CProgram.CultureInfo)) ;
//}


//============================================================================
//                                                                    SetInt32
//----------------------------------------------------------------------------
//public void SetInt32 (string strSection, string strKey, Int32 iValue)
//{
     //SetString (strSection, strKey, iValue.ToString (CProgram.CultureInfo)) ;
//}


//============================================================================
//                                                                   SetString
//----------------------------------------------------------------------------
/*
 *   SetString() inserts a new key/value pair or updates an existing key 
 *   with the specified value. New pairs are appended to the section. The 
 *   section must already exist.
 *
 *   Key labels will always begin in column 1 (no indentation). Letter case 
 *   is ignored when searching for keys.
 */
public Boolean SetString (string strSection, string strKey, string strValue)
{
     Boolean   bfNewKey ;
     Int32     iEndIdx ;
     Int32     iKeyIdx ;           // Location of key in section
     Int32     iSectionIdx ;       // Location of section in contents
     string    strExtract ;
     string    strExtA ;
     string    strExtB ;
     string    strNew ;

// Init
     bfNewKey = false ;
// Verify arguments are present
     if (string.IsNullOrEmpty (strSection))
          return false ;
     if (string.IsNullOrEmpty (strKey))
          return false ;
     if (string.IsNullOrEmpty (strValue))
          strValue = "" ;
// Extract section from within ini file
     strExtract = GetSection (strSection) ;
     if (strExtract == null)
     {
     // Create new section where needed
          CreateSection (strSection) ;
          strExtract = GetSection (strSection) ;
          if (strExtract == null)
               return false ;
     }
// Locate start of section in full image
     iSectionIdx = strContents.IndexOf (strExtract, StringComparison.CurrentCultureIgnoreCase) ;
     if (iSectionIdx == -1)
          return false ;
// Locate key within extracted section
     iKeyIdx = strExtract.IndexOf ("\r\n" + strKey + " ", StringComparison.CurrentCultureIgnoreCase) ;
     if (iKeyIdx == -1)
     {
          iKeyIdx = strExtract.IndexOf ("\r\n" + strKey + "=", StringComparison.CurrentCultureIgnoreCase) ;
          if (iKeyIdx == -1)
          {
               iKeyIdx  = strSection.Length + 2 ;
               bfNewKey = true ;
          }
     }
// Skip CR/LF at start of needle
     iKeyIdx += 2 ;
// Locate end of line for key
     if (bfNewKey)
          iEndIdx = iKeyIdx ;
     else
     {
          iEndIdx = strExtract.IndexOf ("\r", iKeyIdx, StringComparison.CurrentCultureIgnoreCase) ;
          iEndIdx += 2 ;
     }
// Split into extract A & B
     strExtA = strExtract.Substring (0, iKeyIdx) ;
     strExtB = strExtract.Substring (iEndIdx) ;
// Construct new contents
     strNew  = strContents.Substring (0, iSectionIdx) ;
     strNew += strExtA + strKey + "=" + strValue + "\r\n" + strExtB ;
     strNew += strContents.Substring (iSectionIdx + strExtract.Length) ;
// Check if source was memory-based string
     if (strFileSpec == null)
          strContents = strNew ;
     else
     {
     // Save modified contents of ini file
          File.WriteAllText (strFileSpec, strNew) ;
     // Reload new contents in memory
          strContents = File.ReadAllText (strFileSpec) ;
     }
     return true ;
}


//============================================================================
//                                                               CreateSection
//----------------------------------------------------------------------------
public void CreateSection (string strSection)
{
// Add new section to end of INI contents
     strContents += "\r\n\r\n[" + strSection + "]\r\n\r\n" ;
// Check if source was a stored file
     if (strFileSpec != null)
     {
     // Save modified contents of ini file
          File.WriteAllText (strFileSpec, strContents) ;
     // Reload new contents in memory
          strContents = File.ReadAllText (strFileSpec) ;
     }
}


//============================================================================
//                                                                    GetFloat
//----------------------------------------------------------------------------
/*
 *   GetFloat() searches through the ini file which has been loaded into 
 *   memory when the object was created. The value associated with a specified
 *   key within the specified section of the file image.
 *
 *   A key label must begin in column 1 (no indentation). The space character
 *   is the only permitted character between the key and the '=' sign.
 *
 *   If the key or section is not found, the return value will be Zero.
 */
public float GetFloat (string strSection, string strKey)
{
     float     fValue ;
     string    strValue ;

     strValue = GetString (strSection, strKey) ;
     if (! float.TryParse (strValue, out fValue))
          fValue = 0.0F ;

     return fValue ;
}


//============================================================================
//                                                                     GetBool
//----------------------------------------------------------------------------
public Boolean GetBool (string strSection, string strKey)
{
     Boolean   bfValue ;
     string    strInteger ;

     strInteger = GetString (strSection, strKey) ;
     if (! Boolean.TryParse (strInteger, out bfValue))
          bfValue = false ;

     return bfValue ;
}


//============================================================================
//                                                                    GetInt32
//----------------------------------------------------------------------------
/*
 *   GetInt32() searches through the ini file which has been loaded into
 *   memory when the object was created. The value associated with a specified
 *   key within the specified section of the file image.
 *
 *   A key label must begin in column 1 (no indentation). The space character
 *   is the only permitted character between the key and the '=' sign.
 *
 *   If the key or section is not found, the return value will be Zero.
 */
public Int32 GetInt32 (string strSection, string strKey)
{
     Int32     iValue ;
     string    strInteger ;

     strInteger = GetString (strSection, strKey) ;
     if (! int.TryParse (strInteger, out iValue))
          iValue = 0 ;

     return iValue ;
}


//============================================================================
//                                                                   GetString
//----------------------------------------------------------------------------
/*
 *   GetString() searches through the ini file which has been loaded into
 *   memory when the object was created. The value associated with a specified
 *   key within the specified section of the file image.
 *
 *   A key label must begin in column 1 (no indentation). The space character
 *   is the only permitted character between the key and the '=' sign.
 */
public string GetString (string strSection, string strKey)
{
     Int32     iEnd ;
     Int32     iLength ;
     Int32     iStart ;
     string    strExtract ;
     string    strValue ;

// Extract section from within ini file
     strExtract = GetSection (strSection) ;
     if (strExtract == null)
          return null ;
// Locate key within section
     iStart = strExtract.IndexOf ("\r\n" + strKey + " ", StringComparison.CurrentCultureIgnoreCase) ;
     if (iStart == -1)
     {
          iStart = strExtract.IndexOf ("\r\n" + strKey + "=", StringComparison.CurrentCultureIgnoreCase) ;
          if (iStart == -1)
               return null ;
     }
// Locate value
     iStart = strExtract.IndexOf ("=", iStart, StringComparison.CurrentCultureIgnoreCase) ;
     if (iStart == -1)
          return null ;
// Locate end of line
     iEnd = strExtract.IndexOf ("\r\n", iStart, StringComparison.CurrentCultureIgnoreCase) ;
     if (iEnd == -1)
          iEnd = strExtract.Length ;
// Extract value string
     iStart ++ ;
     iLength = iEnd - iStart ;
     if (iLength == 0)
          return null ;
     strValue = strExtract.Substring (iStart, iLength) ;
// Trim value
     strValue = strValue.TrimStart (' ') ;
     strValue = strValue.TrimEnd (' ') ;
// Return string
     return strValue ;
}


//============================================================================
//                                                                  GetSection
//----------------------------------------------------------------------------
/*
 *   GetSection() extracts the specified section, from the ini file, as a
 *   single string. The section name supplied must not contain square
 *   brackets.
 */
public string GetSection (string strSection)
{
     int       iEnd ;
     int       iLength ;
     int       iStart ;
     string    strExtract ;

// Locate limits of section
     iStart = strContents.IndexOf ("\r\n[" + strSection + "]\r\n", StringComparison.CurrentCultureIgnoreCase) ;
     if (iStart == -1)
     {
     // Check if at the very start of the file
          iStart = strContents.IndexOf ("[" + strSection + "]\r\n", StringComparison.CurrentCultureIgnoreCase) ;
          if (iStart != 0)
               return null ;
     }
// Locate start of next section (or end of file)
     iEnd = strContents.IndexOf ("\r\n[", iStart + 3, StringComparison.CurrentCultureIgnoreCase) ;
     if (iEnd == -1)
          iEnd = strContents.Length ;
     if (iStart > 0)
          iStart += 2 ;
     iLength = iEnd - iStart ;
// Extract section
     strExtract = strContents.Substring (iStart, iLength) ;
     return strExtract ;
}


//============================================================================
//                                                                    ReadLine
//----------------------------------------------------------------------------
/*
 *   ReadLine() reads the next assignment line in an INI section string 
 *   using a pre-existing StringReader object.
 */
static public string ReadLine (StringReader strreader)
{
     int       iIndex ;
     string    strLine ;

// Ignore if string reader object is missing
     if (strreader == null)
          return null ;
// Read through INI section
     while (true)
     {
     // Isolate next line in section string
          strLine = strreader.ReadLine () ;
          if (strLine == null)
               break ;
     // Normalise
          strLine = strLine.Trim (' ') ;
     // Ignore inactive parts within section
          if (strLine.StartsWith ("[", StringComparison.CurrentCultureIgnoreCase))
               continue ;
          if (strLine.StartsWith (";", StringComparison.CurrentCultureIgnoreCase))
               continue ;
     // Separate the ingredient code from its percentage
          iIndex = strLine.IndexOf ('=') ;
     // Stop when next assignment line has been found
          if (iIndex >= 0)
               break ;
     }
     return strLine ;
}


//============================================================================
//                                                                GetListValue
//----------------------------------------------------------------------------
/*
 *   GetListValue() returns an Int32 value for a given keyword within an INI 
 *   section stored as a string.
 */
static public Int32 GetListValue (string strList, string strKeyword)
{
     int       iIndex ;
     int       iValue ;
     string    strEntry ;
     string    strLine ;
     string    strValue ;

     StringReader    strreader ;

// Init
     iValue    = 0 ;
     strreader = new StringReader (strList) ;
// Enumerate lines inside the section
     while (true)
     {
     // Extract next clean line from INI section
          strLine = ReadLine (strreader) ;
          if (strLine == null)
               break ;
     // Separate the keyword from its value
          iIndex   = strLine.IndexOf ('=') ;
          strEntry = strLine.Substring (0, iIndex) ;
          strValue = strLine.Substring (iIndex + 1) ;
     // Check if keyword matches target keyword
          if (strEntry.Equals (strKeyword, StringComparison.CurrentCultureIgnoreCase))
               if (int.TryParse (strValue, out iValue))
                    break ;
     }
// Release resources
     strreader.Dispose () ;
     return iValue ;
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}

//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
