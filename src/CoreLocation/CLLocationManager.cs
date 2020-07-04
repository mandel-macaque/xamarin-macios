//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2011-2014 Xamarin Inc
//
// The class can be either constructed from a string (from user code)
// or from a handle (from iphone-sharp.dll internal calls).  This
// delays the creation of the actual managed string until actually
// required
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
#if !MONOMAC
using UIKit;
#endif
using Foundation;
using CoreLocation;
using ObjCRuntime;

namespace CoreLocation {
	public partial class CLLocationManager : NSObject {

#if IOS
		public static bool IsMonitoringAvailable (Type t)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(7,0))
				return IsMonitoringAvailable (new Class (t));
			return false;
		}
#endif

		public static CLAuthorizationStatus Status {
			get {
				// because an instance property in iOS 14, tvOS 14, Watch 7 and macOS 10,16
#if IOS || TVOS
				if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion (14, 0)) {
#elif WATCH
				if (WatchKit.WKInterfaceDevice.CurrentDevice.CheckSystemVersion (7, 0)) {
#else 
				if (PlatformHelper.CheckSystemVersion (10, 16)) {
#endif
					return new CLLocationManager ()._IStatus;
				} else {
					return _SStatus;
				}
			}
		}
	}
}
