using System.Reflection;
using System.Runtime.InteropServices;

#if (NET_1_0)

#elif (NET_1_1)
[assembly: AssemblyTitle("Enyim.Caching for .Net 1.1")]
#elif (NET_2_0)
[assembly: AssemblyTitle("Enyim.Caching for .Net 2.0")]
#elif (NET_3_0)
[assembly: AssemblyTitle("Enyim.Caching for .Net 3.0")]
#elif (NET_3_5)
[assembly: AssemblyTitle("Enyim.Caching for .Net 3.5")]
#elif (NET_4_0)

[assembly: AssemblyTitle("Enyim.Caching for .Net 4.0")]
#elif (NET_4_5)
[assembly: AssemblyTitle("Enyim.Caching for .Net 4.5")]
#elif (NET_4_5_1)
[assembly: AssemblyTitle("Enyim.Caching for .Net  4.5.1")]
#elif (NETCF_1_0)
[assembly: AssemblyTitle("Enyim.Caching for .NET Compact Framework 1.0")]
#elif (NETCF_2_0)
[assembly: AssemblyTitle("Enyim.Caching for .NET Compact Framework 2.0")]
#elif (MONO_1_0)
[assembly: AssemblyTitle("Enyim.Caching for Mono 1.0")]
#elif (MONO_2_0)
[assembly: AssemblyTitle("Enyim.Caching for Mono 2.0")]
#else
using System.Reflection;
using System.Runtime.InteropServices;
[assembly: AssemblyTitle("Enyim")]
#endif

[assembly: AssemblyDescription("Enyim.Caching")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyProduct("Enyim.Caching(Debug)")]
#else
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyProduct("Enyim.Caching")]
#endif



[assembly: AssemblyCompany("enyim.com")]
[assembly: AssemblyCopyright("Copyright © enyim.com, Attila Kiskó 2007-2010")]

[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("a8cd3f39-7731-4ee8-a7ce-444d540d4a4d")]

#region [ License information          ]
/* ************************************************************
 * 
 *    Copyright (c) 2010 Attila Kiskó, enyim.com
 *    
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *    
 *        http://www.apache.org/licenses/LICENSE-2.0
 *    
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *    
 * ************************************************************/
#endregion