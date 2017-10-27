#include <CoreFoundation/CoreFoundation.h>
#include <CoreServices/CoreServices.h>
#include <QuickLook/QuickLook.h>
#include "bindings.h"
#include <AppKit/AppKit.h>

OSStatus GeneratePreviewForURL(void *thisInterface, QLPreviewRequestRef preview, CFURLRef url, CFStringRef contentTypeUTI, CFDictionaryRef options);
void CancelPreviewGeneration(void *thisInterface, QLPreviewRequestRef preview);

/* -----------------------------------------------------------------------------
   Generate a preview for file

   This function's job is to create preview for designated file
   ----------------------------------------------------------------------------- */

// https://developer.apple.com/library/content/documentation/GraphicsImaging/Conceptual/ImageIOGuide/imageio_source/ikpg_source.html
CGImageRef MyCreateCGImageFromFile (NSURL *url)
{
    CGImageRef        myImage = NULL;
    CGImageSourceRef  myImageSource;
    CFDictionaryRef   myOptions = NULL;
    CFStringRef       myKeys[2];
    CFTypeRef         myValues[2];
    
    // Set up options if you want them. The options here are for
    // caching the image in a decoded form and for using floating-point
    // values if the image format supports them.
    myKeys[0] = kCGImageSourceShouldCache;
    myValues[0] = (CFTypeRef)kCFBooleanTrue;
    myKeys[1] = kCGImageSourceShouldAllowFloat;
    myValues[1] = (CFTypeRef)kCFBooleanTrue;
    // Create the dictionary
    myOptions = CFDictionaryCreate(NULL, (const void **) myKeys,
                                   (const void **) myValues, 2,
                                   &kCFTypeDictionaryKeyCallBacks,
                                   & kCFTypeDictionaryValueCallBacks);
    // Create an image source from the URL.
    myImageSource = CGImageSourceCreateWithURL((CFURLRef)url, myOptions);
    CFRelease(myOptions);
    // Make sure the image source exists before continuing
    if (myImageSource == NULL){
        fprintf(stderr, "Image source is NULL.");
        return  NULL;
    }
    // Create an image from the first item in the image source.
    myImage = CGImageSourceCreateImageAtIndex(myImageSource,
                                              0,
                                              NULL);
    
    CFRelease(myImageSource);
    // Make sure the image exists before continuing
    if (myImage == NULL){
        fprintf(stderr, "Image not created from image source.");
        return NULL;
    }
    
    return myImage;
}

OSStatus GeneratePreviewForURL(void *thisInterface, QLPreviewRequestRef preview, CFURLRef url, CFStringRef contentTypeUTI, CFDictionaryRef options)
{
    CFStringRef path = CFURLCopyFileSystemPath (url, kCFURLPOSIXPathStyle);
    
    QuickLookLibrary_ImageProvider * provider = [[QuickLookLibrary_ImageProvider alloc] init];
    QuickLookLibrary_Icons iconType = [provider getIconTypePath:(__bridge NSString *)path];

    CGSize canvasSize = CGSizeMake (64, 64);
    CGRect canvasRect = CGRectMake (0, 0, 64, 64);
    
    // https://developer.apple.com/library/content/samplecode/QuickLookSketch/Listings/GeneratePreviewForURL_m.html
    CGContextRef cgContext = QLPreviewRequestCreateContext(preview, canvasSize, false, NULL);
    if(cgContext) {
            if (iconType == QuickLookLibrary_IconsXamagon) {
                CFBundleRef bundle = CFBundleGetBundleWithIdentifier (CFSTR ("Test.XamarinQuickLook"));

                if (bundle != NULL) {
                    CFURLRef imageUrl = CFBundleCopyResourceURL (bundle, CFSTR("Xamagon.png"), NULL, NULL);
                    if (imageUrl != NULL) {
                        CGImageRef image = MyCreateCGImageFromFile((__bridge NSURL*)imageUrl);
                        if (image != NULL) {
                            CGContextDrawImage (cgContext, canvasRect, image);
                            CFRelease (image);
                        }
                        CFRelease (imageUrl);
                    }
                }
            }
        }
        
        // When we are done with our drawing code QLPreviewRequestFlushContext() is called to flush the context
        QLPreviewRequestFlushContext(preview, cgContext);
        
        CFRelease(cgContext);
    
    CFRelease (path);
    return noErr;
}

void CancelPreviewGeneration(void *thisInterface, QLPreviewRequestRef preview)
{
    // Implement only if supported
}
