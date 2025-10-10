namespace Libray_Managment_System.Enum
{
    public enum PermissionsType
    {
        // User management
        CreateUser,
        ReadUser,
        UpdateUser,
        DeleteUser,

        // Role management
        CreateRole,
        ReadRole,
        UpdateRole,
        DeleteRole,
        AssignRole,

        // Book management
        CreateBook,
        ReadBook,
        UpdateBook,
        DeleteBook,

        // Borrowing
        BorrowBook,
        ReturnBook,
        ReserveBook,

        // Fines & Payments
        ManageFines,
        MakePayment,

        // Reports
        GenerateReport
    }
}
