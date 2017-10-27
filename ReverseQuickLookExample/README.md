# "Reverse" QuickLook Example

Xamarin.Mac does not contain full API coverage, specially when it comes to a number of plugin and extensions surfaces found on macOS. While those "gaps" are valid bugs, and should be reported and added to the backlog, hitting them does not leave and obvious path forward.

This sample shows the "reverse" technique for handling those situations. We can code a majority of our logic in C#, shared with our application, and write a thin layer in objective-c by using the [Embeddinator-4000](https://mono.github.io/Embeddinator-4000/getting-started-objective-c) to expose it to objective-c.

This technique could be applied to any other extension scenario where packaging or API limitations prevent a full C# implementation.

This sample should be considered **unofficial and unsupported** (though feel free to ask questions on the forums).

##Quickstart:

- Open managed/QuickLookLibraryTest/QuickLookLibraryTest.csproj and change the Debug signing key to your own 
- make
- First will pop up the QuickLookLibraryTest test application showing the expected behavior (and registering the quicklook plugin)
- Second two qlmanage instances will pop up wiht the preview and thumbnail image for test/foo.xam (should be xamagon.png)
- This will copy the binary to "~/Library/QuickLook/" which you may want to delete afterwards

## Technical Details

- The application is composed of three components:
   - managed/QuickLookLibrary - A C# library with the logic to determine what preview/thumbnail shoud be show. The logic is trivial in this sample but could be arbitrarily complex.
   - managed/QuickLookLibraryTest - A C# application that is both a test for QuickLookLibrary and more importantly registers the .xam extension UTI. Without this, .xam files will not be given a special UTI and our quicklook extension will not run. Copying an application to /Application or running it prompts launch services to register it.
   - native/XamarinQuickLook - An objective-c quick look extension that invokes the C# code, and if the right value is returned loads a png and draws it.
- Ideally the invocation to Xamarin.Embeddinator-4000.framework/Commands/objcgen would produce a framework instead of a static library. That makes use significantly easier. However, XCode's quick look projects have not been updated with the "embedded binaries" feature and consumption of frameworks is non-trivial.
- Due to current limitations in Xamarin.Embeddinator-4000, we are unable to pass the NSImage / NSUrl directly from C# to objective-c. We have to pass simple types (string, int, enum). 
    - If we are willing to delve into unsafe code, we could use a static reference or gchandle to keep the data pinned into memory and pass an IntPtr of an NSData across instead. You would be responsible to handle proper lifecycle.

## Sample notes
- In theory, an invocation to lsregister should be sufficient instead of launching the test application, but it was inconsistent
- In theory, the test application should not require signing, but without signing launch services seemed to (sometimes?) ignore it's UTI requests.
- The objective-c is significanly more complicated that I had hope, but that has to do with the complexity of programming in CoreFoundation.