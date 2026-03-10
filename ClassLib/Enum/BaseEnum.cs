using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassLib.Enum;

public enum LoginStatuses
{
    OK,
    NG,
    LOCK
}

public enum Genders
{
    Male,
    Female
}

public enum Occupation
{
    [Display(Name = "Software Developer")]
    Developer,

    [Display(Name = "UI/UX Designer")]
    Designer,

    [Display(Name = "QA Tester")]
    Tester,

    [Display(Name = "Project Manager")]
    ProjectManager
}
