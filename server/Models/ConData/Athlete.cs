using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotableAthletes.Models.ConData
{
  [Table("Athletes", Schema = "dbo")]
  public partial class Athlete
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int64 AthleteID
    {
      get;
      set;
    }
    public string FirstName
    {
      get;
      set;
    }
    public string LastName
    {
      get;
      set;
    }
    public int CountryID
    {
      get;
      set;
    }
    public Country Country { get; set; }
  }
}
