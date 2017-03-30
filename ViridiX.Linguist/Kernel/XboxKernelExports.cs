using System;

namespace ViridiX.Linguist.Kernel
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public class XboxKernelExports
    {
        #region Exports

        /// <summary>
        /// 
        /// </summary>
        public long AvGetSavedDataAddress { get; }

        /// <summary>
        /// 
        /// </summary>
        public long AvSendTVEncoderOption { get; }

        /// <summary>
        /// 
        /// </summary>
        public long AvSetDisplayMode { get; }

        /// <summary>
        /// 
        /// </summary>
        public long AvSetSavedDataAddress { get; }

        /// <summary>
        /// 
        /// </summary>
        public long DbgBreakPoint { get; }

        /// <summary>
        /// 
        /// </summary>
        public long DbgBreakPointWithStatus { get; }

        /// <summary>
        /// 
        /// </summary>
        public long DbgLoadImageSymbols { get; }

        /// <summary>
        /// 
        /// </summary>
        public long DbgPrint { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalReadSmcTrayState { get; }

        /// <summary>
        /// 
        /// </summary>
        public long DbgPrompt { get; }

        /// <summary>
        /// 
        /// </summary>
        public long DbgUnLoadImageSymbols { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExAcquireReadWriteLockExclusive { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExAcquireReadWriteLockShared { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExAllocatePool { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExAllocatePoolWithTag { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExEventObjectType { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExFreePool { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExInitializeReadWriteLock { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExInterlockedAddLargeInteger { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExInterlockedAddLargeStatistic { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExInterlockedCompareExchange64 { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExMutantObjectType { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExQueryPoolBlockSize { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExQueryNonVolatileSetting { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExReadWriteRefurbInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExRaiseException { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExRaiseStatus { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExReleaseReadWriteLock { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExSaveNonVolatileSetting { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExSemaphoreObjectType { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExTimerObjectType { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExfInterlockedInsertHeadList { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExfInterlockedInsertTailList { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ExfInterlockedRemoveHeadList { get; }

        /// <summary>
        /// 
        /// </summary>
        public long FscGetCacheSize { get; }

        /// <summary>
        /// 
        /// </summary>
        public long FscInvalidateIdleBlocks { get; }

        /// <summary>
        /// 
        /// </summary>
        public long FscSetCacheSize { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalClearSoftwareInterrupt { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalDisableSystemInterrupt { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IdexDiskPartitionPrefixBuffer { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalDiskModelNumber { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalDiskSerialNumber { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalEnableSystemInterrupt { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalGetInterruptVector { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalReadSMBusValue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalReadWritePCISpace { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalRegisterShutdownNotification { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalRequestSoftwareInterrupt { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalReturnToFirmware { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalWriteSMBusValue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long InterlockedCompareExchange { get; }

        /// <summary>
        /// 
        /// </summary>
        public long InterlockedDecrement { get; }

        /// <summary>
        /// 
        /// </summary>
        public long InterlockedIncrement { get; }

        /// <summary>
        /// 
        /// </summary>
        public long InterlockedExchange { get; }

        /// <summary>
        /// 
        /// </summary>
        public long InterlockedExchangeAdd { get; }

        /// <summary>
        /// 
        /// </summary>
        public long InterlockedFlushSList { get; }

        /// <summary>
        /// 
        /// </summary>
        public long InterlockedPopEntrySList { get; }

        /// <summary>
        /// 
        /// </summary>
        public long InterlockedPushEntrySList { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoAllocateIrp { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoBuildAsynchronousFsdRequest { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoBuildDeviceIoControlRequest { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoBuildSynchronousFsdRequest { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoCheckShareAccess { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoCompletionObjectType { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoCreateDevice { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoCreateFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoCreateSymbolicLink { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoDeleteDevice { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoDeleteSymbolicLink { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoFileObjectType { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoFreeIrp { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoInitializeIrp { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoInvalidDeviceRequest { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoQueryFileInformation { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoQueryVolumeInformation { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoQueueThreadIrp { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoRemoveShareAccess { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoSetIoCompletion { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoSetShareAccess { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoStartNextPacket { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoStartNextPacketByKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoStartPacket { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoSynchronousDeviceIoControlRequest { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoSynchronousFsdRequest { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IofCallDriver { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IofCompleteRequest { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KdDebuggerEnabled { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KdDebuggerNotPresent { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoDismountVolume { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoDismountVolumeByName { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeAlertResumeThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeAlertThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeBoostPriorityThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeBugCheck { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeBugCheckEx { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeCancelTimer { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeConnectInterrupt { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeDelayExecutionThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeDisconnectInterrupt { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeEnterCriticalRegion { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmGlobalData { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeGetCurrentIrql { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeGetCurrentThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInitializeApc { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInitializeDeviceQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInitializeDpc { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInitializeEvent { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInitializeInterrupt { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInitializeMutant { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInitializeQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInitializeSemaphore { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInitializeTimerEx { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInsertByKeyDeviceQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInsertDeviceQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInsertHeadQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInsertQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInsertQueueApc { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInsertQueueDpc { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeInterruptTime { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeIsExecutingDpc { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeLeaveCriticalRegion { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KePulseEvent { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeQueryBasePriorityThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeQueryInterruptTime { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeQueryPerformanceCounter { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeQueryPerformanceFrequency { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeQuerySystemTime { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeRaiseIrqlToDpcLevel { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeRaiseIrqlToSynchLevel { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeReleaseMutant { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeReleaseSemaphore { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeRemoveByKeyDeviceQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeRemoveDeviceQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeRemoveEntryDeviceQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeRemoveQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeRemoveQueueDpc { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeResetEvent { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeRestoreFloatingPointState { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeResumeThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeRundownQueue { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSaveFloatingPointState { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSetBasePriorityThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSetDisableBoostThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSetEvent { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSetEventBoostPriority { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSetPriorityProcess { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSetPriorityThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSetTimer { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSetTimerEx { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeStallExecutionProcessor { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSuspendThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSynchronizeExecution { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeSystemTime { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeTestAlertThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeTickCount { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeTimeIncrement { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeWaitForMultipleObjects { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KeWaitForSingleObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KfRaiseIrql { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KfLowerIrql { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KiBugCheckData { get; }

        /// <summary>
        /// 
        /// </summary>
        public long KiUnlockDispatcherDatabase { get; }

        /// <summary>
        /// 
        /// </summary>
        public long LaunchDataPage { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmAllocateContiguousMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmAllocateContiguousMemoryEx { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmAllocateSystemMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmClaimGpuInstanceMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmCreateKernelStack { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmDeleteKernelStack { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmFreeContiguousMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmFreeSystemMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmGetPhysicalAddress { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmIsAddressValid { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmLockUnlockBufferPages { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmLockUnlockPhysicalPage { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmMapIoSpace { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmPersistContiguousMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmQueryAddressProtect { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmQueryAllocationSize { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmQueryStatistics { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmSetAddressProtect { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmUnmapIoSpace { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtAllocateVirtualMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtCancelTimer { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtClearEvent { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtClose { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtCreateDirectoryObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtCreateEvent { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtCreateFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtCreateIoCompletion { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtCreateMutant { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtCreateSemaphore { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtCreateTimer { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtDeleteFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtDeviceIoControlFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtDuplicateObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtFlushBuffersFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtFreeVirtualMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtFsControlFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtOpenDirectoryObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtOpenFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtOpenSymbolicLinkObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtProtectVirtualMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtPulseEvent { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueueApcThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueryDirectoryFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueryDirectoryObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueryEvent { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueryFullAttributesFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueryInformationFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueryIoCompletion { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueryMutant { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQuerySemaphore { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQuerySymbolicLinkObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueryTimer { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueryVirtualMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtQueryVolumeInformationFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtReadFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtReadFileScatter { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtReleaseMutant { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtReleaseSemaphore { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtRemoveIoCompletion { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtResumeThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtSetEvent { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtSetInformationFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtSetIoCompletion { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtSetSystemTime { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtSetTimerEx { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtSignalAndWaitForSingleObjectEx { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtSuspendThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtUserIoApcDispatcher { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtWaitForSingleObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtWaitForSingleObjectEx { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtWaitForMultipleObjectsEx { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtWriteFile { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtWriteFileGather { get; }

        /// <summary>
        /// 
        /// </summary>
        public long NtYieldExecution { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObCreateObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObDirectoryObjectType { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObInsertObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObMakeTemporaryObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObOpenObjectByName { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObOpenObjectByPointer { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObpObjectHandleTable { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObReferenceObjectByHandle { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObReferenceObjectByName { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObReferenceObjectByPointer { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObSymbolicLinkObjectType { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObfDereferenceObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long ObfReferenceObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long PhyGetLinkState { get; }

        /// <summary>
        /// 
        /// </summary>
        public long PhyInitialize { get; }

        /// <summary>
        /// 
        /// </summary>
        public long PsCreateSystemThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long PsCreateSystemThreadEx { get; }

        /// <summary>
        /// 
        /// </summary>
        public long PsQueryStatistics { get; }

        /// <summary>
        /// 
        /// </summary>
        public long PsSetCreateThreadNotifyRoutine { get; }

        /// <summary>
        /// 
        /// </summary>
        public long PsTerminateSystemThread { get; }

        /// <summary>
        /// 
        /// </summary>
        public long PsThreadObjectType { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlAnsiStringToUnicodeString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlAppendStringToString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlAppendUnicodeStringToString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlAppendUnicodeToString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlAssert { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlCaptureContext { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlCaptureStackBackTrace { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlCharToInteger { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlCompareMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlCompareMemoryUlong { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlCompareString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlCompareUnicodeString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlCopyString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlCopyUnicodeString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlCreateUnicodeString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlDowncaseUnicodeChar { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlDowncaseUnicodeString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlEnterCriticalSection { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlEnterCriticalSectionAndRegion { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlEqualString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlEqualUnicodeString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlExtendedIntegerMultiply { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlExtendedLargeIntegerDivide { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlExtendedMagicDivide { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlFillMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlFillMemoryUlong { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlFreeAnsiString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlGetCallersAddress { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlInitAnsiString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlInitUnicodeString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlInitializeCriticalSection { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlIntegerToChar { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlIntegerToUnicodeString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlLeaveCriticalSection { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlLeaveCriticalSectionAndRegion { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlLowerChar { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlMapGenericMask { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlMoveMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlMultiByteToUnicodeN { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlMultiByteToUnicodeSize { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlNtStatusToDosError { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlRaiseException { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlRaiseStatus { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlTimeFieldsToTime { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlTimeToTimeFields { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlTryEnterCriticalSection { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUlongByteSwap { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUnicodeStringToAnsiString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUnicodeStringToInteger { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUnicodeToMultiByteN { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUnicodeToMultiByteSize { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUnwind { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUpcaseUnicodeChar { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUpcaseUnicodeString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUpcaseUnicodeToMultiByteN { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUpperChar { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUpperString { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlUshortByteSwap { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlWalkFrameChain { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlZeroMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XboxEEPROMKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HardwareInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XboxHDKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XboxKrnlVersion { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XboxSignatureKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XeImageFileName { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XeLoadSection { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XeUnloadSection { get; }

        /// <summary>
        /// 
        /// </summary>
        public long READ_PORT_BUFFER_UCHAR { get; }

        /// <summary>
        /// 
        /// </summary>
        public long READ_PORT_BUFFER_USHORT { get; }

        /// <summary>
        /// 
        /// </summary>
        public long READ_PORT_BUFFER_ULONG { get; }

        /// <summary>
        /// 
        /// </summary>
        public long WRITE_PORT_BUFFER_UCHAR { get; }

        /// <summary>
        /// 
        /// </summary>
        public long WRITE_PORT_BUFFER_USHORT { get; }

        /// <summary>
        /// 
        /// </summary>
        public long WRITE_PORT_BUFFER_ULONG { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcSHAInit { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcSHAUpdate { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcSHAFinal { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcRC4Key { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcRC4Crypt { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcHMAC { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcPKEncPublic { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcPKDecPrivate { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcPKGetKeyLen { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcVerifyPKCS1Signature { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcModExp { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcDESKeyParity { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcKeyTable { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcBlockCrypt { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcBlockCryptCBC { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcCryptService { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XcUpdateCrypto { get; }

        /// <summary>
        /// 
        /// </summary>
        public long RtlRip { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XboxLANKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XboxAlternateSignatureKeys { get; }

        /// <summary>
        /// 
        /// </summary>
        public long XePublicKeyData { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalBootSMCVideoMode { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IdexChannelObject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalIsResetOrShutdownPending { get; }

        /// <summary>
        /// 
        /// </summary>
        public long IoMarkIrpMustComplete { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalInitiateShutdown { get; }

        /// <summary>
        /// 
        /// </summary>
        public long snprintf { get; }

        /// <summary>
        /// 
        /// </summary>
        public long sprintf { get; }

        /// <summary>
        /// 
        /// </summary>
        public long vsnprintf { get; }

        /// <summary>
        /// 
        /// </summary>
        public long vsprintf { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalEnableSecureTrayEject { get; }

        /// <summary>
        /// 
        /// </summary>
        public long HalWriteSmcScratchRegister { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmDbgAllocateMemory { get; }

        /// <summary>
        /// Returns number of pages released.
        /// </summary>
        public long MmDbgFreeMemory { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmDbgQueryAvailablePages { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmDbgReleaseAddress { get; }

        /// <summary>
        /// 
        /// </summary>
        public long MmDbgWriteCheck { get; }

        #endregion

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="table"></param>
        public XboxKernelExports(long[] table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            // Index signifies ordinal number
            AvGetSavedDataAddress = table[1];
            AvSendTVEncoderOption = table[2];
            AvSetDisplayMode = table[3];
            AvSetSavedDataAddress = table[4];
            DbgBreakPoint = table[5];
            DbgBreakPointWithStatus = table[6];
            DbgLoadImageSymbols = table[7];
            DbgPrint = table[8];
            HalReadSmcTrayState = table[9];
            DbgPrompt = table[10];
            DbgUnLoadImageSymbols = table[11];
            ExAcquireReadWriteLockExclusive = table[12];
            ExAcquireReadWriteLockShared = table[13];
            ExAllocatePool = table[14];
            ExAllocatePoolWithTag = table[15];
            ExEventObjectType = table[16];
            ExFreePool = table[17];
            ExInitializeReadWriteLock = table[18];
            ExInterlockedAddLargeInteger = table[19];
            ExInterlockedAddLargeStatistic = table[20];
            ExInterlockedCompareExchange64 = table[21];
            ExMutantObjectType = table[22];
            ExQueryPoolBlockSize = table[23];
            ExQueryNonVolatileSetting = table[24];
            ExReadWriteRefurbInfo = table[25];
            ExRaiseException = table[26];
            ExRaiseStatus = table[27];
            ExReleaseReadWriteLock = table[28];
            ExSaveNonVolatileSetting = table[29];
            ExSemaphoreObjectType = table[30];
            ExTimerObjectType = table[31];
            ExfInterlockedInsertHeadList = table[32];
            ExfInterlockedInsertTailList = table[33];
            ExfInterlockedRemoveHeadList = table[34];
            FscGetCacheSize = table[35];
            FscInvalidateIdleBlocks = table[36];
            FscSetCacheSize = table[37];
            HalClearSoftwareInterrupt = table[38];
            HalDisableSystemInterrupt = table[39];
            IdexDiskPartitionPrefixBuffer = table[40];
            HalDiskModelNumber = table[41];
            HalDiskSerialNumber = table[42];
            HalEnableSystemInterrupt = table[43];
            HalGetInterruptVector = table[44];
            HalReadSMBusValue = table[45];
            HalReadWritePCISpace = table[46];
            HalRegisterShutdownNotification = table[47];
            HalRequestSoftwareInterrupt = table[48];
            HalReturnToFirmware = table[49];
            HalWriteSMBusValue = table[50];
            InterlockedCompareExchange = table[51];
            InterlockedDecrement = table[52];
            InterlockedIncrement = table[53];
            InterlockedExchange = table[54];
            InterlockedExchangeAdd = table[55];
            InterlockedFlushSList = table[56];
            InterlockedPopEntrySList = table[57];
            InterlockedPushEntrySList = table[58];
            IoAllocateIrp = table[59];
            IoBuildAsynchronousFsdRequest = table[60];
            IoBuildDeviceIoControlRequest = table[61];
            IoBuildSynchronousFsdRequest = table[62];
            IoCheckShareAccess = table[63];
            IoCompletionObjectType = table[64];
            IoCreateDevice = table[65];
            IoCreateFile = table[66];
            IoCreateSymbolicLink = table[67];
            IoDeleteDevice = table[68];
            IoDeleteSymbolicLink = table[69];
            IoFileObjectType = table[71];
            IoFreeIrp = table[72];
            IoInitializeIrp = table[73];
            IoInvalidDeviceRequest = table[74];
            IoQueryFileInformation = table[75];
            IoQueryVolumeInformation = table[76];
            IoQueueThreadIrp = table[77];
            IoRemoveShareAccess = table[78];
            IoSetIoCompletion = table[79];
            IoSetShareAccess = table[80];
            IoStartNextPacket = table[81];
            IoStartNextPacketByKey = table[82];
            IoStartPacket = table[83];
            IoSynchronousDeviceIoControlRequest = table[84];
            IoSynchronousFsdRequest = table[85];
            IofCallDriver = table[86];
            IofCompleteRequest = table[87];
            KdDebuggerEnabled = table[88];
            KdDebuggerNotPresent = table[89];
            IoDismountVolume = table[90];
            IoDismountVolumeByName = table[91];
            KeAlertResumeThread = table[92];
            KeAlertThread = table[93];
            KeBoostPriorityThread = table[94];
            KeBugCheck = table[95];
            KeBugCheckEx = table[96];
            KeCancelTimer = table[97];
            KeConnectInterrupt = table[98];
            KeDelayExecutionThread = table[99];
            KeDisconnectInterrupt = table[100];
            KeEnterCriticalRegion = table[101];
            MmGlobalData = table[102];
            KeGetCurrentIrql = table[103];
            KeGetCurrentThread = table[104];
            KeInitializeApc = table[105];
            KeInitializeDeviceQueue = table[106];
            KeInitializeDpc = table[107];
            KeInitializeEvent = table[108];
            KeInitializeInterrupt = table[109];
            KeInitializeMutant = table[110];
            KeInitializeQueue = table[111];
            KeInitializeSemaphore = table[112];
            KeInitializeTimerEx = table[113];
            KeInsertByKeyDeviceQueue = table[114];
            KeInsertDeviceQueue = table[115];
            KeInsertHeadQueue = table[116];
            KeInsertQueue = table[117];
            KeInsertQueueApc = table[118];
            KeInsertQueueDpc = table[119];
            KeInterruptTime = table[120];
            KeIsExecutingDpc = table[121];
            KeLeaveCriticalRegion = table[122];
            KePulseEvent = table[123];
            KeQueryBasePriorityThread = table[124];
            KeQueryInterruptTime = table[125];
            KeQueryPerformanceCounter = table[126];
            KeQueryPerformanceFrequency = table[127];
            KeQuerySystemTime = table[128];
            KeRaiseIrqlToDpcLevel = table[129];
            KeRaiseIrqlToSynchLevel = table[130];
            KeReleaseMutant = table[131];
            KeReleaseSemaphore = table[132];
            KeRemoveByKeyDeviceQueue = table[133];
            KeRemoveDeviceQueue = table[134];
            KeRemoveEntryDeviceQueue = table[135];
            KeRemoveQueue = table[136];
            KeRemoveQueueDpc = table[137];
            KeResetEvent = table[138];
            KeRestoreFloatingPointState = table[139];
            KeResumeThread = table[140];
            KeRundownQueue = table[141];
            KeSaveFloatingPointState = table[142];
            KeSetBasePriorityThread = table[143];
            KeSetDisableBoostThread = table[144];
            KeSetEvent = table[145];
            KeSetEventBoostPriority = table[146];
            KeSetPriorityProcess = table[147];
            KeSetPriorityThread = table[148];
            KeSetTimer = table[149];
            KeSetTimerEx = table[150];
            KeStallExecutionProcessor = table[151];
            KeSuspendThread = table[152];
            KeSynchronizeExecution = table[153];
            KeSystemTime = table[154];
            KeTestAlertThread = table[155];
            KeTickCount = table[156];
            KeTimeIncrement = table[157];
            KeWaitForMultipleObjects = table[158];
            KeWaitForSingleObject = table[159];
            KfRaiseIrql = table[160];
            KfLowerIrql = table[161];
            KiBugCheckData = table[162];
            KiUnlockDispatcherDatabase = table[163];
            LaunchDataPage = table[164];
            MmAllocateContiguousMemory = table[165];
            MmAllocateContiguousMemoryEx = table[166];
            MmAllocateSystemMemory = table[167];
            MmClaimGpuInstanceMemory = table[168];
            MmCreateKernelStack = table[169];
            MmDeleteKernelStack = table[170];
            MmFreeContiguousMemory = table[171];
            MmFreeSystemMemory = table[172];
            MmGetPhysicalAddress = table[172];
            MmIsAddressValid = table[174];
            MmLockUnlockBufferPages = table[175];
            MmLockUnlockPhysicalPage = table[176];
            MmMapIoSpace = table[177];
            MmPersistContiguousMemory = table[178];
            MmQueryAddressProtect = table[179];
            MmQueryAllocationSize = table[180];
            MmQueryStatistics = table[181];
            MmSetAddressProtect = table[182];
            MmUnmapIoSpace = table[183];
            NtAllocateVirtualMemory = table[184];
            NtCancelTimer = table[185];
            NtClearEvent = table[186];
            NtClose = table[187];
            NtCreateDirectoryObject = table[188];
            NtCreateEvent = table[189];
            NtCreateFile = table[190];
            NtCreateIoCompletion = table[191];
            NtCreateMutant = table[192];
            NtCreateSemaphore = table[193];
            NtCreateTimer = table[194];
            NtDeleteFile = table[195];
            NtDeviceIoControlFile = table[196];
            NtDuplicateObject = table[197];
            NtFlushBuffersFile = table[198];
            NtFreeVirtualMemory = table[199];
            NtFsControlFile = table[200];
            NtOpenDirectoryObject = table[201];
            NtOpenFile = table[202];
            NtOpenSymbolicLinkObject = table[203];
            NtProtectVirtualMemory = table[204];
            NtPulseEvent = table[205];
            NtQueueApcThread = table[206];
            NtQueryDirectoryFile = table[207];
            NtQueryDirectoryObject = table[208];
            NtQueryEvent = table[209];
            NtQueryFullAttributesFile = table[210];
            NtQueryInformationFile = table[211];
            NtQueryIoCompletion = table[212];
            NtQueryMutant = table[213];
            NtQuerySemaphore = table[214];
            NtQuerySymbolicLinkObject = table[215];
            NtQueryTimer = table[216];
            NtQueryVirtualMemory = table[217];
            NtQueryVolumeInformationFile = table[218];
            NtReadFile = table[219];
            NtReadFileScatter = table[220];
            NtReleaseMutant = table[221];
            NtReleaseSemaphore = table[222];
            NtRemoveIoCompletion = table[223];
            NtResumeThread = table[224];
            NtSetEvent = table[225];
            NtSetInformationFile = table[226];
            NtSetIoCompletion = table[227];
            NtSetSystemTime = table[228];
            NtSetTimerEx = table[229];
            NtSignalAndWaitForSingleObjectEx = table[230];
            NtSuspendThread = table[231];
            NtUserIoApcDispatcher = table[232];
            NtWaitForSingleObject = table[233];
            NtWaitForSingleObjectEx = table[234];
            NtWaitForMultipleObjectsEx = table[235];
            NtWriteFile = table[236];
            NtWriteFileGather = table[237];
            NtYieldExecution = table[238];
            ObCreateObject = table[239];
            ObDirectoryObjectType = table[240];
            ObInsertObject = table[241];
            ObMakeTemporaryObject = table[242];
            ObOpenObjectByName = table[243];
            ObOpenObjectByPointer = table[244];
            ObpObjectHandleTable = table[245];
            ObReferenceObjectByHandle = table[246];
            ObReferenceObjectByName = table[247];
            ObReferenceObjectByPointer = table[248];
            ObSymbolicLinkObjectType = table[249];
            ObfDereferenceObject = table[250];
            ObfReferenceObject = table[251];
            PhyGetLinkState = table[252];
            PhyInitialize = table[253];
            PsCreateSystemThread = table[254];
            PsCreateSystemThreadEx = table[255];
            PsQueryStatistics = table[256];
            PsSetCreateThreadNotifyRoutine = table[257];
            PsTerminateSystemThread = table[258];
            PsThreadObjectType = table[259];
            RtlAnsiStringToUnicodeString = table[260];
            RtlAppendStringToString = table[261];
            RtlAppendUnicodeStringToString = table[262];
            RtlAppendUnicodeToString = table[263];
            RtlAssert = table[264];
            RtlCaptureContext = table[265];
            RtlCaptureStackBackTrace = table[266];
            RtlCharToInteger = table[267];
            RtlCompareMemory = table[268];
            RtlCompareMemoryUlong = table[269];
            RtlCompareString = table[270];
            RtlCompareUnicodeString = table[271];
            RtlCopyString = table[272];
            RtlCopyUnicodeString = table[273];
            RtlCreateUnicodeString = table[274];
            RtlDowncaseUnicodeChar = table[275];
            RtlDowncaseUnicodeString = table[276];
            RtlEnterCriticalSection = table[277];
            RtlEnterCriticalSectionAndRegion = table[278];
            RtlEqualString = table[279];
            RtlEqualUnicodeString = table[280];
            RtlExtendedIntegerMultiply = table[281];
            RtlExtendedLargeIntegerDivide = table[282];
            RtlExtendedMagicDivide = table[283];
            RtlFillMemory = table[284];
            RtlFillMemoryUlong = table[285];
            RtlFreeAnsiString = table[286];
            RtlGetCallersAddress = table[288];
            RtlInitAnsiString = table[289];
            RtlInitUnicodeString = table[290];
            RtlInitializeCriticalSection = table[291];
            RtlIntegerToChar = table[292];
            RtlIntegerToUnicodeString = table[293];
            RtlLeaveCriticalSection = table[294];
            RtlLeaveCriticalSectionAndRegion = table[295];
            RtlLowerChar = table[296];
            RtlMapGenericMask = table[297];
            RtlMoveMemory = table[298];
            RtlMultiByteToUnicodeN = table[299];
            RtlMultiByteToUnicodeSize = table[300];
            RtlNtStatusToDosError = table[301];
            RtlRaiseException = table[302];
            RtlRaiseStatus = table[303];
            RtlTimeFieldsToTime = table[304];
            RtlTimeToTimeFields = table[305];
            RtlTryEnterCriticalSection = table[306];
            RtlUlongByteSwap = table[307];
            RtlUnicodeStringToAnsiString = table[308];
            RtlUnicodeStringToInteger = table[309];
            RtlUnicodeToMultiByteN = table[310];
            RtlUnicodeToMultiByteSize = table[311];
            RtlUnwind = table[312];
            RtlUpcaseUnicodeChar = table[313];
            RtlUpcaseUnicodeString = table[314];
            RtlUpcaseUnicodeToMultiByteN = table[315];
            RtlUpperChar = table[316];
            RtlUpperString = table[317];
            RtlUshortByteSwap = table[318];
            RtlWalkFrameChain = table[319];
            RtlZeroMemory = table[320];
            XboxEEPROMKey = table[321];
            HardwareInfo = table[322];
            XboxHDKey = table[323];
            XboxKrnlVersion = table[324];
            XboxSignatureKey = table[325];
            XeImageFileName = table[326];
            XeLoadSection = table[327];
            XeUnloadSection = table[328];
            READ_PORT_BUFFER_UCHAR = table[329];
            READ_PORT_BUFFER_USHORT = table[330];
            READ_PORT_BUFFER_ULONG = table[331];
            WRITE_PORT_BUFFER_UCHAR = table[332];
            WRITE_PORT_BUFFER_USHORT = table[333];
            WRITE_PORT_BUFFER_ULONG = table[334];
            XcSHAInit = table[335];
            XcSHAUpdate = table[336];
            XcSHAFinal = table[337];
            XcRC4Key = table[338];
            XcRC4Crypt = table[339];
            XcHMAC = table[340];
            XcPKEncPublic = table[341];
            XcPKDecPrivate = table[342];
            XcPKGetKeyLen = table[343];
            XcVerifyPKCS1Signature = table[344];
            XcModExp = table[345];
            XcDESKeyParity = table[346];
            XcKeyTable = table[347];
            XcBlockCrypt = table[348];
            XcBlockCryptCBC = table[349];
            XcCryptService = table[350];
            XcUpdateCrypto = table[351];
            RtlRip = table[352];
            XboxLANKey = table[353];
            XboxAlternateSignatureKeys = table[354];
            XePublicKeyData = table[355];
            HalBootSMCVideoMode = table[356];
            IdexChannelObject = table[357];
            HalIsResetOrShutdownPending = table[358];
            IoMarkIrpMustComplete = table[359];
            HalInitiateShutdown = table[360];
            snprintf = table[361];
            sprintf = table[362];
            vsnprintf = table[363];
            vsprintf = table[364];
            HalEnableSecureTrayEject = table[365];
            HalWriteSmcScratchRegister = table[366];
            MmDbgAllocateMemory = table[374];
            MmDbgFreeMemory = table[375];
            MmDbgQueryAvailablePages = table[376];
            MmDbgReleaseAddress = table[377];
            MmDbgWriteCheck = table[378];
        }
    }
}
