using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using Foundation;
using HKQuantityRef = System.IntPtr;
using NSErrorRef = System.IntPtr;

#nullable enable

namespace HealthKit {

	[Watch (8,0), iOS (15,0)]
	public static class HKAppleWalkingSteadiness
	{

		[DllImport (Constants.HealthKitLibrary)]
		static extern bool HKAppleWalkingSteadinessClassificationForQuantity (HKQuantityRef value, out nint classificationOut, out NSErrorRef errorOut);

		public static bool TryGetClassification (HKQuantity value, out HKAppleWalkingSteadinessClassification? classification, out NSError? error)
		{
			if (value == null)
				throw new ArgumentNullException (nameof (value));
			classification = null;
			error = null;
			if (HKAppleWalkingSteadinessClassificationForQuantity (value.GetHandle (), out var classificationOut, out var errorPtr)) {
				classification = (HKAppleWalkingSteadinessClassification) (long) classificationOut;
				error = Runtime.GetNSObject<NSError> (errorPtr, false); 
				return true;
			}
			return false; 

		}

		[DllImport (Constants.HealthKitLibrary)]
		static extern HKQuantityRef HKAppleWalkingSteadinessMinimumQuantityForClassification (nint classification);

		public static HKQuantity GetMinimumQuantity (HKAppleWalkingSteadinessClassification classification)
			=> Runtime.GetNSObject<HKQuantity> (HKAppleWalkingSteadinessMinimumQuantityForClassification ((nint) (long) classification), true); 

		[DllImport (Constants.HealthKitLibrary)]
		static extern HKQuantityRef HKAppleWalkingSteadinessMaximumQuantityForClassification (nint classification);

		public static HKQuantity GetMaximumQuantity (HKAppleWalkingSteadinessClassification classification)
			=> Runtime.GetNSObject<HKQuantity> (HKAppleWalkingSteadinessMaximumQuantityForClassification ((nint) (long) classification), true); 
	}

}
