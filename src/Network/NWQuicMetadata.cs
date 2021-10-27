using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;
using Security;

using OS_nw_protocol_metadata = System.IntPtr;
using SecProtocolMetadataRef = System.IntPtr;

#nullable enable

namespace Network {

#if !NET
	[Watch (8, 0), TV (15, 0), Mac (12, 0), iOS (15, 0), MacCatalyst (15, 0)]
#else
	[SupportedOSPlatform ("ios"), SupportedOSPlatform ("tvos"), SupportedOSPlatform ("macos"), SupportedOSPlatform ("maccatalyst")]
#endif
	public class NWQuicMetadata : NWProtocolMetadata {

		public NWQuicMetadata (IntPtr handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_remote_idle_timeout (OS_nw_protocol_metadata metadata);

		public ulong RemoteIdleTimeout
			=> nw_quic_get_remote_idle_timeout (GetCheckedHandle ());
		
		[DllImport (Constants.NetworkLibrary)]
		static extern ushort nw_quic_get_keepalive_interval (OS_nw_protocol_metadata metadata);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_keepalive_interval (OS_nw_protocol_metadata metadata, ushort keepaliveInterval);

		public ushort KeepaliveInterval {
			get => nw_quic_get_keepalive_interval (GetCheckedHandle ());
			set => nw_quic_set_keepalive_interval (GetCheckedHandle (), value);
		}
		
		[DllImport (Constants.NetworkLibrary)]
		static extern string nw_quic_get_application_error_reason (OS_nw_protocol_metadata metadata);

		public string? ApplicationErrorReason
			=> nw_quic_get_application_error_reason (GetCheckedHandle ());
		
		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_application_error (OS_nw_protocol_metadata metadata);

		public ulong ApplicationErrorCode =>
			nw_quic_get_application_error (GetCheckedHandle ());
		
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_application_error (OS_nw_protocol_metadata metadata, ulong application_error, string reason);

		public (ulong error, string? reason) ApplicationError {
			get => (ApplicationErrorCode, ApplicationErrorReason);
			set => nw_quic_set_application_error (GetCheckedHandle (), value.error, value.reason!); 
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern SecProtocolMetadataRef nw_quic_copy_sec_protocol_metadata (OS_nw_protocol_metadata metadata);

		public SecProtocolMetadata SecProtocolMetadata 
			=> new SecProtocolMetadata (nw_quic_copy_sec_protocol_metadata (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_stream_id (OS_nw_protocol_metadata metadata);

		public ulong StreamId
			=> nw_quic_get_stream_id (GetCheckedHandle ());

		// extern uint8_t nw_quic_get_stream_type (nw_protocol_metadata_t _Nonnull stream_metadata) __attribute__((availability(macos, unavailable))) __attribute__((availability(ios, unavailable))) __attribute__((availability(watchos, unavailable))) __attribute__((availability(tvos, unavailable)));
		[NoWatch, NoTV, NoMac, NoiOS]
		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_quic_get_stream_type (OS_nw_protocol_metadata stream_metadata);

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_stream_application_error (OS_nw_protocol_metadata metadata);
		
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_stream_application_error (OS_nw_protocol_metadata metadata, ulong application_error);

		public ulong StreamApplicationError {
			get => nw_quic_get_stream_application_error (GetCheckedHandle ());
			set => nw_quic_set_stream_application_error (GetCheckedHandle (), value);
		}
		
		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_local_max_streams_bidirectional (OS_nw_protocol_metadata metadata);
		
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_local_max_streams_bidirectional (OS_nw_protocol_metadata metadata, ulong max_streams_bidirectional);

		public ulong MaxStreamsBidirectional {
			get => nw_quic_get_local_max_streams_bidirectional (GetCheckedHandle ());
			set => nw_quic_set_local_max_streams_bidirectional (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_local_max_streams_unidirectional (OS_nw_protocol_metadata metadata);
		
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_local_max_streams_unidirectional (OS_nw_protocol_metadata metadata, ulong max_streams_unidirectional);

		public ulong LocalMaxStreamsUnidirectional {
			get => nw_quic_get_local_max_streams_unidirectional (GetCheckedHandle ());
			set => nw_quic_set_local_max_streams_unidirectional (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_remote_max_streams_bidirectional (OS_nw_protocol_metadata metadata);

		public ulong RemoteMaxStreamsBidirectional
			=> nw_quic_get_remote_max_streams_bidirectional (GetCheckedHandle ()); 

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_remote_max_streams_unidirectional (OS_nw_protocol_metadata metadata);
		
		public ulong RemoteMaxStreamsUnidirectional 
			=> nw_quic_get_remote_max_streams_unidirectional (GetCheckedHandle ()); 
	}
}
