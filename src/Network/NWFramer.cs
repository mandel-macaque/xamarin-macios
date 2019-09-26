//
// NWFramer.cs: Bindings the Network nw_framer_t API.
//
// Authors:
//   Manuel de la Pena (mandel@microsoft.com)
//
// Copyright 2019 Microsoft Inc
//
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

using OS_nw_framer=System.IntPtr;
using OS_nw_protocol_metadata=System.IntPtr;
using OS_dispatch_data=System.IntPtr;
using OS_nw_protocol_definition=System.IntPtr;
using OS_nw_protocol_options=System.IntPtr;
using OS_nw_endpoint=System.IntPtr;
using OS_nw_parameters=System.IntPtr;

namespace Network {

	[TV (13,0), Mac (10,15), iOS (13,0), Watch (6,0)]
	public class NWFramer : NWProtocolMetadata {
		public NWFramer (IntPtr handle, bool owns) : base (handle, owns) {}

		[DllImport (Constants.NetworkLibrary)]
		static extern bool nw_framer_write_output_no_copy (OS_nw_framer framer, nuint output_length);

		public bool WriteOutput (nuint outputLength) => nw_framer_write_output_no_copy (GetCheckedHandle (), outputLength);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_framer_write_output_data (OS_nw_framer framer, OS_dispatch_data output_data);

