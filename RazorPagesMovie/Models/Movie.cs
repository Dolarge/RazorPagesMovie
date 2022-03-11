using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesMovie.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;

        [Display(Name ="Release Data")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; } = string.Empty;

        // Entity Framework Core에서 Price를 데이터베이스의 통화에 올바르게 매핑할 수 있게 한다
        [Column(TypeName ="decimal (18,2)")]
        public decimal Price { get; set; }
    }
}