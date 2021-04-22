#if IOS
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace Metal {
	public partial class MTLRasterizationRateLayerDescriptor
	{
/*  Selectors reported as not working by introspection: https://github.com/xamarin/maccore/issues/1976
		[NoMac]
		public double[] HorizontalSampleStorage { 
			get {
				var width = (int)SampleCount.Width;
				var floatArray = new double[width];
				Marshal.Copy (_HorizontalSampleStorage, floatArray, 0, width);
				return floatArray;
			}
		}

		[NoMac]
		public double[] VerticalSampleStorage {
			get {
				var height = (int)SampleCount.Height;
				var floatArray = new double[height];
				Marshal.Copy (_VerticalSampleStorage, floatArray, 0, height);
				return floatArray;
			}
		}
*/
#if NET
		[UnsupportedOSPlatform ("macos")]
#else
		[NoMac]
#endif
		static public MTLRasterizationRateLayerDescriptor Create (MTLSize sampleCount, float[] horizontal, float[] vertical)
		{
			if (horizontal == null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (horizontal));
			if (vertical == null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (vertical));
			if (sampleCount.Width != horizontal.Length)
				throw new ArgumentOutOfRangeException ("Horizontal length should be equal to the sampleCount.Width.");
			if (sampleCount.Height != vertical.Length)
				throw new ArgumentOutOfRangeException ("Vertical length should be equal to the sampleCount.Height.");

			unsafe {
				fixed (void* horizontalHandle = horizontal)
				fixed (void* verticalHandle = vertical) {
					return new MTLRasterizationRateLayerDescriptor (sampleCount, (IntPtr) horizontalHandle, (IntPtr) verticalHandle);
				}
			}
		}
	}
}
#endif
