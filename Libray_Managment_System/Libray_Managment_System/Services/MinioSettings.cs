namespace Library_Managment_System.Services
{
    public class MinioSettings
    {
        public string Endpoint { get; set; } = null!;
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public bool UseSsl { get; set; } // Agar HTTPS ishlatayotgan bo'lsangiz true, aks holda false
    }

}
