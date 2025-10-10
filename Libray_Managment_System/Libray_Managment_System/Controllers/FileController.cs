using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;

    // Dependency Injection orqali IFileStorageService ni oladi
    public FilesController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")] // Fayl yuklash uchun shart
    public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string bucketName = "my-test-bucket")
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Fayl tanlanmagan yoki bo'sh.");
        }

        // Fayl nomini noyob qilish uchun Guid va original kengaytmadan foydalanamiz
        var fileExtension = Path.GetExtension(file.FileName);
        var objectName = $"{Guid.NewGuid()}{fileExtension}"; // Minio'da saqlanadigan fayl nomi

        using (var stream = file.OpenReadStream()) // Fayl streamini ochish
        {
            var fileUrl = await _fileStorageService.UploadFileAsync(bucketName, objectName, stream, file.ContentType);
            return Ok(new { Message = "Fayl muvaffaqiyatli yuklandi.", FileUrl = fileUrl });
        }
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile([FromQuery] string bucketName, [FromQuery] string objectName)
    {
        if (string.IsNullOrEmpty(bucketName) || string.IsNullOrEmpty(objectName))
        {
            return BadRequest("Bucket nomi va obyekt nomi talab qilinadi.");
        }

        try
        {
            // Faylni storage dan yuklab olishga urinish
            var result = await _fileStorageService.DownloadFileAsync(bucketName, objectName);

            // Agar yuklab olinmasa yoki fayl topilmasa
            if (result.StatusCode != 200 || result.Data == null)
                return NotFound(result.Message ?? "Fayl topilmadi.");

            var stream = result.Data; // Stream obyektni olish

            // Content-Type ni aniqlashga harakat qilish yoki universal qiymat berish
            var contentType = "application/octet-stream"; // Fayl turi noma'lum bo'lsa
                                                          // Agar siz fayl turini saqlagan bo'lsangiz, uni bazadan olib foydalansangiz yaxshi bo'ladi

            // Faylni brauzerga jo'natish
            return File(stream, contentType, objectName);
        }
        catch (Minio.Exceptions.MinioException e) when (e.Message.Contains("Object does not exist"))
        {
            return NotFound("Fayl topilmadi.");
        }
        catch (Exception)
        {
            // Kutilmagan xatoliklar uchun
            return StatusCode(500, "Faylni yuklab olishda kutilmagan xatolik yuz berdi.");
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFile([FromQuery] string bucketName, [FromQuery] string objectName)
    {
        if (string.IsNullOrEmpty(bucketName) || string.IsNullOrEmpty(objectName))
        {
            return BadRequest("Bucket nomi va obyekt nomi talab qilinadi.");
        }

        try
        {
            // Faylni o‘chirishga urinish
            var result = await _fileStorageService.RemoveFileAsync(bucketName, objectName);

            // Agar o‘chirish muvaffaqiyatli bo‘lsa
            if (result.Data)
            {
                return Ok(result.Message ?? "Fayl muvaffaqiyatli o'chirildi.");
            }

            // Aks holda fayl topilmadi yoki o‘chirishda muammo bo‘lgan
            return NotFound(result.Message ?? "Fayl topilmadi yoki o‘chirishda muammo yuz berdi.");
        }
        catch (Exception)
        {
            // Kutilmagan xatoliklar uchun
            return StatusCode(500, "Faylni o'chirishda kutilmagan xatolik yuz berdi.");
        }
    }
}
