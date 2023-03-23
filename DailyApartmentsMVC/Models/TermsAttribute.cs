using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class TermsAttribute
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AdditionalTerm> AdditionalTerms { get; } = new List<AdditionalTerm>();
}
