﻿using System.Reflection;
using System.Runtime.InteropServices;
using SampleManualTestCaseConnector;
using Tricentis.TCAddOns;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SampleManualTestCaseConnector")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Tricentis")]
[assembly: AssemblyProduct("SampleManualTestCaseConnector")]
[assembly: AssemblyCopyright("Copyright © Tricentis 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
//The below snippet is required to load the AddOn in Tosca
[assembly: TCAddOnType(typeof(ManualTCInportAddOn))]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5dadac2d-f12a-4d09-b520-ecff1df4c0ce")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
