using UnityEngine;

public class Love {
    public Love(string id, string name, string message, string mediaName, string mediaType) {
        Id = id;
        Name = name;
        Message = message;
        MediaName = mediaName;
        MediaType = GetMediaType(mediaType);
        Debug.unityLogger.Log("media",MediaType);
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string Message { get; set; }
    public string MediaName { get; set; }
    public MediaTypeEnum MediaType { get; set; }

    public enum MediaTypeEnum {
        NONE,
        PHOTO,
        MOVIE
    }

    public static MediaTypeEnum GetMediaType(string value) {
        switch (value) {
            case "none":
                return MediaTypeEnum.NONE;
            case "photo":
                return MediaTypeEnum.PHOTO;
            case "movie":
                return MediaTypeEnum.MOVIE;
            default: return MediaTypeEnum.NONE;
        }
    }
}