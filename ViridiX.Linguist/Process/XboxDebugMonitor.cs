using System;
using System.Diagnostics;
using ViridiX.Mason.Collections;
using ViridiX.Mason.Logging;
using ViridiX.Mason.Utilities;

namespace ViridiX.Linguist.Process
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public class XboxDebugMonitor
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Xbox _xbox;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ILogger _logger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Version _version;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private XboxModule _module;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] _data;

        /// <summary>
        /// The Xbox debug monitor module name.
        /// </summary>
        public const string ModuleName = "xbdm.dll";

        /// <summary>
        /// Gets xbdm data from memory. This is cached for future processing.
        /// </summary>
        public byte[] Data => _data ?? (_data = _xbox.Memory.ReadBytes(Module.BaseAddress, Module.Size));

        /// <summary>
        /// Returns the debug monitor version.
        /// </summary>
        public Version Version
        {
            // TODO: figure out a more elegant way to do this
            get
            {
                if (_version != null) return _version;

                int rsrcOffset = 0;
                foreach (var section in Module.Sections)
                {
                    if (!section.Name.Equals(".rsrc")) continue;
                    rsrcOffset = (int)(section.Base - Module.BaseAddress);
                    break;
                }

                if (rsrcOffset != 0)
                {
                    // half-assed VS_VERSIONINFO parsing that works fine for our intended purposes
                    int major = BitConverter.ToUInt16(Data, rsrcOffset + 0x9A);
                    int minor = BitConverter.ToUInt16(Data, rsrcOffset + 0x98);
                    int build = BitConverter.ToUInt16(Data, rsrcOffset + 0x9E);
                    int revision = BitConverter.ToUInt16(Data, rsrcOffset + 0x9C);
                    _version = new Version(major, minor, build, revision);
                    _logger?.Info("Debug monitor version {0} detected", _version);
                }
                else _version = new Version("0.0.0.0");

                return _version;
            }
        }

        /// <summary>
        /// The Xbox debug monitor module.
        /// </summary>
        public XboxModule Module => _module ?? (_module = _xbox.Process.GetModule(ModuleName));

        public XboxDebugMonitor(Xbox xbox)
        {
            if (xbox == null)
                throw new ArgumentNullException(nameof(xbox));

            _xbox = xbox;
            _logger = xbox.Logger;

            EnableRemoteCodeExecution();

            _logger?.Info("XboxDebugMonitor subsystem initialized");
        }

        /// <summary>
        /// The HrFunctionCall function address.
        /// </summary>
        private long HrFunctionCall => Module.BaseAddress + 
            new DefaultDictionary<int, long>(Version.Build)
            {
                // TODO: more versions
                [3146] = 0x4BBB,
                [5455] = 0xCC30,
                [5558] = 0xD870,
                [5933] = 0xD82D,
                [7887] = 0xCC30
            };

        /// <summary>
        /// The DmSetupFunctionCall function address.
        /// </summary>
        private long DmSetupFunctionCall => Module.BaseAddress +
            new DefaultDictionary<int, long>(Version.Build)
            {
                // TODO: more versions
                [5455] = 0x14957,
                [5558] = 0x15B90,
                [7887] = 0x14957
            };

        /// <summary>
        /// The HrReceiveFileData function address.
        /// </summary>
        private long HrReceiveFileData => Module.BaseAddress + 
            new DefaultDictionary<int, long>(Version.Build)
            {
                [3944] = 0x3EA5,
                [4039] = 0x4630,
                [4134] = 0x48E4,
                [4242] = 0x49DC,
                [4361] = 0x54E7,
                [4432] = 0x56D0,
                [4531] = 0x5C4E,
                [4627] = 0x5CDE,
                [4721] = 0x5D9E,
                [4831] = 0x608D,
                [4928] = 0x618D,
                [5028] = 0x63AD,
                [5120] = 0x6557,
                [5233] = 0xB9B7,
                [5344] = 0xBA47,
                [5455] = 0xB372,
                [5558] = 0xBF62,
                [5659] = 0xBF6F,
                [5788] = 0xBF6F,
                [5849] = 0xBF6F,
                [5933] = 0xBF1F,
                [7887] = 0xB372
            };

        /// <summary>
        /// The HrReceivePartialFile function address.
        /// </summary>
        private long HrReceivePartialFile => Module.BaseAddress + 
            new DefaultDictionary<int, long>(Version.Build)
            {
                // unsupported in earlier versions
                [4531] = 0x5E9A,
                [4627] = 0x5F2A,
                [4721] = 0x5FEA,
                [4831] = 0x62D9,
                [4928] = 0x63D9,
                [5028] = 0x65F9,
                [5120] = 0x67A3,
                [5233] = 0xBC03,
                [5344] = 0xBC93,
                [5455] = 0xB5DA,
                [5558] = 0xC1CA,
                [5659] = 0xC1D7,
                [5788] = 0xC1D7,
                [5849] = 0xC1D7,
                [5933] = 0xC187,
                [7887] = 0xB5DA
            };

        /// <summary>
        /// The FGetDwParam function address.
        /// </summary>
        private long FGetDwParam => Module.BaseAddress +
            new DefaultDictionary<int, long>(Version.Build)
            {
                // TODO: more versions
                [3146] = 0x2694,
                [3944] = 0x3384,
                [5558] = 0xA5DE
            };

        /// <summary>
        /// Enables remote code execution by hooking into the HrFunctionCall xbdm function.
        /// </summary>
        private void EnableRemoteCodeExecution()
        {
            const int debugMonitorCustomHeaderSize = 0x40;
            var scriptBuffer = Module.GetSection(".reloc").Base + debugMonitorCustomHeaderSize;

            string[] cave =
            {
                "use32",
                $"org {scriptBuffer}",

                // function prolog
                "push ebp",
                "mov ebp, esp",
                "sub esp, 0Ch",
                "push edi",

                // check for thread id
                "lea eax, [ebp-4]",         // thread id address
                "push eax",
                "push strThread",
                "push dword [ebp+8]",       // command
                $"mov eax, {FGetDwParam}",
                "call eax",
                "test eax, eax",
                "jz immediateCall",

                // call original code if thread id exists
                "push dword [ebp-4]",
                $"mov eax, {DmSetupFunctionCall}",
                "call eax",
                "jmp lblDone",

                // thread argument doesn't exist, must be an immediate call instead
                "immediateCall:",

                // zero out local variable to hold constructed argument name
                "mov dword [ebp-0Ch], 0",
                "mov dword [ebp-8h], 0",

                // push arguments (leave it up to caller to reverse argument order and supply the correct amount)
                "xor edi, edi",             // argument counter
                "nextArg:",

                    // get argument name
                    "push edi",             // argument index
                    "push argNameFormat",   // format string address
                    "lea eax, [ebp-0Ch]",   // argument name address
                    "push eax",
                    $"mov eax, {_xbox.Kernel.Exports.sprintf}",
                    "call eax",
                    "add esp, 0Ch",

                    // check if it's included in the command
                    "lea eax, [ebp-4]",     // argument value address
                    "push eax",
                    "lea eax, [ebp-0Ch]",   // argument name address
                    "push eax",
                    "push dword [ebp+8]",   // command
                    $"mov eax, {FGetDwParam}",
                    "call eax",
                    "test eax, eax",
                    "jz noMoreArgs",

                    // push it on the stack
                    "push dword [ebp-4]",
                    "inc edi",

                    // move on to the next argument
                    "jmp nextArg",

                "noMoreArgs:",
              
                // get the call address
                "lea eax, [ebp-4]",         // argument value address
                "push eax",
                "push strAddr",
                "push dword [ebp+8]",       // command
                $"mov eax, {FGetDwParam}",
                "call eax",
                "test eax, eax",
                "jz lblError",

                // TODO: set fastcall register context (ecx, edx, xmm0-xmm2)

                // perform the call
                "call dword [ebp-4]",

                // print response message
                "fst dword [ebp-4]",        
                "push dword [ebp-4]",       // floating-point return value
                "push eax",                 // integer return value
                "push returnValuesFormat",  // format string address
                "push dword [ebp+0Ch]",     // response address
                $"mov eax, {_xbox.Kernel.Exports.sprintf}",
                "call eax",
                "add esp, 10h",

                // return codes
                "mov eax, 02DB0000h",       // success
                "jmp lblDone",
                "lblError:",
                "shl edi, 2",
                "add esp, edi",             // remove arguments from the stack since the call wasn't performed
                "mov eax, 82DB0000h",       // error
                "lblDone:",

                // function epilog
                "pop edi",
                "leave",
                "retn 10h",

                // variables
                "strAddr db 'addr', 0",
                "strThread db 'thread', 0",
                "argNameFormat db 'arg%01d', 0",
                "returnValuesFormat db 'eax=0x%X st0=0x%X', 0"
            };

            // write the cave
            byte[] caveBytes = Assembler.Assemble(cave, scriptBuffer);
            _xbox.Memory.WriteBytes(scriptBuffer, caveBytes);

            // write the hook
            byte[] hookBytes = Assembler.Assemble(new[] { $"push {scriptBuffer}", "retn" });
            _xbox.Memory.WriteBytes(HrFunctionCall, hookBytes);
        }
    }
}
