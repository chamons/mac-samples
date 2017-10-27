GENERATOR = native/XamarinQuickLook/build/Debug/XamarinQuickLook.qlgenerator

test:: $(GENERATOR) register
	mkdir -p ~/Library/QuickLook/
	cp -r $(GENERATOR) ~/Library/QuickLook/
	qlmanage -r
	qlmanage -d 4 -p test/foo.xam
	qlmanage -d 4 -t test/foo.xam

$(GENERATOR): managed/binding/QuickLookLibrary.a
	xcodebuild -project native/XamarinQuickLook/XamarinQuickLook.xcodeproj -target XamarinQuickLook -configuration Debug

managed/binding/QuickLookLibrary.a: managed/QuickLookLibrary/bin/Debug/QuickLookLibrary.dll
	mkdir -p managed/binding
	mkdir -p managed/binding/MonoBundle
	cp managed/QuickLookLibrary/bin/Debug/QuickLookLibrary.dll managed/binding/
	cp managed/QuickLookLibrary/bin/Debug/QuickLookLibrary.dll managed/binding/MonoBundle/
	cp /Library/Frameworks/Xamarin.Mac.framework/Versions/Current/lib/mono/Xamarin.Mac/Xamarin.Mac.dll managed/binding/
	cp /Library/Frameworks/Xamarin.Mac.framework/Versions/Current/lib/mono/Xamarin.Mac/Xamarin.Mac.dll managed/binding/MonoBundle
	/Library/Frameworks/Xamarin.Embeddinator-4000.framework/Commands/objcgen managed/binding/QuickLookLibrary.dll -o managed/binding/ --compile --target:static

managed/QuickLookLibrary/bin/Debug/QuickLookLibrary.dll:
	msbuild managed/QuickLookLibrary/QuickLookLibrary.csproj

register:: managed/QuickLookLibrary/bin/Debug/QuickLookLibrary.dll
	msbuild managed/QuickLookLibraryTest/QuickLookLibraryTest.csproj
	managed/QuickLookLibraryTest/bin/Debug/QuickLookLibraryTest.app/Contents/MacOS/QuickLookLibraryTest

clean::
	-/System/Library/Frameworks/CoreServices.framework/Versions/A/Frameworks/LaunchServices.framework/Versions/A/Support/lsregister -u managed/QuickLookLibraryTest/bin/Debug/QuickLookLibraryTest.app
	msbuild managed/QuickLookLibrary/QuickLookLibrary.csproj /t:clean
	msbuild managed/QuickLookLibraryTest/QuickLookLibraryTest.csproj /t:clean
	rm -rf managed/binding/
	rm -rf native/XamarinQuickLook/build/


