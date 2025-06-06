using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace DefenseAPI.Model
{
    public class Personnel
    {
        public int PersId { get; set; }
        public int CoyId { get; set; }
        public int RankId { get; set; }
        public string TypeOfPersonnel { get; set; }
        public string PersNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string PermanentAddress { get; set; }
    }
    public class Ranks
    {
        public int RankId { get; set; }
        public string RankNAme { get; set; }
    }

    public class Comapny
    {
        public int CoyId { get; set; }
        public string CoyName { get; set; }
    }
}
