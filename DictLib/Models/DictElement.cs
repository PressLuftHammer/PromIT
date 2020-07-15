using System.ComponentModel.DataAnnotations;

namespace DictLib.Models
{
    class DictElement
    {     
        [Key]
        [MaxLength(Settings.MAX_WORD_LEN)]
        [MinLength(Settings.MIN_WORD_LEN)]
        public string Word { get; set; }
        public int Count { get; set; }
    }
}
