//
// NWTxtRecord.cs: Bindings the Network nw_txt_record_t API
//
// Authors:
//   Miguel de Icaza (miguel@microsoft.com)
//
// Copyrigh 2019 Microsoft Inc
//
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

using nw_advertise_descriptor_t=System.IntPtr;
using OS_nw_advertise_descriptor=System.IntPtr;
using OS_nw_txt_record=System.IntPtr;


namespace Network {

	[TV (13,0), Mac (10,15), iOS (13,0), Watch (6,0)]
	public enum NWTxtRecordFindKey {
		Invalid = 0,
		NotPresent = 1,
		NoValue = 2,
		EmptyValue = 3,
		NonEmptyValue = 4
	}
	
	[TV (13,0), Mac (10,15), iOS (13,0), Watch (6,0)]
	public class NWTxtRecord : NativeObject {
		public NWTxtRecord (IntPtr handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_txt_record_create_with_bytes (byte[] txtBytes, nuint len);

		public static NWTxtRecord FromBytes (byte [] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return null;
			unsafe {
				fixed (byte *p = &bytes[0]){
					var x = nw_txt_record_create_with_bytes (bytes, (nuint) bytes.Length);
					if (x == IntPtr.Zero)
						return null;
					return new NWTxtRecord (x, owns: true);
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_txt_record_create_dictionary();

		public static NWTxtRecord CreateDictionary ()
		{
			var x = nw_txt_record_create_dictionary ();
			if (x == IntPtr.Zero)
				return null;
			return new NWTxtRecord (x, owns: true);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_txt_record_copy (IntPtr other);
		
		public NWTxtRecord Clone ()
		{
			if (Handle == IntPtr.Zero)
				return null;
			return new NWTxtRecord (nw_txt_record_copy (Handle), owns: true);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern NWTxtRecordFindKey nw_txt_record_find_key (IntPtr handle, string key);
		
		public NWTxtRecordFindKey FindKey (string key)
		{
			return nw_txt_record_find_key (Handle, key);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_txt_record_set_key(IntPtr handle, string key, IntPtr value, nuint valueLen);

		public bool SetKey (string key, byte [] value)
		{
			if (value == null)
				return nw_txt_record_set_key (Handle, key, IntPtr.Zero, 0) != 0;
			else {
				unsafe {
					fixed (void *pv = value)
						return nw_txt_record_set_key (Handle, key, (IntPtr) pv, (nuint) value.Length) != 0;
				}
			}
		}
		
		public bool SetKey (string key, string value)
		{
			var utf8 = new UTF8Encoding();
			return SetKey (key, value == null ? null : utf8.GetBytes (value));
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_txt_record_remove_key (IntPtr handle, string key);

		public bool RemoveKey (string key)
		{
			return nw_txt_record_remove_key (Handle, key) != 0;
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern long nw_txt_record_get_key_count (IntPtr handle);
		
		public long KeyCount {
			get {
				return nw_txt_record_get_key_count (Handle);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_txt_record_is_dictionary (IntPtr handle);

		public bool IsDictionary {
			get => nw_txt_record_is_dictionary (Handle) != 0;
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern bool nw_txt_record_is_equal (OS_nw_txt_record left, OS_nw_txt_record right);

		public bool Equals (NWTxtRecord other)
		{
			if (other == null)
				return false;
			return nw_txt_record_is_equal (GetCheckedHandle (), other.Handle);
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern bool nw_txt_record_apply (OS_nw_txt_record txt_record, void *applier);

		delegate void nw_txt_record_apply_t (IntPtr block, string key, NWTxtRecordFindKey found, IntPtr value, nuint valueLen); 
		static nw_txt_record_apply_t static_ApplyHandler = TrampolineApplyHandler;

		[MonoPInvokeCallback (typeof (nw_txt_record_apply_t))]
		static void TrampolineApplyHandler (IntPtr block, string key, NWTxtRecordFindKey found, IntPtr value, nuint valueLen)
		{
			var del = BlockLiteral.GetTarget<Action<string, NWTxtRecordFindKey, byte[]>> (block);
			if (del != null) {
				var bValue = new byte[valueLen];
				Marshal.Copy (value, bValue, 0, (int)valueLen);
				del (key, found, bValue);
			}
		}

		public bool Apply (Action<string, NWTxtRecordFindKey, byte[]> handler)
		{
			unsafe {
				if (handler == null) {
					return nw_txt_record_apply (GetCheckedHandle (), null);
				}
				BlockLiteral block_handler = new BlockLiteral ();
				BlockLiteral *block_ptr_handler = &block_handler;
				block_handler.SetupBlockUnsafe (static_ApplyHandler, handler);
				try {
					return nw_txt_record_apply (GetCheckedHandle (), (void*) block_ptr_handler);
				} finally {
					block_handler.CleanupBlock ();
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe bool nw_txt_record_access_key (OS_nw_txt_record txt_record, string key, void *access_value);

		delegate void nw_txt_record_access_key_t (IntPtr block, string key, NWTxtRecordFindKey found, IntPtr value, nuint valueLen);
		static nw_txt_record_access_key_t static_AccessKeyHandler = TrampolineAccessKeyHandler;

		[MonoPInvokeCallback (typeof (nw_txt_record_access_key_t))]
		static void TrampolineAccessKeyHandler (IntPtr block, string key, NWTxtRecordFindKey found, IntPtr value, nuint valueLen)
		{
			var del = BlockLiteral.GetTarget<Action<string, NWTxtRecordFindKey, byte[]>> (block);
			if (del != null) {
				var bValue = new byte[valueLen];
				Marshal.Copy (value, bValue, 0, (int)valueLen);
				del (key, found, bValue);
			}
		}

		public bool GetValue (string key, Action<string, NWTxtRecordFindKey, byte[]> handler)
		{
			unsafe {
				if (handler == null) {
					return nw_txt_record_access_key (GetCheckedHandle (),key,  null);
				}
				BlockLiteral block_handler = new BlockLiteral ();
				BlockLiteral *block_ptr_handler = &block_handler;
				block_handler.SetupBlockUnsafe (static_AccessKeyHandler, handler);
				try {
					return nw_txt_record_access_key (GetCheckedHandle (), key, (void*) block_ptr_handler);
				} finally {
					block_handler.CleanupBlock ();
				}
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern bool nw_txt_record_access_bytes (OS_nw_txt_record txt_record, void *access_bytes);

		delegate void nw_txt_record_access_bytes_t (IntPtr block, IntPtr value, nuint valueLen);
		static nw_txt_record_access_bytes_t static_RawBytesHandler = TrampolineRawBytesHandler;

		[MonoPInvokeCallback (typeof (nw_txt_record_access_bytes_t))]
		static void TrampolineRawBytesHandler (IntPtr block, IntPtr value, nuint valueLen)
		{
			var del = BlockLiteral.GetTarget<Action<byte[]>> (block);
			if (del != null) {
				var bValue = new byte[valueLen];
				Marshal.Copy (value, bValue, 0, (int)valueLen);
				del (bValue);
			}
		}

		public bool GetRawBytes (Action<byte[]> handler)
		{
			unsafe {
				if (handler == null) {
					return nw_txt_record_access_bytes (GetCheckedHandle (), null);
				}
				BlockLiteral block_handler = new BlockLiteral ();
				BlockLiteral *block_ptr_handler = &block_handler;
				block_handler.SetupBlockUnsafe (static_RawBytesHandler, handler);
				try {
					return nw_txt_record_access_bytes (GetCheckedHandle (), (void*) block_ptr_handler);
				} finally {
					block_handler.CleanupBlock ();
				}
			}
		}
	}
}
