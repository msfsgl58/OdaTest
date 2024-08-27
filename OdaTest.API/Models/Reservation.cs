using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OdaTest.API.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberSurname { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }

        public DateTime ReservationDate { get; set; }

    }
}
