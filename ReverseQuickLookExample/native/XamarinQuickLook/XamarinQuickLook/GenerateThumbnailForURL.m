#include <CoreFoundation/CoreFoundation.h>
#include <CoreServices/CoreServices.h>
#include <QuickLook/QuickLook.h>
#include "bindings.h"

OSStatus GenerateThumbnailForURL(void *thisInterface, QLThumbnailRequestRef thumbnail, CFURLRef url, CFStringRef contentTypeUTI, CFDictionaryRef options, CGSize maxSize);
void CancelThumbnailGeneration(void *thisInterface, QLThumbnailRequestRef thumbnail);

/* -----------------------------------------------------------------------------
    Generate a thumbnail for file

   This function's job is to create thumbnail for designated file as fast as possible
   ----------------------------------------------------------------------------- */

CGImageRef MyCreateCGImageFromFile (NSURL *url);

OSStatus GenerateThumbnailForURL(void *thisInterface, QLThumbnailRequestRef thumbnail, CFURLRef url, CFStringRef contentTypeUTI, CFDictionaryRef options, CGSize maxSize)
{
    NSLog(@"GenerateThumbnailForURL");

    CFStringRef path = CFURLCopyFileSystemPath (url, kCFURLPOSIXPathStyle);
    
    QuickLookLibrary_ImageProvider * provider = [[QuickLookLibrary_ImageProvider alloc] init];
    QuickLookLibrary_Icons iconType = [provider getIconTypePath:(__bridge NSString *)path];
    
    CGSize canvasSize = CGSizeMake (64, 64);
    CGRect canvasRect = CGRectMake (0, 0, 64, 64);
    
    // https://developer.apple.com/library/content/samplecode/QuickLookSketch/Listings/GeneratePreviewForURL_m.html
    CGContextRef cgContext = QLThumbnailRequestCreateContext (thumbnail, canvasSize, false, NULL);
    if(cgContext) {
        if (iconType == QuickLookLibrary_IconsXamagon) {
            CFBundleRef bundle = CFBundleGetBundleWithIdentifier (CFSTR ("Test.XamarinQuickLook"));
            if (bundle != NULL) {
                CFURLRef imageUrl = CFBundleCopyResourceURL (bundle, CFSTR("Xamagon"), CFSTR("png"), NULL);
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
    
    QLThumbnailRequestFlushContext (thumbnail, cgContext);
    
    CFRelease(cgContext);
    CFRelease (path);
    return noErr;
    
    // To complete your generator please implement the function GenerateThumbnailForURL in GenerateThumbnailForURL.c
    return noErr;
}

void CancelThumbnailGeneration(void *thisInterface, QLThumbnailRequestRef thumbnail)
{
    // Implement only if supported
}
