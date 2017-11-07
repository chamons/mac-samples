## Description

This "sample" is a stream of notes I wrote down while creating an "external" binding for CryptoTokenKit.framework. I have not tested the binding beyond the most trivial "did it load" test, nor did I audit the full API surface.

## Steps followed to create

- `sharpie bind -framework /Applications/Xcode.app/Contents/Developer/Platforms/MacOSX.platform/Developer/SDKs/MacOSX10.13.sdk/System/Library/Frameworks/CryptoTokenKit.framework -sdk macosx10.13 -o binding/`
- Created new XM binding project and redirected ApiDefinitions/StructsAndEnums to copy in binding/
- Removed `using CryptoTokenKit;` and wrapped generated binding in `namespace CryptoTokenKit`
- Since 

`$ file /System/Library/Frameworks/CryptoTokenKit.framework/CryptoTokenKit` 
gave 
`/System/Library/Frameworks/CryptoTokenKit.framework/CryptoTokenKit: Mach-O 64-bit dynamically linked shared library x86_64`

I was able to replace the nint's with long

- Replace SecCertificateRef* and SecKeyAlgorithm* with IntPtr
- Comment out `InSessionWithError` for now since it contains a block, which are tricky to get right.
- Fix TKTokenSessionDelegate methods to have less terrible name
- Comment out TKSmartCardTokenDriverDelegate for now
- Fix TKSmartCardUserInteraction.Cancel to be a method not a property
- Fix TKSmartCardSlot.MakeSmartCard to be a method and not a property
- Remove Verify (since we checked those) and platform attributes (since they aren't needed for local binding).
- Comment out `TKSmartCard_APDULevelTransmit` for now
- Update `		[Field ("TKErrorDomain", "/System/Library/Frameworks/CryptoTokenKit.framework/CryptoTokenKit")]`
- `TKSmartCardUserInteractionDelegate Delegate` -> `ITKSmartCardUserInteractionDelegate Delegate` and add `class ITKSmartCardUserInteractionDelegate {}` and `[Protocolize]` to TKSmartCardUserInteraction.Delegate to handle protocol bits.
- Add `/System/Library/Frameworks/CryptoTokenKit.framework/CryptoTokenKit` to native frameworks
- Ok, it compiles. Let's make a test project and see how it breaks. New XM Project -> Add reference to our binding project.
- Build
- Look at final bundle, oh oh. It copied in CryptoTokenKit.framework. We can't do that.
- Remove native reference from binding library, clean and rebuild. We'll have to manually load it.
- Add `ObjCRuntime.Dlfcn.dlopen ("/System/Library/Frameworks/CryptoTokenKit.framework/CryptoTokenKit", 0);` to AppDelegate.DidFinishLaunching and try to create something with

```	
	var manager = CryptoTokenKit.TKSmartCardSlotManager.DefaultManager;
	System.Console.WriteLine (manager != null);
```
			
- Run. Get a null.
- Open Console (System Log). See "ctk: connecting to slot registration server failed"
- Read documentation. See [note](https://developer.apple.com/documentation/cryptotokenkit/tksmartcardslotmanager?language=objc) 
- Add com.apple.security.smartcard entitlement to Entitlements.plist and enable code signing / entitlements
- We get a valid manager! I don't have smartcard hardware, so I can't test more than that.


## Future improvements

- Audit binding for reasonable names. We mostly blindly accepted what Objective Sharpie gave us, which is not safe.
- Uncomment out:
   - `TKSmartCardTokenDriverDelegate`
   - `InSessionWithError`
   - `TKSmartCard_APDULevelTransmit`

and figure out how to bind them reasonably.

- Likely our real application will need to pass a link_flag to link against CryptoTokenKit if we are submitting to the App Store.
