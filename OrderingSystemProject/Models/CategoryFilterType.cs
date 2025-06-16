using System.ComponentModel.DataAnnotations;

namespace OrderingSystemProject.Models;

public enum CategoryFilterType
{
    [Display(Name = "Starters")] STARTERS = 0,

    [Display(Name = "Mains")] MAINS = 1,

    [Display(Name = "Deserts")] DESERTS = 2,

    [Display(Name = "All")] ALL = -1
}