using System.ComponentModel.DataAnnotations;

namespace EFConcurrencyDemo
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;

        [ConcurrencyCheck]
        public string LastName { get; set; } = string.Empty;
    }
}
