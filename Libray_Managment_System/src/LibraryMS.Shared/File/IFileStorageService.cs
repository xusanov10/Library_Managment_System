using Libray_Managment_System.Services;

public interface IFileStorageService
{
    // Faylni Minio'ga yuklash
    Task<Result<string>> UploadFileAsync(string bucketName, string objectName, Stream data, string contentType);

    // Faylni Minio'dan yuklab olish
    Task<Result<Stream>> DownloadFileAsync(string bucketName, string objectName);

    // Faylning mavjudligini tekshirish
    Task<Result<bool>> FileExistsAsync(string bucketName, string objectName);

    // Faylni Minio'dan o'chirish
    Task<Result<bool>> RemoveFileAsync(string bucketName, string objectName);

    // Bucket mavjudligini tekshirish
    Task<Result<bool>> BucketExistsAsync(string bucketName);

    // Yangi bucket yaratish
    Task CreateBucketAsync(string bucketName);
}
