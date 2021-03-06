﻿//****************************************************************************
//
//   (c) Programify Ltd
//   Application Entry and Exit Point                              CProgram.cs
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   19-12-19  Created project.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


//****************************************************************************
//                                                                   Namespace
//****************************************************************************
namespace Uninstall
{


//****************************************************************************
//                                                                       Class
//****************************************************************************
static class CProgram
{
//-------------------------------------------------------------- Static Public

//--------------------------------------------------------------------- Public

//-------------------------------------------------------------------- Private

//-------------------------------------------------------------------- Structs

//----------------------------------------------------------------- Properties


//****************************************************************************
//                                                                   Functions
//****************************************************************************


//============================================================================
//                                                                        Main
//============================================================================
[STAThread]
static void Main ()
{
     CFormUninstall      cForm ;

// Init Windows .NET Form application
     Application.EnableVisualStyles () ;
     Application.SetCompatibleTextRenderingDefault (false) ;
// Pass control to Windows form
     cForm = new CFormUninstall () ;
     Application.Run (cForm) ;
     cForm.Dispose () ;
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
