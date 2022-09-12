using System;
using System.Runtime.CompilerServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AVFoundation {

	public enum AVSpeechUtteranceRepresentationType {
		String,
		ML,
	}

	public partial class AVSpeechUtterance {

		static bool CheckSystemVersion ()
		{
#if IOS || __MACCATALYST__ || TVOS
			return SystemVersion.CheckiOS (15, 2);
#elif WATCH
			return SystemVersion.CheckwatchOS (8, 3);
#else
			return SystemVersion.CheckmacOS (10, 14);
#endif
		}

		public AVSpeechUtterance (string representation, AVSpeechUtteranceRepresentationType type) {
			if (CheckSystemVersion()) {
				InitializeHandle (InitWithString (representation));
			} else {
				switch (type) {
					case AVSpeechUtteranceRepresentationType.ML:
						InitializeHandle (InitWithString (representation));
						break;
					default:
						InitializeHandle (InitWithMLRepresentation(representation));
						break;
				}
			}
		}
	}
}
