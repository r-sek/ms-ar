import UIKit

open class CameraRollViewController: UIViewController, UINavigationControllerDelegate, UIImagePickerControllerDelegate {
    
    static var filePath = "";
    
    open func open(_ path: String) {
        CameraRollViewController.filePath = path
        if UIImagePickerController.isSourceTypeAvailable(UIImagePickerControllerSourceType.photoLibrary) {
            
            let picker = UIImagePickerController()
            picker.delegate = self
            picker.sourceType = UIImagePickerControllerSourceType.photoLibrary
            self.present(picker, animated: true, completion: nil)
        }
    }
    open func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [String : Any]) {
        if let pickedImage = info[UIImagePickerControllerOriginalImage] as? UIImage {
            var resizedImage = fixOrientation(pickedImage)
            resizedImage = crop(resizedImage)
            resizedImage = resize(resizedImage, newSize: CGSize(width: 341, height: 341))
            let imageData = UIImagePNGRepresentation(resizedImage)
            let fileUrl = URL(fileURLWithPath: CameraRollViewController.filePath)
            do {
                try imageData!.write(to: fileUrl, options: .atomic)
                NSLog("passは"+CameraRollViewController.filePath);
                UnitySendMessage("GameController", "SetImage", CameraRollViewController.filePath);
            } catch {
                print(error)
            }
        }
        picker.dismiss(animated: true, completion: nil)
    }
    // 縦にとると横で保存されているのでオリエンテーションを修正する
    func fixOrientation(_ image: UIImage) -> UIImage {
        if (image.imageOrientation == UIImageOrientation.up) {
            return image;
        }
        
        UIGraphicsBeginImageContextWithOptions(image.size, false, image.scale);
        let rect = CGRect(x: 0, y: 0, width: image.size.width, height: image.size.height)
        image.draw(in: rect)

        let normalizedImage : UIImage = UIGraphicsGetImageFromCurrentImageContext()!
        UIGraphicsEndImageContext();
        return normalizedImage;
    }
    
    fileprivate func crop(_ image: UIImage) -> UIImage {
        let newCropLength: CGFloat = min(image.size.width, image.size.height)
        let x = image.size.width / 2.0 - newCropLength / 2.0;
        let y = image.size.height / 2.0 - newCropLength / 2.0;
        
        let cropRect = CGRect(x: x, y: y, width: newCropLength, height: newCropLength)
        let imageRef = image.cgImage!.cropping(to: cropRect)
        let cropped : UIImage = UIImage(cgImage: imageRef!, scale: 0, orientation: image.imageOrientation)
        
        return cropped
    }
    
    fileprivate func resize(_ image: UIImage, newSize: CGSize) -> UIImage {
        let widthRatio = newSize.width / image.size.width
        let heightRatio = newSize.height / image.size.height
        let ratio = (widthRatio < heightRatio) ? widthRatio : heightRatio
        let resizedSize = CGSize(width: (image.size.width * ratio), height: (image.size.height * ratio))
        UIGraphicsBeginImageContext(resizedSize)
        image.draw(in: CGRect(x: 0, y: 0, width: resizedSize.width, height: resizedSize.height))
        let resizedImage = UIGraphicsGetImageFromCurrentImageContext()
        UIGraphicsEndImageContext()
        return resizedImage!
    }
}
