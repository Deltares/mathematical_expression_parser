namespace Parser.AssemblyInfo

open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[<assembly: AssemblyTitle("MathematicalExpressionParser.Core")>]
[<assembly: AssemblyDescription("The MathematicalExpressionParser.Core library is used to parse mathematical-expression strings to symbolic-expression trees.")>]
[<assembly: AssemblyConfiguration("")>]
[<assembly: AssemblyCompany("Deltares")>]
[<assembly: AssemblyProduct("MathematicalExpressionParser.Core")>]
[<assembly: AssemblyCopyright("Copyright © Deltares 2020")>]
[<assembly: AssemblyTrademark("")>]
[<assembly: AssemblyCulture("")>]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[<assembly: ComVisible(false)>]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[<assembly: Guid("7710a260-d2b1-471f-804e-4446e59511a9")>]

// Version information for an assembly consists of the following four values:
//
//       Major Version
//       Minor Version
//       Build Number
//       Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [<assembly: AssemblyVersion("1.0.*")>]
[<assembly: AssemblyVersion("1.0.0.0")>]
[<assembly: AssemblyFileVersion("1.0.0.0")>]

// Internals visible to:
[<assembly: InternalsVisibleTo("MathematicalExpressionParser.Core.Test")>]

do
    ()