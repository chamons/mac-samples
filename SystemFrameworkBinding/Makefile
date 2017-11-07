all:: binding/ApiDefinitions.cs

binding/:
	mkdir -p binding/

binding/ApiDefinitions.cs: binding/
	sharpie bind -framework /Applications/Xcode.app/Contents/Developer/Platforms/MacOSX.platform/Developer/SDKs/MacOSX10.13.sdk/System/Library/Frameworks/CryptoTokenKit.framework -sdk macosx10.13 -o binding/

clean::
	rm -rf binding/
