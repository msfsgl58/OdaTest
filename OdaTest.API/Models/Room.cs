using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OdaTest.API.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public bool RoomReservation { get; set; }
    }
}
