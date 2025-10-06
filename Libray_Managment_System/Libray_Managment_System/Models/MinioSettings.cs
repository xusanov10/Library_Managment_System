namespace Libray_Managment_System.Models
{
    public class MinioSettings
    {
        public string Endpoint { get; set; } = null!;
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public bool UseSsl { get; set; } // Agar HTTPS ishlatayotgan bo'lsangiz true, aks holda false
    }

}
