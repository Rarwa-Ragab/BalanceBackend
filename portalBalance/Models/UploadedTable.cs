// Ensure this class is defined in a suitable namespace
namespace portalBalance.Models
{
    public class UploadedTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Columns { get; set; }
    }
}
