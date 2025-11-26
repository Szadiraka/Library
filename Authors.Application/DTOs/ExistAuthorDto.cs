
using System.ComponentModel.DataAnnotations;


namespace Authors.Application.DTOs
{
    public  class ExistAuthorDto
    {
        [Required]
        [MinLength(5)]
        public string FullName { get; set; }= string.Empty;

        [DataType(DataType.Date)]
        public DateOnly? BirthDate { get; set; }
    }
}
