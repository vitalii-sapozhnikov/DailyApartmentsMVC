using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class AdditionalTerm
{
    public int Id { get; set; }

    public int PropertyId { get; set; }

    public int AttributeId { get; set; }

    public bool Value { get; set; }

    public virtual TermsAttribute Attribute { get; set; } = null!;

    public virtual Property Property { get; set; } = null!;
}