		public void WriteOutput (DispatchData data)
		{
			if (data == null)
				throw new ArgumentNullException (nameof (data));
			nw_framer_write_output_data (GetCheckedHandle (), data.Handle);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_framer_write_output (OS_nw_framer framer, byte[] output_buffer, nuint output_length);

		public void WriteOutput (byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException (nameof (data));
			unsafe {
				fixed (void* dataHandle = data)
					nw_framer_write_output (GetCheckedHandle (), data, (nuint) data.Length);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_framer_set_wakeup_handler (OS_nw_framer framer, void *wakeup_handler);

		delegate void nw_framer_set_wakeup_handler_t (IntPtr block, OS_nw_framer framer);
		static nw_framer_set_wakeup_handler_t static_WakeupHandler = TrampolineWakeupHandler;

		[MonoPInvokeCallback (typeof (nw_framer_set_wakeup_handler_t))]
		static void TrampolineWakeupHandler (IntPtr block, OS_nw_framer framer)
		{
			var del = BlockLiteral.GetTarget<Action<NWFramer>> (block);
			if (del != null) {
				var nwFramer = new NWFramer (framer, owns: true);
				del (nwFramer);
			}
		}

		public Action<NWFramer> WakeupHandler {
			set {
				unsafe {
					if (value == null) {
						nw_framer_set_wakeup_handler (GetCheckedHandle (), null);
						return;
					}
					BlockLiteral block_handler = new BlockLiteral ();
					BlockLiteral *block_ptr_handler = &block_handler;
					block_handler.SetupBlockUnsafe (static_WakeupHandler, value);
					try {
						nw_framer_set_wakeup_handler (GetCheckedHandle (), (void*) block_ptr_handler);
					} finally {
						block_handler.CleanupBlock ();
					}
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_framer_set_stop_handler (OS_nw_framer framer, void *stop_handler);

		delegate void nw_framer_set_stop_handler_t (IntPtr block, OS_nw_framer framer);
		static nw_framer_set_stop_handler_t static_StopHandler = TrampolineStopHandler;

		[MonoPInvokeCallback (typeof (nw_framer_set_stop_handler_t))]
		static void TrampolineStopHandler (IntPtr block, OS_nw_framer framer)
		{
			var del = BlockLiteral.GetTarget<Action<NWFramer>> (block);
			if (del != null) {
				var nwFramer = new NWFramer (framer, owns: true);
				del (nwFramer);
			}
		}

		public Action<NWFramer> StopHandler {
			set {
				unsafe {
					if (value == null) {
						nw_framer_set_stop_handler (GetCheckedHandle (), null);
						return;
					}
					BlockLiteral block_handler = new BlockLiteral ();
					BlockLiteral *block_ptr_handler = &block_handler;
					block_handler.SetupBlockUnsafe (static_StopHandler, value);
					try {
						nw_framer_set_stop_handler (GetCheckedHandle (), (void*) block_ptr_handler);
					} finally {
						block_handler.CleanupBlock ();
					}
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_framer_set_output_handler (OS_nw_framer framer, void *output_handler);

		delegate void nw_framer_set_output_handler_t (IntPtr block, OS_nw_framer framer, OS_nw_protocol_metadata message, nuint message_length, bool is_complete);
		static nw_framer_set_output_handler_t static_OutputHandler = TrampolineOutputHandler;

		[MonoPInvokeCallback (typeof (nw_framer_set_output_handler_t))]
		static void TrampolineOutputHandler (IntPtr block, OS_nw_framer framer, OS_nw_protocol_metadata message, nuint message_length, bool is_complete)
		{
			var del = BlockLiteral.GetTarget<Action<NWFramer, NWProtocolMetadata, nuint, bool>> (block);
			if (del != null) {
				var nwFramer = new NWFramer (framer, owns: true);
				var nwProtocolMetadata = new NWProtocolMetadata (message, owns: true);
				del (nwFramer, nwProtocolMetadata, message_length, is_complete);
			}
		}
		
		public Action<NWFramer, NWProtocolMetadata, nuint, bool> OutputHandler {
			set {
				unsafe {
					if (value == null) {
						nw_framer_set_output_handler (GetCheckedHandle (), null);
						return;
					}
					BlockLiteral block_handler = new BlockLiteral ();
					BlockLiteral *block_ptr_handler = &block_handler;
					block_handler.SetupBlockUnsafe (static_OutputHandler, value);
					try {
						nw_framer_set_output_handler (GetCheckedHandle (), (void*) block_ptr_handler);
					} finally {
						block_handler.CleanupBlock ();
					}
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_framer_set_input_handler (OS_nw_framer framer, void *input_handler);

		delegate void nw_framer_set_input_handler_t (IntPtr block, OS_nw_framer framer);
		static nw_framer_set_input_handler_t static_InputHandler = TrampolineInputHandler;

		[MonoPInvokeCallback (typeof (nw_framer_set_input_handler_t))]
		static void TrampolineInputHandler (IntPtr block, OS_nw_framer framer)
		{
			var del = BlockLiteral.GetTarget<Action<NWFramer>> (block);
			if (del != null) {
				var nwFramer = new NWFramer (framer, owns: true);
				del (nwFramer);
			}
		}

		public Action<NWFramer> InputHandler {
			set {
				unsafe {
					if (value == null) {
						nw_framer_set_input_handler (GetCheckedHandle (), null);
						return;
					}
					BlockLiteral block_handler = new BlockLiteral ();
					BlockLiteral *block_ptr_handler = &block_handler;
					block_handler.SetupBlockUnsafe (static_InputHandler, value);
					try {
						nw_framer_set_input_handler (GetCheckedHandle (), (void*) block_ptr_handler);
					} finally {
						block_handler.CleanupBlock ();
					}
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_framer_set_cleanup_handler (OS_nw_framer framer, void *cleanup_handler);

		delegate void nw_framer_set_cleanup_handler_t (IntPtr block, OS_nw_framer framer);
		static nw_framer_set_input_handler_t static_CleanupHandler = TrampolineCleanupHandler;

		[MonoPInvokeCallback (typeof (nw_framer_set_input_handler_t))]
		static void TrampolineCleanupHandler (IntPtr block, OS_nw_framer framer)
		{
			var del = BlockLiteral.GetTarget<Action<NWFramer>> (block);
			if (del != null) {
				var nwFramer = new NWFramer (framer, owns: true);
				del (nwFramer);
			}
		}

		public Action<NWFramer> CleanupHandler {
			set {
				unsafe {
					if (value == null) {
						nw_framer_set_cleanup_handler (GetCheckedHandle (), null);
						return;
					}
					BlockLiteral block_handler = new BlockLiteral ();
					BlockLiteral *block_ptr_handler = &block_handler;
					block_handler.SetupBlockUnsafe (static_InputHandler, value);
					try {
						nw_framer_set_cleanup_handler (GetCheckedHandle (), (void*) block_ptr_handler);
					} finally {
						block_handler.CleanupBlock ();
					}
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_framer_schedule_wakeup (OS_nw_framer framer, ulong milliseconds);

		public void ScheduleWakeup (ulong milliseconds) => nw_framer_schedule_wakeup (GetCheckedHandle (), milliseconds);

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_metadata nw_framer_protocol_create_message (OS_nw_protocol_definition definition);

		public static NWProtocolMetadata CreateMessage (NWProtocolDefinition protocolDefinition)
		{
			if (protocolDefinition == null)
				throw new ArgumentNullException (nameof (protocolDefinition));
			return new NWProtocolMetadata (nw_framer_protocol_create_message (protocolDefinition.Handle), owns: true);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_metadata nw_framer_message_create (OS_nw_framer framer);

		public NWProtocolMetadata CreateMessage ()
			=> new NWProtocolMetadata (nw_framer_message_create (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		static extern bool nw_framer_prepend_application_protocol (OS_nw_framer framer, OS_nw_protocol_options protocol_options);

		public bool PrependApplicationProtocol (NWProtocolOptions options)
		{
			if (options == null)
				throw new ArgumentNullException (nameof (options));
			return nw_framer_prepend_application_protocol (GetCheckedHandle (), options.Handle);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_framer_pass_through_output (OS_nw_framer framer);

		public void PassThroughOutput () => nw_framer_pass_through_output (GetCheckedHandle ()); 

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_framer_pass_through_input (OS_nw_framer framer);

		public void PassThroughInput () => nw_framer_pass_through_input (GetCheckedHandle ()); 

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_framer_mark_ready (OS_nw_framer framer);

		public void MarkReady () => nw_framer_mark_ready (GetCheckedHandle ()); 

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_framer_mark_failed_with_error (OS_nw_framer framer, int error_code);

		public void MarkFailedWithError (int errorCode) => nw_framer_mark_failed_with_error (GetCheckedHandle (), errorCode); 

		[DllImport (Constants.NetworkLibrary)]
		static extern bool nw_framer_deliver_input_no_copy (OS_nw_framer framer, nuint input_length, OS_nw_protocol_metadata message, bool is_complete);

		public bool DeliverInput (nuint length, NWProtocolMetadata message, bool isComplete)
		{
			if (message == null)
				throw new ArgumentNullException (nameof (message));
			return nw_framer_deliver_input_no_copy (GetCheckedHandle (), length, message.Handle, isComplete);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_protocol_options nw_framer_create_options (OS_nw_protocol_definition framer_definition);

		public static NWProtocolOptions CreateOptions (NWProtocolDefinition protocolDefinition)
		{
			if (protocolDefinition == null)
				throw new ArgumentNullException (nameof (protocolDefinition));
			return new NWProtocolOptions (nw_framer_create_options (protocolDefinition.Handle), owns: true);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_endpoint nw_framer_copy_remote_endpoint (OS_nw_framer framer);

		public NWEndpoint Endpoint => new NWEndpoint (nw_framer_copy_remote_endpoint (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_parameters nw_framer_copy_parameters (OS_nw_framer framer);

		public NWParameters Parameters => new NWParameters (nw_framer_copy_parameters (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_endpoint nw_framer_copy_local_endpoint (OS_nw_framer framer);

		public NWEndpoint LocalEndpoint => new NWEndpoint (nw_framer_copy_local_endpoint (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_framer_async (OS_nw_framer framer, void *async_block);

		delegate void nw_framer_async_t (IntPtr block);
		static nw_framer_async_t static_ScheduleHandler = TrampolineScheduleHandler;

		[MonoPInvokeCallback (typeof (nw_framer_async_t))]
		static void TrampolineScheduleHandler (IntPtr block) 
		{
			var del = BlockLiteral.GetTarget<Action> (block);
			if (del != null) {
				del ();
			}
		}

		public void ScheduleAsync (Action handler)
		{
			unsafe {
				if (handler == null) {
					nw_framer_async (GetCheckedHandle (), null);
					return;
				}
				BlockLiteral block_handler = new BlockLiteral ();
				BlockLiteral *block_ptr_handler = &block_handler;
				block_handler.SetupBlockUnsafe (static_ScheduleHandler, handler);
				try {
					nw_framer_async (GetCheckedHandle (), (void*) block_ptr_handler);
				} finally {
					block_handler.CleanupBlock ();
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe bool nw_framer_parse_output (OS_nw_framer framer, nuint minimum_incomplete_length, nuint maximum_length, IntPtr temp_buffer, void* parse);

		delegate void nw_framer_parse_output_t (IntPtr block, IntPtr buffer, nuint buffer_length, bool is_complete);
		static nw_framer_parse_output_t static_ParseOutputHandler = TrampolineParseOutputHandler;

		[MonoPInvokeCallback (typeof (nw_framer_parse_output_t))]
		static void TrampolineParseOutputHandler (IntPtr block, IntPtr buffer, nuint buffer_length, bool is_complete)
		{
			var del = BlockLiteral.GetTarget<Action<byte[], bool>> (block);
			if (del != null) {
				var bBuffer = new byte[buffer_length];
				Marshal.Copy (buffer, bBuffer, 0, (int)buffer_length);
				del (bBuffer, is_complete);
			}
		}

		public bool ParseOutput (nuint minimumIncompleteLength, nuint maximumLength, byte[] tempBuffer, Action<byte[], bool> handler)
		{
			unsafe {
				if (handler == null) {
					if (tempBuffer == null)
						return nw_framer_parse_output (GetCheckedHandle (), minimumIncompleteLength, maximumLength, IntPtr.Zero, null);
					else
						fixed (void* tmpBufferPtr = tempBuffer)
							return nw_framer_parse_output (GetCheckedHandle (), minimumIncompleteLength, maximumLength, (IntPtr)tmpBufferPtr, null);
				}
				BlockLiteral block_handler = new BlockLiteral ();
				BlockLiteral *block_ptr_handler = &block_handler;
				block_handler.SetupBlockUnsafe (static_ParseOutputHandler, handler);
				try {
					if (tempBuffer == null)
						return nw_framer_parse_output (GetCheckedHandle (), minimumIncompleteLength, maximumLength, IntPtr.Zero, (void*) block_ptr_handler);
					else
						fixed (void* tmpBufferPtr = tempBuffer)
						return nw_framer_parse_output (GetCheckedHandle (), minimumIncompleteLength, maximumLength, (IntPtr)tmpBufferPtr, (void*) block_ptr_handler);
				} finally {
					block_handler.CleanupBlock ();
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe bool nw_framer_parse_input (OS_nw_framer framer, nuint minimum_incomplete_length, nuint maximum_length, IntPtr temp_buffer, void* parse);

		delegate void nw_framer_parse_input_t (IntPtr block, IntPtr buffer, nuint buffer_length, bool is_complete);
		static nw_framer_parse_input_t static_ParseInputHandler = TrampolineParseInputHandler;

		[MonoPInvokeCallback (typeof (nw_framer_parse_input_t))]
		static void TrampolineParseInputHandler (IntPtr block, IntPtr buffer, nuint buffer_length, bool is_complete)
		{
			var del = BlockLiteral.GetTarget<Action<byte[], bool>> (block);
			if (del != null) {
				var bBuffer = new byte[buffer_length];
				Marshal.Copy (buffer, bBuffer, 0, (int)buffer_length);
				del (bBuffer, is_complete);
			}
		}

		public bool ParseInput (nuint minimumIncompleteLength, nuint maximumLength, byte[] tempBuffer, Action<byte[], bool> handler)
		{
			unsafe {
				if (handler == null) {
					if (tempBuffer == null)
						return nw_framer_parse_input (GetCheckedHandle (), minimumIncompleteLength, maximumLength, IntPtr.Zero, null);
					else
						fixed (void* tmpBufferPtr = tempBuffer)
							return nw_framer_parse_input (GetCheckedHandle (), minimumIncompleteLength, maximumLength, (IntPtr)tmpBufferPtr, null);
				}
				BlockLiteral block_handler = new BlockLiteral ();
				BlockLiteral *block_ptr_handler = &block_handler;
				block_handler.SetupBlockUnsafe (static_ParseInputHandler, handler);
				try {
					if (tempBuffer == null)
						return nw_framer_parse_input (GetCheckedHandle (), minimumIncompleteLength, maximumLength, IntPtr.Zero, (void*) block_ptr_handler);
					else
						fixed (void* tmpBufferPtr = tempBuffer)
						return nw_framer_parse_input (GetCheckedHandle (), minimumIncompleteLength, maximumLength, (IntPtr)tmpBufferPtr, (void*) block_ptr_handler);
				} finally {
					block_handler.CleanupBlock ();
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe void nw_framer_message_set_value (OS_nw_protocol_metadata message, string key, IntPtr value, void *dispose_value);

		public void SetKey (string key, byte[] value)
		{
			// the method takes a callback to cleanup the data, but we do not need that since we are managed
			unsafe {
				fixed (void* valuePtr = value)
					nw_framer_message_set_value (GetCheckedHandle (), key, (IntPtr) valuePtr, null);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_framer_message_set_object_value (OS_nw_protocol_metadata message, string key, IntPtr value);

		public void SetObject (string key, NSObject value)
			=> nw_framer_message_set_object_value (GetCheckedHandle (), key, value.GetHandle ()); 

		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_framer_message_copy_object_value (OS_nw_protocol_metadata message, string key);

		public NSObject GetValue (string key)
			=> Runtime.GetNSObject (nw_framer_message_copy_object_value (GetCheckedHandle (), key));

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_framer_deliver_input (OS_nw_framer framer, IntPtr input_buffer, nuint input_length, OS_nw_protocol_metadata message, bool is_complete);

		public void DeliverInput (byte[] buffer, NWProtocolMetadata message, bool isComplete)
		{
			if (message == null)
				throw new ArgumentNullException (nameof (message));
			unsafe {
				fixed (void * bufferPtr = buffer)
					nw_framer_deliver_input (GetCheckedHandle (),(IntPtr) bufferPtr, (nuint)buffer.Length, message.Handle, isComplete);
			}
		}
	}
}
