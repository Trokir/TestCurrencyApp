using System.ComponentModel.DataAnnotations.Schema;


namespace TestCurrency.Models
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
    }
}
