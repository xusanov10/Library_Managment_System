using Libray_Managment_System.Enum;
using Libray_Managment_System.Models;

namespace Libray_Managment_System.Data
{
    public class PermissionSeeder
    {
        private readonly LibraryManagmentSystemContext _context;

        public PermissionSeeder(LibraryManagmentSystemContext context)
        {
            _context = context;
        }
        public void SeedPermissionsAsync()
        {
            var exists = _context.Permissions.Any();
            if (exists)
                return;

            var toAdd = new List<Permission>();

            foreach (PermissionsType perm in System.Enum.GetValues(typeof(PermissionsType)))
            {
                toAdd.Add(new Permission
                {
                    Name = perm.ToString(),
                    Description = $"Permission for {perm}"
                });
            }

            _context.Permissions.AddRange(toAdd);
            _context.SaveChanges();
        }
    }
}
