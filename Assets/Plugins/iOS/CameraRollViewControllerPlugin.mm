#import <CameraRollSample-Swift.h>

extern "C" {
    void OpenCameraRoll(const char *path) {
        CameraRollViewController *cameraRollViewController = [CameraRollViewController new];
        
        [UnityGetGLViewController() addChildViewController:cameraRollViewController];
        [cameraRollViewController open:[NSString stringWithUTF8String:path]];
    }
}
