# LipoExample

Early in 2018 Apple began rejecting macOS applications submitted to the App Store if they contained libraries that contained 32-bit portions.

This was discovered and filed as issue [#3367](https://github.com/xamarin/xamarin-macios/issues/3367).

This example uses a custom [MSBuild target](https://developer.xamarin.com/samples/mac/UseMSBuildToCopyFilesToBundleExample/) to invoke lipo to remove the 32-bit portion of libMonoPosixHelper.dylib.