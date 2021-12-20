using System.ComponentModel.DataAnnotations;

namespace ApiEroski.Models
{
    public class TicketItem
    {
        [Key]
        public string Nombre { get; set; }
        public int NumTicket { get; set; }
    }
}